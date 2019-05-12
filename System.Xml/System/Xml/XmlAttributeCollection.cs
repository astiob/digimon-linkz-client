using Mono.Xml;
using System;
using System.Collections;

namespace System.Xml
{
	/// <summary>Represents a collection of attributes that can be accessed by name or index.</summary>
	public sealed class XmlAttributeCollection : XmlNamedNodeMap, IEnumerable, ICollection
	{
		private XmlElement ownerElement;

		private XmlDocument ownerDocument;

		internal XmlAttributeCollection(XmlNode parent) : base(parent)
		{
			this.ownerElement = (parent as XmlElement);
			this.ownerDocument = parent.OwnerDocument;
			if (this.ownerElement == null)
			{
				throw new XmlException("invalid construction for XmlAttributeCollection.");
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Xml.XmlAttributeCollection.System.Collections.ICollection.IsSynchronized" />.</summary>
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Xml.XmlAttributeCollection.System.Collections.ICollection.SyncRoot" />.</summary>
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.XmlAttributeCollection.CopyTo(System.Xml.XmlAttribute[],System.Int32)" />.</summary>
		/// <param name="array">The array that is the destination of the objects copied from this collection. </param>
		/// <param name="index">The index in the array where copying begins. </param>
		void ICollection.CopyTo(Array array, int index)
		{
			array.CopyTo(base.Nodes.ToArray(typeof(XmlAttribute)), index);
		}

		private bool IsReadOnly
		{
			get
			{
				return this.ownerElement.IsReadOnly;
			}
		}

		/// <summary>Gets the attribute with the specified name.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlAttribute" /> with the specified name. If the attribute does not exist, this property returns null.</returns>
		/// <param name="name">The qualified name of the attribute. </param>
		public XmlAttribute this[string name]
		{
			get
			{
				return (XmlAttribute)this.GetNamedItem(name);
			}
		}

		/// <summary>Gets the attribute with the specified index.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlAttribute" /> at the specified index.</returns>
		/// <param name="i">The index of the attribute. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index being passed in is out of range. </exception>
		public XmlAttribute this[int i]
		{
			get
			{
				return (XmlAttribute)base.Nodes[i];
			}
		}

		/// <summary>Gets the attribute with the specified local name and namespace Uniform Resource Identifier (URI).</summary>
		/// <returns>The <see cref="T:System.Xml.XmlAttribute" /> with the specified local name and namespace URI. If the attribute does not exist, this property returns null.</returns>
		/// <param name="localName">The local name of the attribute. </param>
		/// <param name="namespaceURI">The namespace URI of the attribute. </param>
		public XmlAttribute this[string localName, string namespaceURI]
		{
			get
			{
				return (XmlAttribute)this.GetNamedItem(localName, namespaceURI);
			}
		}

		/// <summary>Inserts the specified attribute as the last node in the collection.</summary>
		/// <returns>The XmlAttribute to append to the collection.</returns>
		/// <param name="node">The <see cref="T:System.Xml.XmlAttribute" /> to insert. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="node" /> was created from a document different from the one that created this collection. </exception>
		public XmlAttribute Append(XmlAttribute node)
		{
			this.SetNamedItem(node);
			return node;
		}

		/// <summary>Copies all the <see cref="T:System.Xml.XmlAttribute" /> objects from this collection into the given array.</summary>
		/// <param name="array">The array that is the destination of the objects copied from this collection. </param>
		/// <param name="index">The index in the array where copying begins. </param>
		public void CopyTo(XmlAttribute[] array, int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[index + i] = (base.Nodes[i] as XmlAttribute);
			}
		}

		/// <summary>Inserts the specified attribute immediately after the specified reference attribute.</summary>
		/// <returns>The XmlAttribute to insert into the collection.</returns>
		/// <param name="newNode">The <see cref="T:System.Xml.XmlAttribute" /> to insert. </param>
		/// <param name="refNode">The <see cref="T:System.Xml.XmlAttribute" /> that is the reference attribute. <paramref name="newNode" /> is placed after the <paramref name="refNode" />. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newNode" /> was created from a document different from the one that created this collection. Or the <paramref name="refNode" /> is not a member of this collection. </exception>
		public XmlAttribute InsertAfter(XmlAttribute newNode, XmlAttribute refNode)
		{
			if (refNode != null)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (refNode == base.Nodes[i])
					{
						return this.InsertBefore(newNode, (this.Count != i + 1) ? this[i + 1] : null);
					}
				}
				throw new ArgumentException("refNode not found in this collection.");
			}
			if (this.Count == 0)
			{
				return this.InsertBefore(newNode, null);
			}
			return this.InsertBefore(newNode, this[0]);
		}

