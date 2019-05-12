using System;
using System.Collections;

namespace System.Xml.Schema
{
	/// <summary>Contains a cache of XML Schema definition language (XSD) schemas. </summary>
	public class XmlSchemaSet
	{
		private XmlNameTable nameTable;

		private XmlResolver xmlResolver = new XmlUrlResolver();

		private ArrayList schemas;

		private XmlSchemaObjectTable attributes;

		private XmlSchemaObjectTable elements;

		private XmlSchemaObjectTable types;

		private Hashtable idCollection;

		private XmlSchemaObjectTable namedIdentities;

		private XmlSchemaCompilationSettings settings = new XmlSchemaCompilationSettings();

		private bool isCompiled;

		internal Guid CompilationId;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> class.</summary>
		public XmlSchemaSet() : this(new NameTable())
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> class with the specified <see cref="T:System.Xml.XmlNameTable" />.</summary>
		/// <param name="nameTable">The <see cref="T:System.Xml.XmlNameTable" /> object to use.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlNameTable" /> object passed as a parameter is null.</exception>
		public XmlSchemaSet(XmlNameTable nameTable)
		{
			if (nameTable == null)
			{
				throw new ArgumentNullException("nameTable");
			}
			this.nameTable = nameTable;
			this.schemas = new ArrayList();
			this.CompilationId = Guid.NewGuid();
		}

		/// <summary>Sets an event handler for receiving information about XML Schema definition language (XSD) schema validation errors.</summary>
		public event ValidationEventHandler ValidationEventHandler;

		/// <summary>Gets the number of logical XML Schema definition language (XSD) schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>The number of logical schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</returns>
		public int Count
		{
			get
			{
				return this.schemas.Count;
			}
		}

