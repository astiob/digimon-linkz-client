using System;
using System.Collections;

namespace System.Xml
{
	internal class XmlNodeListChildren : XmlNodeList
	{
		private IHasXmlChildNode parent;

		public XmlNodeListChildren(IHasXmlChildNode parent)
		{
			this.parent = parent;
		}

		public override int Count
		{
			get
			{
				int num = 0;
				if (this.parent.LastLinkedChild != null)
				{
					XmlLinkedNode nextLinkedSibling = this.parent.LastLinkedChild.NextLinkedSibling;
					num = 1;
					while (!object.ReferenceEquals(nextLinkedSibling, this.parent.LastLinkedChild))
					{
						nextLinkedSibling = nextLinkedSibling.NextLinkedSibling;
						num++;
					}
				}
				return num;
			}
		}

		public override IEnumerator GetEnumerator()
		{
			return new XmlNodeListChildren.Enumerator(this.parent);
		}

		public override XmlNode Item(int index)
		{
			XmlNode result = null;
			if (this.Count <= index)
			{
				return null;
			}
			if (index >= 0 && this.parent.LastLinkedChild != null)
			{
				XmlLinkedNode nextLinkedSibling = this.parent.LastLinkedChild.NextLinkedSibling;
				int num = 0;
				while (num < index && !object.ReferenceEquals(nextLinkedSibling, this.parent.LastLinkedChild))
				{
					nextLinkedSibling = nextLinkedSibling.NextLinkedSibling;
					num++;
				}
				if (num == index)
				{
					result = nextLinkedSibling;
				}
			}
			return result;
		}

		private class Enumerator : IEnumerator
		{
			private IHasXmlChildNode parent;

			private XmlLinkedNode currentChild;

			private bool passedLastNode;

			internal Enumerator(IHasXmlChildNode parent)
			{
				this.currentChild = null;
				this.parent = parent;
				this.passedLastNode = false;
			}

			public virtual object Current
			{
				get
				{
					if (this.currentChild == null || this.parent.LastLinkedChild == null || this.passedLastNode)
					{
						throw new InvalidOperationException();
					}
					return this.currentChild;
				}
			}

			public virtual bool MoveNext()
			{
				bool result = true;
				if (this.parent.LastLinkedChild == null)
				{
					result = false;
				}
				else if (this.currentChild == null)
				{
					this.currentChild = this.parent.LastLinkedChild.NextLinkedSibling;
				}
				else if (object.ReferenceEquals(this.currentChild, this.parent.LastLinkedChild))
				{
					result = false;
					this.passedLastNode = true;
				}
				else
				{
					this.currentChild = this.currentChild.NextLinkedSibling;
				}
				return result;
			}

			public virtual void Reset()
			{
				this.currentChild = null;
			}
		}
	}
}
