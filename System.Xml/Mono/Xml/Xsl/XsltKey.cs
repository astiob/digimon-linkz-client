using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XsltKey : XPathFunction
	{
		private Expression arg0;

		private Expression arg1;

		private IStaticXsltContext staticContext;

		public XsltKey(FunctionArguments args, IStaticXsltContext ctx) : base(args)
		{
			this.staticContext = ctx;
			if (args == null || args.Tail == null)
			{
				throw new XPathException("key takes 2 args");
			}
			this.arg0 = args.Arg;
			this.arg1 = args.Tail.Arg;
		}

		public Expression KeyName
		{
			get
			{
				return this.arg0;
			}
		}

		public Expression Field
		{
			get
			{
				return this.arg1;
			}
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
				return this.arg0.Peer && this.arg1.Peer;
			}
		}

		public bool PatternMatches(XPathNavigator nav, XsltContext nsmgr)
		{
			XsltCompiledContext xsltCompiledContext = nsmgr as XsltCompiledContext;
			return xsltCompiledContext.MatchesKey(nav, this.staticContext, this.arg0.StaticValueAsString, this.arg1.StaticValueAsString);
		}

		public override object Evaluate(BaseIterator iter)
		{
			XsltCompiledContext xsltCompiledContext = iter.NamespaceManager as XsltCompiledContext;
			return xsltCompiledContext.EvaluateKey(this.staticContext, iter, this.arg0, this.arg1);
		}
	}
}