		/// <summary>Gets all the global attributes in all the XML Schema definition language (XSD) schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</returns>
		public XmlSchemaObjectTable GlobalAttributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new XmlSchemaObjectTable();
				}
				return this.attributes;
			}
		}

		/// <summary>Gets all the global elements in all the XML Schema definition language (XSD) schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</returns>
		public XmlSchemaObjectTable GlobalElements
		{
			get
			{
				if (this.elements == null)
				{
					this.elements = new XmlSchemaObjectTable();
				}
				return this.elements;
			}
		}

		/// <summary>Gets all of the global simple and complex types in all the XML Schema definition language (XSD) schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />.</returns>
		public XmlSchemaObjectTable GlobalTypes
		{
			get
			{
				if (this.types == null)
				{
					this.types = new XmlSchemaObjectTable();
				}
				return this.types;
			}
		}

		/// <summary>Indicates if the XML Schema definition language (XSD) schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> have been compiled.</summary>
		/// <returns>Returns true if the schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> have been compiled since the last time a schema was added or removed from the <see cref="T:System.Xml.Schema.XmlSchemaSet" />; otherwise, false.</returns>
		public bool IsCompiled
		{
			get
			{
				return this.isCompiled;
			}
		}

		/// <summary>Gets the default <see cref="T:System.Xml.XmlNameTable" /> used by the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> when loading new XML Schema definition language (XSD) schemas.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNameTable" />.</returns>
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaCompilationSettings" /> for the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaCompilationSettings" /> for the <see cref="T:System.Xml.Schema.XmlSchemaSet" />. The default is an <see cref="T:System.Xml.Schema.XmlSchemaCompilationSettings" /> instance with the <see cref="P:System.Xml.Schema.XmlSchemaCompilationSettings.EnableUpaCheck" /> property set to true.</returns>
		public XmlSchemaCompilationSettings CompilationSettings
		{
			get
			{
				return this.settings;
			}
			set
			{
				this.settings = value;
			}
		}

		/// <summary>Sets the <see cref="T:System.Xml.XmlResolver" /> used to resolve namespaces or locations referenced in include and import elements of a schema.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlResolver" /> used to resolve namespaces or locations referenced in include and import elements of a schema.</returns>
		public XmlResolver XmlResolver
		{
			internal get
			{
				return this.xmlResolver;
			}
			set
			{
				this.xmlResolver = value;
			}
		}

		internal Hashtable IDCollection
		{
			get
			{
				if (this.idCollection == null)
				{
					this.idCollection = new Hashtable();
				}
				return this.idCollection;
			}
		}

		internal XmlSchemaObjectTable NamedIdentities
		{
			get
			{
				if (this.namedIdentities == null)
				{
					this.namedIdentities = new XmlSchemaObjectTable();
				}
				return this.namedIdentities;
			}
		}

		/// <summary>Adds the XML Schema definition language (XSD) schema at the URL specified to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> object if the schema is valid. If the schema is not valid and a <see cref="T:System.Xml.Schema.ValidationEventHandler" /> is specified, then null is returned and the appropriate validation event is raised. Otherwise, an <see cref="T:System.Xml.Schema.XmlSchemaException" /> is thrown.</returns>
		/// <param name="targetNamespace">The schema targetNamespace property, or null to use the targetNamespace specified in the schema.</param>
		/// <param name="schemaUri">The URL that specifies the schema to load.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">The schema is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The URL passed as a parameter is null or <see cref="F:System.String.Empty" />.</exception>
		public XmlSchema Add(string targetNamespace, string url)
		{
			XmlTextReader xmlTextReader = null;
			XmlSchema result;
			try
			{
				xmlTextReader = new XmlTextReader(url, this.nameTable);
				result = this.Add(targetNamespace, xmlTextReader);
			}
			finally
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
			}
			return result;
		}

		/// <summary>Adds the XML Schema definition language (XSD) schema contained in the <see cref="T:System.Xml.XmlReader" /> to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> object if the schema is valid. If the schema is not valid and a <see cref="T:System.Xml.Schema.ValidationEventHandler" /> is specified, then null is returned and the appropriate validation event is raised. Otherwise, an <see cref="T:System.Xml.Schema.XmlSchemaException" /> is thrown.</returns>
		/// <param name="targetNamespace">The schema targetNamespace property, or null to use the targetNamespace specified in the schema.</param>
		/// <param name="schemaDocument">The <see cref="T:System.Xml.XmlReader" /> object.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">The schema is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object passed as a parameter is null.</exception>
		public XmlSchema Add(string targetNamespace, XmlReader reader)
		{
			XmlSchema xmlSchema = XmlSchema.Read(reader, this.ValidationEventHandler);
			if (xmlSchema.TargetNamespace == null)
			{
				xmlSchema.TargetNamespace = targetNamespace;
			}
			else if (targetNamespace != null && xmlSchema.TargetNamespace != targetNamespace)
			{
				throw new XmlSchemaException("The actual targetNamespace in the schema does not match the parameter.");
			}
			this.Add(xmlSchema);
			return xmlSchema;
		}

		/// <summary>Adds all the XML Schema definition language (XSD) schemas in the given <see cref="T:System.Xml.Schema.XmlSchemaSet" /> to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <param name="schemas">The <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">A schema in the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object passed as a parameter is null.</exception>
		[MonoTODO]
		public void Add(XmlSchemaSet schemaSet)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in schemaSet.schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (!this.schemas.Contains(xmlSchema))
				{
					arrayList.Add(xmlSchema);
				}
			}
			foreach (object obj2 in arrayList)
			{
				XmlSchema schema = (XmlSchema)obj2;
				this.Add(schema);
			}
		}

		/// <summary>Adds the given <see cref="T:System.Xml.Schema.XmlSchema" /> to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> object if the schema is valid. If the schema is not valid and a <see cref="T:System.Xml.Schema.ValidationEventHandler" /> is specified, then null is returned and the appropriate validation event is raised. Otherwise an <see cref="T:System.Xml.Schema.XmlSchemaException" /> is thrown.</returns>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> object to add to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">The schema is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchema" /> object passed as a parameter is null.</exception>
		public XmlSchema Add(XmlSchema schema)
		{
			this.schemas.Add(schema);
			this.ResetCompile();
			return schema;
		}

		/// <summary>Compiles the XML Schema definition language (XSD) schemas added to the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> into one logical schema.</summary>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">An error occurred when validating and compiling the schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</exception>
		public void Compile()
		{
			this.ClearGlobalComponents();
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(this.schemas);
			this.IDCollection.Clear();
			this.NamedIdentities.Clear();
			Hashtable handledUris = new Hashtable();
			foreach (object obj in arrayList)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (!xmlSchema.IsCompiled)
				{
					xmlSchema.CompileSubset(this.ValidationEventHandler, this, this.xmlResolver, handledUris);
				}
			}
			foreach (object obj2 in arrayList)
			{
				XmlSchema xmlSchema2 = (XmlSchema)obj2;
				foreach (object obj3 in xmlSchema2.Elements.Values)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj3;
					xmlSchemaElement.FillSubstitutionElementInfo();
				}
			}
			foreach (object obj4 in arrayList)
			{
				XmlSchema xmlSchema3 = (XmlSchema)obj4;
				xmlSchema3.Validate(this.ValidationEventHandler);
			}
			foreach (object obj5 in arrayList)
			{
				XmlSchema schema = (XmlSchema)obj5;
				this.AddGlobalComponents(schema);
			}
			this.isCompiled = true;
		}

		private void ClearGlobalComponents()
		{
			this.GlobalElements.Clear();
			this.GlobalAttributes.Clear();
			this.GlobalTypes.Clear();
		}

		private void AddGlobalComponents(XmlSchema schema)
		{
			foreach (object obj in schema.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj;
				this.GlobalElements.Add(xmlSchemaElement.QualifiedName, xmlSchemaElement);
			}
			foreach (object obj2 in schema.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj2;
				this.GlobalAttributes.Add(xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
			}
			foreach (object obj3 in schema.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				this.GlobalTypes.Add(xmlSchemaType.QualifiedName, xmlSchemaType);
			}
		}

		/// <summary>Indicates whether an XML Schema definition language (XSD) schema with the specified target namespace URI is in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>Returns true if a schema with the specified target namespace URI is in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />; otherwise, false.</returns>
		/// <param name="targetNamespace">The schema targetNamespace property.</param>
		public bool Contains(string targetNamespace)
		{
			targetNamespace = this.GetSafeNs(targetNamespace);
			foreach (object obj in this.schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (this.GetSafeNs(xmlSchema.TargetNamespace) == targetNamespace)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Indicates whether the specified XML Schema definition language (XSD) <see cref="T:System.Xml.Schema.XmlSchema" /> object is in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.Schema.XmlSchema" /> object is in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />; otherwise, false.</returns>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchemaSet" /> passed as a parameter is null.</exception>
		public bool Contains(XmlSchema targetNamespace)
		{
			foreach (object obj in this.schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (xmlSchema == targetNamespace)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Copies all the <see cref="T:System.Xml.Schema.XmlSchema" /> objects from the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> to the given array, starting at the given index.</summary>
		/// <param name="schemas">The array to copy the objects to.</param>
		/// <param name="index">The index in the array where copying will begin.</param>
		public void CopyTo(XmlSchema[] array, int index)
		{
			this.schemas.CopyTo(array, index);
		}

		internal void CopyTo(Array array, int index)
		{
			this.schemas.CopyTo(array, index);
		}

		private string GetSafeNs(string ns)
		{
			return (ns != null) ? ns : string.Empty;
		}

		/// <summary>Removes the specified XML Schema definition language (XSD) schema from the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchema" /> object removed from the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> or null if the schema was not found in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</returns>
		/// <param name="schema">The <see cref="T:System.Xml.Schema.XmlSchema" /> object to remove from the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">The schema is not a valid schema.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchema" /> passed as a parameter is null.</exception>
		[MonoTODO]
		public XmlSchema Remove(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(this.schemas);
			if (!arrayList.Contains(schema))
			{
				return null;
			}
			if (!schema.IsCompiled)
			{
				schema.CompileSubset(this.ValidationEventHandler, this, this.xmlResolver);
			}
			this.schemas.Remove(schema);
			this.ResetCompile();
			return schema;
		}

		private void ResetCompile()
		{
			this.isCompiled = false;
			this.ClearGlobalComponents();
		}

		/// <summary>Removes the specified XML Schema definition language (XSD) schema and all the schemas it imports from the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.Schema.XmlSchema" /> object and all its imports were successfully removed; otherwise, false.</returns>
		/// <param name="schemaToRemove">The <see cref="T:System.Xml.Schema.XmlSchema" /> object to remove from the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchema" /> passed as a parameter is null.</exception>
		public bool RemoveRecursive(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(this.schemas);
			if (!arrayList.Contains(schema))
			{
				return false;
			}
			arrayList.Remove(schema);
			this.schemas.Remove(schema);
			if (!this.IsCompiled)
			{
				return true;
			}
			this.ClearGlobalComponents();
			foreach (object obj in arrayList)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (xmlSchema.IsCompiled)
				{
					this.AddGlobalComponents(schema);
				}
			}
			return true;
		}

		/// <summary>Reprocesses an XML Schema definition language (XSD) schema that already exists in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> object if the schema is a valid schema. If the schema is not valid and a <see cref="T:System.Xml.Schema.ValidationEventHandler" /> is specified, null is returned and the appropriate validation event is raised. Otherwise, an <see cref="T:System.Xml.Schema.XmlSchemaException" /> is thrown.</returns>
		/// <param name="schema">The schema to reprocess.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">The schema is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchema" /> object passed as a parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.Schema.XmlSchema" /> object passed as a parameter does not already exist in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</exception>
		public XmlSchema Reprocess(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(this.schemas);
			if (!arrayList.Contains(schema))
			{
				throw new ArgumentException("Target schema is not contained in the schema set.");
			}
			this.ClearGlobalComponents();
			foreach (object obj in arrayList)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (schema == xmlSchema)
				{
					schema.CompileSubset(this.ValidationEventHandler, this, this.xmlResolver);
				}
				if (xmlSchema.IsCompiled)
				{
					this.AddGlobalComponents(schema);
				}
			}
			return (!schema.IsCompiled) ? null : schema;
		}

		/// <summary>Returns a collection of all the XML Schema definition language (XSD) schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing all the schemas that have been added to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />. If no schemas have been added to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />, an empty <see cref="T:System.Collections.ICollection" /> object is returned.</returns>
		public ICollection Schemas()
		{
			return this.schemas;
		}

		/// <summary>Returns a collection of all the XML Schema definition language (XSD) schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> that belong to the given namespace.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing all the schemas that have been added to the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> that belong to the given namespace. If no schemas have been added to the <see cref="T:System.Xml.Schema.XmlSchemaSet" />, an empty <see cref="T:System.Collections.ICollection" /> object is returned.</returns>
		/// <param name="targetNamespace">The schema targetNamespace property.</param>
		public ICollection Schemas(string targetNamespace)
		{
			targetNamespace = this.GetSafeNs(targetNamespace);
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (this.GetSafeNs(xmlSchema.TargetNamespace) == targetNamespace)
				{
					arrayList.Add(xmlSchema);
				}
			}
			return arrayList;
		}

		internal bool MissedSubComponents(string targetNamespace)
		{
			foreach (object obj in this.Schemas(targetNamespace))
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				if (xmlSchema.missedSubComponents)
				{
					return true;
				}
			}
			return false;
		}
	}
}
