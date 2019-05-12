using System;

namespace System.Xml.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.Xml.Serialization.XmlSerializer.UnknownElement" /> event.</summary>
	public class XmlElementEventArgs : EventArgs
	{
		private XmlElement attr;

		private int lineNumber;

		private int linePosition;

		private object obj;

		private string expectedElements;

		internal XmlElementEventArgs(XmlElement attr, int lineNum, int linePos, object source)
		{
			this.attr = attr;
			this.lineNumber = lineNum;
			this.linePosition = linePos;
			this.obj = source;
		}

		/// <summary>Gets the object that represents the unknown XML element.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlElement" />.</returns>
		public XmlElement Element
		{
			get
			{
				return this.attr;
			}
		}

		/// <summary>Gets the line number where the unknown element was encountered if the XML reader is an <see cref="T:System.Xml.XmlTextReader" />.</summary>
		/// <returns>The line number where the unknown element was encountered if the XML reader is an <see cref="T:System.Xml.XmlTextReader" />; otherwise, -1.</returns>
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		/// <summary>Gets the place in the line where the unknown element occurs if the XML reader is an <see cref="T:System.Xml.XmlTextReader" />.</summary>
		/// <returns>The number in the line where the unknown element occurs if the XML reader is an <see cref="T:System.Xml.XmlTextReader" />; otherwise, -1.</returns>
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		/// <summary>Gets the object the <see cref="T:System.Xml.Serialization.XmlSerializer" /> is deserializing.</summary>
		/// <returns>The object that is being deserialized by the <see cref="T:System.Xml.Serialization.XmlSerializer" />.</returns>
		public object ObjectBeingDeserialized
		{
			get
			{
				return this.obj;
			}
		}

		/// <summary>Gets a comma-delimited list of XML element names expected to be in an XML document instance.</summary>
		/// <returns>A comma-delimited list of XML element names. Each name is in the following format: <paramref name="namespace" />:<paramref name="name" />.</returns>
		public string ExpectedElements
		{
			get
			{
				return this.expectedElements;
			}
			internal set
			{
				this.expectedElements = value;
			}
		}
	}
}
