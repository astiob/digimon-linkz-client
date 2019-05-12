using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Specifies a default interface to expose to COM. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComDefaultInterfaceAttribute : Attribute
	{
		private Type _type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ComDefaultInterfaceAttribute" /> class with the specified <see cref="T:System.Type" /> object as the default interface exposed to COM.</summary>
		/// <param name="defaultInterface">A <see cref="T:System.Type" /> value indicating the default interface to expose to COM. </param>
		public ComDefaultInterfaceAttribute(Type defaultInterface)
		{
			this._type = defaultInterface;
		}

		/// <summary>Gets the <see cref="T:System.Type" /> object that specifies the default interface to expose to COM.</summary>
		/// <returns>The <see cref="T:System.Type" /> object that specifies the default interface to expose to COM.</returns>
		public Type Value
		{
			get
			{
				return this._type;
			}
		}
	}
}
