using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net
{
	/// <summary>The exception that is thrown when an error occurs processing an HTTP request.</summary>
	[Serializable]
	public class HttpListenerException : System.ComponentModel.Win32Exception
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpListenerException" /> class. </summary>
		public HttpListenerException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpListenerException" /> class using the specified error code.</summary>
		/// <param name="errorCode">A <see cref="T:System.Int32" /> value that identifies the error that occurred.</param>
		public HttpListenerException(int errorCode) : base(errorCode)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpListenerException" /> class using the specified error code and message.</summary>
		/// <param name="errorCode">A <see cref="T:System.Int32" /> value that identifies the error that occurred.</param>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		public HttpListenerException(int errorCode, string message) : base(errorCode, message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpListenerException" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to deserialize the new <see cref="T:System.Net.HttpListenerException" /> object. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object. </param>
		protected HttpListenerException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		/// <summary>Gets a value that identifies the error that occurred.</summary>
		/// <returns>A <see cref="T:System.Int32" /> value.</returns>
		public override int ErrorCode
		{
			get
			{
				return base.ErrorCode;
			}
		}
	}
}
