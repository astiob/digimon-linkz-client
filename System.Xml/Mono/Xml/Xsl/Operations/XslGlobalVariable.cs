using System;
using System.Collections;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslGlobalVariable : XslGeneralVariable
	{
		private static object busyObject = new object();

		public XslGlobalVariable(Compiler c) : base(c)
		{
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			Hashtable globalVariableTable = p.globalVariableTable;
			if (!globalVariableTable.Contains(this))
			{
				globalVariableTable[this] = XslGlobalVariable.busyObject;
				globalVariableTable[this] = this.var.Evaluate(p);
				return;
			}
			if (globalVariableTable[this] == XslGlobalVariable.busyObject)
			{
				throw new XsltException("Circular dependency was detected", null, p.CurrentNode);
			}
		}

		protected override object GetValue(XslTransformProcessor p)
		{
			this.Evaluate(p);
			return p.globalVariableTable[this];
		}

		public override bool IsLocal
		{
			get
			{
				return false;
			}
		}

		public override bool IsParam
		{
			get
			{
				return false;
			}
		}
	}
}
