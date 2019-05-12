using System;

namespace System.Reflection.Emit
{
	internal class ByRefType : DerivedType
	{
		internal ByRefType(Type elementType) : base(elementType)
		{
		}

		protected override bool IsByRefImpl()
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
			return elementName + "&";
		}

		public override Type MakeArrayType()
		{
			throw new ArgumentException("Cannot create an array type of a byref type");
		}

		public override Type MakeArrayType(int rank)
		{
			throw new ArgumentException("Cannot create an array type of a byref type");
		}

		public override Type MakeByRefType()
		{
			throw new ArgumentException("Cannot create a byref type of an already byref type");
		}

		public override Type MakePointerType()
		{
			throw new ArgumentException("Cannot create a pointer type of a byref type");
		}
	}
}
