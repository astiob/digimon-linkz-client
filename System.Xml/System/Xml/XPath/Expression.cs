using System;
using System.Globalization;

namespace System.Xml.XPath
{
	internal abstract class Expression
	{
		public Expression()
		{
		}

		public abstract XPathResultType ReturnType { get; }

		public virtual XPathResultType GetReturnType(BaseIterator iter)
		{
			return this.ReturnType;
		}

		public virtual Expression Optimize()
		{
			return this;
		}

		public virtual bool HasStaticValue
		{
			get
			{
				return false;
			}
		}

		public virtual object StaticValue
		{
			get
			{
				switch (this.ReturnType)
				{
				case XPathResultType.Number:
					return this.StaticValueAsNumber;
				case XPathResultType.String:
					return this.StaticValueAsString;
				case XPathResultType.Boolean:
					return this.StaticValueAsBoolean;
				default:
					return null;
				}
			}
		}

		public virtual string StaticValueAsString
		{
			get
			{
				return (!this.HasStaticValue) ? null : XPathFunctions.ToString(this.StaticValue);
			}
		}

		public virtual double StaticValueAsNumber
		{
			get
			{
				return (!this.HasStaticValue) ? 0.0 : XPathFunctions.ToNumber(this.StaticValue);
			}
		}

		public virtual bool StaticValueAsBoolean
		{
			get
			{
				return this.HasStaticValue && XPathFunctions.ToBoolean(this.StaticValue);
			}
		}

		public virtual XPathNavigator StaticValueAsNavigator
		{
			get
			{
				return this.StaticValue as XPathNavigator;
			}
		}

		public abstract object Evaluate(BaseIterator iter);

		public virtual BaseIterator EvaluateNodeSet(BaseIterator iter)
		{
			XPathResultType returnType = this.GetReturnType(iter);
			switch (returnType)
			{
			case XPathResultType.NodeSet:
			case XPathResultType.Navigator:
			case XPathResultType.Any:
			{
				object obj = this.Evaluate(iter);
				XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
				BaseIterator baseIterator = null;
				if (xpathNodeIterator != null)
				{
					baseIterator = (xpathNodeIterator as BaseIterator);
					if (baseIterator == null)
					{
						baseIterator = new WrapperIterator(xpathNodeIterator, iter.NamespaceManager);
					}
					return baseIterator;
				}
				XPathNavigator xpathNavigator = obj as XPathNavigator;
				if (xpathNavigator != null)
				{
					XPathNodeIterator xpathNodeIterator2 = xpathNavigator.SelectChildren(XPathNodeType.All);
					baseIterator = (xpathNodeIterator2 as BaseIterator);
					if (baseIterator == null && xpathNodeIterator2 != null)
					{
						baseIterator = new WrapperIterator(xpathNodeIterator2, iter.NamespaceManager);
					}
				}
				if (baseIterator != null)
				{
					return baseIterator;
				}
				if (obj == null)
				{
					return new NullIterator(iter);
				}
				returnType = Expression.GetReturnType(obj);
				break;
			}
			}
			throw new XPathException(string.Format("expected nodeset but was {1}: {0}", this.ToString(), returnType));
		}

		protected static XPathResultType GetReturnType(object obj)
		{
			if (obj is string)
			{
				return XPathResultType.String;
			}
			if (obj is bool)
			{
				return XPathResultType.Boolean;
			}
			if (obj is XPathNodeIterator)
			{
				return XPathResultType.NodeSet;
			}
			if (obj is double || obj is int)
			{
				return XPathResultType.Number;
			}
			if (obj is XPathNavigator)
			{
				return XPathResultType.Navigator;
			}
			throw new XPathException("invalid node type: " + obj.GetType().ToString());
		}

		internal virtual XPathNodeType EvaluatedNodeType
		{
			get
			{
				return XPathNodeType.All;
			}
		}

		internal virtual bool IsPositional
		{
			get
			{
				return false;
			}
		}

