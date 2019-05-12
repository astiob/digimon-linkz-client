using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdValidatingReader : XmlReader, IHasXmlParserContext, IHasXmlSchemaInfo, IXmlLineInfo
	{
		private static readonly XmlSchemaAttribute[] emptyAttributeArray = new XmlSchemaAttribute[0];

		private XmlReader reader;

		private XmlResolver resolver;

		private IHasXmlSchemaInfo sourceReaderSchemaInfo;

		private IXmlLineInfo readerLineInfo;

		private ValidationType validationType;

		private XmlSchemaSet schemas = new XmlSchemaSet();

		private bool namespaces = true;

		private bool validationStarted;

		private bool checkIdentity = true;

		private XsdIDManager idManager = new XsdIDManager();

		private bool checkKeyConstraints = true;

		private ArrayList keyTables = new ArrayList();

		private ArrayList currentKeyFieldConsumers;

		private ArrayList tmpKeyrefPool;

		private ArrayList elementQNameStack = new ArrayList();

		private XsdParticleStateManager state = new XsdParticleStateManager();

		private int skipValidationDepth = -1;

		private int xsiNilDepth = -1;

		private StringBuilder storedCharacters = new StringBuilder();

		private bool shouldValidateCharacters;

		private XmlSchemaAttribute[] defaultAttributes = XsdValidatingReader.emptyAttributeArray;

		private int currentDefaultAttribute = -1;

		private ArrayList defaultAttributesCache = new ArrayList();

		private bool defaultAttributeConsumed;

		private object currentAttrType;

		public ValidationEventHandler ValidationEventHandler;

		public XsdValidatingReader(XmlReader reader)
		{
			this.reader = reader;
			this.readerLineInfo = (reader as IXmlLineInfo);
			this.sourceReaderSchemaInfo = (reader as IHasXmlSchemaInfo);
			this.schemas.ValidationEventHandler += this.ValidationEventHandler;
		}

		private XsdValidationContext Context
		{
			get
			{
				return this.state.Context;
			}
		}

		internal ArrayList CurrentKeyFieldConsumers
		{
			get
			{
				if (this.currentKeyFieldConsumers == null)
				{
					this.currentKeyFieldConsumers = new ArrayList();
				}
				return this.currentKeyFieldConsumers;
			}
		}

		public int XsiNilDepth
		{
			get
			{
				return this.xsiNilDepth;
			}
		}

		public bool Namespaces
		{
			get
			{
				return this.namespaces;
			}
			set
			{
				this.namespaces = value;
			}
		}

		public XmlResolver XmlResolver
		{
			set
			{
				this.resolver = value;
			}
		}

		public XmlSchemaSet Schemas
		{
			get
			{
				return this.schemas;
			}
			set
			{
				if (this.validationStarted)
				{
					throw new InvalidOperationException("Schemas must be set before the first call to Read().");
				}
				this.schemas = value;
			}
		}

		public object SchemaType
		{
			get
			{
				if (this.ReadState != ReadState.Interactive)
				{
					return null;
				}
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						return this.SourceReaderSchemaType;
					}
					if (this.currentAttrType == null)
					{
						XmlSchemaComplexType xmlSchemaComplexType = this.Context.ActualType as XmlSchemaComplexType;
						if (xmlSchemaComplexType != null)
						{
							XmlSchemaAttribute xmlSchemaAttribute = xmlSchemaComplexType.AttributeUses[new XmlQualifiedName(this.LocalName, this.NamespaceURI)] as XmlSchemaAttribute;
							if (xmlSchemaAttribute != null)
							{
								this.currentAttrType = xmlSchemaAttribute.AttributeType;
							}
							return this.currentAttrType;
						}
						this.currentAttrType = this.SourceReaderSchemaType;
					}
					return this.currentAttrType;
				}
				else
				{
					if (this.Context.ActualType != null)
					{
						return this.Context.ActualType;
					}
					return this.SourceReaderSchemaType;
				}
			}
		}

		private object SourceReaderSchemaType
		{
			get
			{
				return (this.sourceReaderSchemaInfo == null) ? null : this.sourceReaderSchemaInfo.SchemaType;
			}
		}

		public ValidationType ValidationType
		{
			get
			{
				return this.validationType;
			}
			set
			{
				if (this.validationStarted)
				{
					throw new InvalidOperationException("ValidationType must be set before reading.");
				}
				this.validationType = value;
			}
		}

		public object ReadTypedValue()
		{
			object result = XmlSchemaUtil.ReadTypedValue(this, this.SchemaType, this.NamespaceManager, this.storedCharacters);
			this.storedCharacters.Length = 0;
			return result;
		}

		public override int AttributeCount
		{
			get
			{
				return this.reader.AttributeCount + this.defaultAttributes.Length;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		public override bool CanResolveEntity
		{
			get
			{
				return this.reader.CanResolveEntity;
			}
		}

		public override int Depth
		{
			get
			{
				if (this.currentDefaultAttribute < 0)
				{
					return this.reader.Depth;
				}
				if (this.defaultAttributeConsumed)
				{
					return this.reader.Depth + 2;
				}
				return this.reader.Depth + 1;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.reader.EOF;
			}
		}

		public override bool HasValue
		{
			get
			{
				return this.currentDefaultAttribute >= 0 || this.reader.HasValue;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.currentDefaultAttribute >= 0 || this.reader.IsDefault;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.currentDefaultAttribute < 0 && this.reader.IsEmptyElement;
			}
		}

		public override string this[int i]
		{
			get
			{
				return this.GetAttribute(i);
			}
		}

		public override string this[string name]
		{
			get
			{
				return this.GetAttribute(name);
			}
		}

		public override string this[string localName, string ns]
		{
			get
			{
				return this.GetAttribute(localName, ns);
			}
		}

		public int LineNumber
		{
			get
			{
				return (this.readerLineInfo == null) ? 0 : this.readerLineInfo.LineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return (this.readerLineInfo == null) ? 0 : this.readerLineInfo.LinePosition;
			}
		}

		public override string LocalName
		{
			get
			{
				if (this.currentDefaultAttribute < 0)
				{
					return this.reader.LocalName;
				}
				if (this.defaultAttributeConsumed)
				{
					return string.Empty;
				}
				return this.defaultAttributes[this.currentDefaultAttribute].QualifiedName.Name;
			}
		}

		public override string Name
		{
			get
			{
				if (this.currentDefaultAttribute < 0)
				{
					return this.reader.Name;
				}
				if (this.defaultAttributeConsumed)
				{
					return string.Empty;
				}
				XmlQualifiedName qualifiedName = this.defaultAttributes[this.currentDefaultAttribute].QualifiedName;
				string prefix = this.Prefix;
				if (prefix == string.Empty)
				{
					return qualifiedName.Name;
				}
				return prefix + ":" + qualifiedName.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				if (this.currentDefaultAttribute < 0)
				{
					return this.reader.NamespaceURI;
				}
				if (this.defaultAttributeConsumed)
				{
					return string.Empty;
				}
				return this.defaultAttributes[this.currentDefaultAttribute].QualifiedName.Namespace;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				if (this.currentDefaultAttribute < 0)
				{
					return this.reader.NodeType;
				}
				if (this.defaultAttributeConsumed)
				{
					return XmlNodeType.Text;
				}
				return XmlNodeType.Attribute;
			}
		}

		public XmlParserContext ParserContext
		{
			get
			{
				return XmlSchemaUtil.GetParserContext(this.reader);
			}
		}

		internal XmlNamespaceManager NamespaceManager
		{
			get
			{
				return (this.ParserContext == null) ? null : this.ParserContext.NamespaceManager;
			}
		}

		public override string Prefix
		{
			get
			{
				if (this.currentDefaultAttribute < 0)
				{
					return this.reader.Prefix;
				}
				if (this.defaultAttributeConsumed)
				{
					return string.Empty;
				}
				XmlQualifiedName qualifiedName = this.defaultAttributes[this.currentDefaultAttribute].QualifiedName;
				string text = (this.NamespaceManager == null) ? null : this.NamespaceManager.LookupPrefix(qualifiedName.Namespace, false);
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this.reader.QuoteChar;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return this.reader.ReadState;
			}
		}

		public override string Value
		{
			get
			{
				if (this.currentDefaultAttribute < 0)
				{
					return this.reader.Value;
				}
				string text = this.defaultAttributes[this.currentDefaultAttribute].ValidatedDefaultValue;
				if (text == null)
				{
					text = this.defaultAttributes[this.currentDefaultAttribute].ValidatedFixedValue;
				}
				return text;
			}
		}

		public override string XmlLang
		{
			get
			{
				string text = this.reader.XmlLang;
				if (text != null)
				{
					return text;
				}
				int num = this.FindDefaultAttribute("lang", "http://www.w3.org/XML/1998/namespace");
				if (num < 0)
				{
					return null;
				}
				text = this.defaultAttributes[num].ValidatedDefaultValue;
				if (text == null)
				{
					text = this.defaultAttributes[num].ValidatedFixedValue;
				}
				return text;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				XmlSpace xmlSpace = this.reader.XmlSpace;
				if (xmlSpace != XmlSpace.None)
				{
					return xmlSpace;
				}
				int num = this.FindDefaultAttribute("space", "http://www.w3.org/XML/1998/namespace");
				if (num < 0)
				{
					return XmlSpace.None;
				}
				string text = this.defaultAttributes[num].ValidatedDefaultValue;
				if (text == null)
				{
					text = this.defaultAttributes[num].ValidatedFixedValue;
				}
				return (XmlSpace)((int)Enum.Parse(typeof(XmlSpace), text, false));
			}
		}

		private void HandleError(string error)
		{
			this.HandleError(error, null);
		}

		private void HandleError(string error, Exception innerException)
		{
			this.HandleError(error, innerException, false);
		}

		private void HandleError(string error, Exception innerException, bool isWarning)
		{
			if (this.ValidationType == ValidationType.None)
			{
				return;
			}
			XmlSchemaValidationException schemaException = new XmlSchemaValidationException(error, this, this.BaseURI, null, innerException);
			this.HandleError(schemaException, isWarning);
		}

		private void HandleError(XmlSchemaValidationException schemaException)
		{
			this.HandleError(schemaException, false);
		}

		private void HandleError(XmlSchemaValidationException schemaException, bool isWarning)
		{
			if (this.ValidationType == ValidationType.None)
			{
				return;
			}
			ValidationEventArgs validationEventArgs = new ValidationEventArgs(schemaException, schemaException.Message, (!isWarning) ? XmlSeverityType.Error : XmlSeverityType.Warning);
			if (this.ValidationEventHandler != null)
			{
				this.ValidationEventHandler(this, validationEventArgs);
			}
			else if (validationEventArgs.Severity == XmlSeverityType.Error)
			{
				throw validationEventArgs.Exception;
			}
		}

		private XmlSchemaElement FindElement(string name, string ns)
		{
			return (XmlSchemaElement)this.schemas.GlobalElements[new XmlQualifiedName(name, ns)];
		}

		private XmlSchemaType FindType(XmlQualifiedName qname)
		{
			return (XmlSchemaType)this.schemas.GlobalTypes[qname];
		}

		private void ValidateStartElementParticle()
		{
			if (this.Context.State == null)
			{
				return;
			}
			this.Context.XsiType = null;
			this.state.CurrentElement = null;
			this.Context.EvaluateStartElement(this.reader.LocalName, this.reader.NamespaceURI);
			if (this.Context.IsInvalid)
			{
				this.HandleError("Invalid start element: " + this.reader.NamespaceURI + ":" + this.reader.LocalName);
			}
			this.Context.PushCurrentElement(this.state.CurrentElement);
		}

		private void ValidateEndElementParticle()
		{
			if (this.Context.State != null && !this.Context.EvaluateEndElement())
			{
				this.HandleError("Invalid end element: " + this.reader.Name);
			}
			this.Context.PopCurrentElement();
			this.state.PopContext();
		}

		private void ValidateCharacters()
		{
			if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.reader.Depth)
			{
				this.HandleError("Element item appeared, while current element context is nil.");
			}
			if (this.shouldValidateCharacters)
			{
				this.storedCharacters.Append(this.reader.Value);
			}
		}

		private void ValidateEndSimpleContent()
		{
			if (this.shouldValidateCharacters)
			{
				this.ValidateEndSimpleContentCore();
			}
			this.shouldValidateCharacters = false;
			this.storedCharacters.Length = 0;
		}

		private void ValidateEndSimpleContentCore()
		{
			if (this.Context.ActualType == null)
			{
				return;
			}
			string text = this.storedCharacters.ToString();
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
								this.HandleError("Character content not allowed.");
							}
						}
					}
					else if (text.Length > 0)
					{
						this.HandleError("Character content not allowed.");
					}
				}
			}
			if (xmlSchemaDatatype != null)
			{
				if (this.Context.Element != null && this.Context.Element.ValidatedFixedValue != null && text != this.Context.Element.ValidatedFixedValue)
				{
					this.HandleError("Fixed value constraint was not satisfied.");
				}
				this.AssessStringValid(xmlSchemaSimpleType, xmlSchemaDatatype, text);
			}
			if (this.checkKeyConstraints)
			{
				this.ValidateSimpleContentIdentity(xmlSchemaDatatype, text);
			}
			this.shouldValidateCharacters = false;
		}

		private void AssessStringValid(XmlSchemaSimpleType st, XmlSchemaDatatype dt, string value)
		{
			XmlSchemaDatatype xmlSchemaDatatype = dt;
			if (st != null)
			{
				string normalized = xmlSchemaDatatype.Normalize(value);
				this.ValidateRestrictedSimpleTypeValue(st, ref xmlSchemaDatatype, normalized);
			}
			if (xmlSchemaDatatype != null)
			{
				try
				{
					xmlSchemaDatatype.ParseValue(value, this.NameTable, this.NamespaceManager);
				}
				catch (Exception innerException)
				{
					this.HandleError("Invalidly typed data was specified.", innerException);
				}
			}
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
									xmlSchemaDatatype.ParseValue(normalized, this.NameTable, this.NamespaceManager);
								}
								catch (Exception)
								{
									goto IL_16E;
								}
								goto IL_166;
							}
							try
							{
								this.AssessStringValid(xmlSchemaSimpleType, xmlSchemaSimpleType.Datatype, normalized);
							}
							catch (XmlSchemaValidationException)
							{
								goto IL_16E;
							}
							goto IL_166;
							IL_16E:
							i++;
							continue;
							IL_166:
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
									xmlSchemaDatatype.ParseValue(text, this.NameTable, this.NamespaceManager);
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
					if (!xmlSchemaSimpleTypeRestriction.ValidateValueWithFacets(normalized, this.NameTable, this.NamespaceManager))
					{
						this.HandleError("Specified value was invalid against the facets.");
						return;
					}
				}
				dt = st.Datatype;
			}
		}

		private object GetXsiType(string name)
		{
			XmlQualifiedName xmlQualifiedName = XmlQualifiedName.Parse(name, this);
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
				catch (XmlSchemaValidationException schemaException)
				{
					this.HandleError(schemaException);
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
					catch (XmlSchemaValidationException schemaException2)
					{
						this.HandleError(schemaException2);
					}
				}
				else if (!(xsiType is XmlSchemaDatatype))
				{
					this.HandleError("Primitive data type cannot be derived type using xsi:type specification.");
				}
			}
		}

		private void AssessStartElementSchemaValidity()
		{
			if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.reader.Depth)
			{
				this.HandleError("Element item appeared, while current element context is nil.");
			}
			this.ValidateStartElementParticle();
			string text = this.reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (text != null)
			{
				text = text.Trim(XmlChar.WhitespaceChars);
			}
			bool flag = text == "true";
			if (flag && this.xsiNilDepth < 0)
			{
				this.xsiNilDepth = this.reader.Depth;
			}
			string text2 = this.reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
			if (text2 != null)
			{
				text2 = text2.Trim(XmlChar.WhitespaceChars);
				object xsiType = this.GetXsiType(text2);
				if (xsiType == null)
				{
					this.HandleError("The instance type was not found: " + text2 + " .");
				}
				else
				{
					XmlSchemaType xmlSchemaType = xsiType as XmlSchemaType;
					if (xmlSchemaType != null && this.Context.Element != null)
					{
						XmlSchemaType xmlSchemaType2 = this.Context.Element.ElementType as XmlSchemaType;
						if (xmlSchemaType2 != null && (xmlSchemaType.DerivedBy & xmlSchemaType2.FinalResolved) != XmlSchemaDerivationMethod.Empty)
						{
							this.HandleError("The instance type is prohibited by the type of the context element.");
						}
						if (xmlSchemaType2 != xsiType && (xmlSchemaType.DerivedBy & this.Context.Element.BlockResolved) != XmlSchemaDerivationMethod.Empty)
						{
							this.HandleError("The instance type is prohibited by the context element.");
						}
					}
					XmlSchemaComplexType xmlSchemaComplexType = xsiType as XmlSchemaComplexType;
					if (xmlSchemaComplexType != null && xmlSchemaComplexType.IsAbstract)
					{
						this.HandleError("The instance type is abstract: " + text2 + " .");
					}
					else
					{
						if (this.Context.Element != null)
						{
							this.AssessLocalTypeDerivationOK(xsiType, this.Context.Element.ElementType, this.Context.Element.BlockResolved);
						}
						this.AssessStartElementLocallyValidType(xsiType);
						this.Context.XsiType = xsiType;
					}
				}
			}
			if (this.Context.Element == null)
			{
				this.state.CurrentElement = this.FindElement(this.reader.LocalName, this.reader.NamespaceURI);
				this.Context.PushCurrentElement(this.state.CurrentElement);
			}
			if (this.Context.Element != null)
			{
				if (this.Context.XsiType == null)
				{
					this.AssessElementLocallyValidElement(text);
				}
			}
			else
			{
				XmlSchemaContentProcessing processContents = this.state.ProcessContents;
				if (processContents != XmlSchemaContentProcessing.Skip)
				{
					if (processContents != XmlSchemaContentProcessing.Lax)
					{
						if (text2 == null && (this.schemas.Contains(this.reader.NamespaceURI) || !this.schemas.MissedSubComponents(this.reader.NamespaceURI)))
						{
							this.HandleError("Element declaration for " + new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI) + " is missing.");
						}
					}
				}
			}
			this.state.PushContext();
			XsdValidationState xsdValidationState = null;
			if (this.state.ProcessContents == XmlSchemaContentProcessing.Skip)
			{
				this.skipValidationDepth = this.reader.Depth;
			}
			else
			{
				XmlSchemaComplexType xmlSchemaComplexType2 = this.SchemaType as XmlSchemaComplexType;
				if (xmlSchemaComplexType2 != null)
				{
					xsdValidationState = this.state.Create(xmlSchemaComplexType2.ValidatableParticle);
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
			if (this.checkKeyConstraints)
			{
				this.ValidateKeySelectors();
				this.ValidateKeyFields();
			}
		}

		private void AssessElementLocallyValidElement(string xsiNilValue)
		{
			XmlSchemaElement element = this.Context.Element;
			XmlQualifiedName arg = new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI);
			if (element == null)
			{
				this.HandleError("Element declaration is required for " + arg);
			}
			if (element.ActualIsAbstract)
			{
				this.HandleError("Abstract element declaration was specified for " + arg);
			}
			if (!element.ActualIsNillable && xsiNilValue != null)
			{
				this.HandleError("This element declaration is not nillable: " + arg);
			}
			else if (xsiNilValue == "true" && element.ValidatedFixedValue != null)
			{
				this.HandleError("Schema instance nil was specified, where the element declaration for " + arg + "has fixed value constraints.");
			}
			string attribute = this.reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null)
			{
				this.Context.XsiType = this.GetXsiType(attribute);
				this.AssessLocalTypeDerivationOK(this.Context.XsiType, element.ElementType, element.BlockResolved);
			}
			else
			{
				this.Context.XsiType = null;
			}
			if (element.ElementType != null)
			{
				this.AssessStartElementLocallyValidType(this.SchemaType);
			}
		}

		private void AssessStartElementLocallyValidType(object schemaType)
		{
			if (schemaType == null)
			{
				this.HandleError("Schema type does not exist.");
				return;
			}
			XmlSchemaComplexType xmlSchemaComplexType = schemaType as XmlSchemaComplexType;
			XmlSchemaSimpleType xmlSchemaSimpleType = schemaType as XmlSchemaSimpleType;
			if (xmlSchemaSimpleType != null)
			{
				while (this.reader.MoveToNextAttribute())
				{
					if (!(this.reader.NamespaceURI == "http://www.w3.org/2000/xmlns/"))
					{
						if (this.reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema-instance")
						{
							this.HandleError("Current simple type cannot accept attributes other than schema instance namespace.");
						}
						string localName = this.reader.LocalName;
						if (localName != null)
						{
							if (XsdValidatingReader.<>f__switch$map3 == null)
							{
								XsdValidatingReader.<>f__switch$map3 = new Dictionary<string, int>(4)
								{
									{
										"type",
										0
									},
									{
										"nil",
										0
									},
									{
										"schemaLocation",
										0
									},
									{
										"noNamespaceSchemaLocation",
										0
									}
								};
							}
							int num;
							if (XsdValidatingReader.<>f__switch$map3.TryGetValue(localName, out num))
							{
								if (num == 0)
								{
									continue;
								}
							}
						}
						this.HandleError("Unknown schema instance namespace attribute: " + this.reader.LocalName);
					}
				}
				this.reader.MoveToElement();
			}
			else if (xmlSchemaComplexType != null)
			{
				if (xmlSchemaComplexType.IsAbstract)
				{
					this.HandleError("Target complex type is abstract.");
					return;
				}
				this.AssessElementLocallyValidComplexType(xmlSchemaComplexType);
			}
		}

		private void AssessElementLocallyValidComplexType(XmlSchemaComplexType cType)
		{
			if (cType.IsAbstract)
			{
				this.HandleError("Target complex type is abstract.");
			}
			if (this.reader.MoveToFirstAttribute())
			{
				for (;;)
				{
					string namespaceURI = this.reader.NamespaceURI;
					if (namespaceURI == null)
					{
						goto IL_91;
					}
					if (XsdValidatingReader.<>f__switch$map4 == null)
					{
						XsdValidatingReader.<>f__switch$map4 = new Dictionary<string, int>(2)
						{
							{
								"http://www.w3.org/2000/xmlns/",
								0
							},
							{
								"http://www.w3.org/2001/XMLSchema-instance",
								0
							}
						};
					}
					int num;
					if (!XsdValidatingReader.<>f__switch$map4.TryGetValue(namespaceURI, out num))
					{
						goto IL_91;
					}
					if (num != 0)
					{
						goto IL_91;
					}
					IL_F8:
					if (!this.reader.MoveToNextAttribute())
					{
						break;
					}
					continue;
					IL_91:
					XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI);
					XmlSchemaObject xmlSchemaObject = XmlSchemaUtil.FindAttributeDeclaration(this.reader.NamespaceURI, this.schemas, cType, xmlQualifiedName);
					if (xmlSchemaObject == null)
					{
						this.HandleError("Attribute declaration was not found for " + xmlQualifiedName);
					}
					XmlSchemaAttribute xmlSchemaAttribute = xmlSchemaObject as XmlSchemaAttribute;
					if (xmlSchemaAttribute != null)
					{
						this.AssessAttributeLocallyValidUse(xmlSchemaAttribute);
						this.AssessAttributeLocallyValid(xmlSchemaAttribute);
						goto IL_F8;
					}
					goto IL_F8;
				}
				this.reader.MoveToElement();
			}
			foreach (object obj in cType.AttributeUses)
			{
				XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute)((DictionaryEntry)obj).Value;
				if (this.reader[xmlSchemaAttribute2.QualifiedName.Name, xmlSchemaAttribute2.QualifiedName.Namespace] == null)
				{
					if (xmlSchemaAttribute2.ValidatedUse == XmlSchemaUse.Required && xmlSchemaAttribute2.ValidatedFixedValue == null)
					{
						this.HandleError("Required attribute " + xmlSchemaAttribute2.QualifiedName + " was not found.");
					}
					else if (xmlSchemaAttribute2.ValidatedDefaultValue != null || xmlSchemaAttribute2.ValidatedFixedValue != null)
					{
						this.defaultAttributesCache.Add(xmlSchemaAttribute2);
					}
				}
			}
			if (this.defaultAttributesCache.Count == 0)
			{
				this.defaultAttributes = XsdValidatingReader.emptyAttributeArray;
			}
			else
			{
				this.defaultAttributes = (XmlSchemaAttribute[])this.defaultAttributesCache.ToArray(typeof(XmlSchemaAttribute));
			}
			this.defaultAttributesCache.Clear();
		}

		private void AssessAttributeLocallyValid(XmlSchemaAttribute attr)
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
			if (xmlSchemaDatatype != XmlSchemaSimpleType.AnySimpleType || attr.ValidatedFixedValue != null)
			{
				string text = xmlSchemaDatatype.Normalize(this.reader.Value);
				object parsedValue = null;
				XmlSchemaSimpleType xmlSchemaSimpleType = attr.AttributeType as XmlSchemaSimpleType;
				if (xmlSchemaSimpleType != null)
				{
					this.ValidateRestrictedSimpleTypeValue(xmlSchemaSimpleType, ref xmlSchemaDatatype, text);
				}
				try
				{
					parsedValue = xmlSchemaDatatype.ParseValue(text, this.reader.NameTable, this.NamespaceManager);
				}
				catch (Exception innerException)
				{
					this.HandleError("Attribute value is invalid against its data type " + xmlSchemaDatatype.TokenizedType, innerException);
				}
				if (attr.ValidatedFixedValue != null && attr.ValidatedFixedValue != text)
				{
					this.HandleError("The value of the attribute " + attr.QualifiedName + " does not match with its fixed value.");
					parsedValue = xmlSchemaDatatype.ParseValue(attr.ValidatedFixedValue, this.reader.NameTable, this.NamespaceManager);
				}
				if (this.checkIdentity)
				{
					string text2 = this.idManager.AssessEachAttributeIdentityConstraint(xmlSchemaDatatype, parsedValue, ((XmlQualifiedName)this.elementQNameStack[this.elementQNameStack.Count - 1]).Name);
					if (text2 != null)
					{
						this.HandleError(text2);
					}
				}
			}
		}

		private void AssessAttributeLocallyValidUse(XmlSchemaAttribute attr)
		{
			if (attr.ValidatedUse == XmlSchemaUse.Prohibited)
			{
				this.HandleError("Attribute " + attr.QualifiedName + " is prohibited in this context.");
			}
		}

		private void AssessEndElementSchemaValidity()
		{
			this.ValidateEndSimpleContent();
			this.ValidateEndElementParticle();
			if (this.checkKeyConstraints)
			{
				this.ValidateEndElementKeyConstraints();
			}
			if (this.xsiNilDepth == this.reader.Depth)
			{
				this.xsiNilDepth = -1;
			}
		}

		private void ValidateEndElementKeyConstraints()
		{
			for (int i = 0; i < this.keyTables.Count; i++)
			{
				XsdKeyTable xsdKeyTable = this.keyTables[i] as XsdKeyTable;
				if (xsdKeyTable.StartDepth == this.reader.Depth)
				{
					this.EndIdentityValidation(xsdKeyTable);
				}
				else
				{
					for (int j = 0; j < xsdKeyTable.Entries.Count; j++)
					{
						XsdKeyEntry xsdKeyEntry = xsdKeyTable.Entries[j];
						if (xsdKeyEntry.StartDepth == this.reader.Depth)
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
								if (!xsdKeyEntryField.FieldFound && xsdKeyEntryField.FieldFoundDepth == this.reader.Depth)
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
				if (xsdKeyTable2.StartDepth == this.reader.Depth)
				{
					this.keyTables.RemoveAt(l);
					l--;
				}
			}
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
				if (xsdKeyTable.SelectorMatches(this.elementQNameStack, this.reader.Depth) != null)
				{
					XsdKeyEntry entry = new XsdKeyEntry(xsdKeyTable, this.reader.Depth, this.readerLineInfo);
					xsdKeyTable.Entries.Add(entry);
				}
			}
		}

		private void ValidateKeyFields()
		{
			for (int i = 0; i < this.keyTables.Count; i++)
			{
				XsdKeyTable xsdKeyTable = (XsdKeyTable)this.keyTables[i];
				for (int j = 0; j < xsdKeyTable.Entries.Count; j++)
				{
					try
					{
						this.ProcessKeyEntry(xsdKeyTable.Entries[j]);
					}
					catch (XmlSchemaValidationException schemaException)
					{
						this.HandleError(schemaException);
					}
				}
			}
		}

		private void ProcessKeyEntry(XsdKeyEntry entry)
		{
			bool isXsiNil = this.XsiNilDepth == this.Depth;
			entry.ProcessMatch(false, this.elementQNameStack, this, this.NameTable, this.BaseURI, this.SchemaType, this.NamespaceManager, this.readerLineInfo, this.Depth, null, null, null, isXsiNil, this.CurrentKeyFieldConsumers);
			if (this.MoveToFirstAttribute())
			{
				try
				{
					for (;;)
					{
						string namespaceURI = this.NamespaceURI;
						if (namespaceURI == null)
						{
							goto IL_BC;
						}
						if (XsdValidatingReader.<>f__switch$map5 == null)
						{
							XsdValidatingReader.<>f__switch$map5 = new Dictionary<string, int>(2)
							{
								{
									"http://www.w3.org/2000/xmlns/",
									0
								},
								{
									"http://www.w3.org/2001/XMLSchema-instance",
									0
								}
							};
						}
						int num;
						if (!XsdValidatingReader.<>f__switch$map5.TryGetValue(namespaceURI, out num))
						{
							goto IL_BC;
						}
						if (num != 0)
						{
							goto IL_BC;
						}
						IL_15B:
						if (!this.MoveToNextAttribute())
						{
							break;
						}
						continue;
						IL_BC:
						XmlSchemaDatatype xmlSchemaDatatype = this.SchemaType as XmlSchemaDatatype;
						XmlSchemaSimpleType xmlSchemaSimpleType = this.SchemaType as XmlSchemaSimpleType;
						if (xmlSchemaDatatype == null && xmlSchemaSimpleType != null)
						{
							xmlSchemaDatatype = xmlSchemaSimpleType.Datatype;
						}
						object obj = null;
						if (xmlSchemaDatatype != null)
						{
							obj = xmlSchemaDatatype.ParseValue(this.Value, this.NameTable, this.NamespaceManager);
						}
						if (obj == null)
						{
							obj = this.Value;
						}
						entry.ProcessMatch(true, this.elementQNameStack, this, this.NameTable, this.BaseURI, this.SchemaType, this.NamespaceManager, this.readerLineInfo, this.Depth, this.LocalName, this.NamespaceURI, obj, false, this.CurrentKeyFieldConsumers);
						goto IL_15B;
					}
				}
				finally
				{
					this.MoveToElement();
				}
			}
		}

		private XsdKeyTable CreateNewKeyTable(XmlSchemaIdentityConstraint ident)
		{
			XsdKeyTable xsdKeyTable = new XsdKeyTable(ident);
			xsdKeyTable.StartDepth = this.reader.Depth;
			this.keyTables.Add(xsdKeyTable);
			return xsdKeyTable;
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
							obj = dt.ParseValue(value, this.NameTable, this.NamespaceManager);
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
					if (!xsdKeyEntryField.SetIdentityField(obj, this.reader.Depth == this.xsiNilDepth, dt as XsdAnySimpleType, this.Depth, this.readerLineInfo))
					{
						this.HandleError("Two or more identical key value was found: '" + value + "' .");
					}
					this.currentKeyFieldConsumers.RemoveAt(0);
				}
			}
		}

		private void EndIdentityValidation(XsdKeyTable seq)
		{
			ArrayList arrayList = null;
			for (int i = 0; i < seq.Entries.Count; i++)
			{
				XsdKeyEntry xsdKeyEntry = seq.Entries[i];
				if (!xsdKeyEntry.KeyFound)
				{
					if (seq.SourceSchemaIdentity is XmlSchemaKey)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
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
			if (arrayList != null)
			{
				this.HandleError("Invalid identity constraints were found. Key was not found. " + string.Join(", ", arrayList.ToArray(typeof(string)) as string[]));
			}
			XmlSchemaKeyref xmlSchemaKeyref = seq.SourceSchemaIdentity as XmlSchemaKeyref;
			if (xmlSchemaKeyref != null)
			{
				this.EndKeyrefValidation(seq, xmlSchemaKeyref.Target);
			}
		}

		private void EndKeyrefValidation(XsdKeyTable seq, XmlSchemaIdentityConstraint targetIdent)
		{
			for (int i = this.keyTables.Count - 1; i >= 0; i--)
			{
				XsdKeyTable xsdKeyTable = this.keyTables[i] as XsdKeyTable;
				if (xsdKeyTable.SourceSchemaIdentity == targetIdent)
				{
					seq.ReferencedKey = xsdKeyTable;
					for (int j = 0; j < seq.FinishedEntries.Count; j++)
					{
						XsdKeyEntry xsdKeyEntry = seq.FinishedEntries[j];
						for (int k = 0; k < xsdKeyTable.FinishedEntries.Count; k++)
						{
							XsdKeyEntry other = xsdKeyTable.FinishedEntries[k];
							if (xsdKeyEntry.CompareIdentity(other))
							{
								xsdKeyEntry.KeyRefFound = true;
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
			ArrayList arrayList = null;
			for (int l = 0; l < seq.FinishedEntries.Count; l++)
			{
				XsdKeyEntry xsdKeyEntry2 = seq.FinishedEntries[l];
				if (!xsdKeyEntry2.KeyRefFound)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(string.Concat(new object[]
					{
						" line ",
						xsdKeyEntry2.SelectorLineNumber,
						", position ",
						xsdKeyEntry2.SelectorLinePosition
					}));
				}
			}
			if (arrayList != null)
			{
				this.HandleError("Invalid identity constraints were found. Referenced key was not found: " + string.Join(" / ", arrayList.ToArray(typeof(string)) as string[]));
			}
		}

		public override void Close()
		{
			this.reader.Close();
		}

		public override string GetAttribute(int i)
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				return this.reader.GetAttribute(i);
			}
			if (this.reader.AttributeCount > i)
			{
				this.reader.GetAttribute(i);
			}
			int num = i - this.reader.AttributeCount;
			if (i < this.AttributeCount)
			{
				return this.defaultAttributes[num].DefaultValue;
			}
			throw new ArgumentOutOfRangeException("i", i, "Specified attribute index is out of range.");
		}

		public override string GetAttribute(string name)
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				return this.reader.GetAttribute(name);
			}
			string attribute = this.reader.GetAttribute(name);
			if (attribute != null)
			{
				return attribute;
			}
			XmlQualifiedName xmlQualifiedName = this.SplitQName(name);
			return this.GetDefaultAttribute(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
		}

		private XmlQualifiedName SplitQName(string name)
		{
			if (!XmlChar.IsName(name))
			{
				throw new ArgumentException("Invalid name was specified.", "name");
			}
			Exception ex = null;
			XmlQualifiedName result = XmlSchemaUtil.ToQName(this.reader, name, out ex);
			if (ex != null)
			{
				return XmlQualifiedName.Empty;
			}
			return result;
		}

		public override string GetAttribute(string localName, string ns)
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				return this.reader.GetAttribute(localName, ns);
			}
			string attribute = this.reader.GetAttribute(localName, ns);
			if (attribute != null)
			{
				return attribute;
			}
			return this.GetDefaultAttribute(localName, ns);
		}

		private string GetDefaultAttribute(string localName, string ns)
		{
			int num = this.FindDefaultAttribute(localName, ns);
			if (num < 0)
			{
				return null;
			}
			string text = this.defaultAttributes[num].ValidatedDefaultValue;
			if (text == null)
			{
				text = this.defaultAttributes[num].ValidatedFixedValue;
			}
			return text;
		}

		private int FindDefaultAttribute(string localName, string ns)
		{
			for (int i = 0; i < this.defaultAttributes.Length; i++)
			{
				XmlSchemaAttribute xmlSchemaAttribute = this.defaultAttributes[i];
				if (xmlSchemaAttribute.QualifiedName.Name == localName && (ns == null || xmlSchemaAttribute.QualifiedName.Namespace == ns))
				{
					return i;
				}
			}
			return -1;
		}

		public bool HasLineInfo()
		{
			return this.readerLineInfo != null && this.readerLineInfo.HasLineInfo();
		}

		public override string LookupNamespace(string prefix)
		{
			return this.reader.LookupNamespace(prefix);
		}

		public override void MoveToAttribute(int i)
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				this.reader.MoveToAttribute(i);
				return;
			}
			this.currentAttrType = null;
			if (i < this.reader.AttributeCount)
			{
				this.reader.MoveToAttribute(i);
				this.currentDefaultAttribute = -1;
				this.defaultAttributeConsumed = false;
			}
			if (i < this.AttributeCount)
			{
				this.currentDefaultAttribute = i - this.reader.AttributeCount;
				this.defaultAttributeConsumed = false;
				return;
			}
			throw new ArgumentOutOfRangeException("i", i, "Attribute index is out of range.");
		}

		public override bool MoveToAttribute(string name)
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				return this.reader.MoveToAttribute(name);
			}
			this.currentAttrType = null;
			bool flag = this.reader.MoveToAttribute(name);
			if (flag)
			{
				this.currentDefaultAttribute = -1;
				this.defaultAttributeConsumed = false;
				return true;
			}
			return this.MoveToDefaultAttribute(name, null);
		}

		public override bool MoveToAttribute(string localName, string ns)
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				return this.reader.MoveToAttribute(localName, ns);
			}
			this.currentAttrType = null;
			bool flag = this.reader.MoveToAttribute(localName, ns);
			if (flag)
			{
				this.currentDefaultAttribute = -1;
				this.defaultAttributeConsumed = false;
				return true;
			}
			return this.MoveToDefaultAttribute(localName, ns);
		}

		private bool MoveToDefaultAttribute(string localName, string ns)
		{
			int num = this.FindDefaultAttribute(localName, ns);
			if (num < 0)
			{
				return false;
			}
			this.currentDefaultAttribute = num;
			this.defaultAttributeConsumed = false;
			return true;
		}

		public override bool MoveToElement()
		{
			this.currentDefaultAttribute = -1;
			this.defaultAttributeConsumed = false;
			this.currentAttrType = null;
			return this.reader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				return this.reader.MoveToFirstAttribute();
			}
			this.currentAttrType = null;
			if (this.reader.AttributeCount > 0)
			{
				bool flag = this.reader.MoveToFirstAttribute();
				if (flag)
				{
					this.currentDefaultAttribute = -1;
					this.defaultAttributeConsumed = false;
				}
				return flag;
			}
			if (this.defaultAttributes.Length > 0)
			{
				this.currentDefaultAttribute = 0;
				this.defaultAttributeConsumed = false;
				return true;
			}
			return false;
		}

		public override bool MoveToNextAttribute()
		{
			XmlNodeType nodeType = this.reader.NodeType;
			if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
			{
				return this.reader.MoveToNextAttribute();
			}
			this.currentAttrType = null;
			if (this.currentDefaultAttribute >= 0)
			{
				if (this.defaultAttributes.Length == this.currentDefaultAttribute + 1)
				{
					return false;
				}
				this.currentDefaultAttribute++;
				this.defaultAttributeConsumed = false;
				return true;
			}
			else
			{
				bool flag = this.reader.MoveToNextAttribute();
				if (flag)
				{
					this.currentDefaultAttribute = -1;
					this.defaultAttributeConsumed = false;
					return true;
				}
				if (this.defaultAttributes.Length > 0)
				{
					this.currentDefaultAttribute = 0;
					this.defaultAttributeConsumed = false;
					return true;
				}
				return false;
			}
		}

		private XmlSchema ReadExternalSchema(string uri)
		{
			Uri uri2 = this.resolver.ResolveUri((!(this.BaseURI != string.Empty)) ? null : new Uri(this.BaseURI), uri);
			string url = (!(uri2 != null)) ? string.Empty : uri2.ToString();
			XmlTextReader xmlTextReader = null;
			XmlSchema result;
			try
			{
				xmlTextReader = new XmlTextReader(url, (Stream)this.resolver.GetEntity(uri2, null, typeof(Stream)), this.NameTable);
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

		private void ExamineAdditionalSchema()
		{
			if (this.resolver == null || this.ValidationType == ValidationType.None)
			{
				return;
			}
			XmlSchema xmlSchema = null;
			string text = this.reader.GetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
			bool flag = false;
			if (text != null)
			{
				string[] array = null;
				try
				{
					text = XmlSchemaDatatype.FromName("token", "http://www.w3.org/2001/XMLSchema").Normalize(text);
					array = text.Split(XmlChar.WhitespaceChars);
				}
				catch (Exception innerException)
				{
					if (this.schemas.Count == 0)
					{
						this.HandleError("Invalid schemaLocation attribute format.", innerException, true);
					}
					array = new string[0];
				}
				if (array.Length % 2 != 0 && this.schemas.Count == 0)
				{
					this.HandleError("Invalid schemaLocation attribute format.");
				}
				int i = 0;
				do
				{
					try
					{
						while (i < array.Length)
						{
							xmlSchema = this.ReadExternalSchema(array[i + 1]);
							if (xmlSchema.TargetNamespace == null)
							{
								xmlSchema.TargetNamespace = array[i];
							}
							else if (xmlSchema.TargetNamespace != array[i])
							{
								this.HandleError("Specified schema has different target namespace.");
							}
							if (xmlSchema != null)
							{
								if (!this.schemas.Contains(xmlSchema.TargetNamespace))
								{
									flag = true;
									this.schemas.Add(xmlSchema);
								}
								xmlSchema = null;
							}
							i += 2;
						}
					}
					catch (Exception)
					{
						if (!this.schemas.Contains(array[i]))
						{
							this.HandleError(string.Format("Could not resolve schema location URI: {0}", (i + 1 >= array.Length) ? string.Empty : array[i + 1]), null, true);
						}
						i += 2;
					}
				}
				while (i < array.Length);
			}
			string attribute = this.reader.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null)
			{
				try
				{
					xmlSchema = this.ReadExternalSchema(attribute);
				}
				catch (Exception)
				{
					if (this.schemas.Count != 0)
					{
						this.HandleError("Could not resolve schema location URI: " + attribute, null, true);
					}
				}
				if (xmlSchema != null && xmlSchema.TargetNamespace != null)
				{
					this.HandleError("Specified schema has different target namespace.");
				}
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

		public override bool Read()
		{
			this.validationStarted = true;
			this.currentDefaultAttribute = -1;
			this.defaultAttributeConsumed = false;
			this.currentAttrType = null;
			this.defaultAttributes = XsdValidatingReader.emptyAttributeArray;
			bool flag = this.reader.Read();
			if (this.reader.Depth == 0 && this.reader.NodeType == XmlNodeType.Element)
			{
				DTDValidatingReader dtdvalidatingReader = this.reader as DTDValidatingReader;
				if (dtdvalidatingReader != null && dtdvalidatingReader.DTD == null)
				{
					this.reader = dtdvalidatingReader.Source;
				}
				this.ExamineAdditionalSchema();
			}
			if (this.schemas.Count == 0)
			{
				return flag;
			}
			if (!this.schemas.IsCompiled)
			{
				this.schemas.Compile();
			}
			if (this.checkIdentity)
			{
				this.idManager.OnStartElement();
			}
			if (!flag && this.checkIdentity && this.idManager.HasMissingIDReferences())
			{
				this.HandleError("There are missing ID references: " + this.idManager.GetMissingIDString());
			}
			XmlNodeType nodeType = this.reader.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				if (this.checkKeyConstraints)
				{
					this.elementQNameStack.Add(new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI));
				}
				if (this.skipValidationDepth < 0 || this.reader.Depth <= this.skipValidationDepth)
				{
					this.ValidateEndSimpleContent();
					this.AssessStartElementSchemaValidity();
				}
				if (!this.reader.IsEmptyElement)
				{
					if (this.xsiNilDepth < this.reader.Depth)
					{
						this.shouldValidateCharacters = true;
					}
					return flag;
				}
				break;
			default:
				switch (nodeType)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					goto IL_249;
				case XmlNodeType.EndElement:
					break;
				default:
					return flag;
				}
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				goto IL_249;
			}
			if (this.reader.Depth == this.skipValidationDepth)
			{
				this.skipValidationDepth = -1;
			}
			else if (this.skipValidationDepth < 0 || this.reader.Depth <= this.skipValidationDepth)
			{
				this.AssessEndElementSchemaValidity();
			}
			if (this.checkKeyConstraints)
			{
				this.elementQNameStack.RemoveAt(this.elementQNameStack.Count - 1);
			}
			return flag;
			IL_249:
			if (this.skipValidationDepth < 0 || this.reader.Depth <= this.skipValidationDepth)
			{
				XmlSchemaComplexType xmlSchemaComplexType = this.Context.ActualType as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null)
				{
					XmlSchemaContentType contentType = xmlSchemaComplexType.ContentType;
					if (contentType != XmlSchemaContentType.Empty)
					{
						if (contentType == XmlSchemaContentType.ElementOnly)
						{
							if (this.reader.NodeType != XmlNodeType.Whitespace)
							{
								this.HandleError(string.Format("Not allowed character content is found (current content model '{0}' is element-only).", xmlSchemaComplexType.QualifiedName));
							}
						}
					}
					else
					{
						this.HandleError(string.Format("Not allowed character content is found (current element content model '{0}' is empty).", xmlSchemaComplexType.QualifiedName));
					}
				}
				this.ValidateCharacters();
			}
			return flag;
		}

		public override bool ReadAttributeValue()
		{
			if (this.currentDefaultAttribute < 0)
			{
				return this.reader.ReadAttributeValue();
			}
			if (this.defaultAttributeConsumed)
			{
				return false;
			}
			this.defaultAttributeConsumed = true;
			return true;
		}

		public override string ReadString()
		{
			return base.ReadString();
		}

		public override void ResolveEntity()
		{
			this.reader.ResolveEntity();
		}
	}
}
