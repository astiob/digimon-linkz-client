using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml
{
	internal class XmlIteratorNodeList : XmlNodeList
	{
		private XPathNodeIterator source;

		private XPathNodeIterator iterator;

		private ArrayList list;

		private bool finished;

		public XmlIteratorNodeList(XPathNodeIterator iter)
		{
			this.source = iter;
			this.iterator = iter.Clone();
			this.list = new ArrayList();
		}

		public override int Count
		{
			get
			{
				return this.iterator.Count;
			}
		}

		public override IEnumerator GetEnumerator()
		{
			if (this.finished)
			{
				return this.list.GetEnumerator();
			}
			return new XmlIteratorNodeList.XPathNodeIteratorNodeListIterator(this.source);
		}

		public override XmlNode Item(int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (index < this.list.Count)
			{
				return (XmlNode)this.list[index];
			}
			index++;
			while (this.iterator.CurrentPosition < index)
			{
				if (!this.iterator.MoveNext())
				{
					this.finished = true;
					return null;
				}
				this.list.Add(((IHasXmlNode)this.iterator.Current).GetNode());
			}
			return (XmlNode)this.list[index - 1];
		}

		private class XPathNodeIteratorNodeListIterator : IEnumerator
		{
			private XPathNodeIterator iter;

			private XPathNodeIterator source;

			public XPathNodeIteratorNodeListIterator(XPathNodeIterator source)
			{
				this.source = source;
				this.Reset();
			}

			public bool MoveNext()
			{
				return this.iter.MoveNext();
			}

			public object Current
			{
				get
				{
					return ((IHasXmlNode)this.iter.Current).GetNode();
				}
			}

			public void Reset()
			{
				this.iter = this.source.Clone();
			}
		}
	}
}
