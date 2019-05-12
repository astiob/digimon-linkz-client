using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the redefine element from XML Schema as specified by the World Wide Web Consortium (W3C). This class can be used to allow simple and complex types, groups and attribute groups from external schema files to be redefined in the current schema. This class can also be used to provide versioning for the schema elements.</summary>
	public class XmlSchemaRedefine : XmlSchemaExternal
	{
		private const string xmlname = "redefine";

		private XmlSchemaObjectTable attributeGroups;

		private XmlSchemaObjectTable groups;

		private XmlSchemaObjectCollection items;

		private XmlSchemaObjectTable schemaTypes;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaRedefine" /> class.</summary>
		public XmlSchemaRedefine()
		{
			this.attributeGroups = new XmlSchemaObjectTable();
			this.groups = new XmlSchemaObjectTable();
			this.items = new XmlSchemaObjectCollection(this);
			this.schemaTypes = new XmlSchemaObjectTable();
		}

		/// <summary>Gets the collection of the following classes: <see cref="T:System.Xml.Schema.XmlSchemaAnnotation" />, <see cref="T:System.Xml.Schema.XmlSchemaAttributeGroup" />, <see cref="T:System.Xml.Schema.XmlSchemaComplexType" />, <see cref="T:System.Xml.Schema.XmlSchemaSimpleType" />, and <see cref="T:System.Xml.Schema.XmlSchemaGroup" />.</summary>
		/// <returns>The elements contained within the redefine element.</returns>
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		[XmlElement("group", typeof(XmlSchemaGroup))]
		[XmlElement("complexType", typeof(XmlSchemaComplexType))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
		[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
		public XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> , for all attributes in the schema, which holds the post-compilation value of the AttributeGroups property.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> for all attributes in the schema. The post-compilation value of the AttributeGroups property.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable AttributeGroups
		{
			get
			{
				return this.attributeGroups;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />, for all simple and complex types in the schema, which holds the post-compilation value of the SchemaTypes property.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> for all schema types in the schema. The post-compilation value of the SchemaTypes property.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable SchemaTypes
		{
			get
			{
				return this.schemaTypes;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" />, for all groups in the schema, which holds the post-compilation value of the Groups property.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaObjectTable" /> for all groups in the schema. The post-compilation value of the Groups property.</returns>
		[XmlIgnore]
		public XmlSchemaObjectTable Groups
		{
			get
			{
				return this.groups;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			foreach (XmlSchemaObject xmlSchemaObject in this.Items)
			{
				xmlSchemaObject.SetParent(this);
				xmlSchemaObject.isRedefinedComponent = true;
				xmlSchemaObject.isRedefineChild = true;
			}
		}

		internal static XmlSchemaRedefine Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaRedefine xmlSchemaRedefine = new XmlSchemaRedefine();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "redefine")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaRedefine.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaRedefine.LineNumber = reader.LineNumber;
			xmlSchemaRedefine.LinePosition = reader.LinePosition;
			xmlSchemaRedefine.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaRedefine.Id = reader.Value;
				}
				else if (reader.Name == "schemaLocation")
				{
					xmlSchemaRedefine.SchemaLocation = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for redefine", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaRedefine);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaRedefine;
			}
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "redefine")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaRedefine.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (reader.LocalName == "annotation")
				{
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaRedefine.items.Add(xmlSchemaAnnotation);
					}
				}
				else if (reader.LocalName == "simpleType")
				{
					XmlSchemaSimpleType xmlSchemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
					if (xmlSchemaSimpleType != null)
					{
						xmlSchemaRedefine.items.Add(xmlSchemaSimpleType);
					}
				}
				else if (reader.LocalName == "complexType")
				{
					XmlSchemaComplexType xmlSchemaComplexType = XmlSchemaComplexType.Read(reader, h);
					if (xmlSchemaComplexType != null)
					{
						xmlSchemaRedefine.items.Add(xmlSchemaComplexType);
					}
				}
				else if (reader.LocalName == "group")
				{
					XmlSchemaGroup xmlSchemaGroup = XmlSchemaGroup.Read(reader, h);
					if (xmlSchemaGroup != null)
					{
						xmlSchemaRedefine.items.Add(xmlSchemaGroup);
					}
				}
				else if (reader.LocalName == "attributeGroup")
				{
					XmlSchemaAttributeGroup xmlSchemaAttributeGroup = XmlSchemaAttributeGroup.Read(reader, h);
					if (xmlSchemaAttributeGroup != null)
					{
						xmlSchemaRedefine.items.Add(xmlSchemaAttributeGroup);
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaRedefine;
		}
	}
}
