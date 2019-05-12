using System;
using System.Collections;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class MSXslNodeSet : XPathFunction
	{
		private Expression arg0;

		public MSXslNodeSet(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("element-available takes 1 arg");
			}
			this.arg0 = args.Arg;
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.arg0.Peer;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			XsltCompiledContext nsm = iter.NamespaceManager as XsltCompiledContext;
			XPathNavigator xpathNavigator = (iter.Current == null) ? null : iter.Current.Clone();
			XPathNavigator xpathNavigator2 = this.arg0.EvaluateAs(iter, XPathResultType.Navigator) as XPathNavigator;
			if (xpathNavigator2 != null)
			{
				return new ListIterator(new ArrayList
				{
					xpathNavigator2
				}, nsm);
			}
			if (xpathNavigator != null)
			{
				return new XsltException("Cannot convert the XPath argument to a result tree fragment.", null, xpathNavigator);
			}
			return new XsltException("Cannot convert the XPath argument to a result tree fragment.", null);
		}
	}
}
