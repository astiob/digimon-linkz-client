using System;
using System.Collections;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
	internal class CompiledExpression : XPathExpression
	{
		protected IXmlNamespaceResolver _nsm;

		protected Expression _expr;

		private XPathSorters _sorters;

		private string rawExpression;

		public CompiledExpression(string raw, Expression expr)
		{
			this._expr = expr.Optimize();
			this.rawExpression = raw;
		}

		private CompiledExpression(CompiledExpression other)
		{
			this._nsm = other._nsm;
			this._expr = other._expr;
			this.rawExpression = other.rawExpression;
		}

		public override XPathExpression Clone()
		{
			return new CompiledExpression(this);
		}

		public Expression ExpressionNode
		{
			get
			{
				return this._expr;
			}
		}

		public override void SetContext(XmlNamespaceManager nsManager)
		{
			this._nsm = nsManager;
		}

		public override void SetContext(IXmlNamespaceResolver nsResolver)
		{
			this._nsm = nsResolver;
		}

		internal IXmlNamespaceResolver NamespaceManager
		{
			get
			{
				return this._nsm;
			}
		}

		public override string Expression
		{
			get
			{
				return this.rawExpression;
			}
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return this._expr.ReturnType;
			}
		}

		public object Evaluate(BaseIterator iter)
		{
			if (this._sorters != null)
			{
				return this.EvaluateNodeSet(iter);
			}
			object result;
			try
			{
				result = this._expr.Evaluate(iter);
			}
			catch (XPathException)
			{
				throw;
			}
			catch (XsltException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new XPathException("Error during evaluation", innerException);
			}
			return result;
		}

		public XPathNodeIterator EvaluateNodeSet(BaseIterator iter)
		{
			BaseIterator baseIterator = this._expr.EvaluateNodeSet(iter);
			if (this._sorters != null)
			{
				return this._sorters.Sort(baseIterator);
			}
			return baseIterator;
		}

		public double EvaluateNumber(BaseIterator iter)
		{
			return this._expr.EvaluateNumber(iter);
		}

		public string EvaluateString(BaseIterator iter)
		{
			return this._expr.EvaluateString(iter);
		}

		public bool EvaluateBoolean(BaseIterator iter)
		{
			return this._expr.EvaluateBoolean(iter);
		}

		public override void AddSort(object obj, IComparer cmp)
		{
			if (this._sorters == null)
			{
				this._sorters = new XPathSorters();
			}
			this._sorters.Add(obj, cmp);
		}

		public override void AddSort(object expr, XmlSortOrder orderSort, XmlCaseOrder orderCase, string lang, XmlDataType dataType)
		{
			if (this._sorters == null)
			{
				this._sorters = new XPathSorters();
			}
			this._sorters.Add(expr, orderSort, orderCase, lang, dataType);
		}
	}
}
