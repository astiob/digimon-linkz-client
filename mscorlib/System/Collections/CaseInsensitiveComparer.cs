using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Collections
{
	/// <summary>Compares two objects for equivalence, ignoring the case of strings.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class CaseInsensitiveComparer : IComparer
	{
		private static CaseInsensitiveComparer defaultComparer = new CaseInsensitiveComparer();

		private static CaseInsensitiveComparer defaultInvariantComparer = new CaseInsensitiveComparer(true);

		private CultureInfo culture;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.CaseInsensitiveComparer" /> class using the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread.</summary>
		public CaseInsensitiveComparer()
		{
			this.culture = CultureInfo.CurrentCulture;
		}

		private CaseInsensitiveComparer(bool invariant)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.CaseInsensitiveComparer" /> class using the specified <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use for the new <see cref="T:System.Collections.CaseInsensitiveComparer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is null. </exception>
		public CaseInsensitiveComparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			if (culture.LCID != CultureInfo.InvariantCulture.LCID)
			{
				this.culture = culture;
			}
		}

		/// <summary>Gets an instance of <see cref="T:System.Collections.CaseInsensitiveComparer" /> that is associated with the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread and that is always available.</summary>
		/// <returns>An instance of <see cref="T:System.Collections.CaseInsensitiveComparer" /> that is associated with the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CaseInsensitiveComparer Default
		{
			get
			{
				return CaseInsensitiveComparer.defaultComparer;
			}
		}

		/// <summary>Gets an instance of <see cref="T:System.Collections.CaseInsensitiveComparer" /> that is associated with <see cref="P:System.Globalization.CultureInfo.InvariantCulture" /> and that is always available.</summary>
		/// <returns>An instance of <see cref="T:System.Collections.CaseInsensitiveComparer" /> that is associated with <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CaseInsensitiveComparer DefaultInvariant
		{
			get
			{
				return CaseInsensitiveComparer.defaultInvariantComparer;
			}
		}

		/// <summary>Performs a case-insensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
		/// <returns>Value Condition Less than zero <paramref name="a" /> is less than <paramref name="b" />, with casing ignored. Zero <paramref name="a" /> equals <paramref name="b" />, with casing ignored. Greater than zero <paramref name="a" /> is greater than <paramref name="b" />, with casing ignored. </returns>
		/// <param name="a">The first object to compare. </param>
		/// <param name="b">The second object to compare. </param>
		/// <exception cref="T:System.ArgumentException">Neither <paramref name="a" /> nor <paramref name="b" /> implements the <see cref="T:System.IComparable" /> interface.-or- <paramref name="a" /> and <paramref name="b" /> are of different types. </exception>
		/// <filterpriority>2</filterpriority>
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text == null || text2 == null)
			{
				return Comparer.Default.Compare(a, b);
			}
			if (this.culture != null)
			{
				return this.culture.CompareInfo.Compare(text, text2, CompareOptions.IgnoreCase);
			}
			return CultureInfo.InvariantCulture.CompareInfo.Compare(text, text2, CompareOptions.IgnoreCase);
		}
	}
}
