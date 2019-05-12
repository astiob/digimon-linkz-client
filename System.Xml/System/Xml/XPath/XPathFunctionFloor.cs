using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionFloor : XPathNumericFunction
	{
		private Expression arg0;

		public XPathFunctionFloor(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("floor takes one arg");
			}
			this.arg0 = args.Arg;
		}

		public override bool HasStaticValue
		{
			get
			{
				return this.arg0.HasStaticValue;
			}
		}

		public override double StaticValueAsNumber
		{
			get
			{
				return (!this.HasStaticValue) ? 0.0 : Math.Floor(this.arg0.StaticValueAsNumber);
			}
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
			return Math.Floor(this.arg0.EvaluateNumber(iter));
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"floor(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
