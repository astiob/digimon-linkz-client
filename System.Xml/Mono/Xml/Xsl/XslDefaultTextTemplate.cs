using System;
using System.Collections;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XslDefaultTextTemplate : XslTemplate
	{
		private static XslDefaultTextTemplate instance = new XslDefaultTextTemplate();

		private XslDefaultTextTemplate() : base(null)
		{
		}

		public static XslTemplate Instance
		{
			get
			{
				return XslDefaultTextTemplate.instance;
			}
		}

		public override void Evaluate(XslTransformProcessor p, Hashtable withParams)
		{
			if (p.CurrentNode.NodeType == XPathNodeType.Whitespace)
			{
				if (p.PreserveOutputWhitespace)
				{
					p.Out.WriteWhitespace(p.CurrentNode.Value);
				}
			}
			else
			{
				p.Out.WriteString(p.CurrentNode.Value);
			}
		}
	}
}
