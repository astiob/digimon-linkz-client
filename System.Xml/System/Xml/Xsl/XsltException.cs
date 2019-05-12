using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	/// <summary>The exception that is thrown when an error occurs while processing an XSLT transformation.</summary>
	[Serializable]
	public class XsltException : SystemException
	{
		private int lineNumber;

		private int linePosition;

		private string sourceUri;

		private string templateFrames;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XsltException" /> class.</summary>
		public XsltException() : this(string.Empty, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XsltException" /> class with a specified error message. </summary>
		/// <param name="message">The message that describes the error.</param>
		public XsltException(string message) : this(message, null)
		{
		}

		/// <summary>Initializes a new instance of the XsltException class.</summary>
		/// <param name="message">The description of the error condition. </param>
		/// <param name="innerException">The <see cref="T:System.Exception" /> which threw the XsltException, if any. This value can be null. </param>
		public XsltException(string message, Exception innerException) : this("{0}", message, innerException, 0, 0, null)
		{
		}

		/// <summary>Initializes a new instance of the XsltException class using the information in the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects.</summary>
		/// <param name="info">The SerializationInfo object containing all the properties of an XsltException. </param>
		/// <param name="context">The StreamingContext object. </param>
		protected XsltException(SerializationInfo info, StreamingContext context)
		{
			this.lineNumber = info.GetInt32("lineNumber");
			this.linePosition = info.GetInt32("linePosition");
			this.sourceUri = info.GetString("sourceUri");
			this.templateFrames = info.GetString("templateFrames");
		}

		internal XsltException(string msgFormat, string message, Exception innerException, int lineNumber, int linePosition, string sourceUri) : base(XsltException.CreateMessage(msgFormat, message, lineNumber, linePosition, sourceUri), innerException)
		{
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
			this.sourceUri = sourceUri;
		}

		internal XsltException(string message, Exception innerException, XPathNavigator nav) : base(XsltException.CreateMessage(message, nav), innerException)
		{
			IXmlLineInfo xmlLineInfo = nav as IXmlLineInfo;
			this.lineNumber = ((xmlLineInfo == null) ? 0 : xmlLineInfo.LineNumber);
			this.linePosition = ((xmlLineInfo == null) ? 0 : xmlLineInfo.LinePosition);
			this.sourceUri = ((nav == null) ? string.Empty : nav.BaseURI);
		}

		private static string CreateMessage(string message, XPathNavigator nav)
		{
			IXmlLineInfo xmlLineInfo = nav as IXmlLineInfo;
			int num = (xmlLineInfo == null) ? 0 : xmlLineInfo.LineNumber;
			int num2 = (xmlLineInfo == null) ? 0 : xmlLineInfo.LinePosition;
			string text = (nav == null) ? string.Empty : nav.BaseURI;
			if (num != 0)
			{
				return XsltException.CreateMessage("{0} at {1}({2},{3}).", message, num, num2, text);
			}
			return XsltException.CreateMessage("{0}.", message, num, num2, text);
		}

		private static string CreateMessage(string msgFormat, string message, int lineNumber, int linePosition, string sourceUri)
		{
			return string.Format(CultureInfo.InvariantCulture, msgFormat, new object[]
			{
				message,
				sourceUri,
				lineNumber.ToString(CultureInfo.InvariantCulture),
				linePosition.ToString(CultureInfo.InvariantCulture)
			});
		}

		/// <summary>Gets the line number indicating where the error occurred in the style sheet.</summary>
		/// <returns>The line number indicating where the error occurred in the style sheet.</returns>
		public virtual int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		/// <summary>Gets the line position indicating where the error occurred in the style sheet.</summary>
		/// <returns>The line position indicating where the error occurred in the style sheet.</returns>
		public virtual int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		/// <summary>Gets the formatted error message describing the current exception.</summary>
		/// <returns>The formatted error message describing the current exception.</returns>
		public override string Message
		{
			get
			{
				return (this.templateFrames == null) ? base.Message : (base.Message + this.templateFrames);
			}
		}

		/// <summary>Gets the location path of the style sheet.</summary>
		/// <returns>The location path of the style sheet.</returns>
		public virtual string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
		}

		/// <summary>Streams all the XsltException properties into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class for the given <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">The SerializationInfo object. </param>
		/// <param name="context">The StreamingContext object. </param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("lineNumber", this.lineNumber);
			info.AddValue("linePosition", this.linePosition);
			info.AddValue("sourceUri", this.sourceUri);
			info.AddValue("templateFrames", this.templateFrames);
		}

		internal void AddTemplateFrame(string frame)
		{
			this.templateFrames += frame;
		}
	}
}
