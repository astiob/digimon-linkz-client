using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System.Net.Sockets
{
	/// <summary>The exception that is thrown when a socket error occurs.</summary>
	[Serializable]
	public class SocketException : System.ComponentModel.Win32Exception
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.SocketException" /> class with the last operating system error code.</summary>
		public SocketException() : base(SocketException.WSAGetLastError_internal())
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.SocketException" /> class with the specified error code.</summary>
		/// <param name="errorCode">The error code that indicates the error that occurred. </param>
		public SocketException(int error) : base(error)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.SocketException" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance that contains the information that is required to serialize the new <see cref="T:System.Net.Sockets.SocketException" /> instance. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source of the serialized stream that is associated with the new <see cref="T:System.Net.Sockets.SocketException" /> instance. </param>
		protected SocketException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal SocketException(int error, string message) : base(error, message)
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int WSAGetLastError_internal();

		/// <summary>Gets the error code that is associated with this exception.</summary>
		/// <returns>An integer error code that is associated with this exception.</returns>
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}

		/// <summary>Gets the error code that is associated with this exception.</summary>
		/// <returns>An integer error code that is associated with this exception.</returns>
		public SocketError SocketErrorCode
		{
			get
			{
				return (SocketError)base.NativeErrorCode;
			}
		}

		/// <summary>Gets the error message that is associated with this exception.</summary>
		/// <returns>A string that contains the error message. </returns>
		public override string Message
		{
			get
			{
				return base.Message;
			}
		}
	}
}
