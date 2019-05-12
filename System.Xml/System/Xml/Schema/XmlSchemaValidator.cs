using Mono.Xml.Schema;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Xml.Schema
{
	/// <summary>Represents an XML Schema Definition Language (XSD) Schema validation engine. The <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> class cannot be inherited.</summary>
	public sealed class XmlSchemaValidator
	{
		private static readonly XmlSchemaAttribute[] emptyAttributeArray = new XmlSchemaAttribute[0];

		private object nominalEventSender;

		private IXmlLineInfo lineInfo;

		private IXmlNamespaceResolver nsResolver;

		private Uri sourceUri;

		private XmlNameTable nameTable;

		private XmlSchemaSet schemas;

		private XmlResolver xmlResolver = new XmlUrlResolver();

		private XmlSchemaObject startType;

		private XmlSchemaValidationFlags options;

		private XmlSchemaValidator.Transition transition;

		private XsdParticleStateManager state;

		private ArrayList occuredAtts = new ArrayList();

		private XmlSchemaAttribute[] defaultAttributes = XmlSchemaValidator.emptyAttributeArray;

		private ArrayList defaultAttributesCache = new ArrayList();

		private XsdIDManager idManager = new XsdIDManager();

		private ArrayList keyTables = new ArrayList();

		private ArrayList currentKeyFieldConsumers = new ArrayList();

		private ArrayList tmpKeyrefPool;

		private ArrayList elementQNameStack = new ArrayList();

		private StringBuilder storedCharacters = new StringBuilder();

		private bool shouldValidateCharacters;

		private int depth;

		private int xsiNilDepth = -1;

		private int skipValidationDepth = -1;

		internal XmlSchemaDatatype CurrentAttributeType;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> class.</summary>
		/// <param name="nameTable">An <see cref="T:System.Xml.XmlNameTable" /> object containing element and attribute names as atomized strings.</param>
		/// <param name="schemas">An <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object containing the XML Schema Definition Language (XSD) schemas used for validation.</param>
		/// <param name="namespaceResolver">An <see cref="T:System.Xml.IXmlNamespaceResolver" /> object used for resolving namespaces encountered during validation.</param>
		/// <param name="validationFlags">An <see cref="T:System.Xml.Schema.XmlSchemaValidationFlags" /> value specifying schema validation options.</param>
		/// <exception cref="T:System.ArgumentNullException">One or more of the parameters specified are null.</exception>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">An error occurred while compiling schemas in the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> parameter.</exception>
		public XmlSchemaValidator(XmlNameTable nameTable, XmlSchemaSet schemas, IXmlNamespaceResolver nsResolver, XmlSchemaValidationFlags options)
		{
			this.nameTable = nameTable;
			this.schemas = schemas;
			this.nsResolver = nsResolver;
			this.options = options;
		}

		/// <summary>The <see cref="T:System.Xml.Schema.ValidationEventHandler" /> that receives schema validation warnings and errors encountered during schema validation.</summary>
		public event ValidationEventHandler ValidationEventHandler;

		/// <summary>Gets or sets the object sent as the sender object of a validation event.</summary>
		/// <returns>An <see cref="T:System.Object" />; the default is this <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object.</returns>
		public object ValidationEventSender
		{
			get
			{
				return this.nominalEventSender;
			}
			set
			{
				this.nominalEventSender = value;
			}
		}

		/// <summary>Gets or sets the line number information for the XML node being validated.</summary>
		/// <returns>An <see cref="T:System.Xml.IXmlLineInfo" /> object.</returns>
		public IXmlLineInfo LineInfoProvider
		{
			get
			{
				return this.lineInfo;
			}
			set
			{
				this.lineInfo = value;
			}
		}

		/// <summary>Sets the <see cref="T:System.Xml.XmlResolver" /> object used to resolve xs:import and xs:include elements as well as xsi:schemaLocation and xsi:noNamespaceSchemaLocation attributes.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlResolver" /> object; the default is an <see cref="T:System.Xml.XmlUrlResolver" /> object.</returns>
		public XmlResolver XmlResolver
		{
			set
			{
				this.xmlResolver = value;
			}
		}

		/// <summary>Gets or sets the source URI for the XML node being validated.</summary>
		/// <returns>A <see cref="T:System.Uri" /> object representing the source URI for the XML node being validated; the default is null.</returns>
		public Uri SourceUri
		{
			get
			{
				return this.sourceUri;
			}
			set
			{
				this.sourceUri = value;
			}
		}

		private string BaseUri
		{
			get
			{
				return (!(this.sourceUri != null)) ? string.Empty : this.sourceUri.AbsoluteUri;
			}
		}

		private XsdValidationContext Context
		{
			get
			{
				return this.state.Context;
			}
		}

		private bool IgnoreWarnings
		{
			get
			{
				return (this.options & XmlSchemaValidationFlags.ReportValidationWarnings) == XmlSchemaValidationFlags.None;
			}
		}

		private bool IgnoreIdentity
		{
			get
			{
				return (this.options & XmlSchemaValidationFlags.ProcessIdentityConstraints) == XmlSchemaValidationFlags.None;
			}
		}

		/// <summary>Returns the expected attributes for the current element context.</summary>
		/// <returns>An array of <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> objects or an empty array if there are no expected attributes.</returns>
		public XmlSchemaAttribute[] GetExpectedAttributes()
		{
			XmlSchemaComplexType xmlSchemaComplexType = this.Context.ActualType as XmlSchemaComplexType;
			if (xmlSchemaComplexType == null)
			{
				return XmlSchemaValidator.emptyAttributeArray;
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in xmlSchemaComplexType.AttributeUses)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (!this.occuredAtts.Contains((XmlQualifiedName)dictionaryEntry.Key))
				{
					arrayList.Add(dictionaryEntry.Value);
				}
			}
			return (XmlSchemaAttribute[])arrayList.ToArray(typeof(XmlSchemaAttribute));
		}

		private void CollectAtomicParticles(XmlSchemaParticle p, ArrayList al)
		{
			if (p is XmlSchemaGroupBase)
			{
				foreach (XmlSchemaObject xmlSchemaObject in ((XmlSchemaGroupBase)p).Items)
				{
					XmlSchemaParticle p2 = (XmlSchemaParticle)xmlSchemaObject;
					this.CollectAtomicParticles(p2, al);
				}
			}
			else
			{
				al.Add(p);
			}
		}

		/// <summary>Returns the expected particles in the current element context.</summary>
		/// <returns>An array of <see cref="T:System.Xml.Schema.XmlSchemaParticle" /> objects or an empty array if there are no expected particles.</returns>
		[MonoTODO]
		public XmlSchemaParticle[] GetExpectedParticles()
		{
			ArrayList arrayList = new ArrayList();
			this.Context.State.GetExpectedParticles(arrayList);
			ArrayList arrayList2 = new ArrayList();
			foreach (object obj in arrayList)
			{
				XmlSchemaParticle p = (XmlSchemaParticle)obj;
				this.CollectAtomicParticles(p, arrayList2);
			}
			return (XmlSchemaParticle[])arrayList2.ToArray(typeof(XmlSchemaParticle));
		}

		/// <summary>Validates identity constraints on the default attributes and populates the specified <see cref="T:System.Collections.ArrayList" /> with <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> objects for any attributes with default values that have not already been validated using the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" /> method in the element context.</summary>
		/// <param name="defaultAttributes">An <see cref="T:System.Collections.ArrayList" /> to populate with <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> objects for any attributes not yet validated in the element context.</param>
		public void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributeList)
		{
			if (defaultAttributeList == null)
			{
				throw new ArgumentNullException("defaultAttributeList");
			}
			if (this.transition != XmlSchemaValidator.Transition.StartTag)
			{
				throw new InvalidOperationException("Method 'GetUnsoecifiedDefaultAttributes' works only when the validator state is inside a start tag.");
			}
			foreach (XmlSchemaAttribute xmlSchemaAttribute in this.GetExpectedAttributes())
			{
				if (xmlSchemaAttribute.ValidatedDefaultValue != null || xmlSchemaAttribute.ValidatedFixedValue != null)
				{
					defaultAttributeList.Add(xmlSchemaAttribute);
				}
			}
			defaultAttributeList.AddRange(this.defaultAttributes);
		}

		/// <summary>Adds an XML Schema Definition Language (XSD) schema to the set of schemas used for validation.</summary>
		/// <param name="schema">An <see cref="T:System.Xml.Schema.XmlSchema" /> object to add to the set of schemas used for validation.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchema" /> parameter specified is null.</exception>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The target namespace of the <see cref="T:System.Xml.Schema.XmlSchema" /> parameter matches that of any element or attribute already encountered by the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object.</exception>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaException">The <see cref="T:System.Xml.Schema.XmlSchema" /> parameter is invalid.</exception>
		public void AddSchema(XmlSchema schema)
		{
			if (schema == null)
			{
				throw new ArgumentNullException("schema");
			}
			this.schemas.Add(schema);
			this.schemas.Compile();
		}

		/// <summary>Initializes the state of the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object.</summary>
		/// <exception cref="T:System.InvalidOperationException">Calling the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.Initialize" /> method is valid immediately after the construction of an <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object or after a call to <see cref="M:System.Xml.Schema.XmlSchemaValidator.EndValidation" /> only.</exception>
		public void Initialize()
		{
			this.transition = XmlSchemaValidator.Transition.Content;
			this.state = new XsdParticleStateManager();
			if (!this.schemas.IsCompiled)
			{
				this.schemas.Compile();
			}
		}

		/// <summary>Initializes the state of the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object using the <see cref="T:System.Xml.Schema.XmlSchemaObject" /> specified for partial validation.</summary>
		/// <param name="partialValidationType">An <see cref="T:System.Xml.Schema.XmlSchemaElement" />, <see cref="T:System.Xml.Schema.XmlSchemaAttribute" />, or <see cref="T:System.Xml.Schema.XmlSchemaType" /> object used to initialize the validation context of the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object for partial validation.</param>
		/// <exception cref="T:System.InvalidOperationException">Calling the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.Initialize" /> method is valid immediately after the construction of an <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object or after a call to <see cref="M:System.Xml.Schema.XmlSchemaValidator.EndValidation" /> only.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> parameter is not an <see cref="T:System.Xml.Schema.XmlSchemaElement" />, <see cref="T:System.Xml.Schema.XmlSchemaAttribute" />, or <see cref="T:System.Xml.Schema.XmlSchemaType" /> object.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> parameter cannot be null.</exception>
		public void Initialize(XmlSchemaObject partialValidationType)
		{
			if (partialValidationType == null)
			{
				throw new ArgumentNullException("partialValidationType");
			}
			this.startType = partialValidationType;
			this.Initialize();
		}

		/// <summary>Ends validation and checks identity constraints for the entire XML document.</summary>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">An identity constraint error was found in the XML document.</exception>
		public void EndValidation()
		{
			this.CheckState(XmlSchemaValidator.Transition.Content);
			this.transition = XmlSchemaValidator.Transition.Finished;
			if (this.schemas.Count == 0)
			{
				return;
			}
			if (this.depth > 0)
			{
				throw new InvalidOperationException(string.Format("There are {0} open element(s). ValidateEndElement() must be called for each open element.", this.depth));
			}
			if (!this.IgnoreIdentity && this.idManager.HasMissingIDReferences())
			{
				this.HandleError("There are missing ID references: " + this.idManager.GetMissingIDString());
			}
		}

		/// <summary>Skips validation of the current element content and prepares the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object to validate content in the parent element's context.</summary>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set if the current element content is successfully skipped. This parameter can be null.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Xml.Schema.XmlSchemaValidator.SkipToEndElement(System.Xml.Schema.XmlSchemaInfo)" /> method was not called in the correct sequence. For example, calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.SkipToEndElement(System.Xml.Schema.XmlSchemaInfo)" /> after calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.SkipToEndElement(System.Xml.Schema.XmlSchemaInfo)" />.</exception>
		[MonoTODO]
		public void SkipToEndElement(XmlSchemaInfo info)
		{
			this.CheckState(XmlSchemaValidator.Transition.Content);
			if (this.schemas.Count == 0)
			{
				return;
			}
			this.state.PopContext();
		}

		/// <summary>Validates the attribute name, namespace URI, and value in the current element context.</summary>
		/// <returns>The validated attribute's value.</returns>
		/// <param name="localName">The local name of the attribute to validate.</param>
		/// <param name="namespaceUri">The namespace URI of the attribute to validate.</param>
		/// <param name="attributeValue">The value of the attribute to validate.</param>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set on successful validation of the attribute. This parameter can be null.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The attribute is not valid in the current element context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" /> method was not called in the correct sequence. For example, calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" /> after calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.ValidateEndOfAttributes(System.Xml.Schema.XmlSchemaInfo)" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">One or more of the parameters specified are null.</exception>
		public object ValidateAttribute(string localName, string ns, string attributeValue, XmlSchemaInfo info)
		{
			if (attributeValue == null)
			{
				throw new ArgumentNullException("attributeValue");
			}
			return this.ValidateAttribute(localName, ns, () => attributeValue, info);
		}

		/// <summary>Validates the attribute name, namespace URI, and value in the current element context.</summary>
		/// <returns>The validated attribute's value.</returns>
		/// <param name="localName">The local name of the attribute to validate.</param>
		/// <param name="namespaceUri">The namespace URI of the attribute to validate.</param>
		/// <param name="attributeValue">An <see cref="T:System.Xml.Schema.XmlValueGetter" />delegate used to pass the attribute's value as a Common Language Runtime (CLR) type compatible with the XML Schema Definition Language (XSD) type of the attribute.</param>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set on successful validation of the attribute. This parameter and can be null.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The attribute is not valid in the current element context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" /> method was not called in the correct sequence. For example, calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" /> after calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.ValidateEndOfAttributes(System.Xml.Schema.XmlSchemaInfo)" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">One or more of the parameters specified are null.</exception>
		public object ValidateAttribute(string localName, string ns, XmlValueGetter attributeValue, XmlSchemaInfo info)
		{
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			if (ns == null)
			{
				throw new ArgumentNullException("ns");
			}
			if (attributeValue == null)
			{
				throw new ArgumentNullException("attributeValue");
			}
			this.CheckState(XmlSchemaValidator.Transition.StartTag);
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(localName, ns);
			if (this.occuredAtts.Contains(xmlQualifiedName))
			{
				throw new InvalidOperationException(string.Format("Attribute '{0}' has already been validated in the same element.", xmlQualifiedName));
			}
			this.occuredAtts.Add(xmlQualifiedName);
			if (ns == "http://www.w3.org/2000/xmlns/")
			{
				return null;
			}
			if (this.schemas.Count == 0)
			{
				return null;
			}
			if (this.Context.Element != null && this.Context.XsiType == null)
			{
				if (this.Context.ActualType is XmlSchemaComplexType)
				{
					return this.AssessAttributeElementLocallyValidType(localName, ns, attributeValue, info);
				}
				this.HandleError("Current simple type cannot accept attributes other than schema instance namespace.");
			}
			return null;
		}

		/// <summary>Validates the element in the current context.</summary>
		/// <param name="localName">The local name of the element to validate.</param>
		/// <param name="namespaceUri">The namespace URI of the element to validate.</param>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set on successful validation of the element's name. This parameter can be null.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The element's name is not valid in the current context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateElement" /> method was not called in the correct sequence. For example, the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateElement" /> method is called after calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" />.</exception>
		public void ValidateElement(string localName, string ns, XmlSchemaInfo info)
		{
			this.ValidateElement(localName, ns, info, null, null, null, null);
		}

		/// <summary>Validates the element in the current context with the xsi:Type, xsi:Nil, xsi:SchemaLocation, and xsi:NoNamespaceSchemaLocation attribute values specified.</summary>
		/// <param name="localName">The local name of the element to validate.</param>
		/// <param name="namespaceUri">The namespace URI of the element to validate.</param>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set on successful validation of the element's name. This parameter can be null.</param>
		/// <param name="xsiType">The xsi:Type attribute value of the element. This parameter can be null.</param>
		/// <param name="xsiNil">The xsi:Nil attribute value of the element. This parameter can be null.</param>
		/// <param name="xsiSchemaLocation">The xsi:SchemaLocation attribute value of the element. This parameter can be null.</param>
		/// <param name="xsiNoNamespaceSchemaLocation">The xsi:NoNamespaceSchemaLocation attribute value of the element. This parameter can be null.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The element's name is not valid in the current context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateElement" /> method was not called in the correct sequence. For example, the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateElement" /> method is called after calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" />.</exception>
		public void ValidateElement(string localName, string ns, XmlSchemaInfo info, string xsiType, string xsiNil, string schemaLocation, string noNsSchemaLocation)
		{
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			if (ns == null)
			{
				throw new ArgumentNullException("ns");
			}
			this.CheckState(XmlSchemaValidator.Transition.Content);
			this.transition = XmlSchemaValidator.Transition.StartTag;
			if (schemaLocation != null)
			{
				this.HandleSchemaLocation(schemaLocation);
			}
			if (noNsSchemaLocation != null)
			{
				this.HandleNoNSSchemaLocation(noNsSchemaLocation);
			}
			this.elementQNameStack.Add(new XmlQualifiedName(localName, ns));
			if (this.schemas.Count == 0)
			{
				return;
			}
			if (!this.IgnoreIdentity)
			{
				this.idManager.OnStartElement();
			}
			this.defaultAttributes = XmlSchemaValidator.emptyAttributeArray;
			if (this.skipValidationDepth < 0 || this.depth <= this.skipValidationDepth)
			{
				if (this.shouldValidateCharacters)
				{
					this.ValidateEndSimpleContent(null);
				}
				this.AssessOpenStartElementSchemaValidity(localName, ns);
			}
			if (xsiNil != null)
			{
				this.HandleXsiNil(xsiNil, info);
			}
			if (xsiType != null)
			{
				this.HandleXsiType(xsiType);
			}
			if (this.xsiNilDepth < this.depth)
			{
				this.shouldValidateCharacters = true;
			}
			if (info != null)
			{
				info.IsNil = (this.xsiNilDepth >= 0);
				info.SchemaElement = this.Context.Element;
				info.SchemaType = this.Context.ActualSchemaType;
				info.SchemaAttribute = null;
				info.IsDefault = false;
				info.MemberType = null;
			}
		}

		/// <summary>Verifies if the text content of the element is valid according to its data type for elements with simple content, and verifies if the content of the current element is complete according to its data type for elements with complex content.</summary>
		/// <returns>The parsed, typed text value of the element if the element has simple content.</returns>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set on successful validation of the element. This parameter can be null.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The element's content is not valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateEndElement" /> method was not called in the correct sequence. For example, if the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateEndElement" /> method is called after calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.SkipToEndElement(System.Xml.Schema.XmlSchemaInfo)" />.</exception>
		public object ValidateEndElement(XmlSchemaInfo info)
		{
			return this.ValidateEndElement(info, null);
		}

		/// <summary>Verifies if the text content of the element specified is valid according to its data type.</summary>
		/// <returns>The parsed, typed simple content of the element.</returns>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set on successful validation of the text content of the element. This parameter can be null.</param>
		/// <param name="typedValue">The typed text content of the element.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The element's text content is not valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateEndElement" /> method was not called in the correct sequence (for example, if the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateEndElement" /> method is called after calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.SkipToEndElement(System.Xml.Schema.XmlSchemaInfo)" />), calls to the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateText" /> method have been previously made, or the element has complex content.</exception>
		/// <exception cref="T:System.ArgumentNullException">The typed text content parameter cannot be null.</exception>
		[MonoTODO]
		public object ValidateEndElement(XmlSchemaInfo info, object var)
		{
			if (this.transition == XmlSchemaValidator.Transition.StartTag)
			{
				this.ValidateEndOfAttributes(info);
			}
			this.CheckState(XmlSchemaValidator.Transition.Content);
			this.elementQNameStack.RemoveAt(this.elementQNameStack.Count - 1);
			if (this.schemas.Count == 0)
			{
				return null;
			}
			if (this.depth == 0)
			{
				throw new InvalidOperationException("There was no corresponding call to 'ValidateElement' method.");
			}
			this.depth--;
			object result = null;
			if (this.depth == this.skipValidationDepth)
			{
				this.skipValidationDepth = -1;
			}
			else if (this.skipValidationDepth < 0 || this.depth <= this.skipValidationDepth)
			{
				result = this.AssessEndElementSchemaValidity(info);
			}
			return result;
		}

		/// <summary>Verifies whether all the required attributes in the element context are present and prepares the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> object to validate the child content of the element.</summary>
		/// <param name="schemaInfo">An <see cref="T:System.Xml.Schema.XmlSchemaInfo" /> object whose properties are set on successful verification that all the required attributes in the element context are present. This parameter can be null.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">One or more of the required attributes in the current element context were not found.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Xml.Schema.XmlSchemaValidator.ValidateEndOfAttributes(System.Xml.Schema.XmlSchemaInfo)" /> method was not called in the correct sequence. For example, calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.ValidateEndOfAttributes(System.Xml.Schema.XmlSchemaInfo)" /> after calling <see cref="M:System.Xml.Schema.XmlSchemaValidator.SkipToEndElement(System.Xml.Schema.XmlSchemaInfo)" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">One or more of the parameters specified are null.</exception>
		public void ValidateEndOfAttributes(XmlSchemaInfo info)
		{
			try
			{
				this.CheckState(XmlSchemaValidator.Transition.StartTag);
				this.transition = XmlSchemaValidator.Transition.Content;
				if (this.schemas.Count != 0)
				{
					if (this.skipValidationDepth < 0 || this.depth <= this.skipValidationDepth)
					{
						this.AssessCloseStartElementSchemaValidity(info);
					}
					this.depth++;
				}
			}
			finally
			{
				this.occuredAtts.Clear();
			}
		}

		/// <summary>Validates whether the text string specified is allowed in the current element context, and accumulates the text for validation if the current element has simple content.</summary>
		/// <param name="elementValue">A text string to validate in the current element context.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The text string specified is not allowed in the current element context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateText" /> method was not called in the correct sequence. For example, the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateText" /> method is called after calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The text string parameter cannot be null.</exception>
		public void ValidateText(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.ValidateText(() => value);
		}

		/// <summary>Validates whether the text returned by the <see cref="T:System.Xml.Schema.XmlValueGetter" /> object specified is allowed in the current element context, and accumulates the text for validation if the current element has simple content.</summary>
		/// <param name="elementValue">An <see cref="T:System.Xml.Schema.XmlValueGetter" />delegate used to pass the text value as a Common Language Runtime (CLR) type compatible with the XML Schema Definition Language (XSD) type of the attribute.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">The text string specified is not allowed in the current element context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateText" /> method was not called in the correct sequence. For example, the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateText" /> method is called after calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The text string parameter cannot be null.</exception>
		public void ValidateText(XmlValueGetter getter)
		{
			if (getter == null)
			{
				throw new ArgumentNullException("getter");
			}
			this.CheckState(XmlSchemaValidator.Transition.Content);
			if (this.schemas.Count == 0)
			{
				return;
			}
			if (this.skipValidationDepth >= 0 && this.depth > this.skipValidationDepth)
			{
				return;
			}
			XmlSchemaComplexType xmlSchemaComplexType = this.Context.ActualType as XmlSchemaComplexType;
			if (xmlSchemaComplexType != null)
			{
				XmlSchemaContentType contentType = xmlSchemaComplexType.ContentType;
				if (contentType != XmlSchemaContentType.Empty)
				{
					if (contentType == XmlSchemaContentType.ElementOnly)
					{
						string text = this.storedCharacters.ToString();
						if (text.Length > 0 && !XmlChar.IsWhitespace(text))
						{
							this.HandleError("Not allowed character content was found.");
						}
					}
				}
				else
				{
					this.HandleError("Not allowed character content was found.");
				}
			}
			this.ValidateCharacters(getter);
		}

		/// <summary>Validates whether the white space in the string specified is allowed in the current element context, and accumulates the white space for validation if the current element has simple content.</summary>
		/// <param name="elementValue">A white space string to validate in the current element context.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">White space is not allowed in the current element context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateWhitespace" /> method was not called in the correct sequence. For example, if the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateWhitespace" /> method is called after calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" />.</exception>
		public void ValidateWhitespace(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.ValidateWhitespace(() => value);
		}

		/// <summary>Validates whether the white space returned by the <see cref="T:System.Xml.Schema.XmlValueGetter" /> object specified is allowed in the current element context, and accumulates the white space for validation if the current element has simple content.</summary>
		/// <param name="elementValue">An <see cref="T:System.Xml.Schema.XmlValueGetter" />delegate used to pass the white space value as a Common Language Runtime (CLR) type compatible with the XML Schema Definition Language (XSD) type of the attribute.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">White space is not allowed in the current element context.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateWhitespace" /> method was not called in the correct sequence. For example, if the <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateWhitespace" /> method is called after calling <see cref="Overload:System.Xml.Schema.XmlSchemaValidator.ValidateAttribute" />.</exception>
		public void ValidateWhitespace(XmlValueGetter getter)
		{
			this.ValidateText(getter);
		}

		private void HandleError(string message)
		{
			this.HandleError(message, null, false);
		}

		private void HandleError(string message, Exception innerException)
		{
			this.HandleError(message, innerException, false);
		}

		private void HandleError(string message, Exception innerException, bool isWarning)
		{
			if (isWarning && this.IgnoreWarnings)
			{
				return;
			}
			XmlSchemaValidationException exception = new XmlSchemaValidationException(message, this.nominalEventSender, this.BaseUri, null, innerException);
			this.HandleError(exception, isWarning);
		}

		private void HandleError(XmlSchemaValidationException exception)
		{
			this.HandleError(exception, false);
		}

		private void HandleError(XmlSchemaValidationException exception, bool isWarning)
		{
			if (isWarning && this.IgnoreWarnings)
			{
				return;
			}
			if (this.ValidationEventHandler == null)
			{
				throw exception;
			}
			ValidationEventArgs e = new ValidationEventArgs(exception, exception.Message, (!isWarning) ? XmlSeverityType.Error : XmlSeverityType.Warning);
			this.ValidationEventHandler(this.nominalEventSender, e);
		}

		private void CheckState(XmlSchemaValidator.Transition expected)
		{
			if (this.transition == expected)
			{
				return;
			}
			if (this.transition == XmlSchemaValidator.Transition.None)
			{
				throw new InvalidOperationException("Initialize() must be called before processing validation.");
			}
			throw new InvalidOperationException(string.Format("Unexpected attempt to validate state transition from {0} to {1}.", this.transition, expected));
		}

		private XmlSchemaElement FindElement(string name, string ns)
		{
			return (XmlSchemaElement)this.schemas.GlobalElements[new XmlQualifiedName(name, ns)];
		}

		private XmlSchemaType FindType(XmlQualifiedName qname)
		{
			return (XmlSchemaType)this.schemas.GlobalTypes[qname];
		}

		private void ValidateStartElementParticle(string localName, string ns)
		{
			if (this.Context.State == null)
			{
				return;
			}
			this.Context.XsiType = null;
			this.state.CurrentElement = null;
			this.Context.EvaluateStartElement(localName, ns);
			if (this.Context.IsInvalid)
			{
				this.HandleError("Invalid start element: " + ns + ":" + localName);
			}
			this.Context.PushCurrentElement(this.state.CurrentElement);
		}

		private void AssessOpenStartElementSchemaValidity(string localName, string ns)
		{
			if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.depth)
			{
				this.HandleError("Element item appeared, while current element context is nil.");
			}
			this.ValidateStartElementParticle(localName, ns);
			if (this.Context.Element == null)
			{
				this.state.CurrentElement = this.FindElement(localName, ns);
				this.Context.PushCurrentElement(this.state.CurrentElement);
			}
			if (!this.IgnoreIdentity)
			{
				this.ValidateKeySelectors();
				this.ValidateKeyFields(false, this.xsiNilDepth == this.depth, this.Context.ActualType, null, null, null);
			}
		}

		private void AssessCloseStartElementSchemaValidity(XmlSchemaInfo info)
		{
			if (this.Context.XsiType != null)
			{
				this.AssessCloseStartElementLocallyValidType(info);
			}
			else if (this.Context.Element != null)
			{
				this.AssessElementLocallyValidElement();
				if (this.Context.Element.ElementType != null)
				{
					this.AssessCloseStartElementLocallyValidType(info);
				}
			}
			if (this.Context.Element == null)
			{
				XmlSchemaContentProcessing processContents = this.state.ProcessContents;
				if (processContents != XmlSchemaContentProcessing.Skip)
				{
					if (processContents != XmlSchemaContentProcessing.Lax)
					{
						XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)this.elementQNameStack[this.elementQNameStack.Count - 1];
						if (this.Context.XsiType == null && (this.schemas.Contains(xmlQualifiedName.Namespace) || !this.schemas.MissedSubComponents(xmlQualifiedName.Namespace)))
						{
							this.HandleError("Element declaration for " + xmlQualifiedName + " is missing.");
						}
					}
				}
			}
			this.state.PushContext();
			XsdValidationState xsdValidationState = null;
			if (this.state.ProcessContents == XmlSchemaContentProcessing.Skip)
			{
				this.skipValidationDepth = this.depth;
			}
			else
			{
				XmlSchemaComplexType xmlSchemaComplexType = this.Context.ActualType as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null)
				{
					xsdValidationState = this.state.Create(xmlSchemaComplexType.ValidatableParticle);
				}
				else if (this.state.ProcessContents == XmlSchemaContentProcessing.Lax)
				{
					xsdValidationState = this.state.Create(XmlSchemaAny.AnyTypeContent);
				}
				else
				{
					xsdValidationState = this.state.Create(XmlSchemaParticle.Empty);
				}
			}
			this.Context.State = xsdValidationState;
		}

		private void AssessElementLocallyValidElement()
		{
			XmlSchemaElement element = this.Context.Element;
			XmlQualifiedName arg = (XmlQualifiedName)this.elementQNameStack[this.elementQNameStack.Count - 1];
			if (element == null)
			{
				this.HandleError("Element declaration is required for " + arg);
			}
			if (element.ActualIsAbstract)
			{
				this.HandleError("Abstract element declaration was specified for " + arg);
			}
		}

		private void AssessCloseStartElementLocallyValidType(XmlSchemaInfo info)
		{
			object actualType = this.Context.ActualType;
			if (actualType == null)
			{
				this.HandleError("Schema type does not exist.");
				return;
			}
			XmlSchemaComplexType xmlSchemaComplexType = actualType as XmlSchemaComplexType;
			XmlSchemaSimpleType xmlSchemaSimpleType = actualType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType == null)
			{
				if (xmlSchemaComplexType != null)
				{
					this.AssessCloseStartElementLocallyValidComplexType(xmlSchemaComplexType, info);
				}
			}
		}

		private void AssessCloseStartElementLocallyValidComplexType(XmlSchemaComplexType cType, XmlSchemaInfo info)
		{
			if (cType.IsAbstract)
			{
				this.HandleError("Target complex type is abstract.");
				return;
			}
			foreach (XmlSchemaAttribute xmlSchemaAttribute in this.GetExpectedAttributes())
			{
				if (xmlSchemaAttribute.ValidatedUse == XmlSchemaUse.Required && xmlSchemaAttribute.ValidatedFixedValue == null)
				{
					this.HandleError("Required attribute " + xmlSchemaAttribute.QualifiedName + " was not found.");
				}
				else if (xmlSchemaAttribute.ValidatedDefaultValue != null || xmlSchemaAttribute.ValidatedFixedValue != null)
				{
					this.defaultAttributesCache.Add(xmlSchemaAttribute);
				}
			}
			if (this.defaultAttributesCache.Count == 0)
			{
				this.defaultAttributes = XmlSchemaValidator.emptyAttributeArray;
			}
			else
			{
				this.defaultAttributes = (XmlSchemaAttribute[])this.defaultAttributesCache.ToArray(typeof(XmlSchemaAttribute));
			}
			this.defaultAttributesCache.Clear();
			if (!this.IgnoreIdentity)
			{
				foreach (XmlSchemaAttribute xmlSchemaAttribute2 in this.defaultAttributes)
				{
					XmlSchemaDatatype dt = (xmlSchemaAttribute2.AttributeType as XmlSchemaDatatype) ?? xmlSchemaAttribute2.AttributeSchemaType.Datatype;
					object parsedValue = xmlSchemaAttribute2.ValidatedFixedValue ?? xmlSchemaAttribute2.ValidatedDefaultValue;
					string text = this.idManager.AssessEachAttributeIdentityConstraint(dt, parsedValue, ((XmlQualifiedName)this.elementQNameStack[this.elementQNameStack.Count - 1]).Name);
					if (text != null)
					{
						this.HandleError(text);
					}
				}
			}
			if (!this.IgnoreIdentity)
			{
				foreach (XmlSchemaAttribute xmlSchemaAttribute3 in this.defaultAttributes)
				{
					this.ValidateKeyFieldsAttribute(xmlSchemaAttribute3, xmlSchemaAttribute3.ValidatedFixedValue ?? xmlSchemaAttribute3.ValidatedDefaultValue);
				}
			}
		}

		private object AssessAttributeElementLocallyValidType(string localName, string ns, XmlValueGetter getter, XmlSchemaInfo info)
		{
			XmlSchemaComplexType cType = this.Context.ActualType as XmlSchemaComplexType;
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(localName, ns);
			XmlSchemaObject xmlSchemaObject = XmlSchemaUtil.FindAttributeDeclaration(ns, this.schemas, cType, xmlQualifiedName);
			if (xmlSchemaObject == null)
			{
				this.HandleError("Attribute declaration was not found for " + xmlQualifiedName);
			}
			XmlSchemaAttribute xmlSchemaAttribute = xmlSchemaObject as XmlSchemaAttribute;
			if (xmlSchemaAttribute != null)
			{
				this.AssessAttributeLocallyValidUse(xmlSchemaAttribute);
				return this.AssessAttributeLocallyValid(xmlSchemaAttribute, info, getter);
			}
			return null;
		}

		private object AssessAttributeLocallyValid(XmlSchemaAttribute attr, XmlSchemaInfo info, XmlValueGetter getter)
		{
			if (attr.AttributeType == null)
			{
				this.HandleError("Attribute type is missing for " + attr.QualifiedName);
			}
			XmlSchemaDatatype xmlSchemaDatatype = attr.AttributeType as XmlSchemaDatatype;
			if (xmlSchemaDatatype == null)
			{
				xmlSchemaDatatype = ((XmlSchemaSimpleType)attr.AttributeType).Datatype;
			}
			object obj = null;
			if (xmlSchemaDatatype == XmlSchemaSimpleType.AnySimpleType)
			{
				if (attr.ValidatedFixedValue == null)
				{
					goto IL_115;
				}
			}
			try
			{
				this.CurrentAttributeType = xmlSchemaDatatype;
				obj = getter();
			}
			catch (Exception innerException)
			{
				this.HandleError(string.Format("Attribute value is invalid against its data type {0}", (xmlSchemaDatatype == null) ? XmlTokenizedType.CDATA : xmlSchemaDatatype.TokenizedType), innerException);
			}
			XmlSchemaSimpleType xmlSchemaSimpleType = attr.AttributeType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				this.ValidateRestrictedSimpleTypeValue(xmlSchemaSimpleType, ref xmlSchemaDatatype, new XmlAtomicValue(obj, attr.AttributeSchemaType).Value);
			}
			if (attr.ValidatedFixedValue != null)
			{
				if (!XmlSchemaUtil.AreSchemaDatatypeEqual(attr.AttributeSchemaType, attr.ValidatedFixedTypedValue, attr.AttributeSchemaType, obj))
				{
					this.HandleError(string.Format("The value of the attribute {0} does not match with its fixed value '{1}' in the space of type {2}", attr.QualifiedName, attr.ValidatedFixedValue, xmlSchemaDatatype));
				}
				obj = attr.ValidatedFixedTypedValue;
			}
			IL_115:
			if (!this.IgnoreIdentity)
			{
				string text = this.idManager.AssessEachAttributeIdentityConstraint(xmlSchemaDatatype, obj, ((XmlQualifiedName)this.elementQNameStack[this.elementQNameStack.Count - 1]).Name);
				if (text != null)
				{
					this.HandleError(text);
				}
			}
			if (!this.IgnoreIdentity)
			{
				this.ValidateKeyFieldsAttribute(attr, obj);
			}
			return obj;
		}

		private void AssessAttributeLocallyValidUse(XmlSchemaAttribute attr)
		{
			if (attr.ValidatedUse == XmlSchemaUse.Prohibited)
			{
				this.HandleError("Attribute " + attr.QualifiedName + " is prohibited in this context.");
			}
		}

		private object AssessEndElementSchemaValidity(XmlSchemaInfo info)
		{
			object result = this.ValidateEndSimpleContent(info);
			this.ValidateEndElementParticle();
			if (!this.IgnoreIdentity)
			{
				this.ValidateEndElementKeyConstraints();
			}
			if (this.xsiNilDepth == this.depth)
			{
				this.xsiNilDepth = -1;
			}
			return result;
		}

		private void ValidateEndElementParticle()
		{
			if (this.Context.State != null && !this.Context.EvaluateEndElement())
			{
				this.HandleError("Invalid end element. There are still required content items.");
			}
			this.Context.PopCurrentElement();
			this.state.PopContext();
			this.Context.XsiType = null;
		}

		private void ValidateCharacters(XmlValueGetter getter)
		{
			if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.depth)
			{
				this.HandleError("Element item appeared, while current element context is nil.");
			}
			if (this.shouldValidateCharacters)
			{
				this.CurrentAttributeType = null;
				this.storedCharacters.Append(getter());
			}
		}

		private object ValidateEndSimpleContent(XmlSchemaInfo info)
		{
			object result = null;
			if (this.shouldValidateCharacters)
			{
				result = this.ValidateEndSimpleContentCore(info);
			}
			this.shouldValidateCharacters = false;
			this.storedCharacters.Length = 0;
			return result;
		}

		private object ValidateEndSimpleContentCore(XmlSchemaInfo info)
		{
			if (this.Context.ActualType == null)
			{
				return null;
			}
			string text = this.storedCharacters.ToString();
			object result = null;
			if (text.Length == 0 && this.Context.Element != null && this.Context.Element.ValidatedDefaultValue != null)
			{
				text = this.Context.Element.ValidatedDefaultValue;
			}
			XmlSchemaDatatype xmlSchemaDatatype = this.Context.ActualType as XmlSchemaDatatype;
			XmlSchemaSimpleType xmlSchemaSimpleType = this.Context.ActualType as XmlSchemaSimpleType;
			if (xmlSchemaDatatype == null)
			{
				if (xmlSchemaSimpleType != null)
				{
					xmlSchemaDatatype = xmlSchemaSimpleType.Datatype;
				}
				else
				{
					XmlSchemaComplexType xmlSchemaComplexType = this.Context.ActualType as XmlSchemaComplexType;
					xmlSchemaDatatype = xmlSchemaComplexType.Datatype;
					XmlSchemaContentType contentType = xmlSchemaComplexType.ContentType;
					if (contentType != XmlSchemaContentType.Empty)
					{
						if (contentType == XmlSchemaContentType.ElementOnly)
						{
							if (text.Length > 0 && !XmlChar.IsWhitespace(text))
							{
								this.HandleError("Character content not allowed in an elementOnly model.");
							}
						}
					}
					else if (text.Length > 0)
					{
						this.HandleError("Character content not allowed in an empty model.");
					}
				}
			}
			if (xmlSchemaDatatype != null)
			{
				if (this.Context.Element != null && this.Context.Element.ValidatedFixedValue != null && text != this.Context.Element.ValidatedFixedValue)
				{
					this.HandleError("Fixed value constraint was not satisfied.");
				}
				result = this.AssessStringValid(xmlSchemaSimpleType, xmlSchemaDatatype, text);
			}
			if (!this.IgnoreIdentity)
			{
				this.ValidateSimpleContentIdentity(xmlSchemaDatatype, text);
			}
			this.shouldValidateCharacters = false;
			if (info != null)
			{
				info.IsNil = (this.xsiNilDepth >= 0);
				info.SchemaElement = null;
				info.SchemaType = (this.Context.ActualType as XmlSchemaType);
				if (info.SchemaType == null)
				{
					info.SchemaType = XmlSchemaType.GetBuiltInSimpleType(xmlSchemaDatatype.TypeCode);
				}
				info.SchemaAttribute = null;
				info.IsDefault = false;
				info.MemberType = null;
			}
			return result;
		}

		private object AssessStringValid(XmlSchemaSimpleType st, XmlSchemaDatatype dt, string value)
		{
			XmlSchemaDatatype xmlSchemaDatatype = dt;
			object result = null;
			if (st != null)
			{
				string text = xmlSchemaDatatype.Normalize(value);
				XmlSchemaDerivationMethod derivedBy = st.DerivedBy;
				if (derivedBy != XmlSchemaDerivationMethod.Restriction)
				{
					if (derivedBy != XmlSchemaDerivationMethod.List)
					{
						if (derivedBy == XmlSchemaDerivationMethod.Union)
						{
							XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = st.Content as XmlSchemaSimpleTypeUnion;
							string text2 = text;
							bool flag = false;
							object[] validatedTypes = xmlSchemaSimpleTypeUnion.ValidatedTypes;
							int i = 0;
							while (i < validatedTypes.Length)
							{
								object obj = validatedTypes[i];
								XmlSchemaDatatype xmlSchemaDatatype2 = obj as XmlSchemaDatatype;
								XmlSchemaSimpleType xmlSchemaSimpleType = obj as XmlSchemaSimpleType;
								if (xmlSchemaDatatype2 != null)
								{
									try
									{
										result = xmlSchemaDatatype2.ParseValue(text2, this.nameTable, this.nsResolver);
									}
									catch (Exception)
									{
										goto IL_1A2;
									}
									goto IL_19A;
								}
								try
								{
									result = this.AssessStringValid(xmlSchemaSimpleType, xmlSchemaSimpleType.Datatype, text2);
								}
								catch (XmlSchemaValidationException)
								{
									goto IL_1A2;
								}
								goto IL_19A;
								IL_1A2:
								i++;
								continue;
								IL_19A:
								flag = true;
								break;
							}
							if (!flag)
							{
								this.HandleError("Union type value contains one or more invalid values.");
							}
						}
					}
					else
					{
						XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = st.Content as XmlSchemaSimpleTypeList;
						string[] array = text.Split(XmlChar.WhitespaceChars);
						object[] array2 = new object[array.Length];
						XmlSchemaDatatype xmlSchemaDatatype2 = xmlSchemaSimpleTypeList.ValidatedListItemType as XmlSchemaDatatype;
						XmlSchemaSimpleType xmlSchemaSimpleType = xmlSchemaSimpleTypeList.ValidatedListItemType as XmlSchemaSimpleType;
						for (int j = 0; j < array.Length; j++)
						{
							string text3 = array[j];
							if (!(text3 == string.Empty))
							{
								if (xmlSchemaDatatype2 != null)
								{
									try
									{
										array2[j] = xmlSchemaDatatype2.ParseValue(text3, this.nameTable, this.nsResolver);
									}
									catch (Exception innerException)
									{
										this.HandleError("List type value contains one or more invalid values.", innerException);
										break;
									}
								}
								else
								{
									this.AssessStringValid(xmlSchemaSimpleType, xmlSchemaSimpleType.Datatype, text3);
								}
							}
						}
						result = array2;
					}
				}
				else
				{
					XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = st.Content as XmlSchemaSimpleTypeRestriction;
					if (xmlSchemaSimpleTypeRestriction != null)
					{
						XmlSchemaSimpleType xmlSchemaSimpleType2 = st.BaseXmlSchemaType as XmlSchemaSimpleType;
						if (xmlSchemaSimpleType2 != null)
						{
							result = this.AssessStringValid(xmlSchemaSimpleType2, dt, value);
						}
						if (!xmlSchemaSimpleTypeRestriction.ValidateValueWithFacets(value, this.nameTable, this.nsResolver))
						{
							this.HandleError("Specified value was invalid against the facets.");
							goto IL_237;
						}
					}
					xmlSchemaDatatype = st.Datatype;
				}
			}
			IL_237:
			if (xmlSchemaDatatype != null)
			{
				try
				{
					result = xmlSchemaDatatype.ParseValue(value, this.nameTable, this.nsResolver);
				}
				catch (Exception innerException2)
				{
					this.HandleError(string.Format("Invalidly typed data was specified.", new object[0]), innerException2);
				}
			}
			return result;
		}

		private void ValidateRestrictedSimpleTypeValue(XmlSchemaSimpleType st, ref XmlSchemaDatatype dt, string normalized)
		{
			XmlSchemaDerivationMethod derivedBy = st.DerivedBy;
			if (derivedBy != XmlSchemaDerivationMethod.Restriction)
			{
				if (derivedBy != XmlSchemaDerivationMethod.List)
				{
					if (derivedBy == XmlSchemaDerivationMethod.Union)
					{
						XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = st.Content as XmlSchemaSimpleTypeUnion;
						bool flag = false;
						object[] validatedTypes = xmlSchemaSimpleTypeUnion.ValidatedTypes;
						int i = 0;
						while (i < validatedTypes.Length)
						{
							object obj = validatedTypes[i];
							XmlSchemaDatatype xmlSchemaDatatype = obj as XmlSchemaDatatype;
							XmlSchemaSimpleType xmlSchemaSimpleType = obj as XmlSchemaSimpleType;
							if (xmlSchemaDatatype != null)
							{
								try
								{
									xmlSchemaDatatype.ParseValue(normalized, this.nameTable, this.nsResolver);
								}
								catch (Exception)
								{
									goto IL_170;
								}
								goto IL_168;
							}
							try
							{
								this.AssessStringValid(xmlSchemaSimpleType, xmlSchemaSimpleType.Datatype, normalized);
							}
							catch (XmlSchemaValidationException)
							{
								goto IL_170;
							}
							goto IL_168;
							IL_170:
							i++;
							continue;
							IL_168:
							flag = true;
							break;
						}
						if (!flag)
						{
							this.HandleError("Union type value contains one or more invalid values.");
						}
					}
				}
				else
				{
					XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = st.Content as XmlSchemaSimpleTypeList;
					string[] array = normalized.Split(XmlChar.WhitespaceChars);
					XmlSchemaDatatype xmlSchemaDatatype = xmlSchemaSimpleTypeList.ValidatedListItemType as XmlSchemaDatatype;
					XmlSchemaSimpleType xmlSchemaSimpleType = xmlSchemaSimpleTypeList.ValidatedListItemType as XmlSchemaSimpleType;
					foreach (string text in array)
					{
						if (!(text == string.Empty))
						{
							if (xmlSchemaDatatype != null)
							{
								try
								{
									xmlSchemaDatatype.ParseValue(text, this.nameTable, this.nsResolver);
								}
								catch (Exception innerException)
								{
									this.HandleError("List type value contains one or more invalid values.", innerException);
									break;
								}
							}
							else
							{
								this.AssessStringValid(xmlSchemaSimpleType, xmlSchemaSimpleType.Datatype, text);
							}
						}
					}
				}
			}
			else
			{
				XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = st.Content as XmlSchemaSimpleTypeRestriction;
				if (xmlSchemaSimpleTypeRestriction != null)
				{
					XmlSchemaSimpleType xmlSchemaSimpleType2 = st.BaseXmlSchemaType as XmlSchemaSimpleType;
					if (xmlSchemaSimpleType2 != null)
					{
						this.AssessStringValid(xmlSchemaSimpleType2, dt, normalized);
					}
					if (!xmlSchemaSimpleTypeRestriction.ValidateValueWithFacets(normalized, this.nameTable, this.nsResolver))
					{
						this.HandleError("Specified value was invalid against the facets.");
						return;
					}
				}
				dt = st.Datatype;
			}
		}

		private XsdKeyTable CreateNewKeyTable(XmlSchemaIdentityConstraint ident)
		{
			XsdKeyTable xsdKeyTable = new XsdKeyTable(ident);
			xsdKeyTable.StartDepth = this.depth;
			this.keyTables.Add(xsdKeyTable);
			return xsdKeyTable;
		}

		private void ValidateKeySelectors()
		{
			if (this.tmpKeyrefPool != null)
			{
				this.tmpKeyrefPool.Clear();
			}
			if (this.Context.Element != null && this.Context.Element.Constraints.Count > 0)
			{
				for (int i = 0; i < this.Context.Element.Constraints.Count; i++)
				{
					XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)this.Context.Element.Constraints[i];
					XsdKeyTable value = this.CreateNewKeyTable(xmlSchemaIdentityConstraint);
					if (xmlSchemaIdentityConstraint is XmlSchemaKeyref)
					{
						if (this.tmpKeyrefPool == null)
						{
							this.tmpKeyrefPool = new ArrayList();
						}
						this.tmpKeyrefPool.Add(value);
					}
				}
			}
			for (int j = 0; j < this.keyTables.Count; j++)
			{
				XsdKeyTable xsdKeyTable = (XsdKeyTable)this.keyTables[j];
				if (xsdKeyTable.SelectorMatches(this.elementQNameStack, this.depth) != null)
				{
					XsdKeyEntry entry = new XsdKeyEntry(xsdKeyTable, this.depth, this.lineInfo);
					xsdKeyTable.Entries.Add(entry);
				}
			}
		}

		private void ValidateKeyFieldsAttribute(XmlSchemaAttribute attr, object value)
		{
			this.ValidateKeyFields(true, false, attr.AttributeType, attr.QualifiedName.Name, attr.QualifiedName.Namespace, value);
		}

		private void ValidateKeyFields(bool isAttr, bool isNil, object schemaType, string attrName, string attrNs, object value)
		{
			for (int i = 0; i < this.keyTables.Count; i++)
			{
				XsdKeyTable xsdKeyTable = (XsdKeyTable)this.keyTables[i];
				for (int j = 0; j < xsdKeyTable.Entries.Count; j++)
				{
					this.CurrentAttributeType = null;
					try
					{
						xsdKeyTable.Entries[j].ProcessMatch(isAttr, this.elementQNameStack, this.nominalEventSender, this.nameTable, this.BaseUri, schemaType, this.nsResolver, this.lineInfo, (!isAttr) ? this.depth : (this.depth + 1), attrName, attrNs, value, isNil, this.currentKeyFieldConsumers);
					}
					catch (XmlSchemaValidationException exception)
					{
						this.HandleError(exception);
					}
				}
			}
		}

		private void ValidateEndElementKeyConstraints()
		{
			for (int i = 0; i < this.keyTables.Count; i++)
			{
				XsdKeyTable xsdKeyTable = this.keyTables[i] as XsdKeyTable;
				if (xsdKeyTable.StartDepth == this.depth)
				{
					this.ValidateEndKeyConstraint(xsdKeyTable);
				}
				else
				{
					for (int j = 0; j < xsdKeyTable.Entries.Count; j++)
					{
						XsdKeyEntry xsdKeyEntry = xsdKeyTable.Entries[j];
						if (xsdKeyEntry.StartDepth == this.depth)
						{
							if (xsdKeyEntry.KeyFound)
							{
								xsdKeyTable.FinishedEntries.Add(xsdKeyEntry);
							}
							else if (xsdKeyTable.SourceSchemaIdentity is XmlSchemaKey)
							{
								this.HandleError("Key sequence is missing.");
							}
							xsdKeyTable.Entries.RemoveAt(j);
							j--;
						}
						else
						{
							for (int k = 0; k < xsdKeyEntry.KeyFields.Count; k++)
							{
								XsdKeyEntryField xsdKeyEntryField = xsdKeyEntry.KeyFields[k];
								if (!xsdKeyEntryField.FieldFound && xsdKeyEntryField.FieldFoundDepth == this.depth)
								{
									xsdKeyEntryField.FieldFoundDepth = 0;
									xsdKeyEntryField.FieldFoundPath = null;
								}
							}
						}
					}
				}
			}
			for (int l = 0; l < this.keyTables.Count; l++)
			{
				XsdKeyTable xsdKeyTable2 = this.keyTables[l] as XsdKeyTable;
				if (xsdKeyTable2.StartDepth == this.depth)
				{
					this.keyTables.RemoveAt(l);
					l--;
				}
			}
		}

		private void ValidateEndKeyConstraint(XsdKeyTable seq)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < seq.Entries.Count; i++)
			{
				XsdKeyEntry xsdKeyEntry = seq.Entries[i];
				if (!xsdKeyEntry.KeyFound)
				{
					if (seq.SourceSchemaIdentity is XmlSchemaKey)
					{
						arrayList.Add(string.Concat(new object[]
						{
							"line ",
							xsdKeyEntry.SelectorLineNumber,
							"position ",
							xsdKeyEntry.SelectorLinePosition
						}));
					}
				}
			}
			if (arrayList.Count > 0)
			{
				this.HandleError("Invalid identity constraints were found. Key was not found. " + string.Join(", ", arrayList.ToArray(typeof(string)) as string[]));
			}
			arrayList.Clear();
			XmlSchemaKeyref xmlSchemaKeyref = seq.SourceSchemaIdentity as XmlSchemaKeyref;
			if (xmlSchemaKeyref != null)
			{
				for (int j = this.keyTables.Count - 1; j >= 0; j--)
				{
					XsdKeyTable xsdKeyTable = this.keyTables[j] as XsdKeyTable;
					if (xsdKeyTable.SourceSchemaIdentity == xmlSchemaKeyref.Target)
					{
						seq.ReferencedKey = xsdKeyTable;
						for (int k = 0; k < seq.FinishedEntries.Count; k++)
						{
							XsdKeyEntry xsdKeyEntry2 = seq.FinishedEntries[k];
							for (int l = 0; l < xsdKeyTable.FinishedEntries.Count; l++)
							{
								XsdKeyEntry other = xsdKeyTable.FinishedEntries[l];
								if (xsdKeyEntry2.CompareIdentity(other))
								{
									xsdKeyEntry2.KeyRefFound = true;
									break;
								}
							}
						}
					}
				}
				if (seq.ReferencedKey == null)
				{
					this.HandleError("Target key was not found.");
				}
				for (int m = 0; m < seq.FinishedEntries.Count; m++)
				{
					XsdKeyEntry xsdKeyEntry3 = seq.FinishedEntries[m];
					if (!xsdKeyEntry3.KeyRefFound)
					{
						arrayList.Add(string.Concat(new object[]
						{
							" line ",
							xsdKeyEntry3.SelectorLineNumber,
							", position ",
							xsdKeyEntry3.SelectorLinePosition
						}));
					}
				}
				if (arrayList.Count > 0)
				{
					this.HandleError("Invalid identity constraints were found. Referenced key was not found: " + string.Join(" / ", arrayList.ToArray(typeof(string)) as string[]));
				}
			}
		}

		private void ValidateSimpleContentIdentity(XmlSchemaDatatype dt, string value)
		{
			if (this.currentKeyFieldConsumers != null)
			{
				while (this.currentKeyFieldConsumers.Count > 0)
				{
					XsdKeyEntryField xsdKeyEntryField = this.currentKeyFieldConsumers[0] as XsdKeyEntryField;
					if (xsdKeyEntryField.Identity != null)
					{
						this.HandleError("Two or more identical field was found. Former value is '" + xsdKeyEntryField.Identity + "' .");
					}
					object obj = null;
					if (dt != null)
					{
						try
						{
							obj = dt.ParseValue(value, this.nameTable, this.nsResolver);
						}
						catch (Exception innerException)
						{
							this.HandleError("Identity value is invalid against its data type " + dt.TokenizedType, innerException);
						}
					}
					if (obj == null)
					{
						obj = value;
					}
					if (!xsdKeyEntryField.SetIdentityField(obj, this.depth == this.xsiNilDepth, dt as XsdAnySimpleType, this.depth, this.lineInfo))
					{
						this.HandleError("Two or more identical key value was found: '" + value + "' .");
					}
					this.currentKeyFieldConsumers.RemoveAt(0);
				}
			}
		}

		private object GetXsiType(string name)
		{
			XmlQualifiedName xmlQualifiedName = XmlQualifiedName.Parse(name, this.nsResolver, true);
			object result;
			if (xmlQualifiedName == XmlSchemaComplexType.AnyTypeName)
			{
				result = XmlSchemaComplexType.AnyType;
			}
			else if (XmlSchemaUtil.IsBuiltInDatatypeName(xmlQualifiedName))
			{
				result = XmlSchemaDatatype.FromName(xmlQualifiedName);
			}
			else
			{
				result = this.FindType(xmlQualifiedName);
			}
			return result;
		}

		private void HandleXsiType(string typename)
		{
			XmlSchemaElement element = this.Context.Element;
			object xsiType = this.GetXsiType(typename);
			if (xsiType == null)
			{
				this.HandleError("The instance type was not found: " + typename);
				return;
			}
			XmlSchemaType xmlSchemaType = xsiType as XmlSchemaType;
			if (xmlSchemaType != null && this.Context.Element != null)
			{
				XmlSchemaType xmlSchemaType2 = element.ElementType as XmlSchemaType;
				if (xmlSchemaType2 != null && (xmlSchemaType.DerivedBy & xmlSchemaType2.FinalResolved) != XmlSchemaDerivationMethod.Empty)
				{
					this.HandleError("The instance type is prohibited by the type of the context element.");
				}
				if (xmlSchemaType2 != xsiType && (xmlSchemaType.DerivedBy & element.BlockResolved) != XmlSchemaDerivationMethod.Empty)
				{
					this.HandleError("The instance type is prohibited by the context element.");
				}
			}
			XmlSchemaComplexType xmlSchemaComplexType = xsiType as XmlSchemaComplexType;
			if (xmlSchemaComplexType != null && xmlSchemaComplexType.IsAbstract)
			{
				this.HandleError("The instance type is abstract: " + typename);
			}
			else
			{
				if (element != null)
				{
					this.AssessLocalTypeDerivationOK(xsiType, element.ElementType, element.BlockResolved);
				}
				this.Context.XsiType = xsiType;
			}
		}

		private void AssessLocalTypeDerivationOK(object xsiType, object baseType, XmlSchemaDerivationMethod flag)
		{
			XmlSchemaType xmlSchemaType = xsiType as XmlSchemaType;
			XmlSchemaComplexType xmlSchemaComplexType = baseType as XmlSchemaComplexType;
			XmlSchemaComplexType xmlSchemaComplexType2 = xmlSchemaType as XmlSchemaComplexType;
			if (xsiType != baseType)
			{
				if (xmlSchemaComplexType != null)
				{
					flag |= xmlSchemaComplexType.BlockResolved;
				}
				if (flag == XmlSchemaDerivationMethod.All)
				{
					this.HandleError("Prohibited element type substitution.");
					return;
				}
				if (xmlSchemaType != null && (flag & xmlSchemaType.DerivedBy) != XmlSchemaDerivationMethod.Empty)
				{
					this.HandleError("Prohibited element type substitution.");
					return;
				}
			}
			if (xmlSchemaComplexType2 != null)
			{
				try
				{
					xmlSchemaComplexType2.ValidateTypeDerivationOK(baseType, null, null);
				}
				catch (XmlSchemaValidationException exception)
				{
					this.HandleError(exception);
				}
			}
			else
			{
				XmlSchemaSimpleType xmlSchemaSimpleType = xsiType as XmlSchemaSimpleType;
				if (xmlSchemaSimpleType != null)
				{
					try
					{
						xmlSchemaSimpleType.ValidateTypeDerivationOK(baseType, null, null, true);
					}
					catch (XmlSchemaValidationException exception2)
					{
						this.HandleError(exception2);
					}
				}
				else if (!(xsiType is XmlSchemaDatatype))
				{
					this.HandleError("Primitive data type cannot be derived type using xsi:type specification.");
				}
			}
		}

		private void HandleXsiNil(string value, XmlSchemaInfo info)
		{
			XmlSchemaElement element = this.Context.Element;
			if (!element.ActualIsNillable)
			{
				this.HandleError(string.Format("Current element '{0}' is not nillable and thus does not allow occurence of 'nil' attribute.", this.Context.Element.QualifiedName));
				return;
			}
			value = value.Trim(XmlChar.WhitespaceChars);
			if (value == "true")
			{
				if (element.ValidatedFixedValue != null)
				{
					this.HandleError("Schema instance nil was specified, where the element declaration for " + element.QualifiedName + "has fixed value constraints.");
				}
				this.xsiNilDepth = this.depth;
				if (info != null)
				{
					info.IsNil = true;
				}
			}
		}

		private XmlSchema ReadExternalSchema(string uri)
		{
			Uri uri2 = new Uri(this.SourceUri, uri.Trim(XmlChar.WhitespaceChars));
			XmlTextReader xmlTextReader = null;
			XmlSchema result;
			try
			{
				xmlTextReader = new XmlTextReader(uri2.ToString(), (Stream)this.xmlResolver.GetEntity(uri2, null, typeof(Stream)), this.nameTable);
				result = XmlSchema.Read(xmlTextReader, this.ValidationEventHandler);
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

		private void HandleSchemaLocation(string schemaLocation)
		{
			if (this.xmlResolver == null)
			{
				return;
			}
			XmlSchema xmlSchema = null;
			bool flag = false;
			string[] array = null;
			try
			{
				schemaLocation = (XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Token).Datatype.ParseValue(schemaLocation, null, null) as string);
				array = schemaLocation.Split(XmlChar.WhitespaceChars);
			}
			catch (Exception innerException)
			{
				this.HandleError("Invalid schemaLocation attribute format.", innerException, true);
				array = new string[0];
			}
			if (array.Length % 2 != 0)
			{
				this.HandleError("Invalid schemaLocation attribute format.");
			}
			int i = 0;
			while (i < array.Length)
			{
				try
				{
					xmlSchema = this.ReadExternalSchema(array[i + 1]);
				}
				catch (Exception innerException2)
				{
					this.HandleError("Could not resolve schema location URI: " + schemaLocation, innerException2, true);
					goto IL_10B;
				}
				goto IL_A7;
				IL_10B:
				i += 2;
				continue;
				IL_A7:
				if (xmlSchema.TargetNamespace == null)
				{
					xmlSchema.TargetNamespace = array[i];
				}
				else if (xmlSchema.TargetNamespace != array[i])
				{
					this.HandleError("Specified schema has different target namespace.");
				}
				if (xmlSchema != null && !this.schemas.Contains(xmlSchema.TargetNamespace))
				{
					flag = true;
					this.schemas.Add(xmlSchema);
					goto IL_10B;
				}
				goto IL_10B;
			}
			if (flag)
			{
				this.schemas.Compile();
			}
		}

		private void HandleNoNSSchemaLocation(string noNsSchemaLocation)
		{
			if (this.xmlResolver == null)
			{
				return;
			}
			XmlSchema xmlSchema = null;
			bool flag = false;
			try
			{
				xmlSchema = this.ReadExternalSchema(noNsSchemaLocation);
			}
			catch (Exception innerException)
			{
				this.HandleError("Could not resolve schema location URI: " + noNsSchemaLocation, innerException, true);
			}
			if (xmlSchema != null && xmlSchema.TargetNamespace != null)
			{
				this.HandleError("Specified schema has different target namespace.");
			}
			if (xmlSchema != null && !this.schemas.Contains(xmlSchema.TargetNamespace))
			{
				flag = true;
				this.schemas.Add(xmlSchema);
			}
			if (flag)
			{
				this.schemas.Compile();
			}
		}

		private enum Transition
		{
			None,
			Content,
			StartTag,
			Finished
		}
	}
}
