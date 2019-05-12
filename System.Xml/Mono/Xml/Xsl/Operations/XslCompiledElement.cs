using System;

namespace Mono.Xml.Xsl.Operations
{
	internal abstract class XslCompiledElement : XslCompiledElementBase
	{
		public XslCompiledElement(Compiler c) : base(c)
		{
			this.Compile(c);
		}
	}
}
