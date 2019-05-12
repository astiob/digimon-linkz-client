using System;

namespace System.Runtime.InteropServices
{
	/// <summary>The StructLayoutAttribute class allows the user to control the physical layout of the data fields of a class or structure.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	[ComVisible(true)]
	public sealed class StructLayoutAttribute : Attribute
	{
		/// <summary>Indicates how string data fields within the class should be marshaled as LPWSTR or LPSTR by default.</summary>
		public CharSet CharSet = CharSet.Auto;

		/// <summary>Controls the alignment of data fields of a class or structure in memory.</summary>
		public int Pack = 8;

		/// <summary>Indicates the absolute size of the class or structure.</summary>
		public int Size;

		private LayoutKind lkind;

		/// <summary>Initalizes a new instance of the <see cref="T:System.Runtime.InteropServices.StructLayoutAttribute" /> class with the specified <see cref="T:System.Runtime.InteropServices.LayoutKind" /> enumeration member.</summary>
		/// <param name="layoutKind">One of the <see cref="T:System.Runtime.InteropServices.LayoutKind" /> values that specifes how the class or structure should be arranged. </param>
		public StructLayoutAttribute(short layoutKind)
		{
			this.lkind = (LayoutKind)layoutKind;
		}

		/// <summary>Initalizes a new instance of the <see cref="T:System.Runtime.InteropServices.StructLayoutAttribute" /> class with the specified <see cref="T:System.Runtime.InteropServices.LayoutKind" /> enumeration member.</summary>
		/// <param name="layoutKind">One of the <see cref="T:System.Runtime.InteropServices.LayoutKind" /> values that specifes how the class or structure should be arranged. </param>
		public StructLayoutAttribute(LayoutKind layoutKind)
		{
			this.lkind = layoutKind;
		}

		/// <summary>Gets the <see cref="T:System.Runtime.InteropServices.LayoutKind" /> value that specifies how the class or structure is arranged.</summary>
		/// <returns>The <see cref="T:System.Runtime.InteropServices.LayoutKind" /> value that specifies how the class or structure is arranged.</returns>
		public LayoutKind Value
		{
			get
			{
				return this.lkind;
			}
		}
	}
}
