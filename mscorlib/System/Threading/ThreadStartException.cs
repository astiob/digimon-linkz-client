using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	/// <summary>The exception that is thrown when a failure occurs in a managed thread after the underlying operating system thread has been started, but before the thread is ready to execute user code.</summary>
	[Serializable]
	public sealed class ThreadStartException : SystemException
	{
		internal ThreadStartException() : base("Thread Start Error")
		{
		}

		internal ThreadStartException(string message) : base(message)
		{
		}

		internal ThreadStartException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal ThreadStartException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
