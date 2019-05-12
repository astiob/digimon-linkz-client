using System;
using System.IO;

namespace System.ComponentModel
{
	/// <summary>Provides an implementation of a <see cref="T:System.ComponentModel.LicenseProvider" />. The provider works in a similar fashion to the Microsoft .NET Framework standard licensing model.</summary>
	public class LicFileLicenseProvider : LicenseProvider
	{
		/// <summary>Returns a license for the instance of the component, if one is available.</summary>
		/// <returns>A valid <see cref="T:System.ComponentModel.License" />. If this method cannot find a valid <see cref="T:System.ComponentModel.License" /> or a valid <paramref name="context" /> parameter, it returns null.</returns>
		/// <param name="context">A <see cref="T:System.ComponentModel.LicenseContext" /> that specifies where you can use the licensed object. </param>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the component requesting the <see cref="T:System.ComponentModel.License" />. </param>
		/// <param name="instance">An object that requests the <see cref="T:System.ComponentModel.License" />. </param>
		/// <param name="allowExceptions">true if a <see cref="T:System.ComponentModel.LicenseException" /> should be thrown when a component cannot be granted a license; otherwise, false. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
			try
			{
				if (context == null || context.UsageMode != LicenseUsageMode.Designtime)
				{
					return null;
				}
				string text = Path.GetDirectoryName(type.Assembly.Location);
				text = Path.Combine(text, type.FullName + ".LIC");
				if (!File.Exists(text))
				{
					return null;
				}
				StreamReader streamReader = new StreamReader(text);
				string key = streamReader.ReadLine();
				streamReader.Close();
				if (this.IsKeyValid(key, type))
				{
					return new LicFileLicense(key);
				}
			}
			catch
			{
				if (allowExceptions)
				{
					throw;
				}
			}
			return null;
		}

		/// <summary>Returns a key for the specified type.</summary>
		/// <returns>A confirmation that the <paramref name="type" /> parameter is licensed.</returns>
		/// <param name="type">The object type to return the key. </param>
		protected virtual string GetKey(Type type)
		{
			return type.FullName + " is a licensed component.";
		}

		/// <summary>Determines whether the key that the <see cref="M:System.ComponentModel.LicFileLicenseProvider.GetLicense(System.ComponentModel.LicenseContext,System.Type,System.Object,System.Boolean)" /> method retrieves is valid for the specified type.</summary>
		/// <returns>true if the key is a valid <see cref="P:System.ComponentModel.License.LicenseKey" /> for the specified type; otherwise, false.</returns>
		/// <param name="key">The <see cref="P:System.ComponentModel.License.LicenseKey" /> to check. </param>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the component requesting the <see cref="T:System.ComponentModel.License" />. </param>
		protected virtual bool IsKeyValid(string key, Type type)
		{
			return key != null && key.Equals(this.GetKey(type));
		}
	}
}