		/// <summary>Inserts the specified attribute immediately before the specified reference attribute.</summary>
		/// <returns>The XmlAttribute to insert into the collection.</returns>
		/// <param name="newNode">The <see cref="T:System.Xml.XmlAttribute" /> to insert. </param>
		/// <param name="refNode">The <see cref="T:System.Xml.XmlAttribute" /> that is the reference attribute. <paramref name="newNode" /> is placed before the <paramref name="refNode" />. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="newNode" /> was created from a document different from the one that created this collection. Or the <paramref name="refNode" /> is not a member of this collection. </exception>
		public XmlAttribute InsertBefore(XmlAttribute newNode, XmlAttribute refNode)
		{
			if (newNode.OwnerDocument != this.ownerDocument)
			{
				throw new ArgumentException("different document created this newNode.");
			}
			this.ownerDocument.onNodeInserting(newNode, null);
			int num = this.Count;
			if (refNode != null)
			{
				for (int i = 0; i < this.Count; i++)
				{
					XmlNode xmlNode = base.Nodes[i] as XmlNode;
					if (xmlNode == refNode)
					{
						num = i;
						break;
					}
				}
				if (num == this.Count)
				{
					throw new ArgumentException("refNode not found in this collection.");
				}
			}
			base.SetNamedItem(newNode, num, false);
			this.ownerDocument.onNodeInserted(newNode, null);
			return newNode;
		}

		/// <summary>Inserts the specified attribute as the first node in the collection.</summary>
		/// <returns>The XmlAttribute added to the collection.</returns>
		/// <param name="node">The <see cref="T:System.Xml.XmlAttribute" /> to insert. </param>
		public XmlAttribute Prepend(XmlAttribute node)
		{
			return this.InsertAfter(node, null);
		}

