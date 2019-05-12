using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Globalization;

namespace System.Xml.XPath
{
	internal class XPathSorter
	{
		private readonly Expression _expr;

		private readonly IComparer _cmp;

		private readonly XmlDataType _type;

		public XPathSorter(object expr, IComparer cmp)
		{
			this._expr = XPathSorter.ExpressionFromObject(expr);
			this._cmp = cmp;
			this._type = XmlDataType.Text;
		}

		public XPathSorter(object expr, XmlSortOrder orderSort, XmlCaseOrder orderCase, string lang, XmlDataType dataType)
		{
			this._expr = XPathSorter.ExpressionFromObject(expr);
			this._type = dataType;
			if (dataType == XmlDataType.Number)
			{
				this._cmp = new XPathSorter.XPathNumberComparer(orderSort);
			}
			else
			{
				this._cmp = new XPathSorter.XPathTextComparer(orderSort, orderCase, lang);
			}
		}

		private static Expression ExpressionFromObject(object expr)
		{
			if (expr is CompiledExpression)
			{
				return ((CompiledExpression)expr).ExpressionNode;
			}
			if (expr is string)
			{
				return new XPathParser().Compile((string)expr);
			}
			throw new XPathException("Invalid query object");
		}

		public object Evaluate(BaseIterator iter)
		{
			if (this._type == XmlDataType.Number)
			{
				return this._expr.EvaluateNumber(iter);
			}
			return this._expr.EvaluateString(iter);
		}

		public int Compare(object o1, object o2)
		{
			return this._cmp.Compare(o1, o2);
		}

		private class XPathNumberComparer : IComparer
		{
			private int _nMulSort;

			public XPathNumberComparer(XmlSortOrder orderSort)
			{
				this._nMulSort = ((orderSort != XmlSortOrder.Ascending) ? -1 : 1);
			}

			int IComparer.Compare(object o1, object o2)
			{
				double num = (double)o1;
				double num2 = (double)o2;
				if (num < num2)
				{
					return -this._nMulSort;
				}
				if (num > num2)
				{
					return this._nMulSort;
				}
				if (num == num2)
				{
					return 0;
				}
				if (double.IsNaN(num))
				{
					return (!double.IsNaN(num2)) ? (-this._nMulSort) : 0;
				}
				return this._nMulSort;
			}
		}

		private class XPathTextComparer : IComparer
		{
			private int _nMulSort;

			private int _nMulCase;

			private XmlCaseOrder _orderCase;

			private CultureInfo _ci;

			public XPathTextComparer(XmlSortOrder orderSort, XmlCaseOrder orderCase, string strLang)
			{
				this._orderCase = orderCase;
				this._nMulCase = ((orderCase != XmlCaseOrder.UpperFirst) ? 1 : -1);
				this._nMulSort = ((orderSort != XmlSortOrder.Ascending) ? -1 : 1);
				if (strLang == null || strLang == string.Empty)
				{
					this._ci = CultureInfo.CurrentCulture;
				}
				else
				{
					this._ci = new CultureInfo(strLang);
				}
			}

			int IComparer.Compare(object o1, object o2)
			{
				string strA = (string)o1;
				string strB = (string)o2;
				int num = string.Compare(strA, strB, true, this._ci);
				if (num != 0 || this._orderCase == XmlCaseOrder.None)
				{
					return num * this._nMulSort;
				}
				return this._nMulSort * this._nMulCase * string.Compare(strA, strB, false, this._ci);
			}
		}
	}
}
