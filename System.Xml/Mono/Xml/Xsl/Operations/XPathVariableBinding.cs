using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XPathVariableBinding : Expression
	{
		private XslGeneralVariable v;

		public XPathVariableBinding(XslGeneralVariable v)
		{
			this.v = v;
		}

		public override string ToString()
		{
			return "$" + this.v.Name.ToString();
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		public override XPathResultType GetReturnType(BaseIterator iter)
		{
			return XPathResultType.Any;
		}

		public override object Evaluate(BaseIterator iter)
		{
			return this.v.Evaluate(iter.NamespaceManager as XsltContext);
		}
	}
}
