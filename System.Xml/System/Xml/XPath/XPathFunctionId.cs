using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class XPathFunctionId : XPathFunction
	{
		private Expression arg0;

		private static char[] rgchWhitespace = new char[]
		{
			' ',
			'\t',
			'\r',
			'\n'
		};

		public XPathFunctionId(FunctionArguments args) : base(args)
		{
			if (args == null || args.Tail != null)
			{
				throw new XPathException("id takes 1 arg");
			}
			this.arg0 = args.Arg;
		}

		public Expression Id
		{
			get
			{
				return this.arg0;
			}
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
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
			object obj = this.arg0.Evaluate(iter);
			XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
			string text;
			if (xpathNodeIterator != null)
			{
				text = string.Empty;
				while (xpathNodeIterator.MoveNext())
				{
					XPathNavigator xpathNavigator = xpathNodeIterator.Current;
					text = text + xpathNavigator.Value + " ";
				}
			}
			else
			{
				text = XPathFunctions.ToString(obj);
			}
			XPathNavigator xpathNavigator2 = iter.Current.Clone();
			ArrayList arrayList = new ArrayList();
			string[] array = text.Split(XPathFunctionId.rgchWhitespace);
			for (int i = 0; i < array.Length; i++)
			{
				if (xpathNavigator2.MoveToId(array[i]))
				{
					arrayList.Add(xpathNavigator2.Clone());
				}
			}
			arrayList.Sort(XPathNavigatorComparer.Instance);
			return new ListIterator(iter, arrayList);
		}

		public override string ToString()
		{
			return "id(" + this.arg0.ToString() + ")";
		}
	}
}
