using System;
using System.Collections;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
	internal static class ExpressionCache
	{
		private static readonly Hashtable table_per_ctx = new Hashtable();

		private static object dummy = new object();

		private static object cache_lock = new object();

		public static XPathExpression Get(string xpath, IStaticXsltContext ctx)
		{
			object key = (ctx == null) ? ExpressionCache.dummy : ctx;
			object obj = ExpressionCache.cache_lock;
			lock (obj)
			{
				WeakReference weakReference = ExpressionCache.table_per_ctx[key] as WeakReference;
				if (weakReference == null)
				{
					return null;
				}
				Hashtable hashtable = weakReference.Target as Hashtable;
				if (hashtable == null)
				{
					ExpressionCache.table_per_ctx[key] = null;
					return null;
				}
				weakReference = (hashtable[xpath] as WeakReference);
				if (weakReference != null)
				{
					XPathExpression xpathExpression = weakReference.Target as XPathExpression;
					if (xpathExpression != null)
					{
						return xpathExpression;
					}
					hashtable[xpath] = null;
				}
			}
			return null;
		}

		public static void Set(string xpath, IStaticXsltContext ctx, XPathExpression exp)
		{
			object key = (ctx == null) ? ExpressionCache.dummy : ctx;
			Hashtable hashtable = null;
			object obj = ExpressionCache.cache_lock;
			lock (obj)
			{
				WeakReference weakReference = ExpressionCache.table_per_ctx[key] as WeakReference;
				if (weakReference != null && weakReference.IsAlive)
				{
					hashtable = (Hashtable)weakReference.Target;
				}
				if (hashtable == null)
				{
					hashtable = new Hashtable();
					ExpressionCache.table_per_ctx[key] = new WeakReference(hashtable);
				}
				hashtable[xpath] = new WeakReference(exp);
			}
		}
	}
}
