using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XslDefaultNodeTemplate : XslTemplate
	{
		private XmlQualifiedName mode;

		private static XslDefaultNodeTemplate instance = new XslDefaultNodeTemplate(XmlQualifiedName.Empty);

		public XslDefaultNodeTemplate(XmlQualifiedName mode) : base(null)
		{
			this.mode = mode;
		}

		public static XslTemplate Instance
		{
			get
			{
				return XslDefaultNodeTemplate.instance;
			}
		}

		public override void Evaluate(XslTransformProcessor p, Hashtable withParams)
		{
			p.ApplyTemplates(p.CurrentNode.SelectChildren(XPathNodeType.All), this.mode, null);
		}
	}
}
