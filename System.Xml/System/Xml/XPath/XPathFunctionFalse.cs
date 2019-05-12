using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionFalse : XPathBooleanFunction
	{
		public XPathFunctionFalse(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				throw new XPathException("false takes 0 args");
			}
		}

		public override bool HasStaticValue
		{
			get
			{
				return true;
			}
		}

		public override bool StaticValueAsBoolean
		{
			get
			{
				return false;
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
			return false;
		}

		public override string ToString()
		{
			return "false()";
		}
	}
}
