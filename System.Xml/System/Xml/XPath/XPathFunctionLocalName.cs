using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionLocalName : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionLocalName(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("local-name takes 1 or zero args");
				}
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
				return this.arg0 == null || this.arg0.Peer;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			if (this.arg0 == null)
			{
				return iter.Current.LocalName;
			}
			BaseIterator baseIterator = this.arg0.EvaluateNodeSet(iter);
			if (baseIterator == null || !baseIterator.MoveNext())
			{
				return string.Empty;
			}
			return baseIterator.Current.LocalName;
		}

		public override string ToString()
		{
			return "local-name(" + this.arg0.ToString() + ")";
		}
	}
}
