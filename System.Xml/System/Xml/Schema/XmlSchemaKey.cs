using System;

namespace System.Xml.Schema
{
	/// <summary>This class represents the key element from XMLSchema as specified by the World Wide Web Consortium (W3C).</summary>
	public class XmlSchemaKey : XmlSchemaIdentityConstraint
	{
		private const string xmlname = "key";

		internal override int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			return base.Compile(h, schema);
		}

		internal static XmlSchemaKey Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaKey xmlSchemaKey = new XmlSchemaKey();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "key")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaKey.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaKey.LineNumber = reader.LineNumber;
			xmlSchemaKey.LinePosition = reader.LinePosition;
			xmlSchemaKey.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaKey.Id = reader.Value;
				}
				else if (reader.Name == "name")
				{
					xmlSchemaKey.Name = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for key", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaKey);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaKey;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "key")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaKey.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaKey.Annotation = xmlSchemaAnnotation;
					}
				}
				else if (num <= 2 && reader.LocalName == "selector")
				{
					num = 3;
					XmlSchemaXPath xmlSchemaXPath = XmlSchemaXPath.Read(reader, h, "selector");
					if (xmlSchemaXPath != null)
					{
						xmlSchemaKey.Selector = xmlSchemaXPath;
					}
				}
				else if (num <= 3 && reader.LocalName == "field")
				{
					num = 3;
					if (xmlSchemaKey.Selector == null)
					{
						XmlSchemaObject.error(h, "selector must be defined before field declarations", null);
					}
					XmlSchemaXPath xmlSchemaXPath2 = XmlSchemaXPath.Read(reader, h, "field");
					if (xmlSchemaXPath2 != null)
					{
						xmlSchemaKey.Fields.Add(xmlSchemaXPath2);
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaKey;
		}
	}
}
