using System;

namespace System.Xml.XPath
{
	internal abstract class NodeSet : Expression
	{
		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		internal abstract bool Subtree { get; }
	}
}
