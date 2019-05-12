using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Xml.Schema
{
	/// <summary>Provides the collections for contained elements in the <see cref="T:System.Xml.Schema.XmlSchema" /> class (for example, Attributes, AttributeGroups, Elements, and so on).</summary>
	public class XmlSchemaObjectTable
	{
		private HybridDictionary table;

		internal XmlSchemaObjectTable()
		{
			this.table = new HybridDictionary();
		}

		/// <summary>Gets the number of items contained in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</summary>
		/// <returns>The number of items contained in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</returns>
		public int Count
		{
			get
			{
				return this.table.Count;
			}
		}

		/// <summary>Returns the element in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> specified by qualified name.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> of the element in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> specified by qualified name.</returns>
		/// <param name="name">The <see cref="T:System.Xml.XmlQualifiedName" /> of the element to return.</param>
		public XmlSchemaObject this[XmlQualifiedName name]
		{
			get
			{
				return (XmlSchemaObject)this.table[name];
			}
		}

		/// <summary>Returns a collection of all the named elements in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</summary>
		/// <returns>A collection of all the named elements in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</returns>
		public ICollection Names
		{
			get
			{
				return this.table.Keys;
			}
		}

		/// <summary>Returns a collection of all the values for all the elements in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</summary>
		/// <returns>A collection of all the values for all the elements in the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</returns>
		public ICollection Values
		{
			get
			{
				return this.table.Values;
			}
		}

		/// <summary>Determines if the qualified name specified exists in the collection.</summary>
		/// <returns>true if the qualified name specified exists in the collection; otherwise, false.</returns>
		/// <param name="name">The <see cref="T:System.Xml.XmlQualifiedName" />.</param>
		public bool Contains(XmlQualifiedName name)
		{
			return this.table.Contains(name);
		}

		/// <summary>Returns an enumerator that can iterate through the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> that can iterate through <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</returns>
		public IDictionaryEnumerator GetEnumerator()
		{
			return new XmlSchemaObjectTable.XmlSchemaObjectTableEnumerator(this);
		}

		internal void Add(XmlQualifiedName name, XmlSchemaObject value)
		{
			this.table[name] = value;
		}

		internal void Clear()
		{
			this.table.Clear();
		}

		internal void Set(XmlQualifiedName name, XmlSchemaObject value)
		{
			this.table[name] = value;
		}

		internal class XmlSchemaObjectTableEnumerator : IEnumerator, IDictionaryEnumerator
		{
			private IDictionaryEnumerator xenum;

			private IEnumerable tmp;

			internal XmlSchemaObjectTableEnumerator(XmlSchemaObjectTable table)
			{
				this.tmp = table.table;
				this.xenum = (IDictionaryEnumerator)this.tmp.GetEnumerator();
			}

			bool IEnumerator.MoveNext()
			{
				return this.xenum.MoveNext();
			}

			void IEnumerator.Reset()
			{
				this.xenum.Reset();
			}

			object IEnumerator.Current
			{
				get
				{
					return this.xenum.Entry;
				}
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					return this.xenum.Entry;
				}
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					return (XmlQualifiedName)this.xenum.Key;
				}
			}

			object IDictionaryEnumerator.Value
			{
				get
				{
					return (XmlSchemaObject)this.xenum.Value;
				}
			}

			public XmlSchemaObject Current
			{
				get
				{
					return (XmlSchemaObject)this.xenum.Value;
				}
			}

			public DictionaryEntry Entry
			{
				get
				{
					return this.xenum.Entry;
				}
			}

			public XmlQualifiedName Key
			{
				get
				{
					return (XmlQualifiedName)this.xenum.Key;
				}
			}

			public XmlSchemaObject Value
			{
				get
				{
					return (XmlSchemaObject)this.xenum.Value;
				}
			}

			public bool MoveNext()
			{
				return this.xenum.MoveNext();
			}
		}
	}
}
