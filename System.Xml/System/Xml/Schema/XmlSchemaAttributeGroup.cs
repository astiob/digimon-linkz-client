using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the attributeGroup element from the XML Schema as specified by the World Wide Web Consortium (W3C). AttributesGroups provides a mechanism to group a set of attribute declarations so that they can be incorporated as a group into complex type definitions.</summary>
	public class XmlSchemaAttributeGroup : XmlSchemaAnnotated
	{
		private const string xmlname = "attributeGroup";

		private XmlSchemaAnyAttribute anyAttribute;

		private XmlSchemaObjectCollection attributes;

		private string name;

		private XmlSchemaAttributeGroup redefined;

		private XmlQualifiedName qualifiedName;

		private XmlSchemaObjectTable attributeUses;

		private XmlSchemaAnyAttribute anyAttributeUse;

		internal bool AttributeGroupRecursionCheck;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaAttributeGroup" /> class.</summary>
		public XmlSchemaAttributeGroup()
		{
			this.attributes = new XmlSchemaObjectCollection();
			this.qualifiedName = XmlQualifiedName.Empty;
		}

		/// <summary>Gets or sets the name of the attribute group.</summary>
		/// <returns>The name of the attribute group.</returns>
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>Gets the collection of attributes for the attribute group. Contains XmlSchemaAttribute and XmlSchemaAttributeGroupRef elements.</summary>
		/// <returns>The collection of attributes for the attribute group.</returns>
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		internal XmlSchemaObjectTable AttributeUses
		{
			get
			{
				return this.attributeUses;
			}
		}

		internal XmlSchemaAnyAttribute AnyAttributeUse
		{
			get
			{
				return this.anyAttributeUse;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaAnyAttribute" /> component of the attribute group.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaAnyAttribute" />.</returns>
		[XmlElement("anyAttribute")]
		public XmlSchemaAnyAttribute AnyAttribute
		{
			get
			{
				return this.anyAttribute;
			}
			set
			{
				this.anyAttribute = value;
			}
		}

		/// <summary>Gets the redefined attribute group property from the XML Schema.</summary>
		/// <returns>The redefined attribute group property.</returns>
		[XmlIgnore]
		public XmlSchemaAttributeGroup RedefinedAttributeGroup
		{
			get
			{
				return this.redefined;
			}
		}

		/// <summary>Gets the qualified name of the attribute group.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlQualifiedName" /> object representing the qualified name of the attribute group.</returns>
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qualifiedName;
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
				return this.errorCount;
			}
			this.errorCount = 0;
			if (this.redefinedObject != null)
			{
				this.errorCount += this.redefined.Compile(h, schema);
				if (this.errorCount == 0)
				{
					this.redefined = (XmlSchemaAttributeGroup)this.redefinedObject;
				}
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			if (this.Name == null || this.Name == string.Empty)
			{
				base.error(h, "Name is required in top level simpletype");
			}
			else if (!XmlSchemaUtil.CheckNCName(this.Name))
			{
				base.error(h, "name attribute of a simpleType must be NCName");
			}
			else
			{
				this.qualifiedName = new XmlQualifiedName(this.Name, base.AncestorSchema.TargetNamespace);
			}
			if (this.AnyAttribute != null)
			{
				this.errorCount += this.AnyAttribute.Compile(h, schema);
			}
			foreach (XmlSchemaObject xmlSchemaObject in this.Attributes)
			{
				if (xmlSchemaObject is XmlSchemaAttribute)
				{
					XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
					this.errorCount += xmlSchemaAttribute.Compile(h, schema);
				}
				else if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
				{
					XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)xmlSchemaObject;
					this.errorCount += xmlSchemaAttributeGroupRef.Compile(h, schema);
				}
				else
				{
					base.error(h, "invalid type of object in Attributes property");
				}
			}
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.CompilationId))
			{
				return this.errorCount;
			}
			if (this.redefined == null && this.redefinedObject != null)
			{
				this.redefinedObject.Compile(h, schema);
				this.redefined = (XmlSchemaAttributeGroup)this.redefinedObject;
				this.redefined.Validate(h, schema);
			}
			XmlSchemaObjectCollection xmlSchemaObjectCollection = this.Attributes;
			this.attributeUses = new XmlSchemaObjectTable();
			this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, xmlSchemaObjectCollection, this.AnyAttribute, ref this.anyAttributeUse, this.redefined, false);
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		internal static XmlSchemaAttributeGroup Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaAttributeGroup xmlSchemaAttributeGroup = new XmlSchemaAttributeGroup();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "attributeGroup")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAttributeGroup.Read, name=" + reader.Name, null);
				reader.SkipToEnd();
				return null;
			}
			xmlSchemaAttributeGroup.LineNumber = reader.LineNumber;
			xmlSchemaAttributeGroup.LinePosition = reader.LinePosition;
			xmlSchemaAttributeGroup.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaAttributeGroup.Id = reader.Value;
				}
				else if (reader.Name == "name")
				{
					xmlSchemaAttributeGroup.name = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for attributeGroup in this context", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaAttributeGroup);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaAttributeGroup;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "attributeGroup")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAttributeGroup.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaAttributeGroup.Annotation = xmlSchemaAnnotation;
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
								xmlSchemaAttributeGroup.Attributes.Add(xmlSchemaAttribute);
							}
							continue;
						}
						if (reader.LocalName == "attributeGroup")
						{
							num = 2;
							XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = XmlSchemaAttributeGroupRef.Read(reader, h);
							if (xmlSchemaAttributeGroupRef != null)
							{
								xmlSchemaAttributeGroup.attributes.Add(xmlSchemaAttributeGroupRef);
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
							xmlSchemaAttributeGroup.AnyAttribute = xmlSchemaAnyAttribute;
						}
					}
					else
					{
						reader.RaiseInvalidElementError();
					}
				}
			}
			return xmlSchemaAttributeGroup;
		}
	}
}
