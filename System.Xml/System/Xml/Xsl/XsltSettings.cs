using System;

namespace System.Xml.Xsl
{
	/// <summary>Specifies the XSLT features to support during execution of the XSLT style sheet.</summary>
	public sealed class XsltSettings
	{
		private static readonly XsltSettings defaultSettings = new XsltSettings(true);

		private static readonly XsltSettings trustedXslt = new XsltSettings(true);

		private bool readOnly;

		private bool enableDocument;

		private bool enableScript;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XsltSettings" /> class with default settings.</summary>
		public XsltSettings()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XsltSettings" /> class with the specified settings.</summary>
		/// <param name="enableDocumentFunction">true to enable support for the XSLT document() function; otherwise, false.</param>
		/// <param name="enableScript">true to enable support for embedded scripts blocks; otherwise, false.</param>
		public XsltSettings(bool enableDocumentFunction, bool enableScript)
		{
			this.enableDocument = enableDocumentFunction;
			this.enableScript = enableScript;
		}

		private XsltSettings(bool readOnly)
		{
			this.readOnly = readOnly;
		}

		static XsltSettings()
		{
			XsltSettings.trustedXslt.enableDocument = true;
			XsltSettings.trustedXslt.enableScript = true;
		}

		/// <summary>Gets an <see cref="T:System.Xml.Xsl.XsltSettings" /> object with default settings. Support for the XSLT document() function and embedded script blocks is disabled.</summary>
		/// <returns>An <see cref="T:System.Xml.Xsl.XsltSettings" /> object with the <see cref="P:System.Xml.Xsl.XsltSettings.EnableDocumentFunction" /> and <see cref="P:System.Xml.Xsl.XsltSettings.EnableScript" /> properties set to false.</returns>
		public static XsltSettings Default
		{
			get
			{
				return XsltSettings.defaultSettings;
			}
		}

		/// <summary>Gets an <see cref="T:System.Xml.Xsl.XsltSettings" /> object that enables support for the XSLT document() function and embedded script blocks.</summary>
		/// <returns>An <see cref="T:System.Xml.Xsl.XsltSettings" /> object with the <see cref="P:System.Xml.Xsl.XsltSettings.EnableDocumentFunction" /> and <see cref="P:System.Xml.Xsl.XsltSettings.EnableScript" /> properties set to true.</returns>
		public static XsltSettings TrustedXslt
		{
			get
			{
				return XsltSettings.trustedXslt;
			}
		}

		/// <summary>Gets or sets a value indicating whether to enable support for the XSLT document() function.</summary>
		/// <returns>true to support the XSLT document() function; otherwise, false. The default is false.</returns>
		public bool EnableDocumentFunction
		{
			get
			{
				return this.enableDocument;
			}
			set
			{
				if (!this.readOnly)
				{
					this.enableDocument = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether to enable support for embedded script blocks.</summary>
		/// <returns>true to support script blocks in XSLT style sheets; otherwise, false. The default is false.</returns>
		public bool EnableScript
		{
			get
			{
				return this.enableScript;
			}
			set
			{
				if (!this.readOnly)
				{
					this.enableScript = value;
				}
			}
		}
	}
}
