using System;

namespace System.Xml
{
	/// <summary>Defines the namespace scope.</summary>
	public enum XmlNamespaceScope
	{
		/// <summary>All namespaces defined in the scope of the current node. This includes the xmlns:xml namespace which is always declared implicitly. The order of the namespaces returned is not defined.</summary>
		All,
		/// <summary>All namespaces defined in the scope of the current node, excluding the xmlns:xml namespace, which is always declared implicitly. The order of the namespaces returned is not defined.</summary>
		ExcludeXml,
		/// <summary>All namespaces that are defined locally at the current node.</summary>
		Local
	}
}
