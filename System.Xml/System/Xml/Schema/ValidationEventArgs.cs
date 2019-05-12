using System;

namespace System.Xml.Schema
{
	/// <summary>Returns detailed information related to the ValidationEventHandler.</summary>
	public class ValidationEventArgs : EventArgs
	{
		private XmlSchemaException exception;

		private string message;

		private XmlSeverityType severity;

		private ValidationEventArgs()
		{
		}

		internal ValidationEventArgs(XmlSchemaException ex, string message, XmlSeverityType severity)
		{
			this.exception = ex;
			this.message = message;
			this.severity = severity;
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchemaException" /> associated with the validation event.</summary>
		/// <returns>The XmlSchemaException associated with the validation event.</returns>
		public XmlSchemaException Exception
		{
			get
			{
				return this.exception;
			}
		}

		/// <summary>Gets the text description corresponding to the validation event.</summary>
		/// <returns>The text description.</returns>
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		/// <summary>Gets the severity of the validation event.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSeverityType" /> value representing the severity of the validation event.</returns>
		public XmlSeverityType Severity
		{
			get
			{
				return this.severity;
			}
		}
	}
}
