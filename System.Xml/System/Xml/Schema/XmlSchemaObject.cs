using System;
using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Represents the root class for the Xml schema object model hierarchy and serves as a base class for classes such as the <see cref="T:System.Xml.Schema.XmlSchema" /> class.</summary>
	public abstract class XmlSchemaObject
	{
		private int lineNumber;

		private int linePosition;

		private string sourceUri;

		private XmlSerializerNamespaces namespaces;

		internal ArrayList unhandledAttributeList;

		internal bool isCompiled;

		internal int errorCount;

		internal Guid CompilationId;

		internal Guid ValidationId;

		internal bool isRedefineChild;

		internal bool isRedefinedComponent;

		internal XmlSchemaObject redefinedObject;

		private XmlSchemaObject parent;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Schema.XmlSchemaObject" /> class.</summary>
		protected XmlSchemaObject()
		{
			this.namespaces = new XmlSerializerNamespaces();
			this.unhandledAttributeList = null;
			this.CompilationId = Guid.Empty;
		}

		/// <summary>Gets or sets the line number in the file to which the schema element refers.</summary>
		/// <returns>The line number.</returns>
		[XmlIgnore]
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
			set
			{
				this.lineNumber = value;
			}
		}

		/// <summary>Gets or sets the line position in the file to which the schema element refers.</summary>
		/// <returns>The line position.</returns>
		[XmlIgnore]
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
			set
			{
				this.linePosition = value;
			}
		}

		/// <summary>Gets or sets the source location for the file that loaded the schema.</summary>
		/// <returns>The source location (URI) for the file.</returns>
		[XmlIgnore]
		public string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
			set
			{
				this.sourceUri = value;
			}
		}

		/// <summary>Gets or sets the parent of this <see cref="T:System.Xml.Schema.XmlSchemaObject" />.</summary>
		/// <returns>The parent <see cref="T:System.Xml.Schema.XmlSchemaObject" /> of this <see cref="T:System.Xml.Schema.XmlSchemaObject" />.</returns>
		[XmlIgnore]
		public XmlSchemaObject Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		internal XmlSchema AncestorSchema
		{
			get
			{
				for (XmlSchemaObject xmlSchemaObject = this.Parent; xmlSchemaObject != null; xmlSchemaObject = xmlSchemaObject.Parent)
				{
					if (xmlSchemaObject is XmlSchema)
					{
						return (XmlSchema)xmlSchemaObject;
					}
				}
				throw new Exception(string.Format("INTERNAL ERROR: Parent object is not set properly : {0} ({1},{2})", this.SourceUri, this.LineNumber, this.LinePosition));
			}
		}

		internal virtual void SetParent(XmlSchemaObject parent)
		{
			this.Parent = parent;
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> to use with this schema object.</summary>
		/// <returns>The <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> property for the schema object.</returns>
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces Namespaces
		{
			get
			{
				return this.namespaces;
			}
			set
			{
				this.namespaces = value;
			}
		}

		internal void error(ValidationEventHandler handle, string message)
		{
			this.errorCount++;
			XmlSchemaObject.error(handle, message, null, this, null);
		}

		internal void warn(ValidationEventHandler handle, string message)
		{
			XmlSchemaObject.warn(handle, message, null, this, null);
		}

		internal static void error(ValidationEventHandler handle, string message, Exception innerException)
		{
			XmlSchemaObject.error(handle, message, innerException, null, null);
		}

		internal static void warn(ValidationEventHandler handle, string message, Exception innerException)
		{
			XmlSchemaObject.warn(handle, message, innerException, null, null);
		}

		internal static void error(ValidationEventHandler handle, string message, Exception innerException, XmlSchemaObject xsobj, object sender)
		{
			ValidationHandler.RaiseValidationEvent(handle, innerException, message, xsobj, sender, null, XmlSeverityType.Error);
		}

		internal static void warn(ValidationEventHandler handle, string message, Exception innerException, XmlSchemaObject xsobj, object sender)
		{
			ValidationHandler.RaiseValidationEvent(handle, innerException, message, xsobj, sender, null, XmlSeverityType.Warning);
		}

		internal virtual int Compile(ValidationEventHandler h, XmlSchema schema)
		{
			return 0;
		}

		internal virtual int Validate(ValidationEventHandler h, XmlSchema schema)
		{
			return 0;
		}

		internal bool IsValidated(Guid validationId)
		{
			return this.ValidationId == validationId;
		}

		internal virtual void CopyInfo(XmlSchemaParticle obj)
		{
			obj.LineNumber = this.LineNumber;
			obj.LinePosition = this.LinePosition;
			obj.SourceUri = this.SourceUri;
			obj.errorCount = this.errorCount;
		}
	}
}
