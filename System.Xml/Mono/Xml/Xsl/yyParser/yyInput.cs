using System;

namespace Mono.Xml.Xsl.yyParser
{
	internal interface yyInput
	{
		bool advance();

		int token();

		object value();
	}
}
