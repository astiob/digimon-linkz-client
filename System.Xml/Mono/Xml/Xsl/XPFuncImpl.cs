using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal abstract class XPFuncImpl : IXsltContextFunction
	{
		private int minargs;

		private int maxargs;

		private XPathResultType returnType;

		private XPathResultType[] argTypes;

		public XPFuncImpl()
		{
		}

		public XPFuncImpl(int minArgs, int maxArgs, XPathResultType returnType, XPathResultType[] argTypes)
		{
			this.Init(minArgs, maxArgs, returnType, argTypes);
		}

		protected void Init(int minArgs, int maxArgs, XPathResultType returnType, XPathResultType[] argTypes)
		{
			this.minargs = minArgs;
			this.maxargs = maxArgs;
			this.returnType = returnType;
			this.argTypes = argTypes;
		}

		public int Minargs
		{
			get
			{
				return this.minargs;
			}
		}

		public int Maxargs
		{
			get
			{
				return this.maxargs;
			}
		}

		public XPathResultType ReturnType
		{
			get
			{
				return this.returnType;
			}
		}

		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.Invoke((XsltCompiledContext)xsltContext, args, docContext);
		}

		public abstract object Invoke(XsltCompiledContext xsltContext, object[] args, XPathNavigator docContext);

		public static XPathResultType GetXPathType(Type type, XPathNavigator node)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			switch (typeCode)
			{
			case TypeCode.Object:
				if (typeof(XPathNavigator).IsAssignableFrom(type) || typeof(IXPathNavigable).IsAssignableFrom(type))
				{
					return XPathResultType.Navigator;
				}
				if (typeof(XPathNodeIterator).IsAssignableFrom(type))
				{
					return XPathResultType.NodeSet;
				}
				return XPathResultType.Any;
			default:
				switch (typeCode)
				{
				case TypeCode.DateTime:
					throw new XsltException("Invalid type DateTime was specified.", null, node);
				case TypeCode.String:
					return XPathResultType.String;
				}
				return XPathResultType.Number;
			case TypeCode.Boolean:
				return XPathResultType.Boolean;
			}
		}
	}
}
