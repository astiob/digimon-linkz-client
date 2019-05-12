using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Xml
{
	/// <summary>Returns detailed information about the last exception.</summary>
	[Serializable]
	public class XmlException : SystemException
	{
		private const string Xml_DefaultException = "Xml_DefaultException";

		private const string Xml_UserException = "Xml_UserException";

		private int lineNumber;

		private int linePosition;

		private string sourceUri;

		private string res;

		private string[] messages;

		/// <summary>Initializes a new instance of the XmlException class.</summary>
		public XmlException()
		{
			this.res = "Xml_DefaultException";
			this.messages = new string[1];
		}

		/// <summary>Initializes a new instance of the XmlException class.</summary>
		/// <param name="message">The description of the error condition. </param>
		/// <param name="innerException">The <see cref="T:System.Exception" /> that threw the XmlException, if any. This value can be null. </param>
		public XmlException(string message, Exception innerException) : base(message, innerException)
		{
			this.res = "Xml_UserException";
			this.messages = new string[]
			{
				message
			};
		}

		/// <summary>Initializes a new instance of the XmlException class using the information in the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects.</summary>
		/// <param name="info">The SerializationInfo object containing all the properties of an XmlException. </param>
		/// <param name="context">The StreamingContext object containing the context information. </param>
		protected XmlException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.lineNumber = info.GetInt32("lineNumber");
			this.linePosition = info.GetInt32("linePosition");
			this.res = info.GetString("res");
			this.messages = (string[])info.GetValue("args", typeof(string[]));
			this.sourceUri = info.GetString("sourceUri");
		}

		/// <summary>Initializes a new instance of the XmlException class with a specified error message.</summary>
		/// <param name="message">The error description. </param>
		public XmlException(string message) : base(message)
		{
			this.res = "Xml_UserException";
			this.messages = new string[]
			{
				message
			};
		}

		internal XmlException(IXmlLineInfo li, string sourceUri, string message) : this(li, null, sourceUri, message)
		{
		}

		internal XmlException(IXmlLineInfo li, Exception innerException, string sourceUri, string message) : this(message, innerException)
		{
			if (li != null)
			{
				this.lineNumber = li.LineNumber;
				this.linePosition = li.LinePosition;
			}
			this.sourceUri = sourceUri;
		}

		/// <summary>Initializes a new instance of the XmlException class with the specified message, inner exception, line number, and line position.</summary>
		/// <param name="message">The error description. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. This value can be null. </param>
		/// <param name="lineNumber">The line number indicating where the error occurred. </param>
		/// <param name="linePosition">The line position indicating where the error occurred. </param>
		public XmlException(string message, Exception innerException, int lineNumber, int linePosition) : this(message, innerException)
		{
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		internal XmlException(string message, int lineNumber, int linePosition, object sourceObject, string sourceUri, Exception innerException) : base(XmlException.GetMessage(message, sourceUri, lineNumber, linePosition, sourceObject), innerException)
		{
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
			this.sourceUri = sourceUri;
		}

		private static string GetMessage(string message, string sourceUri, int lineNumber, int linePosition, object sourceObj)
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

		/// <summary>Gets the location of the XML file.</summary>
		/// <returns>The source URI for the XML data. If there is no source URI, this property returns null.</returns>
		public string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
		}

		/// <summary>Gets a message describing the current exception.</summary>
		/// <returns>The error message that explains the reason for the exception.</returns>
		public override string Message
		{
			get
			{
				if (this.lineNumber == 0)
				{
					return base.Message;
				}
				return string.Format(CultureInfo.InvariantCulture, "{0} {3} Line {1}, position {2}.", new object[]
				{
					base.Message,
					this.lineNumber,
					this.linePosition,
					this.sourceUri
				});
			}
		}

		/// <summary>Streams all the XmlException properties into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class for the given <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">The SerializationInfo object. </param>
		/// <param name="context">The StreamingContext object. </param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("lineNumber", this.lineNumber);
			info.AddValue("linePosition", this.linePosition);
			info.AddValue("res", this.res);
			info.AddValue("args", this.messages);
			info.AddValue("sourceUri", this.sourceUri);
		}
	}
}
