using System;
using System.Collections;

namespace System.Xml
{
	internal class XmlNodeArrayList : XmlNodeList
	{
		private ArrayList _rgNodes;

		public XmlNodeArrayList(ArrayList rgNodes)
		{
			this._rgNodes = rgNodes;
		}

		public override int Count
		{
			get
			{
				return this._rgNodes.Count;
			}
		}

		public override IEnumerator GetEnumerator()
		{
			return this._rgNodes.GetEnumerator();
		}

		public override XmlNode Item(int index)
		{
			if (index < 0 || this._rgNodes.Count <= index)
			{
				return null;
			}
			return (XmlNode)this._rgNodes[index];
		}
	}
}
