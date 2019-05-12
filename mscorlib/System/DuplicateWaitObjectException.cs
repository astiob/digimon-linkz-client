using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when an object appears more than once in an array of synchronization objects.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class DuplicateWaitObjectException : ArgumentException
	{
		private const int Result = -2146233047;

		/// <summary>Initializes a new instance of the <see cref="T:System.DuplicateWaitObjectException" /> class.</summary>
		public DuplicateWaitObjectException() : base(Locale.GetText("Duplicate objects in argument."))
		{
			base.HResult = -2146233047;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DuplicateWaitObjectException" /> class with the name of the parameter that causes this exception.</summary>
		/// <param name="parameterName">The name of the parameter that caused the exception. </param>
		public DuplicateWaitObjectException(string parameterName) : base(Locale.GetText("Duplicate objects in argument."), parameterName)
		{
			base.HResult = -2146233047;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DuplicateWaitObjectException" /> class with a specified error message and the name of the parameter that causes this exception.</summary>
		/// <param name="parameterName">The name of the parameter that caused the exception. </param>
		/// <param name="message">The message that describes the error. </param>
		public DuplicateWaitObjectException(string parameterName, string message) : base(message, parameterName)
		{
			base.HResult = -2146233047;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DuplicateWaitObjectException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
		public DuplicateWaitObjectException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233047;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DuplicateWaitObjectException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected DuplicateWaitObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
