using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Contains the <see cref="T:System.Runtime.InteropServices.FUNCFLAGS" /> that were originally imported for this method from the COM type library.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class TypeLibFuncAttribute : Attribute
	{
		private TypeLibFuncFlags flags;

		/// <summary>Initializes a new instance of the TypeLibFuncAttribute class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value.</summary>
		/// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for the attributed method as found in the type library it was imported from. </param>
		public TypeLibFuncAttribute(short flags)
		{
			this.flags = (TypeLibFuncFlags)flags;
		}

		/// <summary>Initializes a new instance of the TypeLibFuncAttribute class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value.</summary>
		/// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for the attributed method as found in the type library it was imported from. </param>
		public TypeLibFuncAttribute(TypeLibFuncFlags flags)
		{
			this.flags = flags;
		}

		/// <summary>Gets the <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for this method.</summary>
		/// <returns>The <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for this method.</returns>
		public TypeLibFuncFlags Value
		{
			get
			{
				return this.flags;
			}
		}
	}
}
