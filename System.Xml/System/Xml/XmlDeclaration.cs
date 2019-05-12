using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Xml
{
	/// <summary>Represents the XML declaration node &lt;?xml version='1.0'...?&gt;.</summary>
	public class XmlDeclaration : XmlLinkedNode
	{
		private string encoding = "UTF-8";

		private string standalone;

		private string version;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlDeclaration" /> class.</summary>
		/// <param name="version">The XML version; see the <see cref="P:System.Xml.XmlDeclaration.Version" /> property.</param>
		/// <param name="encoding">The encoding scheme; see the <see cref="P:System.Xml.XmlDeclaration.Encoding" /> property.</param>
		/// <param name="standalone">Indicates whether the XML document depends on an external DTD; see the <see cref="P:System.Xml.XmlDeclaration.Standalone" /> property.</param>
		/// <param name="doc">The parent XML document.</param>
		protected internal XmlDeclaration(string version, string encoding, string standalone, XmlDocument doc) : base(doc)
		{
			if (encoding == null)
			{
				encoding = string.Empty;
			}
			if (standalone == null)
			{
				standalone = string.Empty;
			}
			this.version = version;
			this.encoding = encoding;
			this.standalone = standalone;
		}

		/// <summary>Gets or sets the encoding level of the XML document.</summary>
		/// <returns>The valid character encoding name. The most commonly supported character encoding names for XML are the following: Category Encoding Names Unicode UTF-8, UTF-16 ISO 10646 ISO-10646-UCS-2, ISO-10646-UCS-4 ISO 8859 ISO-8859-n (where "n" is a digit from 1 to 9) JIS X-0208-1997 ISO-2022-JP, Shift_JIS, EUC-JP This value is optional. If a value is not set, this property returns String.Empty.If an encoding attribute is not included, UTF-8 encoding is assumed when the document is written or saved out.</returns>
		public string Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				this.encoding = ((value != null) ? value : string.Empty);
			}
		}

		/// <summary>Gets or sets the concatenated values of the XmlDeclaration.</summary>
		/// <returns>The concatenated values of the XmlDeclaration (that is, everything between &lt;?xml and ?&gt;).</returns>
		public override string InnerText
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.ParseInput(value);
			}
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>For XmlDeclaration nodes, the local name is xml.</returns>
		public override string LocalName
		{
			get
			{
				return "xml";
			}
		}

		/// <summary>Gets the qualified name of the node.</summary>
		/// <returns>For XmlDeclaration nodes, the name is xml.</returns>
		public override string Name
		{
			get
			{
				return "xml";
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>For XmlDeclaration nodes, this value is XmlNodeType.XmlDeclaration.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		/// <summary>Gets or sets the value of the standalone attribute.</summary>
		/// <returns>Valid values are yes if all entity declarations required by the XML document are contained within the document or no if an external document type definition (DTD) is required. If a standalone attribute is not present in the XML declaration, this property returns String.Empty.</returns>
		public string Standalone
		{
			get
			{
				return this.standalone;
			}
			set
			{
				if (value != null)
				{
					if (string.Compare(value, "YES", true, CultureInfo.InvariantCulture) == 0)
					{
						this.standalone = "yes";
					}
					if (string.Compare(value, "NO", true, CultureInfo.InvariantCulture) == 0)
					{
						this.standalone = "no";
					}
				}
				else
				{
					this.standalone = string.Empty;
				}
			}
		}

		/// <summary>Gets or sets the value of the XmlDeclaration.</summary>
		/// <returns>The contents of the XmlDeclaration (that is, everything between &lt;?xml and ?&gt;).</returns>
		public override string Value
		{
			get
			{
				string arg = string.Empty;
				string arg2 = string.Empty;
				if (this.encoding != string.Empty)
				{
					arg = string.Format(" encoding=\"{0}\"", this.encoding);
				}
				if (this.standalone != string.Empty)
				{
					arg2 = string.Format(" standalone=\"{0}\"", this.standalone);
				}
				return string.Format("version=\"{0}\"{1}{2}", this.Version, arg, arg2);
			}
			set
			{
				this.ParseInput(value);
			}
		}

		/// <summary>Gets the XML version of the document.</summary>
		/// <returns>The value is always 1.0.</returns>
		public string Version
		{
			get
			{
				return this.version;
			}
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The cloned node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. Because XmlDeclaration nodes do not have children, the cloned node always includes the data value, regardless of the parameter setting. </param>
		public override XmlNode CloneNode(bool deep)
		{
			return new XmlDeclaration(this.Version, this.Encoding, this.standalone, this.OwnerDocument);
		}

		/// <summary>Saves the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />. Because XmlDeclaration nodes do not have children, this method has no effect.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			w.WriteRaw(string.Format("<?xml {0}?>", this.Value));
		}

		private int SkipWhitespace(string input, int index)
		{
			while (index < input.Length)
			{
				if (!XmlChar.IsWhitespace((int)input[index]))
				{
					break;
				}
				index++;
			}
			return index;
		}

		private void ParseInput(string input)
		{
			int num = this.SkipWhitespace(input, 0);
			if (num + 7 > input.Length || input.IndexOf("version", num, 7) != num)
			{
				throw new XmlException("Missing 'version' specification.");
			}
			num = this.SkipWhitespace(input, num + 7);
			char c = input[num];
			if (c != '=')
			{
				throw new XmlException("Invalid 'version' specification.");
			}
			num++;
			num = this.SkipWhitespace(input, num);
			c = input[num];
			if (c != '"' && c != '\'')
			{
				throw new XmlException("Invalid 'version' specification.");
			}
			num++;
			int num2 = input.IndexOf(c, num);
			if (num2 < 0 || input.IndexOf("1.0", num, 3) != num)
			{
				throw new XmlException("Invalid 'version' specification.");
			}
			num += 4;
			if (num == input.Length)
			{
				return;
			}
			if (!XmlChar.IsWhitespace((int)input[num]))
			{
				throw new XmlException("Invalid XML declaration.");
			}
			num = this.SkipWhitespace(input, num + 1);
			if (num == input.Length)
			{
				return;
			}
			if (input.Length > num + 8 && input.IndexOf("encoding", num, 8) > 0)
			{
				num = this.SkipWhitespace(input, num + 8);
				c = input[num];
				if (c != '=')
				{
					throw new XmlException("Invalid 'version' specification.");
				}
				num++;
				num = this.SkipWhitespace(input, num);
				c = input[num];
				if (c != '"' && c != '\'')
				{
					throw new XmlException("Invalid 'encoding' specification.");
				}
				num2 = input.IndexOf(c, num + 1);
				if (num2 < 0)
				{
					throw new XmlException("Invalid 'encoding' specification.");
				}
				this.Encoding = input.Substring(num + 1, num2 - num - 1);
				num = num2 + 1;
				if (num == input.Length)
				{
					return;
				}
				if (!XmlChar.IsWhitespace((int)input[num]))
				{
					throw new XmlException("Invalid XML declaration.");
				}
				num = this.SkipWhitespace(input, num + 1);
			}
			if (input.Length > num + 10 && input.IndexOf("standalone", num, 10) > 0)
			{
				num = this.SkipWhitespace(input, num + 10);
				c = input[num];
				if (c != '=')
				{
					throw new XmlException("Invalid 'version' specification.");
				}
				num++;
				num = this.SkipWhitespace(input, num);
				c = input[num];
				if (c != '"' && c != '\'')
				{
					throw new XmlException("Invalid 'standalone' specification.");
				}
				num2 = input.IndexOf(c, num + 1);
				if (num2 < 0)
				{
					throw new XmlException("Invalid 'standalone' specification.");
				}
				string text = input.Substring(num + 1, num2 - num - 1);
				string text2 = text;
				if (text2 != null)
				{
					if (XmlDeclaration.<>f__switch$map4A == null)
					{
						XmlDeclaration.<>f__switch$map4A = new Dictionary<string, int>(2)
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
					int num3;
					if (XmlDeclaration.<>f__switch$map4A.TryGetValue(text2, out num3))
					{
						if (num3 == 0)
						{
							this.Standalone = text;
							num = num2 + 1;
							num = this.SkipWhitespace(input, num);
							goto IL_308;
						}
					}
				}
				throw new XmlException("Invalid standalone specification.");
			}
			IL_308:
			if (num != input.Length)
			{
				throw new XmlException("Invalid XML declaration.");
			}
		}
	}
}
