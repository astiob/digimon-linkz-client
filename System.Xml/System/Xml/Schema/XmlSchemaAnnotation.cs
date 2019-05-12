using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the World Wide Web Consortium (W3C) annotation element.</summary>
	public class XmlSchemaAnnotation : XmlSchemaObject
	{
		private const string xmlname = "annotation";

		private string id;

		private XmlSchemaObjectCollection items;

		private XmlAttribute[] unhandledAttributes;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaAnnotation" /> class.</summary>
		public XmlSchemaAnnotation()
		{
			this.items = new XmlSchemaObjectCollection();
		}

		/// <summary>Gets or sets the string id.</summary>
		/// <returns>The string id. The default is String.Empty.Optional.</returns>
		[XmlAttribute("id", DataType = "ID")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		/// <summary>Gets the Items collection that is used to store the appinfo and documentation child elements.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaObjectCollection" /> of appinfo and documentation child elements.</returns>
		[XmlElement("appinfo", typeof(XmlSchemaAppInfo))]
		[XmlElement("documentation", typeof(XmlSchemaDocumentation))]
		public XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets or sets the qualified attributes that do not belong to the schema's target namespace.</summary>
		/// <returns>An array of <see cref="T:System.Xml.XmlAttribute" /> objects that do not belong to the schema's target namespace.</returns>
		[XmlAnyAttribute]
		public XmlAttribute[] UnhandledAttributes
		{
			get
			{
				if (this.unhandledAttributeList != null)
				{
					this.unhandledAttributes = (XmlAttribute[])this.unhandledAttributeList.ToArray(typeof(XmlAttribute));
					this.unhandledAttributeList = null;
				}
				return this.unhandledAttributes;
			}
			set
			{
				this.unhandledAttributes = value;
				this.unhandledAttributeList = null;
			}
		}

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			if (this.CompilationId == schema.CompilationId)
			{
				return 0;
			}
			this.CompilationId = schema.CompilationId;
			return 0;
		}

		internal override int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			return 0;
		}

		internal static XmlSchemaAnnotation Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaAnnotation xmlSchemaAnnotation = new XmlSchemaAnnotation();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "annotation")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAnnotation.Read, name=" + reader.Name, null);
				reader.SkipToEnd();
				return null;
			}
			xmlSchemaAnnotation.LineNumber = reader.LineNumber;
			xmlSchemaAnnotation.LinePosition = reader.LinePosition;
			xmlSchemaAnnotation.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaAnnotation.Id = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for annotation", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaAnnotation);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaAnnotation;
			}
			bool flag = false;
			string text = null;
			while (!reader.EOF)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					reader.ReadNextElement();
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					bool flag2 = true;
					string text2 = "annotation";
					if (text != null)
					{
						text2 = text;
						text = null;
						flag2 = false;
					}
					if (reader.LocalName != text2)
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAnnotation.Read, name=" + reader.Name + ",expected=" + text2, null);
					}
					if (flag2)
					{
						break;
					}
				}
				else if (reader.LocalName == "appinfo")
				{
					XmlSchemaAppInfo xmlSchemaAppInfo = XmlSchemaAppInfo.Read(reader, h, out flag);
					if (xmlSchemaAppInfo != null)
					{
						xmlSchemaAnnotation.items.Add(xmlSchemaAppInfo);
					}
				}
				else if (reader.LocalName == "documentation")
				{
					XmlSchemaDocumentation xmlSchemaDocumentation = XmlSchemaDocumentation.Read(reader, h, out flag);
					if (xmlSchemaDocumentation != null)
					{
						xmlSchemaAnnotation.items.Add(xmlSchemaDocumentation);
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaAnnotation;
		}
	}
}
