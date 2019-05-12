using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XmlSchemaValidatingReader : XmlReader, IHasXmlParserContext, IXmlSchemaInfo, IXmlLineInfo, IXmlNamespaceResolver
	{
		private static readonly XmlSchemaAttribute[] emptyAttributeArray = new XmlSchemaAttribute[0];

		private XmlReader reader;

		private XmlSchemaValidationFlags options;

		private XmlSchemaValidator v;

		private XmlValueGetter getter;

		private XmlSchemaInfo xsinfo;

		private IXmlLineInfo readerLineInfo;

		private ValidationType validationType;

		private IXmlNamespaceResolver nsResolver;

		private XmlSchemaAttribute[] defaultAttributes = XmlSchemaValidatingReader.emptyAttributeArray;

		private int currentDefaultAttribute = -1;

		private ArrayList defaultAttributesCache = new ArrayList();

		private bool defaultAttributeConsumed;

		private XmlSchemaType currentAttrType;

		private bool validationDone;

		private XmlSchemaElement element;

		public XmlSchemaValidatingReader(XmlReader reader, XmlReaderSettings settings)
		{
			XmlSchemaValidatingReader <>f__this = this;
			IXmlNamespaceResolver xmlNamespaceResolver = reader as IXmlNamespaceResolver;
			if (xmlNamespaceResolver == null)
			{
				xmlNamespaceResolver = new XmlNamespaceManager(reader.NameTable);
			}
			XmlSchemaSet xmlSchemaSet = settings.Schemas;
			if (xmlSchemaSet == null)
			{
				xmlSchemaSet = new XmlSchemaSet();
			}
			this.options = settings.ValidationFlags;
			this.reader = reader;
			this.v = new XmlSchemaValidator(reader.NameTable, xmlSchemaSet, xmlNamespaceResolver, this.options);
			if (reader.BaseURI != string.Empty)
			{
				this.v.SourceUri = new Uri(reader.BaseURI);
			}
			this.readerLineInfo = (reader as IXmlLineInfo);
			this.getter = delegate()
			{
				if (<>f__this.v.CurrentAttributeType != null)
				{
					return <>f__this.v.CurrentAttributeType.ParseValue(<>f__this.Value, <>f__this.NameTable, <>f__this);
				}
				return <>f__this.Value;
			};
			this.xsinfo = new XmlSchemaInfo();
			this.v.LineInfoProvider = this;
			this.v.ValidationEventSender = reader;
			this.nsResolver = xmlNamespaceResolver;
			this.ValidationEventHandler += delegate(object o, ValidationEventArgs e)
			{
				settings.OnValidationError(o, e);
			};
			if (settings != null && settings.Schemas != null)
			{
				this.v.XmlResolver = settings.Schemas.XmlResolver;
			}
			else
			{
				this.v.XmlResolver = new XmlUrlResolver();
			}
			this.v.Initialize();
		}

		public event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.v.ValidationEventHandler += value;
			}
			remove
			{
				this.v.ValidationEventHandler -= value;
			}
		}

		int IXmlLineInfo.LineNumber
		{
			get
			{
				return (this.readerLineInfo == null) ? 0 : this.readerLineInfo.LineNumber;
			}
		}

		int IXmlLineInfo.LinePosition
		{
			get
			{
				return (this.readerLineInfo == null) ? 0 : this.readerLineInfo.LinePosition;
			}
		}

		bool IXmlLineInfo.HasLineInfo()
		{
			return this.readerLineInfo != null && this.readerLineInfo.HasLineInfo();
		}

		public XmlSchemaType ElementSchemaType
		{
			get
			{
				return (this.element == null) ? null : this.element.ElementSchemaType;
			}
		}

		private void ResetStateOnRead()
		{
			this.currentDefaultAttribute = -1;
			this.defaultAttributeConsumed = false;
			this.currentAttrType = null;
			this.defaultAttributes = XmlSchemaValidatingReader.emptyAttributeArray;
			this.v.CurrentAttributeType = null;
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

		public XmlSchemaType SchemaType
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
						return null;
					}
					if (this.currentAttrType == null)
					{
						XmlSchemaComplexType xmlSchemaComplexType = this.ElementSchemaType as XmlSchemaComplexType;
						if (xmlSchemaComplexType != null)
						{
							XmlSchemaAttribute xmlSchemaAttribute = xmlSchemaComplexType.AttributeUses[new XmlQualifiedName(this.LocalName, this.NamespaceURI)] as XmlSchemaAttribute;
							if (xmlSchemaAttribute != null)
							{
								this.currentAttrType = xmlSchemaAttribute.AttributeSchemaType;
							}
							return this.currentAttrType;
						}
					}
					return this.currentAttrType;
				}
				else
				{
					if (this.ElementSchemaType != null)
					{
						return this.ElementSchemaType;
					}
					return null;
				}
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
				if (this.ReadState != ReadState.Initial)
				{
					throw new InvalidOperationException("ValidationType must be set before reading.");
				}
				this.validationType = value;
			}
		}

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			IXmlNamespaceResolver xmlNamespaceResolver = this.reader as IXmlNamespaceResolver;
			if (xmlNamespaceResolver == null)
			{
				throw new NotSupportedException("The input XmlReader does not implement IXmlNamespaceResolver and thus this validating reader cannot collect in-scope namespaces.");
			}
			return xmlNamespaceResolver.GetNamespacesInScope(scope);
		}

		public string LookupPrefix(string ns)
		{
			return this.nsResolver.LookupPrefix(ns);
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
				string text = this.nsResolver.LookupPrefix(qualifiedName.Namespace);
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

		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this;
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
			XmlConvert.VerifyName(name);
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

		public override bool Read()
		{
			if (!this.reader.Read())
			{
				if (!this.validationDone)
				{
					this.v.EndValidation();
					this.validationDone = true;
				}
				return false;
			}
			this.ResetStateOnRead();
			XmlNodeType nodeType = this.reader.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
			{
				string attribute = this.reader.GetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
				string attribute2 = this.reader.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
				string attribute3 = this.reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
				string attribute4 = this.reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
				this.v.ValidateElement(this.reader.LocalName, this.reader.NamespaceURI, this.xsinfo, attribute3, attribute4, attribute, attribute2);
				if (this.reader.MoveToFirstAttribute())
				{
					for (;;)
					{
						string namespaceURI = this.reader.NamespaceURI;
						if (namespaceURI == null)
						{
							goto IL_202;
						}
						if (XmlSchemaValidatingReader.<>f__switch$map1 == null)
						{
							XmlSchemaValidatingReader.<>f__switch$map1 = new Dictionary<string, int>(2)
							{
								{
									"http://www.w3.org/2001/XMLSchema-instance",
									0
								},
								{
									"http://www.w3.org/2000/xmlns/",
									1
								}
							};
						}
						int num;
						if (!XmlSchemaValidatingReader.<>f__switch$map1.TryGetValue(namespaceURI, out num))
						{
							goto IL_202;
						}
						if (num == 0)
						{
							string localName = this.reader.LocalName;
							if (localName != null)
							{
								if (XmlSchemaValidatingReader.<>f__switch$map0 == null)
								{
									XmlSchemaValidatingReader.<>f__switch$map0 = new Dictionary<string, int>(4)
									{
										{
											"schemaLocation",
											0
										},
										{
											"noNamespaceSchemaLocation",
											0
										},
										{
											"nil",
											0
										},
										{
											"type",
											0
										}
									};
								}
								int num2;
								if (XmlSchemaValidatingReader.<>f__switch$map0.TryGetValue(localName, out num2))
								{
									if (num2 == 0)
									{
										goto IL_230;
									}
								}
							}
							goto IL_202;
						}
						if (num != 1)
						{
							goto IL_202;
						}
						IL_230:
						if (!this.reader.MoveToNextAttribute())
						{
							break;
						}
						continue;
						IL_202:
						this.v.ValidateAttribute(this.reader.LocalName, this.reader.NamespaceURI, this.getter, this.xsinfo);
						goto IL_230;
					}
					this.reader.MoveToElement();
				}
				this.v.GetUnspecifiedDefaultAttributes(this.defaultAttributesCache);
				this.defaultAttributes = (XmlSchemaAttribute[])this.defaultAttributesCache.ToArray(typeof(XmlSchemaAttribute));
				this.v.ValidateEndOfAttributes(this.xsinfo);
				this.defaultAttributesCache.Clear();
				if (!this.reader.IsEmptyElement)
				{
					return true;
				}
				break;
			}
			default:
				switch (nodeType)
				{
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					this.v.ValidateWhitespace(this.getter);
					return true;
				case XmlNodeType.EndElement:
					break;
				default:
					return true;
				}
				break;
			case XmlNodeType.Text:
				this.v.ValidateText(this.getter);
				return true;
			}
			this.v.ValidateEndElement(this.xsinfo);
			return true;
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

		public override void ResolveEntity()
		{
			this.reader.ResolveEntity();
		}

		public bool IsNil
		{
			get
			{
				return this.xsinfo.IsNil;
			}
		}

		public XmlSchemaSimpleType MemberType
		{
			get
			{
				return this.xsinfo.MemberType;
			}
		}

		public XmlSchemaAttribute SchemaAttribute
		{
			get
			{
				return this.xsinfo.SchemaAttribute;
			}
		}

		public XmlSchemaElement SchemaElement
		{
			get
			{
				return this.xsinfo.SchemaElement;
			}
		}

		public XmlSchemaValidity Validity
		{
			get
			{
				return this.xsinfo.Validity;
			}
		}
	}
}
