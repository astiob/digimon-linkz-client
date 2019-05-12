using System;

namespace CROOZ.Chopin.Core
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class CpSerializeFieldAttribute : Attribute
	{
		private string \uE000;

		private int \uE001;

		public CpSerializeFieldAttribute(string name) : this(int.MaxValue, name)
		{
		}

		public CpSerializeFieldAttribute(int order) : this(order, null)
		{
		}

		public CpSerializeFieldAttribute(int order, string name)
		{
			this.\uE001 = order;
			this.\uE000 = name;
		}

		public string Name
		{
			get
			{
				return this.\uE000;
			}
		}

		public int Order
		{
			get
			{
				return this.\uE001;
			}
		}
	}
}
