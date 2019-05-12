using System;
using System.Text;

namespace System.Xml.XPath
{
	internal class XPathFunctionTranslate : XPathFunction
	{
		private Expression arg0;

		private Expression arg1;

		private Expression arg2;

		public XPathFunctionTranslate(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail == null || args.Tail.Tail == null || args.Tail.Tail.Tail != null)
			{
				throw new XPathException("translate takes 3 args");
			}
			this.arg0 = args.Arg;
			this.arg1 = args.Tail.Arg;
			this.arg2 = args.Tail.Tail.Arg;
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.String;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.arg0.Peer && this.arg1.Peer && this.arg2.Peer;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			string text = this.arg0.EvaluateString(iter);
			string text2 = this.arg1.EvaluateString(iter);
			string text3 = this.arg2.EvaluateString(iter);
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			int i = 0;
			int length = text.Length;
			int length2 = text3.Length;
			while (i < length)
			{
				int num = text2.IndexOf(text[i]);
				if (num != -1)
				{
					if (num < length2)
					{
						stringBuilder.Append(text3[num]);
					}
				}
				else
				{
					stringBuilder.Append(text[i]);
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"string-length(",
				this.arg0.ToString(),
				",",
				this.arg1.ToString(),
				",",
				this.arg2.ToString(),
				")"
			});
		}
	}
}
