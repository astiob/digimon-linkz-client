using Mono.Xml;
using System;
using System.Collections;

namespace System.Xml
{
	/// <summary>Represents a collection of nodes that can be accessed by name or index.</summary>
	public class XmlNamedNodeMap : IEnumerable
	{
		private static readonly IEnumerator emptyEnumerator = new XmlNode[0].GetEnumerator();

		private XmlNode parent;

		private ArrayList nodeList;

		private bool readOnly;

		internal XmlNamedNodeMap(XmlNode parent)
		{
			this.parent = parent;
		}

		private ArrayList NodeList
		{
			get
			{
				if (this.nodeList == null)
				{
					this.nodeList = new ArrayList();
				}
				return this.nodeList;
			}
		}

		/// <summary>Gets the number of nodes in the XmlNamedNodeMap.</summary>
		/// <returns>The number of nodes.</returns>
		public virtual int Count
		{
			get
			{
				return (this.nodeList != null) ? this.nodeList.Count : 0;
			}
		}

		/// <summary>Provides support for the "foreach" style iteration over the collection of nodes in the XmlNamedNodeMap.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" />.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			if (this.nodeList == null)
			{
				return XmlNamedNodeMap.emptyEnumerator;
			}
			return this.nodeList.GetEnumerator();
		}

