using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates the physical position of fields within the unmanaged representation of a class or structure.</summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class FieldOffsetAttribute : Attribute
	{
		private int val;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.FieldOffsetAttribute" /> class with the offset in the structure to the beginning of the field.</summary>
		/// <param name="offset">The offset, in bytes, from the beginning of the structure to the beginning of the field. </param>
		public FieldOffsetAttribute(int offset)
		{
			this.val = offset;
		}

		/// <summary>Gets the offset from the beginning of the structure to the beginning of the field.</summary>
		/// <returns>The offset from the beginning of the structure to the beginning of the field.</returns>
		public int Value
		{
			get
			{
				return this.val;
			}
		}
	}
}
