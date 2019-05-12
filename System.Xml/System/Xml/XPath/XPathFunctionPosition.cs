using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionPosition : XPathFunction
	{
		public XPathFunctionPosition(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				throw new XPathException("position takes 0 args");
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
			return (double)iter.CurrentPosition;
		}

		public override string ToString()
		{
			return "position()";
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
