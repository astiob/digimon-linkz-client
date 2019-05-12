using System;
using System.Collections;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Represents a collection of <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> objects used by the <see cref="T:System.Xml.Serialization.XmlSerializer" /> to override the default way it serializes a class.</summary>
	public class XmlElementAttributes : CollectionBase
	{
		/// <summary>Gets or sets an <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> from the collection.</summary>
		/// <returns>The <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> at the specified index.</returns>
		/// <param name="index">The zero-based index of the collection member to get or set. </param>
		public XmlElementAttribute this[int index]
		{
			get
			{
				return (XmlElementAttribute)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		/// <summary>Adds an <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> to the collection.</summary>
		/// <returns>The zero-based index of the newly added item.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> to add. </param>
		public int Add(XmlElementAttribute attribute)
		{
			return base.List.Add(attribute);
		}

		/// <summary>Gets a value that specifies whether the collection contains the specified object.</summary>
		/// <returns>true, if the object exists in the collection; otherwise, false.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlElementAttribute" />  in question. </param>
		public bool Contains(XmlElementAttribute attribute)
		{
			return base.List.Contains(attribute);
		}

		/// <summary>Gets the index of the specified <see cref="T:System.Xml.Serialization.XmlElementAttribute" />.</summary>
		/// <returns>The zero-based index of the <see cref="T:System.Xml.Serialization.XmlElementAttribute" />.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlElementAttribute" />  you are interested in.</param>
		public int IndexOf(XmlElementAttribute attribute)
		{
			return base.List.IndexOf(attribute);
		}

		/// <summary>Inserts an <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> into the collection.</summary>
		/// <param name="index">The zero-based index where the member is added. </param>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> to insert. </param>
		public void Insert(int index, XmlElementAttribute attribute)
		{
			base.List.Insert(index, attribute);
		}

		/// <summary>Removes the specified object from the collection.</summary>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> to remove from the collection. </param>
		public void Remove(XmlElementAttribute attribute)
		{
			base.List.Remove(attribute);
		}

		/// <summary>Copies the <see cref="T:System.Xml.Serialization.XmlElementAttributes" />, or a portion of it to a one-dimensional array.</summary>
		/// <param name="array">The <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> array to copy to. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		public void CopyTo(XmlElementAttribute[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			if (this.Count == 0)
			{
				return;
			}
			sb.Append("XEAS ");
			for (int i = 0; i < this.Count; i++)
			{
				this[i].AddKeyHash(sb);
			}
			sb.Append('|');
		}
	}
}
