using System;
using System.ComponentModel.Design;
using System.Reflection;

namespace System.ComponentModel
{
	/// <summary>Provides properties and methods to add a license to a component and to manage a <see cref="T:System.ComponentModel.LicenseProvider" />. This class cannot be inherited.</summary>
	public sealed class LicenseManager
	{
		private static LicenseContext mycontext;

		private static object contextLockUser;

		private static object lockObject = new object();

		private LicenseManager()
		{
		}

		/// <summary>Gets or sets the current <see cref="T:System.ComponentModel.LicenseContext" />, which specifies when you can use the licensed object.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.LicenseContext" /> that specifies when you can use the licensed object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.ComponentModel.LicenseManager.CurrentContext" /> property is currently locked and cannot be changed.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public static LicenseContext CurrentContext
		{
			get
			{
				object obj = LicenseManager.lockObject;
				LicenseContext result;
				lock (obj)
				{
					if (LicenseManager.mycontext == null)
					{
						LicenseManager.mycontext = new RuntimeLicenseContext();
					}
					result = LicenseManager.mycontext;
				}
				return result;
			}
			set
			{
				object obj = LicenseManager.lockObject;
				lock (obj)
				{
					if (LicenseManager.contextLockUser != null)
					{
						throw new InvalidOperationException("The CurrentContext property of the LicenseManager is currently locked and cannot be changed.");
					}
					LicenseManager.mycontext = value;
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.ComponentModel.LicenseUsageMode" /> which specifies when you can use the licensed object for the <see cref="P:System.ComponentModel.LicenseManager.CurrentContext" />.</summary>
		/// <returns>One of the <see cref="T:System.ComponentModel.LicenseUsageMode" /> values, as specified in the <see cref="P:System.ComponentModel.LicenseManager.CurrentContext" /> property.</returns>
		public static LicenseUsageMode UsageMode
		{
			get
			{
				return LicenseManager.CurrentContext.UsageMode;
			}
		}

		/// <summary>Creates an instance of the specified type, given a context in which you can use the licensed instance.</summary>
		/// <returns>An instance of the specified type.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type to create. </param>
		/// <param name="creationContext">A <see cref="T:System.ComponentModel.LicenseContext" /> that specifies when you can use the licensed instance. </param>
		public static object CreateWithContext(Type type, LicenseContext creationContext)
		{
			return LicenseManager.CreateWithContext(type, creationContext, new object[0]);
		}

		/// <summary>Creates an instance of the specified type with the specified arguments, given a context in which you can use the licensed instance.</summary>
		/// <returns>An instance of the specified type with the given array of arguments.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type to create. </param>
		/// <param name="creationContext">A <see cref="T:System.ComponentModel.LicenseContext" /> that specifies when you can use the licensed instance. </param>
		/// <param name="args">An array of type <see cref="T:System.Object" /> that represents the arguments for the type. </param>
		public static object CreateWithContext(Type type, LicenseContext creationContext, object[] args)
		{
			object result = null;
			object obj = LicenseManager.lockObject;
			lock (obj)
			{
				object contextUser = new object();
				LicenseContext currentContext = LicenseManager.CurrentContext;
				LicenseManager.CurrentContext = creationContext;
				LicenseManager.LockContext(contextUser);
				try
				{
					result = Activator.CreateInstance(type, args);
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
				finally
				{
					LicenseManager.UnlockContext(contextUser);
					LicenseManager.CurrentContext = currentContext;
				}
			}
			return result;
		}

		/// <summary>Returns whether the given type has a valid license.</summary>
		/// <returns>true if the given type is licensed; otherwise, false.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> to find a valid license for. </param>
		public static bool IsLicensed(Type type)
		{
			License license = null;
			if (!LicenseManager.privateGetLicense(type, null, false, out license))
			{
				return false;
			}
			if (license != null)
			{
				license.Dispose();
			}
			return true;
		}

		/// <summary>Determines whether a valid license can be granted for the specified type.</summary>
		/// <returns>true if a valid license can be granted; otherwise, false.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of object that requests the <see cref="T:System.ComponentModel.License" />. </param>
		public static bool IsValid(Type type)
		{
			License license = null;
			if (!LicenseManager.privateGetLicense(type, null, false, out license))
			{
				return false;
			}
			if (license != null)
			{
				license.Dispose();
			}
			return true;
		}

		/// <summary>Determines whether a valid license can be granted for the specified instance of the type. This method creates a valid <see cref="T:System.ComponentModel.License" />.</summary>
		/// <returns>true if a valid <see cref="T:System.ComponentModel.License" /> can be granted; otherwise, false.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of object that requests the license. </param>
		/// <param name="instance">An object of the specified type or a type derived from the specified type. </param>
		/// <param name="license">A <see cref="T:System.ComponentModel.License" /> that is a valid license, or null if a valid license cannot be granted. </param>
		public static bool IsValid(Type type, object instance, out License license)
		{
			return LicenseManager.privateGetLicense(type, null, false, out license);
		}

		/// <summary>Prevents changes being made to the current <see cref="T:System.ComponentModel.LicenseContext" /> of the given object.</summary>
		/// <param name="contextUser">The object whose current context you want to lock. </param>
		/// <exception cref="T:System.InvalidOperationException">The context is already locked.</exception>
		public static void LockContext(object contextUser)
		{
			object obj = LicenseManager.lockObject;
			lock (obj)
			{
				LicenseManager.contextLockUser = contextUser;
			}
		}

		/// <summary>Allows changes to be made to the current <see cref="T:System.ComponentModel.LicenseContext" /> of the given object.</summary>
		/// <param name="contextUser">The object whose current context you want to unlock. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="contextUser" /> represents a different user than the one specified in a previous call to <see cref="M:System.ComponentModel.LicenseManager.LockContext(System.Object)" />. </exception>
		public static void UnlockContext(object contextUser)
		{
			object obj = LicenseManager.lockObject;
			lock (obj)
			{
				if (LicenseManager.contextLockUser != null)
				{
					if (LicenseManager.contextLockUser != contextUser)
					{
						throw new ArgumentException("The CurrentContext property of the LicenseManager can only be unlocked with the same contextUser.");
					}
					LicenseManager.contextLockUser = null;
				}
			}
		}

		/// <summary>Determines whether a license can be granted for the specified type.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of object that requests the license. </param>
		/// <exception cref="T:System.ComponentModel.LicenseException">A <see cref="T:System.ComponentModel.License" /> cannot be granted. </exception>
		public static void Validate(Type type)
		{
			License license = null;
			if (!LicenseManager.privateGetLicense(type, null, true, out license))
			{
				throw new LicenseException(type, null);
			}
			if (license != null)
			{
				license.Dispose();
			}
		}

		/// <summary>Determines whether a license can be granted for the instance of the specified type.</summary>
		/// <returns>A valid <see cref="T:System.ComponentModel.License" />.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of object that requests the license. </param>
		/// <param name="instance">An <see cref="T:System.Object" /> of the specified type or a type derived from the specified type. </param>
		/// <exception cref="T:System.ComponentModel.LicenseException">The type is licensed, but a <see cref="T:System.ComponentModel.License" /> cannot be granted. </exception>
		public static License Validate(Type type, object instance)
		{
			License result = null;
			if (!LicenseManager.privateGetLicense(type, instance, true, out result))
			{
				throw new LicenseException(type, instance);
			}
			return result;
		}

		private static bool privateGetLicense(Type type, object instance, bool allowExceptions, out License license)
		{
			bool result = false;
			License license2 = null;
			LicenseProviderAttribute licenseProviderAttribute = (LicenseProviderAttribute)Attribute.GetCustomAttribute(type, typeof(LicenseProviderAttribute), true);
			if (licenseProviderAttribute != null)
			{
				Type licenseProvider = licenseProviderAttribute.LicenseProvider;
				if (licenseProvider != null)
				{
					LicenseProvider licenseProvider2 = (LicenseProvider)Activator.CreateInstance(licenseProvider);
					if (licenseProvider2 != null)
					{
						license2 = licenseProvider2.GetLicense(LicenseManager.CurrentContext, type, instance, allowExceptions);
						if (license2 != null)
						{
							result = true;
						}
					}
				}
			}
			else
			{
				result = true;
			}
			license = license2;
			return result;
		}
	}
}
