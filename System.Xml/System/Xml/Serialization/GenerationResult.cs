using System;

namespace System.Xml.Serialization
{
	internal class GenerationResult
	{
		public XmlMapping Mapping;

		public string ReaderClassName;

		public string ReadMethodName;

		public string WriterClassName;

		public string WriteMethodName;

		public string Namespace;

		public string SerializerClassName;

		public string BaseSerializerClassName;

		public string ImplementationClassName;
	}
}
