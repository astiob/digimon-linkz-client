using System;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	internal class SimpleXsltDebugger
	{
		public void OnCompile(XPathNavigator style)
		{
			Console.Write("Compiling: ");
			this.PrintXPathNavigator(style);
			Console.WriteLine();
		}

		public void OnExecute(XPathNodeIterator currentNodeSet, XPathNavigator style, XsltContext xsltContext)
		{
			Console.Write("Executing: ");
			this.PrintXPathNavigator(style);
			Console.WriteLine(" / NodeSet: (type {1}) {0} / XsltContext: {2}", currentNodeSet, currentNodeSet.GetType(), xsltContext);
		}

		private void PrintXPathNavigator(XPathNavigator nav)
		{
			IXmlLineInfo xmlLineInfo = nav as IXmlLineInfo;
			IXmlLineInfo xmlLineInfo3;
			if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
			{
				IXmlLineInfo xmlLineInfo2 = xmlLineInfo;
				xmlLineInfo3 = xmlLineInfo2;
			}
			else
			{
				xmlLineInfo3 = null;
			}
			xmlLineInfo = xmlLineInfo3;
			Console.Write("({0}, {1}) element {2}", (xmlLineInfo == null) ? 0 : xmlLineInfo.LineNumber, (xmlLineInfo == null) ? 0 : xmlLineInfo.LinePosition, nav.Name);
		}
	}
}
