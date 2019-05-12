using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Collections
{
	/// <summary>Compares two objects for equivalence, where string comparisons are case-sensitive.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public sealed class Comparer : ISerializable, IComparer
	{
		/// <summary>Represents an instance of <see cref="T:System.Collections.Comparer" /> that is associated with the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly Comparer Default = new Comparer();

		/// <summary>Represents an instance of <see cref="T:System.Collections.Comparer" /> that is associated with <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);

		private CompareInfo m_compareInfo;

		private Comparer()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Comparer" /> class using the specified <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use for the new <see cref="T:System.Collections.Comparer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is null. </exception>
		public Comparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this.m_compareInfo = culture.CompareInfo;
		}

		/// <summary>Performs a case-sensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
		/// <returns>Value Condition Less than zero <paramref name="a" /> is less than <paramref name="b" />. Zero <paramref name="a" /> equals <paramref name="b" />. Greater than zero <paramref name="a" /> is greater than <paramref name="b" />. </returns>
		/// <param name="a">The first object to compare. </param>
		/// <param name="b">The second object to compare. </param>
		/// <exception cref="T:System.ArgumentException">Neither <paramref name="a" /> nor <paramref name="b" /> implements the <see cref="T:System.IComparable" /> interface.-or- <paramref name="a" /> and <paramref name="b" /> are of different types and neither one can handle comparisons with the other. </exception>
		/// <filterpriority>2</filterpriority>
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			if (this.m_compareInfo != null)
			{
				string text = a as string;
				string text2 = b as string;
				if (text != null && text2 != null)
				{
					return this.m_compareInfo.Compare(text, text2);
				}
			}
			if (a is IComparable)
			{
				return (a as IComparable).CompareTo(b);
			}
			if (b is IComparable)
			{
				return -(b as IComparable).CompareTo(a);
			}
			throw new ArgumentException(Locale.GetText("Neither 'a' nor 'b' implements IComparable."));
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data required for serialization.  </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to populate with data.</param>
		/// <param name="context">The context information about the source or destination of the serialization.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
		/// <filterpriority>2</filterpriority>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("CompareInfo", this.m_compareInfo, typeof(CompareInfo));
		}
	}
}
