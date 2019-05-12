using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
	internal class XslMessage : XslCompiledElement
	{
		private static TextWriter output;

		private bool terminate;

		private XslOperation children;

		public XslMessage(Compiler c) : base(c)
		{
		}

		static XslMessage()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("MONO_XSLT_MESSAGE_OUTPUT");
			if (environmentVariable != null)
			{
				if (XslMessage.<>f__switch$mapA == null)
				{
					XslMessage.<>f__switch$mapA = new Dictionary<string, int>(2)
					{
						{
							"none",
							0
						},
						{
							"stderr",
							1
						}
					};
				}
				int num;
				if (XslMessage.<>f__switch$mapA.TryGetValue(environmentVariable, out num))
				{
					if (num == 0)
					{
						XslMessage.output = TextWriter.Null;
						return;
					}
					if (num == 1)
					{
						XslMessage.output = Console.Error;
						return;
					}
				}
			}
			XslMessage.output = Console.Out;
		}

		protected override void Compile(Compiler c)
		{
			if (c.Debugger != null)
			{
				c.Debugger.DebugCompile(base.DebugInput);
			}
			c.CheckExtraAttributes("message", new string[]
			{
				"terminate"
			});
			this.terminate = c.ParseYesNoAttribute("terminate", false);
			if (!c.Input.MoveToFirstChild())
			{
				return;
			}
			this.children = c.CompileTemplateContent();
			c.Input.MoveToParent();
		}

		public override void Evaluate(XslTransformProcessor p)
		{
			if (p.Debugger != null)
			{
				p.Debugger.DebugExecute(p, base.DebugInput);
			}
			if (this.children != null)
			{
				XslMessage.output.Write(this.children.EvaluateAsString(p));
			}
			if (this.terminate)
			{
				throw new XsltException("Transformation terminated.", null, p.CurrentNode);
			}
		}
	}
}
