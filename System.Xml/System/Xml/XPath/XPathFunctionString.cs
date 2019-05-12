using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionString : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionString(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("string takes 1 or zero args");
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
				return iter.Current.Value;
			}
			return this.arg0.EvaluateString(iter);
		}

		public override string ToString()
		{
			return "string(" + this.arg0.ToString() + ")";
		}
	}
}
