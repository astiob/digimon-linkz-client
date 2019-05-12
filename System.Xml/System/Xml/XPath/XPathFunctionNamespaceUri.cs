using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionNamespaceUri : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionNamespaceUri(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("namespace-uri takes 1 or zero args");
				}
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.arg0 == null || this.arg0.Peer;
			}
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.String;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			if (this.arg0 == null)
			{
				return iter.Current.NamespaceURI;
			}
			BaseIterator baseIterator = this.arg0.EvaluateNodeSet(iter);
			if (baseIterator == null || !baseIterator.MoveNext())
			{
				return string.Empty;
			}
			return baseIterator.Current.NamespaceURI;
		}

		public override string ToString()
		{
			return "namespace-uri(" + this.arg0.ToString() + ")";
		}
	}
}
