using System;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	internal interface IStaticXsltContext
	{
		Expression TryGetVariable(string nm);

		Expression TryGetFunction(XmlQualifiedName nm, FunctionArguments args);

		XmlQualifiedName LookupQName(string s);

		string LookupNamespace(string prefix);
	}
}
