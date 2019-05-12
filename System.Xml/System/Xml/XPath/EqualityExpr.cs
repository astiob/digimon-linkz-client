using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal abstract class EqualityExpr : ExprBoolean
	{
		private bool trueVal;

		public EqualityExpr(Expression left, Expression right, bool trueVal) : base(left, right)
		{
			this.trueVal = trueVal;
		}

		public override bool StaticValueAsBoolean
		{
			get
			{
				if (!this.HasStaticValue)
				{
					return false;
				}
				if ((this._left.ReturnType == XPathResultType.Navigator || this._right.ReturnType == XPathResultType.Navigator) && this._left.ReturnType == this._right.ReturnType)
				{
					return this._left.StaticValueAsNavigator.IsSamePosition(this._right.StaticValueAsNavigator) == this.trueVal;
				}
				if (this._left.ReturnType == XPathResultType.Boolean | this._right.ReturnType == XPathResultType.Boolean)
				{
					return this._left.StaticValueAsBoolean == this._right.StaticValueAsBoolean == this.trueVal;
				}
				if (this._left.ReturnType == XPathResultType.Number | this._right.ReturnType == XPathResultType.Number)
				{
					return this._left.StaticValueAsNumber == this._right.StaticValueAsNumber == this.trueVal;
				}
				if (this._left.ReturnType == XPathResultType.String | this._right.ReturnType == XPathResultType.String)
				{
					return this._left.StaticValueAsString == this._right.StaticValueAsString == this.trueVal;
				}
				return this._left.StaticValue == this._right.StaticValue == this.trueVal;
			}
		}

		public override bool EvaluateBoolean(BaseIterator iter)
		{
			XPathResultType xpathResultType = this._left.GetReturnType(iter);
			XPathResultType xpathResultType2 = this._right.GetReturnType(iter);
			if (xpathResultType == XPathResultType.Any)
			{
				xpathResultType = Expression.GetReturnType(this._left.Evaluate(iter));
			}
			if (xpathResultType2 == XPathResultType.Any)
			{
				xpathResultType2 = Expression.GetReturnType(this._right.Evaluate(iter));
			}
			if (xpathResultType == XPathResultType.Navigator)
			{
				xpathResultType = XPathResultType.String;
			}
			if (xpathResultType2 == XPathResultType.Navigator)
			{
				xpathResultType2 = XPathResultType.String;
			}
			if (xpathResultType == XPathResultType.NodeSet || xpathResultType2 == XPathResultType.NodeSet)
			{
				Expression expression;
				Expression expression2;
				if (xpathResultType != XPathResultType.NodeSet)
				{
					expression = this._right;
					expression2 = this._left;
					XPathResultType xpathResultType3 = xpathResultType;
					xpathResultType2 = xpathResultType3;
				}
				else
				{
					expression = this._left;
					expression2 = this._right;
				}
				if (xpathResultType2 == XPathResultType.Boolean)
				{
					return expression.EvaluateBoolean(iter) == expression2.EvaluateBoolean(iter) == this.trueVal;
				}
				BaseIterator baseIterator = expression.EvaluateNodeSet(iter);
				if (xpathResultType2 == XPathResultType.Number)
				{
					double num = expression2.EvaluateNumber(iter);
					while (baseIterator.MoveNext())
					{
						if (XPathFunctions.ToNumber(baseIterator.Current.Value) == num == this.trueVal)
						{
							return true;
						}
					}
				}
				else if (xpathResultType2 == XPathResultType.String)
				{
					string b = expression2.EvaluateString(iter);
					while (baseIterator.MoveNext())
					{
						if (baseIterator.Current.Value == b == this.trueVal)
						{
							return true;
						}
					}
				}
				else if (xpathResultType2 == XPathResultType.NodeSet)
				{
					BaseIterator baseIterator2 = expression2.EvaluateNodeSet(iter);
					ArrayList arrayList = new ArrayList();
					while (baseIterator.MoveNext())
					{
						XPathNavigator xpathNavigator = baseIterator.Current;
						arrayList.Add(XPathFunctions.ToString(xpathNavigator.Value));
					}
					while (baseIterator2.MoveNext())
					{
						XPathNavigator xpathNavigator2 = baseIterator2.Current;
						string a = XPathFunctions.ToString(xpathNavigator2.Value);
						for (int i = 0; i < arrayList.Count; i++)
						{
							if (a == (string)arrayList[i] == this.trueVal)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			else
			{
				if (xpathResultType == XPathResultType.Boolean || xpathResultType2 == XPathResultType.Boolean)
				{
					return this._left.EvaluateBoolean(iter) == this._right.EvaluateBoolean(iter) == this.trueVal;
				}
				if (xpathResultType == XPathResultType.Number || xpathResultType2 == XPathResultType.Number)
				{
					return this._left.EvaluateNumber(iter) == this._right.EvaluateNumber(iter) == this.trueVal;
				}
				return this._left.EvaluateString(iter) == this._right.EvaluateString(iter) == this.trueVal;
			}
		}
	}
}