		internal virtual bool Peer
		{
			get
			{
				return false;
			}
		}

		public virtual double EvaluateNumber(BaseIterator iter)
		{
			XPathResultType xpathResultType = this.GetReturnType(iter);
			object obj;
			if (xpathResultType == XPathResultType.NodeSet)
			{
				obj = this.EvaluateString(iter);
				xpathResultType = XPathResultType.String;
			}
			else
			{
				obj = this.Evaluate(iter);
			}
			if (xpathResultType == XPathResultType.Any)
			{
				xpathResultType = Expression.GetReturnType(obj);
			}
			switch (xpathResultType)
			{
			case XPathResultType.Number:
				if (obj is double)
				{
					return (double)obj;
				}
				if (obj is IConvertible)
				{
					return ((IConvertible)obj).ToDouble(CultureInfo.InvariantCulture);
				}
				return (double)obj;
			case XPathResultType.String:
				return XPathFunctions.ToNumber((string)obj);
			case XPathResultType.Boolean:
				return (!(bool)obj) ? 0.0 : 1.0;
			case XPathResultType.NodeSet:
				return XPathFunctions.ToNumber(this.EvaluateString(iter));
			case XPathResultType.Navigator:
				return XPathFunctions.ToNumber(((XPathNavigator)obj).Value);
			default:
				throw new XPathException("invalid node type");
			}
		}

		public virtual string EvaluateString(BaseIterator iter)
		{
			object obj = this.Evaluate(iter);
			XPathResultType returnType = this.GetReturnType(iter);
			if (returnType == XPathResultType.Any)
			{
				returnType = Expression.GetReturnType(obj);
			}
			switch (returnType)
			{
			case XPathResultType.Number:
			{
				double d = (double)obj;
				return XPathFunctions.ToString(d);
			}
			case XPathResultType.String:
				return (string)obj;
			case XPathResultType.Boolean:
				return (!(bool)obj) ? "false" : "true";
			case XPathResultType.NodeSet:
			{
				BaseIterator baseIterator = (BaseIterator)obj;
				if (baseIterator == null || !baseIterator.MoveNext())
				{
					return string.Empty;
				}
				return baseIterator.Current.Value;
			}
			case XPathResultType.Navigator:
				return ((XPathNavigator)obj).Value;
			default:
				throw new XPathException("invalid node type");
			}
		}

		public virtual bool EvaluateBoolean(BaseIterator iter)
		{
			object obj = this.Evaluate(iter);
			XPathResultType returnType = this.GetReturnType(iter);
			if (returnType == XPathResultType.Any)
			{
				returnType = Expression.GetReturnType(obj);
			}
			switch (returnType)
			{
			case XPathResultType.Number:
			{
				double num = Convert.ToDouble(obj);
				return num != 0.0 && num != -0.0 && !double.IsNaN(num);
			}
			case XPathResultType.String:
				return ((string)obj).Length != 0;
			case XPathResultType.Boolean:
				return (bool)obj;
			case XPathResultType.NodeSet:
			{
				BaseIterator baseIterator = (BaseIterator)obj;
				return baseIterator != null && baseIterator.MoveNext();
			}
			case XPathResultType.Navigator:
				return ((XPathNavigator)obj).HasChildren;
			default:
				throw new XPathException("invalid node type");
			}
		}

		public object EvaluateAs(BaseIterator iter, XPathResultType type)
		{
			switch (type)
			{
			case XPathResultType.Number:
				return this.EvaluateNumber(iter);
			case XPathResultType.String:
				return this.EvaluateString(iter);
			case XPathResultType.Boolean:
				return this.EvaluateBoolean(iter);
			case XPathResultType.NodeSet:
				return this.EvaluateNodeSet(iter);
			default:
				return this.Evaluate(iter);
			}
		}

		public virtual bool RequireSorting
		{
			get
			{
				return false;
			}
		}
	}
}