		/// <summary>Retrieves an <see cref="T:System.Xml.XmlNode" /> specified by name.</summary>
		/// <returns>An XmlNode with the specified name or null if a matching node is not found.</returns>
		/// <param name="name">The qualified name of the node to retrieve. It is matched against the <see cref="P:System.Xml.XmlNode.Name" /> property of the matching node. </param>
		public virtual XmlNode GetNamedItem(string name)
		{
			if (this.nodeList == null)
			{
				return null;
			}
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				XmlNode xmlNode = (XmlNode)this.nodeList[i];
				if (xmlNode.Name == name)
				{
					return xmlNode;
				}
			}
			return null;
		}

		/// <summary>Retrieves a node with the matching <see cref="P:System.Xml.XmlNode.LocalName" /> and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNode" /> with the matching local name and namespace URI or null if a matching node was not found.</returns>
		/// <param name="localName">The local name of the node to retrieve. </param>
		/// <param name="namespaceURI">The namespace Uniform Resource Identifier (URI) of the node to retrieve. </param>
		public virtual XmlNode GetNamedItem(string localName, string namespaceURI)
		{
			if (this.nodeList == null)
			{
				return null;
			}
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				XmlNode xmlNode = (XmlNode)this.nodeList[i];
				if (xmlNode.LocalName == localName && xmlNode.NamespaceURI == namespaceURI)
				{
					return xmlNode;
				}
			}
			return null;
		}

		/// <summary>Retrieves the node at the specified index in the XmlNamedNodeMap.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> at the specified index. If <paramref name="index" /> is less than 0 or greater than or equal to the <see cref="P:System.Xml.XmlNamedNodeMap.Count" /> property, null is returned.</returns>
		/// <param name="index">The index position of the node to retrieve from the XmlNamedNodeMap. The index is zero-based; therefore, the index of the first node is 0 and the index of the last node is <see cref="P:System.Xml.XmlNamedNodeMap.Count" /> -1. </param>
		public virtual XmlNode Item(int index)
		{
			if (this.nodeList == null || index < 0 || index >= this.nodeList.Count)
			{
				return null;
			}
			return (XmlNode)this.nodeList[index];
		}

		/// <summary>Removes the node from the XmlNamedNodeMap.</summary>
		/// <returns>The XmlNode removed from this XmlNamedNodeMap or null if a matching node was not found.</returns>
		/// <param name="name">The qualified name of the node to remove. The name is matched against the <see cref="P:System.Xml.XmlNode.Name" /> property of the matching node. </param>
		public virtual XmlNode RemoveNamedItem(string name)
		{
			if (this.nodeList == null)
			{
				return null;
			}
			int i = 0;
			while (i < this.nodeList.Count)
			{
				XmlNode xmlNode = (XmlNode)this.nodeList[i];
				if (xmlNode.Name == name)
				{
					if (xmlNode.IsReadOnly)
					{
						throw new InvalidOperationException("Cannot remove. This node is read only: " + name);
					}
					this.nodeList.Remove(xmlNode);
					XmlAttribute xmlAttribute = xmlNode as XmlAttribute;
					if (xmlAttribute != null)
					{
						DTDAttributeDefinition attributeDefinition = xmlAttribute.GetAttributeDefinition();
						if (attributeDefinition != null && attributeDefinition.DefaultValue != null)
						{
							XmlAttribute xmlAttribute2 = xmlAttribute.OwnerDocument.CreateAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, true, false);
							xmlAttribute2.Value = attributeDefinition.DefaultValue;
							xmlAttribute2.SetDefault();
							xmlAttribute.OwnerElement.SetAttributeNode(xmlAttribute2);
						}
					}
					return xmlNode;
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		/// <summary>Removes a node with the matching <see cref="P:System.Xml.XmlNode.LocalName" /> and <see cref="P:System.Xml.XmlNode.NamespaceURI" />.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> removed or null if a matching node was not found.</returns>
		/// <param name="localName">The local name of the node to remove. </param>
		/// <param name="namespaceURI">The namespace URI of the node to remove. </param>
		public virtual XmlNode RemoveNamedItem(string localName, string namespaceURI)
		{
			if (this.nodeList == null)
			{
				return null;
			}
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				XmlNode xmlNode = (XmlNode)this.nodeList[i];
				if (xmlNode.LocalName == localName && xmlNode.NamespaceURI == namespaceURI)
				{
					this.nodeList.Remove(xmlNode);
					return xmlNode;
				}
			}
			return null;
		}

		/// <summary>Adds an <see cref="T:System.Xml.XmlNode" /> using its <see cref="P:System.Xml.XmlNode.Name" /> property </summary>
		/// <returns>If the <paramref name="node" /> replaces an existing node with the same name, the old node is returned; otherwise, null is returned.</returns>
		/// <param name="node">An XmlNode to store in the XmlNamedNodeMap. If a node with that name is already present in the map, it is replaced by the new one. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="node" /> was created from a different <see cref="T:System.Xml.XmlDocument" /> than the one that created the XmlNamedNodeMap; or the XmlNamedNodeMap is read-only. </exception>
		public virtual XmlNode SetNamedItem(XmlNode node)
		{
			return this.SetNamedItem(node, -1, true);
		}

		internal XmlNode SetNamedItem(XmlNode node, bool raiseEvent)
		{
			return this.SetNamedItem(node, -1, raiseEvent);
		}

		internal XmlNode SetNamedItem(XmlNode node, int pos, bool raiseEvent)
		{
			if (this.readOnly || node.OwnerDocument != this.parent.OwnerDocument)
			{
				throw new ArgumentException("Cannot add to NodeMap.");
			}
			if (raiseEvent)
			{
				this.parent.OwnerDocument.onNodeInserting(node, this.parent);
			}
			XmlNode result;
			try
			{
				for (int i = 0; i < this.NodeList.Count; i++)
				{
					XmlNode xmlNode = (XmlNode)this.nodeList[i];
					if (xmlNode.LocalName == node.LocalName && xmlNode.NamespaceURI == node.NamespaceURI)
					{
						this.nodeList.Remove(xmlNode);
						if (pos < 0)
						{
							this.nodeList.Add(node);
						}
						else
						{
							this.nodeList.Insert(pos, node);
						}
						return xmlNode;
					}
				}
				if (pos < 0)
				{
					this.nodeList.Add(node);
				}
				else
				{
					this.nodeList.Insert(pos, node);
				}
				result = node;
			}
			finally
			{
				if (raiseEvent)
				{
					this.parent.OwnerDocument.onNodeInserted(node, this.parent);
				}
			}
			return result;
		}

		internal ArrayList Nodes
		{
			get
			{
				return this.NodeList;
			}
		}
	}
}
