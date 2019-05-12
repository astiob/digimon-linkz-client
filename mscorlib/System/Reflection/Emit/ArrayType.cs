using System;
using System.Text;

namespace System.Reflection.Emit
{
	internal class ArrayType : DerivedType
	{
		private int rank;

		internal ArrayType(Type elementType, int rank) : base(elementType)
		{
			this.rank = rank;
		}

		protected override bool IsArrayImpl()
		{
			return true;
		}

		public override int GetArrayRank()
		{
			return (this.rank != 0) ? this.rank : 1;
		}

		public override Type BaseType
		{
			get
			{
				return typeof(Array);
			}
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			if (((ModuleBuilder)this.elementType.Module).assemblyb.IsCompilerContext)
			{
				return (this.elementType.Attributes & TypeAttributes.VisibilityMask) | TypeAttributes.Sealed | TypeAttributes.Serializable;
			}
			return this.elementType.Attributes;
		}

		internal override string FormatName(string elementName)
		{
			if (elementName == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(elementName);
			stringBuilder.Append("[");
			for (int i = 1; i < this.rank; i++)
			{
				stringBuilder.Append(",");
			}
			if (this.rank == 1)
			{
				stringBuilder.Append("*");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}
	}
}
