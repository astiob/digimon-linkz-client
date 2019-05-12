using System;

namespace System.Xml.Schema
{
	internal class ValidationHandler
	{
		public static void RaiseValidationEvent(ValidationEventHandler handle, Exception innerException, string message, XmlSchemaObject xsobj, object sender, string sourceUri, XmlSeverityType severity)
		{
			XmlSchemaException ex = new XmlSchemaException(message, sender, sourceUri, xsobj, innerException);
			ValidationEventArgs validationEventArgs = new ValidationEventArgs(ex, message, severity);
			if (handle == null)
			{
				if (validationEventArgs.Severity == XmlSeverityType.Error)
				{
					throw validationEventArgs.Exception;
				}
			}
			else
			{
				handle(sender, validationEventArgs);
			}
		}
	}
}
