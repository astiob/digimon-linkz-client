using Mono.Xml.XPath;
using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class XslTemplate
	{
		private XmlQualifiedName name;

		private Pattern match;

		private XmlQualifiedName mode;

		private double priority = double.NaN;

		private ArrayList parameters;

		private XslOperation content;

		private static int nextId;

		public readonly int Id = XslTemplate.nextId++;

		private XslStylesheet style;

		private int stackSize;

		public XslTemplate(Compiler c)
		{
			if (c == null)
			{
				return;
			}
			this.style = c.CurrentStylesheet;
			c.PushScope();
			if (c.Input.Name == "template" && c.Input.NamespaceURI == "http://www.w3.org/1999/XSL/Transform" && c.Input.MoveToAttribute("mode", string.Empty))
			{
				c.Input.MoveToParent();
				if (!c.Input.MoveToAttribute("match", string.Empty))
				{
					throw new XsltCompileException("XSLT 'template' element must not have 'mode' attribute when it does not have 'match' attribute", null, c.Input);
				}
				c.Input.MoveToParent();
			}
			if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
			{
				this.name = XmlQualifiedName.Empty;
				this.match = c.CompilePattern("/", c.Input);
				this.mode = XmlQualifiedName.Empty;
			}
			else
			{
				this.name = c.ParseQNameAttribute("name");
				this.match = c.CompilePattern(c.GetAttribute("match"), c.Input);
				this.mode = c.ParseQNameAttribute("mode");
				string attribute = c.GetAttribute("priority");
				if (attribute != null)
				{
					try
					{
						this.priority = double.Parse(attribute, CultureInfo.InvariantCulture);
					}
					catch (FormatException innerException)
					{
						throw new XsltException("Invalid priority number format", innerException, c.Input);
					}
				}
			}
			this.Parse(c);
			this.stackSize = c.PopScope().VariableHighTide;
		}

		public XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		public Pattern Match
		{
			get
			{
				return this.match;
			}
		}

		public XmlQualifiedName Mode
		{
			get
			{
				return this.mode;
			}
		}

		public double Priority
		{
			get
			{
				return this.priority;
			}
		}

		public XslStylesheet Parent
		{
			get
			{
				return this.style;
			}
		}

		private void Parse(Compiler c)
		{
			if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
			{
				this.content = c.CompileTemplateContent();
				return;
			}
			if (c.Input.MoveToFirstChild())
			{
				bool flag = true;
				XPathNavigator xpathNavigator = c.Input.Clone();
				bool flag2 = false;
				for (;;)
				{
					if (flag2)
					{
						flag2 = false;
						xpathNavigator.MoveTo(c.Input);
					}
					if (c.Input.NodeType == XPathNodeType.Text)
					{
						break;
					}
					if (c.Input.NodeType == XPathNodeType.Element)
					{
						if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
						{
							goto Block_6;
						}
						if (c.Input.LocalName != "param")
						{
							goto Block_7;
						}
						if (this.parameters == null)
						{
							this.parameters = new ArrayList();
						}
						this.parameters.Add(new XslLocalParam(c));
						flag2 = true;
					}
					if (!c.Input.MoveToNext())
					{
						goto IL_106;
					}
				}
				flag = false;
				goto IL_106;
				Block_6:
				flag = false;
				goto IL_106;
				Block_7:
				flag = false;
				IL_106:
				if (!flag)
				{
					c.Input.MoveTo(xpathNavigator);
					this.content = c.CompileTemplateContent();
				}
				c.Input.MoveToParent();
			}
		}

		private string LocationMessage
		{
			get
			{
				XslCompiledElementBase xslCompiledElementBase = (XslCompiledElementBase)this.content;
				return string.Format(" from\nxsl:template {0} at {1} ({2},{3})", new object[]
				{
					this.Match,
					this.style.BaseURI,
					xslCompiledElementBase.LineNumber,
					xslCompiledElementBase.LinePosition
				});
			}
		}

		private void AppendTemplateFrame(XsltException ex)
		{
			ex.AddTemplateFrame(this.LocationMessage);
		}

		public virtual void Evaluate(XslTransformProcessor p, Hashtable withParams)
		{
			if (XslTransform.TemplateStackFrameError)
			{
				try
				{
					this.EvaluateCore(p, withParams);
				}
				catch (XsltException ex)
				{
					this.AppendTemplateFrame(ex);
					throw ex;
				}
				catch (Exception)
				{
					XsltException ex2 = new XsltException("Error during XSLT processing: ", null, p.CurrentNode);
					this.AppendTemplateFrame(ex2);
					throw ex2;
				}
			}
			else
			{
				this.EvaluateCore(p, withParams);
			}
		}

		private void EvaluateCore(XslTransformProcessor p, Hashtable withParams)
		{
			if (XslTransform.TemplateStackFrameOutput != null)
			{
				XslTransform.TemplateStackFrameOutput.WriteLine(this.LocationMessage);
			}
			p.PushStack(this.stackSize);
			if (this.parameters != null)
			{
				if (withParams == null)
				{
					int count = this.parameters.Count;
					for (int i = 0; i < count; i++)
					{
						XslLocalParam xslLocalParam = (XslLocalParam)this.parameters[i];
						xslLocalParam.Evaluate(p);
					}
				}
				else
				{
					int count2 = this.parameters.Count;
					for (int j = 0; j < count2; j++)
					{
						XslLocalParam xslLocalParam2 = (XslLocalParam)this.parameters[j];
						object obj = withParams[xslLocalParam2.Name];
						if (obj != null)
						{
							xslLocalParam2.Override(p, obj);
						}
						else
						{
							xslLocalParam2.Evaluate(p);
						}
					}
				}
			}
			if (this.content != null)
			{
				this.content.Evaluate(p);
			}
			p.PopStack();
		}

		public void Evaluate(XslTransformProcessor p)
		{
			this.Evaluate(p, null);
		}
	}
}
