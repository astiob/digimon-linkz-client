using SimpleJson;
using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	internal class SessionValues
	{
		private ICloudService m_CloudService;

		private string m_ValueFileName;

		private IDictionary<string, object> m_Values;

		internal SessionValues(ICloudService cloudService, string valueFileName)
		{
			this.m_CloudService = cloudService;
			this.m_ValueFileName = valueFileName;
			if (!this.Restore())
			{
				this.m_Values = new Dictionary<string, object>();
			}
		}

		public SessionValues(IDictionary<string, object> v)
		{
			this.m_Values = v;
		}

		public SessionValues(string jsonData)
		{
			if (!this.Restore(jsonData))
			{
				this.m_Values = new Dictionary<string, object>();
			}
		}

		public void PutItems(IDictionary<string, object> items)
		{
			this.PutItems(items, true);
		}

		public void PutItems(IDictionary<string, object> items, bool saveOnValueChange)
		{
			if (items.Count == 0)
			{
				return;
			}
			bool flag = saveOnValueChange;
			foreach (KeyValuePair<string, object> keyValuePair in items)
			{
				if (!flag)
				{
					this.m_Values[keyValuePair.Key] = keyValuePair.Value;
				}
				else
				{
					object obj;
					if (!this.m_Values.TryGetValue(keyValuePair.Key, out obj))
					{
						flag = false;
					}
					else if (!keyValuePair.Value.Equals(obj))
					{
						flag = false;
					}
					if (!flag)
					{
						this.m_Values[keyValuePair.Key] = keyValuePair.Value;
					}
				}
			}
			if (saveOnValueChange && !flag)
			{
				this.Save();
			}
		}

		public void PutItem(string name, object value)
		{
			this.PutItem(name, value, true);
		}

		public void PutItem(string name, object value, bool saveOnValueChange)
		{
			object obj;
			if (saveOnValueChange && this.m_Values.TryGetValue(name, out obj) && string.Compare(obj.ToString(), value.ToString()) == 0)
			{
				return;
			}
			this.m_Values[name] = value;
			if (saveOnValueChange)
			{
				this.Save();
			}
		}

		public bool TryGetBool(string name, bool defaultValue)
		{
			return this.TryGetValue<bool>(name, defaultValue);
		}

		public int TryGetInt(string name, int defaultValue)
		{
			int num = this.TryGetValue<int>(name, defaultValue);
			if (num != defaultValue)
			{
				return num;
			}
			return Convert.ToInt32(this.TryGetValue<long>(name, (long)defaultValue));
		}

		public long TryGetLong(string name, long defaultValue)
		{
			return this.TryGetValue<long>(name, defaultValue);
		}

		public string TryGetString(string name, string defaultValue)
		{
			return this.TryGetValue<string>(name, defaultValue);
		}

		public List<string> TryGetStringList(string name, List<string> defaultValue)
		{
			return this.TryGetValueList<string>(name, defaultValue);
		}

		public string[] TryGetStringArray(string name, string[] defaultValue)
		{
			return this.TryGetValueArray<string>(name, defaultValue);
		}

		public List<int> TryGetIntList(string name, List<int> defaultValue)
		{
			return this.TryGetValueList<int>(name, defaultValue);
		}

		public int[] TryGetIntArray(string name, int[] defaultValue)
		{
			int[] array = this.TryGetValueArray<int>(name, defaultValue);
			if (array != defaultValue)
			{
				return array;
			}
			long[] array2 = this.TryGetValueArray<long>(name, null);
			if (array2 == null)
			{
				return defaultValue;
			}
			return Array.ConvertAll<long, int>(array2, (long val) => (int)val);
		}

		public SessionValues TryGetValue(string name)
		{
			IDictionary<string, object> dictionary = this.TryGetValue<IDictionary<string, object>>(name, null);
			if (dictionary == null)
			{
				return null;
			}
			return new SessionValues(dictionary);
		}

		private T TryGetValue<T>(string name, T defaultValue)
		{
			object obj = null;
			if (!this.m_Values.TryGetValue(name, out obj))
			{
				return defaultValue;
			}
			try
			{
				return (T)((object)obj);
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			return defaultValue;
		}

		private List<T> TryGetValueList<T>(string name, List<T> defaultValue)
		{
			List<object> list = this.TryGetValue<List<object>>(name, null);
			if (list == null)
			{
				return defaultValue;
			}
			List<T> list2 = new List<T>(list.Count);
			try
			{
				foreach (object obj in list)
				{
					T item = (T)((object)obj);
					list2.Add(item);
				}
				return list2;
			}
			catch (InvalidCastException)
			{
			}
			catch (NullReferenceException)
			{
			}
			return defaultValue;
		}

		private T[] TryGetValueArray<T>(string name, T[] defaultValue)
		{
			List<T> list = this.TryGetValueList<T>(name, null);
			if (list == null)
			{
				return defaultValue;
			}
			return list.ToArray();
		}

		private bool Restore(string jsonData)
		{
			object obj = null;
			if (!SimpleJson.TryDeserializeObject(jsonData, out obj))
			{
				return false;
			}
			try
			{
				this.m_Values = (IDictionary<string, object>)obj;
				if (this.m_Values != null)
				{
					return true;
				}
			}
			catch (InvalidCastException)
			{
			}
			return false;
		}

		private bool Restore()
		{
			return this.Restore(this.m_CloudService.RestoreFile(this.m_ValueFileName));
		}

		private bool Save()
		{
			string data = SimpleJson.SerializeObject(this.m_Values);
			return this.m_CloudService.SaveFile(this.m_ValueFileName, data);
		}
	}
}
