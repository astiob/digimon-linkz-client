using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when an attempt to access a class member fails.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class MemberAccessException : SystemException
	{
		private const int Result = -2146233062;

		/// <summary>Initializes a new instance of the <see cref="T:System.MemberAccessException" /> class.</summary>
		public MemberAccessException() : base(Locale.GetText("Cannot access a class member."))
		{
			base.HResult = -2146233062;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MemberAccessException" /> class with a specified error message.</summary>
		/// <param name="message">The message that describes the error. </param>
		public MemberAccessException(string message) : base(message)
		{
			base.HResult = -2146233062;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MemberAccessException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected MemberAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MemberAccessException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
		public MemberAccessException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233062;
		}
	}
}
