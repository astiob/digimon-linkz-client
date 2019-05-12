using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the extension element for simple content from XML Schema as specified by the World Wide Web Consortium (W3C). This class can be used to derive simple types by extension. Such derivations are used to extend the simple type content of the element by adding attributes.</summary>
	public class XmlSchemaSimpleContentExtension : XmlSchemaContent
	{
		private const string xmlname = "extension";

		private XmlSchemaAnyAttribute any;

		private XmlSchemaObjectCollection attributes;

		private XmlQualifiedName baseTypeName;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaSimpleContentExtension" /> class.</summary>
		public XmlSchemaSimpleContentExtension()
		{
			this.baseTypeName = XmlQualifiedName.Empty;
			this.attributes = new XmlSchemaObjectCollection();
		}

		/// <summary>Gets or sets the name of a built-in data type or simple type from which this type is extended.</summary>
		/// <returns>The base type name.</returns>
		[XmlAttribute("base")]
		public XmlQualifiedName BaseTypeName
		{
			get
			{
				return this.baseTypeName;
			}
			set
			{
				this.baseTypeName = value;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Xml.Schema.XmlSchemaAttribute" /> and <see cref="T:System.Xml.Schema.XmlSchemaAttributeGroupRef" />.</summary>
		/// <returns>The collection of attributes for the simpleType element.</returns>
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		/// <summary>Gets or sets the XmlSchemaAnyAttribute to be used for the attribute value.</summary>
		/// <returns>The XmlSchemaAnyAttribute.Optional.</returns>
		[XmlElement("anyAttribute")]
		public XmlSchemaAnyAttribute AnyAttribute
		{
			get
			{
				return this.any;
			}
			set
			{
				this.any = value;
			}
		}

		internal override bool IsExtension
		{
			get
			{
				return true;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.AnyAttribute != null)
			{
				this.AnyAttribute.SetParent(this);
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Attributes)
			{
				xmlSchemaObject.SetParent(this);
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			if (this.isRedefinedComponent)
			{
				if (base.Annotation != null)
				{
					base.Annotation.isRedefinedComponent = true;
				}
				if (this.AnyAttribute != null)
				{
					this.AnyAttribute.isRedefinedComponent = true;
				}
				foreach (XmlSchemaObject xmlSchemaObject in this.Attributes)
				{
					xmlSchemaObject.isRedefinedComponent = true;
				}
			}
			if (this.BaseTypeName == null || this.BaseTypeName.IsEmpty)
			{
				base.error(h, "base must be present, as a QName");
			}
			else if (!XmlSchemaUtil.CheckQName(this.BaseTypeName))
			{
				base.error(h, "BaseTypeName must be a QName");
			}
			if (this.AnyAttribute != null)
			{
				this.errorCount += this.AnyAttribute.Compile(h, schema);
			}
			foreach (XmlSchemaObject xmlSchemaObject2 in this.Attributes)
			{
				if (xmlSchemaObject2 is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject2;
					this.errorCount += xmlSchemaAttribute.Compile(h, schema);
				}
				else if (xmlSchemaObject2 is XmlSchemaAttributeGroupRef)
				{
					XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)xmlSchemaObject2;
					this.errorCount += xmlSchemaAttributeGroupRef.Compile(h, schema);
				}
				else
				{
					base.error(h, xmlSchemaObject2.GetType() + " is not valid in this place::SimpleConentExtension");
				}
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override XmlQualifiedName GetBaseTypeName()
		{
			return this.baseTypeName;
		}

		internal override XmlSchemaParticle GetParticle()
		{
			return null;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.ValidationId))
			{
				return this.errorCount;
			}
			XmlSchemaType xmlSchemaType = schema.FindSchemaType(this.baseTypeName);
			if (xmlSchemaType != null)
			{
				XmlSchemaComplexType xmlSchemaComplexType = xmlSchemaType as XmlSchemaComplexType;
				if (xmlSchemaComplexType != null && xmlSchemaComplexType.ContentModel is XmlSchemaComplexContent)
				{
					base.error(h, "Specified type is complex type which contains complex content.");
				}
				xmlSchemaType.Validate(h, schema);
				this.actualBaseSchemaType = xmlSchemaType;
			}
			else if (this.baseTypeName == XmlSchemaComplexType.AnyTypeName)
			{
				this.actualBaseSchemaType = XmlSchemaComplexType.AnyType;
			}
			else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.baseTypeName))
			{
				this.actualBaseSchemaType = XmlSchemaDatatype.FromName(this.baseTypeName);
				if (this.actualBaseSchemaType == null)
				{
					base.error(h, "Invalid schema datatype name is specified.");
				}
			}
			else if (!schema.IsNamespaceAbsent(this.baseTypeName.Namespace))
			{
				base.error(h, "Referenced base schema type " + this.baseTypeName + " was not found in the corresponding schema.");
			}
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		internal static XmlSchemaSimpleContentExtension Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = new XmlSchemaSimpleContentExtension();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "extension")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAttributeGroup.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaSimpleContentExtension.LineNumber = reader.LineNumber;
			xmlSchemaSimpleContentExtension.LinePosition = reader.LinePosition;
			xmlSchemaSimpleContentExtension.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "base")
				{
					Exception ex;
					xmlSchemaSimpleContentExtension.baseTypeName = XmlSchemaUtil.ReadQNameAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for base attribute", ex);
					}
				}
				else if (reader.Name == "id")
				{
					xmlSchemaSimpleContentExtension.Id = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for extension in this context", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaSimpleContentExtension);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaSimpleContentExtension;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "extension")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleContentExtension.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaSimpleContentExtension.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					if (num <= 2)
					{
						if (reader.LocalName == "attribute")
						{
							num = 2;
							XmlSchemaAttribute xmlSchemaAttribute = XmlSchemaAttribute.Read(reader, h);
							if (xmlSchemaAttribute != null)
							{
								xmlSchemaSimpleContentExtension.Attributes.Add(xmlSchemaAttribute);
							}
							continue;
						}
						if (reader.LocalName == "attributeGroup")
						{
							num = 2;
							XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = XmlSchemaAttributeGroupRef.Read(reader, h);
							if (xmlSchemaAttributeGroupRef != null)
							{
								xmlSchemaSimpleContentExtension.attributes.Add(xmlSchemaAttributeGroupRef);
							}
							continue;
						}
					}
					if (num <= 3 && reader.LocalName == "anyAttribute")
					{
						num = 4;
						XmlSchemaAnyAttribute xmlSchemaAnyAttribute = XmlSchemaAnyAttribute.Read(reader, h);
						if (xmlSchemaAnyAttribute != null)
						{
							xmlSchemaSimpleContentExtension.AnyAttribute = xmlSchemaAnyAttribute;
						}
					}
					else
					{
						reader.RaiseInvalidElementError();
					}
				}
			}
			return xmlSchemaSimpleContentExtension;
		}
	}
}
