using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Returns detailed information about the schema exception.</summary>
	[Serializable]
	public class XmlSchemaException : SystemException
	{
		private bool hasLineInfo;

		private int lineNumber;

		private int linePosition;

		private XmlSchemaObject sourceObj;

		private string sourceUri;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaException" /> class.</summary>
		public XmlSchemaException() : this("A schema error occured.", null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaException" /> class with the exception message specified.</summary>
		/// <param name="message">A string description of the error condition.</param>
		public XmlSchemaException(string message) : this(message, null)
		{
		}

		/// <summary>Constructs a new XmlSchemaException object with the given SerializationInfo and StreamingContext information that contains all the properties of the XmlSchemaException.</summary>
		/// <param name="info">SerializationInfo.</param>
		/// <param name="context">StreamingContext.</param>
		protected XmlSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.hasLineInfo = info.GetBoolean("hasLineInfo");
			this.lineNumber = info.GetInt32("lineNumber");
			this.linePosition = info.GetInt32("linePosition");
			this.sourceUri = info.GetString("sourceUri");
			this.sourceObj = (info.GetValue("sourceObj", typeof(XmlSchemaObject)) as XmlSchemaObject);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaException" /> class with the exception message specified, and the original <see cref="T:System.Exception" /> object, line number, and line position of the XML that cause this exception specified.</summary>
		/// <param name="message">A string description of the error condition.</param>
		/// <param name="innerException">The original T:System.Exception object that caused this exception.</param>
		/// <param name="lineNumber">The line number of the XML that caused this exception.</param>
		/// <param name="linePosition">The line position of the XML that caused this exception.</param>
		public XmlSchemaException(string message, Exception innerException, int lineNumber, int linePosition) : this(message, lineNumber, linePosition, null, null, innerException)
		{
		}

		internal XmlSchemaException(string message, int lineNumber, int linePosition, XmlSchemaObject sourceObject, string sourceUri, Exception innerException) : base(XmlSchemaException.GetMessage(message, sourceUri, lineNumber, linePosition, sourceObject), innerException)
		{
			this.hasLineInfo = true;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
			this.sourceObj = sourceObject;
			this.sourceUri = sourceUri;
		}

		internal XmlSchemaException(string message, object sender, string sourceUri, XmlSchemaObject sourceObject, Exception innerException) : base(XmlSchemaException.GetMessage(message, sourceUri, sender, sourceObject), innerException)
		{
			IXmlLineInfo xmlLineInfo = sender as IXmlLineInfo;
			if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
			{
				this.hasLineInfo = true;
				this.lineNumber = xmlLineInfo.LineNumber;
				this.linePosition = xmlLineInfo.LinePosition;
			}
			this.sourceObj = sourceObject;
		}

		internal XmlSchemaException(string message, XmlSchemaObject sourceObject, Exception innerException) : base(XmlSchemaException.GetMessage(message, null, 0, 0, sourceObject), innerException)
		{
			this.hasLineInfo = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaException" /> class with the exception message and original <see cref="T:System.Exception" /> object that caused this exception specified.</summary>
		/// <param name="message">A string description of the error condition.</param>
		/// <param name="innerException">The original T:System.Exception object that caused this exception.</param>
		public XmlSchemaException(string message, Exception innerException) : base(XmlSchemaException.GetMessage(message, null, 0, 0, null), innerException)
		{
		}

		/// <summary>Gets the line number indicating where the error occurred.</summary>
		/// <returns>The line number indicating where the error occurred.</returns>
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		/// <summary>Gets the line position indicating where the error occurred.</summary>
		/// <returns>The line position indicating where the error occurred.</returns>
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		/// <summary>The XmlSchemaObject that produced the XmlSchemaException.</summary>
		/// <returns>A valid object instance represents a structural validation error in the XML Schema Object Model (SOM).</returns>
		public XmlSchemaObject SourceSchemaObject
		{
			get
			{
				return this.sourceObj;
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) location of the schema that caused the exception.</summary>
		/// <returns>The URI location of the schema that caused the exception.</returns>
		public string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
		}

		private static string GetMessage(string message, string sourceUri, object sender, XmlSchemaObject sourceObj)
		{
			IXmlLineInfo xmlLineInfo = sender as IXmlLineInfo;
			if (xmlLineInfo == null)
			{
				return XmlSchemaException.GetMessage(message, sourceUri, 0, 0, sourceObj);
			}
			return XmlSchemaException.GetMessage(message, sourceUri, xmlLineInfo.LineNumber, xmlLineInfo.LinePosition, sourceObj);
		}

		private static string GetMessage(string message, string sourceUri, int lineNumber, int linePosition, XmlSchemaObject sourceObj)
		{
			string text = "XmlSchema error: " + message;
			if (lineNumber > 0)
			{
				text += string.Format(CultureInfo.InvariantCulture, " XML {0} Line {1}, Position {2}.", new object[]
				{
					(sourceUri == null || !(sourceUri != string.Empty)) ? string.Empty : ("URI: " + sourceUri + " ."),
					lineNumber,
					linePosition
				});
			}
			return text;
		}

		/// <summary>Gets the description of the error condition of this exception.</summary>
		/// <returns>The description of the error condition of this exception.</returns>
		public override string Message
		{
			get
			{
				return base.Message;
			}
		}

		/// <summary>Streams all the XmlSchemaException properties into the SerializationInfo class for the given StreamingContext.</summary>
		/// <param name="info">The SerializationInfo. </param>
		/// <param name="context">The StreamingContext information. </param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("hasLineInfo", this.hasLineInfo);
			info.AddValue("lineNumber", this.lineNumber);
			info.AddValue("linePosition", this.linePosition);
			info.AddValue("sourceUri", this.sourceUri);
			info.AddValue("sourceObj", this.sourceObj);
		}
	}
}
