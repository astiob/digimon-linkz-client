using System;

namespace System.Xml.XPath
{
	internal abstract class XPathNumericFunction : XPathFunction
	{
		internal XPathNumericFunction(FunctionArguments args) : base(args)
		{
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		public override object StaticValue
		{
			get
			{
				return this.StaticValueAsNumber;
			}
		}
	}
}
