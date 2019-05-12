using System;
using System.Collections.Generic;

namespace Mono.Xml
{
	internal class DTDCollectionBase : DictionaryBase
	{
		private DTDObjectModel root;

		protected DTDCollectionBase(DTDObjectModel root)
		{
			this.root = root;
		}

		protected DTDObjectModel Root
		{
			get
			{
				return this.root;
			}
		}

		public DictionaryBase InnerHashtable
		{
			get
			{
				return this;
			}
		}

		protected void BaseAdd(string name, DTDNode value)
		{
			base.Add(new KeyValuePair<string, DTDNode>(name, value));
		}

		public bool Contains(string key)
		{
			foreach (KeyValuePair<string, DTDNode> keyValuePair in this)
			{
				if (keyValuePair.Key == key)
				{
					return true;
				}
			}
			return false;
		}

		protected object BaseGet(string name)
		{
			foreach (KeyValuePair<string, DTDNode> keyValuePair in this)
			{
				if (keyValuePair.Key == name)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}
	}
}
