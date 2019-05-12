using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Enables iteration over a collection of <see cref="T:System.Xml.Schema.XmlSchema" /> objects. </summary>
	[MonoTODO]
	public class XmlSchemaEnumerator : IEnumerator<XmlSchema>, IDisposable, IEnumerator
	{
		private IEnumerator e;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSchemaEnumerator" /> class. </summary>
		/// <param name="list">The <see cref="T:System.Xml.Serialization.XmlSchemas" /> object you want to iterate over.</param>
		public XmlSchemaEnumerator(XmlSchemas list)
		{
			this.e = list.GetEnumerator();
		}

		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		void IEnumerator.Reset()
		{
			this.e.Reset();
		}

		/// <summary>Gets the current element in the collection.</summary>
		/// <returns>The current <see cref="T:System.Xml.Schema.XmlSchema" /> object in the collection.</returns>
		public XmlSchema Current
		{
			get
			{
				return (XmlSchema)this.e.Current;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Xml.Serialization.XmlSchemaEnumerator" />.</summary>
		public void Dispose()
		{
		}

		/// <summary>Advances the enumerator to the next item in the collection.</summary>
		/// <returns>true if the move is successful; otherwise, false.</returns>
		public bool MoveNext()
		{
			return this.e.MoveNext();
		}
	}
}
