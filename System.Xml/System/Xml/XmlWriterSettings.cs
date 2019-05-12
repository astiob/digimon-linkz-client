using System;
using System.Text;

namespace System.Xml
{
	/// <summary>Specifies a set of features to support on the <see cref="T:System.Xml.XmlWriter" /> object created by the <see cref="Overload:System.Xml.XmlWriter.Create" /> method.</summary>
	public sealed class XmlWriterSettings
	{
		private bool checkCharacters;

		private bool closeOutput;

		private ConformanceLevel conformance;

		private Encoding encoding;

		private bool indent;

		private string indentChars;

		private string newLineChars;

		private bool newLineOnAttributes;

		private NewLineHandling newLineHandling;

		private bool omitXmlDeclaration;

		private XmlOutputMethod outputMethod;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlWriterSettings" /> class.</summary>
		public XmlWriterSettings()
		{
			this.Reset();
		}

		/// <summary>Creates a copy of the <see cref="T:System.Xml.XmlWriterSettings" /> instance.</summary>
		/// <returns>The cloned <see cref="T:System.Xml.XmlWriterSettings" /> object.</returns>
		public XmlWriterSettings Clone()
		{
			return (XmlWriterSettings)base.MemberwiseClone();
		}

		/// <summary>Resets the members of the settings class to their default values.</summary>
		public void Reset()
		{
			this.checkCharacters = true;
			this.closeOutput = false;
			this.conformance = ConformanceLevel.Document;
			this.encoding = Encoding.UTF8;
			this.indent = false;
			this.indentChars = "  ";
			this.newLineChars = Environment.NewLine;
			this.newLineOnAttributes = false;
			this.newLineHandling = NewLineHandling.None;
			this.omitXmlDeclaration = false;
			this.outputMethod = XmlOutputMethod.AutoDetect;
		}

		/// <summary>Gets or sets a value indicating whether to do character checking.</summary>
		/// <returns>true to do character checking; otherwise false. The default is true.</returns>
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

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Xml.XmlWriter" /> should also close the underlying stream or <see cref="T:System.IO.TextWriter" /> when the <see cref="M:System.Xml.XmlWriter.Close" /> method is called.</summary>
		/// <returns>true to also close the underlying stream or <see cref="T:System.IO.TextWriter" />; otherwise false. The default is false.</returns>
		public bool CloseOutput
		{
			get
			{
				return this.closeOutput;
			}
			set
			{
				this.closeOutput = value;
			}
		}

		/// <summary>Gets or sets the level of conformance which the <see cref="T:System.Xml.XmlWriter" /> complies with.</summary>
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

		/// <summary>Gets or sets the type of text encoding to use.</summary>
		/// <returns>The text encoding to use. The default is Encoding.UTF-8.</returns>
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				this.encoding = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to indent elements.</summary>
		/// <returns>true to write individual elements on new lines and indent; otherwise false. The default is false.</returns>
		public bool Indent
		{
			get
			{
				return this.indent;
			}
			set
			{
				this.indent = value;
			}
		}

		/// <summary>Gets or sets the character string to use when indenting. This setting is used when the <see cref="P:System.Xml.XmlWriterSettings.Indent" /> property is set to true.</summary>
		/// <returns>The character string to use when indenting. This can be set to any string value. However, to ensure valid XML, you should specify only valid white space characters, such as space characters, tabs, carriage returns, or line feeds. The default is two spaces.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value assigned to the <see cref="P:System.Xml.XmlWriterSettings.IndentChars" /> is null. </exception>
		public string IndentChars
		{
			get
			{
				return this.indentChars;
			}
			set
			{
				this.indentChars = value;
			}
		}

		/// <summary>Gets or sets the character string to use for line breaks. </summary>
		/// <returns>The character string to use for line breaks. This can be set to any string value. However, to ensure valid XML, you should specify only valid white space characters, such as space characters, tabs, carriage returns, or line feeds. The default is \r\n (carriage return, new line).</returns>
		/// <exception cref="T:System.ArgumentNullException">The value assigned to the <see cref="P:System.Xml.XmlWriterSettings.NewLineChars" /> is null. </exception>
		public string NewLineChars
		{
			get
			{
				return this.newLineChars;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.newLineChars = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to write attributes on a new line.</summary>
		/// <returns>true to write attributes on individual lines; otherwise false. The default is false.Note:This setting has no effect when the <see cref="P:System.Xml.XmlWriterSettings.Indent" /> property value is false.When <see cref="P:System.Xml.XmlWriterSettings.NewLineOnAttributes" /> is set to true, each attribute is pre-pended with a new line and one extra level of indentation.</returns>
		public bool NewLineOnAttributes
		{
			get
			{
				return this.newLineOnAttributes;
			}
			set
			{
				this.newLineOnAttributes = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to normalize line breaks in the output.</summary>
		/// <returns>One of the <see cref="T:System.Xml.NewLineHandling" /> values. The default is <see cref="F:System.Xml.NewLineHandling.Replace" />.</returns>
		public NewLineHandling NewLineHandling
		{
			get
			{
				return this.newLineHandling;
			}
			set
			{
				this.newLineHandling = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to write an XML declaration.</summary>
		/// <returns>true to omit the XML declaration; otherwise false. The default is false, an XML declaration is written.</returns>
		public bool OmitXmlDeclaration
		{
			get
			{
				return this.omitXmlDeclaration;
			}
			set
			{
				this.omitXmlDeclaration = value;
			}
		}

		/// <summary>Gets the method used to serialize the <see cref="T:System.Xml.XmlWriter" /> output.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XmlOutputMethod" /> values. The default is <see cref="F:System.Xml.XmlOutputMethod.Xml" />.</returns>
		public XmlOutputMethod OutputMethod
		{
			get
			{
				return this.outputMethod;
			}
		}

		internal NamespaceHandling NamespaceHandling { get; set; }
	}
}
