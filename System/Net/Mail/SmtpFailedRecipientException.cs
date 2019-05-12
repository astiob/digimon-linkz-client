using System;
using System.Runtime.Serialization;

namespace System.Net.Mail
{
	/// <summary>Represents the exception that is thrown when the <see cref="T:System.Net.Mail.SmtpClient" /> is not able to complete a <see cref="Overload:System.Net.Mail.SmtpClient.Send" /> or <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> operation to a particular recipient.</summary>
	[Serializable]
	public class SmtpFailedRecipientException : SmtpException, ISerializable
	{
		private string failedRecipient;

		/// <summary>Initializes an empty instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class.</summary>
		public SmtpFailedRecipientException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class with the specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that contains the error message.</param>
		public SmtpFailedRecipientException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the information required to serialize the new <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />. </param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source and destination of the serialized stream that is associated with the new instance. </param>
		protected SmtpFailedRecipientException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			this.failedRecipient = serializationInfo.GetString("failedRecipient");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class with the specified status code and e-mail address.</summary>
		/// <param name="statusCode">An <see cref="T:System.Net.Mail.SmtpStatusCode" /> value.</param>
		/// <param name="failedRecipient">A <see cref="T:System.String" /> that contains the e-mail address.</param>
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient) : base(statusCode)
		{
			this.failedRecipient = failedRecipient;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified error message and inner exception.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. </param>
		public SmtpFailedRecipientException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpException" /> class with the specified error message, e-mail address, and inner exception.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error that occurred.</param>
		/// <param name="failedRecipient">A <see cref="T:System.String" /> that contains the e-mail address.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public SmtpFailedRecipientException(string message, string failedRecipient, Exception innerException) : base(message, innerException)
		{
			this.failedRecipient = failedRecipient;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" /> class with the specified status code, e-mail address, and server response.</summary>
		/// <param name="statusCode">An <see cref="T:System.Net.Mail.SmtpStatusCode" /> value.</param>
		/// <param name="failedRecipient">A <see cref="T:System.String" /> that contains the e-mail address.</param>
		/// <param name="serverResponse">A <see cref="T:System.String" /> that contains the server response.</param>
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient, string serverResponse) : base(statusCode, serverResponse)
		{
			this.failedRecipient = failedRecipient;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data that is needed to serialize the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance, which holds the serialized data for the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> instance that contains the destination of the serialized stream that is associated with the new <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Indicates the e-mail address with delivery difficulties.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the e-mail address.</returns>
		public string FailedRecipient
		{
			get
			{
				return this.failedRecipient;
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data that is needed to serialize the <see cref="T:System.Net.Mail.SmtpFailedRecipientException" />.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("failedRecipient", this.failedRecipient);
		}
	}
}
