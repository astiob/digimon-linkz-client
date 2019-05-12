using System;

namespace System.Xml.XPath
{
	internal class AxisSpecifier
	{
		protected Axes _axis;

		public AxisSpecifier(Axes axis)
		{
			this._axis = axis;
		}

		public XPathNodeType NodeType
		{
			get
			{
				Axes axis = this._axis;
				if (axis == Axes.Attribute)
				{
					return XPathNodeType.Attribute;
				}
				if (axis != Axes.Namespace)
				{
					return XPathNodeType.Element;
				}
				return XPathNodeType.Namespace;
			}
		}

		public override string ToString()
		{
			switch (this._axis)
			{
			case Axes.Ancestor:
				return "ancestor";
			case Axes.AncestorOrSelf:
				return "ancestor-or-self";
			case Axes.Attribute:
				return "attribute";
			case Axes.Child:
				return "child";
			case Axes.Descendant:
				return "descendant";
			case Axes.DescendantOrSelf:
				return "descendant-or-self";
			case Axes.Following:
				return "following";
			case Axes.FollowingSibling:
				return "following-sibling";
			case Axes.Namespace:
				return "namespace";
			case Axes.Parent:
				return "parent";
			case Axes.Preceding:
				return "preceding";
			case Axes.PrecedingSibling:
				return "preceding-sibling";
			case Axes.Self:
				return "self";
			default:
				throw new IndexOutOfRangeException();
			}
		}

		public Axes Axis
		{
			get
			{
				return this._axis;
			}
		}

		public BaseIterator Evaluate(BaseIterator iter)
		{
			switch (this._axis)
			{
			case Axes.Ancestor:
				return new AncestorIterator(iter);
			case Axes.AncestorOrSelf:
				return new AncestorOrSelfIterator(iter);
			case Axes.Attribute:
				return new AttributeIterator(iter);
			case Axes.Child:
				return new ChildIterator(iter);
			case Axes.Descendant:
				return new DescendantIterator(iter);
			case Axes.DescendantOrSelf:
				return new DescendantOrSelfIterator(iter);
			case Axes.Following:
				return new FollowingIterator(iter);
			case Axes.FollowingSibling:
				return new FollowingSiblingIterator(iter);
			case Axes.Namespace:
				return new NamespaceIterator(iter);
			case Axes.Parent:
				return new ParentIterator(iter);
			case Axes.Preceding:
				return new PrecedingIterator(iter);
			case Axes.PrecedingSibling:
				return new PrecedingSiblingIterator(iter);
			case Axes.Self:
				return new SelfIterator(iter);
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}
}
