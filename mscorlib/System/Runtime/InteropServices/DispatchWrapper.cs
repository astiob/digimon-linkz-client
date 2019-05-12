using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Wraps objects the marshaler should marshal as a VT_DISPATCH.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class DispatchWrapper
	{
		private object wrappedObject;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.DispatchWrapper" /> class with the object being wrapped.</summary>
		/// <param name="obj">The object to be wrapped and converted to <see cref="F:System.Runtime.InteropServices.VarEnum.VT_DISPATCH" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="obj" /> is not a class or an array.-or- <paramref name="obj" /> does not support IDispatch. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="obj" /> parameter was marked with a <see cref="T:System.Runtime.InteropServices.ComVisibleAttribute" />  attribute that was passed a value of false.-or-The <paramref name="obj" /> parameter inherits from a type marked with a <see cref="T:System.Runtime.InteropServices.ComVisibleAttribute" />  attribute that was passed a value of false.</exception>
		public DispatchWrapper(object obj)
		{
			Marshal.GetIDispatchForObject(obj);
			this.wrappedObject = obj;
		}

		/// <summary>Gets the object wrapped by the <see cref="T:System.Runtime.InteropServices.DispatchWrapper" />.</summary>
		/// <returns>The object wrapped by the <see cref="T:System.Runtime.InteropServices.DispatchWrapper" />.</returns>
		public object WrappedObject
		{
			get
			{
				return this.wrappedObject;
			}
		}
	}
}
