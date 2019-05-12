using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
	/// <summary>Provides a typed class that represents a compiled XPath expression.</summary>
	public abstract class XPathExpression
	{
		internal XPathExpression()
		{
		}

		/// <summary>When overridden in a derived class, gets a string representation of the <see cref="T:System.Xml.XPath.XPathExpression" />.</summary>
		/// <returns>A string representation of the <see cref="T:System.Xml.XPath.XPathExpression" />.</returns>
		public abstract string Expression { get; }

		/// <summary>When overridden in a derived class, gets the result type of the XPath expression.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathResultType" /> value representing the result type of the XPath expression.</returns>
		public abstract XPathResultType ReturnType { get; }

		/// <summary>When overridden in a derived class, sorts the nodes selected by the XPath expression according to the specified <see cref="T:System.Collections.IComparer" /> object.</summary>
		/// <param name="expr">An object representing the sort key. This can be the string value of the node or an <see cref="T:System.Xml.XPath.XPathExpression" /> object with a compiled XPath expression.</param>
		/// <param name="comparer">An <see cref="T:System.Collections.IComparer" /> object that provides the specific data type comparisons for comparing two objects for equivalence. </param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The <see cref="T:System.Xml.XPath.XPathExpression" /> or sort key includes a prefix and either an <see cref="T:System.Xml.XmlNamespaceManager" /> is not provided, or the prefix cannot be found in the supplied <see cref="T:System.Xml.XmlNamespaceManager" />.</exception>
		public abstract void AddSort(object expr, IComparer comparer);

		/// <summary>When overridden in a derived class, sorts the nodes selected by the XPath expression according to the supplied parameters.</summary>
		/// <param name="expr">An object representing the sort key. This can be the string value of the node or an <see cref="T:System.Xml.XPath.XPathExpression" /> object with a compiled XPath expression. </param>
		/// <param name="order">An <see cref="T:System.Xml.XPath.XmlSortOrder" /> value indicating the sort order. </param>
		/// <param name="caseOrder">An <see cref="T:System.Xml.XPath.XmlCaseOrder" /> value indicating how to sort uppercase and lowercase letters.</param>
		/// <param name="lang">The language to use for comparison. Uses the <see cref="T:System.Globalization.CultureInfo" /> class that can be passed to the <see cref="Overload:System.String.Compare" /> method for the language types, for example, "us-en" for U.S. English. If an empty string is specified, the system environment is used to determine the <see cref="T:System.Globalization.CultureInfo" />. </param>
		/// <param name="dataType">An <see cref="T:System.Xml.XPath.XmlDataType" /> value indicating the sort order for the data type. </param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The <see cref="T:System.Xml.XPath.XPathExpression" /> or sort key includes a prefix and either an <see cref="T:System.Xml.XmlNamespaceManager" /> is not provided, or the prefix cannot be found in the supplied <see cref="T:System.Xml.XmlNamespaceManager" />. </exception>
		public abstract void AddSort(object expr, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType);

		/// <summary>When overridden in a derived class, returns a clone of this <see cref="T:System.Xml.XPath.XPathExpression" />.</summary>
		/// <returns>A new <see cref="T:System.Xml.XPath.XPathExpression" /> object.</returns>
		public abstract XPathExpression Clone();

		/// <summary>When overridden in a derived class, specifies the <see cref="T:System.Xml.XmlNamespaceManager" /> object to use for namespace resolution.</summary>
		/// <param name="nsManager">An <see cref="T:System.Xml.XmlNamespaceManager" /> object to use for namespace resolution. </param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The <see cref="T:System.Xml.XmlNamespaceManager" /> object parameter is not derived from the <see cref="T:System.Xml.XmlNamespaceManager" /> class. </exception>
		public abstract void SetContext(XmlNamespaceManager nsManager);

		/// <summary>Compiles the XPath expression specified and returns an <see cref="T:System.Xml.XPath.XPathExpression" /> object representing the XPath expression.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathExpression" /> object.</returns>
		/// <param name="xpath">An XPath expression.</param>
		/// <exception cref="T:System.ArgumentException">The XPath expression parameter is not a valid XPath expression.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public static XPathExpression Compile(string xpath)
		{
			return XPathExpression.Compile(xpath, null, null);
		}

		/// <summary>Compiles the specified XPath expression, with the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object specified for namespace resolution, and returns an <see cref="T:System.Xml.XPath.XPathExpression" /> object that represents the XPath expression.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathExpression" /> object.</returns>
		/// <param name="xpath">An XPath expression.</param>
		/// <param name="nsResolver">An object that implements the <see cref="T:System.Xml.IXmlNamespaceResolver" /> interface for namespace resolution.</param>
		/// <exception cref="T:System.ArgumentException">The XPath expression parameter is not a valid XPath expression.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public static XPathExpression Compile(string xpath, IXmlNamespaceResolver nsmgr)
		{
			return XPathExpression.Compile(xpath, nsmgr, null);
		}

		internal static XPathExpression Compile(string xpath, IXmlNamespaceResolver nsmgr, IStaticXsltContext ctx)
		{
			XPathParser xpathParser = new XPathParser(ctx);
			CompiledExpression compiledExpression = new CompiledExpression(xpath, xpathParser.Compile(xpath));
			compiledExpression.SetContext(nsmgr);
			return compiledExpression;
		}

		/// <summary>When overridden in a derived class, specifies the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object to use for namespace resolution.</summary>
		/// <param name="nsResolver">An object that implements the <see cref="T:System.Xml.IXmlNamespaceResolver" /> interface to use for namespace resolution.</param>
		/// <exception cref="T:System.Xml.XPath.XPathException">The <see cref="T:System.Xml.IXmlNamespaceResolver" /> object parameter is not derived from <see cref="T:System.Xml.IXmlNamespaceResolver" />. </exception>
		public abstract void SetContext(IXmlNamespaceResolver nsResolver);
	}
}
