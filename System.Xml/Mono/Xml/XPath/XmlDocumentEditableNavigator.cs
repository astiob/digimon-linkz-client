using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
	internal class XmlDocumentEditableNavigator : XPathNavigator, IHasXmlNode
	{
		private static readonly bool isXmlDocumentNavigatorImpl = typeof(XmlDocumentEditableNavigator).Assembly == typeof(XmlDocument).Assembly;

		private XPathEditableDocument document;

		private XPathNavigator navigator;

		public XmlDocumentEditableNavigator(XPathEditableDocument doc)
		{
			this.document = doc;
			if (XmlDocumentEditableNavigator.isXmlDocumentNavigatorImpl)
			{
				this.navigator = new XmlDocumentNavigator(doc.Node);
			}
			else
			{
				this.navigator = doc.CreateNavigator();
			}
		}

		public XmlDocumentEditableNavigator(XmlDocumentEditableNavigator nav)
		{
			this.document = nav.document;
			this.navigator = nav.navigator.Clone();
		}

		public override string BaseURI
		{
			get
			{
				return this.navigator.BaseURI;
			}
		}

		public override bool CanEdit
		{
			get
			{
				return true;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.navigator.IsEmptyElement;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.navigator.LocalName;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.navigator.NameTable;
			}
		}

		public override string Name
		{
			get
			{
				return this.navigator.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return this.navigator.NamespaceURI;
			}
		}

		public override XPathNodeType NodeType
		{
			get
			{
				return this.navigator.NodeType;
			}
		}

		public override string Prefix
		{
			get
			{
				return this.navigator.Prefix;
			}
		}

		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this.navigator.SchemaInfo;
			}
		}

		public override object UnderlyingObject
		{
			get
			{
				return this.navigator.UnderlyingObject;
			}
		}

		public override string Value
		{
			get
			{
				return this.navigator.Value;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.navigator.XmlLang;
			}
		}

		public override bool HasChildren
		{
			get
			{
				return this.navigator.HasChildren;
			}
		}

		public override bool HasAttributes
		{
			get
			{
				return this.navigator.HasAttributes;
			}
		}

		public override XPathNavigator Clone()
		{
			return new XmlDocumentEditableNavigator(this);
		}

		public override XPathNavigator CreateNavigator()
		{
			return this.navigator.Clone();
		}

		public XmlNode GetNode()
		{
			return ((IHasXmlNode)this.navigator).GetNode();
		}

		public override bool IsSamePosition(XPathNavigator other)
		{
			XmlDocumentEditableNavigator xmlDocumentEditableNavigator = other as XmlDocumentEditableNavigator;
			if (xmlDocumentEditableNavigator != null)
			{
				return this.navigator.IsSamePosition(xmlDocumentEditableNavigator.navigator);
			}
			return this.navigator.IsSamePosition(xmlDocumentEditableNavigator);
		}

		public override bool MoveTo(XPathNavigator other)
		{
			XmlDocumentEditableNavigator xmlDocumentEditableNavigator = other as XmlDocumentEditableNavigator;
			if (xmlDocumentEditableNavigator != null)
			{
				return this.navigator.MoveTo(xmlDocumentEditableNavigator.navigator);
			}
			return this.navigator.MoveTo(xmlDocumentEditableNavigator);
		}

		public override bool MoveToFirstAttribute()
		{
			return this.navigator.MoveToFirstAttribute();
		}

		public override bool MoveToFirstChild()
		{
			return this.navigator.MoveToFirstChild();
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope scope)
		{
			return this.navigator.MoveToFirstNamespace(scope);
		}

		public override bool MoveToId(string id)
		{
			return this.navigator.MoveToId(id);
		}

		public override bool MoveToNext()
		{
			return this.navigator.MoveToNext();
		}

		public override bool MoveToNextAttribute()
		{
			return this.navigator.MoveToNextAttribute();
		}

		public override bool MoveToNextNamespace(XPathNamespaceScope scope)
		{
			return this.navigator.MoveToNextNamespace(scope);
		}

		public override bool MoveToParent()
		{
			return this.navigator.MoveToParent();
		}

		public override bool MoveToPrevious()
		{
			return this.navigator.MoveToPrevious();
		}

		public override XmlWriter AppendChild()
		{
			XmlNode node = ((IHasXmlNode)this.navigator).GetNode();
			if (node == null)
			{
				throw new InvalidOperationException("Should not happen.");
			}
			return new XmlDocumentInsertionWriter(node, null);
		}

		public override void DeleteRange(XPathNavigator lastSiblingToDelete)
		{
			if (lastSiblingToDelete == null)
			{
				throw new ArgumentNullException();
			}
			XmlNode node = ((IHasXmlNode)this.navigator).GetNode();
			XmlNode xmlNode = null;
			if (lastSiblingToDelete is IHasXmlNode)
			{
				xmlNode = ((IHasXmlNode)lastSiblingToDelete).GetNode();
			}
			if (!this.navigator.MoveToParent())
			{
				throw new InvalidOperationException("There is no parent to remove current node.");
			}
			if (xmlNode == null || node.ParentNode != xmlNode.ParentNode)
			{
				throw new InvalidOperationException("Argument XPathNavigator has different parent node.");
			}
			XmlNode parentNode = node.ParentNode;
			bool flag = true;
			XmlNode xmlNode2 = node;
			while (flag)
			{
				flag = (xmlNode2 != xmlNode);
				XmlNode nextSibling = xmlNode2.NextSibling;
				parentNode.RemoveChild(xmlNode2);
				xmlNode2 = nextSibling;
			}
		}

		public override XmlWriter ReplaceRange(XPathNavigator nav)
		{
			if (nav == null)
			{
				throw new ArgumentNullException();
			}
			XmlNode start = ((IHasXmlNode)this.navigator).GetNode();
			XmlNode end = null;
			if (nav is IHasXmlNode)
			{
				end = ((IHasXmlNode)nav).GetNode();
			}
			if (end == null || start.ParentNode != end.ParentNode)
			{
				throw new InvalidOperationException("Argument XPathNavigator has different parent node.");
			}
			XmlDocumentInsertionWriter xmlDocumentInsertionWriter = (XmlDocumentInsertionWriter)this.InsertBefore();
			XPathNavigator prev = this.Clone();
			if (!prev.MoveToPrevious())
			{
				prev = null;
			}
			XPathNavigator parentNav = this.Clone();
			parentNav.MoveToParent();
			xmlDocumentInsertionWriter.Closed += delegate(XmlWriter writer)
			{
				XmlNode parentNode = start.ParentNode;
				bool flag = true;
				XmlNode xmlNode = start;
				while (flag)
				{
					flag = (xmlNode != end);
					XmlNode nextSibling = xmlNode.NextSibling;
					parentNode.RemoveChild(xmlNode);
					xmlNode = nextSibling;
				}
				if (prev != null)
				{
					this.MoveTo(prev);
					this.MoveToNext();
				}
				else
				{
					this.MoveTo(parentNav);
					this.MoveToFirstChild();
				}
			};
			return xmlDocumentInsertionWriter;
		}

		public override XmlWriter InsertBefore()
		{
			XmlNode node = ((IHasXmlNode)this.navigator).GetNode();
			return new XmlDocumentInsertionWriter(node.ParentNode, node);
		}

		public override XmlWriter CreateAttributes()
		{
			XmlNode node = ((IHasXmlNode)this.navigator).GetNode();
			return new XmlDocumentAttributeWriter(node);
		}

		public override void DeleteSelf()
		{
			XmlNode node = ((IHasXmlNode)this.navigator).GetNode();
			XmlAttribute xmlAttribute = node as XmlAttribute;
			if (xmlAttribute != null)
			{
				if (xmlAttribute.OwnerElement == null)
				{
					throw new InvalidOperationException("This attribute node cannot be removed since it has no owner element.");
				}
				this.navigator.MoveToParent();
				xmlAttribute.OwnerElement.RemoveAttributeNode(xmlAttribute);
			}
			else
			{
				if (node.ParentNode == null)
				{
					throw new InvalidOperationException("This node cannot be removed since it has no parent.");
				}
				this.navigator.MoveToParent();
				node.ParentNode.RemoveChild(node);
			}
		}

		public override void ReplaceSelf(XmlReader reader)
		{
			XmlNode node = ((IHasXmlNode)this.navigator).GetNode();
			XmlNode parentNode = node.ParentNode;
			if (parentNode == null)
			{
				throw new InvalidOperationException("This node cannot be removed since it has no parent.");
			}
			bool flag = false;
			if (!this.MoveToPrevious())
			{
				this.MoveToParent();
			}
			else
			{
				flag = true;
			}
			XmlDocument xmlDocument = (parentNode.NodeType != XmlNodeType.Document) ? parentNode.OwnerDocument : (parentNode as XmlDocument);
			bool flag2 = false;
			if (reader.ReadState == ReadState.Initial)
			{
				reader.Read();
				if (reader.EOF)
				{
					flag2 = true;
				}
				else
				{
					while (!reader.EOF)
					{
						parentNode.AppendChild(xmlDocument.ReadNode(reader));
					}
				}
			}
			else if (reader.EOF)
			{
				flag2 = true;
			}
			else
			{
				parentNode.AppendChild(xmlDocument.ReadNode(reader));
			}
			if (flag2)
			{
				throw new InvalidOperationException("Content is required in argument XmlReader to replace current node.");
			}
			parentNode.RemoveChild(node);
			if (flag)
			{
				this.MoveToNext();
			}
			else
			{
				this.MoveToFirstChild();
			}
		}

		public override void SetValue(string value)
		{
			XmlNode node = ((IHasXmlNode)this.navigator).GetNode();
			while (node.FirstChild != null)
			{
				node.RemoveChild(node.FirstChild);
			}
			node.InnerText = value;
		}

		public override void MoveToRoot()
		{
			this.navigator.MoveToRoot();
		}

		public override bool MoveToNamespace(string name)
		{
			return this.navigator.MoveToNamespace(name);
		}

		public override bool MoveToFirst()
		{
			return this.navigator.MoveToFirst();
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return this.navigator.MoveToAttribute(localName, namespaceURI);
		}

		public override bool IsDescendant(XPathNavigator nav)
		{
			XmlDocumentEditableNavigator xmlDocumentEditableNavigator = nav as XmlDocumentEditableNavigator;
			if (xmlDocumentEditableNavigator != null)
			{
				return this.navigator.IsDescendant(xmlDocumentEditableNavigator.navigator);
			}
			return this.navigator.IsDescendant(nav);
		}

		public override string GetNamespace(string name)
		{
			return this.navigator.GetNamespace(name);
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			return this.navigator.GetAttribute(localName, namespaceURI);
		}

		public override XmlNodeOrder ComparePosition(XPathNavigator nav)
		{
			XmlDocumentEditableNavigator xmlDocumentEditableNavigator = nav as XmlDocumentEditableNavigator;
			if (xmlDocumentEditableNavigator != null)
			{
				return this.navigator.ComparePosition(xmlDocumentEditableNavigator.navigator);
			}
			return this.navigator.ComparePosition(nav);
		}
	}
}
