using System;
using System.Xml;

namespace Mono.Xml
{
	internal interface IHasXmlParserContext
	{
		XmlParserContext ParserContext { get; }
	}
}
