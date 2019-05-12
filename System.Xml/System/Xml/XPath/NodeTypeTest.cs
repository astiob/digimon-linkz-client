using System;

namespace System.Xml.XPath
{
	internal class NodeTypeTest : NodeTest
	{
		public readonly XPathNodeType type;

		protected string _param;

		public NodeTypeTest(Axes axis) : base(axis)
		{
			this.type = this._axis.NodeType;
		}

		public NodeTypeTest(Axes axis, XPathNodeType type) : base(axis)
		{
			this.type = type;
		}

		public NodeTypeTest(Axes axis, XPathNodeType type, string param) : base(axis)
		{
			this.type = type;
			this._param = param;
			if (param != null && type != XPathNodeType.ProcessingInstruction)
			{
				throw new XPathException("No argument allowed for " + NodeTypeTest.ToString(type) + "() test");
			}
		}

		internal NodeTypeTest(NodeTypeTest other, Axes axis) : base(axis)
		{
			this.type = other.type;
			this._param = other._param;
		}

		public override string ToString()
		{
			string text = NodeTypeTest.ToString(this.type);
			if (this.type == XPathNodeType.ProcessingInstruction && this._param != null)
			{
				text = text + "('" + this._param + "')";
			}
			else
			{
				text += "()";
			}
			return this._axis.ToString() + "::" + text;
		}

		private static string ToString(XPathNodeType type)
		{
			switch (type)
			{
			case XPathNodeType.Element:
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
			case XPathNodeType.All:
				return "node";
			case XPathNodeType.Text:
				return "text";
			case XPathNodeType.ProcessingInstruction:
				return "processing-instruction";
			case XPathNodeType.Comment:
				return "comment";
			}
			return "node-type [" + type.ToString() + "]";
		}

		public override bool Match(IXmlNamespaceResolver nsm, XPathNavigator nav)
		{
			XPathNodeType nodeType = nav.NodeType;
			switch (this.type)
			{
			case XPathNodeType.Text:
				switch (nodeType)
				{
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					return true;
				default:
					return false;
				}
				break;
			case XPathNodeType.ProcessingInstruction:
				return nodeType == XPathNodeType.ProcessingInstruction && (this._param == null || !(nav.Name != this._param));
			case XPathNodeType.All:
				return true;
			}
			return this.type == nodeType;
		}

		public override void GetInfo(out string name, out string ns, out XPathNodeType nodetype, IXmlNamespaceResolver nsm)
		{
			name = this._param;
			ns = null;
			nodetype = this.type;
		}
	}
}
