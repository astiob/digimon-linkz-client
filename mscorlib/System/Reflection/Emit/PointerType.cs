using System;

namespace System.Reflection.Emit
{
	internal class PointerType : DerivedType
	{
		internal PointerType(Type elementType) : base(elementType)
		{
		}

		protected override bool IsPointerImpl()
		{
			return true;
		}

		public override Type BaseType
		{
			get
			{
				return typeof(Array);
			}
		}

		internal override string FormatName(string elementName)
		{
			if (elementName == null)
			{
				return null;
			}
			return elementName + "*";
		}
	}
}
