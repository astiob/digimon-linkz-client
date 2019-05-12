using System;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XsltGenerateId : XPathFunction
	{
		private Expression arg0;

		public XsltGenerateId(FunctionArguments args) : base(args)
		{
			if (args != null)
			{
				if (args.Tail != null)
				{
					throw new XPathException("generate-id takes 1 or no args");
				}
				this.arg0 = args.Arg;
			}
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
				return this.arg0.Peer;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			XPathNavigator xpathNavigator;
			if (this.arg0 != null)
			{
				XPathNodeIterator xpathNodeIterator = this.arg0.EvaluateNodeSet(iter);
				if (!xpathNodeIterator.MoveNext())
				{
					return string.Empty;
				}
				xpathNavigator = xpathNodeIterator.Current.Clone();
			}
			else
			{
				xpathNavigator = iter.Current.Clone();
			}
			StringBuilder stringBuilder = new StringBuilder("Mono");
			stringBuilder.Append(XmlConvert.EncodeLocalName(xpathNavigator.BaseURI));
			stringBuilder.Replace('_', 'm');
			stringBuilder.Append(xpathNavigator.NodeType);
			stringBuilder.Append('m');
			do
			{
				stringBuilder.Append(this.IndexInParent(xpathNavigator));
				stringBuilder.Append('m');
			}
			while (xpathNavigator.MoveToParent());
			return stringBuilder.ToString();
		}

		private int IndexInParent(XPathNavigator nav)
		{
			int num = 0;
			while (nav.MoveToPrevious())
			{
				num++;
			}
			return num;
		}
	}
}
