using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the notation element from XML Schema as specified by the World Wide Web Consortium (W3C). An XML Schema notation declaration is a reconstruction of XML 1.0 NOTATION declarations. The purpose of notations is to describe the format of non-XML data within an XML document.</summary>
	public class XmlSchemaNotation : XmlSchemaAnnotated
	{
		private const string xmlname = "notation";

		private string name;

		private string pub;

		private string system;

		private XmlQualifiedName qualifiedName;

		/// <summary>Gets or sets the name of the notation.</summary>
		/// <returns>The name of the notation.</returns>
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

		/// <summary>Gets or sets the public identifier.</summary>
		/// <returns>The public identifier. The value must be a valid Uniform Resource Identifier (URI).</returns>
		[XmlAttribute("public")]
		public string Public
		{
			get
			{
				return this.pub;
			}
			set
			{
				this.pub = value;
			}
		}

		/// <summary>Gets or sets the system identifier.</summary>
		/// <returns>The system identifier. The value must be a valid URI.</returns>
		[XmlAttribute("system")]
		public string System
		{
			get
			{
				return this.system;
			}
			set
			{
				this.system = value;
			}
		}

		[XmlIgnore]
		internal XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qualifiedName;
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			if (this.Name == null)
			{
				base.error(h, "Required attribute name must be present");
			}
			else if (!XmlSchemaUtil.CheckNCName(this.name))
			{
				base.error(h, "attribute name must be NCName");
			}
			else
			{
				this.qualifiedName = new XmlQualifiedName(this.Name, base.AncestorSchema.TargetNamespace);
			}
			if (this.Public == null)
			{
				base.error(h, "public must be present");
			}
			else if (!XmlSchemaUtil.CheckAnyUri(this.Public))
			{
				base.error(h, "public must be anyURI");
			}
			if (this.system != null && !XmlSchemaUtil.CheckAnyUri(this.system))
			{
				base.error(h, "system must be present and of Type anyURI");
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			return this.errorCount;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			return this.errorCount;
		}

		internal static XmlSchemaNotation Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaNotation xmlSchemaNotation = new XmlSchemaNotation();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "notation")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaInclude.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaNotation.LineNumber = reader.LineNumber;
			xmlSchemaNotation.LinePosition = reader.LinePosition;
			xmlSchemaNotation.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaNotation.Id = reader.Value;
				}
				else if (reader.Name == "name")
				{
					xmlSchemaNotation.name = reader.Value;
				}
				else if (reader.Name == "public")
				{
					xmlSchemaNotation.pub = reader.Value;
				}
				else if (reader.Name == "system")
				{
					xmlSchemaNotation.system = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for notation", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaNotation);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaNotation;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "notation")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaNotation.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaNotation.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaNotation;
		}
	}
}
