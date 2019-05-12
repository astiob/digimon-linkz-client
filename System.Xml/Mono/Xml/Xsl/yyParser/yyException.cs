using System;

namespace Mono.Xml.Xsl.yyParser
{
	internal class yyException : Exception
	{
		public yyException(string message) : base(message)
		{
		}
	}
}
