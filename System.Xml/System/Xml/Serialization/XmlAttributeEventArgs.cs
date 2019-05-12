using System;

namespace System.Xml.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.Xml.Serialization.XmlSerializer.UnknownAttribute" /> event.</summary>
	public class XmlAttributeEventArgs : EventArgs
	{
		private XmlAttribute attr;

		private int lineNumber;

		private int linePosition;

		private object obj;

		private string expectedAttributes;

		internal XmlAttributeEventArgs(XmlAttribute attr, int lineNum, int linePos, object source)
		{
			this.attr = attr;
			this.lineNumber = lineNum;
			this.linePosition = linePos;
			this.obj = source;
		}

		/// <summary>Gets an object that represents the unknown XML attribute.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlAttribute" /> that represents the unknown XML attribute.</returns>
		public XmlAttribute Attr
		{
			get
			{
				return this.attr;
			}
		}

		/// <summary>Gets the line number of the unknown XML attribute.</summary>
		/// <returns>The line number of the unknown XML attribute.</returns>
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		/// <summary>Gets the position in the line of the unknown XML attribute.</summary>
		/// <returns>The position number of the unknown XML attribute.</returns>
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		/// <summary>Gets the object being deserialized.</summary>
		/// <returns>The object being deserialized.</returns>
		public object ObjectBeingDeserialized
		{
			get
			{
				return this.obj;
			}
		}

		/// <summary>Gets a comma-delimited list of XML attribute names expected to be in an XML document instance.</summary>
		/// <returns>A comma-delimited list of XML attribute names. Each name is in the following format: <paramref name="namespace" />:<paramref name="name" />.</returns>
		public string ExpectedAttributes
		{
			get
			{
				return this.expectedAttributes;
			}
			internal set
			{
				this.expectedAttributes = value;
			}
		}
	}
}
