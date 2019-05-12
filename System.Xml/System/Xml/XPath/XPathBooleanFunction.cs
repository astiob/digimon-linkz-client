using System;

namespace System.Xml.XPath
{
	internal abstract class XPathBooleanFunction : XPathFunction
	{
		public XPathBooleanFunction(FunctionArguments args) : base(args)
		{
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Boolean;
			}
		}

		public override object StaticValue
		{
			get
			{
				return this.StaticValueAsBoolean;
			}
		}
	}
}
