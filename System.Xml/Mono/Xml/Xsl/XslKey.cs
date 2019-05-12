using Mono.Xml.XPath;
using System;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XslKey
	{
		private XmlQualifiedName name;

		private CompiledExpression useExpr;

		private Pattern matchPattern;

		public XslKey(Compiler c)
		{
			this.name = c.ParseQNameAttribute("name");
			c.KeyCompilationMode = true;
			this.useExpr = c.CompileExpression(c.GetAttribute("use"));
			if (this.useExpr == null)
			{
				this.useExpr = c.CompileExpression(".");
			}
			c.AssertAttribute("match");
			string attribute = c.GetAttribute("match");
			this.matchPattern = c.CompilePattern(attribute, c.Input);
			c.KeyCompilationMode = false;
		}

		public XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		internal CompiledExpression Use
		{
			get
			{
				return this.useExpr;
			}
		}

		internal Pattern Match
		{
			get
			{
				return this.matchPattern;
			}
		}
	}
}
