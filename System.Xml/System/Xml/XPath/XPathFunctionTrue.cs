using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionTrue : XPathBooleanFunction
	{
		public XPathFunctionTrue(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				throw new XPathException("true takes 0 args");
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
				return true;
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
			return true;
		}

		public override string ToString()
		{
			return "true()";
		}
	}
}
