using System;
using System.Collections;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Represents a collection of <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> objects.</summary>
	public class XmlAnyElementAttributes : CollectionBase
	{
		/// <summary>Gets or sets the <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> at the specified index.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> at the specified index.</returns>
		/// <param name="index">The index of the <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" />. </param>
		public XmlAnyElementAttribute this[int index]
		{
			get
			{
				return (XmlAnyElementAttribute)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		/// <summary>Adds an <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> to the collection.</summary>
		/// <returns>The index of the newly added <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" />.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> to add. </param>
		public int Add(XmlAnyElementAttribute attribute)
		{
			return base.List.Add(attribute);
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> exists in the collection.</summary>
		/// <returns>true if the <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> exists in the collection; otherwise, false.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> you are interested in. </param>
		public bool Contains(XmlAnyElementAttribute attribute)
		{
			return base.List.Contains(attribute);
		}

		/// <summary>Gets the index of the specified <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" />.</summary>
		/// <returns>The index of the specified <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" />.</returns>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> whose index you want. </param>
		public int IndexOf(XmlAnyElementAttribute attribute)
		{
			return base.List.IndexOf(attribute);
		}

		/// <summary>Inserts an <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> into the collection at the specified index.</summary>
		/// <param name="index">The index where the <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> is inserted. </param>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> to insert. </param>
		public void Insert(int index, XmlAnyElementAttribute attribute)
		{
			base.List.Insert(index, attribute);
		}

		/// <summary>Removes the specified <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> from the collection.</summary>
		/// <param name="attribute">The <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> to remove. </param>
		public void Remove(XmlAnyElementAttribute attribute)
		{
			base.List.Remove(attribute);
		}

		/// <summary>Copies the entire collection to a compatible one-dimensional array of <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> objects, starting at the specified index of the target array. </summary>
		/// <param name="array">The one-dimensional array of <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> objects that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		public void CopyTo(XmlAnyElementAttribute[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		internal void AddKeyHash(StringBuilder sb)
		{
			if (this.Count == 0)
			{
				return;
			}
			sb.Append("XAEAS ");
			for (int i = 0; i < this.Count; i++)
			{
				this[i].AddKeyHash(sb);
			}
			sb.Append('|');
		}
	}
}
