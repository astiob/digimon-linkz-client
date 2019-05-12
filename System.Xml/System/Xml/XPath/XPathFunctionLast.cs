using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionLast : XPathFunction
	{
		public XPathFunctionLast(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				throw new XPathException("last takes 0 args");
			}
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
				return true;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			return (double)iter.Count;
		}

		public override string ToString()
		{
			return "last()";
		}

		internal override bool IsPositional
		{
			get
			{
				return true;
			}
		}
	}
}
