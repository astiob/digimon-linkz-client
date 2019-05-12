using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when an attempt to unload an application domain fails.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class CannotUnloadAppDomainException : SystemException
	{
		private const int Result = -2146234347;

		/// <summary>Initializes a new instance of the <see cref="T:System.CannotUnloadAppDomainException" /> class.</summary>
		public CannotUnloadAppDomainException() : base(Locale.GetText("Attempt to unload application domain failed."))
		{
			base.HResult = -2146234347;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.CannotUnloadAppDomainException" /> class with a specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. </param>
		public CannotUnloadAppDomainException(string message) : base(message)
		{
			base.HResult = -2146234347;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.CannotUnloadAppDomainException" /> class from serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected CannotUnloadAppDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.CannotUnloadAppDomainException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
		public CannotUnloadAppDomainException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146234347;
		}
	}
}
