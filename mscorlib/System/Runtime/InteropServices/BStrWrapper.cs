using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Marshals data of type VT_BSTR from managed to unmanaged code. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class BStrWrapper
	{
		private string _value;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.BStrWrapper" /> class with the specified <see cref="T:System.String" /> object.</summary>
		/// <param name="value">The <see cref="T:System.String" /> object to wrap and marshal as VT_BSTR.</param>
		public BStrWrapper(string value)
		{
			this._value = value;
		}

		/// <summary>Gets the wrapped <see cref="T:System.String" /> object to marshal as type VT_BSTR.</summary>
		/// <returns>The <see cref="T:System.String" /> object wrapped by <see cref="T:System.Runtime.InteropServices.BStrWrapper" />.</returns>
		public string WrappedObject
		{
			get
			{
				return this._value;
			}
		}
	}
}
