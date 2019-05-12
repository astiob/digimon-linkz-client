using System;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
	internal abstract class XslCompiledElementBase : XslOperation
	{
		private int lineNumber;

		private int linePosition;

		private XPathNavigator debugInput;

		public XslCompiledElementBase(Compiler c)
		{
			IXmlLineInfo xmlLineInfo = c.Input as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				this.lineNumber = xmlLineInfo.LineNumber;
				this.linePosition = xmlLineInfo.LinePosition;
			}
			if (c.Debugger != null)
			{
				this.debugInput = c.Input.Clone();
			}
		}

		public XPathNavigator DebugInput
		{
			get
			{
				return this.debugInput;
			}
		}

		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		protected abstract void Compile(Compiler c);
	}
}
