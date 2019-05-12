using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Specifies the details of how a method is implemented. This class cannot be inherited. </summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class MethodImplAttribute : Attribute
	{
		private MethodImplOptions _val;

		/// <summary>A <see cref="T:System.Runtime.CompilerServices.MethodCodeType" /> value indicating what kind of implementation is provided for this method.</summary>
		public MethodCodeType MethodCodeType;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.MethodImplAttribute" /> class.</summary>
		public MethodImplAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.MethodImplAttribute" /> class with the specified <see cref="T:System.Runtime.CompilerServices.MethodImplOptions" /> value.</summary>
		/// <param name="value">A bitmask representing the desired <see cref="T:System.Runtime.CompilerServices.MethodImplOptions" /> value which specifies properties of the attributed method. </param>
		public MethodImplAttribute(short value)
		{
			this._val = (MethodImplOptions)value;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.MethodImplAttribute" /> class with the specified <see cref="T:System.Runtime.CompilerServices.MethodImplOptions" /> value.</summary>
		/// <param name="methodImplOptions">An enumeration value specifying properties of the attributed method. </param>
		public MethodImplAttribute(MethodImplOptions methodImplOptions)
		{
			this._val = methodImplOptions;
		}

		/// <summary>Gets the <see cref="T:System.Runtime.CompilerServices.MethodImplOptions" /> value describing the attributed method.</summary>
		/// <returns>The <see cref="T:System.Runtime.CompilerServices.MethodImplOptions" /> value describing the attributed method.</returns>
		public MethodImplOptions Value
		{
			get
			{
				return this._val;
			}
		}
	}
}
