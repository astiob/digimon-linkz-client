using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	/// <summary>The exception thrown when an invalid COM object is used.</summary>
	[ComVisible(true)]
	[Serializable]
	public class InvalidComObjectException : SystemException
	{
		private const int ErrorCode = -2146233049;

		/// <summary>Initializes an instance of the InvalidComObjectException with default properties.</summary>
		public InvalidComObjectException() : base(Locale.GetText("Invalid COM object is used"))
		{
			base.HResult = -2146233049;
		}

		/// <summary>Initializes an instance of the InvalidComObjectException with a message.</summary>
		/// <param name="message">The message that indicates the reason for the exception. </param>
		public InvalidComObjectException(string message) : base(message)
		{
			base.HResult = -2146233049;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.InvalidComObjectException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
		public InvalidComObjectException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233049;
		}

		/// <summary>Initializes a new instance of the COMException class from serialization data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		protected InvalidComObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
