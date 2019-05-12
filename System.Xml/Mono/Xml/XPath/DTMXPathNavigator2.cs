using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class DTMXPathNavigator2 : XPathNavigator, IXmlLineInfo
	{
		private DTMXPathDocument2 document;

		private bool currentIsNode;

		private bool currentIsAttr;

		private int currentNode;

		private int currentAttr;

		private int currentNs;

		public DTMXPathNavigator2(DTMXPathDocument2 document)
		{
			this.MoveToRoot();
			this.document = document;
		}

		public DTMXPathNavigator2(DTMXPathNavigator2 org)
		{
			this.document = org.document;
			this.currentIsNode = org.currentIsNode;
			this.currentIsAttr = org.currentIsAttr;
			this.currentNode = org.currentNode;
			this.currentAttr = org.currentAttr;
			this.currentNs = org.currentNs;
		}

		int IXmlLineInfo.LineNumber
		{
			get
			{
				return (!this.currentIsAttr) ? this.nodes[this.currentNode].LineNumber : this.attributes[this.currentAttr].LineNumber;
			}
		}

		int IXmlLineInfo.LinePosition
		{
			get
			{
				return (!this.currentIsAttr) ? this.nodes[this.currentNode].LinePosition : this.attributes[this.currentAttr].LinePosition;
			}
		}

		bool IXmlLineInfo.HasLineInfo()
		{
			return true;
		}

		private XmlNameTable nameTable
		{
			get
			{
				return this.document.NameTable;
			}
		}

		private DTMXPathLinkedNode2[] nodes
		{
			get
			{
				return this.document.Nodes;
			}
		}

		private DTMXPathAttributeNode2[] attributes
		{
			get
			{
				return this.document.Attributes;
			}
		}

		private DTMXPathNamespaceNode2[] namespaces
		{
			get
			{
				return this.document.Namespaces;
			}
		}

		private string[] atomicStringPool
		{
			get
			{
				return this.document.AtomicStringPool;
			}
		}

		private string[] nonAtomicStringPool
		{
			get
			{
				return this.document.NonAtomicStringPool;
			}
		}

		private Hashtable idTable
		{
			get
			{
				return this.document.IdTable;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.atomicStringPool[this.nodes[this.currentNode].BaseURI];
			}
		}

		public override bool HasAttributes
		{
			get
			{
				return this.currentIsNode && this.nodes[this.currentNode].FirstAttribute != 0;
			}
		}

		public override bool HasChildren
		{
			get
			{
				return this.currentIsNode && this.nodes[this.currentNode].FirstChild != 0;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.currentIsNode && this.nodes[this.currentNode].IsEmptyElement;
			}
		}

		public override string LocalName
		{
			get
			{
				if (this.currentIsNode)
				{
					return this.atomicStringPool[this.nodes[this.currentNode].LocalName];
				}
				if (this.currentIsAttr)
				{
					return this.atomicStringPool[this.attributes[this.currentAttr].LocalName];
				}
				return this.atomicStringPool[this.namespaces[this.currentNs].Name];
			}
		}

		public override string Name
		{
			get
			{
				string text;
				string text2;
				if (this.currentIsNode)
				{
					text = this.atomicStringPool[this.nodes[this.currentNode].Prefix];
					text2 = this.atomicStringPool[this.nodes[this.currentNode].LocalName];
				}
				else
				{
					if (!this.currentIsAttr)
					{
						return this.atomicStringPool[this.namespaces[this.currentNs].Name];
					}
					text = this.atomicStringPool[this.attributes[this.currentAttr].Prefix];
					text2 = this.atomicStringPool[this.attributes[this.currentAttr].LocalName];
				}
				if (text != string.Empty)
				{
					return text + ':' + text2;
				}
				return text2;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				if (this.currentIsNode)
				{
					return this.atomicStringPool[this.nodes[this.currentNode].NamespaceURI];
				}
				if (this.currentIsAttr)
				{
					return this.atomicStringPool[this.attributes[this.currentAttr].NamespaceURI];
				}
				return string.Empty;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		public override XPathNodeType NodeType
		{
			get
			{
				if (this.currentIsNode)
				{
					return this.nodes[this.currentNode].NodeType;
				}
				if (this.currentIsAttr)
				{
					return XPathNodeType.Attribute;
				}
				return XPathNodeType.Namespace;
			}
		}

		public override string Prefix
		{
			get
			{
				if (this.currentIsNode)
				{
					return this.atomicStringPool[this.nodes[this.currentNode].Prefix];
				}
				if (this.currentIsAttr)
				{
					return this.atomicStringPool[this.attributes[this.currentAttr].Prefix];
				}
				return string.Empty;
			}
		}

		public override string Value
		{
			get
			{
				if (this.currentIsAttr)
				{
					return this.nonAtomicStringPool[this.attributes[this.currentAttr].Value];
				}
				if (!this.currentIsNode)
				{
					return this.atomicStringPool[this.namespaces[this.currentNs].Namespace];
				}
				switch (this.nodes[this.currentNode].NodeType)
				{
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return this.nonAtomicStringPool[this.nodes[this.currentNode].Value];
				default:
				{
					int firstChild = this.nodes[this.currentNode].FirstChild;
					if (firstChild == 0)
					{
						return string.Empty;
					}
					StringBuilder stringBuilder = null;
					this.BuildValue(firstChild, ref stringBuilder);
					return (stringBuilder != null) ? stringBuilder.ToString() : string.Empty;
				}
				}
			}
		}

		private void BuildValue(int iter, ref StringBuilder valueBuilder)
		{
			int num = this.nodes[this.currentNode].NextSibling;
			if (num == 0)
			{
				int parent = this.currentNode;
				do
				{
					parent = this.nodes[parent].Parent;
					num = this.nodes[parent].NextSibling;
				}
				while (num == 0 && parent != 0);
				if (num == 0)
				{
					num = this.nodes.Length;
				}
			}
			while (iter < num)
			{
				switch (this.nodes[iter].NodeType)
				{
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					if (valueBuilder == null)
					{
						valueBuilder = new StringBuilder();
					}
					valueBuilder.Append(this.nonAtomicStringPool[this.nodes[iter].Value]);
					break;
				}
				iter++;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.atomicStringPool[this.nodes[this.currentNode].XmlLang];
			}
		}

		public override XPathNavigator Clone()
		{
			return new DTMXPathNavigator2(this);
		}

		public override XmlNodeOrder ComparePosition(XPathNavigator nav)
		{
			DTMXPathNavigator2 dtmxpathNavigator = nav as DTMXPathNavigator2;
			if (dtmxpathNavigator == null || dtmxpathNavigator.document != this.document)
			{
				return XmlNodeOrder.Unknown;
			}
			if (this.currentNode > dtmxpathNavigator.currentNode)
			{
				return XmlNodeOrder.After;
			}
			if (this.currentNode < dtmxpathNavigator.currentNode)
			{
				return XmlNodeOrder.Before;
			}
			if (dtmxpathNavigator.currentIsAttr)
			{
				if (!this.currentIsAttr)
				{
					return XmlNodeOrder.Before;
				}
				if (this.currentAttr > dtmxpathNavigator.currentAttr)
				{
					return XmlNodeOrder.After;
				}
				if (this.currentAttr < dtmxpathNavigator.currentAttr)
				{
					return XmlNodeOrder.Before;
				}
				return XmlNodeOrder.Same;
			}
			else
			{
				if (dtmxpathNavigator.currentIsNode)
				{
					return dtmxpathNavigator.currentIsNode ? XmlNodeOrder.Same : XmlNodeOrder.Before;
				}
				if (this.currentIsNode)
				{
					return XmlNodeOrder.Before;
				}
				if (this.currentNs > dtmxpathNavigator.currentNs)
				{
					return XmlNodeOrder.After;
				}
				if (this.currentNs < dtmxpathNavigator.currentNs)
				{
					return XmlNodeOrder.Before;
				}
				return XmlNodeOrder.Same;
			}
		}

		private int findAttribute(string localName, string namespaceURI)
		{
			if (this.currentIsNode && this.nodes[this.currentNode].NodeType == XPathNodeType.Element)
			{
				for (int num = this.nodes[this.currentNode].FirstAttribute; num != 0; num = this.attributes[num].NextAttribute)
				{
					if (this.atomicStringPool[this.attributes[num].LocalName] == localName && this.atomicStringPool[this.attributes[num].NamespaceURI] == namespaceURI)
					{
						return num;
					}
				}
			}
			return 0;
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			int num = this.findAttribute(localName, namespaceURI);
			return (num == 0) ? string.Empty : this.nonAtomicStringPool[this.attributes[num].Value];
		}

		public override string GetNamespace(string name)
		{
			if (this.currentIsNode && this.nodes[this.currentNode].NodeType == XPathNodeType.Element)
			{
				for (int num = this.nodes[this.currentNode].FirstNamespace; num != 0; num = this.namespaces[num].NextNamespace)
				{
					if (this.atomicStringPool[this.namespaces[num].Name] == name)
					{
						return this.atomicStringPool[this.namespaces[num].Namespace];
					}
				}
			}
			return string.Empty;
		}

		public override bool IsDescendant(XPathNavigator nav)
		{
			DTMXPathNavigator2 dtmxpathNavigator = nav as DTMXPathNavigator2;
			if (dtmxpathNavigator == null || dtmxpathNavigator.document != this.document)
			{
				return false;
			}
			if (dtmxpathNavigator.currentNode == this.currentNode)
			{
				return !dtmxpathNavigator.currentIsNode;
			}
			int parent = this.nodes[dtmxpathNavigator.currentNode].Parent;
			if (parent < this.currentNode)
			{
				return false;
			}
			while (parent != 0)
			{
				if (parent == this.currentNode)
				{
					return true;
				}
				parent = this.nodes[parent].Parent;
			}
			return false;
		}

		public override bool IsSamePosition(XPathNavigator other)
		{
			DTMXPathNavigator2 dtmxpathNavigator = other as DTMXPathNavigator2;
			if (dtmxpathNavigator == null || dtmxpathNavigator.document != this.document)
			{
				return false;
			}
			if (this.currentNode != dtmxpathNavigator.currentNode || this.currentIsAttr != dtmxpathNavigator.currentIsAttr || this.currentIsNode != dtmxpathNavigator.currentIsNode)
			{
				return false;
			}
			if (this.currentIsAttr)
			{
				return this.currentAttr == dtmxpathNavigator.currentAttr;
			}
			return this.currentIsNode || this.currentNs == dtmxpathNavigator.currentNs;
		}

		public override bool MoveTo(XPathNavigator other)
		{
			DTMXPathNavigator2 dtmxpathNavigator = other as DTMXPathNavigator2;
			if (dtmxpathNavigator == null || dtmxpathNavigator.document != this.document)
			{
				return false;
			}
			this.currentNode = dtmxpathNavigator.currentNode;
			this.currentAttr = dtmxpathNavigator.currentAttr;
			this.currentNs = dtmxpathNavigator.currentNs;
			this.currentIsNode = dtmxpathNavigator.currentIsNode;
			this.currentIsAttr = dtmxpathNavigator.currentIsAttr;
			return true;
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			int num = this.findAttribute(localName, namespaceURI);
			if (num == 0)
			{
				return false;
			}
			this.currentAttr = num;
			this.currentIsAttr = true;
			this.currentIsNode = false;
			return true;
		}

		public override bool MoveToFirst()
		{
			if (this.currentIsAttr)
			{
				return false;
			}
			int num = this.nodes[this.currentNode].PreviousSibling;
			if (num == 0)
			{
				return false;
			}
			num = this.nodes[num].Parent;
			num = this.nodes[num].FirstChild;
			this.currentNode = num;
			this.currentIsNode = true;
			return true;
		}

		public override bool MoveToFirstAttribute()
		{
			if (!this.currentIsNode)
			{
				return false;
			}
			int firstAttribute = this.nodes[this.currentNode].FirstAttribute;
			if (firstAttribute == 0)
			{
				return false;
			}
			this.currentAttr = firstAttribute;
			this.currentIsAttr = true;
			this.currentIsNode = false;
			return true;
		}

		public override bool MoveToFirstChild()
		{
			if (!this.currentIsNode)
			{
				return false;
			}
			int firstChild = this.nodes[this.currentNode].FirstChild;
			if (firstChild == 0)
			{
				return false;
			}
			this.currentNode = firstChild;
			return true;
		}

		private bool moveToSpecifiedNamespace(int cur, XPathNamespaceScope namespaceScope)
		{
			if (cur == 0)
			{
				return false;
			}
			if (namespaceScope == XPathNamespaceScope.Local && this.namespaces[cur].DeclaredElement != this.currentNode)
			{
				return false;
			}
			if (namespaceScope != XPathNamespaceScope.All && this.namespaces[cur].Namespace == 2)
			{
				return false;
			}
			if (cur != 0)
			{
				this.moveToNamespace(cur);
				return true;
			}
			return false;
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			if (!this.currentIsNode)
			{
				return false;
			}
			int firstNamespace = this.nodes[this.currentNode].FirstNamespace;
			return this.moveToSpecifiedNamespace(firstNamespace, namespaceScope);
		}

		public override bool MoveToId(string id)
		{
			if (this.idTable.ContainsKey(id))
			{
				this.currentNode = (int)this.idTable[id];
				this.currentIsNode = true;
				this.currentIsAttr = false;
				return true;
			}
			return false;
		}

		private void moveToNamespace(int nsNode)
		{
			this.currentIsNode = (this.currentIsAttr = false);
			this.currentNs = nsNode;
		}

		public override bool MoveToNamespace(string name)
		{
			int num = this.nodes[this.currentNode].FirstNamespace;
			if (num == 0)
			{
				return false;
			}
			while (num != 0)
			{
				if (this.atomicStringPool[this.namespaces[num].Name] == name)
				{
					this.moveToNamespace(num);
					return true;
				}
				num = this.namespaces[num].NextNamespace;
			}
			return false;
		}

		public override bool MoveToNext()
		{
			if (this.currentIsAttr)
			{
				return false;
			}
			int nextSibling = this.nodes[this.currentNode].NextSibling;
			if (nextSibling == 0)
			{
				return false;
			}
			this.currentNode = nextSibling;
			this.currentIsNode = true;
			return true;
		}

		public override bool MoveToNextAttribute()
		{
			if (!this.currentIsAttr)
			{
				return false;
			}
			int nextAttribute = this.attributes[this.currentAttr].NextAttribute;
			if (nextAttribute == 0)
			{
				return false;
			}
			this.currentAttr = nextAttribute;
			return true;
		}

		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			if (this.currentIsAttr || this.currentIsNode)
			{
				return false;
			}
			int nextNamespace = this.namespaces[this.currentNs].NextNamespace;
			return this.moveToSpecifiedNamespace(nextNamespace, namespaceScope);
		}

		public override bool MoveToParent()
		{
			if (!this.currentIsNode)
			{
				this.currentIsNode = true;
				this.currentIsAttr = false;
				return true;
			}
			int parent = this.nodes[this.currentNode].Parent;
			if (parent == 0)
			{
				return false;
			}
			this.currentNode = parent;
			return true;
		}

		public override bool MoveToPrevious()
		{
			if (this.currentIsAttr)
			{
				return false;
			}
			int previousSibling = this.nodes[this.currentNode].PreviousSibling;
			if (previousSibling == 0)
			{
				return false;
			}
			this.currentNode = previousSibling;
			this.currentIsNode = true;
			return true;
		}

		public override void MoveToRoot()
		{
			this.currentNode = 1;
			this.currentIsNode = true;
			this.currentIsAttr = false;
		}
	}
}
