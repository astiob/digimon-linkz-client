using System;
using System.Collections;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting
{
	internal class ProviderData
	{
		internal string Ref;

		internal string Type;

		internal string Id;

		internal Hashtable CustomProperties = new Hashtable();

		internal IList CustomData;

		public void CopyFrom(ProviderData other)
		{
			if (this.Ref == null)
			{
				this.Ref = other.Ref;
			}
			if (this.Id == null)
			{
				this.Id = other.Id;
			}
			if (this.Type == null)
			{
				this.Type = other.Type;
			}
			foreach (object obj in other.CustomProperties)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (!this.CustomProperties.ContainsKey(dictionaryEntry.Key))
				{
					this.CustomProperties[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
			if (other.CustomData != null)
			{
				if (this.CustomData == null)
				{
					this.CustomData = new ArrayList();
				}
				foreach (object obj2 in other.CustomData)
				{
					SinkProviderData value = (SinkProviderData)obj2;
					this.CustomData.Add(value);
				}
			}
		}
	}
}
