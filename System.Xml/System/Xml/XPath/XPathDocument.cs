using Mono.Xml.XPath;
using System;
using System.IO;

namespace System.Xml.XPath
{
	/// <summary>Provides a fast, read-only, in-memory representation of an XML document by using the XPath data model.</summary>
	public class XPathDocument : IXPathNavigable
	{
		private IXPathNavigable document;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathDocument" /> class from the XML data in the specified <see cref="T:System.IO.Stream" /> object.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> object that contains the XML data.</param>
		/// <exception cref="T:System.Xml.XmlException">An error was encountered in the XML data. The <see cref="T:System.Xml.XPath.XPathDocument" /> remains empty. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.IO.Stream" /> object passed as a parameter is null.</exception>
		public XPathDocument(Stream stream)
		{
			this.Initialize(new XmlValidatingReader(new XmlTextReader(stream))
			{
				ValidationType = ValidationType.None
			}, XmlSpace.None);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathDocument" /> class from the XML data in the specified file.</summary>
		/// <param name="uri">The path of the file that contains the XML data.</param>
		/// <exception cref="T:System.Xml.XmlException">An error was encountered in the XML data. The <see cref="T:System.Xml.XPath.XPathDocument" /> remains empty. </exception>
		/// <exception cref="T:System.ArgumentNullException">The file path parameter is null.</exception>
		public XPathDocument(string uri) : this(uri, XmlSpace.None)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathDocument" /> class from the XML data that is contained in the specified <see cref="T:System.IO.TextReader" /> object.</summary>
		/// <param name="textReader">The <see cref="T:System.IO.TextReader" /> object that contains the XML data.</param>
		/// <exception cref="T:System.Xml.XmlException">An error was encountered in the XML data. The <see cref="T:System.Xml.XPath.XPathDocument" /> remains empty. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.IO.TextReader" /> object passed as a parameter is null.</exception>
		public XPathDocument(TextReader reader)
		{
			this.Initialize(new XmlValidatingReader(new XmlTextReader(reader))
			{
				ValidationType = ValidationType.None
			}, XmlSpace.None);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathDocument" /> class from the XML data that is contained in the specified <see cref="T:System.Xml.XmlReader" /> object.</summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> object that contains the XML data. </param>
		/// <exception cref="T:System.Xml.XmlException">An error was encountered in the XML data. The <see cref="T:System.Xml.XPath.XPathDocument" /> remains empty. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object passed as a parameter is null.</exception>
		public XPathDocument(XmlReader reader) : this(reader, XmlSpace.None)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathDocument" /> class from the XML data in the file specified with the white space handling specified.</summary>
		/// <param name="uri">The path of the file that contains the XML data.</param>
		/// <param name="space">An <see cref="T:System.Xml.XmlSpace" /> object.</param>
		/// <exception cref="T:System.Xml.XmlException">An error was encountered in the XML data. The <see cref="T:System.Xml.XPath.XPathDocument" /> remains empty. </exception>
		/// <exception cref="T:System.ArgumentNullException">The file path parameter or <see cref="T:System.Xml.XmlSpace" /> object parameter is null.</exception>
		public XPathDocument(string uri, XmlSpace space)
		{
			XmlValidatingReader xmlValidatingReader = null;
			try
			{
				xmlValidatingReader = new XmlValidatingReader(new XmlTextReader(uri));
				xmlValidatingReader.ValidationType = ValidationType.None;
				this.Initialize(xmlValidatingReader, space);
			}
			finally
			{
				if (xmlValidatingReader != null)
				{
					xmlValidatingReader.Close();
				}
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XPath.XPathDocument" /> class from the XML data that is contained in the specified <see cref="T:System.Xml.XmlReader" /> object with the specified white space handling.</summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> object that contains the XML data.</param>
		/// <param name="space">An <see cref="T:System.Xml.XmlSpace" /> object.</param>
		/// <exception cref="T:System.Xml.XmlException">An error was encountered in the XML data. The <see cref="T:System.Xml.XPath.XPathDocument" /> remains empty. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object parameter or <see cref="T:System.Xml.XmlSpace" /> object parameter is null.</exception>
		public XPathDocument(XmlReader reader, XmlSpace space)
		{
			this.Initialize(reader, space);
		}

		private void Initialize(XmlReader reader, XmlSpace space)
		{
			this.document = new DTMXPathDocumentBuilder2(reader, space).CreateDocument();
		}

		/// <summary>Initializes a read-only <see cref="T:System.Xml.XPath.XPathNavigator" /> object for navigating through nodes in this <see cref="T:System.Xml.XPath.XPathDocument" />.</summary>
		/// <returns>A read-only <see cref="T:System.Xml.XPath.XPathNavigator" /> object.</returns>
		public XPathNavigator CreateNavigator()
		{
			return this.document.CreateNavigator();
		}
	}
}
