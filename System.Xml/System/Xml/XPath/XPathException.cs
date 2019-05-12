using System;
using System.Runtime.Serialization;

namespace System.Xml.XPath
{
	/// <summary>Provides the exception thrown when an error occurs while processing an XPath expression. </summary>
	[Serializable]
	public class XPathException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathException" /> class.</summary>
		public XPathException() : base(string.Empty)
		{
		}

		/// <summary>Uses the information in the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects to initialize a new instance of the <see cref="T:System.Xml.XPath.XPathException" /> class.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains all the properties of an <see cref="T:System.Xml.XPath.XPathException" />. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object. </param>
		protected XPathException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathException" /> class using the specified exception message and <see cref="T:System.Exception" /> object.</summary>
		/// <param name="message">The description of the error condition. </param>
		/// <param name="innerException">The <see cref="T:System.Exception" /> that threw the <see cref="T:System.Xml.XPath.XPathException" />, if any. This value can be null. </param>
		public XPathException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathException" /> class with the specified exception message.</summary>
		/// <param name="message">The description of the error condition.</param>
		public XPathException(string message) : base(message, null)
		{
		}

		/// <summary>Gets the description of the error condition for this exception.</summary>
		/// <returns>The string description of the error condition for this exception.</returns>
		public override string Message
		{
			get
			{
				return base.Message;
			}
		}

		/// <summary>Streams all the <see cref="T:System.Xml.XPath.XPathException" /> properties into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class for the specified <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
