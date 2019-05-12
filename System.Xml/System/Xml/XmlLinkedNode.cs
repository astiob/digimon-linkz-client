using System;

namespace System.Xml
{
	/// <summary>Gets the node immediately preceding or following this node.</summary>
	public abstract class XmlLinkedNode : XmlNode
	{
		private XmlLinkedNode nextSibling;

		internal XmlLinkedNode(XmlDocument doc) : base(doc)
		{
		}

		internal bool IsRooted
		{
			get
			{
				for (XmlNode parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
				{
					if (parentNode.NodeType == XmlNodeType.Document)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>Gets the node immediately following this node.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> immediately following this node or null if one does not exist.</returns>
		public override XmlNode NextSibling
		{
			get
			{
				return (this.ParentNode != null && this.ParentNode.LastChild != this) ? this.nextSibling : null;
			}
		}

		internal XmlLinkedNode NextLinkedSibling
		{
			get
			{
				return this.nextSibling;
			}
			set
			{
				this.nextSibling = value;
			}
		}

		/// <summary>Gets the node immediately preceding this node.</summary>
		/// <returns>The preceding <see cref="T:System.Xml.XmlNode" /> or null if one does not exist.</returns>
		public override XmlNode PreviousSibling
		{
			get
			{
				if (this.ParentNode != null)
				{
					XmlNode firstChild = this.ParentNode.FirstChild;
					if (firstChild != this)
					{
						while (firstChild.NextSibling != this)
						{
							if ((firstChild = firstChild.NextSibling) == null)
							{
								goto IL_39;
							}
						}
						return firstChild;
					}
				}
				IL_39:
				return null;
			}
		}
	}
}
