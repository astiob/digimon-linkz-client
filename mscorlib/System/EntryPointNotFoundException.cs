using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when an attempt to load a class fails due to the absence of an entry method.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class EntryPointNotFoundException : TypeLoadException
	{
		private const int Result = -2146233053;

		/// <summary>Initializes a new instance of the <see cref="T:System.EntryPointNotFoundException" /> class.</summary>
		public EntryPointNotFoundException() : base(Locale.GetText("Cannot load class because of missing entry method."))
		{
			base.HResult = -2146233053;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.EntryPointNotFoundException" /> class with a specified error message.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		public EntryPointNotFoundException(string message) : base(message)
		{
			base.HResult = -2146233053;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.EntryPointNotFoundException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected EntryPointNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.EntryPointNotFoundException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
		public EntryPointNotFoundException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233053;
		}
	}
}
