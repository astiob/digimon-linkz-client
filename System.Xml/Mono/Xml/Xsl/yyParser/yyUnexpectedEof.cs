using System;

namespace Mono.Xml.Xsl.yyParser
{
	internal class yyUnexpectedEof : yyException
	{
		public yyUnexpectedEof(string message) : base(message)
		{
		}

		public yyUnexpectedEof() : base(string.Empty)
		{
		}
	}
}
