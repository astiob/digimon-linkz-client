using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when there is an attempt to read or write protected memory.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class AccessViolationException : SystemException
	{
		private const int Result = -2147467261;

		/// <summary>Initializes a new instance of the <see cref="T:System.AccessViolationException" /> class with a system-supplied message that describes the error.</summary>
		public AccessViolationException() : base(Locale.GetText("Attempted to read or write protected memory. This is often an indication that other memory has been corrupted."))
		{
			base.HResult = -2147467261;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.AccessViolationException" /> class with a specified message that describes the error.</summary>
		/// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		public AccessViolationException(string message) : base(message)
		{
			base.HResult = -2147467261;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.AccessViolationException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
		public AccessViolationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.AccessViolationException" /> class with serialized data.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
		protected AccessViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
