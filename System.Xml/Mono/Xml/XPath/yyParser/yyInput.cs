using System;

namespace Mono.Xml.XPath.yyParser
{
	internal interface yyInput
	{
		bool advance();

		int token();

		object value();
	}
}
