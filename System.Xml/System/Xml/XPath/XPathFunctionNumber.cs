using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionNumber : XPathNumericFunction
	{
		private Expression arg0;

		public XPathFunctionNumber(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				this.arg0 = args.Arg;
				if (args.Tail != null)
				{
					throw new XPathException("number takes 1 or zero args");
				}
			}
		}

		public override Expression Optimize()
		{
			if (this.arg0 == null)
			{
				return this;
			}
			this.arg0 = this.arg0.Optimize();
			return this.arg0.HasStaticValue ? new ExprNumber(this.StaticValueAsNumber) : this;
		}

		public override bool HasStaticValue
		{
			get
			{
				return this.arg0 != null && this.arg0.HasStaticValue;
			}
		}

		public override double StaticValueAsNumber
		{
			get
			{
				return (this.arg0 == null) ? 0.0 : this.arg0.StaticValueAsNumber;
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
				return XPathFunctions.ToNumber(iter.Current.Value);
			}
			return this.arg0.EvaluateNumber(iter);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"number(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
