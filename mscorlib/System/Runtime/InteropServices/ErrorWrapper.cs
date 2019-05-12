using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Wraps objects the marshaler should marshal as a VT_ERROR.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class ErrorWrapper
	{
		private int errorCode;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ErrorWrapper" /> class with the HRESULT that corresponds to the exception supplied.</summary>
		/// <param name="e">The exception to be converted to an error code. </param>
		public ErrorWrapper(Exception e)
		{
			this.errorCode = Marshal.GetHRForException(e);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ErrorWrapper" /> class with the HRESULT of the error.</summary>
		/// <param name="errorCode">The HRESULT of the error. </param>
		public ErrorWrapper(int errorCode)
		{
			this.errorCode = errorCode;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.ErrorWrapper" /> class with an object containing the HRESULT of the error.</summary>
		/// <param name="errorCode">The object containing the HRESULT of the error. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="errorCode" /> parameter is not an <see cref="T:System.Int32" /> type.</exception>
		public ErrorWrapper(object errorCode)
		{
			if (errorCode.GetType() != typeof(int))
			{
				throw new ArgumentException("errorCode has to be an int type");
			}
			this.errorCode = (int)errorCode;
		}

		/// <summary>Gets the error code of the wrapper.</summary>
		/// <returns>The HRESULT of the error.</returns>
		public int ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}
	}
}
