using System;
using System.Runtime.Serialization;

namespace System.Net
{
	/// <summary>The exception that is thrown when an error is made while using a network protocol.</summary>
	[Serializable]
	public class ProtocolViolationException : InvalidOperationException, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.ProtocolViolationException" /> class.</summary>
		public ProtocolViolationException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.ProtocolViolationException" /> class with the specified message.</summary>
		/// <param name="message">The error message string. </param>
		public ProtocolViolationException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.ProtocolViolationException" /> class from the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> instances.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the information that is required to deserialize the <see cref="T:System.Net.ProtocolViolationException" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source of the serialized stream that is associated with the new <see cref="T:System.Net.ProtocolViolationException" />. </param>
		protected ProtocolViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Serializes this instance into the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</summary>
		/// <param name="serializationInfo">The object into which this <see cref="T:System.Net.ProtocolViolationException" /> will be serialized. </param>
		/// <param name="streamingContext">The destination of the serialization. </param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data required to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
