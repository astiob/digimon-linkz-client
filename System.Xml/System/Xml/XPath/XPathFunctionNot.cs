using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionNot : XPathBooleanFunction
	{
		private Expression arg0;

		public XPathFunctionNot(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("not takes one arg");
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
			return !this.arg0.EvaluateBoolean(iter);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"not(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
