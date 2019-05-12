using Mono.Xml.Xsl;
using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
	internal abstract class Pattern
	{
		internal static Pattern Compile(string s, Compiler comp)
		{
			return Pattern.Compile(comp.patternParser.Compile(s));
		}

		internal static Pattern Compile(Expression e)
		{
			if (e is ExprUNION)
			{
				return new UnionPattern(Pattern.Compile(((ExprUNION)e).left), Pattern.Compile(((ExprUNION)e).right));
			}
			if (e is ExprRoot)
			{
				return new LocationPathPattern(new NodeTypeTest(Axes.Self, XPathNodeType.Root));
			}
			if (e is NodeTest)
			{
				return new LocationPathPattern((NodeTest)e);
			}
			if (e is ExprFilter)
			{
				return new LocationPathPattern((ExprFilter)e);
			}
			if (e is ExprSLASH)
			{
				Pattern prev = Pattern.Compile(((ExprSLASH)e).left);
				LocationPathPattern locationPathPattern = (LocationPathPattern)Pattern.Compile(((ExprSLASH)e).right);
				locationPathPattern.SetPreviousPattern(prev, false);
				return locationPathPattern;
			}
			if (e is ExprSLASH2)
			{
				if (((ExprSLASH2)e).left is ExprRoot)
				{
					return Pattern.Compile(((ExprSLASH2)e).right);
				}
				Pattern prev2 = Pattern.Compile(((ExprSLASH2)e).left);
				LocationPathPattern locationPathPattern2 = (LocationPathPattern)Pattern.Compile(((ExprSLASH2)e).right);
				locationPathPattern2.SetPreviousPattern(prev2, true);
				return locationPathPattern2;
			}
			else
			{
				if (e is XPathFunctionId)
				{
					ExprLiteral exprLiteral = ((XPathFunctionId)e).Id as ExprLiteral;
					return new IdPattern(exprLiteral.Value);
				}
				if (e is XsltKey)
				{
					return new KeyPattern((XsltKey)e);
				}
				return null;
			}
		}

		public virtual double DefaultPriority
		{
			get
			{
				return 0.5;
			}
		}

		public virtual XPathNodeType EvaluatedNodeType
		{
			get
			{
				return XPathNodeType.All;
			}
		}

		public abstract bool Matches(XPathNavigator node, XsltContext ctx);
	}
}
