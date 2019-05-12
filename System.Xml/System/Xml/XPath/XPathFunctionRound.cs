using System;

namespace System.Xml.XPath
{
	internal class XPathFunctionRound : XPathNumericFunction
	{
		private Expression arg0;

		public XPathFunctionRound(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("round takes one arg");
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
				return (!this.HasStaticValue) ? 0.0 : this.Round(this.arg0.StaticValueAsNumber);
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
			return this.Round(this.arg0.EvaluateNumber(iter));
		}

		private double Round(double arg)
		{
			if (arg < -0.5 || arg > 0.0)
			{
				return Math.Floor(arg + 0.5);
			}
			return Math.Round(arg);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"round(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
