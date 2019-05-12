using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionSum : XPathNumericFunction
	{
		private Expression arg0;

		public XPathFunctionSum(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("sum takes one arg");
			}
			this.arg0 = args.Arg;
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
			XPathNodeIterator xpathNodeIterator = this.arg0.EvaluateNodeSet(iter);
			double num = 0.0;
			while (xpathNodeIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = xpathNodeIterator.Current;
				num += XPathFunctions.ToNumber(xpathNavigator.Value);
			}
			return num;
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"sum(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
