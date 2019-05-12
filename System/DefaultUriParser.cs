using System;

namespace System
{
	internal class DefaultUriParser : System.UriParser
	{
		public DefaultUriParser()
		{
		}

		public DefaultUriParser(string scheme)
		{
			this.scheme_name = scheme;
		}
	}
}
