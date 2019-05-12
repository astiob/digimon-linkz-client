using System;
using System.Xml.Schema;

namespace System.Xml
{
	/// <summary>Specifies a set of features to support on the <see cref="T:System.Xml.XmlReader" /> object created by the <see cref="Overload:System.Xml.XmlReader.Create" /> method. </summary>
	public sealed class XmlReaderSettings
	{
		private bool checkCharacters;

		private bool closeInput;

		private ConformanceLevel conformance;

		private bool ignoreComments;

		private bool ignoreProcessingInstructions;

		private bool ignoreWhitespace;

		private int lineNumberOffset;

		private int linePositionOffset;

		private bool prohibitDtd;

		private XmlNameTable nameTable;

		private XmlSchemaSet schemas;

		private bool schemasNeedsInitialization;

		private XmlSchemaValidationFlags validationFlags;

		private ValidationType validationType;

		private XmlResolver xmlResolver;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlReaderSettings" /> class.</summary>
		public XmlReaderSettings()
		{
			this.Reset();
		}

		/// <summary>Occurs when the reader encounters validation errors.</summary>
		public event ValidationEventHandler ValidationEventHandler;

		/// <summary>Creates a copy of the <see cref="T:System.Xml.XmlReaderSettings" /> instance.</summary>
		/// <returns>The cloned <see cref="T:System.Xml.XmlReaderSettings" /> object.</returns>
		public XmlReaderSettings Clone()
		{
			return (XmlReaderSettings)base.MemberwiseClone();
		}

		/// <summary>Resets the members of the settings class to their default values.</summary>
		public void Reset()
		{
			this.checkCharacters = true;
			this.closeInput = false;
			this.conformance = ConformanceLevel.Document;
			this.ignoreComments = false;
			this.ignoreProcessingInstructions = false;
			this.ignoreWhitespace = false;
			this.lineNumberOffset = 0;
			this.linePositionOffset = 0;
			this.prohibitDtd = true;
			this.schemas = null;
			this.schemasNeedsInitialization = true;
			this.validationFlags = (XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.AllowXmlAttributes);
			this.validationType = ValidationType.None;
			this.xmlResolver = new XmlUrlResolver();
		}

