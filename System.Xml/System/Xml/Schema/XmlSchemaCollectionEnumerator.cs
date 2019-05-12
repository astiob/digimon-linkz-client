using System;
using System.Collections;

namespace System.Xml.Schema
{
	/// <summary>Supports a simple iteration over a collection. This class cannot be inherited. </summary>
	public sealed class XmlSchemaCollectionEnumerator : IEnumerator
	{
		private IEnumerator xenum;

		internal XmlSchemaCollectionEnumerator(ICollection col)
		{
			this.xenum = col.GetEnumerator();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.Schema.XmlSchemaCollectionEnumerator.MoveNext" />.</summary>
		bool IEnumerator.MoveNext()
		{
			return this.xenum.MoveNext();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.Schema.XmlSchemaCollectionEnumerator.System.Collections.IEnumerator.Reset" />.</summary>
		void IEnumerator.Reset()
		{
			this.xenum.Reset();
		}

		/// <summary>For a description of this member, see <see cref="P:System.Xml.Schema.XmlSchemaCollectionEnumerator.Current" />.</summary>
		object IEnumerator.Current
		{
			get
			{
				return this.xenum.Current;
			}
		}

		/// <summary>Gets the current <see cref="T:System.Xml.Schema.XmlSchema" /> in the collection.</summary>
		/// <returns>The current XmlSchema in the collection.</returns>
		public XmlSchema Current
		{
			get
			{
				return (XmlSchema)this.xenum.Current;
			}
		}

		/// <summary>Advances the enumerator to the next schema in the collection.</summary>
		/// <returns>true if the move was successful; false if the enumerator has passed the end of the collection.</returns>
		public bool MoveNext()
		{
			return this.xenum.MoveNext();
		}
	}
}
