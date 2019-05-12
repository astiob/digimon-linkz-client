using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace OAuth
{
	public class WebParameterCollection : IEnumerable<WebParameter>, ICollection<WebParameter>, IList<WebParameter>, IEnumerable
	{
		private IList<WebParameter> _parameters;

		public WebParameterCollection(IEnumerable<WebParameter> parameters)
		{
			this._parameters = new List<WebParameter>(parameters);
		}

		public WebParameterCollection(NameValueCollection collection) : this()
		{
			this.AddCollection(collection);
		}

		public WebParameterCollection(IDictionary<string, string> collection) : this()
		{
			this.AddCollection(collection);
		}

		public WebParameterCollection()
		{
			this._parameters = new List<WebParameter>(0);
		}

		public WebParameterCollection(int capacity)
		{
			this._parameters = new List<WebParameter>(capacity);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public virtual WebParameter this[string name]
		{
			get
			{
				IEnumerable<WebParameter> source = this.Where((WebParameter p) => p.Name.Equals(name));
				if (source.Count<WebParameter>() == 0)
				{
					return null;
				}
				if (source.Count<WebParameter>() == 1)
				{
					return source.Single<WebParameter>();
				}
				string value = string.Join(",", source.Select((WebParameter p) => p.Value).ToArray<string>());
				return new WebParameter(name, value);
			}
		}

		public virtual IEnumerable<string> Names
		{
			get
			{
				return this._parameters.Select((WebParameter p) => p.Name);
			}
		}

		public virtual IEnumerable<string> Values
		{
			get
			{
				return this._parameters.Select((WebParameter p) => p.Value);
			}
		}

		public virtual void AddRange(NameValueCollection collection)
		{
			this.AddCollection(collection);
		}

		private void AddCollection(NameValueCollection collection)
		{
			IEnumerable<WebParameter> enumerable = collection.AllKeys.Select((string key) => new WebParameter(key, collection[key]));
			foreach (WebParameter item in enumerable)
			{
				this._parameters.Add(item);
			}
		}

		public void AddCollection(IDictionary<string, string> collection)
		{
			foreach (WebParameter item in collection.Keys.Select((string key) => new WebParameter(key, collection[key])))
			{
				this._parameters.Add(item);
			}
		}

		private void AddCollection(IEnumerable<WebParameter> collection)
		{
			foreach (WebParameter item in collection.Select((WebParameter parameter) => new WebParameter(parameter.Name, parameter.Value)))
			{
				this._parameters.Add(item);
			}
		}

		public virtual void AddRange(WebParameterCollection collection)
		{
			this.AddCollection(collection);
		}

		public virtual void AddRange(IEnumerable<WebParameter> collection)
		{
			this.AddCollection(collection);
		}

		public virtual void Sort(Comparison<WebParameter> comparison)
		{
			List<WebParameter> list = new List<WebParameter>(this._parameters);
			list.Sort(comparison);
			this._parameters = list;
		}

		public virtual bool RemoveAll(IEnumerable<WebParameter> parameters)
		{
			WebParameter[] array = parameters.ToArray<WebParameter>();
			bool flag = array.Aggregate(true, (bool current, WebParameter parameter) => current & this._parameters.Remove(parameter));
			return flag && array.Length > 0;
		}

		public virtual void Add(string name, string value)
		{
			WebParameter item = new WebParameter(name, value);
			this._parameters.Add(item);
		}

		public virtual IEnumerator<WebParameter> GetEnumerator()
		{
			return this._parameters.GetEnumerator();
		}

		public virtual void Add(WebParameter parameter)
		{
			this._parameters.Add(parameter);
		}

		public virtual void Clear()
		{
			this._parameters.Clear();
		}

		public virtual bool Contains(WebParameter parameter)
		{
			return this._parameters.Contains(parameter);
		}

		public virtual void CopyTo(WebParameter[] parameters, int arrayIndex)
		{
			this._parameters.CopyTo(parameters, arrayIndex);
		}

		public virtual bool Remove(WebParameter parameter)
		{
			return this._parameters.Remove(parameter);
		}

		public virtual int Count
		{
			get
			{
				return this._parameters.Count;
			}
		}

		public virtual bool IsReadOnly
		{
			get
			{
				return this._parameters.IsReadOnly;
			}
		}

		public virtual int IndexOf(WebParameter parameter)
		{
			return this._parameters.IndexOf(parameter);
		}

		public virtual void Insert(int index, WebParameter parameter)
		{
			this._parameters.Insert(index, parameter);
		}

		public virtual void RemoveAt(int index)
		{
			this._parameters.RemoveAt(index);
		}

		public virtual WebParameter this[int index]
		{
			get
			{
				return this._parameters[index];
			}
			set
			{
				this._parameters[index] = value;
			}
		}
	}
}
