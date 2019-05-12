using System;
using System.Runtime.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the exception thrown when XML Schema Definition Language (XSD) schema validation errors and warnings are encountered in an XML document being validated. </summary>
	[Serializable]
	public class XmlSchemaValidationException : XmlSchemaException
	{
		private object source_object;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaValidationException" /> class.</summary>
		public XmlSchemaValidationException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaValidationException" /> class with the exception message specified.</summary>
		/// <param name="message">A string description of the error condition.</param>
		public XmlSchemaValidationException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaValidationException" /> class with the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects specified.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object.</param>
		protected XmlSchemaValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaValidationException" /> class with the exception message specified, and the original <see cref="T:System.Exception" /> object, line number, and line position of the XML that cause this exception specified.</summary>
		/// <param name="message">A string description of the error condition.</param>
		/// <param name="innerException">The original <see cref="T:System.Exception" /> object that caused this exception.</param>
		/// <param name="lineNumber">The line number of the XML that caused this exception.</param>
		/// <param name="linePosition">The line position of the XML that caused this exception.</param>
		public XmlSchemaValidationException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, lineNumber, linePosition, null, null, innerException)
		{
		}

		internal XmlSchemaValidationException(string message, int lineNumber, int linePosition, XmlSchemaObject sourceObject, string sourceUri, Exception innerException) : base(message, lineNumber, linePosition, sourceObject, sourceUri, innerException)
		{
		}

		internal XmlSchemaValidationException(string message, object sender, string sourceUri, XmlSchemaObject sourceObject, Exception innerException) : base(message, sender, sourceUri, sourceObject, innerException)
		{
		}

		internal XmlSchemaValidationException(string message, XmlSchemaObject sourceObject, Exception innerException) : base(message, sourceObject, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaValidationException" /> class with the exception message and original <see cref="T:System.Exception" /> object that caused this exception specified.</summary>
		/// <param name="message">A string description of the error condition.</param>
		/// <param name="innerException">The original <see cref="T:System.Exception" /> object that caused this exception.</param>
		public XmlSchemaValidationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Constructs a new <see cref="T:System.Xml.Schema.XmlSchemaValidationException" /> object with the given <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> information that contains all the properties of the <see cref="T:System.Xml.Schema.XmlSchemaValidationException" />.</summary>
		/// <param name="info">
		///   <see cref="T:System.Runtime.Serialization.SerializationInfo" />
		/// </param>
		/// <param name="context">
		///   <see cref="T:System.Runtime.Serialization.StreamingContext" />
		/// </param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		/// <summary>Sets the XML node that causes the error.</summary>
		/// <param name="sourceObject">The source object.</param>
		protected internal void SetSourceObject(object o)
		{
			this.source_object = o;
		}

		/// <summary>Gets the XML node that caused this <see cref="T:System.Xml.Schema.XmlSchemaValidationException" />.</summary>
		/// <returns>The XML node that caused this <see cref="T:System.Xml.Schema.XmlSchemaValidationException" />.</returns>
		public object SourceObject
		{
			get
			{
				return this.source_object;
			}
		}
	}
}
