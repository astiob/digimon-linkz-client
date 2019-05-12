using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XslOutput
	{
		private string uri;

		private XmlQualifiedName customMethod;

		private OutputMethod method = OutputMethod.Unknown;

		private string version;

		private Encoding encoding = Encoding.UTF8;

		private bool omitXmlDeclaration;

		private StandaloneType standalone;

		private string doctypePublic;

		private string doctypeSystem;

		private XmlQualifiedName[] cdataSectionElements;

		private string indent;

		private string mediaType;

		private string stylesheetVersion;

		private ArrayList cdSectsList = new ArrayList();

		public XslOutput(string uri, string stylesheetVersion)
		{
			this.uri = uri;
			this.stylesheetVersion = stylesheetVersion;
		}

		public OutputMethod Method
		{
			get
			{
				return this.method;
			}
		}

		public XmlQualifiedName CustomMethod
		{
			get
			{
				return this.customMethod;
			}
		}

		public string Version
		{
			get
			{
				return this.version;
			}
		}

		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		public string Uri
		{
			get
			{
				return this.uri;
			}
		}

		public bool OmitXmlDeclaration
		{
			get
			{
				return this.omitXmlDeclaration;
			}
		}

		public StandaloneType Standalone
		{
			get
			{
				return this.standalone;
			}
		}

		public string DoctypePublic
		{
			get
			{
				return this.doctypePublic;
			}
		}

		public string DoctypeSystem
		{
			get
			{
				return this.doctypeSystem;
			}
		}

		public XmlQualifiedName[] CDataSectionElements
		{
			get
			{
				if (this.cdataSectionElements == null)
				{
					this.cdataSectionElements = (this.cdSectsList.ToArray(typeof(XmlQualifiedName)) as XmlQualifiedName[]);
				}
				return this.cdataSectionElements;
			}
		}

		public string Indent
		{
			get
			{
				return this.indent;
			}
		}

		public string MediaType
		{
			get
			{
				return this.mediaType;
			}
		}

		public void Fill(XPathNavigator nav)
		{
			if (nav.MoveToFirstAttribute())
			{
				this.ProcessAttribute(nav);
				while (nav.MoveToNextAttribute())
				{
					this.ProcessAttribute(nav);
				}
				nav.MoveToParent();
			}
		}

		private void ProcessAttribute(XPathNavigator nav)
		{
			if (nav.NamespaceURI != string.Empty)
			{
				return;
			}
			string value = nav.Value;
			string localName = nav.LocalName;
			switch (localName)
			{
			case "cdata-section-elements":
				if (value.Length > 0)
				{
					this.cdSectsList.AddRange(XslNameUtil.FromListString(value, nav));
				}
				return;
			case "method":
			{
				if (value.Length == 0)
				{
					return;
				}
				string text = value;
				switch (text)
				{
				case "xml":
					this.method = OutputMethod.XML;
					goto IL_269;
				case "html":
					this.omitXmlDeclaration = true;
					this.method = OutputMethod.HTML;
					goto IL_269;
				case "text":
					this.omitXmlDeclaration = true;
					this.method = OutputMethod.Text;
					goto IL_269;
				}
				this.method = OutputMethod.Custom;
				this.customMethod = XslNameUtil.FromString(value, nav);
				if (this.customMethod.Namespace == string.Empty)
				{
					IXmlLineInfo xmlLineInfo = nav as IXmlLineInfo;
					throw new XsltCompileException(new ArgumentException("Invalid output method value: '" + value + "'. It must be either 'xml' or 'html' or 'text' or QName."), nav.BaseURI, (xmlLineInfo == null) ? 0 : xmlLineInfo.LineNumber, (xmlLineInfo == null) ? 0 : xmlLineInfo.LinePosition);
				}
				IL_269:
				return;
			}
			case "version":
				if (value.Length > 0)
				{
					this.version = value;
				}
				return;
			case "encoding":
				if (value.Length > 0)
				{
					try
					{
						this.encoding = Encoding.GetEncoding(value);
					}
					catch (ArgumentException)
					{
					}
					catch (NotSupportedException)
					{
					}
				}
				return;
			case "standalone":
			{
				string text = value;
				if (text != null)
				{
					if (XslOutput.<>f__switch$map20 == null)
					{
						XslOutput.<>f__switch$map20 = new Dictionary<string, int>(2)
						{
							{
								"yes",
								0
							},
							{
								"no",
								1
							}
						};
					}
					int num2;
					if (XslOutput.<>f__switch$map20.TryGetValue(text, out num2))
					{
						if (num2 == 0)
						{
							this.standalone = StandaloneType.YES;
							goto IL_397;
						}
						if (num2 == 1)
						{
							this.standalone = StandaloneType.NO;
							goto IL_397;
						}
					}
				}
				if (!(this.stylesheetVersion != "1.0"))
				{
					IXmlLineInfo xmlLineInfo2 = nav as IXmlLineInfo;
					throw new XsltCompileException(new XsltException("'" + value + "' is an invalid value for 'standalone' attribute.", null), nav.BaseURI, (xmlLineInfo2 == null) ? 0 : xmlLineInfo2.LineNumber, (xmlLineInfo2 == null) ? 0 : xmlLineInfo2.LinePosition);
				}
				IL_397:
				return;
			}
			case "doctype-public":
				this.doctypePublic = value;
				return;
			case "doctype-system":
				this.doctypeSystem = value;
				return;
			case "media-type":
				if (value.Length > 0)
				{
					this.mediaType = value;
				}
				return;
			case "omit-xml-declaration":
			{
				string text = value;
				if (text != null)
				{
					if (XslOutput.<>f__switch$map21 == null)
					{
						XslOutput.<>f__switch$map21 = new Dictionary<string, int>(2)
						{
							{
								"yes",
								0
							},
							{
								"no",
								1
							}
						};
					}
					int num2;
					if (XslOutput.<>f__switch$map21.TryGetValue(text, out num2))
					{
						if (num2 == 0)
						{
							this.omitXmlDeclaration = true;
							goto IL_4AF;
						}
						if (num2 == 1)
						{
							this.omitXmlDeclaration = false;
							goto IL_4AF;
						}
					}
				}
				if (!(this.stylesheetVersion != "1.0"))
				{
					IXmlLineInfo xmlLineInfo3 = nav as IXmlLineInfo;
					throw new XsltCompileException(new XsltException("'" + value + "' is an invalid value for 'omit-xml-declaration' attribute.", null), nav.BaseURI, (xmlLineInfo3 == null) ? 0 : xmlLineInfo3.LineNumber, (xmlLineInfo3 == null) ? 0 : xmlLineInfo3.LinePosition);
				}
				IL_4AF:
				return;
			}
			case "indent":
			{
				this.indent = value;
				if (this.stylesheetVersion != "1.0")
				{
					return;
				}
				string text = value;
				if (text != null)
				{
					if (XslOutput.<>f__switch$map22 == null)
					{
						XslOutput.<>f__switch$map22 = new Dictionary<string, int>(2)
						{
							{
								"yes",
								0
							},
							{
								"no",
								0
							}
						};
					}
					int num2;
					if (XslOutput.<>f__switch$map22.TryGetValue(text, out num2))
					{
						if (num2 == 0)
						{
							goto IL_568;
						}
					}
				}
				OutputMethod outputMethod = this.method;
				if (outputMethod != OutputMethod.Custom)
				{
					throw new XsltCompileException(string.Format("Unexpected 'indent' attribute value in 'output' element: '{0}'", value), null, nav);
				}
				IL_568:
				return;
			}
			}
			if (!(this.stylesheetVersion != "1.0"))
			{
				IXmlLineInfo xmlLineInfo4 = nav as IXmlLineInfo;
				throw new XsltCompileException(new XsltException("'" + nav.LocalName + "' is an invalid attribute for 'output' element.", null), nav.BaseURI, (xmlLineInfo4 == null) ? 0 : xmlLineInfo4.LineNumber, (xmlLineInfo4 == null) ? 0 : xmlLineInfo4.LinePosition);
			}
		}
	}
}
