using Mono.Xml.Xsl;
using System;
using System.IO;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	/// <summary>Transforms XML data using an XSLT style sheet.</summary>
	[MonoTODO]
	public sealed class XslCompiledTransform
	{
		private bool enable_debug;

		private object debugger;

		private CompiledStylesheet s;

		private XmlWriterSettings output_settings = new XmlWriterSettings();

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XslCompiledTransform" /> class. </summary>
		public XslCompiledTransform() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XslCompiledTransform" /> class with the specified debug setting. </summary>
		/// <param name="enableDebug">true to generate debug information; otherwise false. Setting this to true enables you to debug the style sheet with the Microsoft Visual Studio Debugger.</param>
		public XslCompiledTransform(bool enableDebug)
		{
			this.enable_debug = enableDebug;
			if (this.enable_debug)
			{
				this.debugger = new NoOperationDebugger();
			}
			this.output_settings.ConformanceLevel = ConformanceLevel.Fragment;
		}

		/// <summary>Gets an <see cref="T:System.Xml.XmlWriterSettings" /> object that contains the output information derived from the xsl:output element of the style sheet.</summary>
		/// <returns>A read-only <see cref="T:System.Xml.XmlWriterSettings" /> object that contains the output information derived from the xsl:output element of the style sheet. This value can be null.</returns>
		[MonoTODO]
		public XmlWriterSettings OutputSettings
		{
			get
			{
				return this.output_settings;
			}
		}

		/// <summary>Executes the transform using the input document specified by the URI and outputs the results to a file.</summary>
		/// <param name="inputUri">The URI of the input document.</param>
		/// <param name="resultsFile">The URI of the output file.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputUri" /> or <paramref name="resultsFile" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The input document cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The <paramref name="inputUri" /> or <paramref name="resultsFile" /> value includes a filename or directory cannot be found.</exception>
		/// <exception cref="T:System.Net.WebException">The <paramref name="inputUri" /> or <paramref name="resultsFile" /> value cannot be resolved.-or-An error occurred while processing the request</exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="inputUri" /> or <paramref name="resultsFile" /> is not a valid URI.</exception>
		/// <exception cref="T:System.Xml.XmlException">There was a parsing error loading the input document.</exception>
		public void Transform(string inputfile, string outputfile)
		{
			using (Stream stream = File.Create(outputfile))
			{
				this.Transform(new XPathDocument(inputfile, XmlSpace.Preserve), null, stream);
			}
		}

		/// <summary>Executes the transform using the input document specified by the URI and outputs the results to an <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="inputUri">The URI of the input document.</param>
		/// <param name="results">The <see cref="T:System.Xml.XmlWriter" /> to which you want to output.If the style sheet contains an xsl:output element, you should create the <see cref="T:System.Xml.XmlWriter" /> using the <see cref="T:System.Xml.XmlWriterSettings" /> object returned from the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property. This ensures that the <see cref="T:System.Xml.XmlWriter" /> has the correct output settings.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputUri" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The <paramref name="inputUri" /> value includes a filename or directory cannot be found.</exception>
		/// <exception cref="T:System.Net.WebException">The <paramref name="inputUri" /> value cannot be resolved.-or-An error occurred while processing the request.</exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="inputUri" /> is not a valid URI.</exception>
		/// <exception cref="T:System.Xml.XmlException">There was a parsing error loading the input document.</exception>
		public void Transform(string inputfile, XmlWriter output)
		{
			this.Transform(inputfile, null, output);
		}

		/// <summary>Executes the transform using the input document specified by the URI and outputs the results to stream. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments.</summary>
		/// <param name="inputUri">The URI of the input document.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The stream to which you want to output.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputUri" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The <paramref name="inputUri" /> value includes a filename or directory cannot be found.</exception>
		/// <exception cref="T:System.Net.WebException">The <paramref name="inputUri" /> value cannot be resolved.-or-An error occurred while processing the request</exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="inputUri" /> is not a valid URI.</exception>
		/// <exception cref="T:System.Xml.XmlException">There was a parsing error loading the input document.</exception>
		public void Transform(string inputfile, XsltArgumentList args, Stream output)
		{
			this.Transform(new XPathDocument(inputfile, XmlSpace.Preserve), args, output);
		}

		/// <summary>Executes the transform using the input document specified by the URI and outputs the results to a <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="inputUri">The URI of the input document.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The <see cref="T:System.IO.TextWriter" /> to which you want to output.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputUri" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The <paramref name="inputUri" /> value includes a filename or directory cannot be found.</exception>
		/// <exception cref="T:System.Net.WebException">The <paramref name="inputUri" /> value cannot be resolved.-or-An error occurred while processing the request</exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="inputUri" /> is not a valid URI.</exception>
		/// <exception cref="T:System.Xml.XmlException">There was a parsing error loading the input document.</exception>
		public void Transform(string inputfile, XsltArgumentList args, TextWriter output)
		{
			this.Transform(new XPathDocument(inputfile, XmlSpace.Preserve), args, output);
		}

		/// <summary>Executes the transform using the input document specified by the URI and outputs the results to an <see cref="T:System.Xml.XmlWriter" />. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments.</summary>
		/// <param name="inputUri">The URI of the input document.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The <see cref="T:System.Xml.XmlWriter" /> to which you want to output.If the style sheet contains an xsl:output element, you should create the <see cref="T:System.Xml.XmlWriter" /> using the <see cref="T:System.Xml.XmlWriterSettings" /> object returned from the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property. This ensures that the <see cref="T:System.Xml.XmlWriter" /> has the correct output settings.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputUri" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The <paramref name="inputtUri" /> value includes a filename or directory cannot be found.</exception>
		/// <exception cref="T:System.Net.WebException">The <paramref name="inputUri" /> value cannot be resolved.-or-An error occurred while processing the request.</exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="inputUri" /> is not a valid URI.</exception>
		/// <exception cref="T:System.Xml.XmlException">There was a parsing error loading the input document.</exception>
		public void Transform(string inputfile, XsltArgumentList args, XmlWriter output)
		{
			this.Transform(new XPathDocument(inputfile, XmlSpace.Preserve), args, output);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XmlReader" /> object and outputs the results to an <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="input">The <see cref="T:System.Xml.XmlReader" /> containing the input document.</param>
		/// <param name="results">The <see cref="T:System.Xml.XmlWriter" /> to which you want to output.If the style sheet contains an xsl:output element, you should create the <see cref="T:System.Xml.XmlWriter" /> using the <see cref="T:System.Xml.XmlWriterSettings" /> object returned from the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property. This ensures that the <see cref="T:System.Xml.XmlWriter" /> has the correct output settings.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(XmlReader reader, XmlWriter output)
		{
			this.Transform(reader, null, output);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XmlReader" /> object and outputs the results to a stream. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments.</summary>
		/// <param name="input">An <see cref="T:System.Xml.XmlReader" /> containing the input document.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The stream to which you want to output.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(XmlReader reader, XsltArgumentList args, Stream output)
		{
			this.Transform(new XPathDocument(reader, XmlSpace.Preserve), args, output);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XmlReader" /> object and outputs the results to a <see cref="T:System.IO.TextWriter" />. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments.</summary>
		/// <param name="input">An <see cref="T:System.Xml.XmlReader" /> containing the input document.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The <see cref="T:System.IO.TextWriter" /> to which you want to output.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(XmlReader reader, XsltArgumentList args, TextWriter output)
		{
			this.Transform(new XPathDocument(reader, XmlSpace.Preserve), args, output);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XmlReader" /> object and outputs the results to an <see cref="T:System.Xml.XmlWriter" />. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments.</summary>
		/// <param name="input">An <see cref="T:System.Xml.XmlReader" /> containing the input document.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The <see cref="T:System.Xml.XmlWriter" /> to which you want to output.If the style sheet contains an xsl:output element, you should create the <see cref="T:System.Xml.XmlWriter" /> using the <see cref="T:System.Xml.XmlWriterSettings" /> object returned from the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property. This ensures that the <see cref="T:System.Xml.XmlWriter" /> has the correct output settings.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(XmlReader reader, XsltArgumentList args, XmlWriter output)
		{
			this.Transform(reader, args, output, null);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XPath.IXPathNavigable" /> object and outputs the results to an <see cref="T:System.IO.TextWriter" />. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments.</summary>
		/// <param name="input">An object implementing the <see cref="T:System.Xml.XPath.IXPathNavigable" /> interface. In the Microsoft .NET Framework, this can be either an <see cref="T:System.Xml.XmlNode" /> (typically an <see cref="T:System.Xml.XmlDocument" />), or an <see cref="T:System.Xml.XPath.XPathDocument" /> containing the data to be transformed.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The <see cref="T:System.IO.TextWriter" /> to which you want to output.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(IXPathNavigable input, XsltArgumentList args, TextWriter output)
		{
			this.Transform(input.CreateNavigator(), args, output);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XPath.IXPathNavigable" /> object and outputs the results to a stream. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional runtime arguments.</summary>
		/// <param name="input">An object implementing the <see cref="T:System.Xml.XPath.IXPathNavigable" /> interface. In the Microsoft .NET Framework, this can be either an <see cref="T:System.Xml.XmlNode" /> (typically an <see cref="T:System.Xml.XmlDocument" />), or an <see cref="T:System.Xml.XPath.XPathDocument" /> containing the data to be transformed.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The stream to which you want to output.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(IXPathNavigable input, XsltArgumentList args, Stream output)
		{
			this.Transform(input.CreateNavigator(), args, output);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XPath.IXPathNavigable" /> object and outputs the results to an <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="input">An object implementing the <see cref="T:System.Xml.XPath.IXPathNavigable" /> interface. In the Microsoft .NET Framework, this can be either an <see cref="T:System.Xml.XmlNode" /> (typically an <see cref="T:System.Xml.XmlDocument" />), or an <see cref="T:System.Xml.XPath.XPathDocument" /> containing the data to be transformed.</param>
		/// <param name="results">The <see cref="T:System.Xml.XmlWriter" /> to which you want to output.If the style sheet contains an xsl:output element, you should create the <see cref="T:System.Xml.XmlWriter" /> using the <see cref="T:System.Xml.XmlWriterSettings" /> object returned from the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property. This ensures that the <see cref="T:System.Xml.XmlWriter" /> has the correct output settings.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(IXPathNavigable input, XmlWriter output)
		{
			this.Transform(input, null, output);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XPath.IXPathNavigable" /> object and outputs the results to an <see cref="T:System.Xml.XmlWriter" />. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments.</summary>
		/// <param name="input">An object implementing the <see cref="T:System.Xml.XPath.IXPathNavigable" /> interface. In the Microsoft .NET Framework, this can be either an <see cref="T:System.Xml.XmlNode" /> (typically an <see cref="T:System.Xml.XmlDocument" />), or an <see cref="T:System.Xml.XPath.XPathDocument" /> containing the data to be transformed.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The <see cref="T:System.Xml.XmlWriter" /> to which you want to output.If the style sheet contains an xsl:output element, you should create the <see cref="T:System.Xml.XmlWriter" /> using the <see cref="T:System.Xml.XmlWriterSettings" /> object returned from the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property. This ensures that the <see cref="T:System.Xml.XmlWriter" /> has the correct output settings.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(IXPathNavigable input, XsltArgumentList args, XmlWriter output)
		{
			this.Transform(input.CreateNavigator(), args, output, null);
		}

		/// <summary>Executes the transform using the input document specified by the <see cref="T:System.Xml.XmlReader" /> object and outputs the results to an <see cref="T:System.Xml.XmlWriter" />. The <see cref="T:System.Xml.Xsl.XsltArgumentList" /> provides additional run-time arguments and the XmlResolver resolves the XSLT document() function.</summary>
		/// <param name="input">An <see cref="T:System.Xml.XmlReader" /> containing the input document.</param>
		/// <param name="arguments">An <see cref="T:System.Xml.Xsl.XsltArgumentList" /> containing the namespace-qualified arguments used as input to the transform. This value can be null.</param>
		/// <param name="results">The <see cref="T:System.Xml.XmlWriter" /> to which you want to output.If the style sheet contains an xsl:output element, you should create the <see cref="T:System.Xml.XmlWriter" /> using the <see cref="T:System.Xml.XmlWriterSettings" /> object returned from the <see cref="P:System.Xml.Xsl.XslCompiledTransform.OutputSettings" /> property. This ensures that the <see cref="T:System.Xml.XmlWriter" /> has the correct output settings.</param>
		/// <param name="documentResolver">The <see cref="T:System.Xml.XmlResolver" /> used to resolve the XSLT document() function. If this is null, the document() function is not resolved.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="input" /> or <paramref name="results" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
		public void Transform(XmlReader input, XsltArgumentList args, XmlWriter output, XmlResolver resolver)
		{
			this.Transform(new XPathDocument(input, XmlSpace.Preserve).CreateNavigator(), args, output, resolver);
		}

		private void Transform(XPathNavigator input, XsltArgumentList args, XmlWriter output, XmlResolver resolver)
		{
			if (this.s == null)
			{
				throw new XsltException("No stylesheet was loaded.", null);
			}
			Outputter outputtter = new GenericOutputter(output, this.s.Outputs, null);
			new XslTransformProcessor(this.s, this.debugger).Process(input, outputtter, args, resolver);
			output.Flush();
		}

		private void Transform(XPathNavigator input, XsltArgumentList args, Stream output)
		{
			XslOutput xslOutput = (XslOutput)this.s.Outputs[string.Empty];
			this.Transform(input, args, new StreamWriter(output, xslOutput.Encoding));
		}

		private void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output)
		{
			if (this.s == null)
			{
				throw new XsltException("No stylesheet was loaded.", null);
			}
			Outputter outputter = new GenericOutputter(output, this.s.Outputs, output.Encoding);
			new XslTransformProcessor(this.s, this.debugger).Process(input, outputter, args, null);
			outputter.Done();
			output.Flush();
		}

		private XmlReader GetXmlReader(string url)
		{
			XmlResolver xmlResolver = new XmlUrlResolver();
			Uri uri = xmlResolver.ResolveUri(null, url);
			Stream input = xmlResolver.GetEntity(uri, null, typeof(Stream)) as Stream;
			return new XmlValidatingReader(new XmlTextReader(uri.ToString(), input)
			{
				XmlResolver = xmlResolver
			})
			{
				XmlResolver = xmlResolver,
				ValidationType = ValidationType.None
			};
		}

		/// <summary>Loads and compiles the style sheet located at the specified URI.</summary>
		/// <param name="stylesheetUri">The URI of the style sheet.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stylesheetUri" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">The style sheet contains an error.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The style sheet cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The <paramref name="stylesheetUri" /> value includes a filename or directory that cannot be found.</exception>
		/// <exception cref="T:System.Net.WebException">The <paramref name="stylesheetUri" /> value cannot be resolved.-or-An error occurred while processing the request.</exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="stylesheetUri" /> is not a valid URI.</exception>
		/// <exception cref="T:System.Xml.XmlException">There was a parsing error loading the style sheet.</exception>
		public void Load(string url)
		{
			using (XmlReader xmlReader = this.GetXmlReader(url))
			{
				this.Load(xmlReader);
			}
		}

		/// <summary>Compiles the style sheet contained in the <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <param name="stylesheet">An <see cref="T:System.Xml.XmlReader" /> containing the style sheet.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stylesheet" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">The style sheet contains an error.</exception>
		public void Load(XmlReader stylesheet)
		{
			this.Load(stylesheet, null, null);
		}

		/// <summary>Compiles the style sheet contained in the <see cref="T:System.Xml.XPath.IXPathNavigable" /> object.</summary>
		/// <param name="stylesheet">An object implementing the <see cref="T:System.Xml.XPath.IXPathNavigable" /> interface. In the Microsoft .NET Framework, this can be either an <see cref="T:System.Xml.XmlNode" /> (typically an <see cref="T:System.Xml.XmlDocument" />), or an <see cref="T:System.Xml.XPath.XPathDocument" /> containing the style sheet.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stylesheet" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">The style sheet contains an error.</exception>
		public void Load(IXPathNavigable stylesheet)
		{
			this.Load(stylesheet.CreateNavigator(), null, null);
		}

		/// <summary>Compiles the XSLT style sheet contained in the <see cref="T:System.Xml.XPath.IXPathNavigable" />. The <see cref="T:System.Xml.XmlResolver" /> resolves any XSLT import or include elements and the XSLT settings determine the permissions for the style sheet.</summary>
		/// <param name="stylesheet">An object implementing the <see cref="T:System.Xml.XPath.IXPathNavigable" /> interface. In the Microsoft .NET Framework, this can be either an <see cref="T:System.Xml.XmlNode" /> (typically an <see cref="T:System.Xml.XmlDocument" />), or an <see cref="T:System.Xml.XPath.XPathDocument" /> containing the style sheet.</param>
		/// <param name="settings">The <see cref="T:System.Xml.Xsl.XsltSettings" /> to apply to the style sheet. If this is null, the <see cref="P:System.Xml.Xsl.XsltSettings.Default" /> setting is applied.</param>
		/// <param name="stylesheetResolver">The <see cref="T:System.Xml.XmlResolver" /> used to resolve any style sheets referenced in XSLT import and include elements. If this is null, external resources are not resolved.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stylesheet" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">The style sheet contains an error.</exception>
		public void Load(IXPathNavigable stylesheet, XsltSettings settings, XmlResolver resolver)
		{
			this.Load(stylesheet.CreateNavigator(), settings, resolver);
		}

		/// <summary>Compiles the XSLT style sheet contained in the <see cref="T:System.Xml.XmlReader" />. The <see cref="T:System.Xml.XmlResolver" /> resolves any XSLT import or include elements and the XSLT settings determine the permissions for the style sheet.</summary>
		/// <param name="stylesheet">The <see cref="T:System.Xml.XmlReader" /> containing the style sheet.</param>
		/// <param name="settings">The <see cref="T:System.Xml.Xsl.XsltSettings" /> to apply to the style sheet. If this is null, the <see cref="P:System.Xml.Xsl.XsltSettings.Default" /> setting is applied.</param>
		/// <param name="stylesheetResolver">The <see cref="T:System.Xml.XmlResolver" /> used to resolve any style sheets referenced in XSLT import and include elements. If this is null, external resources are not resolved.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stylesheet" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">The style sheet contains an error.</exception>
		public void Load(XmlReader stylesheet, XsltSettings settings, XmlResolver resolver)
		{
			this.Load(new XPathDocument(stylesheet, XmlSpace.Preserve).CreateNavigator(), settings, resolver);
		}

		/// <summary>Loads and compiles the XSLT style sheet specified by the URI. The <see cref="T:System.Xml.XmlResolver" /> resolves any XSLT import or include elements and the XSLT settings determine the permissions for the style sheet.</summary>
		/// <param name="stylesheetUri">The URI of the style sheet.</param>
		/// <param name="settings">The <see cref="T:System.Xml.Xsl.XsltSettings" /> to apply to the style sheet. If this is null, the <see cref="P:System.Xml.Xsl.XsltSettings.Default" /> setting is applied.</param>
		/// <param name="stylesheetResolver">The <see cref="T:System.Xml.XmlResolver" /> used to resolve the style sheet URI and any style sheets referenced in XSLT import and include elements. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stylesheetUri" /> or <paramref name="stylesheetResolver" /> value is null.</exception>
		/// <exception cref="T:System.Xml.Xsl.XsltException">The style sheet contains an error.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The style sheet cannot be found.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The <paramref name="stylesheetUri" /> value includes a filename or directory that cannot be found.</exception>
		/// <exception cref="T:System.Net.WebException">The <paramref name="stylesheetUri" /> value cannot be resolved.-or-An error occurred while processing the request.</exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="stylesheetUri" /> is not a valid URI.</exception>
		/// <exception cref="T:System.Xml.XmlException">There was a parsing error loading the style sheet.</exception>
		public void Load(string stylesheet, XsltSettings settings, XmlResolver resolver)
		{
			this.Load(new XPathDocument(stylesheet, XmlSpace.Preserve).CreateNavigator(), settings, resolver);
		}

		private void Load(XPathNavigator stylesheet, XsltSettings settings, XmlResolver resolver)
		{
			this.s = new Compiler(this.debugger).Compile(stylesheet, resolver, null);
		}
	}
}
