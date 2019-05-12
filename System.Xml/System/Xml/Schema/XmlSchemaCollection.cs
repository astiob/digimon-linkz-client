using System;
using System.Collections;

namespace System.Xml.Schema
{
	/// <summary>Contains a cache of XML Schema definition language (XSD) and XML-Data Reduced (XDR) schemas. This class cannot be inherited.</summary>
	[Obsolete("Use XmlSchemaSet.")]
	public sealed class XmlSchemaCollection : IEnumerable, ICollection
	{
		private XmlSchemaSet schemaSet;

		/// <summary>Initializes a new instance of the XmlSchemaCollection class.</summary>
		public XmlSchemaCollection() : this(new NameTable())
		{
		}

		/// <summary>Initializes a new instance of the XmlSchemaCollection class with the specified <see cref="T:System.Xml.XmlNameTable" />. The XmlNameTable is used when loading schemas.</summary>
		/// <param name="nametable">The XmlNameTable to use. </param>
		public XmlSchemaCollection(XmlNameTable nameTable) : this(new XmlSchemaSet(nameTable))
		{
			this.schemaSet.ValidationEventHandler += this.OnValidationError;
		}

		internal XmlSchemaCollection(XmlSchemaSet schemaSet)
		{
			this.schemaSet = schemaSet;
		}

		/// <summary>Sets an event handler for receiving information about the XDR and XML schema validation errors.</summary>
		public event ValidationEventHandler ValidationEventHandler;

