using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Represents the collection of XML schemas.</summary>
	public class XmlSchemas : CollectionBase, IEnumerable<XmlSchema>, IEnumerable
	{
		private static string msdataNS = "urn:schemas-microsoft-com:xml-msdata";

		private Hashtable table = new Hashtable();

		IEnumerator<XmlSchema> IEnumerable<XmlSchema>.GetEnumerator()
		{
			return new XmlSchemaEnumerator(this);
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchema" /> object at the specified index. </summary>
		/// <returns>The specified <see cref="T:System.Xml.Schema.XmlSchema" />.</returns>
		/// <param name="index">The index of the item to retrieve.</param>
		public XmlSchema this[int index]
		{
			get
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				return (XmlSchema)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		/// <summary>Gets a specified <see cref="T:System.Xml.Schema.XmlSchema" /> object that represents the XML schema associated with the specified namespace.</summary>
		/// <returns>The specified <see cref="T:System.Xml.Schema.XmlSchema" /> object.</returns>
		/// <param name="ns">The namespace of the specified object.</param>
		public XmlSchema this[string ns]
		{
			get
			{
				return (XmlSchema)this.table[(ns == null) ? string.Empty : ns];
			}
		}

		/// <summary>Gets a value that indicates whether the schemas have been compiled.</summary>
		/// <returns>true, if the schemas have been compiled; otherwise, false.</returns>
		[MonoTODO]
		public bool IsCompiled
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Processes the element and attribute names in the XML schemas and, optionally, validates the XML schemas. </summary>
		/// <param name="handler">A <see cref="T:System.Xml.Schema.ValidationEventHandler" /> that specifies the callback method that handles errors and warnings during XML Schema validation, if the strict parameter is set to true.</param>
		/// <param name="fullCompile">true to validate the XML schemas in the collection using the <see cref="M:System.Xml.Serialization.XmlSchemas.Compile(System.Xml.Schema.ValidationEventHandler,System.Boolean)" /> method of the <see cref="T:System.Xml.Serialization.XmlSchemas" /> class; otherwise, false.</param>
		[MonoTODO]
		public void Compile(ValidationEventHandler handler, bool fullCompile)
		{
			foreach (object obj in this)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (fullCompile || !xmlSchema.IsCompiled)
				{
					xmlSchema.Compile(handler);
				}
			}
		}

		/// <summary>Adds an object to the end of the collection.</summary>
		/// <returns>The index at which the <see cref="T:System.Xml.Schema.XmlSchema" /> is added.</returns>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> object to be added to the collection of objects. </param>
		public int Add(XmlSchema schema)
		{
			this.Insert(this.Count, schema);
			return this.Count - 1;
		}

		/// <summary>Adds an instance of the <see cref="T:System.Xml.Serialization.XmlSchemas" /> class to the end of the collection.</summary>
		/// <param name="schemas">The <see cref="T:System.Xml.Serialization.XmlSchemas" /> object to be added to the end of the collection. </param>
		public void Add(XmlSchemas schemas)
		{
			foreach (object obj in schemas)
			{
				XmlSchema schema = (XmlSchema)obj;
				this.Add(schema);
			}
		}

		/// <summary>Adds an <see cref="T:System.Xml.Schema.XmlSchema" /> object that represents an assembly reference to the collection.</summary>
		/// <returns>The index at which the <see cref="T:System.Xml.Schema.XmlSchema" /> is added.</returns>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> to add.</param>
		/// <param name="baseUri">The <see cref="T:System.Uri" /> of the schema object.</param>
		[MonoNotSupported("")]
		public int Add(XmlSchema schema, Uri baseUri)
		{
			throw new NotImplementedException();
		}

		/// <summary>Adds an <see cref="T:System.Xml.Schema.XmlSchema" /> object that represents an assembly reference to the collection.</summary>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> to add.</param>
		[MonoNotSupported("")]
		public void AddReference(XmlSchema schema)
		{
			throw new NotImplementedException();
		}

		/// <summary>Determines whether the <see cref="T:System.Xml.Serialization.XmlSchemas" /> contains a specific schema.</summary>
		/// <returns>true, if the collection contains the specified item; otherwise, false.</returns>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> object to locate. </param>
		public bool Contains(XmlSchema schema)
		{
			return base.List.Contains(schema);
		}

		/// <summary>Returns a value that indicates whether the collection contains an <see cref="T:System.Xml.Schema.XmlSchema" /> object that belongs to the specified namespace.</summary>
		/// <returns>true if the item is found; otherwise, false.</returns>
		/// <param name="targetNamespace">The namespace of the item to check for.</param>
		[MonoNotSupported("")]
		public bool Contains(string targetNamespace)
		{
			throw new NotImplementedException();
		}

		/// <summary>Copies the entire <see cref="T:System.Xml.Serialization.XmlSchemas" /> to a compatible one-dimensional <see cref="T:System.Array" />, which starts at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the schemas copied from <see cref="T:System.Xml.Serialization.XmlSchemas" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">A 32-bit integer that represents the index in the array where copying begins.</param>
		public void CopyTo(XmlSchema[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		/// <summary>Locates in one of the XML schemas an <see cref="T:System.Xml.Schema.XmlSchemaObject" /> of the specified name and type. </summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObject" /> instance, such as an <see cref="T:System.Xml.Schema.XmlSchemaElement" /> or <see cref="T:System.Xml.Schema.XmlSchemaAttribute" />.</returns>
		/// <param name="name">An <see cref="T:System.Xml.XmlQualifiedName" /> that specifies a fully qualified name with a namespace used to locate an <see cref="T:System.Xml.Schema.XmlSchema" /> object in the collection.</param>
		/// <param name="type">The <see cref="T:System.Type" /> of the object to find. Possible types include: <see cref="T:System.Xml.Schema.XmlSchemaGroup" />, <see cref="T:System.Xml.Schema.XmlSchemaAttributeGroup" />, <see cref="T:System.Xml.Schema.XmlSchemaElement" />, <see cref="T:System.Xml.Schema.XmlSchemaAttribute" />, and <see cref="T:System.Xml.Schema.XmlSchemaNotation" />.</param>
		public object Find(XmlQualifiedName name, Type type)
		{
			XmlSchema xmlSchema = this.table[name.Namespace] as XmlSchema;
			if (xmlSchema == null)
			{
				foreach (object obj in this)
				{
					XmlSchema schema = (XmlSchema)obj;
					object obj2 = this.Find(schema, name, type);
					if (obj2 != null)
					{
						return obj2;
					}
				}
				return null;
			}
			object obj3 = this.Find(xmlSchema, name, type);
			if (obj3 == null)
			{
				foreach (object obj4 in this)
				{
					XmlSchema schema2 = (XmlSchema)obj4;
					object obj5 = this.Find(schema2, name, type);
					if (obj5 != null)
					{
						return obj5;
					}
				}
			}
			return obj3;
		}

		private object Find(XmlSchema schema, XmlQualifiedName name, Type type)
		{
			if (!schema.IsCompiled)
			{
				schema.Compile(null);
			}
			XmlSchemaObjectTable xmlSchemaObjectTable = null;
			if (type == typeof(XmlSchemaSimpleType) || type == typeof(XmlSchemaComplexType))
			{
				xmlSchemaObjectTable = schema.SchemaTypes;
			}
			else if (type == typeof(XmlSchemaAttribute))
			{
				xmlSchemaObjectTable = schema.Attributes;
			}
			else if (type == typeof(XmlSchemaAttributeGroup))
			{
				xmlSchemaObjectTable = schema.AttributeGroups;
			}
			else if (type == typeof(XmlSchemaElement))
			{
				xmlSchemaObjectTable = schema.Elements;
			}
			else if (type == typeof(XmlSchemaGroup))
			{
				xmlSchemaObjectTable = schema.Groups;
			}
			else if (type == typeof(XmlSchemaNotation))
			{
				xmlSchemaObjectTable = schema.Notations;
			}
			object obj = (xmlSchemaObjectTable == null) ? null : xmlSchemaObjectTable[name];
			if (obj != null && obj.GetType() != type)
			{
				return null;
			}
			return obj;
		}

		/// <summary>Gets a collection of schemas that belong to the same namespace.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> implementation that contains the schemas.</returns>
		/// <param name="ns">The namespace of the schemas to retrieve.</param>
		[MonoNotSupported("")]
		public IList GetSchemas(string ns)
		{
			throw new NotImplementedException();
		}

		/// <summary>Searches for the specified schema and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Xml.Serialization.XmlSchemas" />.</summary>
		/// <returns>The zero-based index of the first occurrence of the value within the entire <see cref="T:System.Xml.Serialization.XmlSchemas" />, if found; otherwise, -1.</returns>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> to locate. </param>
		public int IndexOf(XmlSchema schema)
		{
			return base.List.IndexOf(schema);
		}

		/// <summary>Inserts a schema into the <see cref="T:System.Xml.Serialization.XmlSchemas" /> at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="schema" /> should be inserted. </param>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> object to be inserted. </param>
		public void Insert(int index, XmlSchema schema)
		{
			base.List.Insert(index, schema);
		}

		/// <summary>Static method that determines whether the specified XML schema contains a custom IsDataSet attribute set to true, or its equivalent. </summary>
		/// <returns>true if the specified schema exists; otherwise, false.</returns>
		/// <param name="schema">The XML schema to check for an IsDataSet attribute with a true value.</param>
		public static bool IsDataSet(XmlSchema schema)
		{
			XmlSchemaElement xmlSchemaElement = (schema.Items.Count != 1) ? null : (schema.Items[0] as XmlSchemaElement);
			if (xmlSchemaElement != null && xmlSchemaElement.UnhandledAttributes != null && xmlSchemaElement.UnhandledAttributes.Length > 0)
			{
				for (int i = 0; i < xmlSchemaElement.UnhandledAttributes.Length; i++)
				{
					XmlAttribute xmlAttribute = xmlSchemaElement.UnhandledAttributes[i];
					if (xmlAttribute.NamespaceURI == XmlSchemas.msdataNS && xmlAttribute.LocalName == "IsDataSet")
					{
						return xmlAttribute.Value.ToLower(CultureInfo.InvariantCulture) == "true";
					}
				}
			}
			return false;
		}

		/// <summary>Performs additional custom processes when clearing the contents of the <see cref="T:System.Xml.Serialization.XmlSchemas" /> instance.</summary>
		protected override void OnClear()
		{
			this.table.Clear();
		}

		/// <summary>Performs additional custom processes before inserting a new element into the <see cref="T:System.Xml.Serialization.XmlSchemas" /> instance.</summary>
		/// <param name="index">The zero-based index at which to insert <paramref name="value" />. </param>
		/// <param name="value">The new value of the element at <paramref name="index" />. </param>
		protected override void OnInsert(int index, object value)
		{
			string text = ((XmlSchema)value).TargetNamespace;
			if (text == null)
			{
				text = string.Empty;
			}
			this.table[text] = value;
		}

		/// <summary>Performs additional custom processes when removing an element from the <see cref="T:System.Xml.Serialization.XmlSchemas" /> instance.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> can be found. </param>
		/// <param name="value">The value of the element to remove at <paramref name="index" />. </param>
		protected override void OnRemove(int index, object value)
		{
			this.table.Remove(value);
		}

		/// <summary>Performs additional custom processes before setting a value in the <see cref="T:System.Xml.Serialization.XmlSchemas" /> instance.</summary>
		/// <param name="index">The zero-based index at which <paramref name="oldValue" /> can be found. </param>
		/// <param name="oldValue">The value to replace with <paramref name="newValue" />. </param>
		/// <param name="newValue">The new value of the element at <paramref name="index" />. </param>
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			string text = ((XmlSchema)oldValue).TargetNamespace;
			if (text == null)
			{
				text = string.Empty;
			}
			this.table[text] = newValue;
		}

		/// <summary>Removes the first occurrence of a specific schema from the <see cref="T:System.Xml.Serialization.XmlSchemas" />.</summary>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> to remove. </param>
		public void Remove(XmlSchema schema)
		{
			base.List.Remove(schema);
		}
	}
}
