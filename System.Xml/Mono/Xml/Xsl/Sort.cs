using Mono.Xml.Xsl.Operations;
using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class Sort
	{
		private string lang;

		private XmlDataType dataType;

		private XmlSortOrder order;

		private XmlCaseOrder caseOrder;

		private XslAvt langAvt;

		private XslAvt dataTypeAvt;

		private XslAvt orderAvt;

		private XslAvt caseOrderAvt;

		private XPathExpression expr;

		public Sort(Compiler c)
		{
			c.CheckExtraAttributes("sort", new string[]
			{
				"select",
				"lang",
				"data-type",
				"order",
				"case-order"
			});
			this.expr = c.CompileExpression(c.GetAttribute("select"));
			if (this.expr == null)
			{
				this.expr = c.CompileExpression("string(.)");
			}
			this.langAvt = c.ParseAvtAttribute("lang");
			this.dataTypeAvt = c.ParseAvtAttribute("data-type");
			this.orderAvt = c.ParseAvtAttribute("order");
			this.caseOrderAvt = c.ParseAvtAttribute("case-order");
			this.lang = this.ParseLang(XslAvt.AttemptPreCalc(ref this.langAvt));
			this.dataType = this.ParseDataType(XslAvt.AttemptPreCalc(ref this.dataTypeAvt));
			this.order = this.ParseOrder(XslAvt.AttemptPreCalc(ref this.orderAvt));
			this.caseOrder = this.ParseCaseOrder(XslAvt.AttemptPreCalc(ref this.caseOrderAvt));
		}

		public bool IsContextDependent
		{
			get
			{
				return this.orderAvt != null || this.caseOrderAvt != null || this.langAvt != null || this.dataTypeAvt != null;
			}
		}

		private string ParseLang(string value)
		{
			return value;
		}

		private XmlDataType ParseDataType(string value)
		{
			if (value != null)
			{
				if (Sort.<>f__switch$map10 == null)
				{
					Sort.<>f__switch$map10 = new Dictionary<string, int>(2)
					{
						{
							"number",
							0
						},
						{
							"text",
							1
						}
					};
				}
				int num;
				if (Sort.<>f__switch$map10.TryGetValue(value, out num))
				{
					if (num == 0)
					{
						return XmlDataType.Number;
					}
					if (num != 1)
					{
					}
				}
			}
			return XmlDataType.Text;
		}

		private XmlSortOrder ParseOrder(string value)
		{
			if (value != null)
			{
				if (Sort.<>f__switch$map11 == null)
				{
					Sort.<>f__switch$map11 = new Dictionary<string, int>(2)
					{
						{
							"descending",
							0
						},
						{
							"ascending",
							1
						}
					};
				}
				int num;
				if (Sort.<>f__switch$map11.TryGetValue(value, out num))
				{
					if (num == 0)
					{
						return XmlSortOrder.Descending;
					}
					if (num != 1)
					{
					}
				}
			}
			return XmlSortOrder.Ascending;
		}

		private XmlCaseOrder ParseCaseOrder(string value)
		{
			if (value != null)
			{
				if (Sort.<>f__switch$map12 == null)
				{
					Sort.<>f__switch$map12 = new Dictionary<string, int>(2)
					{
						{
							"upper-first",
							0
						},
						{
							"lower-first",
							1
						}
					};
				}
				int num;
				if (Sort.<>f__switch$map12.TryGetValue(value, out num))
				{
					if (num == 0)
					{
						return XmlCaseOrder.UpperFirst;
					}
					if (num == 1)
					{
						return XmlCaseOrder.LowerFirst;
					}
				}
			}
			return XmlCaseOrder.None;
		}

		public void AddToExpr(XPathExpression e, XslTransformProcessor p)
		{
			e.AddSort(this.expr, (this.orderAvt != null) ? this.ParseOrder(this.orderAvt.Evaluate(p)) : this.order, (this.caseOrderAvt != null) ? this.ParseCaseOrder(this.caseOrderAvt.Evaluate(p)) : this.caseOrder, (this.langAvt != null) ? this.ParseLang(this.langAvt.Evaluate(p)) : this.lang, (this.dataTypeAvt != null) ? this.ParseDataType(this.dataTypeAvt.Evaluate(p)) : this.dataType);
		}

		public XPathSorter ToXPathSorter(XslTransformProcessor p)
		{
			return new XPathSorter(this.expr, (this.orderAvt != null) ? this.ParseOrder(this.orderAvt.Evaluate(p)) : this.order, (this.caseOrderAvt != null) ? this.ParseCaseOrder(this.caseOrderAvt.Evaluate(p)) : this.caseOrder, (this.langAvt != null) ? this.ParseLang(this.langAvt.Evaluate(p)) : this.lang, (this.dataTypeAvt != null) ? this.ParseDataType(this.dataTypeAvt.Evaluate(p)) : this.dataType);
		}
	}
}
