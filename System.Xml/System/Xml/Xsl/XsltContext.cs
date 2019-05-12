using System;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	/// <summary>Encapsulates the current execution context of the Extensible Stylesheet Language for Transformations (XSLT) processor allowing XML Path Language (XPath) to resolve functions, parameters, and namespaces within XPath expressions.</summary>
	public abstract class XsltContext : XmlNamespaceManager
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XsltContext" /> class.</summary>
		protected XsltContext() : base(new NameTable())
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Xsl.XsltContext" /> class with the specified <see cref="T:System.Xml.NameTable" />.</summary>
		/// <param name="table">The <see cref="T:System.Xml.NameTable" /> to use. </param>
		protected XsltContext(NameTable table) : base(table)
		{
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether to include white space nodes in the output.</summary>
		/// <returns>true to check white space nodes in the source document for inclusion in the output; false to not evaluate white space nodes. The default is true.</returns>
		public abstract bool Whitespace { get; }

		/// <summary>When overridden in a derived class, evaluates whether to preserve white space nodes or strip them for the given context.</summary>
		/// <returns>Returns true if the white space is to be preserved or false if the white space is to be stripped.</returns>
		/// <param name="node">The white space node that is to be preserved or stripped in the current context. </param>
		public abstract bool PreserveWhitespace(XPathNavigator nav);

		/// <summary>When overridden in a derived class, compares the base Uniform Resource Identifiers (URIs) of two documents based upon the order the documents were loaded by the XSLT processor (that is, the <see cref="T:System.Xml.Xsl.XslTransform" /> class).</summary>
		/// <returns>An integer value describing the relative order of the two base URIs: -1 if <paramref name="baseUri" /> occurs before <paramref name="nextbaseUri" />; 0 if the two base URIs are identical; and 1 if <paramref name="baseUri" /> occurs after <paramref name="nextbaseUri" />.</returns>
		/// <param name="baseUri">The base URI of the first document to compare. </param>
		/// <param name="nextbaseUri">The base URI of the second document to compare. </param>
		public abstract int CompareDocument(string baseUri, string nextbaseUri);

		/// <summary>When overridden in a derived class, resolves a function reference and returns an <see cref="T:System.Xml.Xsl.IXsltContextFunction" /> representing the function. The <see cref="T:System.Xml.Xsl.IXsltContextFunction" /> is used at execution time to get the return value of the function.</summary>
		/// <returns>An <see cref="T:System.Xml.Xsl.IXsltContextFunction" /> representing the function.</returns>
		/// <param name="prefix">The prefix of the function as it appears in the XPath expression. </param>
		/// <param name="name">The name of the function. </param>
		/// <param name="ArgTypes">An array of argument types for the function being resolved. This allows you to select between methods with the same name (for example, overloaded methods). </param>
		public abstract IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes);

		/// <summary>When overridden in a derived class, resolves a variable reference and returns an <see cref="T:System.Xml.Xsl.IXsltContextVariable" /> representing the variable.</summary>
		/// <returns>An <see cref="T:System.Xml.Xsl.IXsltContextVariable" /> representing the variable at runtime.</returns>
		/// <param name="prefix">The prefix of the variable as it appears in the XPath expression. </param>
		/// <param name="name">The name of the variable. </param>
		public abstract IXsltContextVariable ResolveVariable(string prefix, string name);

		internal virtual IXsltContextVariable ResolveVariable(XmlQualifiedName name)
		{
			return this.ResolveVariable(this.LookupPrefix(name.Namespace), name.Name);
		}

		internal virtual IXsltContextFunction ResolveFunction(XmlQualifiedName name, XPathResultType[] argTypes)
		{
			return this.ResolveFunction(name.Name, name.Namespace, argTypes);
		}
	}
}
