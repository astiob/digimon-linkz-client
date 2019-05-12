using System;
using System.Collections;

namespace Mono.Xml
{
	internal class DTDAttListDeclaration : DTDNode
	{
		private string name;

		private Hashtable attributeOrders = new Hashtable();

		private ArrayList attributes = new ArrayList();

		internal DTDAttListDeclaration(DTDObjectModel root)
		{
			base.SetRoot(root);
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public DTDAttributeDefinition this[int i]
		{
			get
			{
				return this.Get(i);
			}
		}

		public DTDAttributeDefinition this[string name]
		{
			get
			{
				return this.Get(name);
			}
		}

		public DTDAttributeDefinition Get(int i)
		{
			return this.attributes[i] as DTDAttributeDefinition;
		}

		public DTDAttributeDefinition Get(string name)
		{
			object obj = this.attributeOrders[name];
			if (obj != null)
			{
				return this.attributes[(int)obj] as DTDAttributeDefinition;
			}
			return null;
		}

		public IList Definitions
		{
			get
			{
				return this.attributes;
			}
		}

		public void Add(DTDAttributeDefinition def)
		{
			if (this.attributeOrders[def.Name] != null)
			{
				throw new InvalidOperationException(string.Format("Attribute definition for {0} was already added at element {1}.", def.Name, this.Name));
			}
			def.SetRoot(base.Root);
			this.attributeOrders.Add(def.Name, this.attributes.Count);
			this.attributes.Add(def);
		}

		public int Count
		{
			get
			{
				return this.attributeOrders.Count;
			}
		}
	}
}
