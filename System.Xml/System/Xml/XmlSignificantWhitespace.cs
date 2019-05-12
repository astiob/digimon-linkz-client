using System;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents white space between markup in a mixed content node or white space within an xml:space= 'preserve' scope. This is also referred to as significant white space.</summary>
	public class XmlSignificantWhitespace : XmlCharacterData
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlSignificantWhitespace" /> class.</summary>
		/// <param name="strData">The white space characters of the node.</param>
		/// <param name="doc">The <see cref="T:System.Xml.XmlDocument" /> object.</param>
		protected internal XmlSignificantWhitespace(string strData, XmlDocument doc) : base(strData, doc)
		{
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>For XmlSignificantWhitespace nodes, this property returns #significant-whitespace.</returns>
		public override string LocalName
		{
			get
			{
				return "#significant-whitespace";
			}
		}

		/// <summary>Gets the qualified name of the node.</summary>
		/// <returns>For XmlSignificantWhitespace nodes, this property returns #significant-whitespace.</returns>
		public override string Name
		{
			get
			{
				return "#significant-whitespace";
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>For XmlSignificantWhitespace nodes, this value is XmlNodeType.SignificantWhitespace.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.SignificantWhitespace;
			}
		}

		internal override XPathNodeType XPathNodeType
		{
			get
			{
				return XPathNodeType.SignificantWhitespace;
			}
		}

		/// <summary>Gets or sets the value of the node.</summary>
		/// <returns>The white space characters found in the node.</returns>
		/// <exception cref="T:System.ArgumentException">Setting Value to invalid white space characters. </exception>
		public override string Value
		{
			get
			{
				return this.Data;
			}
			set
			{
				if (!XmlChar.IsWhitespace(value))
				{
					throw new ArgumentException("Invalid whitespace characters.");
				}
				base.Data = value;
			}
		}

		/// <summary>Gets the parent of the current node.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> parent node of the current node.</returns>
		public override XmlNode ParentNode
		{
			get
			{
				return base.ParentNode;
			}
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The cloned node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. For significant white space nodes, the cloned node always includes the data value, regardless of the parameter setting. </param>
		public override XmlNode CloneNode(bool deep)
		{
			return new XmlSignificantWhitespace(this.Data, this.OwnerDocument);
		}

		/// <summary>Saves all the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			w.WriteWhitespace(this.Data);
		}
	}
}
