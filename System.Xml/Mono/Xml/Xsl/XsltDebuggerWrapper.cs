using System;
using System.Reflection;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XsltDebuggerWrapper
	{
		private readonly MethodInfo on_compile;

		private readonly MethodInfo on_execute;

		private readonly object impl;

		public XsltDebuggerWrapper(object impl)
		{
			this.impl = impl;
			this.on_compile = impl.GetType().GetMethod("OnCompile", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (this.on_compile == null)
			{
				throw new InvalidOperationException("INTERNAL ERROR: the debugger does not look like what System.Xml.dll expects. OnCompile method was not found");
			}
			this.on_execute = impl.GetType().GetMethod("OnExecute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (this.on_execute == null)
			{
				throw new InvalidOperationException("INTERNAL ERROR: the debugger does not look like what System.Xml.dll expects. OnExecute method was not found");
			}
		}

		public void DebugCompile(XPathNavigator style)
		{
			this.on_compile.Invoke(this.impl, new object[]
			{
				style.Clone()
			});
		}

		public void DebugExecute(XslTransformProcessor p, XPathNavigator style)
		{
			this.on_execute.Invoke(this.impl, new object[]
			{
				p.CurrentNodeset.Clone(),
				style.Clone(),
				p.XPathContext
			});
		}
	}
}
