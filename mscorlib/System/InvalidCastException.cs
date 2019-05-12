using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown for invalid casting or explicit conversion.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class InvalidCastException : SystemException
	{
		private const int Result = -2147467262;

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidCastException" /> class.</summary>
		public InvalidCastException() : base(Locale.GetText("Cannot cast from source type to destination type."))
		{
			base.HResult = -2147467262;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidCastException" /> class with a specified error message.</summary>
		/// <param name="message">The message that describes the error. </param>
		public InvalidCastException(string message) : base(message)
		{
			base.HResult = -2147467262;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidCastException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
		public InvalidCastException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147467262;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidCastException" /> class with a specified message and error code.</summary>
		/// <param name="message">The message that indicates the reason the exception occurred.</param>
		/// <param name="errorCode">The error code (HRESULT) value associated with the exception.</param>
		public InvalidCastException(string message, int errorCode) : base(message)
		{
			base.HResult = errorCode;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidCastException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected InvalidCastException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
