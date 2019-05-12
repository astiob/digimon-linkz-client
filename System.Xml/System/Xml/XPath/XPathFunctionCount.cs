using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionCount : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionCount(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("count takes 1 arg");
			}
			this.arg0 = args.Arg;
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
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
			return (double)this.arg0.EvaluateNodeSet(iter).Count;
		}

		public override bool EvaluateBoolean(BaseIterator iter)
		{
			if (this.arg0.GetReturnType(iter) == XPathResultType.NodeSet)
			{
				return this.arg0.EvaluateBoolean(iter);
			}
			return this.arg0.EvaluateNodeSet(iter).MoveNext();
		}

		public override string ToString()
		{
			return "count(" + this.arg0.ToString() + ")";
		}
	}
}
