using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionCeil : XPathNumericFunction
	{
		private Expression arg0;

		public XPathFunctionCeil(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("ceil takes one arg");
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
				return (!this.HasStaticValue) ? 0.0 : Math.Ceiling(this.arg0.StaticValueAsNumber);
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
			return Math.Ceiling(this.arg0.EvaluateNumber(iter));
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"ceil(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
