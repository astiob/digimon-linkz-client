using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
	/// <summary>Provides a cursor model for navigating and editing XML data.</summary>
	public abstract class XPathNavigator : XPathItem, ICloneable, IXPathNavigable, IXmlNamespaceResolver
	{
		private static readonly char[] escape_text_chars = new char[]
		{
			'&',
			'<',
			'>'
		};

		private static readonly char[] escape_attr_chars = new char[]
		{
			'"',
			'&',
			'<',
			'>',
			'\r',
			'\n'
		};

		/// <summary>For a description of this member, see <see cref="M:System.Xml.XPath.XPathNavigator.Clone" />.</summary>
		/// <returns>An <see cref="T:System.Object" />.</returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>Gets an <see cref="T:System.Collections.IEqualityComparer" /> used for equality comparison of <see cref="T:System.Xml.XPath.XPathNavigator" /> objects.</summary>
		/// <returns>An <see cref="T:System.Collections.IEqualityComparer" /> used for equality comparison of <see cref="T:System.Xml.XPath.XPathNavigator" /> objects.</returns>
		public static IEqualityComparer NavigatorComparer
		{
			get
			{
				return XPathNavigatorComparer.Instance;
			}
		}

		/// <summary>When overridden in a derived class, gets the base URI for the current node.</summary>
		/// <returns>The location from which the node was loaded, or <see cref="F:System.String.Empty" /> if there is no value.</returns>
		public abstract string BaseURI { get; }

		/// <summary>Gets a value indicating whether the <see cref="T:System.Xml.XPath.XPathNavigator" /> can edit the underlying XML data.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> can edit the underlying XML data; otherwise false.</returns>
		public virtual bool CanEdit
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the current node has any attributes.</summary>
		/// <returns>Returns true if the current node has attributes; returns false if the current node has no attributes, or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element node.</returns>
		public virtual bool HasAttributes
		{
			get
			{
				if (!this.MoveToFirstAttribute())
				{
					return false;
				}
				this.MoveToParent();
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the current node has any child nodes.</summary>
		/// <returns>Returns true if the current node has any child nodes; otherwise, false.</returns>
		public virtual bool HasChildren
		{
			get
			{
				if (!this.MoveToFirstChild())
				{
					return false;
				}
				this.MoveToParent();
				return true;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current node is an empty element without an end element tag.</summary>
		/// <returns>Returns true if the current node is an empty element; otherwise, false.</returns>
		public abstract bool IsEmptyElement { get; }

		/// <summary>When overridden in a derived class, gets the <see cref="P:System.Xml.XPath.XPathNavigator.Name" /> of the current node without any namespace prefix.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the local name of the current node, or <see cref="F:System.String.Empty" /> if the current node does not have a name (for example, text or comment nodes).</returns>
		public abstract string LocalName { get; }

		/// <summary>When overridden in a derived class, gets the qualified name of the current node.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the qualified <see cref="P:System.Xml.XPath.XPathNavigator.Name" /> of the current node, or <see cref="F:System.String.Empty" /> if the current node does not have a name (for example, text or comment nodes).</returns>
		public abstract string Name { get; }

		/// <summary>When overridden in a derived class, gets the namespace URI of the current node.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the namespace URI of the current node, or <see cref="F:System.String.Empty" /> if the current node has no namespace URI.</returns>
		public abstract string NamespaceURI { get; }

		/// <summary>When overridden in a derived class, gets the <see cref="T:System.Xml.XmlNameTable" /> of the <see cref="T:System.Xml.XPath.XPathNavigator" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNameTable" /> object enabling you to get the atomized version of a <see cref="T:System.String" /> within the XML document.</returns>
		public abstract XmlNameTable NameTable { get; }

		/// <summary>When overridden in a derived class, gets the <see cref="T:System.Xml.XPath.XPathNodeType" /> of the current node.</summary>
		/// <returns>One of the <see cref="T:System.Xml.XPath.XPathNodeType" /> values representing the current node.</returns>
		public abstract XPathNodeType NodeType { get; }

		/// <summary>When overridden in a derived class, gets the namespace prefix associated with the current node.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the namespace prefix associated with the current node.</returns>
		public abstract string Prefix { get; }

		/// <summary>Gets the xml:lang scope for the current node.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value of the xml:lang scope, or <see cref="F:System.String.Empty" /> if the current node has no xml:lang scope value to return.</returns>
		public virtual string XmlLang
		{
			get
			{
				XPathNavigator xpathNavigator = this.Clone();
				XPathNodeType nodeType = xpathNavigator.NodeType;
				if (nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace)
				{
					xpathNavigator.MoveToParent();
				}
				while (!xpathNavigator.MoveToAttribute("lang", "http://www.w3.org/XML/1998/namespace"))
				{
					if (!xpathNavigator.MoveToParent())
					{
						return string.Empty;
					}
				}
				return xpathNavigator.Value;
			}
		}

		/// <summary>When overridden in a derived class, creates a new <see cref="T:System.Xml.XPath.XPathNavigator" /> positioned at the same node as this <see cref="T:System.Xml.XPath.XPathNavigator" />.</summary>
		/// <returns>A new <see cref="T:System.Xml.XPath.XPathNavigator" /> positioned at the same node as this <see cref="T:System.Xml.XPath.XPathNavigator" />.</returns>
		public abstract XPathNavigator Clone();

		/// <summary>Compares the position of the current <see cref="T:System.Xml.XPath.XPathNavigator" /> with the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> specified.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlNodeOrder" /> value representing the comparative position of the two <see cref="T:System.Xml.XPath.XPathNavigator" /> objects.</returns>
		/// <param name="nav">The <see cref="T:System.Xml.XPath.XPathNavigator" /> to compare against.</param>
		public virtual XmlNodeOrder ComparePosition(XPathNavigator nav)
		{
			if (this.IsSamePosition(nav))
			{
				return XmlNodeOrder.Same;
			}
			if (this.IsDescendant(nav))
			{
				return XmlNodeOrder.Before;
			}
			if (nav.IsDescendant(this))
			{
				return XmlNodeOrder.After;
			}
			XPathNavigator xpathNavigator = this.Clone();
			XPathNavigator xpathNavigator2 = nav.Clone();
			xpathNavigator.MoveToRoot();
			xpathNavigator2.MoveToRoot();
			if (!xpathNavigator.IsSamePosition(xpathNavigator2))
			{
				return XmlNodeOrder.Unknown;
			}
			xpathNavigator.MoveTo(this);
			xpathNavigator2.MoveTo(nav);
			int num = 0;
			while (xpathNavigator.MoveToParent())
			{
				num++;
			}
			xpathNavigator.MoveTo(this);
			int num2 = 0;
			while (xpathNavigator2.MoveToParent())
			{
				num2++;
			}
			xpathNavigator2.MoveTo(nav);
			int i;
			for (i = num; i > num2; i--)
			{
				xpathNavigator.MoveToParent();
			}
			for (int j = num2; j > i; j--)
			{
				xpathNavigator2.MoveToParent();
			}
			while (!xpathNavigator.IsSamePosition(xpathNavigator2))
			{
				xpathNavigator.MoveToParent();
				xpathNavigator2.MoveToParent();
				i--;
			}
			xpathNavigator.MoveTo(this);
			for (int k = num; k > i + 1; k--)
			{
				xpathNavigator.MoveToParent();
			}
			xpathNavigator2.MoveTo(nav);
			for (int l = num2; l > i + 1; l--)
			{
				xpathNavigator2.MoveToParent();
			}
			if (xpathNavigator.NodeType == XPathNodeType.Namespace)
			{
				if (xpathNavigator2.NodeType != XPathNodeType.Namespace)
				{
					return XmlNodeOrder.Before;
				}
				while (xpathNavigator.MoveToNextNamespace())
				{
					if (xpathNavigator.IsSamePosition(xpathNavigator2))
					{
						return XmlNodeOrder.Before;
					}
				}
				return XmlNodeOrder.After;
			}
			else
			{
				if (xpathNavigator2.NodeType == XPathNodeType.Namespace)
				{
					return XmlNodeOrder.After;
				}
				if (xpathNavigator.NodeType != XPathNodeType.Attribute)
				{
					while (xpathNavigator.MoveToNext())
					{
						if (xpathNavigator.IsSamePosition(xpathNavigator2))
						{
							return XmlNodeOrder.Before;
						}
					}
					return XmlNodeOrder.After;
				}
				if (xpathNavigator2.NodeType != XPathNodeType.Attribute)
				{
					return XmlNodeOrder.Before;
				}
				while (xpathNavigator.MoveToNextAttribute())
				{
					if (xpathNavigator.IsSamePosition(xpathNavigator2))
					{
						return XmlNodeOrder.Before;
					}
				}
				return XmlNodeOrder.After;
			}
		}

		/// <summary>Compiles a string representing an XPath expression and returns an <see cref="T:System.Xml.XPath.XPathExpression" /> object.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathExpression" /> object representing the XPath expression.</returns>
		/// <param name="xpath">A string representing an XPath expression.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="xpath" /> parameter contains an XPath expression that is not valid.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual XPathExpression Compile(string xpath)
		{
			return XPathExpression.Compile(xpath);
		}

		internal virtual XPathExpression Compile(string xpath, IStaticXsltContext ctx)
		{
			return XPathExpression.Compile(xpath, null, ctx);
		}

		/// <summary>Evaluates the specified XPath expression and returns the typed result.</summary>
		/// <returns>The result of the expression (Boolean, number, string, or node set). This maps to <see cref="T:System.Boolean" />, <see cref="T:System.Double" />, <see cref="T:System.String" />, or <see cref="T:System.Xml.XPath.XPathNodeIterator" /> objects respectively.</returns>
		/// <param name="xpath">A string representing an XPath expression that can be evaluated.</param>
		/// <exception cref="T:System.ArgumentException">The return type of the XPath expression is a node set.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual object Evaluate(string xpath)
		{
			return this.Evaluate(this.Compile(xpath));
		}

		/// <summary>Evaluates the <see cref="T:System.Xml.XPath.XPathExpression" /> and returns the typed result.</summary>
		/// <returns>The result of the expression (Boolean, number, string, or node set). This maps to <see cref="T:System.Boolean" />, <see cref="T:System.Double" />, <see cref="T:System.String" />, or <see cref="T:System.Xml.XPath.XPathNodeIterator" /> objects respectively.</returns>
		/// <param name="expr">An <see cref="T:System.Xml.XPath.XPathExpression" /> that can be evaluated.</param>
		/// <exception cref="T:System.ArgumentException">The return type of the XPath expression is a node set.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual object Evaluate(XPathExpression expr)
		{
			return this.Evaluate(expr, null);
		}

		/// <summary>Uses the supplied context to evaluate the <see cref="T:System.Xml.XPath.XPathExpression" />, and returns the typed result.</summary>
		/// <returns>The result of the expression (Boolean, number, string, or node set). This maps to <see cref="T:System.Boolean" />, <see cref="T:System.Double" />, <see cref="T:System.String" />, or <see cref="T:System.Xml.XPath.XPathNodeIterator" /> objects respectively.</returns>
		/// <param name="expr">An <see cref="T:System.Xml.XPath.XPathExpression" /> that can be evaluated.</param>
		/// <param name="context">An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that points to the selected node set that the evaluation is to be performed on.</param>
		/// <exception cref="T:System.ArgumentException">The return type of the XPath expression is a node set.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual object Evaluate(XPathExpression expr, XPathNodeIterator context)
		{
			return this.Evaluate(expr, context, null);
		}

		private BaseIterator ToBaseIterator(XPathNodeIterator iter, IXmlNamespaceResolver ctx)
		{
			BaseIterator baseIterator = iter as BaseIterator;
			if (baseIterator == null)
			{
				baseIterator = new WrapperIterator(iter, ctx);
			}
			return baseIterator;
		}

		private object Evaluate(XPathExpression expr, XPathNodeIterator context, IXmlNamespaceResolver ctx)
		{
			CompiledExpression compiledExpression = (CompiledExpression)expr;
			if (ctx == null)
			{
				ctx = compiledExpression.NamespaceManager;
			}
			if (context == null)
			{
				context = new NullIterator(this, ctx);
			}
			BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
			baseIterator.NamespaceManager = ctx;
			return compiledExpression.Evaluate(baseIterator);
		}

		internal XPathNodeIterator EvaluateNodeSet(XPathExpression expr, XPathNodeIterator context, IXmlNamespaceResolver ctx)
		{
			CompiledExpression compiledExpression = (CompiledExpression)expr;
			if (ctx == null)
			{
				ctx = compiledExpression.NamespaceManager;
			}
			if (context == null)
			{
				context = new NullIterator(this, compiledExpression.NamespaceManager);
			}
			BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
			baseIterator.NamespaceManager = ctx;
			return compiledExpression.EvaluateNodeSet(baseIterator);
		}

		internal string EvaluateString(XPathExpression expr, XPathNodeIterator context, IXmlNamespaceResolver ctx)
		{
			CompiledExpression compiledExpression = (CompiledExpression)expr;
			if (ctx == null)
			{
				ctx = compiledExpression.NamespaceManager;
			}
			if (context == null)
			{
				context = new NullIterator(this, compiledExpression.NamespaceManager);
			}
			BaseIterator iter = this.ToBaseIterator(context, ctx);
			return compiledExpression.EvaluateString(iter);
		}

		internal double EvaluateNumber(XPathExpression expr, XPathNodeIterator context, IXmlNamespaceResolver ctx)
		{
			CompiledExpression compiledExpression = (CompiledExpression)expr;
			if (ctx == null)
			{
				ctx = compiledExpression.NamespaceManager;
			}
			if (context == null)
			{
				context = new NullIterator(this, compiledExpression.NamespaceManager);
			}
			BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
			baseIterator.NamespaceManager = ctx;
			return compiledExpression.EvaluateNumber(baseIterator);
		}

		internal bool EvaluateBoolean(XPathExpression expr, XPathNodeIterator context, IXmlNamespaceResolver ctx)
		{
			CompiledExpression compiledExpression = (CompiledExpression)expr;
			if (ctx == null)
			{
				ctx = compiledExpression.NamespaceManager;
			}
			if (context == null)
			{
				context = new NullIterator(this, compiledExpression.NamespaceManager);
			}
			BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
			baseIterator.NamespaceManager = ctx;
			return compiledExpression.EvaluateBoolean(baseIterator);
		}

		/// <summary>Gets the value of the attribute with the specified local name and namespace URI.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value of the specified attribute; <see cref="F:System.String.Empty" /> if a matching attribute is not found, or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element node.</returns>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="namespaceURI">The namespace URI of the attribute.</param>
		public virtual string GetAttribute(string localName, string namespaceURI)
		{
			if (!this.MoveToAttribute(localName, namespaceURI))
			{
				return string.Empty;
			}
			string value = this.Value;
			this.MoveToParent();
			return value;
		}

		/// <summary>Returns the value of the namespace node corresponding to the specified local name.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the value of the namespace node; <see cref="F:System.String.Empty" /> if a matching namespace node is not found, or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element node.</returns>
		/// <param name="name">The local name of the namespace node.</param>
		public virtual string GetNamespace(string name)
		{
			if (!this.MoveToNamespace(name))
			{
				return string.Empty;
			}
			string value = this.Value;
			this.MoveToParent();
			return value;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Xml.XPath.XPathNavigator" /> is a descendant of the current <see cref="T:System.Xml.XPath.XPathNavigator" />.</summary>
		/// <returns>Returns true if the specified <see cref="T:System.Xml.XPath.XPathNavigator" /> is a descendant of the current <see cref="T:System.Xml.XPath.XPathNavigator" />; otherwise, false.</returns>
		/// <param name="nav">The <see cref="T:System.Xml.XPath.XPathNavigator" /> to compare to this <see cref="T:System.Xml.XPath.XPathNavigator" />.</param>
		public virtual bool IsDescendant(XPathNavigator nav)
		{
			if (nav != null)
			{
				nav = nav.Clone();
				while (nav.MoveToParent())
				{
					if (this.IsSamePosition(nav))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>When overridden in a derived class, determines whether the current <see cref="T:System.Xml.XPath.XPathNavigator" /> is at the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator" />.</summary>
		/// <returns>Returns true if the two <see cref="T:System.Xml.XPath.XPathNavigator" /> objects have the same position; otherwise, false.</returns>
		/// <param name="other">The <see cref="T:System.Xml.XPath.XPathNavigator" /> to compare to this <see cref="T:System.Xml.XPath.XPathNavigator" />.</param>
		public abstract bool IsSamePosition(XPathNavigator other);

		/// <summary>Determines whether the current node matches the specified XPath expression.</summary>
		/// <returns>Returns true if the current node matches the specified XPath expression; otherwise, false.</returns>
		/// <param name="xpath">The XPath expression.</param>
		/// <exception cref="T:System.ArgumentException">The XPath expression cannot be evaluated.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual bool Matches(string xpath)
		{
			return this.Matches(this.Compile(xpath));
		}

		/// <summary>Determines whether the current node matches the specified <see cref="T:System.Xml.XPath.XPathExpression" />.</summary>
		/// <returns>Returns true if the current node matches the <see cref="T:System.Xml.XPath.XPathExpression" />; otherwise, false.</returns>
		/// <param name="expr">An <see cref="T:System.Xml.XPath.XPathExpression" /> object containing the compiled XPath expression.</param>
		/// <exception cref="T:System.ArgumentException">The XPath expression cannot be evaluated.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual bool Matches(XPathExpression expr)
		{
			Expression expression = ((CompiledExpression)expr).ExpressionNode;
			if (expression is ExprRoot)
			{
				return this.NodeType == XPathNodeType.Root;
			}
			NodeTest nodeTest = expression as NodeTest;
			if (nodeTest == null)
			{
				if (expression is ExprFilter)
				{
					do
					{
						expression = ((ExprFilter)expression).LeftHandSide;
					}
					while (expression is ExprFilter);
					if (expression is NodeTest && !((NodeTest)expression).Match(((CompiledExpression)expr).NamespaceManager, this))
					{
						return false;
					}
				}
				switch (expression.ReturnType)
				{
				case XPathResultType.NodeSet:
				case XPathResultType.Any:
				{
					XPathNodeType evaluatedNodeType = expression.EvaluatedNodeType;
					if (evaluatedNodeType == XPathNodeType.Attribute || evaluatedNodeType == XPathNodeType.Namespace)
					{
						if (this.NodeType != expression.EvaluatedNodeType)
						{
							return false;
						}
					}
					XPathNodeIterator xpathNodeIterator = this.Select(expr);
					while (xpathNodeIterator.MoveNext())
					{
						if (this.IsSamePosition(xpathNodeIterator.Current))
						{
							return true;
						}
					}
					XPathNavigator xpathNavigator = this.Clone();
					while (xpathNavigator.MoveToParent())
					{
						xpathNodeIterator = xpathNavigator.Select(expr);
						while (xpathNodeIterator.MoveNext())
						{
							if (this.IsSamePosition(xpathNodeIterator.Current))
							{
								return true;
							}
						}
					}
					return false;
				}
				}
				return false;
			}
			Axes axis = nodeTest.Axis.Axis;
			if (axis != Axes.Attribute && axis != Axes.Child)
			{
				throw new XPathException("Only child and attribute pattern are allowed for a pattern.");
			}
			return nodeTest.Match(((CompiledExpression)expr).NamespaceManager, this);
		}

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator" />.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator" />; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="other">The <see cref="T:System.Xml.XPath.XPathNavigator" /> positioned on the node that you want to move to. </param>
		public abstract bool MoveTo(XPathNavigator other);

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the attribute with the matching local name and namespace URI.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the attribute; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="namespaceURI">The namespace URI of the attribute; null for an empty namespace.</param>
		public virtual bool MoveToAttribute(string localName, string namespaceURI)
		{
			if (this.MoveToFirstAttribute())
			{
				while (!(this.LocalName == localName) || !(this.NamespaceURI == namespaceURI))
				{
					if (!this.MoveToNextAttribute())
					{
						this.MoveToParent();
						return false;
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the namespace node with the specified namespace prefix.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the specified namespace; false if a matching namespace node was not found, or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="name">The namespace prefix of the namespace node.</param>
		public virtual bool MoveToNamespace(string name)
		{
			if (this.MoveToFirstNamespace())
			{
				while (!(this.LocalName == name))
				{
					if (!this.MoveToNextNamespace())
					{
						this.MoveToParent();
						return false;
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the first sibling node of the current node.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the first sibling node of the current node; false if there is no first sibling, or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is currently positioned on an attribute node. If the <see cref="T:System.Xml.XPath.XPathNavigator" /> is already positioned on the first sibling, <see cref="T:System.Xml.XPath.XPathNavigator" /> will return true and will not move its position.If <see cref="T:System.Xml.XPath.XPathNavigator.MoveToFirst" /> returns false because there is no first sibling, or if <see cref="T:System.Xml.XPath.XPathNavigator" /> is currently positioned on an attribute, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public virtual bool MoveToFirst()
		{
			return this.MoveToFirstImpl();
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the root node that the current node belongs to.</summary>
		public virtual void MoveToRoot()
		{
			while (this.MoveToParent())
			{
			}
		}

		internal bool MoveToFirstImpl()
		{
			XPathNodeType nodeType = this.NodeType;
			if (nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace)
			{
				return false;
			}
			if (!this.MoveToParent())
			{
				return false;
			}
			this.MoveToFirstChild();
			return true;
		}

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the first attribute of the current node.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the first attribute of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public abstract bool MoveToFirstAttribute();

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the first child node of the current node.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the first child node of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public abstract bool MoveToFirstChild();

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to first namespace node of the current node.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the first namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public bool MoveToFirstNamespace()
		{
			return this.MoveToFirstNamespace(XPathNamespaceScope.All);
		}

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the first namespace node that matches the <see cref="T:System.Xml.XPath.XPathNamespaceScope" /> specified.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the first namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="namespaceScope">An <see cref="T:System.Xml.XPath.XPathNamespaceScope" /> value describing the namespace scope. </param>
		public abstract bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope);

		/// <summary>When overridden in a derived class, moves to the node that has an attribute of type ID whose value matches the specified <see cref="T:System.String" />.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving; otherwise, false. If false, the position of the navigator is unchanged.</returns>
		/// <param name="id">A <see cref="T:System.String" /> representing the ID value of the node to which you want to move.</param>
		public abstract bool MoveToId(string id);

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the next sibling node of the current node.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the next sibling node; otherwise, false if there are no more siblings or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public abstract bool MoveToNext();

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the next attribute.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the next attribute; false if there are no more attributes. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public abstract bool MoveToNextAttribute();

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the next namespace node.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the next namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public bool MoveToNextNamespace()
		{
			return this.MoveToNextNamespace(XPathNamespaceScope.All);
		}

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the next namespace node matching the <see cref="T:System.Xml.XPath.XPathNamespaceScope" /> specified.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the next namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="namespaceScope">An <see cref="T:System.Xml.XPath.XPathNamespaceScope" /> value describing the namespace scope. </param>
		public abstract bool MoveToNextNamespace(XPathNamespaceScope namespaceScope);

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the parent node of the current node.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the parent node of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public abstract bool MoveToParent();

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the previous sibling node of the current node.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the previous sibling node; otherwise, false if there is no previous sibling node or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		public abstract bool MoveToPrevious();

		/// <summary>Selects a node set, using the specified XPath expression.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> pointing to the selected node set.</returns>
		/// <param name="xpath">A <see cref="T:System.String" /> representing an XPath expression.</param>
		/// <exception cref="T:System.ArgumentException">The XPath expression contains an error or its return type is not a node set.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual XPathNodeIterator Select(string xpath)
		{
			return this.Select(this.Compile(xpath));
		}

		/// <summary>Selects a node set using the specified <see cref="T:System.Xml.XPath.XPathExpression" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that points to the selected node set.</returns>
		/// <param name="expr">An <see cref="T:System.Xml.XPath.XPathExpression" /> object containing the compiled XPath query.</param>
		/// <exception cref="T:System.ArgumentException">The XPath expression contains an error or its return type is not a node set.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual XPathNodeIterator Select(XPathExpression expr)
		{
			return this.Select(expr, null);
		}

		internal XPathNodeIterator Select(XPathExpression expr, IXmlNamespaceResolver ctx)
		{
			CompiledExpression compiledExpression = (CompiledExpression)expr;
			if (ctx == null)
			{
				ctx = compiledExpression.NamespaceManager;
			}
			BaseIterator iter = new NullIterator(this, ctx);
			return compiledExpression.EvaluateNodeSet(iter);
		}

		/// <summary>Selects all the ancestor nodes of the current node that have a matching <see cref="T:System.Xml.XPath.XPathNodeType" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that contains the selected nodes. The returned nodes are in reverse document order.</returns>
		/// <param name="type">The <see cref="T:System.Xml.XPath.XPathNodeType" /> of the ancestor nodes.</param>
		/// <param name="matchSelf">To include the context node in the selection, true; otherwise, false.</param>
		public virtual XPathNodeIterator SelectAncestors(XPathNodeType type, bool matchSelf)
		{
			Axes axis = (!matchSelf) ? Axes.Ancestor : Axes.AncestorOrSelf;
			return this.SelectTest(new NodeTypeTest(axis, type));
		}

		/// <summary>Selects all the ancestor nodes of the current node that have the specified local name and namespace URI.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that contains the selected nodes. The returned nodes are in reverse document order.</returns>
		/// <param name="name">The local name of the ancestor nodes.</param>
		/// <param name="namespaceURI">The namespace URI of the ancestor nodes.</param>
		/// <param name="matchSelf">To include the context node in the selection, true; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">null cannot be passed as a parameter.</exception>
		public virtual XPathNodeIterator SelectAncestors(string name, string namespaceURI, bool matchSelf)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			Axes axis = (!matchSelf) ? Axes.Ancestor : Axes.AncestorOrSelf;
			XmlQualifiedName name2 = new XmlQualifiedName(name, namespaceURI);
			return this.SelectTest(new NodeNameTest(axis, name2, true));
		}

		private static IEnumerable EnumerateChildren(XPathNavigator n, XPathNodeType type)
		{
			if (!n.MoveToFirstChild())
			{
				yield break;
			}
			n.MoveToParent();
			XPathNavigator nav = n.Clone();
			nav.MoveToFirstChild();
			XPathNavigator nav2 = null;
			do
			{
				if (type == XPathNodeType.All || nav.NodeType == type)
				{
					if (nav2 == null)
					{
						nav2 = nav.Clone();
					}
					else
					{
						nav2.MoveTo(nav);
					}
					yield return nav2;
				}
			}
			while (nav.MoveToNext());
			yield break;
		}

		/// <summary>Selects all the child nodes of the current node that have the matching <see cref="T:System.Xml.XPath.XPathNodeType" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that contains the selected nodes.</returns>
		/// <param name="type">The <see cref="T:System.Xml.XPath.XPathNodeType" /> of the child nodes.</param>
		public virtual XPathNodeIterator SelectChildren(XPathNodeType type)
		{
			return new WrapperIterator(new XPathNavigator.EnumerableIterator(XPathNavigator.EnumerateChildren(this, type), 0), null);
		}

		private static IEnumerable EnumerateChildren(XPathNavigator n, string name, string ns)
		{
			if (!n.MoveToFirstChild())
			{
				yield break;
			}
			n.MoveToParent();
			XPathNavigator nav = n.Clone();
			nav.MoveToFirstChild();
			XPathNavigator nav2 = nav.Clone();
			do
			{
				if ((name == string.Empty || nav.LocalName == name) && (ns == string.Empty || nav.NamespaceURI == ns))
				{
					nav2.MoveTo(nav);
					yield return nav2;
				}
			}
			while (nav.MoveToNext());
			yield break;
		}

		/// <summary>Selects all the child nodes of the current node that have the local name and namespace URI specified.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that contains the selected nodes.</returns>
		/// <param name="name">The local name of the child nodes. </param>
		/// <param name="namespaceURI">The namespace URI of the child nodes. </param>
		/// <exception cref="T:System.ArgumentNullException">null cannot be passed as a parameter.</exception>
		public virtual XPathNodeIterator SelectChildren(string name, string namespaceURI)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			return new WrapperIterator(new XPathNavigator.EnumerableIterator(XPathNavigator.EnumerateChildren(this, name, namespaceURI), 0), null);
		}

		/// <summary>Selects all the descendant nodes of the current node that have a matching <see cref="T:System.Xml.XPath.XPathNodeType" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that contains the selected nodes.</returns>
		/// <param name="type">The <see cref="T:System.Xml.XPath.XPathNodeType" /> of the descendant nodes.</param>
		/// <param name="matchSelf">true to include the context node in the selection; otherwise, false.</param>
		public virtual XPathNodeIterator SelectDescendants(XPathNodeType type, bool matchSelf)
		{
			Axes axis = (!matchSelf) ? Axes.Descendant : Axes.DescendantOrSelf;
			return this.SelectTest(new NodeTypeTest(axis, type));
		}

		/// <summary>Selects all the descendant nodes of the current node with the local name and namespace URI specified.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that contains the selected nodes.</returns>
		/// <param name="name">The local name of the descendant nodes. </param>
		/// <param name="namespaceURI">The namespace URI of the descendant nodes. </param>
		/// <param name="matchSelf">true to include the context node in the selection; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">null cannot be passed as a parameter.</exception>
		public virtual XPathNodeIterator SelectDescendants(string name, string namespaceURI, bool matchSelf)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			Axes axis = (!matchSelf) ? Axes.Descendant : Axes.DescendantOrSelf;
			XmlQualifiedName name2 = new XmlQualifiedName(name, namespaceURI);
			return this.SelectTest(new NodeNameTest(axis, name2, true));
		}

		internal XPathNodeIterator SelectTest(NodeTest test)
		{
			return test.EvaluateNodeSet(new NullIterator(this));
		}

		/// <summary>Gets the text value of the current node.</summary>
		/// <returns>A string that contains the text value of the current node.</returns>
		public override string ToString()
		{
			return this.Value;
		}

		/// <summary>Verifies that the XML data in the <see cref="T:System.Xml.XPath.XPathNavigator" /> conforms to the XML Schema definition language (XSD) schema provided.</summary>
		/// <returns>true if no schema validation errors occurred; otherwise, false.</returns>
		/// <param name="schemas">The <see cref="T:System.Xml.Schema.XmlSchemaSet" /> containing the schemas used to validate the XML data contained in the <see cref="T:System.Xml.XPath.XPathNavigator" />.</param>
		/// <param name="validationEventHandler">The <see cref="T:System.Xml.Schema.ValidationEventHandler" /> that receives information about schema validation warnings and errors.</param>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaValidationException">A schema validation error occurred, and no <see cref="T:System.Xml.Schema.ValidationEventHandler" /> was specified to handle validation errors.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on a node that is not an element, attribute, or the root node or there is not type information to perform validation.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="M:System.Xml.XPath.XPathNavigator.CheckValidity(System.Xml.Schema.XmlSchemaSet,System.Xml.Schema.ValidationEventHandler)" /> method was called with an <see cref="T:System.Xml.Schema.XmlSchemaSet" /> parameter when the <see cref="T:System.Xml.XPath.XPathNavigator" /> was not positioned on the root node of the XML data.</exception>
		public virtual bool CheckValidity(XmlSchemaSet schemas, ValidationEventHandler handler)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.NameTable = this.NameTable;
			xmlReaderSettings.SetSchemas(schemas);
			xmlReaderSettings.ValidationEventHandler += handler;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			try
			{
				XmlReader xmlReader = XmlReader.Create(this.ReadSubtree(), xmlReaderSettings);
				while (!xmlReader.EOF)
				{
					xmlReader.Read();
				}
			}
			catch (XmlSchemaValidationException)
			{
				return false;
			}
			return true;
		}

		/// <summary>Returns a copy of the <see cref="T:System.Xml.XPath.XPathNavigator" />.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> copy of this <see cref="T:System.Xml.XPath.XPathNavigator" />.</returns>
		public virtual XPathNavigator CreateNavigator()
		{
			return this.Clone();
		}

		/// <summary>Evaluates the specified XPath expression and returns the typed result, using the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object specified to resolve namespace prefixes in the XPath expression.</summary>
		/// <returns>The result of the expression (Boolean, number, string, or node set). This maps to <see cref="T:System.Boolean" />, <see cref="T:System.Double" />, <see cref="T:System.String" />, or <see cref="T:System.Xml.XPath.XPathNodeIterator" /> objects respectively.</returns>
		/// <param name="xpath">A string representing an XPath expression that can be evaluated.</param>
		/// <param name="resolver">The <see cref="T:System.Xml.IXmlNamespaceResolver" /> object used to resolve namespace prefixes in the XPath expression.</param>
		/// <exception cref="T:System.ArgumentException">The return type of the XPath expression is a node set.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual object Evaluate(string xpath, IXmlNamespaceResolver nsResolver)
		{
			return this.Evaluate(this.Compile(xpath), null, nsResolver);
		}

		/// <summary>Returns the in-scope namespaces of the current node.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IDictionary`2" /> collection of namespace names keyed by prefix.</returns>
		/// <param name="scope">An <see cref="T:System.Xml.XmlNamespaceScope" /> value specifying the namespaces to return.</param>
		public virtual IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			XPathNamespaceScope namespaceScope = (scope != XmlNamespaceScope.Local) ? ((scope != XmlNamespaceScope.ExcludeXml) ? XPathNamespaceScope.All : XPathNamespaceScope.ExcludeXml) : XPathNamespaceScope.Local;
			XPathNavigator xpathNavigator = this.Clone();
			if (xpathNavigator.NodeType != XPathNodeType.Element)
			{
				xpathNavigator.MoveToParent();
			}
			if (!xpathNavigator.MoveToFirstNamespace(namespaceScope))
			{
				return dictionary;
			}
			do
			{
				dictionary.Add(xpathNavigator.Name, xpathNavigator.Value);
			}
			while (xpathNavigator.MoveToNextNamespace(namespaceScope));
			return dictionary;
		}

		/// <summary>Gets the namespace URI for the specified prefix.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the namespace URI assigned to the namespace prefix specified; null if no namespace URI is assigned to the prefix specified. The <see cref="T:System.String" /> returned is atomized.</returns>
		/// <param name="prefix">The prefix whose namespace URI you want to resolve. To match the default namespace, pass <see cref="F:System.String.Empty" />.</param>
		public virtual string LookupNamespace(string prefix)
		{
			XPathNavigator xpathNavigator = this.Clone();
			if (xpathNavigator.NodeType != XPathNodeType.Element)
			{
				xpathNavigator.MoveToParent();
			}
			if (xpathNavigator.MoveToNamespace(prefix))
			{
				return xpathNavigator.Value;
			}
			return null;
		}

		/// <summary>Gets the prefix declared for the specified namespace URI.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the namespace prefix assigned to the namespace URI specified; otherwise, <see cref="F:System.String.Empty" /> if no prefix is assigned to the namespace URI specified. The <see cref="T:System.String" /> returned is atomized.</returns>
		/// <param name="namespaceURI">The namespace URI to resolve for the prefix.</param>
		public virtual string LookupPrefix(string namespaceUri)
		{
			XPathNavigator xpathNavigator = this.Clone();
			if (xpathNavigator.NodeType != XPathNodeType.Element)
			{
				xpathNavigator.MoveToParent();
			}
			if (!xpathNavigator.MoveToFirstNamespace())
			{
				return null;
			}
			while (!(xpathNavigator.Value == namespaceUri))
			{
				if (!xpathNavigator.MoveToNextNamespace())
				{
					return null;
				}
			}
			return xpathNavigator.Name;
		}

		private bool MoveTo(XPathNodeIterator iter)
		{
			if (iter.MoveNext())
			{
				this.MoveTo(iter.Current);
				return true;
			}
			return false;
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the child node of the <see cref="T:System.Xml.XPath.XPathNodeType" /> specified.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the child node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="type">The <see cref="T:System.Xml.XPath.XPathNodeType" /> of the child node to move to.</param>
		public virtual bool MoveToChild(XPathNodeType type)
		{
			return this.MoveTo(this.SelectChildren(type));
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the child node with the local name and namespace URI specified.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the child node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="localName">The local name of the child node to move to.</param>
		/// <param name="namespaceURI">The namespace URI of the child node to move to.</param>
		public virtual bool MoveToChild(string localName, string namespaceURI)
		{
			return this.MoveTo(this.SelectChildren(localName, namespaceURI));
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the next sibling node with the local name and namespace URI specified.</summary>
		/// <returns>Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the next sibling node; false if there are no more siblings, or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="localName">The local name of the next sibling node to move to.</param>
		/// <param name="namespaceURI">The namespace URI of the next sibling node to move to.</param>
		public virtual bool MoveToNext(string localName, string namespaceURI)
		{
			XPathNavigator xpathNavigator = this.Clone();
			while (xpathNavigator.MoveToNext())
			{
				if (xpathNavigator.LocalName == localName && xpathNavigator.NamespaceURI == namespaceURI)
				{
					this.MoveTo(xpathNavigator);
					return true;
				}
			}
			return false;
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the next sibling node of the current node that matches the <see cref="T:System.Xml.XPath.XPathNodeType" /> specified.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is successful moving to the next sibling node; otherwise, false if there are no more siblings or if the <see cref="T:System.Xml.XPath.XPathNavigator" /> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> is unchanged.</returns>
		/// <param name="type">The <see cref="T:System.Xml.XPath.XPathNodeType" /> of the sibling node to move to.</param>
		public virtual bool MoveToNext(XPathNodeType type)
		{
			XPathNavigator xpathNavigator = this.Clone();
			while (xpathNavigator.MoveToNext())
			{
				if (type == XPathNodeType.All || xpathNavigator.NodeType == type)
				{
					this.MoveTo(xpathNavigator);
					return true;
				}
			}
			return false;
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the element with the local name and namespace URI specified in document order.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> moved successfully; otherwise false.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		public virtual bool MoveToFollowing(string localName, string namespaceURI)
		{
			return this.MoveToFollowing(localName, namespaceURI, null);
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the element with the local name and namespace URI specified, to the boundary specified, in document order.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> moved successfully; otherwise false.</returns>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceURI">The namespace URI of the element.</param>
		/// <param name="end">The <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the element boundary which the current <see cref="T:System.Xml.XPath.XPathNavigator" /> will not move past while searching for the following element.</param>
		public virtual bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator end)
		{
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			localName = this.NameTable.Get(localName);
			if (localName == null)
			{
				return false;
			}
			namespaceURI = this.NameTable.Get(namespaceURI);
			if (namespaceURI == null)
			{
				return false;
			}
			XPathNavigator xpathNavigator = this.Clone();
			XPathNodeType nodeType = xpathNavigator.NodeType;
			if (nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace)
			{
				xpathNavigator.MoveToParent();
			}
			for (;;)
			{
				if (!xpathNavigator.MoveToFirstChild())
				{
					while (!xpathNavigator.MoveToNext())
					{
						if (!xpathNavigator.MoveToParent())
						{
							return false;
						}
					}
				}
				if (end != null && end.IsSamePosition(xpathNavigator))
				{
					return false;
				}
				if (object.ReferenceEquals(localName, xpathNavigator.LocalName) && object.ReferenceEquals(namespaceURI, xpathNavigator.NamespaceURI))
				{
					goto Block_12;
				}
			}
			return false;
			Block_12:
			this.MoveTo(xpathNavigator);
			return true;
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the following element of the <see cref="T:System.Xml.XPath.XPathNodeType" /> specified in document order.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> moved successfully; otherwise false.</returns>
		/// <param name="type">The <see cref="T:System.Xml.XPath.XPathNodeType" /> of the element. The <see cref="T:System.Xml.XPath.XPathNodeType" /> cannot be <see cref="F:System.Xml.XPath.XPathNodeType.Attribute" /> or <see cref="F:System.Xml.XPath.XPathNodeType.Namespace" />.</param>
		public virtual bool MoveToFollowing(XPathNodeType type)
		{
			return this.MoveToFollowing(type, null);
		}

		/// <summary>Moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> to the following element of the <see cref="T:System.Xml.XPath.XPathNodeType" /> specified, to the boundary specified, in document order.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> moved successfully; otherwise false.</returns>
		/// <param name="type">The <see cref="T:System.Xml.XPath.XPathNodeType" /> of the element. The <see cref="T:System.Xml.XPath.XPathNodeType" /> cannot be <see cref="F:System.Xml.XPath.XPathNodeType.Attribute" /> or <see cref="F:System.Xml.XPath.XPathNodeType.Namespace" />.</param>
		/// <param name="end">The <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the element boundary which the current <see cref="T:System.Xml.XPath.XPathNavigator" /> will not move past while searching for the following element.</param>
		public virtual bool MoveToFollowing(XPathNodeType type, XPathNavigator end)
		{
			if (type == XPathNodeType.Root)
			{
				return false;
			}
			XPathNavigator xpathNavigator = this.Clone();
			XPathNodeType nodeType = xpathNavigator.NodeType;
			if (nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace)
			{
				xpathNavigator.MoveToParent();
			}
			for (;;)
			{
				if (!xpathNavigator.MoveToFirstChild())
				{
					while (!xpathNavigator.MoveToNext())
					{
						if (!xpathNavigator.MoveToParent())
						{
							return false;
						}
					}
				}
				if (end != null && end.IsSamePosition(xpathNavigator))
				{
					return false;
				}
				if (type == XPathNodeType.All || xpathNavigator.NodeType == type)
				{
					goto IL_8F;
				}
			}
			return false;
			IL_8F:
			this.MoveTo(xpathNavigator);
			return true;
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlReader" /> object that contains the current node and its child nodes.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object that contains the current node and its child nodes.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element node or the root node.</exception>
		public virtual XmlReader ReadSubtree()
		{
			XPathNodeType nodeType = this.NodeType;
			if (nodeType != XPathNodeType.Root && nodeType != XPathNodeType.Element)
			{
				throw new InvalidOperationException(string.Format("NodeType {0} is not supported to read as a subtree of an XPathNavigator.", this.NodeType));
			}
			return new XPathNavigatorReader(this);
		}

		/// <summary>Selects a node set using the specified XPath expression with the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object specified to resolve namespace prefixes.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNodeIterator" /> that points to the selected node set.</returns>
		/// <param name="xpath">A <see cref="T:System.String" /> representing an XPath expression.</param>
		/// <param name="resolver">The <see cref="T:System.Xml.IXmlNamespaceResolver" /> object used to resolve namespace prefixes.</param>
		/// <exception cref="T:System.ArgumentException">The XPath expression contains an error or its return type is not a node set.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath expression is not valid.</exception>
		public virtual XPathNodeIterator Select(string xpath, IXmlNamespaceResolver nsResolver)
		{
			return this.Select(this.Compile(xpath), nsResolver);
		}

		/// <summary>Selects a single node in the <see cref="T:System.Xml.XPath.XPathNavigator" /> using the specified XPath query.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> object that contains the first matching node for the XPath query specified; otherwise, null if there are no query results.</returns>
		/// <param name="xpath">A <see cref="T:System.String" /> representing an XPath expression.</param>
		/// <exception cref="T:System.ArgumentException">An error was encountered in the XPath query or the return type of the XPath expression is not a node.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath query is not valid.</exception>
		public virtual XPathNavigator SelectSingleNode(string xpath)
		{
			return this.SelectSingleNode(xpath, null);
		}

		/// <summary>Selects a single node in the <see cref="T:System.Xml.XPath.XPathNavigator" /> object using the specified XPath query with the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object specified to resolve namespace prefixes.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> object that contains the first matching node for the XPath query specified; otherwise null if there are no query results.</returns>
		/// <param name="xpath">A <see cref="T:System.String" /> representing an XPath expression.</param>
		/// <param name="resolver">The <see cref="T:System.Xml.IXmlNamespaceResolver" /> object used to resolve namespace prefixes in the XPath query.</param>
		/// <exception cref="T:System.ArgumentException">An error was encountered in the XPath query or the return type of the XPath expression is not a node.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath query is not valid.</exception>
		public virtual XPathNavigator SelectSingleNode(string xpath, IXmlNamespaceResolver nsResolver)
		{
			XPathExpression xpathExpression = this.Compile(xpath);
			xpathExpression.SetContext(nsResolver);
			return this.SelectSingleNode(xpathExpression);
		}

		/// <summary>Selects a single node in the <see cref="T:System.Xml.XPath.XPathNavigator" /> using the specified <see cref="T:System.Xml.XPath.XPathExpression" /> object.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> object that contains the first matching node for the XPath query specified; otherwise null if there are no query results.</returns>
		/// <param name="expression">An <see cref="T:System.Xml.XPath.XPathExpression" /> object containing the compiled XPath query.</param>
		/// <exception cref="T:System.ArgumentException">An error was encountered in the XPath query or the return type of the XPath expression is not a node.</exception>
		/// <exception cref="T:System.Xml.XPath.XPathException">The XPath query is not valid.</exception>
		public virtual XPathNavigator SelectSingleNode(XPathExpression expression)
		{
			XPathNodeIterator xpathNodeIterator = this.Select(expression);
			if (xpathNodeIterator.MoveNext())
			{
				return xpathNodeIterator.Current;
			}
			return null;
		}

		/// <summary>Gets the current node's value as the <see cref="T:System.Type" /> specified, using the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object specified to resolve namespace prefixes.</summary>
		/// <returns>The value of the current node as the <see cref="T:System.Type" /> requested.</returns>
		/// <param name="returnType">The <see cref="T:System.Type" /> to return the current node's value as.</param>
		/// <param name="nsResolver">The <see cref="T:System.Xml.IXmlNamespaceResolver" /> object used to resolve namespace prefixes.</param>
		/// <exception cref="T:System.FormatException">The current node's value is not in the correct format for the target type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		public override object ValueAs(Type type, IXmlNamespaceResolver nsResolver)
		{
			return new XmlAtomicValue(this.Value, XmlSchemaSimpleType.XsString).ValueAs(type, nsResolver);
		}

		/// <summary>Streams the current node and its child nodes to the <see cref="T:System.Xml.XmlWriter" /> object specified.</summary>
		/// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> object to stream to.</param>
		public virtual void WriteSubtree(XmlWriter writer)
		{
			writer.WriteNode(this, false);
		}

		private static string EscapeString(string value, bool attr)
		{
			char[] anyOf = (!attr) ? XPathNavigator.escape_text_chars : XPathNavigator.escape_attr_chars;
			if (value.IndexOfAny(anyOf) < 0)
			{
				return value;
			}
			StringBuilder stringBuilder = new StringBuilder(value, value.Length + 10);
			if (attr)
			{
				stringBuilder.Replace("\"", "&quot;");
			}
			stringBuilder.Replace("<", "&lt;");
			stringBuilder.Replace(">", "&gt;");
			if (attr)
			{
				stringBuilder.Replace("\r\n", "&#10;");
				stringBuilder.Replace("\r", "&#10;");
				stringBuilder.Replace("\n", "&#10;");
			}
			return stringBuilder.ToString();
		}

		/// <summary>Gets or sets the markup representing the child nodes of the current node.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the markup of the child nodes of the current node.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Xml.XPath.XPathNavigator.InnerXml" /> property cannot be set.</exception>
		public virtual string InnerXml
		{
			get
			{
				switch (this.NodeType)
				{
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
					return XPathNavigator.EscapeString(this.Value, true);
				case XPathNodeType.Text:
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					return string.Empty;
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					return this.Value;
				}
				XmlReader xmlReader = this.ReadSubtree();
				xmlReader.Read();
				int num = xmlReader.Depth;
				if (this.NodeType != XPathNodeType.Root)
				{
					xmlReader.Read();
				}
				else
				{
					num = -1;
				}
				StringWriter stringWriter = new StringWriter();
				XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
				{
					Indent = true,
					ConformanceLevel = ConformanceLevel.Fragment,
					OmitXmlDeclaration = true
				});
				while (!xmlReader.EOF && xmlReader.Depth > num)
				{
					xmlWriter.WriteNode(xmlReader, false);
				}
				return stringWriter.ToString();
			}
			set
			{
				this.DeleteChildren();
				if (this.NodeType == XPathNodeType.Attribute)
				{
					this.SetValue(value);
					return;
				}
				this.AppendChild(value);
			}
		}

		/// <summary>Gets a value indicating if the current node represents an XPath node.</summary>
		/// <returns>Always returns true.</returns>
		public sealed override bool IsNode
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets the markup representing the opening and closing tags of the current node and its child nodes.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the markup representing the opening and closing tags of the current node and its child nodes.</returns>
		public virtual string OuterXml
		{
			get
			{
				switch (this.NodeType)
				{
				case XPathNodeType.Attribute:
					return string.Concat(new string[]
					{
						this.Prefix,
						(this.Prefix.Length <= 0) ? string.Empty : ":",
						this.LocalName,
						"=\"",
						XPathNavigator.EscapeString(this.Value, true),
						"\""
					});
				case XPathNodeType.Namespace:
					return string.Concat(new string[]
					{
						"xmlns",
						(this.LocalName.Length <= 0) ? string.Empty : ":",
						this.LocalName,
						"=\"",
						XPathNavigator.EscapeString(this.Value, true),
						"\""
					});
				case XPathNodeType.Text:
					return XPathNavigator.EscapeString(this.Value, false);
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
					return this.Value;
				default:
				{
					XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
					xmlWriterSettings.Indent = true;
					xmlWriterSettings.OmitXmlDeclaration = true;
					xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
					StringBuilder stringBuilder = new StringBuilder();
					using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
					{
						this.WriteSubtree(xmlWriter);
					}
					return stringBuilder.ToString();
				}
				}
			}
			set
			{
				switch (this.NodeType)
				{
				case XPathNodeType.Root:
				case XPathNodeType.Attribute:
				case XPathNodeType.Namespace:
					throw new XmlException("Setting OuterXml Root, Attribute and Namespace is not supported.");
				}
				this.DeleteSelf();
				this.AppendChild(value);
				this.MoveToFirstChild();
			}
		}

		/// <summary>Gets the schema information that has been assigned to the current node as a result of schema validation.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.IXmlSchemaInfo" /> object that contains the schema information for the current node.</returns>
		public virtual IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets the current node as a boxed object of the most appropriate .NET Framework type.</summary>
		/// <returns>The current node as a boxed object of the most appropriate .NET Framework type.</returns>
		public override object TypedValue
		{
			get
			{
				XPathNodeType nodeType = this.NodeType;
				if (nodeType == XPathNodeType.Element || nodeType == XPathNodeType.Attribute)
				{
					if (this.XmlType != null)
					{
						XmlSchemaDatatype datatype = this.XmlType.Datatype;
						if (datatype != null)
						{
							return datatype.ParseValue(this.Value, this.NameTable, this);
						}
					}
				}
				return this.Value;
			}
		}

		/// <summary>Used by <see cref="T:System.Xml.XPath.XPathNavigator" /> implementations which provide a "virtualized" XML view over a store, to provide access to underlying objects.</summary>
		/// <returns>The default is null.</returns>
		public virtual object UnderlyingObject
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets the current node's value as a <see cref="T:System.Boolean" />.</summary>
		/// <returns>The current node's value as a <see cref="T:System.Boolean" />.</returns>
		/// <exception cref="T:System.FormatException">The current node's string value cannot be converted to a <see cref="T:System.Boolean" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Boolean" /> is not valid.</exception>
		public override bool ValueAsBoolean
		{
			get
			{
				return XQueryConvert.StringToBoolean(this.Value);
			}
		}

		/// <summary>Gets the current node's value as a <see cref="T:System.DateTime" />.</summary>
		/// <returns>The current node's value as a <see cref="T:System.DateTime" />.</returns>
		/// <exception cref="T:System.FormatException">The current node's string value cannot be converted to a <see cref="T:System.DateTime" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.DateTime" /> is not valid.</exception>
		public override DateTime ValueAsDateTime
		{
			get
			{
				return XmlConvert.ToDateTime(this.Value);
			}
		}

		/// <summary>Gets the current node's value as a <see cref="T:System.Double" />.</summary>
		/// <returns>The current node's value as a <see cref="T:System.Double" />.</returns>
		/// <exception cref="T:System.FormatException">The current node's string value cannot be converted to a <see cref="T:System.Double" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Double" /> is not valid.</exception>
		public override double ValueAsDouble
		{
			get
			{
				return XQueryConvert.StringToDouble(this.Value);
			}
		}

		/// <summary>Gets the current node's value as an <see cref="T:System.Int32" />.</summary>
		/// <returns>The current node's value as an <see cref="T:System.Int32" />.</returns>
		/// <exception cref="T:System.FormatException">The current node's string value cannot be converted to a <see cref="T:System.Int32" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Int32" /> is not valid.</exception>
		public override int ValueAsInt
		{
			get
			{
				return XQueryConvert.StringToInt(this.Value);
			}
		}

		/// <summary>Gets the current node's value as an <see cref="T:System.Int64" />.</summary>
		/// <returns>The current node's value as an <see cref="T:System.Int64" />.</returns>
		/// <exception cref="T:System.FormatException">The current node's string value cannot be converted to a <see cref="T:System.Int64" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Int64" /> is not valid.</exception>
		public override long ValueAsLong
		{
			get
			{
				return XQueryConvert.StringToInteger(this.Value);
			}
		}

		/// <summary>Gets the .NET Framework <see cref="T:System.Type" /> of the current node.</summary>
		/// <returns>The .NET Framework <see cref="T:System.Type" /> of the current node. The default value is <see cref="T:System.String" />.</returns>
		public override Type ValueType
		{
			get
			{
				return (this.SchemaInfo == null || this.SchemaInfo.SchemaType == null || this.SchemaInfo.SchemaType.Datatype == null) ? null : this.SchemaInfo.SchemaType.Datatype.ValueType;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchemaType" /> information for the current node.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaType" /> object; default is null.</returns>
		public override XmlSchemaType XmlType
		{
			get
			{
				if (this.SchemaInfo != null)
				{
					return this.SchemaInfo.SchemaType;
				}
				return null;
			}
		}

		private XmlReader CreateFragmentReader(string fragment)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(this.NameTable);
			foreach (KeyValuePair<string, string> keyValuePair in this.GetNamespacesInScope(XmlNamespaceScope.All))
			{
				xmlNamespaceManager.AddNamespace(keyValuePair.Key, keyValuePair.Value);
			}
			return XmlReader.Create(new StringReader(fragment), xmlReaderSettings, new XmlParserContext(this.NameTable, xmlNamespaceManager, null, XmlSpace.None));
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlWriter" /> object used to create one or more new child nodes at the end of the list of child nodes of the current node. </summary>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object used to create new child nodes at the end of the list of child nodes of the current node.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on is not the root node or an element node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual XmlWriter AppendChild()
		{
			throw new NotSupportedException();
		}

		/// <summary>Creates a new child node at the end of the list of child nodes of the current node using the XML data string specified.</summary>
		/// <param name="newChild">The XML data string for the new child node.</param>
		/// <exception cref="T:System.ArgumentNullException">The XML data string parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on is not the root node or an element node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML data string parameter is not well-formed.</exception>
		public virtual void AppendChild(string xmlFragments)
		{
			this.AppendChild(this.CreateFragmentReader(xmlFragments));
		}

		/// <summary>Creates a new child node at the end of the list of child nodes of the current node using the XML contents of the <see cref="T:System.Xml.XmlReader" /> object specified.</summary>
		/// <param name="newChild">An <see cref="T:System.Xml.XmlReader" /> object positioned on the XML data for the new child node.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.XmlReader" /> object is in an error state or closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on is not the root node or an element node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML contents of the <see cref="T:System.Xml.XmlReader" /> object parameter is not well-formed.</exception>
		public virtual void AppendChild(XmlReader reader)
		{
			XmlWriter xmlWriter = this.AppendChild();
			while (!reader.EOF)
			{
				xmlWriter.WriteNode(reader, false);
			}
			xmlWriter.Close();
		}

		/// <summary>Creates a new child node at the end of the list of child nodes of the current node using the nodes in the <see cref="T:System.Xml.XPath.XPathNavigator" /> specified.</summary>
		/// <param name="newChild">An <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the node to add as the new child node.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on is not the root node or an element node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void AppendChild(XPathNavigator nav)
		{
			this.AppendChild(new XPathNavigatorReader(nav));
		}

		/// <summary>Creates a new child element node at the end of the list of child nodes of the current node using the namespace prefix, local name and namespace URI specified with the value specified.</summary>
		/// <param name="prefix">The namespace prefix of the new child element node (if any).</param>
		/// <param name="localName">The local name of the new child element node (if any).</param>
		/// <param name="namespaceURI">The namespace URI of the new child element node (if any). <see cref="F:System.String.Empty" /> and null are equivalent.</param>
		/// <param name="value">The value of the new child element node. If <see cref="F:System.String.Empty" /> or null are passed, an empty element is created.</param>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on is not the root node or an element node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void AppendChildElement(string prefix, string name, string ns, string value)
		{
			XmlWriter xmlWriter = this.AppendChild();
			xmlWriter.WriteStartElement(prefix, name, ns);
			xmlWriter.WriteString(value);
			xmlWriter.WriteEndElement();
			xmlWriter.Close();
		}

		/// <summary>Creates an attribute node on the current element node using the namespace prefix, local name and namespace URI specified with the value specified.</summary>
		/// <param name="prefix">The namespace prefix of the new attribute node (if any).</param>
		/// <param name="localName">The local name of the new attribute node which cannot <see cref="F:System.String.Empty" /> or null.</param>
		/// <param name="namespaceURI">The namespace URI for the new attribute node (if any).</param>
		/// <param name="value">The value of the new attribute node. If <see cref="F:System.String.Empty" /> or null are passed, an empty attribute node is created.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void CreateAttribute(string prefix, string localName, string namespaceURI, string value)
		{
			using (XmlWriter xmlWriter = this.CreateAttributes())
			{
				xmlWriter.WriteAttributeString(prefix, localName, namespaceURI, value);
			}
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlWriter" /> object used to create new attributes on the current element.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object used to create new attributes on the current element.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual XmlWriter CreateAttributes()
		{
			throw new NotSupportedException();
		}

		/// <summary>Deletes the current node and its child nodes.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on a node that cannot be deleted such as the root node or a namespace node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void DeleteSelf()
		{
			throw new NotSupportedException();
		}

		/// <summary>Deletes a range of sibling nodes from the current node to the node specified.</summary>
		/// <param name="lastSiblingToDelete">An <see cref="T:System.Xml.XPath.XPathNavigator" /> positioned on the last sibling node in the range to delete.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> specified is null.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.InvalidOperationException">The last node to delete specified is not a valid sibling node of the current node.</exception>
		public virtual void DeleteRange(XPathNavigator nav)
		{
			throw new NotSupportedException();
		}

		/// <summary>Replaces a range of sibling nodes from the current node to the node specified.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object used to specify the replacement range.</returns>
		/// <param name="lastSiblingToReplace">An <see cref="T:System.Xml.XPath.XPathNavigator" /> positioned on the last sibling node in the range to replace.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> specified is null.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.InvalidOperationException">The last node to replace specified is not a valid sibling node of the current node.</exception>
		public virtual XmlWriter ReplaceRange(XPathNavigator nav)
		{
			throw new NotSupportedException();
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlWriter" /> object used to create a new sibling node after the currently selected node.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object used to create a new sibling node after the currently selected node.</returns>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted after the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual XmlWriter InsertAfter()
		{
			switch (this.NodeType)
			{
			case XPathNodeType.Root:
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
				throw new InvalidOperationException(string.Format("Insertion after {0} is not allowed.", this.NodeType));
			}
			XPathNavigator xpathNavigator = this.Clone();
			if (xpathNavigator.MoveToNext())
			{
				return xpathNavigator.InsertBefore();
			}
			if (xpathNavigator.MoveToParent())
			{
				return xpathNavigator.AppendChild();
			}
			throw new InvalidOperationException("Could not move to parent to insert sibling node");
		}

		/// <summary>Creates a new sibling node after the currently selected node using the XML string specified.</summary>
		/// <param name="newSibling">The XML data string for the new sibling node.</param>
		/// <exception cref="T:System.ArgumentNullException">The XML string parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted after the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML string parameter is not well-formed.</exception>
		public virtual void InsertAfter(string xmlFragments)
		{
			this.InsertAfter(this.CreateFragmentReader(xmlFragments));
		}

		/// <summary>Creates a new sibling node after the currently selected node using the XML contents of the <see cref="T:System.Xml.XmlReader" /> object specified.</summary>
		/// <param name="newSibling">An <see cref="T:System.Xml.XmlReader" /> object positioned on the XML data for the new sibling node.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.XmlReader" /> object is in an error state or closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted after the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML contents of the <see cref="T:System.Xml.XmlReader" /> object parameter is not well-formed.</exception>
		public virtual void InsertAfter(XmlReader reader)
		{
			using (XmlWriter xmlWriter = this.InsertAfter())
			{
				xmlWriter.WriteNode(reader, false);
			}
		}

		/// <summary>Creates a new sibling node after the currently selected node using the nodes in the <see cref="T:System.Xml.XPath.XPathNavigator" /> object specified.</summary>
		/// <param name="newSibling">An <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the node to add as the new sibling node.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted after the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void InsertAfter(XPathNavigator nav)
		{
			this.InsertAfter(new XPathNavigatorReader(nav));
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlWriter" /> object used to create a new sibling node before the currently selected node.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object used to create a new sibling node before the currently selected node.</returns>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted before the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual XmlWriter InsertBefore()
		{
			throw new NotSupportedException();
		}

		/// <summary>Creates a new sibling node before the currently selected node using the XML string specified.</summary>
		/// <param name="newSibling">The XML data string for the new sibling node.</param>
		/// <exception cref="T:System.ArgumentNullException">The XML string parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted before the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML string parameter is not well-formed.</exception>
		public virtual void InsertBefore(string xmlFragments)
		{
			this.InsertBefore(this.CreateFragmentReader(xmlFragments));
		}

		/// <summary>Creates a new sibling node before the currently selected node using the XML contents of the <see cref="T:System.Xml.XmlReader" /> object specified.</summary>
		/// <param name="newSibling">An <see cref="T:System.Xml.XmlReader" /> object positioned on the XML data for the new sibling node.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.XmlReader" /> object is in an error state or closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted before the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML contents of the <see cref="T:System.Xml.XmlReader" /> object parameter is not well-formed.</exception>
		public virtual void InsertBefore(XmlReader reader)
		{
			using (XmlWriter xmlWriter = this.InsertBefore())
			{
				xmlWriter.WriteNode(reader, false);
			}
		}

		/// <summary>Creates a new sibling node before the currently selected node using the nodes in the <see cref="T:System.Xml.XPath.XPathNavigator" /> specified.</summary>
		/// <param name="newSibling">An <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the node to add as the new sibling node.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted before the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void InsertBefore(XPathNavigator nav)
		{
			this.InsertBefore(new XPathNavigatorReader(nav));
		}

		/// <summary>Creates a new sibling element after the current node using the namespace prefix, local name and namespace URI specified, with the value specified.</summary>
		/// <param name="prefix">The namespace prefix of the new child element (if any).</param>
		/// <param name="localName">The local name of the new child element (if any).</param>
		/// <param name="namespaceURI">The namespace URI of the new child element (if any). <see cref="F:System.String.Empty" /> and null are equivalent.</param>
		/// <param name="value">The value of the new child element. If <see cref="F:System.String.Empty" /> or null are passed, an empty element is created.</param>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted after the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void InsertElementAfter(string prefix, string localName, string namespaceURI, string value)
		{
			using (XmlWriter xmlWriter = this.InsertAfter())
			{
				xmlWriter.WriteElementString(prefix, localName, namespaceURI, value);
			}
		}

		/// <summary>Creates a new sibling element before the current node using the namespace prefix, local name, and namespace URI specified, with the value specified.</summary>
		/// <param name="prefix">The namespace prefix of the new child element (if any).</param>
		/// <param name="localName">The local name of the new child element (if any).</param>
		/// <param name="namespaceURI">The namespace URI of the new child element (if any). <see cref="F:System.String.Empty" /> and null are equivalent.</param>
		/// <param name="value">The value of the new child element. If <see cref="F:System.String.Empty" /> or null are passed, an empty element is created.</param>
		/// <exception cref="T:System.InvalidOperationException">The position of the <see cref="T:System.Xml.XPath.XPathNavigator" /> does not allow a new sibling node to be inserted before the current node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void InsertElementBefore(string prefix, string localName, string namespaceURI, string value)
		{
			using (XmlWriter xmlWriter = this.InsertBefore())
			{
				xmlWriter.WriteElementString(prefix, localName, namespaceURI, value);
			}
		}

		/// <summary>Returns an <see cref="T:System.Xml.XmlWriter" /> object used to create a new child node at the beginning of the list of child nodes of the current node.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlWriter" /> object used to create a new child node at the beginning of the list of child nodes of the current node.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on does not allow a new child node to be prepended.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual XmlWriter PrependChild()
		{
			XPathNavigator xpathNavigator = this.Clone();
			if (xpathNavigator.MoveToFirstChild())
			{
				return xpathNavigator.InsertBefore();
			}
			return this.AppendChild();
		}

		/// <summary>Creates a new child node at the beginning of the list of child nodes of the current node using the XML string specified.</summary>
		/// <param name="newChild">The XML data string for the new child node.</param>
		/// <exception cref="T:System.ArgumentNullException">The XML string parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on does not allow a new child node to be prepended.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML string parameter is not well-formed.</exception>
		public virtual void PrependChild(string xmlFragments)
		{
			this.PrependChild(this.CreateFragmentReader(xmlFragments));
		}

		/// <summary>Creates a new child node at the beginning of the list of child nodes of the current node using the XML contents of the <see cref="T:System.Xml.XmlReader" /> object specified.</summary>
		/// <param name="newChild">An <see cref="T:System.Xml.XmlReader" /> object positioned on the XML data for the new child node.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.XmlReader" /> object is in an error state or closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on does not allow a new child node to be prepended.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML contents of the <see cref="T:System.Xml.XmlReader" /> object parameter is not well-formed.</exception>
		public virtual void PrependChild(XmlReader reader)
		{
			using (XmlWriter xmlWriter = this.PrependChild())
			{
				xmlWriter.WriteNode(reader, false);
			}
		}

		/// <summary>Creates a new child node at the beginning of the list of child nodes of the current node using the nodes in the <see cref="T:System.Xml.XPath.XPathNavigator" /> object specified.</summary>
		/// <param name="newChild">An <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the node to add as the new child node.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on does not allow a new child node to be prepended.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void PrependChild(XPathNavigator nav)
		{
			this.PrependChild(new XPathNavigatorReader(nav));
		}

		/// <summary>Creates a new child element at the beginning of the list of child nodes of the current node using the namespace prefix, local name, and namespace URI specified with the value specified.</summary>
		/// <param name="prefix">The namespace prefix of the new child element (if any).</param>
		/// <param name="localName">The local name of the new child element (if any).</param>
		/// <param name="namespaceURI">The namespace URI of the new child element (if any). <see cref="F:System.String.Empty" /> and null are equivalent.</param>
		/// <param name="value">The value of the new child element. If <see cref="F:System.String.Empty" /> or null are passed, an empty element is created.</param>
		/// <exception cref="T:System.InvalidOperationException">The current node the <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on does not allow a new child node to be prepended.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void PrependChildElement(string prefix, string localName, string namespaceURI, string value)
		{
			using (XmlWriter xmlWriter = this.PrependChild())
			{
				xmlWriter.WriteElementString(prefix, localName, namespaceURI, value);
			}
		}

		/// <summary>Replaces the current node with the content of the string specified.</summary>
		/// <param name="newNode">The XML data string for the new node.</param>
		/// <exception cref="T:System.ArgumentNullException">The XML string parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element, text, processing instruction, or comment node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML string parameter is not well-formed.</exception>
		public virtual void ReplaceSelf(string xmlFragment)
		{
			this.ReplaceSelf(this.CreateFragmentReader(xmlFragment));
		}

		/// <summary>Replaces the current node with the contents of the <see cref="T:System.Xml.XmlReader" /> object specified.</summary>
		/// <param name="newNode">An <see cref="T:System.Xml.XmlReader" /> object positioned on the XML data for the new node.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.XmlReader" /> object is in an error state or closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XmlReader" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element, text, processing instruction, or comment node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML contents of the <see cref="T:System.Xml.XmlReader" /> object parameter is not well-formed.</exception>
		public virtual void ReplaceSelf(XmlReader reader)
		{
			throw new NotSupportedException();
		}

		/// <summary>Replaces the current node with the contents of the <see cref="T:System.Xml.XPath.XPathNavigator" /> object specified.</summary>
		/// <param name="newNode">An <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the new node.</param>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> object parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element, text, processing instruction, or comment node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		/// <exception cref="T:System.Xml.XmlException">The XML contents of the <see cref="T:System.Xml.XPath.XPathNavigator" /> object parameter is not well-formed.</exception>
		public virtual void ReplaceSelf(XPathNavigator navigator)
		{
			this.ReplaceSelf(new XPathNavigatorReader(navigator));
		}

		/// <summary>Sets the typed value of the current node.</summary>
		/// <param name="typedValue">The new typed value of the node.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support the type of the object specified.</exception>
		/// <exception cref="T:System.ArgumentNullException">The value specified cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is not positioned on an element or attribute node.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		[MonoTODO]
		public virtual void SetTypedValue(object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>Sets the value of the current node.</summary>
		/// <param name="value">The new value of the node.</param>
		/// <exception cref="T:System.ArgumentNullException">The value parameter is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> is positioned on the root node, a namespace node, or the specified value is invalid.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Xml.XPath.XPathNavigator" /> does not support editing.</exception>
		public virtual void SetValue(string value)
		{
			throw new NotSupportedException();
		}

		private void DeleteChildren()
		{
			switch (this.NodeType)
			{
			case XPathNodeType.Attribute:
				return;
			case XPathNodeType.Namespace:
				throw new InvalidOperationException("Removing namespace node content is not supported.");
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				this.DeleteSelf();
				return;
			default:
			{
				if (!this.HasChildren)
				{
					return;
				}
				XPathNavigator xpathNavigator = this.Clone();
				xpathNavigator.MoveToFirstChild();
				while (!xpathNavigator.IsSamePosition(this))
				{
					xpathNavigator.DeleteSelf();
				}
				return;
			}
			}
		}

		private class EnumerableIterator : XPathNodeIterator
		{
			private IEnumerable source;

			private IEnumerator e;

			private int pos;

			public EnumerableIterator(IEnumerable source, int pos)
			{
				this.source = source;
				for (int i = 0; i < pos; i++)
				{
					this.MoveNext();
				}
			}

			public override XPathNodeIterator Clone()
			{
				return new XPathNavigator.EnumerableIterator(this.source, this.pos);
			}

			public override bool MoveNext()
			{
				if (this.e == null)
				{
					this.e = this.source.GetEnumerator();
				}
				if (!this.e.MoveNext())
				{
					return false;
				}
				this.pos++;
				return true;
			}

			public override int CurrentPosition
			{
				get
				{
					return this.pos;
				}
			}

			public override XPathNavigator Current
			{
				get
				{
					return (this.pos != 0) ? ((XPathNavigator)this.e.Current) : null;
				}
			}
		}
	}
}
