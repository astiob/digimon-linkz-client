using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Xml.XPath
{
	internal class XPathFunctionConcat : XPathFunction
	{
		private ArrayList rgs;

		public XPathFunctionConcat(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail == null)
			{
				throw new XPathException("concat takes 2 or more args");
			}
			args.ToArrayList(this.rgs = new ArrayList());
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
				for (int i = 0; i < this.rgs.Count; i++)
				{
					if (!((Expression)this.rgs[i]).Peer)
					{
						return false;
					}
				}
				return true;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.rgs.Count;
			for (int i = 0; i < count; i++)
			{
				stringBuilder.Append(((Expression)this.rgs[i]).EvaluateString(iter));
			}
			return stringBuilder.ToString();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("concat(");
			for (int i = 0; i < this.rgs.Count - 1; i++)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}", new object[]
				{
					this.rgs[i].ToString()
				});
				stringBuilder.Append(',');
			}
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				this.rgs[this.rgs.Count - 1].ToString()
			});
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}
	}
}
