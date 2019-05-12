using System;

namespace Mono.Xml.XPath.yyParser
{
	internal class yyException : Exception
	{
		public yyException(string message) : base(message)
		{
		}
	}
}
