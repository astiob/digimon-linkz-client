using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when the time allotted for a process or operation has expired.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class TimeoutException : SystemException
	{
		private const int Result = -2146233083;

		/// <summary>Initializes a new instance of the <see cref="T:System.TimeoutException" /> class.</summary>
		public TimeoutException() : base(Locale.GetText("The operation has timed-out."))
		{
			base.HResult = -2146233083;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.TimeoutException" /> class with the specified error message.</summary>
		/// <param name="message">The message that describes the error. </param>
		public TimeoutException(string message) : base(message)
		{
			base.HResult = -2146233083;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.TimeoutException" /> class with the specified error message and inner exception.</summary>
		/// <param name="message">The message that describes the error. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
		public TimeoutException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233083;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.TimeoutException" /> class with serialized data.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains contextual information about the source or destination. The <paramref name="context" /> parameter is reserved for future use, and can be specified as null.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null, or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
		protected TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
