using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionName : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionName(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("name takes 1 or zero args");
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
				return iter.Current.Name;
			}
			BaseIterator baseIterator = this.arg0.EvaluateNodeSet(iter);
			if (baseIterator == null || !baseIterator.MoveNext())
			{
				return string.Empty;
			}
			return baseIterator.Current.Name;
		}

		public override string ToString()
		{
			return "name(" + ((this.arg0 == null) ? string.Empty : this.arg0.ToString()) + ")";
		}
	}
}
