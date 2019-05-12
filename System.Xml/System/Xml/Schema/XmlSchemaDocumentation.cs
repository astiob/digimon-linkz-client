using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the documentation element from XML Schema as specified by the World Wide Web Consortium (W3C). This class specifies information to be read or used by humans within an annotation.</summary>
	public class XmlSchemaDocumentation : XmlSchemaObject
	{
		private string language;

		private XmlNode[] markup;

		private string source;

		/// <summary>Gets or sets an array of XmlNodes that represents the documentation child nodes.</summary>
		/// <returns>The array that represents the documentation child nodes.</returns>
		[XmlAnyElement]
		[XmlText]
		public XmlNode[] Markup
		{
			get
			{
				return this.markup;
			}
			set
			{
				this.markup = value;
			}
		}

		/// <summary>Gets or sets the Uniform Resource Identifier (URI) source of the information.</summary>
		/// <returns>A URI reference. The default is String.Empty.Optional.</returns>
		[XmlAttribute("source", DataType = "anyURI")]
		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				this.source = value;
			}
		}

		/// <summary>Gets or sets the xml:lang attribute. This serves as an indicator of the language used in the contents.</summary>
		/// <returns>The xml:lang attribute.Optional.</returns>
		[XmlAttribute("xml:lang")]
		public string Language
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = value;
			}
		}

		internal static XmlSchemaDocumentation Read(XmlSchemaReader reader, ValidationEventHandler h, out bool skip)
		{
			skip = false;
			XmlSchemaDocumentation xmlSchemaDocumentation = new XmlSchemaDocumentation();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "documentation")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaDocumentation.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaDocumentation.LineNumber = reader.LineNumber;
			xmlSchemaDocumentation.LinePosition = reader.LinePosition;
			xmlSchemaDocumentation.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "source")
				{
					xmlSchemaDocumentation.source = reader.Value;
				}
				else if (reader.Name == "xml:lang")
				{
					xmlSchemaDocumentation.language = reader.Value;
				}
				else
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for documentation", null);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				xmlSchemaDocumentation.Markup = new XmlNode[0];
				return xmlSchemaDocumentation;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.ReadNode(reader));
			XmlNode firstChild = xmlDocument.FirstChild;
			if (firstChild != null && firstChild.ChildNodes != null)
			{
				xmlSchemaDocumentation.Markup = new XmlNode[firstChild.ChildNodes.Count];
				for (int i = 0; i < firstChild.ChildNodes.Count; i++)
				{
					xmlSchemaDocumentation.Markup[i] = firstChild.ChildNodes[i];
				}
			}
			if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.EndElement)
			{
				skip = true;
			}
			return xmlSchemaDocumentation;
		}
	}
}
