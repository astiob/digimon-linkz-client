using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Stores channel data for the remoting channels.</summary>
	[ComVisible(true)]
	[Serializable]
	public class ChannelDataStore : IChannelDataStore
	{
		private string[] _channelURIs;

		private DictionaryEntry[] _extraData;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Channels.ChannelDataStore" /> class with the URIs that the current channel maps to.</summary>
		/// <param name="channelURIs">An array of channel URIs that the current channel maps to. </param>
		public ChannelDataStore(string[] channelURIs)
		{
			this._channelURIs = channelURIs;
		}

		/// <summary>Gets or sets an array of channel URIs that the current channel maps to.</summary>
		/// <returns>An array of channel URIs that the current channel maps to.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public string[] ChannelUris
		{
			get
			{
				return this._channelURIs;
			}
			set
			{
				this._channelURIs = value;
			}
		}

		/// <summary>Gets or sets the data object that is associated with the specified key for the implementing channel.</summary>
		/// <returns>The specified data object for the implementing channel.</returns>
		/// <param name="key">The key that the data object is associated with. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public object this[object key]
		{
			get
			{
				if (this._extraData == null)
				{
					return null;
				}
				foreach (DictionaryEntry dictionaryEntry in this._extraData)
				{
					if (dictionaryEntry.Key.Equals(key))
					{
						return dictionaryEntry.Value;
					}
				}
				return null;
			}
			set
			{
				if (this._extraData == null)
				{
					this._extraData = new DictionaryEntry[]
					{
						new DictionaryEntry(key, value)
					};
				}
				else
				{
					DictionaryEntry[] array = new DictionaryEntry[this._extraData.Length + 1];
					this._extraData.CopyTo(array, 0);
					array[this._extraData.Length] = new DictionaryEntry(key, value);
					this._extraData = array;
				}
			}
		}
	}
}
