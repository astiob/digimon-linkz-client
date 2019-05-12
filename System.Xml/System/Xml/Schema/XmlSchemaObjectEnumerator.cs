using System;
using System.Collections;

namespace System.Xml.Schema
{
	/// <summary>Represents the enumerator for the <see cref="T:System.Xml.Schema.XmlSchemaObjectCollection" />.</summary>
	public class XmlSchemaObjectEnumerator : IEnumerator
	{
		private IEnumerator ienum;

		internal XmlSchemaObjectEnumerator(IList list)
		{
			this.ienum = list.GetEnumerator();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.Schema.XmlSchemaObjectEnumerator.MoveNext" />.</summary>
		bool IEnumerator.MoveNext()
		{
			return this.ienum.MoveNext();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.Schema.XmlSchemaObjectEnumerator.Reset" />.</summary>
		void IEnumerator.Reset()
		{
			this.ienum.Reset();
		}

		/// <summary>For a description of this member, see <see cref="P:System.Xml.Schema.XmlSchemaObjectEnumerator.Current" />.</summary>
		object IEnumerator.Current
		{
			get
			{
				return (XmlSchemaObject)this.ienum.Current;
			}
		}

		/// <summary>Gets the current <see cref="T:System.Xml.Schema.XmlSchemaObject" /> in the collection.</summary>
		/// <returns>The current <see cref="T:System.Xml.Schema.XmlSchemaObject" />.</returns>
		public XmlSchemaObject Current
		{
			get
			{
				return (XmlSchemaObject)this.ienum.Current;
			}
		}

		/// <summary>Moves to the next item in the collection.</summary>
		/// <returns>false at the end of the collection.</returns>
		public bool MoveNext()
		{
			return this.ienum.MoveNext();
		}

		/// <summary>Resets the enumerator to the start of the collection.</summary>
		public void Reset()
		{
			this.ienum.Reset();
		}
	}
}
