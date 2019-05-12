using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	internal abstract class Identity
	{
		protected string _objectUri;

		protected IMessageSink _channelSink;

		protected IMessageSink _envoySink;

		private DynamicPropertyCollection _clientDynamicProperties;

		private DynamicPropertyCollection _serverDynamicProperties;

		protected ObjRef _objRef;

		private bool _disposed;

		public Identity(string objectUri)
		{
			this._objectUri = objectUri;
		}

		public abstract ObjRef CreateObjRef(Type requestedType);

		public bool IsFromThisAppDomain
		{
			get
			{
				return this._channelSink == null;
			}
		}

		public IMessageSink ChannelSink
		{
			get
			{
				return this._channelSink;
			}
			set
			{
				this._channelSink = value;
			}
		}

		public IMessageSink EnvoySink
		{
			get
			{
				return this._envoySink;
			}
		}

		public string ObjectUri
		{
			get
			{
				return this._objectUri;
			}
			set
			{
				this._objectUri = value;
			}
		}

		public bool IsConnected
		{
			get
			{
				return this._objectUri != null;
			}
		}

		public bool Disposed
		{
			get
			{
				return this._disposed;
			}
			set
			{
				this._disposed = value;
			}
		}

		public DynamicPropertyCollection ClientDynamicProperties
		{
			get
			{
				if (this._clientDynamicProperties == null)
				{
					this._clientDynamicProperties = new DynamicPropertyCollection();
				}
				return this._clientDynamicProperties;
			}
		}

		public DynamicPropertyCollection ServerDynamicProperties
		{
			get
			{
				if (this._serverDynamicProperties == null)
				{
					this._serverDynamicProperties = new DynamicPropertyCollection();
				}
				return this._serverDynamicProperties;
			}
		}

		public bool HasClientDynamicSinks
		{
			get
			{
				return this._clientDynamicProperties != null && this._clientDynamicProperties.HasProperties;
			}
		}

		public bool HasServerDynamicSinks
		{
			get
			{
				return this._serverDynamicProperties != null && this._serverDynamicProperties.HasProperties;
			}
		}

		public void NotifyClientDynamicSinks(bool start, IMessage req_msg, bool client_site, bool async)
		{
			if (this._clientDynamicProperties != null && this._clientDynamicProperties.HasProperties)
			{
				this._clientDynamicProperties.NotifyMessage(start, req_msg, client_site, async);
			}
		}

		public void NotifyServerDynamicSinks(bool start, IMessage req_msg, bool client_site, bool async)
		{
			if (this._serverDynamicProperties != null && this._serverDynamicProperties.HasProperties)
			{
				this._serverDynamicProperties.NotifyMessage(start, req_msg, client_site, async);
			}
		}
	}
}
