using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FacebookGames
{
	[Serializable]
	public class PipePacket
	{
		public string Serialize()
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			};
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("", "");
			XmlSerializer xmlSerializer = new XmlSerializer(base.GetType());
			StringWriter stringWriter = new StringWriter();
			string result;
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
			{
				xmlSerializer.Serialize(xmlWriter, this, xmlSerializerNamespaces);
				result = stringWriter.ToString();
			}
			return result;
		}

		public static string GetMessageType(string message)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(message);
				if (xmlDocument.DocumentElement != null)
				{
					return xmlDocument.DocumentElement.Name;
				}
			}
			catch
			{
			}
			return "Unknown";
		}

		public static T Deserialize<T>(string message) where T : PipePacket
		{
			return new XmlSerializer(typeof(T)).Deserialize(new StringReader(message)) as T;
		}
	}
}
