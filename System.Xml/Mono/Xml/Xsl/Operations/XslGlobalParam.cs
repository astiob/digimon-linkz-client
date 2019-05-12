using System;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslGlobalParam : XslGlobalVariable
	{
		public XslGlobalParam(Compiler c) : base(c)
		{
		}

		public void Override(XslTransformProcessor p, object paramVal)
		{
			p.globalVariableTable[this] = paramVal;
		}

		public override bool IsParam
		{
			get
			{
				return true;
			}
		}
	}
}
