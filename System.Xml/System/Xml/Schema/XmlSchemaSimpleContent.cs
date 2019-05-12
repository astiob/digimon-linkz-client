using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the simpleContent element from XML Schema as specified by the World Wide Web Consortium (W3C). This class is for simple and complex types with simple content model.</summary>
	public class XmlSchemaSimpleContent : XmlSchemaContentModel
	{
		private const string xmlname = "simpleContent";

		private XmlSchemaContent content;

		/// <summary>Gets one of the <see cref="T:System.Xml.Schema.XmlSchemaSimpleContentRestriction" /> or <see cref="T:System.Xml.Schema.XmlSchemaSimpleContentExtension" />.</summary>
		/// <returns>The content contained within the XmlSchemaSimpleContentRestriction or XmlSchemaSimpleContentExtension class.</returns>
		[XmlElement("restriction", typeof(XmlSchemaSimpleContentRestriction))]
		[XmlElement("extension", typeof(XmlSchemaSimpleContentExtension))]
		public override XmlSchemaContent Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		internal override void SetParent(XmlSchemaObject parent)
		{
			base.SetParent(parent);
			if (this.Content != null)
			{
				this.Content.SetParent(this);
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			if (this.Content == null)
			{
				base.error(h, "Content must be present in a simpleContent");
			}
			else if (this.Content is XmlSchemaSimpleContentRestriction)
			{
				XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = (XmlSchemaSimpleContentRestriction)this.Content;
				this.errorCount += xmlSchemaSimpleContentRestriction.Compile(h, schema);
			}
			else if (this.Content is XmlSchemaSimpleContentExtension)
			{
				XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = (XmlSchemaSimpleContentExtension)this.Content;
				this.errorCount += xmlSchemaSimpleContentExtension.Compile(h, schema);
			}
			else
			{
				base.error(h, "simpleContent can't have any value other than restriction or extention");
			}
			XmlSchemaUtil.CompileID(base.Id, this, schema.IDCollection, h);
			this.CompilationId = schema.CompilationId;
			return this.errorCount;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			if (base.IsValidated(schema.ValidationId))
			{
				return this.errorCount;
			}
			this.errorCount += this.Content.Validate(h, schema);
			this.ValidationId = schema.ValidationId;
			return this.errorCount;
		}

		internal static XmlSchemaSimpleContent Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaSimpleContent xmlSchemaSimpleContent = new XmlSchemaSimpleContent();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "simpleContent")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexContent.Read, name=" + reader.Name, null);
				reader.SkipToEnd();
				return null;
			}
			xmlSchemaSimpleContent.LineNumber = reader.LineNumber;
			xmlSchemaSimpleContent.LinePosition = reader.LinePosition;
			xmlSchemaSimpleContent.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaSimpleContent.Id = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for simpleContent", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaSimpleContent);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaSimpleContent;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "simpleContent")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleContent.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaSimpleContent.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					if (num <= 2)
					{
						if (reader.LocalName == "restriction")
						{
							num = 3;
							XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = XmlSchemaSimpleContentRestriction.Read(reader, h);
							if (xmlSchemaSimpleContentRestriction != null)
							{
								xmlSchemaSimpleContent.content = xmlSchemaSimpleContentRestriction;
							}
							continue;
						}
						if (reader.LocalName == "extension")
						{
							num = 3;
							XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = XmlSchemaSimpleContentExtension.Read(reader, h);
							if (xmlSchemaSimpleContentExtension != null)
							{
								xmlSchemaSimpleContent.content = xmlSchemaSimpleContentExtension;
							}
							continue;
						}
					}
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaSimpleContent;
		}
	}
}
