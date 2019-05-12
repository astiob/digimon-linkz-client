using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Wraps objects the marshaler should marshal as a VT_UNKNOWN.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class UnknownWrapper
	{
		private object InternalObject;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.UnknownWrapper" /> class with the object to be wrapped.</summary>
		/// <param name="obj">The object being wrapped. </param>
		public UnknownWrapper(object obj)
		{
			this.InternalObject = obj;
		}

		/// <summary>Gets the object contained by this wrapper.</summary>
		/// <returns>The wrapped object.</returns>
		public object WrappedObject
		{
			get
			{
				return this.InternalObject;
			}
		}
	}
}
