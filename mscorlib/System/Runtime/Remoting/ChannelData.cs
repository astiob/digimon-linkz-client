using System;
using System.Collections;

namespace System.Runtime.Remoting
{
	internal class ChannelData
	{
		internal string Ref;

		internal string Type;

		internal string Id;

		internal string DelayLoadAsClientChannel;

		private ArrayList _serverProviders = new ArrayList();

		private ArrayList _clientProviders = new ArrayList();

		private Hashtable _customProperties = new Hashtable();

		internal ArrayList ServerProviders
		{
			get
			{
				if (this._serverProviders == null)
				{
					this._serverProviders = new ArrayList();
				}
				return this._serverProviders;
			}
		}

		public ArrayList ClientProviders
		{
			get
			{
				if (this._clientProviders == null)
				{
					this._clientProviders = new ArrayList();
				}
				return this._clientProviders;
			}
		}

		public Hashtable CustomProperties
		{
			get
			{
				if (this._customProperties == null)
				{
					this._customProperties = new Hashtable();
				}
				return this._customProperties;
			}
		}

		public void CopyFrom(ChannelData other)
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
			if (this.DelayLoadAsClientChannel == null)
			{
				this.DelayLoadAsClientChannel = other.DelayLoadAsClientChannel;
			}
			if (other._customProperties != null)
			{
				foreach (object obj in other._customProperties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (!this.CustomProperties.ContainsKey(dictionaryEntry.Key))
					{
						this.CustomProperties[dictionaryEntry.Key] = dictionaryEntry.Value;
					}
				}
			}
			if (this._serverProviders == null && other._serverProviders != null)
			{
				foreach (object obj2 in other._serverProviders)
				{
					ProviderData other2 = (ProviderData)obj2;
					ProviderData providerData = new ProviderData();
					providerData.CopyFrom(other2);
					this.ServerProviders.Add(providerData);
				}
			}
			if (this._clientProviders == null && other._clientProviders != null)
			{
				foreach (object obj3 in other._clientProviders)
				{
					ProviderData other3 = (ProviderData)obj3;
					ProviderData providerData2 = new ProviderData();
					providerData2.CopyFrom(other3);
					this.ClientProviders.Add(providerData2);
				}
			}
		}
	}
}