		/// <summary>Removes the specified attribute from the collection.</summary>
		/// <returns>The node removed or null if it is not found in the collection.</returns>
		/// <param name="node">The <see cref="T:System.Xml.XmlAttribute" /> to remove. </param>
		public XmlAttribute Remove(XmlAttribute node)
		{
			if (this.IsReadOnly)
			{
				throw new ArgumentException("This attribute collection is read-only.");
			}
			if (node == null)
			{
				throw new ArgumentException("Specified node is null.");
			}
			if (node.OwnerDocument != this.ownerDocument)
			{
				throw new ArgumentException("Specified node is in a different document.");
			}
			if (node.OwnerElement != this.ownerElement)
			{
				throw new ArgumentException("The specified attribute is not contained in the element.");
			}
			XmlAttribute xmlAttribute = null;
			for (int i = 0; i < this.Count; i++)
			{
				XmlAttribute xmlAttribute2 = (XmlAttribute)base.Nodes[i];
				if (xmlAttribute2 == node)
				{
					xmlAttribute = xmlAttribute2;
					break;
				}
			}
			if (xmlAttribute != null)
			{
				this.ownerDocument.onNodeRemoving(node, this.ownerElement);
				base.RemoveNamedItem(xmlAttribute.LocalName, xmlAttribute.NamespaceURI);
				this.RemoveIdenticalAttribute(xmlAttribute);
				this.ownerDocument.onNodeRemoved(node, this.ownerElement);
			}
			DTDAttributeDefinition attributeDefinition = xmlAttribute.GetAttributeDefinition();
			if (attributeDefinition != null && attributeDefinition.DefaultValue != null)
			{
				XmlAttribute xmlAttribute3 = this.ownerDocument.CreateAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, true, false);
				xmlAttribute3.Value = attributeDefinition.DefaultValue;
				xmlAttribute3.SetDefault();
				this.SetNamedItem(xmlAttribute3);
			}
			xmlAttribute.AttributeOwnerElement = null;
			return xmlAttribute;
		}

		/// <summary>Removes all attributes from the collection.</summary>
		public void RemoveAll()
		{
			int i = 0;
			while (i < this.Count)
			{
				XmlAttribute xmlAttribute = this[i];
				if (!xmlAttribute.Specified)
				{
					i++;
				}
				this.Remove(xmlAttribute);
			}
		}

		/// <summary>Removes the attribute corresponding to the specified index from the collection.</summary>
		/// <returns>Returns null if there is no attribute at the specified index.</returns>
		/// <param name="i">The index of the node to remove. The first node has index 0. </param>
		public XmlAttribute RemoveAt(int i)
		{
			if (this.Count <= i)
			{
				return null;
			}
			return this.Remove((XmlAttribute)base.Nodes[i]);
		}

		/// <summary>Adds a <see cref="T:System.Xml.XmlNode" /> using its <see cref="P:System.Xml.XmlNode.Name" /> property </summary>
		/// <returns>If the <paramref name="node" /> replaces an existing node with the same name, the old node is returned; otherwise, the added node is returned.</returns>
		/// <param name="node">An attribute node to store in this collection. The node will later be accessible using the name of the node. If a node with that name is already present in the collection, it is replaced by the new one; otherwise, the node is appended to the end of the collection. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="node" /> was created from a different <see cref="T:System.Xml.XmlDocument" /> than the one that created this collection.This XmlAttributeCollection is read-only. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is an <see cref="T:System.Xml.XmlAttribute" /> that is already an attribute of another <see cref="T:System.Xml.XmlElement" /> object. To re-use attributes in other elements, you must clone the XmlAttribute objects you want to re-use. </exception>
		public override XmlNode SetNamedItem(XmlNode node)
		{
			if (this.IsReadOnly)
			{
				throw new ArgumentException("this AttributeCollection is read only.");
			}
			XmlAttribute xmlAttribute = node as XmlAttribute;
			if (xmlAttribute.OwnerElement == this.ownerElement)
			{
				return node;
			}
			if (xmlAttribute.OwnerElement != null)
			{
				throw new ArgumentException("This attribute is already set to another element.");
			}
			this.ownerElement.OwnerDocument.onNodeInserting(node, this.ownerElement);
			xmlAttribute.AttributeOwnerElement = this.ownerElement;
			XmlNode xmlNode = base.SetNamedItem(node, -1, false);
			this.AdjustIdenticalAttributes(node as XmlAttribute, (xmlNode != node) ? xmlNode : null);
			this.ownerElement.OwnerDocument.onNodeInserted(node, this.ownerElement);
			return xmlNode as XmlAttribute;
		}

		internal void AddIdenticalAttribute()
		{
			this.SetIdenticalAttribute(false);
		}

		internal void RemoveIdenticalAttribute()
		{
			this.SetIdenticalAttribute(true);
		}

		private void SetIdenticalAttribute(bool remove)
		{
			if (this.ownerElement == null)
			{
				return;
			}
			XmlDocumentType documentType = this.ownerDocument.DocumentType;
			if (documentType == null || documentType.DTD == null)
			{
				return;
			}
			DTDElementDeclaration dtdelementDeclaration = documentType.DTD.ElementDecls[this.ownerElement.Name];
			for (int i = 0; i < this.Count; i++)
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Nodes[i];
				DTDAttributeDefinition dtdattributeDefinition = (dtdelementDeclaration != null) ? dtdelementDeclaration.Attributes[xmlAttribute.Name] : null;
				if (dtdattributeDefinition != null && dtdattributeDefinition.Datatype.TokenizedType == XmlTokenizedType.ID)
				{
					if (remove)
					{
						if (this.ownerDocument.GetIdenticalAttribute(xmlAttribute.Value) != null)
						{
							this.ownerDocument.RemoveIdenticalAttribute(xmlAttribute.Value);
							return;
						}
					}
					else
					{
						if (this.ownerDocument.GetIdenticalAttribute(xmlAttribute.Value) != null)
						{
							throw new XmlException(string.Format("ID value {0} already exists in this document.", xmlAttribute.Value));
						}
						this.ownerDocument.AddIdenticalAttribute(xmlAttribute);
						return;
					}
				}
			}
		}

		private void AdjustIdenticalAttributes(XmlAttribute node, XmlNode existing)
		{
			if (this.ownerElement == null)
			{
				return;
			}
			if (existing != null)
			{
				this.RemoveIdenticalAttribute(existing);
			}
			XmlDocumentType documentType = node.OwnerDocument.DocumentType;
			if (documentType == null || documentType.DTD == null)
			{
				return;
			}
			DTDAttListDeclaration dtdattListDeclaration = documentType.DTD.AttListDecls[this.ownerElement.Name];
			DTDAttributeDefinition dtdattributeDefinition = (dtdattListDeclaration != null) ? dtdattListDeclaration.Get(node.Name) : null;
			if (dtdattributeDefinition == null || dtdattributeDefinition.Datatype.TokenizedType != XmlTokenizedType.ID)
			{
				return;
			}
			this.ownerDocument.AddIdenticalAttribute(node);
		}

		private XmlNode RemoveIdenticalAttribute(XmlNode existing)
		{
			if (this.ownerElement == null)
			{
				return existing;
			}
			if (existing != null && this.ownerDocument.GetIdenticalAttribute(existing.Value) != null)
			{
				this.ownerDocument.RemoveIdenticalAttribute(existing.Value);
			}
			return existing;
		}
	}
}
