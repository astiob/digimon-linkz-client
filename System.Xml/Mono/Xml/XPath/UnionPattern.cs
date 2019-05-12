using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
	internal class UnionPattern : Pattern
	{
		public readonly Pattern p0;

		public readonly Pattern p1;

		public UnionPattern(Pattern p0, Pattern p1)
		{
			this.p0 = p0;
			this.p1 = p1;
		}

		public override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return (this.p0.EvaluatedNodeType != this.p1.EvaluatedNodeType) ? XPathNodeType.All : this.p0.EvaluatedNodeType;
			}
		}

		public override bool Matches(XPathNavigator node, XsltContext ctx)
		{
			return this.p0.Matches(node, ctx) || this.p1.Matches(node, ctx);
		}

		public override string ToString()
		{
			return this.p0.ToString() + " | " + this.p1.ToString();
		}
	}
}
