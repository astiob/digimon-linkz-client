using System;

namespace System.Xml.Schema
{
	/// <summary>Represents the maxInclusive element from XML Schema as specified by the World Wide Web Consortium (W3C). This class can be used to specify a restriction on the maximum value of a simpleType element. The element value must be less than or equal to the value of the maxInclusive element.</summary>
	public class XmlSchemaMaxInclusiveFacet : XmlSchemaFacet
	{
		private const string xmlname = "maxInclusive";

		internal override XmlSchemaFacet.Facet ThisFacet
		{
			get
			{
				return XmlSchemaFacet.Facet.maxInclusive;
			}
		}

		internal static XmlSchemaMaxInclusiveFacet Read(XmlSchemaReader reader, ValidationEventHandler h)
		{
			XmlSchemaMaxInclusiveFacet xmlSchemaMaxInclusiveFacet = new XmlSchemaMaxInclusiveFacet();
			reader.MoveToElement();
			if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "maxInclusive")
			{
				XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaMaxInclusiveFacet.Read, name=" + reader.Name, null);
				reader.Skip();
				return null;
			}
			xmlSchemaMaxInclusiveFacet.LineNumber = reader.LineNumber;
			xmlSchemaMaxInclusiveFacet.LinePosition = reader.LinePosition;
			xmlSchemaMaxInclusiveFacet.SourceUri = reader.BaseURI;
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "id")
				{
					xmlSchemaMaxInclusiveFacet.Id = reader.Value;
				}
				else if (reader.Name == "fixed")
				{
					Exception ex;
					xmlSchemaMaxInclusiveFacet.IsFixed = XmlSchemaUtil.ReadBoolAttribute(reader, out ex);
					if (ex != null)
					{
						XmlSchemaObject.error(h, reader.Value + " is not a valid value for fixed attribute", ex);
					}
				}
				else if (reader.Name == "value")
				{
					xmlSchemaMaxInclusiveFacet.Value = reader.Value;
				}
				else if ((reader.NamespaceURI == string.Empty && reader.Name != "xmlns") || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
				{
					XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for maxInclusive", null);
				}
				else
				{
					XmlSchemaUtil.ReadUnhandledAttribute(reader, xmlSchemaMaxInclusiveFacet);
				}
			}
			reader.MoveToElement();
			if (reader.IsEmptyElement)
			{
				return xmlSchemaMaxInclusiveFacet;
			}
			int num = 1;
			while (reader.ReadNextElement())
			{
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (reader.LocalName != "maxInclusive")
					{
						XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaMaxInclusiveFacet.Read, name=" + reader.Name, null);
					}
					break;
				}
				if (num <= 1 && reader.LocalName == "annotation")
				{
					num = 2;
					XmlSchemaAnnotation xmlSchemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
					if (xmlSchemaAnnotation != null)
					{
						xmlSchemaMaxInclusiveFacet.Annotation = xmlSchemaAnnotation;
					}
				}
				else
				{
					reader.RaiseInvalidElementError();
				}
			}
			return xmlSchemaMaxInclusiveFacet;
		}
	}
}
