using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Xml;

namespace Mono.Xml.Xsl
{
	internal class VariableScope
	{
		private ArrayList variableNames;

		private Hashtable variables;

		private VariableScope parent;

		private int nextSlot;

		private int highTide;

		public VariableScope(VariableScope parent)
		{
			this.parent = parent;
			if (parent != null)
			{
				this.nextSlot = parent.nextSlot;
			}
		}

		internal void giveHighTideToParent()
		{
			if (this.parent != null)
			{
				this.parent.highTide = Math.Max(this.VariableHighTide, this.parent.VariableHighTide);
			}
		}

		public int VariableHighTide
		{
			get
			{
				return Math.Max(this.highTide, this.nextSlot);
			}
		}

		public VariableScope Parent
		{
			get
			{
				return this.parent;
			}
		}

		public int AddVariable(XslLocalVariable v)
		{
			if (this.variables == null)
			{
				this.variableNames = new ArrayList();
				this.variables = new Hashtable();
			}
			this.variables[v.Name] = v;
			int num = this.variableNames.IndexOf(v.Name);
			if (num >= 0)
			{
				return num;
			}
			this.variableNames.Add(v.Name);
			return this.nextSlot++;
		}

		public XslLocalVariable ResolveStatic(XmlQualifiedName name)
		{
			for (VariableScope variableScope = this; variableScope != null; variableScope = variableScope.Parent)
			{
				if (variableScope.variables != null)
				{
					XslLocalVariable xslLocalVariable = variableScope.variables[name] as XslLocalVariable;
					if (xslLocalVariable != null)
					{
						return xslLocalVariable;
					}
				}
			}
			return null;
		}

		public XslLocalVariable Resolve(XslTransformProcessor p, XmlQualifiedName name)
		{
			for (VariableScope variableScope = this; variableScope != null; variableScope = variableScope.Parent)
			{
				if (variableScope.variables != null)
				{
					XslLocalVariable xslLocalVariable = variableScope.variables[name] as XslLocalVariable;
					if (xslLocalVariable != null && xslLocalVariable.IsEvaluated(p))
					{
						return xslLocalVariable;
					}
				}
			}
			return null;
		}
	}
}
