using System;
using System.Globalization;

namespace System.Xml.XPath
{
	internal class XPathFunctionLang : XPathFunction
	{
		private Expression arg0;

		public XPathFunctionLang(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("lang takes one arg");
			}
			this.arg0 = args.Arg;
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Boolean;
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
			return this.EvaluateBoolean(iter);
		}

		public override bool EvaluateBoolean(BaseIterator iter)
		{
			string a = this.arg0.EvaluateString(iter).ToLower(CultureInfo.InvariantCulture);
			string text = iter.Current.XmlLang.ToLower(CultureInfo.InvariantCulture);
			return a == text || a == text.Split(new char[]
			{
				'-'
			})[0];
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"lang(",
				this.arg0.ToString(),
				")"
			});
		}
	}
}
