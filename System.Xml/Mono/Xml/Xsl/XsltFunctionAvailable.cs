using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XsltFunctionAvailable : XPathFunction
	{
		private Expression arg0;

		private IStaticXsltContext ctx;

		public XsltFunctionAvailable(FunctionArguments args, IStaticXsltContext ctx) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("element-available takes 1 arg");
			}
			this.arg0 = args.Arg;
			this.ctx = ctx;
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Boolean;
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
			string text = this.arg0.EvaluateString(iter);
			int num = text.IndexOf(':');
			if (num > 0)
			{
				return (iter.NamespaceManager as XsltCompiledContext).ResolveFunction(XslNameUtil.FromString(text, this.ctx), null) != null;
			}
			return text == "boolean" || text == "ceiling" || text == "concat" || text == "contains" || text == "count" || text == "false" || text == "floor" || text == "id" || text == "lang" || text == "last" || text == "local-name" || text == "name" || text == "namespace-uri" || text == "normalize-space" || text == "not" || text == "number" || text == "position" || text == "round" || text == "starts-with" || text == "string" || text == "string-length" || text == "substring" || text == "substring-after" || text == "substring-before" || text == "sum" || text == "translate" || text == "true" || text == "document" || text == "format-number" || text == "function-available" || text == "generate-id" || text == "key" || text == "current" || text == "unparsed-entity-uri" || text == "element-available" || text == "system-property";
		}
	}
}