		/// <summary>For a description of this member, see <see cref="P:System.Xml.Schema.XmlSchemaCollection.Count" />.</summary>
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.Schema.XmlSchemaCollection.CopyTo(System.Xml.Schema.XmlSchema[],System.Int32)" />.</summary>
		/// <param name="array">The array to copy the objects to. </param>
		/// <param name="index">The index in <paramref name="array" /> where copying will begin. </param>
		void ICollection.CopyTo(Array array, int index)
		{
			XmlSchemaSet obj = this.schemaSet;
			lock (obj)
			{
				this.schemaSet.CopyTo(array, index);
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Xml.Schema.XmlSchemaCollection.System.Collections.ICollection.IsSynchronized" />.</summary>
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.Schema.XmlSchemaCollection.GetEnumerator" />.</summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>For a description of this member, see <see cref="P:System.Xml.Schema.XmlSchemaCollection.System.Collections.ICollection.SyncRoot" />.</summary>
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		internal XmlSchemaSet SchemaSet
		{
			get
			{
				return this.schemaSet;
			}
		}

		/// <summary>Gets the number of namespaces defined in this collection.</summary>
		/// <returns>The number of namespaces defined in this collection.</returns>
		public int Count
		{
			get
			{
				return this.schemaSet.Count;
			}
		}

		/// <summary>Gets the default XmlNameTable used by the XmlSchemaCollection when loading new schemas.</summary>
		/// <returns>An XmlNameTable.</returns>
		public XmlNameTable NameTable
		{
			get
			{
				return this.schemaSet.NameTable;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchema" /> associated with the given namespace URI.</summary>
		/// <returns>The XmlSchema associated with the namespace URI; null if there is no loaded schema associated with the given namespace or if the namespace is associated with an XDR schema.</returns>
		/// <param name="ns">The namespace URI associated with the schema you want to return. This will typically be the targetNamespace of the schema. </param>
		public XmlSchema this[string ns]
		{
			get
			{
				ICollection collection = this.schemaSet.Schemas(ns);
				if (collection == null)
				{
					return null;
				}
				IEnumerator enumerator = collection.GetEnumerator();
				if (enumerator.MoveNext())
				{
					return (XmlSchema)enumerator.Current;
				}
				return null;
			}
		}

		/// <summary>Adds the schema contained in the <see cref="T:System.Xml.XmlReader" /> to the schema collection.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchema" /> added to the schema collection; null if the schema being added is an XDR schema or if there are compilation errors in the schema.</returns>
		/// <param name="ns">The namespace URI associated with the schema. For XML Schemas, this will typically be the targetNamespace. </param>
		/// <param name="reader">
		///   <see cref="T:System.Xml.XmlReader" /> containing the schema to add. </param>
		/// <exception cref="T:System.Xml.XmlException">The schema is not a valid schema. </exception>
		public XmlSchema Add(string ns, XmlReader reader)
		{
			return this.Add(ns, reader, new XmlUrlResolver());
		}

		/// <summary>Adds the schema contained in the <see cref="T:System.Xml.XmlReader" /> to the schema collection. The specified <see cref="T:System.Xml.XmlResolver" /> is used to resolve any external resources.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchema" /> added to the schema collection; null if the schema being added is an XDR schema or if there are compilation errors in the schema.</returns>
		/// <param name="ns">The namespace URI associated with the schema. For XML Schemas, this will typically be the targetNamespace. </param>
		/// <param name="reader">
		///   <see cref="T:System.Xml.XmlReader" /> containing the schema to add. </param>
		/// <param name="resolver">The <see cref="T:System.Xml.XmlResolver" /> used to resolve namespaces referenced in include and import elements or x-schema attribute (XDR schemas). If this is null, external references are not resolved. </param>
		/// <exception cref="T:System.Xml.XmlException">The schema is not a valid schema. </exception>
		public XmlSchema Add(string ns, XmlReader reader, XmlResolver resolver)
		{
			XmlSchema xmlSchema = XmlSchema.Read(reader, this.ValidationEventHandler);
			if (xmlSchema.TargetNamespace == null)
			{
				xmlSchema.TargetNamespace = ns;
			}
			else if (ns != null && xmlSchema.TargetNamespace != ns)
			{
				throw new XmlSchemaException("The actual targetNamespace in the schema does not match the parameter.");
			}
			return this.Add(xmlSchema);
		}

		/// <summary>Adds the schema located by the given URL into the schema collection.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchema" /> added to the schema collection; null if the schema being added is an XDR schema or if there are compilation errors in the schema. </returns>
		/// <param name="ns">The namespace URI associated with the schema. For XML Schemas, this will typically be the targetNamespace. </param>
		/// <param name="uri">The URL that specifies the schema to load. </param>
		/// <exception cref="T:System.Xml.XmlException">The schema is not a valid schema. </exception>
		public XmlSchema Add(string ns, string uri)
		{
			XmlReader xmlReader = new XmlTextReader(uri);
			XmlSchema result;
			try
			{
				result = this.Add(ns, xmlReader);
			}
			finally
			{
				xmlReader.Close();
			}
			return result;
		}

		/// <summary>Adds the <see cref="T:System.Xml.Schema.XmlSchema" /> to the collection.</summary>
		/// <returns>The XmlSchema object.</returns>
		/// <param name="schema">The XmlSchema to add to the collection. </param>
		public XmlSchema Add(XmlSchema schema)
		{
			return this.Add(schema, new XmlUrlResolver());
		}

		/// <summary>Adds the <see cref="T:System.Xml.Schema.XmlSchema" /> to the collection. The specified <see cref="T:System.Xml.XmlResolver" /> is used to resolve any external references.</summary>
		/// <returns>The XmlSchema added to the schema collection.</returns>
		/// <param name="schema">The XmlSchema to add to the collection. </param>
		/// <param name="resolver">The <see cref="T:System.Xml.XmlResolver" /> used to resolve namespaces referenced in include and import elements. If this is null, external references are not resolved. </param>
		/// <exception cref="T:System.Xml.XmlException">The schema is not a valid schema. </exception>
		public XmlSchema Add(XmlSchema schema, XmlResolver resolver)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet(this.schemaSet.NameTable);
			xmlSchemaSet.Add(this.schemaSet);
			xmlSchemaSet.Add(schema);
			xmlSchemaSet.ValidationEventHandler += this.ValidationEventHandler;
			xmlSchemaSet.XmlResolver = resolver;
			xmlSchemaSet.Compile();
			if (!xmlSchemaSet.IsCompiled)
			{
				return null;
			}
			this.schemaSet = xmlSchemaSet;
			return schema;
		}

		/// <summary>Adds all the namespaces defined in the given collection (including their associated schemas) to this collection.</summary>
		/// <param name="schema">The XmlSchemaCollection you want to add to this collection. </param>
		public void Add(XmlSchemaCollection schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet(this.schemaSet.NameTable);
			xmlSchemaSet.Add(this.schemaSet);
			xmlSchemaSet.Add(schema.schemaSet);
			xmlSchemaSet.ValidationEventHandler += this.ValidationEventHandler;
			xmlSchemaSet.XmlResolver = this.schemaSet.XmlResolver;
			xmlSchemaSet.Compile();
			if (!xmlSchemaSet.IsCompiled)
			{
				return;
			}
			this.schemaSet = xmlSchemaSet;
		}

		/// <summary>Gets a value indicating whether a schema with the specified namespace is in the collection.</summary>
		/// <returns>true if a schema with the specified namespace is in the collection; otherwise, false.</returns>
		/// <param name="ns">The namespace URI associated with the schema. For XML Schemas, this will typically be the target namespace. </param>
		public bool Contains(string ns)
		{
			XmlSchemaSet obj = this.schemaSet;
			bool result;
			lock (obj)
			{
				result = this.schemaSet.Contains(ns);
			}
			return result;
		}

		/// <summary>Gets a value indicating whether the targetNamespace of the specified <see cref="T:System.Xml.Schema.XmlSchema" /> is in the collection.</summary>
		/// <returns>true if there is a schema in the collection with the same targetNamespace; otherwise, false.</returns>
		/// <param name="schema">The XmlSchema object. </param>
		public bool Contains(XmlSchema schema)
		{
			XmlSchemaSet obj = this.schemaSet;
			bool result;
			lock (obj)
			{
				result = this.schemaSet.Contains(schema);
			}
			return result;
		}

		/// <summary>Copies all the XmlSchema objects from this collection into the given array starting at the given index.</summary>
		/// <param name="array">The array to copy the objects to. </param>
		/// <param name="index">The index in <paramref name="array" /> where copying will begin. </param>
		public void CopyTo(XmlSchema[] array, int index)
		{
			XmlSchemaSet obj = this.schemaSet;
			lock (obj)
			{
				this.schemaSet.CopyTo(array, index);
			}
		}

		/// <summary>Provides support for the "for each" style iteration over the collection of schemas.</summary>
		/// <returns>An enumerator for iterating over all schemas in the current collection.</returns>
		public XmlSchemaCollectionEnumerator GetEnumerator()
		{
			return new XmlSchemaCollectionEnumerator(this.schemaSet.Schemas());
		}

		private void OnValidationError(object o, ValidationEventArgs e)
		{
			if (this.ValidationEventHandler != null)
			{
				this.ValidationEventHandler(o, e);
			}
			else if (e.Severity == XmlSeverityType.Error)
			{
				throw e.Exception;
			}
		}
	}
}
