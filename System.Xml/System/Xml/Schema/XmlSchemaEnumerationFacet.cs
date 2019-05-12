using System;

namespace System.Xml.Schema
{
	/// <summary>Represents the enumeration facet from XML Schema as specified by the World Wide Web Consortium (W3C). This class specifies a list of valid values for a simpleType element. Declaration is contained within a restriction declaration.</summary>
	public class XmlSchemaEnumerationFacet : XmlSchemaFacet
	{
		private const string xmlname = "enumeration";

		internal override XmlSchemaFacet.Facet ThisFacet
		{
			get
			{
				return XmlSchemaFacet.Facet.enumeration;
			}
		}

		internal static XmlSchemaEnumerationFacet Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = new XmlSchemaEnumerationFacet();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "enumeration")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaEnumerationFacet.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaEnumerationFacet.LineNumber = reader.LineNumber;
			xmlSchemaEnumerationFacet.LinePosition = reader.LinePosition;
			xmlSchemaEnumerationFacet.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaEnumerationFacet.Id = reader.Value;
				}
				else if (reader.Name == "value")
				{
					xmlSchemaEnumerationFacet.Value = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for enumeration", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaEnumerationFacet);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaEnumerationFacet;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "enumeration")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaEnumerationFacet.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaEnumerationFacet.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaEnumerationFacet;
		}
	}
}