		/// <summary>Gets or sets a value indicating whether to do character checking.</summary>
		/// <returns>true to do character checking; otherwise false. The default is true.Note:If the <see cref="T:System.Xml.XmlReader" /> is processing text data, it always checks that the XML names and text content are valid, regardless of the property setting. Setting <see cref="P:System.Xml.XmlReaderSettings.CheckCharacters" /> to false turns off character checking for character entity references.</returns>
		public bool CheckCharacters
		{
			get
			{
				return this.checkCharacters;
			}
			set
			{
				this.checkCharacters = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the underlying stream or <see cref="T:System.IO.TextReader" /> should be closed when the reader is closed.</summary>
		/// <returns>true to close the underlying stream or <see cref="T:System.IO.TextReader" /> when the reader is closed; otherwise false. The default is false.</returns>
		public bool CloseInput
		{
			get
			{
				return this.closeInput;
			}
			set
			{
				this.closeInput = value;
			}
		}

		/// <summary>Gets or sets the level of conformance which the <see cref="T:System.Xml.XmlReader" /> will comply.</summary>
		/// <returns>One of the <see cref="T:System.Xml.ConformanceLevel" /> values. The default is ConformanceLevel.Document.</returns>
		public ConformanceLevel ConformanceLevel
		{
			get
			{
				return this.conformance;
			}
			set
			{
				this.conformance = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to ignore comments.</summary>
		/// <returns>true to ignore comments; otherwise false. The default is false.</returns>
		public bool IgnoreComments
		{
			get
			{
				return this.ignoreComments;
			}
			set
			{
				this.ignoreComments = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to ignore processing instructions.</summary>
		/// <returns>true to ignore processing instructions; otherwise false. The default is false.</returns>
		public bool IgnoreProcessingInstructions
		{
			get
			{
				return this.ignoreProcessingInstructions;
			}
			set
			{
				this.ignoreProcessingInstructions = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to ignore insignificant white space.</summary>
		/// <returns>true to ignore white space; otherwise false. The default is false.</returns>
		public bool IgnoreWhitespace
		{
			get
			{
				return this.ignoreWhitespace;
			}
			set
			{
				this.ignoreWhitespace = value;
			}
		}

		/// <summary>Gets or sets line number offset of the <see cref="T:System.Xml.XmlReader" /> object.</summary>
		/// <returns>The line number offset. The default is 0.</returns>
		public int LineNumberOffset
		{
			get
			{
				return this.lineNumberOffset;
			}
			set
			{
				this.lineNumberOffset = value;
			}
		}

		/// <summary>Gets or sets line position offset of the <see cref="T:System.Xml.XmlReader" /> object.</summary>
		/// <returns>The line number offset. The default is 0.</returns>
		public int LinePositionOffset
		{
			get
			{
				return this.linePositionOffset;
			}
			set
			{
				this.linePositionOffset = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to prohibit document type definition (DTD) processing.</summary>
		/// <returns>true to prohibit DTD processing; otherwise false. The default is true.</returns>
		public bool ProhibitDtd
		{
			get
			{
				return this.prohibitDtd;
			}
			set
			{
				this.prohibitDtd = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.XmlNameTable" /> used for atomized string comparisons.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNameTable" /> that stores all the atomized strings used by all <see cref="T:System.Xml.XmlReader" /> instances created using this <see cref="T:System.Xml.XmlReaderSettings" /> object.The default is null. The created <see cref="T:System.Xml.XmlReader" /> instance will use a new empty <see cref="T:System.Xml.NameTable" /> if this value is null.</returns>
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
			set
			{
				this.nameTable = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> to use when performing schema validation.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaSet" /> to use. The default is an empty <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object.</returns>
		public XmlSchemaSet Schemas
		{
			get
			{
				if (this.schemasNeedsInitialization)
				{
					this.schemas = new XmlSchemaSet();
					this.schemasNeedsInitialization = false;
				}
				return this.schemas;
			}
			set
			{
				this.schemas = value;
				this.schemasNeedsInitialization = false;
			}
		}

		internal void OnValidationError(object o, ValidationEventArgs e)
		{
			if (this.ValidationEventHandler != null)
			{
				this.ValidationEventHandler(o, e);
			}
			else if (e.Severity == XmlSeverityType.Error)
			{
				throw e.Exception;
			}
		}

		internal void SetSchemas(XmlSchemaSet schemas)
		{
			this.schemas = schemas;
		}

		/// <summary>Gets or sets a value indicating the schema validation settings. This setting applies to schema validating <see cref="T:System.Xml.XmlReader" /> objects (<see cref="P:System.Xml.XmlReaderSettings.ValidationType" /> property set to ValidationType.Schema).</summary>
		/// <returns>A set of <see cref="T:System.Xml.Schema.XmlSchemaValidationFlags" /> values. <see cref="F:System.Xml.Schema.XmlSchemaValidationFlags.ProcessIdentityConstraints" /> and <see cref="F:System.Xml.Schema.XmlSchemaValidationFlags.AllowXmlAttributes" /> are enabled by default. <see cref="F:System.Xml.Schema.XmlSchemaValidationFlags.ProcessInlineSchema" />, <see cref="F:System.Xml.Schema.XmlSchemaValidationFlags.ProcessSchemaLocation" />, and <see cref="F:System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings" /> are disabled by default.</returns>
		public XmlSchemaValidationFlags ValidationFlags
		{
			get
			{
				return this.validationFlags;
			}
			set
			{
				this.validationFlags = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Xml.XmlReader" /> will perform validation or type assignment when reading.</summary>
		/// <returns>One of the <see cref="T:System.Xml.ValidationType" /> values. The default is ValidationType.None.</returns>
		public ValidationType ValidationType
		{
			get
			{
				return this.validationType;
			}
			set
			{
				this.validationType = value;
			}
		}

		/// <summary>Sets the <see cref="T:System.Xml.XmlResolver" /> used to access external documents.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlResolver" /> used to access external documents. If set to null, an <see cref="T:System.Xml.XmlException" /> is thrown when the <see cref="T:System.Xml.XmlReader" /> tries to access an external resource. The default is a new <see cref="T:System.Xml.XmlUrlResolver" /> with no credentials.</returns>
		public XmlResolver XmlResolver
		{
			internal get
			{
				return this.xmlResolver;
			}
			set
			{
				this.xmlResolver = value;
			}
		}
	}
}
