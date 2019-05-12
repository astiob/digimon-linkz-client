using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Collections
{
	/// <summary>Supplies a hash code for an object, using a hashing algorithm that ignores the case of strings.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Obsolete("Please use StringComparer instead.")]
	[Serializable]
	public class CaseInsensitiveHashCodeProvider : IHashCodeProvider
	{
		private static readonly CaseInsensitiveHashCodeProvider singletonInvariant = new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture);

		private static CaseInsensitiveHashCodeProvider singleton;

		private static readonly object sync = new object();

		private TextInfo m_text;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.CaseInsensitiveHashCodeProvider" /> class using the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread.</summary>
		public CaseInsensitiveHashCodeProvider()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			if (!CaseInsensitiveHashCodeProvider.AreEqual(currentCulture, CultureInfo.InvariantCulture))
			{
				this.m_text = CultureInfo.CurrentCulture.TextInfo;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.CaseInsensitiveHashCodeProvider" /> class using the specified <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use for the new <see cref="T:System.Collections.CaseInsensitiveHashCodeProvider" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is null. </exception>
		public CaseInsensitiveHashCodeProvider(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			if (!CaseInsensitiveHashCodeProvider.AreEqual(culture, CultureInfo.InvariantCulture))
			{
				this.m_text = culture.TextInfo;
			}
		}

		/// <summary>Gets an instance of <see cref="T:System.Collections.CaseInsensitiveHashCodeProvider" /> that is associated with the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread and that is always available.</summary>
		/// <returns>An instance of <see cref="T:System.Collections.CaseInsensitiveHashCodeProvider" /> that is associated with the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CaseInsensitiveHashCodeProvider Default
		{
			get
			{
				object obj = CaseInsensitiveHashCodeProvider.sync;
				CaseInsensitiveHashCodeProvider result;
				lock (obj)
				{
					if (CaseInsensitiveHashCodeProvider.singleton == null)
					{
						CaseInsensitiveHashCodeProvider.singleton = new CaseInsensitiveHashCodeProvider();
					}
					else if (CaseInsensitiveHashCodeProvider.singleton.m_text == null)
					{
						if (!CaseInsensitiveHashCodeProvider.AreEqual(CultureInfo.CurrentCulture, CultureInfo.InvariantCulture))
						{
							CaseInsensitiveHashCodeProvider.singleton = new CaseInsensitiveHashCodeProvider();
						}
					}
					else if (!CaseInsensitiveHashCodeProvider.AreEqual(CaseInsensitiveHashCodeProvider.singleton.m_text, CultureInfo.CurrentCulture))
					{
						CaseInsensitiveHashCodeProvider.singleton = new CaseInsensitiveHashCodeProvider();
					}
					result = CaseInsensitiveHashCodeProvider.singleton;
				}
				return result;
			}
		}

		private static bool AreEqual(CultureInfo a, CultureInfo b)
		{
			return a.Name == b.Name;
		}

		private static bool AreEqual(TextInfo info, CultureInfo culture)
		{
			return info.CultureName == culture.Name;
		}

		/// <summary>Gets an instance of <see cref="T:System.Collections.CaseInsensitiveHashCodeProvider" /> that is associated with <see cref="P:System.Globalization.CultureInfo.InvariantCulture" /> and that is always available.</summary>
		/// <returns>An instance of <see cref="T:System.Collections.CaseInsensitiveHashCodeProvider" /> that is associated with <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CaseInsensitiveHashCodeProvider DefaultInvariant
		{
			get
			{
				return CaseInsensitiveHashCodeProvider.singletonInvariant;
			}
		}

		/// <summary>Returns a hash code for the given object, using a hashing algorithm that ignores the case of strings.</summary>
		/// <returns>A hash code for the given object, using a hashing algorithm that ignores the case of strings.</returns>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="obj" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public int GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			string text = obj as string;
			if (text == null)
			{
				return obj.GetHashCode();
			}
			int num = 0;
			if (this.m_text != null && !CaseInsensitiveHashCodeProvider.AreEqual(this.m_text, CultureInfo.InvariantCulture))
			{
				foreach (char c in this.m_text.ToLower(text))
				{
					num = num * 31 + (int)c;
				}
			}
			else
			{
				for (int j = 0; j < text.Length; j++)
				{
					char c = char.ToLower(text[j], CultureInfo.InvariantCulture);
					num = num * 31 + (int)c;
				}
			}
			return num;
		}
	}
}
