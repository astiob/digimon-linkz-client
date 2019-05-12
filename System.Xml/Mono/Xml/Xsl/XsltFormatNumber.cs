using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XsltFormatNumber : XPathFunction
	{
		private Expression arg0;

		private Expression arg1;

		private Expression arg2;

		private IStaticXsltContext ctx;

		public XsltFormatNumber(FunctionArguments args, IStaticXsltContext ctx) : base(args)
		{
			if (args == null || args.Tail == null || (args.Tail.Tail != null && args.Tail.Tail.Tail != null))
			{
				throw new XPathException("format-number takes 2 or 3 args");
			}
			this.arg0 = args.Arg;
			this.arg1 = args.Tail.Arg;
			if (args.Tail.Tail != null)
			{
				this.arg2 = args.Tail.Tail.Arg;
				this.ctx = ctx;
			}
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.String;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.arg0.Peer && this.arg1.Peer && (this.arg2 == null || this.arg2.Peer);
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			double number = this.arg0.EvaluateNumber(iter);
			string pattern = this.arg1.EvaluateString(iter);
			XmlQualifiedName name = XmlQualifiedName.Empty;
			if (this.arg2 != null)
			{
				name = XslNameUtil.FromString(this.arg2.EvaluateString(iter), this.ctx);
			}
			object result;
			try
			{
				result = ((XsltCompiledContext)iter.NamespaceManager).Processor.CompiledStyle.LookupDecimalFormat(name).FormatNumber(number, pattern);
			}
			catch (ArgumentException ex)
			{
				throw new XsltException(ex.Message, ex, iter.Current);
			}
			return result;
		}
	}
}
