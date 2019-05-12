using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when the execution stack overflows because it contains too many nested method calls. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public sealed class StackOverflowException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.StackOverflowException" /> class, setting the <see cref="P:System.Exception.Message" /> property of the new instance to a system-supplied message that describes the error, such as "The requested operation caused a stack overflow." This message takes into account the current system culture.</summary>
		public StackOverflowException() : base(Locale.GetText("The requested operation caused a stack overflow."))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.StackOverflowException" /> class with a specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of message is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
		public StackOverflowException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.StackOverflowException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
		public StackOverflowException(string message, Exception innerException) : base(message, innerException)
		{
		}

		internal StackOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
