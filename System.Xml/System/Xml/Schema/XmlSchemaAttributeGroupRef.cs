using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the attributeGroup element with the ref attribute from the XML Schema as specified by the World Wide Web Consortium (W3C). AttributesGroupRef is the reference for an attributeGroup, name property contains the attribute group being referenced. </summary>
	public class XmlSchemaAttributeGroupRef : XmlSchemaAnnotated
	{
		private const string xmlname = "attributeGroup";

		private XmlQualifiedName refName;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaAttributeGroupRef" /> class.</summary>
		public XmlSchemaAttributeGroupRef()
		{
			this.refName = XmlQualifiedName.Empty;
		}

		/// <summary>Gets or sets the name of the referenced attributeGroup element.</summary>
		/// <returns>The name of the referenced attribute group. The value must be a QName.</returns>
		[XmlAttribute("ref")]
		public XmlQualifiedName RefName
		{
			get
			{
				return this.refName;
			}
			set
			{
				this.refName = value;
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			this.errorCount = 0;
			if (this.RefName == null || this.RefName.IsEmpty)
			{
				base.error(h, "ref must be present");
			}
			else if (!XmlSchemaUtil.CheckQName(this.RefName))
			{
				base.error(h, "ref must be a valid qname");
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			return this.errorCount;
		}

		internal static XmlSchemaAttributeGroupRef Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = new XmlSchemaAttributeGroupRef();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "attributeGroup")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAttributeGroupRef.Read, name=" + reader.Name, null);
				reader.SkipToEnd();
				return null;
			}
			xmlSchemaAttributeGroupRef.LineNumber = reader.LineNumber;
			xmlSchemaAttributeGroupRef.LinePosition = reader.LinePosition;
			xmlSchemaAttributeGroupRef.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaAttributeGroupRef.Id = reader.Value;
				}
				else if (reader.Name == "ref")
				{
					Exception ex;
					xmlSchemaAttributeGroupRef.refName = XmlSchemaUtil.ReadQNameAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for ref attribute", ex);
					}
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for attributeGroup in this context", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaAttributeGroupRef);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaAttributeGroupRef;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "attributeGroup")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAttributeGroupRef.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaAttributeGroupRef.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaAttributeGroupRef;
		}
	}
}
