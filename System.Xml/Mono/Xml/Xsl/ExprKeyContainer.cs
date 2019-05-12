using System;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class ExprKeyContainer : Expression
	{
		private Expression expr;

		public ExprKeyContainer(Expression expr)
		{
			this.expr = expr;
		}

		public Expression BodyExpression
		{
			get
			{
				return this.expr;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			return this.expr.Evaluate(iter);
		}

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return this.expr.EvaluatedNodeType;
			}
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return this.expr.ReturnType;
			}
		}
	}
}
