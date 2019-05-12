using Mono.Xml.Xsl;
using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
	internal class KeyPattern : LocationPathPattern
	{
		private XsltKey key;

		public KeyPattern(XsltKey key) : base(null)
		{
			this.key = key;
		}

		public override bool Matches(XPathNavigator node, XsltContext ctx)
		{
			return this.key.PatternMatches(node, ctx);
		}

		public override double DefaultPriority
		{
			get
			{
				return 0.5;
			}
		}
	}
}
