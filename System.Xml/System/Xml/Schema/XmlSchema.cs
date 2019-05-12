using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>An in-memory representation of an XML Schema as specified in the World Wide Web Consortium (W3C) XML Schema Part 1: Structures and XML Schema Part 2: Datatypes specifications.</summary>
	[XmlRoot("schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
	public class XmlSchema : XmlSchemaObject
	{
		/// <summary>The XML schema namespace. This field is constant.</summary>
		public const string Namespace = "http://www.w3.org/2001/XMLSchema";

		/// <summary>The XML schema instance namespace. This field is constant. </summary>
		public const string InstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";

		internal const string XdtNamespace = "http://www.w3.org/2003/11/xpath-datatypes";

		private const string xmlname = "schema";

		private XmlSchemaForm attributeFormDefault;

		private XmlSchemaObjectTable attributeGroups;

		private XmlSchemaObjectTable attributes;

		private XmlSchemaDerivationMethod blockDefault;

		private XmlSchemaForm elementFormDefault;

		private XmlSchemaObjectTable elements;

		private XmlSchemaDerivationMethod finalDefault;

		private XmlSchemaObjectTable groups;

		private string id;

		private XmlSchemaObjectCollection includes;

		private XmlSchemaObjectCollection items;

		private XmlSchemaObjectTable notations;

		private XmlSchemaObjectTable schemaTypes;

		private string targetNamespace;

		private XmlAttribute[] unhandledAttributes;

		private string version;

		private XmlSchemaSet schemas;

		private XmlNameTable nameTable;

		internal bool missedSubComponents;

		private XmlSchemaObjectCollection compilationItems;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchema" /> class.</summary>
		public XmlSchema()
		{
			this.attributeFormDefault = XmlSchemaForm.None;
			this.blockDefault = XmlSchemaDerivationMethod.None;
			this.elementFormDefault = XmlSchemaForm.None;
			this.finalDefault = XmlSchemaDerivationMethod.None;
			this.includes = new XmlSchemaObjectCollection();
			this.isCompiled = false;
			this.items = new XmlSchemaObjectCollection();
			this.attributeGroups = new XmlSchemaObjectTable();
			this.attributes = new XmlSchemaObjectTable();
			this.elements = new XmlSchemaObjectTable();
			this.groups = new XmlSchemaObjectTable();
			this.notations = new XmlSchemaObjectTable();
			this.schemaTypes = new XmlSchemaObjectTable();
		}

		/// <summary>Gets or sets the form for attributes declared in the target namespace of the schema.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaForm" /> value that indicates if attributes from the target namespace are required to be qualified with the namespace prefix. The default is <see cref="F:System.Xml.Schema.XmlSchemaForm.None" />.</returns>
		[XmlAttribute("attributeFormDefault")]
		[DefaultValue(XmlSchemaForm.None)]
		public XmlSchemaForm AttributeFormDefault
		{
			get
			{
				return this.attributeFormDefault;
			}
			set
			{
				this.attributeFormDefault = value;
			}
		}

		/// <summary>Gets or sets the blockDefault attribute which sets the default value of the block attribute on element and complex types in the targetNamespace of the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaDerivationMethod" /> value representing the different methods for preventing derivation. The default value is XmlSchemaDerivationMethod.None.</returns>
		[XmlAttribute("blockDefault")]
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		public XmlSchemaDerivationMethod BlockDefault
		{
			get
			{
				return this.blockDefault;
			}
			set
			{
				this.blockDefault = value;
			}
		}

		/// <summary>Gets or sets the finalDefault attribute which sets the default value of the final attribute on elements and complex types in the target namespace of the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaDerivationMethod" /> value representing the different methods for preventing derivation. The default value is XmlSchemaDerivationMethod.None.</returns>
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		[XmlAttribute("finalDefault")]
		public XmlSchemaDerivationMethod FinalDefault
		{
			get
			{
				return this.finalDefault;
			}
			set
			{
				this.finalDefault = value;
			}
		}

		/// <summary>Gets or sets the form for elements declared in the target namespace of the schema.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaForm" /> value that indicates if elements from the target namespace are required to be qualified with the namespace prefix. The default is <see cref="F:System.Xml.Schema.XmlSchemaForm.None" />.</returns>
		[DefaultValue(XmlSchemaForm.None)]
		[XmlAttribute("elementFormDefault")]
		public XmlSchemaForm ElementFormDefault
		{
			get
			{
				return this.elementFormDefault;
			}
			set
			{
				this.elementFormDefault = value;
			}
		}

		/// <summary>Gets or sets the Uniform Resource Identifier (URI) of the schema target namespace.</summary>
		/// <returns>The schema target namespace.</returns>
		[XmlAttribute("targetNamespace", DataType = "anyURI")]
		public string TargetNamespace
		{
			get
			{
				return this.targetNamespace;
			}
			set
			{
				this.targetNamespace = value;
			}
		}

		/// <summary>Gets or sets the version of the schema.</summary>
		/// <returns>The version of the schema. The default value is String.Empty.</returns>
		[XmlAttribute("version", DataType = "token")]
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		/// <summary>Gets the collection of included and imported schemas.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectCollection" /> of the included and imported schemas.</returns>
		[XmlElement("redefine", typeof(XmlSchemaRedefine))]
		[XmlElement("import", typeof(XmlSchemaImport))]
		[XmlElement("include", typeof(XmlSchemaInclude))]
		public XmlSchemaObjectCollection Includes
		{
			get
			{
				return this.includes;
			}
		}

		/// <summary>Gets the collection of schema elements in the schema and is used to add new element types at the schema element level.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectCollection" /> of schema elements in the schema.</returns>
		[XmlElement("group", typeof(XmlSchemaGroup))]
		[XmlElement("notation", typeof(XmlSchemaNotation))]
		[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		[XmlElement("element", typeof(XmlSchemaElement))]
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		[XmlElement("complexType", typeof(XmlSchemaComplexType))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
		public XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Indicates if the schema has been compiled.</summary>
		/// <returns>true if schema has been compiled, otherwise, false. The default value is false.</returns>
		[XmlIgnore]
		public bool IsCompiled
		{
			get
			{
				return this.CompilationId != Guid.Empty;
			}
		}

		/// <summary>Gets the post-schema-compilation value for all the attributes in the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> collection of all the attributes in the schema.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		/// <summary>Gets the post-schema-compilation value of all the global attribute groups in the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> collection of all the global attribute groups in the schema.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable AttributeGroups
		{
			get
			{
				return this.attributeGroups;
			}
		}

		/// <summary>Gets the post-schema-compilation value of all schema types in the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectCollection" /> of all schema types in the schema.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable SchemaTypes
		{
			get
			{
				return this.schemaTypes;
			}
		}

		/// <summary>Gets the post-schema-compilation value for all the elements in the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> collection of all the elements in the schema.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable Elements
		{
			get
			{
				return this.elements;
			}
		}

		/// <summary>Gets or sets the string ID.</summary>
		/// <returns>The ID of the string. The default value is String.Empty.</returns>
		[XmlAttribute("id", DataType = "ID")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		/// <summary>Gets and sets the qualified attributes which do not belong to the schema target namespace.</summary>
		/// <returns>An array of qualified <see cref="T:System.Xml.XmlAttribute" /> objects that do not belong to the schema target namespace.</returns>
		[XmlAnyAttribute]
		public XmlAttribute[] UnhandledAttributes
		{
			get
			{
				if (this.unhandledAttributeList != null)
				{
					this.unhandledAttributes = (XmlAttribute[])this.unhandledAttributeList.ToArray(typeof(XmlAttribute));
					this.unhandledAttributeList = null;
				}
				return this.unhandledAttributes;
			}
			set
			{
				this.unhandledAttributes = value;
				this.unhandledAttributeList = null;
			}
		}

		/// <summary>Gets the post-schema-compilation value of all the groups in the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> collection of all the groups in the schema.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable Groups
		{
			get
			{
				return this.groups;
			}
		}

		/// <summary>Gets the post-schema-compilation value for all notations in the schema.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> collection of all notations in the schema.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable Notations
		{
			get
			{
				return this.notations;
			}
		}

		internal XmlSchemaObjectTable NamedIdentities
		{
			get
			{
				return this.schemas.NamedIdentities;
			}
		}

		internal XmlSchemaSet Schemas
		{
			get
			{
				return this.schemas;
			}
		}

		internal Hashtable IDCollection
		{
			get
			{
				return this.schemas.IDCollection;
			}
		}

		/// <summary>Compiles the XML Schema Object Model (SOM) into schema information for validation. Used to check the syntactic and semantic structure of the programmatically built SOM. Semantic validation checking is performed during compilation.</summary>
		/// <param name="validationEventHandler">The validation event handler that receives information about XML Schema validation errors. </param>
		[Obsolete("Use XmlSchemaSet.Compile() instead.")]
		public void Compile(ValidationEventHandler handler)
		{
			this.Compile(handler, new XmlUrlResolver());
		}

		/// <summary>Compiles the XML Schema Object Model (SOM) into schema information for validation. Used to check the syntactic and semantic structure of the programmatically built SOM. Semantic validation checking is performed during compilation.</summary>
		/// <param name="validationEventHandler">The validation event handler that receives information about the XML Schema validation errors. </param>
		/// <param name="resolver">The XmlResolver used to resolve namespaces referenced in include and import elements. </param>
		[Obsolete("Use XmlSchemaSet.Compile() instead.")]
		public void Compile(ValidationEventHandler handler, XmlResolver resolver)
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			if (handler != null)
			{
				xmlSchemaSet.ValidationEventHandler += handler;
			}
			xmlSchemaSet.XmlResolver = resolver;
			xmlSchemaSet.Add(this);
			xmlSchemaSet.Compile();
		}

		internal void CompileSubset(ValidationEventHandler handler, XmlSchemaSet col, XmlResolver resolver)
		{
			Hashtable handledUris = new Hashtable();
			this.CompileSubset(handler, col, resolver, handledUris);
		}

		internal void CompileSubset(ValidationEventHandler handler, XmlSchemaSet col, XmlResolver resolver, Hashtable handledUris)
		{
			if (base.SourceUri != null && base.SourceUri.Length > 0)
			{
				if (handledUris.Contains(base.SourceUri))
				{
					return;
				}
				handledUris.Add(base.SourceUri, base.SourceUri);
			}
			this.DoCompile(handler, handledUris, col, resolver);
		}

		private void SetParent()
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].SetParent(this);
			}
			for (int j = 0; j < this.Includes.Count; j++)
			{
				this.Includes[j].SetParent(this);
			}
		}

		private void DoCompile(ValidationEventHandler handler, Hashtable handledUris, XmlSchemaSet col, XmlResolver resolver)
		{
			this.SetParent();
			this.CompilationId = col.CompilationId;
			this.schemas = col;
			if (!this.schemas.Contains(this))
			{
				this.schemas.Add(this);
			}
			this.attributeGroups.Clear();
			this.attributes.Clear();
			this.elements.Clear();
			this.groups.Clear();
			this.notations.Clear();
			this.schemaTypes.Clear();
			if (this.BlockDefault != XmlSchemaDerivationMethod.All)
			{
				if ((this.BlockDefault & XmlSchemaDerivationMethod.List) != XmlSchemaDerivationMethod.Empty)
				{
					base.error(handler, "list is not allowed in blockDefault attribute");
				}
				if ((this.BlockDefault & XmlSchemaDerivationMethod.Union) != XmlSchemaDerivationMethod.Empty)
				{
					base.error(handler, "union is not allowed in blockDefault attribute");
				}
			}
			if (this.FinalDefault != XmlSchemaDerivationMethod.All && (this.FinalDefault & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
			{
				base.error(handler, "substitution is not allowed in finalDefault attribute");
			}
			XmlSchemaUtil.CompileID(this.Id, this, col.IDCollection, handler);
			if (this.TargetNamespace != null)
			{
				if (this.TargetNamespace.Length == 0)
				{
					base.error(handler, "The targetNamespace attribute cannot have have empty string as its value.");
				}
				if (!XmlSchemaUtil.CheckAnyUri(this.TargetNamespace))
				{
					base.error(handler, this.TargetNamespace + " is not a valid value for targetNamespace attribute of schema");
				}
			}
			if (!XmlSchemaUtil.CheckNormalizedString(this.Version))
			{
				base.error(handler, this.Version + "is not a valid value for version attribute of schema");
			}
			this.compilationItems = new XmlSchemaObjectCollection();
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.compilationItems.Add(this.Items[i]);
			}
			for (int j = 0; j < this.Includes.Count; j++)
			{
				this.ProcessExternal(handler, handledUris, resolver, this.Includes[j] as XmlSchemaExternal, col);
			}
			for (int k = 0; k < this.compilationItems.Count; k++)
			{
				XmlSchemaObject xmlSchemaObject = this.compilationItems[k];
				if (xmlSchemaObject is XmlSchemaAnnotation)
				{
					int num = ((XmlSchemaAnnotation)xmlSchemaObject).Compile(handler, this);
					this.errorCount += num;
				}
				else if (xmlSchemaObject is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
					int num2 = xmlSchemaAttribute.Compile(handler, this);
					this.errorCount += num2;
					if (num2 == 0)
					{
						XmlSchemaUtil.AddToTable(this.Attributes, xmlSchemaAttribute, xmlSchemaAttribute.QualifiedName, handler);
					}
				}
				else if (xmlSchemaObject is XmlSchemaAttributeGroup)
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)xmlSchemaObject;
					int num3 = xmlSchemaAttributeGroup.Compile(handler, this);
					this.errorCount += num3;
					if (num3 == 0)
					{
						XmlSchemaUtil.AddToTable(this.AttributeGroups, xmlSchemaAttributeGroup, xmlSchemaAttributeGroup.QualifiedName, handler);
					}
				}
				else if (xmlSchemaObject is XmlSchemaComplexType)
				{
					XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)xmlSchemaObject;
					int num4 = xmlSchemaComplexType.Compile(handler, this);
					this.errorCount += num4;
					if (num4 == 0)
					{
						XmlSchemaUtil.AddToTable(this.schemaTypes, xmlSchemaComplexType, xmlSchemaComplexType.QualifiedName, handler);
					}
				}
				else if (xmlSchemaObject is XmlSchemaSimpleType)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaObject;
					xmlSchemaSimpleType.islocal = false;
					int num5 = xmlSchemaSimpleType.Compile(handler, this);
					this.errorCount += num5;
					if (num5 == 0)
					{
						XmlSchemaUtil.AddToTable(this.SchemaTypes, xmlSchemaSimpleType, xmlSchemaSimpleType.QualifiedName, handler);
					}
				}
				else if (xmlSchemaObject is XmlSchemaElement)
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
					xmlSchemaElement.parentIsSchema = true;
					int num6 = xmlSchemaElement.Compile(handler, this);
					this.errorCount += num6;
					if (num6 == 0)
					{
						XmlSchemaUtil.AddToTable(this.Elements, xmlSchemaElement, xmlSchemaElement.QualifiedName, handler);
					}
				}
				else if (xmlSchemaObject is XmlSchemaGroup)
				{
					XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)xmlSchemaObject;
					int num7 = xmlSchemaGroup.Compile(handler, this);
					this.errorCount += num7;
					if (num7 == 0)
					{
						XmlSchemaUtil.AddToTable(this.Groups, xmlSchemaGroup, xmlSchemaGroup.QualifiedName, handler);
					}
				}
				else if (xmlSchemaObject is XmlSchemaNotation)
				{
					XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)xmlSchemaObject;
					int num8 = xmlSchemaNotation.Compile(handler, this);
					this.errorCount += num8;
					if (num8 == 0)
					{
						XmlSchemaUtil.AddToTable(this.Notations, xmlSchemaNotation, xmlSchemaNotation.QualifiedName, handler);
					}
				}
				else
				{
					ValidationHandler.RaiseValidationEvent(handler, null, string.Format("Object of Type {0} is not valid in Item Property of Schema", xmlSchemaObject.GetType().Name), null, this, null, XmlSeverityType.Error);
				}
			}
		}

		private string GetResolvedUri(XmlResolver resolver, string relativeUri)
		{
			Uri baseUri = null;
			if (base.SourceUri != null && base.SourceUri != string.Empty)
			{
				baseUri = new Uri(base.SourceUri);
			}
			Uri uri = resolver.ResolveUri(baseUri, relativeUri);
			return (!(uri != null)) ? string.Empty : uri.OriginalString;
		}

		private void ProcessExternal(ValidationEventHandler handler, Hashtable handledUris, XmlResolver resolver, XmlSchemaExternal ext, XmlSchemaSet col)
		{
			if (ext == null)
			{
				base.error(handler, string.Format("Object of Type {0} is not valid in Includes Property of XmlSchema", ext.GetType().Name));
				return;
			}
			XmlSchemaImport xmlSchemaImport = ext as XmlSchemaImport;
			if (ext.SchemaLocation == null && xmlSchemaImport == null)
			{
				return;
			}
			XmlSchema xmlSchema = null;
			if (ext.SchemaLocation != null)
			{
				Stream stream = null;
				string text = null;
				if (resolver != null)
				{
					text = this.GetResolvedUri(resolver, ext.SchemaLocation);
					if (handledUris.Contains(text))
					{
						return;
					}
					handledUris.Add(text, text);
					try
					{
						stream = (resolver.GetEntity(new Uri(text), null, typeof(Stream)) as Stream);
					}
					catch (Exception)
					{
						base.warn(handler, "Could not resolve schema location URI: " + text);
						stream = null;
					}
				}
				XmlSchemaRedefine xmlSchemaRedefine = ext as XmlSchemaRedefine;
				if (xmlSchemaRedefine != null)
				{
					for (int i = 0; i < xmlSchemaRedefine.Items.Count; i++)
					{
						XmlSchemaObject xmlSchemaObject = xmlSchemaRedefine.Items[i];
						xmlSchemaObject.isRedefinedComponent = true;
						xmlSchemaObject.isRedefineChild = true;
						if (xmlSchemaObject is XmlSchemaType || xmlSchemaObject is XmlSchemaGroup || xmlSchemaObject is XmlSchemaAttributeGroup)
						{
							this.compilationItems.Add(xmlSchemaObject);
						}
						else
						{
							base.error(handler, "Redefinition is only allowed to simpleType, complexType, group and attributeGroup.");
						}
					}
				}
				if (stream == null)
				{
					this.missedSubComponents = true;
					return;
				}
				XmlTextReader xmlTextReader = null;
				try
				{
					xmlTextReader = new XmlTextReader(text, stream, this.nameTable);
					xmlSchema = XmlSchema.Read(xmlTextReader, handler);
				}
				finally
				{
					if (xmlTextReader != null)
					{
						xmlTextReader.Close();
					}
				}
				xmlSchema.schemas = this.schemas;
				xmlSchema.SetParent();
				ext.Schema = xmlSchema;
			}
			if (xmlSchemaImport != null)
			{
				if (ext.Schema == null && ext.SchemaLocation == null)
				{
					foreach (object obj in col.Schemas())
					{
						XmlSchema xmlSchema2 = (XmlSchema)obj;
						if (xmlSchema2.TargetNamespace == xmlSchemaImport.Namespace)
						{
							xmlSchema = xmlSchema2;
							xmlSchema.schemas = this.schemas;
							xmlSchema.SetParent();
							ext.Schema = xmlSchema;
							break;
						}
					}
					if (xmlSchema == null)
					{
						return;
					}
				}
				else if (xmlSchema != null)
				{
					if (this.TargetNamespace == xmlSchema.TargetNamespace)
					{
						base.error(handler, "Target namespace must be different from that of included schema.");
						return;
					}
					if (xmlSchema.TargetNamespace != xmlSchemaImport.Namespace)
					{
						base.error(handler, "Attribute namespace and its importing schema's target namespace must be the same.");
						return;
					}
				}
			}
			else if (xmlSchema != null)
			{
				if (this.TargetNamespace == null && xmlSchema.TargetNamespace != null)
				{
					base.error(handler, "Target namespace is required to include a schema which has its own target namespace");
					return;
				}
				if (this.TargetNamespace != null && xmlSchema.TargetNamespace == null)
				{
					xmlSchema.TargetNamespace = this.TargetNamespace;
				}
			}
			if (xmlSchema != null)
			{
				this.AddExternalComponentsTo(xmlSchema, this.compilationItems, handler, handledUris, resolver, col);
			}
		}

		private void AddExternalComponentsTo(XmlSchema s, XmlSchemaObjectCollection items, ValidationEventHandler handler, Hashtable handledUris, XmlResolver resolver, XmlSchemaSet col)
		{
			foreach (XmlSchemaObject xmlSchemaObject in s.Includes)
			{
				XmlSchemaExternal ext = (XmlSchemaExternal)xmlSchemaObject;
				this.ProcessExternal(handler, handledUris, resolver, ext, col);
			}
			foreach (XmlSchemaObject item in s.Items)
			{
				items.Add(item);
			}
		}

		internal bool IsNamespaceAbsent(string ns)
		{
			return !this.schemas.Contains(ns);
		}

		internal XmlSchemaAttribute FindAttribute(XmlQualifiedName name)
		{
			foreach (object obj in this.schemas.Schemas())
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				XmlSchemaAttribute xmlSchemaAttribute = xmlSchema.Attributes[name] as XmlSchemaAttribute;
				if (xmlSchemaAttribute != null)
				{
					return xmlSchemaAttribute;
				}
			}
			return null;
		}

		internal XmlSchemaAttributeGroup FindAttributeGroup(XmlQualifiedName name)
		{
			foreach (object obj in this.schemas.Schemas())
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = xmlSchema.AttributeGroups[name] as XmlSchemaAttributeGroup;
				if (xmlSchemaAttributeGroup != null)
				{
					return xmlSchemaAttributeGroup;
				}
			}
			return null;
		}

		internal XmlSchemaElement FindElement(XmlQualifiedName name)
		{
			foreach (object obj in this.schemas.Schemas())
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				XmlSchemaElement xmlSchemaElement = xmlSchema.Elements[name] as XmlSchemaElement;
				if (xmlSchemaElement != null)
				{
					return xmlSchemaElement;
				}
			}
			return null;
		}

		internal XmlSchemaType FindSchemaType(XmlQualifiedName name)
		{
			foreach (object obj in this.schemas.Schemas())
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				XmlSchemaType xmlSchemaType = xmlSchema.SchemaTypes[name] as XmlSchemaType;
				if (xmlSchemaType != null)
				{
					return xmlSchemaType;
				}
			}
			return null;
		}

		internal void Validate(ValidationEventHandler handler)
		{
			this.ValidationId = this.CompilationId;
			foreach (object obj in this.Attributes.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj;
				this.errorCount += xmlSchemaAttribute.Validate(handler, this);
			}
			foreach (object obj2 in this.AttributeGroups.Values)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)obj2;
				this.errorCount += xmlSchemaAttributeGroup.Validate(handler, this);
			}
			foreach (object obj3 in this.SchemaTypes.Values)
			{
				XmlSchemaType xmlSchemaType = (XmlSchemaType)obj3;
				this.errorCount += xmlSchemaType.Validate(handler, this);
			}
			foreach (object obj4 in this.Elements.Values)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)obj4;
				this.errorCount += xmlSchemaElement.Validate(handler, this);
			}
			foreach (object obj5 in this.Groups.Values)
			{
				XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)obj5;
				this.errorCount += xmlSchemaGroup.Validate(handler, this);
			}
			foreach (object obj6 in this.Notations.Values)
			{
				XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)obj6;
				this.errorCount += xmlSchemaNotation.Validate(handler, this);
			}
			if (this.errorCount == 0)
			{
				this.isCompiled = true;
			}
			this.errorCount = 0;
		}

		/// <summary>Reads an XML Schema from the supplied <see cref="T:System.IO.TextReader" />.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchema" /> object representing the XML Schema.</returns>
		/// <param name="reader">The TextReader containing the XML Schema to read. </param>
		/// <param name="validationEventHandler">The validation event handler that receives information about the XML Schema syntax errors. </param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">An <see cref="T:System.Xml.Schema.XmlSchemaException" /> is raised if no <see cref="T:System.Xml.Schema.ValidationEventHandler" /> is specified.</exception>
		public static XmlSchema Read(TextReader reader, ValidationEventHandler validationEventHandler)
		{
			return XmlSchema.Read(new XmlTextReader(reader), validationEventHandler);
		}

		/// <summary>Reads an XML Schema  from the supplied stream.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchema" /> object representing the XML Schema.</returns>
		/// <param name="stream">The supplied data stream. </param>
		/// <param name="validationEventHandler">The validation event handler that receives information about XML Schema syntax errors. </param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">An <see cref="T:System.Xml.Schema.XmlSchemaException" /> is raised if no <see cref="T:System.Xml.Schema.ValidationEventHandler" /> is specified.</exception>
		public static XmlSchema Read(Stream stream, ValidationEventHandler validationEventHandler)
		{
			return XmlSchema.Read(new XmlTextReader(stream), validationEventHandler);
		}

		/// <summary>Reads an XML Schema from the supplied <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchema" /> object representing the XML Schema.</returns>
		/// <param name="reader">The XmlReader containing the XML Schema to read. </param>
		/// <param name="validationEventHandler">The validation event handler that receives information about the XML Schema syntax errors. </param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">An <see cref="T:System.Xml.Schema.XmlSchemaException" /> is raised if no <see cref="T:System.Xml.Schema.ValidationEventHandler" /> is specified.</exception>
		public static XmlSchema Read(XmlReader rdr, ValidationEventHandler validationEventHandler)
		{
			XmlSchemaReader xmlSchemaReader = new XmlSchemaReader(rdr, validationEventHandler);
			if (xmlSchemaReader.ReadState == ReadState.Initial)
			{
				xmlSchemaReader.ReadNextElement();
			}
			int depth = xmlSchemaReader.Depth;
			for (;;)
			{
				XmlNodeType nodeType = xmlSchemaReader.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					XmlSchemaObject.error(validationEventHandler, "This should never happen. XmlSchema.Read 1 ", null);
				}
				else
				{
					if (xmlSchemaReader.LocalName == "schema")
					{
						break;
					}
					XmlSchemaObject.error(validationEventHandler, "The root element must be schema", null);
				}
				if (xmlSchemaReader.Depth <= depth || !xmlSchemaReader.ReadNextElement())
				{
					goto IL_E7;
				}
			}
			XmlSchema xmlSchema = new XmlSchema();
			xmlSchema.nameTable = rdr.NameTable;
			xmlSchema.LineNumber = xmlSchemaReader.LineNumber;
			xmlSchema.LinePosition = xmlSchemaReader.LinePosition;
			xmlSchema.SourceUri = xmlSchemaReader.BaseURI;
			XmlSchema.ReadAttributes(xmlSchema, xmlSchemaReader, validationEventHandler);
			xmlSchemaReader.MoveToElement();
			if (!xmlSchemaReader.IsEmptyElement)
			{
				XmlSchema.ReadContent(xmlSchema, xmlSchemaReader, validationEventHandler);
			}
			else
			{
				rdr.Skip();
			}
			return xmlSchema;
			IL_E7:
			throw new XmlSchemaException("The top level schema must have namespace http://www.w3.org/2001/XMLSchema", null);
		}

		private static void ReadAttributes(XmlSchema schema, XmlSchemaReader reader, ValidationEventHandler h)
		{
			reader.MoveToElement();
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				switch (name)
				{
				case "attributeFormDefault":
				{
					Exception ex;
					schema.attributeFormDefault = XmlSchemaUtil.ReadFormAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for attributeFormDefault.", ex);
					}
					continue;
				}
				case "blockDefault":
				{
					Exception ex;
					schema.blockDefault = XmlSchemaUtil.ReadDerivationAttribute(reader, out ex, "blockDefault", XmlSchemaUtil.ElementBlockAllowed);
					if (ex != null)
					{
						XmlSchemaObject.error(h, ex.Message, ex);
					}
					continue;
				}
				case "elementFormDefault":
				{
					Exception ex;
					schema.elementFormDefault = XmlSchemaUtil.ReadFormAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for elementFormDefault.", ex);
					}
					continue;
				}
				case "finalDefault":
				{
					Exception ex;
					schema.finalDefault = XmlSchemaUtil.ReadDerivationAttribute(reader, out ex, "finalDefault", XmlSchemaUtil.FinalAllowed);
					if (ex != null)
					{
						XmlSchemaObject.error(h, ex.Message, ex);
					}
					continue;
				}
				case "id":
					schema.id = reader.Value;
					continue;
				case "targetNamespace":
					schema.targetNamespace = reader.Value;
					continue;
				case "version":
					schema.version = reader.Value;
					continue;
				}
				if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " attribute is not allowed in schema element", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, schema);
				}
			}
		}

		private static void ReadContent(XmlSchema schema, XmlSchemaReader reader, ValidationEventHandler h)
		{
			reader.MoveToElement();
			if (reader.LocalName != "schema" && reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" && reader.NodeType != XmlNodeType.Element)
			{
				XmlSchemaObject.error(h, "UNREACHABLE CODE REACHED: Method: Schema.ReadContent, " + reader.LocalName + ", " + reader.NamespaceURI, null);
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "schema")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchema.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1)
				{
					if (reader.LocalName == "include")
					{
						XmlSchemaInclude xmlSchemaInclude = XmlSchemaInclude.Read(reader, h);
						if (xmlSchemaInclude != null)
						{
							schema.includes.Add(xmlSchemaInclude);
						}
						continue;
					}
					if (reader.LocalName == "import")
					{
						XmlSchemaImport xmlSchemaImport = XmlSchemaImport.Read(reader, h);
						if (xmlSchemaImport != null)
						{
							schema.includes.Add(xmlSchemaImport);
						}
						continue;
					}
					if (reader.LocalName == "redefine")
					{
						XmlSchemaRedefine xmlSchemaRedefine = XmlSchemaRedefine.Read(reader, h);
						if (xmlSchemaRedefine != null)
						{
							schema.includes.Add(xmlSchemaRedefine);
						}
						continue;
					}
					if (reader.LocalName == "annotation")
					{
						XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
						if (xmlSchemaAnnotation != null)
						{
							schema.items.Add(xmlSchemaAnnotation);
						}
						continue;
					}
				}
				if (num <= 2)
				{
					num = 2;
					if (reader.LocalName == "simpleType")
					{
						XmlSchemaSimpleType xmlSchemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
						if (xmlSchemaSimpleType != null)
						{
							schema.items.Add(xmlSchemaSimpleType);
						}
						continue;
					}
					if (reader.LocalName == "complexType")
					{
						XmlSchemaComplexType xmlSchemaComplexType = XmlSchemaComplexType.Read(reader, h);
						if (xmlSchemaComplexType != null)
						{
							schema.items.Add(xmlSchemaComplexType);
						}
						continue;
					}
					if (reader.LocalName == "group")
					{
						XmlSchemaGroup xmlSchemaGroup = XmlSchemaGroup.Read(reader, h);
						if (xmlSchemaGroup != null)
						{
							schema.items.Add(xmlSchemaGroup);
						}
						continue;
					}
					if (reader.LocalName == "attributeGroup")
					{
						XmlSchemaAttributeGroup xmlSchemaAttributeGroup = XmlSchemaAttributeGroup.Read(reader, h);
						if (xmlSchemaAttributeGroup != null)
						{
							schema.items.Add(xmlSchemaAttributeGroup);
						}
						continue;
					}
					if (reader.LocalName == "element")
					{
						XmlSchemaElement xmlSchemaElement = XmlSchemaElement.Read(reader, h);
						if (xmlSchemaElement != null)
						{
							schema.items.Add(xmlSchemaElement);
						}
						continue;
					}
					if (reader.LocalName == "attribute")
					{
						XmlSchemaAttribute xmlSchemaAttribute = XmlSchemaAttribute.Read(reader, h);
						if (xmlSchemaAttribute != null)
						{
							schema.items.Add(xmlSchemaAttribute);
						}
						continue;
					}
					if (reader.LocalName == "notation")
					{
						XmlSchemaNotation xmlSchemaNotation = XmlSchemaNotation.Read(reader, h);
						if (xmlSchemaNotation != null)
						{
							schema.items.Add(xmlSchemaNotation);
						}
						continue;
					}
					if (reader.LocalName == "annotation")
					{
						XmlSchemaAnnotation xmlSchemaAnnotation2 = XmlSchemaAnnotation.Read(reader, h);
						if (xmlSchemaAnnotation2 != null)
						{
							schema.items.Add(xmlSchemaAnnotation2);
						}
						continue;
					}
				}
				reader.RaiseInvalidElementError();
			}
		}

		/// <summary>Writes the XML Schema to the supplied data stream.</summary>
		/// <param name="stream">The supplied data stream. </param>
		public void Write(Stream stream)
		{
			this.Write(stream, null);
		}

		/// <summary>Writes the XML Schema to the supplied <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="writer">The <see cref="T:System.IO.TextWriter" /> to write to.</param>
		public void Write(TextWriter writer)
		{
			this.Write(writer, null);
		}

		/// <summary>Writes the XML Schema to the supplied <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="writer" /> parameter is null.</exception>
		public void Write(XmlWriter writer)
		{
			this.Write(writer, null);
		}

		/// <summary>Writes the XML Schema to the supplied <see cref="T:System.IO.Stream" /> using the <see cref="T:System.Xml.XmlNamespaceManager" /> specified.</summary>
		/// <param name="stream">The supplied data stream. </param>
		/// <param name="namespaceManager">The <see cref="T:System.Xml.XmlNamespaceManager" />.</param>
		public void Write(Stream stream, XmlNamespaceManager namespaceManager)
		{
			this.Write(new XmlTextWriter(stream, null), namespaceManager);
		}

		/// <summary>Writes the XML Schema to the supplied <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="writer">The <see cref="T:System.IO.TextWriter" /> to write to.</param>
		/// <param name="namespaceManager">The <see cref="T:System.Xml.XmlNamespaceManager" />. </param>
		public void Write(TextWriter writer, XmlNamespaceManager namespaceManager)
		{
			this.Write(new XmlTextWriter(writer)
			{
				Formatting = Formatting.Indented
			}, namespaceManager);
		}

		/// <summary>Writes the XML Schema to the supplied <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
		/// <param name="namespaceManager">The <see cref="T:System.Xml.XmlNamespaceManager" />. </param>
		public void Write(XmlWriter writer, XmlNamespaceManager namespaceManager)
		{
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			if (namespaceManager != null)
			{
				foreach (object obj in namespaceManager)
				{
					string text = (string)obj;
					if (text != "xml" && text != "xmlns")
					{
						xmlSerializerNamespaces.Add(text, namespaceManager.LookupNamespace(text));
					}
				}
			}
			if (base.Namespaces != null && base.Namespaces.Count > 0)
			{
				XmlQualifiedName[] array = base.Namespaces.ToArray();
				foreach (XmlQualifiedName xmlQualifiedName in array)
				{
					xmlSerializerNamespaces.Add(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
				}
				string text2 = string.Empty;
				bool flag = true;
				int num = 1;
				while (flag)
				{
					flag = false;
					foreach (XmlQualifiedName xmlQualifiedName2 in array)
					{
						if (xmlQualifiedName2.Name == text2)
						{
							text2 = "q" + num;
							flag = true;
							break;
						}
					}
					num++;
				}
				xmlSerializerNamespaces.Add(text2, "http://www.w3.org/2001/XMLSchema");
			}
			if (xmlSerializerNamespaces.Count == 0)
			{
				xmlSerializerNamespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
				if (this.TargetNamespace != null && this.TargetNamespace.Length != 0)
				{
					xmlSerializerNamespaces.Add("tns", this.TargetNamespace);
				}
			}
			XmlSchemaSerializer xmlSchemaSerializer = new XmlSchemaSerializer();
			XmlSerializerNamespaces namespaces = base.Namespaces;
			try
			{
				base.Namespaces = null;
				xmlSchemaSerializer.Serialize(writer, this, xmlSerializerNamespaces);
			}
			finally
			{
				base.Namespaces = namespaces;
			}
			writer.Flush();
		}
	}
}
