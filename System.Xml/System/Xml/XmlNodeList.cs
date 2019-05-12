using System;
using System.Collections;

namespace System.Xml
{
	/// <summary>Represents an ordered collection of nodes.</summary>
	public abstract class XmlNodeList : IEnumerable
	{
		/// <summary>Gets the number of nodes in the XmlNodeList.</summary>
		/// <returns>The number of nodes.</returns>
		public abstract int Count { get; }

		/// <summary>Retrieves a node at the given index.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> in the collection. If index is greater than or equal to the number of nodes in the list, this returns null.</returns>
		/// <param name="i">Zero-based index into the list of nodes. </param>
		public virtual XmlNode this[int i]
		{
			get
			{
				return this.Item(i);
			}
		}

		/// <summary>Provides a simple "foreach" style iteration over the collection of nodes in the XmlNodeList.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" />.</returns>
		public abstract IEnumerator GetEnumerator();

		/// <summary>Retrieves a node at the given index.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> in the collection. If <paramref name="index" /> is greater than or equal to the number of nodes in the list, this returns null.</returns>
		/// <param name="index">Zero-based index into the list of nodes. </param>
		public abstract XmlNode Item(int index);
	}
}
