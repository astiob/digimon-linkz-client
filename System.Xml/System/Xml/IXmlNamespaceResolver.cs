using System;
using System.Collections.Generic;

namespace System.Xml
{
	/// <summary>Provides read-only access to a set of prefix and namespace mappings.</summary>
	public interface IXmlNamespaceResolver
	{
		/// <summary>Gets a collection of defined prefix-namespace mappings that are currently in scope.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionary" /> that contains the current in-scope namespaces.</returns>
		/// <param name="scope">An <see cref="T:System.Xml.XmlNamespaceScope" /> value that specifies the type of namespace nodes to return.</param>
		IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope);

		/// <summary>Gets the namespace URI mapped to the specified prefix.</summary>
		/// <returns>The namespace URI that is mapped to the prefix; null if the prefix is not mapped to a namespace URI.</returns>
		/// <param name="prefix">The prefix whose namespace URI you wish to find.</param>
		string LookupNamespace(string prefix);

		/// <summary>Gets the prefix that is mapped to the specified namespace URI.</summary>
		/// <returns>The prefix that is mapped to the namespace URI; null if the namespace URI is not mapped to a prefix.</returns>
		/// <param name="namespaceName">The namespace URI whose prefix you wish to find.</param>
		string LookupPrefix(string ns);
	}
}
