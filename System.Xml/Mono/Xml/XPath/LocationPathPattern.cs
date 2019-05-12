using Mono.Xml.Xsl;
using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
	internal class LocationPathPattern : Pattern
	{
		private LocationPathPattern patternPrevious;

		private bool isAncestor;

		private NodeTest nodeTest;

		private ExprFilter filter;

		public LocationPathPattern(NodeTest nodeTest)
		{
			this.nodeTest = nodeTest;
		}

		public LocationPathPattern(ExprFilter filter)
		{
			this.filter = filter;
			while (!(filter.expr is NodeTest))
			{
				filter = (ExprFilter)filter.expr;
			}
			this.nodeTest = (NodeTest)filter.expr;
		}

		internal void SetPreviousPattern(Pattern prev, bool isAncestor)
		{
			LocationPathPattern lastPathPattern = this.LastPathPattern;
			lastPathPattern.patternPrevious = (LocationPathPattern)prev;
			lastPathPattern.isAncestor = isAncestor;
		}

		public override double DefaultPriority
		{
			get
			{
				if (this.patternPrevious != null || this.filter != null)
				{
					return 0.5;
				}
				NodeNameTest nodeNameTest = this.nodeTest as NodeNameTest;
				if (nodeNameTest == null)
				{
					return -0.5;
				}
				if (nodeNameTest.Name.Name == "*" || nodeNameTest.Name.Name.Length == 0)
				{
					return -0.25;
				}
				return 0.0;
			}
		}

		public override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return this.nodeTest.EvaluatedNodeType;
			}
		}

		public override bool Matches(XPathNavigator node, XsltContext ctx)
		{
			if (!this.nodeTest.Match(ctx, node))
			{
				return false;
			}
			if (this.nodeTest is NodeTypeTest && ((NodeTypeTest)this.nodeTest).type == XPathNodeType.All && (node.NodeType == XPathNodeType.Root || node.NodeType == XPathNodeType.Attribute))
			{
				return false;
			}
			if (this.filter == null && this.patternPrevious == null)
			{
				return true;
			}
			XPathNavigator navCache;
			if (this.patternPrevious != null)
			{
				navCache = ((XsltCompiledContext)ctx).GetNavCache(this, node);
				if (this.isAncestor)
				{
					while (navCache.MoveToParent())
					{
						if (this.patternPrevious.Matches(navCache, ctx))
						{
							goto IL_D9;
						}
					}
					return false;
				}
				navCache.MoveToParent();
				if (!this.patternPrevious.Matches(navCache, ctx))
				{
					return false;
				}
			}
			IL_D9:
			if (this.filter == null)
			{
				return true;
			}
			if (!this.filter.IsPositional && !(this.filter.expr is ExprFilter))
			{
				return this.filter.pred.EvaluateBoolean(new NullIterator(node, ctx));
			}
			navCache = ((XsltCompiledContext)ctx).GetNavCache(this, node);
			navCache.MoveToParent();
			BaseIterator baseIterator = this.filter.EvaluateNodeSet(new NullIterator(navCache, ctx));
			while (baseIterator.MoveNext())
			{
				if (node.IsSamePosition(baseIterator.Current))
				{
					return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			string text = string.Empty;
			if (this.patternPrevious != null)
			{
				text = this.patternPrevious.ToString() + ((!this.isAncestor) ? "/" : "//");
			}
			if (this.filter != null)
			{
				text += this.filter.ToString();
			}
			else
			{
				text += this.nodeTest.ToString();
			}
			return text;
		}

		public LocationPathPattern LastPathPattern
		{
			get
			{
				LocationPathPattern locationPathPattern = this;
				while (locationPathPattern.patternPrevious != null)
				{
					locationPathPattern = locationPathPattern.patternPrevious;
				}
				return locationPathPattern;
			}
		}
	}
}
