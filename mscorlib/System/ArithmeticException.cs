using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown for errors in an arithmetic, casting, or conversion operation.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class ArithmeticException : SystemException
	{
		private const int Result = -2147024362;

		/// <summary>Initializes a new instance of the <see cref="T:System.ArithmeticException" /> class.</summary>
		public ArithmeticException() : base(Locale.GetText("Overflow or underflow in the arithmetic operation."))
		{
			base.HResult = -2147024362;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ArithmeticException" /> class with a specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. </param>
		public ArithmeticException(string message) : base(message)
		{
			base.HResult = -2147024362;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ArithmeticException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception. </param>
		public ArithmeticException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024362;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ArithmeticException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected ArithmeticException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
