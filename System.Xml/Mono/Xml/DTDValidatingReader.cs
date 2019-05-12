using Mono.Xml2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml
{
	internal class DTDValidatingReader : XmlReader, IHasXmlParserContext, IHasXmlSchemaInfo, IXmlLineInfo, IXmlNamespaceResolver
	{
		private EntityResolvingXmlReader reader;

		private System.Xml.XmlTextReader sourceTextReader;

		private XmlValidatingReader validatingReader;

		private DTDObjectModel dtd;

		private XmlResolver resolver;

		private string currentElement;

		private DTDValidatingReader.AttributeSlot[] attributes;

		private int attributeCount;

		private int currentAttribute = -1;

		private bool consumedAttribute;

		private Stack elementStack;

		private Stack automataStack;

		private bool popScope;

		private bool isStandalone;

		private DTDAutomata currentAutomata;

		private DTDAutomata previousAutomata;

		private ArrayList idList;

		private ArrayList missingIDReferences;

		private XmlNamespaceManager nsmgr;

		private string currentTextValue;

		private string constructingTextValue;

		private bool shouldResetCurrentTextValue;

		private bool isSignificantWhitespace;

		private bool isWhitespace;

		private bool isText;

		private Stack attributeValueEntityStack = new Stack();

		private StringBuilder valueBuilder;

		private char[] whitespaceChars = new char[]
		{
			' '
		};

		public DTDValidatingReader(XmlReader reader) : this(reader, null)
		{
		}

		internal DTDValidatingReader(XmlReader reader, XmlValidatingReader validatingReader)
		{
			this.reader = new EntityResolvingXmlReader(reader);
			this.sourceTextReader = (reader as System.Xml.XmlTextReader);
			this.elementStack = new Stack();
			this.automataStack = new Stack();
			this.attributes = new DTDValidatingReader.AttributeSlot[10];
			this.nsmgr = new XmlNamespaceManager(reader.NameTable);
			this.validatingReader = validatingReader;
			this.valueBuilder = new StringBuilder();
			this.idList = new ArrayList();
			this.missingIDReferences = new ArrayList();
			System.Xml.XmlTextReader xmlTextReader = reader as System.Xml.XmlTextReader;
			if (xmlTextReader != null)
			{
				this.resolver = xmlTextReader.Resolver;
			}
			else
			{
				this.resolver = new XmlUrlResolver();
			}
		}

		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			IXmlNamespaceResolver xmlNamespaceResolver = this.reader;
			IDictionary<string, string> result;
			if (xmlNamespaceResolver != null)
			{
				IDictionary<string, string> namespacesInScope = xmlNamespaceResolver.GetNamespacesInScope(scope);
				result = namespacesInScope;
			}
			else
			{
				result = new Dictionary<string, string>();
			}
			return result;
		}

		bool IXmlLineInfo.HasLineInfo()
		{
			IXmlLineInfo xmlLineInfo = this.reader;
			return xmlLineInfo != null && xmlLineInfo.HasLineInfo();
		}

		string IXmlNamespaceResolver.LookupPrefix(string ns)
		{
			IXmlNamespaceResolver xmlNamespaceResolver = this.reader;
			return (xmlNamespaceResolver == null) ? null : xmlNamespaceResolver.LookupPrefix(ns);
		}

		internal EntityResolvingXmlReader Source
		{
			get
			{
				return this.reader;
			}
		}

		public DTDObjectModel DTD
		{
			get
			{
				return this.dtd;
			}
		}

		public EntityHandling EntityHandling
		{
			get
			{
				return this.reader.EntityHandling;
			}
			set
			{
				this.reader.EntityHandling = value;
			}
		}

		public override void Close()
		{
			this.reader.Close();
		}

		private int GetAttributeIndex(string name)
		{
			for (int i = 0; i < this.attributeCount; i++)
			{
				if (this.attributes[i].Name == name)
				{
					return i;
				}
			}
			return -1;
		}

		private int GetAttributeIndex(string localName, string ns)
		{
			for (int i = 0; i < this.attributeCount; i++)
			{
				if (this.attributes[i].LocalName == localName && this.attributes[i].NS == ns)
				{
					return i;
				}
			}
			return -1;
		}

		public override string GetAttribute(int i)
		{
			if (this.currentTextValue != null)
			{
				throw new IndexOutOfRangeException("Specified index is out of range: " + i);
			}
			if (this.attributeCount <= i)
			{
				throw new IndexOutOfRangeException("Specified index is out of range: " + i);
			}
			return this.attributes[i].Value;
		}

		public override string GetAttribute(string name)
		{
			if (this.currentTextValue != null)
			{
				return null;
			}
			int attributeIndex = this.GetAttributeIndex(name);
			return (attributeIndex >= 0) ? this.attributes[attributeIndex].Value : null;
		}

		public override string GetAttribute(string name, string ns)
		{
			if (this.currentTextValue != null)
			{
				return null;
			}
			int attributeIndex = this.GetAttributeIndex(name, ns);
			return (attributeIndex >= 0) ? this.attributes[attributeIndex].Value : null;
		}

		public override string LookupNamespace(string prefix)
		{
			string text = this.nsmgr.LookupNamespace(this.NameTable.Get(prefix));
			return (!(text == string.Empty)) ? text : null;
		}

		public override void MoveToAttribute(int i)
		{
			if (this.currentTextValue != null)
			{
				throw new IndexOutOfRangeException("The index is out of range.");
			}
			if (this.attributeCount <= i)
			{
				throw new IndexOutOfRangeException("The index is out of range.");
			}
			if (i < this.reader.AttributeCount)
			{
				this.reader.MoveToAttribute(i);
			}
			this.currentAttribute = i;
			this.consumedAttribute = false;
		}

		public override bool MoveToAttribute(string name)
		{
			if (this.currentTextValue != null)
			{
				return false;
			}
			int attributeIndex = this.GetAttributeIndex(name);
			if (attributeIndex < 0)
			{
				return false;
			}
			if (attributeIndex < this.reader.AttributeCount)
			{
				this.reader.MoveToAttribute(attributeIndex);
			}
			this.currentAttribute = attributeIndex;
			this.consumedAttribute = false;
			return true;
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			if (this.currentTextValue != null)
			{
				return false;
			}
			int attributeIndex = this.GetAttributeIndex(name, ns);
			if (attributeIndex < 0)
			{
				return false;
			}
			if (attributeIndex < this.reader.AttributeCount)
			{
				this.reader.MoveToAttribute(attributeIndex);
			}
			this.currentAttribute = attributeIndex;
			this.consumedAttribute = false;
			return true;
		}

		public override bool MoveToElement()
		{
			if (this.currentTextValue != null)
			{
				return false;
			}
			if (!this.reader.MoveToElement() && !this.IsDefault)
			{
				return false;
			}
			this.currentAttribute = -1;
			this.consumedAttribute = false;
			return true;
		}

		public override bool MoveToFirstAttribute()
		{
			if (this.currentTextValue != null)
			{
				return false;
			}
			if (this.attributeCount == 0)
			{
				return false;
			}
			this.currentAttribute = 0;
			this.reader.MoveToFirstAttribute();
			this.consumedAttribute = false;
			return true;
		}

		public override bool MoveToNextAttribute()
		{
			if (this.currentTextValue != null)
			{
				return false;
			}
			if (this.currentAttribute == -1)
			{
				return this.MoveToFirstAttribute();
			}
			if (++this.currentAttribute == this.attributeCount)
			{
				this.currentAttribute--;
				return false;
			}
			if (this.currentAttribute < this.reader.AttributeCount)
			{
				this.reader.MoveToAttribute(this.currentAttribute);
			}
			this.consumedAttribute = false;
			return true;
		}

		public override bool Read()
		{
			if (this.currentTextValue != null)
			{
				this.shouldResetCurrentTextValue = true;
			}
			if (this.currentAttribute >= 0)
			{
				this.MoveToElement();
			}
			this.currentElement = null;
			this.currentAttribute = -1;
			this.consumedAttribute = false;
			this.attributeCount = 0;
			this.isWhitespace = false;
			this.isSignificantWhitespace = false;
			this.isText = false;
			bool flag = this.ReadContent() || this.currentTextValue != null;
			if (!flag && (this.Settings == null || (this.Settings.ValidationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) == XmlSchemaValidationFlags.None) && this.missingIDReferences.Count > 0)
			{
				this.HandleError("Missing ID reference was found: " + string.Join(",", this.missingIDReferences.ToArray(typeof(string)) as string[]), XmlSeverityType.Error);
				this.missingIDReferences.Clear();
			}
			if (this.validatingReader != null)
			{
				this.EntityHandling = this.validatingReader.EntityHandling;
			}
			return flag;
		}

		private bool ReadContent()
		{
			switch (this.reader.ReadState)
			{
			case ReadState.Error:
			case ReadState.EndOfFile:
			case ReadState.Closed:
				return false;
			default:
			{
				if (this.popScope)
				{
					this.nsmgr.PopScope();
					this.popScope = false;
					if (this.elementStack.Count == 0)
					{
						this.currentAutomata = null;
					}
				}
				bool flag = !this.reader.EOF;
				if (this.shouldResetCurrentTextValue)
				{
					this.currentTextValue = null;
					this.shouldResetCurrentTextValue = false;
				}
				else
				{
					flag = this.reader.Read();
				}
				if (flag)
				{
					return this.ProcessContent();
				}
				if (this.elementStack.Count != 0)
				{
					throw new InvalidOperationException("Unexpected end of XmlReader.");
				}
				return false;
			}
			}
		}

		private bool ProcessContent()
		{
			switch (this.reader.NodeType)
			{
			case XmlNodeType.Element:
				if (this.constructingTextValue != null)
				{
					this.currentTextValue = this.constructingTextValue;
					this.constructingTextValue = null;
					if (this.isWhitespace)
					{
						this.ValidateWhitespaceNode();
					}
					return true;
				}
				this.ProcessStartElement();
				goto IL_1BB;
			case XmlNodeType.Attribute:
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Document:
			case XmlNodeType.Notation:
			case XmlNodeType.EndEntity:
				goto IL_1BB;
			case XmlNodeType.Text:
				this.isWhitespace = (this.isSignificantWhitespace = false);
				this.isText = true;
				break;
			case XmlNodeType.CDATA:
				this.isSignificantWhitespace = (this.isWhitespace = false);
				this.isText = true;
				this.ValidateText();
				if (this.currentTextValue != null)
				{
					this.currentTextValue = this.constructingTextValue;
					this.constructingTextValue = null;
					return true;
				}
				goto IL_1BB;
			case XmlNodeType.DocumentType:
				this.ReadDoctype();
				goto IL_1BB;
			case XmlNodeType.DocumentFragment:
				break;
			case XmlNodeType.Whitespace:
				if (!this.isText && !this.isSignificantWhitespace)
				{
					this.isWhitespace = true;
				}
				break;
			case XmlNodeType.SignificantWhitespace:
				if (!this.isText)
				{
					this.isSignificantWhitespace = true;
				}
				this.isWhitespace = false;
				break;
			case XmlNodeType.EndElement:
				if (this.constructingTextValue != null)
				{
					this.currentTextValue = this.constructingTextValue;
					this.constructingTextValue = null;
					return true;
				}
				this.ProcessEndElement();
				goto IL_1BB;
			case XmlNodeType.XmlDeclaration:
				this.FillAttributes();
				if (this.GetAttribute("standalone") == "yes")
				{
					this.isStandalone = true;
				}
				goto IL_1BB;
			default:
				goto IL_1BB;
			}
			if (this.reader.NodeType != XmlNodeType.DocumentFragment)
			{
				this.ValidateText();
			}
			IL_1BB:
			if (this.isWhitespace)
			{
				this.ValidateWhitespaceNode();
			}
			this.currentTextValue = this.constructingTextValue;
			this.constructingTextValue = null;
			return true;
		}

		private void FillAttributes()
		{
			if (this.reader.MoveToFirstAttribute())
			{
				do
				{
					DTDValidatingReader.AttributeSlot attributeSlot = this.GetAttributeSlot();
					attributeSlot.Name = this.reader.Name;
					attributeSlot.LocalName = this.reader.LocalName;
					attributeSlot.Prefix = this.reader.Prefix;
					attributeSlot.NS = this.reader.NamespaceURI;
					attributeSlot.Value = this.reader.Value;
				}
				while (this.reader.MoveToNextAttribute());
				this.reader.MoveToElement();
			}
		}

		private void ValidateText()
		{
			if (this.currentAutomata == null)
			{
				return;
			}
			DTDElementDeclaration dtdelementDeclaration = null;
			if (this.elementStack.Count > 0)
			{
				dtdelementDeclaration = this.dtd.ElementDecls[this.elementStack.Peek() as string];
			}
			if (dtdelementDeclaration != null && !dtdelementDeclaration.IsMixedContent && !dtdelementDeclaration.IsAny && !this.isWhitespace)
			{
				this.HandleError(string.Format("Current element {0} does not allow character data content.", this.elementStack.Peek() as string), XmlSeverityType.Error);
				this.currentAutomata = this.previousAutomata;
			}
		}

		private void ValidateWhitespaceNode()
		{
			if (this.isStandalone && this.DTD != null && this.elementStack.Count > 0)
			{
				DTDElementDeclaration dtdelementDeclaration = this.DTD.ElementDecls[this.elementStack.Peek() as string];
				if (dtdelementDeclaration != null && !dtdelementDeclaration.IsInternalSubset && !dtdelementDeclaration.IsMixedContent && !dtdelementDeclaration.IsAny && !dtdelementDeclaration.IsEmpty)
				{
					this.HandleError("In a standalone document, whitespace cannot appear in an element which is declared to contain only element children.", XmlSeverityType.Error);
				}
			}
		}

		private void HandleError(string message, XmlSeverityType severity)
		{
			if (this.validatingReader != null && this.validatingReader.ValidationType == ValidationType.None)
			{
				return;
			}
			bool flag = ((IXmlLineInfo)this).HasLineInfo();
			XmlSchemaException ex = new XmlSchemaException(message, (!flag) ? 0 : ((IXmlLineInfo)this).LineNumber, (!flag) ? 0 : ((IXmlLineInfo)this).LinePosition, null, this.BaseURI, null);
			this.HandleError(ex, severity);
		}

		private void HandleError(XmlSchemaException ex, XmlSeverityType severity)
		{
			if (this.validatingReader != null && this.validatingReader.ValidationType == ValidationType.None)
			{
				return;
			}
			if (this.validatingReader != null)
			{
				this.validatingReader.OnValidationEvent(this, new ValidationEventArgs(ex, ex.Message, severity));
			}
			else if (severity == XmlSeverityType.Error)
			{
				throw ex;
			}
		}

		private void ValidateAttributes(DTDAttListDeclaration decl, bool validate)
		{
			this.DtdValidateAttributes(decl, validate);
			for (int i = 0; i < this.attributeCount; i++)
			{
				DTDValidatingReader.AttributeSlot attributeSlot = this.attributes[i];
				if (attributeSlot.Name == "xmlns" || attributeSlot.Prefix == "xmlns")
				{
					this.nsmgr.AddNamespace((!(attributeSlot.Prefix == "xmlns")) ? string.Empty : attributeSlot.LocalName, attributeSlot.Value);
				}
			}
			for (int j = 0; j < this.attributeCount; j++)
			{
				DTDValidatingReader.AttributeSlot attributeSlot2 = this.attributes[j];
				if (attributeSlot2.Name == "xmlns")
				{
					attributeSlot2.NS = "http://www.w3.org/2000/xmlns/";
				}
				else if (attributeSlot2.Prefix.Length > 0)
				{
					attributeSlot2.NS = this.LookupNamespace(attributeSlot2.Prefix);
				}
				else
				{
					attributeSlot2.NS = string.Empty;
				}
			}
		}

		private DTDValidatingReader.AttributeSlot GetAttributeSlot()
		{
			if (this.attributeCount == this.attributes.Length)
			{
				DTDValidatingReader.AttributeSlot[] destinationArray = new DTDValidatingReader.AttributeSlot[this.attributeCount << 1];
				Array.Copy(this.attributes, destinationArray, this.attributeCount);
				this.attributes = destinationArray;
			}
			if (this.attributes[this.attributeCount] == null)
			{
				this.attributes[this.attributeCount] = new DTDValidatingReader.AttributeSlot();
			}
			DTDValidatingReader.AttributeSlot attributeSlot = this.attributes[this.attributeCount];
			attributeSlot.Clear();
			this.attributeCount++;
			return attributeSlot;
		}

		private void DtdValidateAttributes(DTDAttListDeclaration decl, bool validate)
		{
			while (this.reader.MoveToNextAttribute())
			{
				string name = this.reader.Name;
				DTDValidatingReader.AttributeSlot attributeSlot = this.GetAttributeSlot();
				attributeSlot.Name = this.reader.Name;
				attributeSlot.LocalName = this.reader.LocalName;
				attributeSlot.Prefix = this.reader.Prefix;
				XmlReader xmlReader = this.reader;
				string text = string.Empty;
				while (this.attributeValueEntityStack.Count >= 0)
				{
					if (!xmlReader.ReadAttributeValue())
					{
						if (this.attributeValueEntityStack.Count <= 0)
						{
							break;
						}
						xmlReader = (this.attributeValueEntityStack.Pop() as XmlReader);
					}
					else
					{
						XmlNodeType nodeType = xmlReader.NodeType;
						if (nodeType != XmlNodeType.EntityReference)
						{
							if (nodeType != XmlNodeType.EndEntity)
							{
								text += xmlReader.Value;
							}
						}
						else
						{
							DTDEntityDeclaration dtdentityDeclaration = this.DTD.EntityDecls[xmlReader.Name];
							if (dtdentityDeclaration == null)
							{
								this.HandleError(string.Format("Referenced entity {0} is not declared.", xmlReader.Name), XmlSeverityType.Error);
							}
							else
							{
								System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(dtdentityDeclaration.EntityValue, XmlNodeType.Attribute, this.ParserContext);
								this.attributeValueEntityStack.Push(xmlReader);
								xmlReader = xmlTextReader;
							}
						}
					}
				}
				this.reader.MoveToElement();
				this.reader.MoveToAttribute(name);
				attributeSlot.Value = this.FilterNormalization(name, text);
				if (validate)
				{
					DTDAttributeDefinition dtdattributeDefinition = decl[this.reader.Name];
					if (dtdattributeDefinition != null)
					{
						if (dtdattributeDefinition.EnumeratedAttributeDeclaration.Count > 0 && !dtdattributeDefinition.EnumeratedAttributeDeclaration.Contains(attributeSlot.Value))
						{
							this.HandleError(string.Format("Attribute enumeration constraint error in attribute {0}, value {1}.", this.reader.Name, text), XmlSeverityType.Error);
						}
						if (dtdattributeDefinition.EnumeratedNotations.Count > 0 && !dtdattributeDefinition.EnumeratedNotations.Contains(attributeSlot.Value))
						{
							this.HandleError(string.Format("Attribute notation enumeration constraint error in attribute {0}, value {1}.", this.reader.Name, text), XmlSeverityType.Error);
						}
						string text2 = null;
						if (dtdattributeDefinition.Datatype != null)
						{
							text2 = this.FilterNormalization(dtdattributeDefinition.Name, text);
						}
						else
						{
							text2 = text;
						}
						string[] array = null;
						switch (dtdattributeDefinition.Datatype.TokenizedType)
						{
						case XmlTokenizedType.IDREFS:
						case XmlTokenizedType.ENTITIES:
						case XmlTokenizedType.NMTOKENS:
							try
							{
								array = (dtdattributeDefinition.Datatype.ParseValue(text2, this.NameTable, null) as string[]);
							}
							catch (Exception)
							{
								this.HandleError("Attribute value is invalid against its data type.", XmlSeverityType.Error);
								array = new string[0];
							}
							break;
						case XmlTokenizedType.ENTITY:
						case XmlTokenizedType.NMTOKEN:
							goto IL_2D6;
						default:
							goto IL_2D6;
						}
						IL_31C:
						switch (dtdattributeDefinition.Datatype.TokenizedType)
						{
						case XmlTokenizedType.ID:
							if (this.idList.Contains(text2))
							{
								this.HandleError(string.Format("Node with ID {0} was already appeared.", text), XmlSeverityType.Error);
							}
							else
							{
								if (this.missingIDReferences.Contains(text2))
								{
									this.missingIDReferences.Remove(text2);
								}
								this.idList.Add(text2);
							}
							break;
						case XmlTokenizedType.IDREF:
							if (!this.idList.Contains(text2))
							{
								this.missingIDReferences.Add(text2);
							}
							break;
						case XmlTokenizedType.IDREFS:
							foreach (string text3 in array)
							{
								if (!this.idList.Contains(text3))
								{
									this.missingIDReferences.Add(text3);
								}
							}
							break;
						case XmlTokenizedType.ENTITY:
						{
							DTDEntityDeclaration dtdentityDeclaration2 = this.dtd.EntityDecls[text2];
							if (dtdentityDeclaration2 == null)
							{
								this.HandleError("Reference to undeclared entity was found in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
							}
							else if (dtdentityDeclaration2.NotationName == null)
							{
								this.HandleError("The entity specified by entity type value must be an unparsed entity. The entity definition has no NDATA in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
							}
							break;
						}
						case XmlTokenizedType.ENTITIES:
							foreach (string rawValue in array)
							{
								DTDEntityDeclaration dtdentityDeclaration2 = this.dtd.EntityDecls[this.FilterNormalization(this.reader.Name, rawValue)];
								if (dtdentityDeclaration2 == null)
								{
									this.HandleError("Reference to undeclared entity was found in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
								}
								else if (dtdentityDeclaration2.NotationName == null)
								{
									this.HandleError("The entity specified by ENTITIES type value must be an unparsed entity. The entity definition has no NDATA in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
								}
							}
							break;
						}
						if (this.isStandalone && !dtdattributeDefinition.IsInternalSubset && text != text2)
						{
							this.HandleError("In standalone document, attribute value characters must not be checked against external definition.", XmlSeverityType.Error);
						}
						if (dtdattributeDefinition.OccurenceType == DTDAttributeOccurenceType.Fixed && text != dtdattributeDefinition.DefaultValue)
						{
							this.HandleError(string.Format("Fixed attribute {0} in element {1} has invalid value {2}.", dtdattributeDefinition.Name, decl.Name, text), XmlSeverityType.Error);
							continue;
						}
						continue;
						try
						{
							IL_2D6:
							dtdattributeDefinition.Datatype.ParseValue(text2, this.NameTable, null);
						}
						catch (Exception ex)
						{
							this.HandleError(string.Format("Attribute value is invalid against its data type '{0}'. {1}", dtdattributeDefinition.Datatype, ex.Message), XmlSeverityType.Error);
						}
						goto IL_31C;
					}
					this.HandleError(string.Format("Attribute {0} is not declared.", this.reader.Name), XmlSeverityType.Error);
				}
			}
			if (validate)
			{
				this.VerifyDeclaredAttributes(decl);
			}
			this.MoveToElement();
		}

		private void ReadDoctype()
		{
			this.FillAttributes();
			IHasXmlParserContext hasXmlParserContext = this.reader;
			if (hasXmlParserContext != null)
			{
				this.dtd = hasXmlParserContext.ParserContext.Dtd;
			}
			if (this.dtd == null)
			{
				Mono.Xml2.XmlTextReader xmlTextReader = new Mono.Xml2.XmlTextReader(string.Empty, XmlNodeType.Document, null);
				xmlTextReader.XmlResolver = this.resolver;
				xmlTextReader.GenerateDTDObjectModel(this.reader.Name, this.reader["PUBLIC"], this.reader["SYSTEM"], this.reader.Value);
				this.dtd = xmlTextReader.DTD;
			}
			this.currentAutomata = this.dtd.RootAutomata;
			for (int i = 0; i < this.DTD.Errors.Length; i++)
			{
				this.HandleError(this.DTD.Errors[i].Message, XmlSeverityType.Error);
			}
			foreach (DTDNode dtdnode in this.dtd.EntityDecls.Values)
			{
				DTDEntityDeclaration dtdentityDeclaration = (DTDEntityDeclaration)dtdnode;
				if (dtdentityDeclaration.NotationName != null && this.dtd.NotationDecls[dtdentityDeclaration.NotationName] == null)
				{
					this.HandleError("Target notation was not found for NData in entity declaration " + dtdentityDeclaration.Name + ".", XmlSeverityType.Error);
				}
			}
			foreach (DTDNode dtdnode2 in this.dtd.AttListDecls.Values)
			{
				DTDAttListDeclaration dtdattListDeclaration = (DTDAttListDeclaration)dtdnode2;
				foreach (object obj in dtdattListDeclaration.Definitions)
				{
					DTDAttributeDefinition dtdattributeDefinition = (DTDAttributeDefinition)obj;
					if (dtdattributeDefinition.Datatype.TokenizedType == XmlTokenizedType.NOTATION)
					{
						foreach (object obj2 in dtdattributeDefinition.EnumeratedNotations)
						{
							string name = (string)obj2;
							if (this.dtd.NotationDecls[name] == null)
							{
								this.HandleError("Target notation was not found for NOTATION typed attribute default " + dtdattributeDefinition.Name + ".", XmlSeverityType.Error);
							}
						}
					}
				}
			}
		}

		private void ProcessStartElement()
		{
			this.nsmgr.PushScope();
			this.popScope = this.reader.IsEmptyElement;
			this.elementStack.Push(this.reader.Name);
			this.currentElement = this.Name;
			if (this.currentAutomata == null)
			{
				this.ValidateAttributes(null, false);
				if (this.reader.IsEmptyElement)
				{
					this.ProcessEndElement();
				}
				return;
			}
			this.previousAutomata = this.currentAutomata;
			this.currentAutomata = this.currentAutomata.TryStartElement(this.reader.Name);
			if (this.currentAutomata == this.DTD.Invalid)
			{
				this.HandleError(string.Format("Invalid start element found: {0}", this.reader.Name), XmlSeverityType.Error);
				this.currentAutomata = this.previousAutomata;
			}
			DTDElementDeclaration dtdelementDeclaration = this.DTD.ElementDecls[this.reader.Name];
			if (dtdelementDeclaration == null)
			{
				this.HandleError(string.Format("Element {0} is not declared.", this.reader.Name), XmlSeverityType.Error);
				this.currentAutomata = this.previousAutomata;
			}
			this.automataStack.Push(this.currentAutomata);
			if (dtdelementDeclaration != null)
			{
				this.currentAutomata = dtdelementDeclaration.ContentModel.GetAutomata();
			}
			DTDAttListDeclaration dtdattListDeclaration = this.dtd.AttListDecls[this.currentElement];
			if (dtdattListDeclaration != null)
			{
				this.ValidateAttributes(dtdattListDeclaration, true);
				this.currentAttribute = -1;
			}
			else
			{
				if (this.reader.HasAttributes)
				{
					this.HandleError(string.Format("Attributes are found on element {0} while it has no attribute definitions.", this.currentElement), XmlSeverityType.Error);
				}
				this.ValidateAttributes(null, false);
			}
			if (this.reader.IsEmptyElement)
			{
				this.ProcessEndElement();
			}
		}

		private void ProcessEndElement()
		{
			this.popScope = true;
			this.elementStack.Pop();
			if (this.currentAutomata == null)
			{
				return;
			}
			if (this.DTD.ElementDecls[this.reader.Name] == null)
			{
				this.HandleError(string.Format("Element {0} is not declared.", this.reader.Name), XmlSeverityType.Error);
			}
			this.previousAutomata = this.currentAutomata;
			DTDAutomata dtdautomata = this.currentAutomata.TryEndElement();
			if (dtdautomata == this.DTD.Invalid)
			{
				this.HandleError(string.Format("Invalid end element found: {0}", this.reader.Name), XmlSeverityType.Error);
				this.currentAutomata = this.previousAutomata;
			}
			this.currentAutomata = (this.automataStack.Pop() as DTDAutomata);
		}

		private void VerifyDeclaredAttributes(DTDAttListDeclaration decl)
		{
			for (int i = 0; i < decl.Definitions.Count; i++)
			{
				DTDAttributeDefinition dtdattributeDefinition = (DTDAttributeDefinition)decl.Definitions[i];
				bool flag = false;
				for (int j = 0; j < this.attributeCount; j++)
				{
					if (this.attributes[j].Name == dtdattributeDefinition.Name)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (dtdattributeDefinition.OccurenceType == DTDAttributeOccurenceType.Required)
					{
						this.HandleError(string.Format("Required attribute {0} in element {1} not found .", dtdattributeDefinition.Name, decl.Name), XmlSeverityType.Error);
					}
					else if (dtdattributeDefinition.DefaultValue != null)
					{
						if (this.isStandalone && !dtdattributeDefinition.IsInternalSubset)
						{
							this.HandleError("In standalone document, external default value definition must not be applied.", XmlSeverityType.Error);
						}
						switch (this.validatingReader.ValidationType)
						{
						case ValidationType.None:
						case ValidationType.DTD:
							break;
						case ValidationType.Auto:
							if (this.validatingReader.Schemas.Count != 0)
							{
								goto IL_197;
							}
							break;
						default:
							goto IL_197;
						}
						DTDValidatingReader.AttributeSlot attributeSlot = this.GetAttributeSlot();
						attributeSlot.Name = dtdattributeDefinition.Name;
						int num = dtdattributeDefinition.Name.IndexOf(':');
						attributeSlot.LocalName = ((num >= 0) ? dtdattributeDefinition.Name.Substring(num + 1) : dtdattributeDefinition.Name);
						string prefix = (num >= 0) ? dtdattributeDefinition.Name.Substring(0, num) : string.Empty;
						attributeSlot.Prefix = prefix;
						attributeSlot.Value = dtdattributeDefinition.DefaultValue;
						attributeSlot.IsDefault = true;
					}
				}
				IL_197:;
			}
		}

		public override bool ReadAttributeValue()
		{
			if (this.consumedAttribute)
			{
				return false;
			}
			if (this.NodeType == XmlNodeType.Attribute && this.EntityHandling == EntityHandling.ExpandEntities)
			{
				this.consumedAttribute = true;
				return true;
			}
			if (this.IsDefault)
			{
				this.consumedAttribute = true;
				return true;
			}
			return this.reader.ReadAttributeValue();
		}

		public override void ResolveEntity()
		{
			this.reader.ResolveEntity();
		}

		public override int AttributeCount
		{
			get
			{
				if (this.currentTextValue != null)
				{
					return 0;
				}
				return this.attributeCount;
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
				return true;
			}
		}

		public override int Depth
		{
			get
			{
				int num = this.reader.Depth;
				if (this.currentTextValue != null && this.reader.NodeType == XmlNodeType.EndElement)
				{
					num++;
				}
				return (!this.IsDefault) ? num : (num + 1);
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
				return this.currentAttribute >= 0 || this.currentTextValue != null || this.reader.HasValue;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.currentTextValue == null && this.currentAttribute != -1 && this.attributes[this.currentAttribute].IsDefault;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.currentTextValue == null && this.reader.IsEmptyElement;
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

		public override string this[string name, string ns]
		{
			get
			{
				return this.GetAttribute(name, ns);
			}
		}

		public int LineNumber
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.reader;
				return (xmlLineInfo == null) ? 0 : xmlLineInfo.LineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				IXmlLineInfo xmlLineInfo = this.reader;
				return (xmlLineInfo == null) ? 0 : xmlLineInfo.LinePosition;
			}
		}

		public override string LocalName
		{
			get
			{
				if (this.currentTextValue != null || this.consumedAttribute)
				{
					return string.Empty;
				}
				if (this.NodeType == XmlNodeType.Attribute)
				{
					return this.attributes[this.currentAttribute].LocalName;
				}
				return this.reader.LocalName;
			}
		}

		public override string Name
		{
			get
			{
				if (this.currentTextValue != null || this.consumedAttribute)
				{
					return string.Empty;
				}
				if (this.NodeType == XmlNodeType.Attribute)
				{
					return this.attributes[this.currentAttribute].Name;
				}
				return this.reader.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				if (this.currentTextValue != null || this.consumedAttribute)
				{
					return string.Empty;
				}
				XmlNodeType nodeType = this.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType == XmlNodeType.Attribute)
					{
						return this.attributes[this.currentAttribute].NS;
					}
					if (nodeType != XmlNodeType.EndElement)
					{
						return string.Empty;
					}
				}
				return this.nsmgr.LookupNamespace(this.Prefix);
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
				if (this.currentTextValue != null)
				{
					return (!this.isSignificantWhitespace) ? ((!this.isWhitespace) ? XmlNodeType.Text : XmlNodeType.Whitespace) : XmlNodeType.SignificantWhitespace;
				}
				return (!this.consumedAttribute) ? ((!this.IsDefault) ? this.reader.NodeType : XmlNodeType.Attribute) : XmlNodeType.Text;
			}
		}

		public XmlParserContext ParserContext
		{
			get
			{
				return XmlSchemaUtil.GetParserContext(this.reader);
			}
		}

		public override string Prefix
		{
			get
			{
				if (this.currentTextValue != null || this.consumedAttribute)
				{
					return string.Empty;
				}
				if (this.NodeType == XmlNodeType.Attribute)
				{
					return this.attributes[this.currentAttribute].Prefix;
				}
				return this.reader.Prefix;
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
				if (this.reader.ReadState == ReadState.EndOfFile && this.currentTextValue != null)
				{
					return ReadState.Interactive;
				}
				return this.reader.ReadState;
			}
		}

		public object SchemaType
		{
			get
			{
				if (this.DTD == null || this.currentAttribute == -1 || this.currentElement == null)
				{
					return null;
				}
				DTDAttListDeclaration dtdattListDeclaration = this.DTD.AttListDecls[this.currentElement];
				DTDAttributeDefinition dtdattributeDefinition = (dtdattListDeclaration == null) ? null : dtdattListDeclaration[this.attributes[this.currentAttribute].Name];
				return (dtdattributeDefinition == null) ? null : dtdattributeDefinition.Datatype;
			}
		}

		private string FilterNormalization(string attrName, string rawValue)
		{
			if (this.DTD == null || this.sourceTextReader == null || !this.sourceTextReader.Normalization)
			{
				return rawValue;
			}
			DTDAttributeDefinition dtdattributeDefinition = this.dtd.AttListDecls[this.currentElement].Get(attrName);
			this.valueBuilder.Append(rawValue);
			this.valueBuilder.Replace('\r', ' ');
			this.valueBuilder.Replace('\n', ' ');
			this.valueBuilder.Replace('\t', ' ');
			string result;
			try
			{
				if (dtdattributeDefinition == null || dtdattributeDefinition.Datatype.TokenizedType == XmlTokenizedType.CDATA)
				{
					result = this.valueBuilder.ToString();
				}
				else
				{
					for (int i = 0; i < this.valueBuilder.Length; i++)
					{
						if (this.valueBuilder[i] == ' ')
						{
							while (++i < this.valueBuilder.Length && this.valueBuilder[i] == ' ')
							{
								this.valueBuilder.Remove(i, 1);
							}
						}
					}
					result = this.valueBuilder.ToString().Trim(this.whitespaceChars);
				}
			}
			finally
			{
				this.valueBuilder.Length = 0;
			}
			return result;
		}

		public override string Value
		{
			get
			{
				if (this.currentTextValue != null)
				{
					return this.currentTextValue;
				}
				if (this.NodeType == XmlNodeType.Attribute || this.consumedAttribute)
				{
					return this.attributes[this.currentAttribute].Value;
				}
				return this.reader.Value;
			}
		}

		public override string XmlLang
		{
			get
			{
				string text = this["xml:lang"];
				return (text == null) ? this.reader.XmlLang : text;
			}
		}

		internal XmlResolver Resolver
		{
			get
			{
				return this.resolver;
			}
		}

		public XmlResolver XmlResolver
		{
			set
			{
				if (this.dtd != null)
				{
					this.dtd.XmlResolver = value;
				}
				this.resolver = value;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				string text = this["xml:space"];
				string text2 = text;
				if (text2 != null)
				{
					if (DTDValidatingReader.<>f__switch$map43 == null)
					{
						DTDValidatingReader.<>f__switch$map43 = new Dictionary<string, int>(2)
						{
							{
								"preserve",
								0
							},
							{
								"default",
								1
							}
						};
					}
					int num;
					if (DTDValidatingReader.<>f__switch$map43.TryGetValue(text2, out num))
					{
						if (num == 0)
						{
							return XmlSpace.Preserve;
						}
						if (num == 1)
						{
							return XmlSpace.Default;
						}
					}
				}
				return this.reader.XmlSpace;
			}
		}

		private class AttributeSlot
		{
			public string Name;

			public string LocalName;

			public string NS;

			public string Prefix;

			public string Value;

			public bool IsDefault;

			public void Clear()
			{
				this.Prefix = string.Empty;
				this.LocalName = string.Empty;
				this.NS = string.Empty;
				this.Value = string.Empty;
				this.IsDefault = false;
			}
		}
	}
}
