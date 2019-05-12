using System;

namespace System.Xml.XPath
{
	internal class PredicateIterator : BaseIterator
	{
		private BaseIterator _iter;

		private Expression _pred;

		private XPathResultType resType;

		private bool finished;

		public PredicateIterator(BaseIterator iter, Expression pred) : base(iter.NamespaceManager)
		{
			this._iter = iter;
			this._pred = pred;
			this.resType = pred.GetReturnType(iter);
		}

		private PredicateIterator(PredicateIterator other) : base(other)
		{
			this._iter = (BaseIterator)other._iter.Clone();
			this._pred = other._pred;
			this.resType = other.resType;
			this.finished = other.finished;
		}

		public override XPathNodeIterator Clone()
		{
			return new PredicateIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (this.finished)
			{
				return false;
			}
			while (this._iter.MoveNext())
			{
				XPathResultType xpathResultType = this.resType;
				if (xpathResultType != XPathResultType.Number)
				{
					if (xpathResultType != XPathResultType.Any)
					{
						if (!this._pred.EvaluateBoolean(this._iter))
						{
							continue;
						}
					}
					else
					{
						object obj = this._pred.Evaluate(this._iter);
						if (obj is double)
						{
							if ((double)obj != (double)this._iter.ComparablePosition)
							{
								continue;
							}
						}
						else if (!XPathFunctions.ToBoolean(obj))
						{
							continue;
						}
					}
				}
				else if (this._pred.EvaluateNumber(this._iter) != (double)this._iter.ComparablePosition)
				{
					continue;
				}
				return true;
			}
			this.finished = true;
			return false;
		}

		public override XPathNavigator Current
		{
			get
			{
				return (this.CurrentPosition != 0) ? this._iter.Current : null;
			}
		}

		public override bool ReverseAxis
		{
			get
			{
				return this._iter.ReverseAxis;
			}
		}

		public override string ToString()
		{
			return this._iter.GetType().FullName;
		}
	}
}
