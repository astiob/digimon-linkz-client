using System;
using System.Collections;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Represents a collection of <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> objects.</summary>
	public class XmlArrayItemAttributes : CollectionBase
	{
		/// <summary>Gets or sets the item at the specified index.</summary>
		/// <returns>The <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> at the specified index.</returns>
		/// <param name="index">The zero-based index of the collection member to get or set. </param>
		public XmlArrayItemAttribute this[int index]
		{
			get
			{
				return (XmlArrayItemAttribute)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		/// <summary>Adds an <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> to the collection.</summary>
		/// <returns>The index of the added item.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> to add to the collection. </param>
		public int Add(XmlArrayItemAttribute attribute)
		{
			return base.List.Add(attribute);
		}

		/// <summary>Determines whether the collection contains the specified <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" />. </summary>
		/// <returns>true if the collection contains the specified <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" />; otherwise, false.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> to check for.</param>
		public bool Contains(XmlArrayItemAttribute attribute)
		{
			return base.List.Contains(attribute);
		}

		/// <summary>Copies an <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> array to the collection, starting at a specified target index. </summary>
		/// <param name="array">The array of <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> objects to copy to the collection.</param>
		/// <param name="index">The index at which the copied attributes begin.</param>
		public void CopyTo(XmlArrayItemAttribute[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		/// <summary>Returns the zero-based index of the first occurrence of the specified <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> in the collection or 1 if the attribute is not found in the collection. </summary>
		/// <returns>The first index of the <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> in the collection or -1 if the attribute is not found in the collection.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> to locate in the collection.</param>
		public int IndexOf(XmlArrayItemAttribute attribute)
		{
			return base.List.IndexOf(attribute);
		}

		/// <summary>Inserts an <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> into the collection at the specified index. </summary>
		/// <param name="index">The index at which the attribute is inserted.</param>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" />  to insert.</param>
		public void Insert(int index, XmlArrayItemAttribute attribute)
		{
			base.List.Insert(index, attribute);
		}

		/// <summary>Removes an <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> from the collection, if it is present. </summary>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlArrayItemAttribute" /> to remove.</param>
		public void Remove(XmlArrayItemAttribute attribute)
		{
			base.List.Remove(attribute);
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			if (this.Count == 0)
			{
				return;
			}
			sb.Append("XAIAS ");
			for (int i = 0; i < this.Count; i++)
			{
				this[i].AddKeyHash(sb);
			}
			sb.Append('|');
		}
	}
}
