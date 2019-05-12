using System;

namespace System.Xml.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.Xml.Serialization.XmlSerializer.UnknownNode" /> event.</summary>
	public class XmlNodeEventArgs : EventArgs
	{
		private int linenumber;

		private int lineposition;

		private string localname;

		private string name;

		private string nsuri;

		private XmlNodeType nodetype;

		private object source;

		private string text;

		internal XmlNodeEventArgs(int linenumber, int lineposition, string localname, string name, string nsuri, XmlNodeType nodetype, object source, string text)
		{
			this.linenumber = linenumber;
			this.lineposition = lineposition;
			this.localname = localname;
			this.name = name;
			this.nsuri = nsuri;
			this.nodetype = nodetype;
			this.source = source;
			this.text = text;
		}

		/// <summary>Gets the line number of the unknown XML node.</summary>
		/// <returns>The line number of the unknown XML node.</returns>
		public int LineNumber
		{
			get
			{
				return this.linenumber;
			}
		}

		/// <summary>Gets the position in the line of the unknown XML node.</summary>
		/// <returns>The position number of the unknown XML node.</returns>
		public int LinePosition
		{
			get
			{
				return this.lineposition;
			}
		}

		/// <summary>Gets the XML local name of the XML node being deserialized.</summary>
		/// <returns>The XML local name of the node being deserialized.</returns>
		public string LocalName
		{
			get
			{
				return this.localname;
			}
		}

		/// <summary>Gets the name of the XML node being deserialized.</summary>
		/// <returns>The name of the node being deserialized.</returns>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets the namespace URI that is associated with the XML node being deserialized.</summary>
		/// <returns>The namespace URI that is associated with the XML node being deserialized.</returns>
		public string NamespaceURI
		{
			get
			{
				return this.nsuri;
			}
		}

		/// <summary>Gets the type of the XML node being deserialized.</summary>
		/// <returns>The <see cref="T:System.Xml.XmlNodeType" /> that represents the XML node being deserialized.</returns>
		public XmlNodeType NodeType
		{
			get
			{
				return this.nodetype;
			}
		}

		/// <summary>Gets the object being deserialized.</summary>
		/// <returns>The <see cref="T:System.Object" /> being deserialized.</returns>
		public object ObjectBeingDeserialized
		{
			get
			{
				return this.source;
			}
		}

		/// <summary>Gets the text of the XML node being deserialized.</summary>
		/// <returns>The text of the XML node being deserialized.</returns>
		public string Text
		{
			get
			{
				return this.text;
			}
		}
	}
}
