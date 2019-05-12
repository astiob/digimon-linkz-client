using Mono.Xml.Xsl;
using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
	internal class IdPattern : LocationPathPattern
	{
		private string[] ids;

		public IdPattern(string arg0) : base(null)
		{
			this.ids = arg0.Split(XmlChar.WhitespaceChars);
		}

		public override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return XPathNodeType.Element;
			}
		}

		public override bool Matches(XPathNavigator node, XsltContext ctx)
		{
			XPathNavigator navCache = ((XsltCompiledContext)ctx).GetNavCache(this, node);
			for (int i = 0; i < this.ids.Length; i++)
			{
				if (navCache.MoveToId(this.ids[i]) && navCache.IsSamePosition(node))
				{
					return true;
				}
			}
			return false;
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
