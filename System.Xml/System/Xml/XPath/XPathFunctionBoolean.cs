using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionBoolean : XPathBooleanFunction
	{
		private Expression arg0;

		public XPathFunctionBoolean(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("boolean takes 1 or zero args");
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

		public override object Evaluate(BaseIterator iter)
		{
			if (this.arg0 == null)
			{
				return XPathFunctions.ToBoolean(iter.Current.Value);
			}
			return this.arg0.EvaluateBoolean(iter);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"boolean(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
