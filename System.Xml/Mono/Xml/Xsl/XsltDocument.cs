using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XsltDocument : XPathFunction
	{
		private Expression arg0;

		private Expression arg1;

		private XPathNavigator doc;

		private static string VoidBaseUriFlag = "&^)(*&%*^$&$VOID!BASE!URI!";

		public XsltDocument(FunctionArguments args, Compiler c) : base(args)
		{
			if (args == null || (args.Tail != null && args.Tail.Tail != null))
			{
				throw new XPathException("document takes one or two args");
			}
			this.arg0 = args.Arg;
			if (args.Tail != null)
			{
				this.arg1 = args.Tail.Arg;
			}
			this.doc = c.Input.Clone();
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.arg0.Peer && (this.arg1 == null || this.arg1.Peer);
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			string baseUri = null;
			if (this.arg1 != null)
			{
				XPathNodeIterator xpathNodeIterator = this.arg1.EvaluateNodeSet(iter);
				if (xpathNodeIterator.MoveNext())
				{
					baseUri = xpathNodeIterator.Current.BaseURI;
				}
				else
				{
					baseUri = XsltDocument.VoidBaseUriFlag;
				}
			}
			object obj = this.arg0.Evaluate(iter);
			if (obj is XPathNodeIterator)
			{
				return this.GetDocument(iter.NamespaceManager as XsltCompiledContext, (XPathNodeIterator)obj, baseUri);
			}
			return this.GetDocument(iter.NamespaceManager as XsltCompiledContext, (!(obj is IFormattable)) ? obj.ToString() : ((IFormattable)obj).ToString(null, CultureInfo.InvariantCulture), baseUri);
		}

		private Uri Resolve(string thisUri, string baseUri, XslTransformProcessor p)
		{
			XmlResolver resolver = p.Resolver;
			if (resolver == null)
			{
				return null;
			}
			Uri baseUri2 = null;
			if (!object.ReferenceEquals(baseUri, XsltDocument.VoidBaseUriFlag) && baseUri != string.Empty)
			{
				baseUri2 = resolver.ResolveUri(null, baseUri);
			}
			return resolver.ResolveUri(baseUri2, thisUri);
		}

		private XPathNodeIterator GetDocument(XsltCompiledContext xsltContext, XPathNodeIterator itr, string baseUri)
		{
			ArrayList arrayList = new ArrayList();
			try
			{
				Hashtable hashtable = new Hashtable();
				while (itr.MoveNext())
				{
					XPathNavigator xpathNavigator = itr.Current;
					Uri uri = this.Resolve(xpathNavigator.Value, (baseUri == null) ? this.doc.BaseURI : baseUri, xsltContext.Processor);
					if (!hashtable.ContainsKey(uri))
					{
						hashtable.Add(uri, null);
						if (uri != null && uri.ToString() == string.Empty)
						{
							XPathNavigator xpathNavigator2 = this.doc.Clone();
							xpathNavigator2.MoveToRoot();
							arrayList.Add(xpathNavigator2);
						}
						else
						{
							arrayList.Add(xsltContext.Processor.GetDocument(uri));
						}
					}
				}
			}
			catch (Exception)
			{
				arrayList.Clear();
			}
			return new ListIterator(arrayList, xsltContext);
		}

		private XPathNodeIterator GetDocument(XsltCompiledContext xsltContext, string arg0, string baseUri)
		{
			XPathNodeIterator result;
			try
			{
				Uri uri = this.Resolve(arg0, (baseUri == null) ? this.doc.BaseURI : baseUri, xsltContext.Processor);
				XPathNavigator xpathNavigator;
				if (uri != null && uri.ToString() == string.Empty)
				{
					xpathNavigator = this.doc.Clone();
					xpathNavigator.MoveToRoot();
				}
				else
				{
					xpathNavigator = xsltContext.Processor.GetDocument(uri);
				}
				result = new SelfIterator(xpathNavigator, xsltContext);
			}
			catch (Exception)
			{
				result = new ListIterator(new ArrayList(), xsltContext);
			}
			return result;
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"document(",
				this.arg0.ToString(),
				(this.arg1 == null) ? string.Empty : ",",
				(this.arg1 == null) ? string.Empty : this.arg1.ToString(),
				")"
			});
		}
	}
}
