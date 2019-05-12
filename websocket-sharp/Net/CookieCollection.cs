using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WebSocketSharp.Net
{
	[Serializable]
	public class CookieCollection : ICollection, IEnumerable
	{
		private List<Cookie> _list;

		private object _sync;

		public CookieCollection()
		{
			this._list = new List<Cookie>();
		}

		internal IList<Cookie> List
		{
			get
			{
				return this._list;
			}
		}

		internal IEnumerable<Cookie> Sorted
		{
			get
			{
				List<Cookie> list = new List<Cookie>(this._list);
				if (list.Count > 1)
				{
					list.Sort(new Comparison<Cookie>(CookieCollection.compareCookieWithinSorted));
				}
				return list;
			}
		}

		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public Cookie this[int index]
		{
			get
			{
				if (index < 0 || index >= this._list.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this._list[index];
			}
		}

		public Cookie this[string name]
		{
			get
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				foreach (Cookie cookie in this.Sorted)
				{
					if (cookie.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
					{
						return cookie;
					}
				}
				return null;
			}
		}

		public object SyncRoot
		{
			get
			{
				object result;
				if ((result = this._sync) == null)
				{
					result = (this._sync = ((ICollection)this._list).SyncRoot);
				}
				return result;
			}
		}

		private static int compareCookieWithinSort(Cookie x, Cookie y)
		{
			return x.Name.Length + x.Value.Length - (y.Name.Length + y.Value.Length);
		}

		private static int compareCookieWithinSorted(Cookie x, Cookie y)
		{
			int num;
			return ((num = x.Version - y.Version) == 0) ? (((num = x.Name.CompareTo(y.Name)) == 0) ? (y.Path.Length - x.Path.Length) : num) : num;
		}

		private static CookieCollection parseRequest(string value)
		{
			CookieCollection cookieCollection = new CookieCollection();
			Cookie cookie = null;
			int num = 0;
			string[] array = CookieCollection.splitCookieHeaderValue(value);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Length != 0)
				{
					if (text.StartsWith("$version", StringComparison.InvariantCultureIgnoreCase))
					{
						num = int.Parse(text.GetValueInternal("=").Trim(new char[]
						{
							'"'
						}));
					}
					else if (text.StartsWith("$path", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Path = text.GetValueInternal("=");
						}
					}
					else if (text.StartsWith("$domain", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Domain = text.GetValueInternal("=");
						}
					}
					else if (text.StartsWith("$port", StringComparison.InvariantCultureIgnoreCase))
					{
						string port = (!text.Equals("$port", StringComparison.InvariantCultureIgnoreCase)) ? text.GetValueInternal("=") : "\"\"";
						if (cookie != null)
						{
							cookie.Port = port;
						}
					}
					else
					{
						if (cookie != null)
						{
							cookieCollection.Add(cookie);
						}
						string value2 = string.Empty;
						int num2 = text.IndexOf('=');
						string name;
						if (num2 == -1)
						{
							name = text;
						}
						else if (num2 == text.Length - 1)
						{
							name = text.Substring(0, num2).TrimEnd(new char[]
							{
								' '
							});
						}
						else
						{
							name = text.Substring(0, num2).TrimEnd(new char[]
							{
								' '
							});
							value2 = text.Substring(num2 + 1).TrimStart(new char[]
							{
								' '
							});
						}
						cookie = new Cookie(name, value2);
						if (num != 0)
						{
							cookie.Version = num;
						}
					}
				}
			}
			if (cookie != null)
			{
				cookieCollection.Add(cookie);
			}
			return cookieCollection;
		}

		private static CookieCollection parseResponse(string value)
		{
			CookieCollection cookieCollection = new CookieCollection();
			Cookie cookie = null;
			string[] array = CookieCollection.splitCookieHeaderValue(value);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Length != 0)
				{
					if (text.StartsWith("version", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Version = int.Parse(text.GetValueInternal("=").Trim(new char[]
							{
								'"'
							}));
						}
					}
					else if (text.StartsWith("expires", StringComparison.InvariantCultureIgnoreCase))
					{
						StringBuilder stringBuilder = new StringBuilder(text.GetValueInternal("="), 32);
						if (i < array.Length - 1)
						{
							stringBuilder.AppendFormat(", {0}", array[++i].Trim());
						}
						DateTime now;
						if (!DateTime.TryParseExact(stringBuilder.ToString(), new string[]
						{
							"ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'",
							"r"
						}, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out now))
						{
							now = DateTime.Now;
						}
						if (cookie != null && cookie.Expires == DateTime.MinValue)
						{
							cookie.Expires = now.ToLocalTime();
						}
					}
					else if (text.StartsWith("max-age", StringComparison.InvariantCultureIgnoreCase))
					{
						int num = int.Parse(text.GetValueInternal("=").Trim(new char[]
						{
							'"'
						}));
						DateTime expires = DateTime.Now.AddSeconds((double)num);
						if (cookie != null)
						{
							cookie.Expires = expires;
						}
					}
					else if (text.StartsWith("path", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Path = text.GetValueInternal("=");
						}
					}
					else if (text.StartsWith("domain", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Domain = text.GetValueInternal("=");
						}
					}
					else if (text.StartsWith("port", StringComparison.InvariantCultureIgnoreCase))
					{
						string port = (!text.Equals("port", StringComparison.InvariantCultureIgnoreCase)) ? text.GetValueInternal("=") : "\"\"";
						if (cookie != null)
						{
							cookie.Port = port;
						}
					}
					else if (text.StartsWith("comment", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Comment = text.GetValueInternal("=").UrlDecode();
						}
					}
					else if (text.StartsWith("commenturl", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.CommentUri = text.GetValueInternal("=").Trim(new char[]
							{
								'"'
							}).ToUri();
						}
					}
					else if (text.StartsWith("discard", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Discard = true;
						}
					}
					else if (text.StartsWith("secure", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.Secure = true;
						}
					}
					else if (text.StartsWith("httponly", StringComparison.InvariantCultureIgnoreCase))
					{
						if (cookie != null)
						{
							cookie.HttpOnly = true;
						}
					}
					else
					{
						if (cookie != null)
						{
							cookieCollection.Add(cookie);
						}
						string value2 = string.Empty;
						int num2 = text.IndexOf('=');
						string name;
						if (num2 == -1)
						{
							name = text;
						}
						else if (num2 == text.Length - 1)
						{
							name = text.Substring(0, num2).TrimEnd(new char[]
							{
								' '
							});
						}
						else
						{
							name = text.Substring(0, num2).TrimEnd(new char[]
							{
								' '
							});
							value2 = text.Substring(num2 + 1).TrimStart(new char[]
							{
								' '
							});
						}
						cookie = new Cookie(name, value2);
					}
				}
			}
			if (cookie != null)
			{
				cookieCollection.Add(cookie);
			}
			return cookieCollection;
		}

		private int searchCookie(Cookie cookie)
		{
			string name = cookie.Name;
			string path = cookie.Path;
			string domain = cookie.Domain;
			int version = cookie.Version;
			for (int i = this._list.Count - 1; i >= 0; i--)
			{
				Cookie cookie2 = this._list[i];
				if (cookie2.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && cookie2.Path.Equals(path, StringComparison.InvariantCulture) && cookie2.Domain.Equals(domain, StringComparison.InvariantCultureIgnoreCase) && cookie2.Version == version)
				{
					return i;
				}
			}
			return -1;
		}

		private static string[] splitCookieHeaderValue(string value)
		{
			return new List<string>(value.SplitHeaderValue(new char[]
			{
				',',
				';'
			})).ToArray();
		}

		internal static CookieCollection Parse(string value, bool response)
		{
			return (!response) ? CookieCollection.parseRequest(value) : CookieCollection.parseResponse(value);
		}

		internal void SetOrRemove(Cookie cookie)
		{
			int num = this.searchCookie(cookie);
			if (num == -1)
			{
				if (!cookie.Expired)
				{
					this._list.Add(cookie);
				}
				return;
			}
			if (!cookie.Expired)
			{
				this._list[num] = cookie;
				return;
			}
			this._list.RemoveAt(num);
		}

		internal void SetOrRemove(CookieCollection cookies)
		{
			IEnumerator enumerator = cookies.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Cookie orRemove = (Cookie)obj;
					this.SetOrRemove(orRemove);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		internal void Sort()
		{
			if (this._list.Count > 1)
			{
				this._list.Sort(new Comparison<Cookie>(CookieCollection.compareCookieWithinSort));
			}
		}

		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			int num = this.searchCookie(cookie);
			if (num == -1)
			{
				this._list.Add(cookie);
				return;
			}
			this._list[num] = cookie;
		}

		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			IEnumerator enumerator = cookies.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Cookie cookie = (Cookie)obj;
					this.Add(cookie);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Must not be less than zero.");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException("Must not be multidimensional.", "array");
			}
			if (array.Length - index < this._list.Count)
			{
				throw new ArgumentException("The number of elements in this collection is greater than the available space of the destination array.");
			}
			if (!array.GetType().GetElementType().IsAssignableFrom(typeof(Cookie)))
			{
				throw new InvalidCastException("The elements in this collection cannot be cast automatically to the type of the destination array.");
			}
			((ICollection)this._list).CopyTo(array, index);
		}

		public void CopyTo(Cookie[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Must not be less than zero.");
			}
			if (array.Length - index < this._list.Count)
			{
				throw new ArgumentException("The number of elements in this collection is greater than the available space of the destination array.");
			}
			this._list.CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return this._list.GetEnumerator();
		}
	}
}
