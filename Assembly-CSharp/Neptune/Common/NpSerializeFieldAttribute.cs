using CROOZ.Chopin.Core;
using System;

namespace Neptune.Common
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class NpSerializeFieldAttribute : CpSerializeFieldAttribute
	{
		public NpSerializeFieldAttribute(string name) : base(int.MaxValue, name)
		{
		}

		public NpSerializeFieldAttribute(int order) : base(order, null)
		{
		}

		public NpSerializeFieldAttribute(int order, string name) : base(order, name)
		{
		}
	}
}
