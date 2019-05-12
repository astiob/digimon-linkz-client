using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace WebSocketSharp.Net
{
	public sealed class HttpListener : IDisposable
	{
		private AuthenticationSchemes _authSchemes;

		private AuthenticationSchemeSelector _authSchemeSelector;

		private string _certFolderPath;

		private Dictionary<HttpConnection, HttpConnection> _connections;

		private List<HttpListenerContext> _contextQueue;

		private Func<IIdentity, NetworkCredential> _credentialsFinder;

		private X509Certificate2 _defaultCert;

		private bool _disposed;

		private bool _ignoreWriteExceptions;

		private bool _listening;

		private HttpListenerPrefixCollection _prefixes;

		private string _realm;

		private Dictionary<HttpListenerContext, HttpListenerContext> _registry;

		private List<ListenerAsyncResult> _waitQueue;

		public HttpListener()
		{
			this._authSchemes = AuthenticationSchemes.Anonymous;
			this._connections = new Dictionary<HttpConnection, HttpConnection>();
			this._contextQueue = new List<HttpListenerContext>();
			this._prefixes = new HttpListenerPrefixCollection(this);
			this._registry = new Dictionary<HttpListenerContext, HttpListenerContext>();
			this._waitQueue = new List<ListenerAsyncResult>();
		}

		void IDisposable.Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this.close(true);
			this._disposed = true;
		}

		internal bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		public AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				this.CheckDisposed();
				return this._authSchemes;
			}
			set
			{
				this.CheckDisposed();
				this._authSchemes = value;
			}
		}

		public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate
		{
			get
			{
				this.CheckDisposed();
				return this._authSchemeSelector;
			}
			set
			{
				this.CheckDisposed();
				this._authSchemeSelector = value;
			}
		}

		public string CertificateFolderPath
		{
			get
			{
				this.CheckDisposed();
				return (this._certFolderPath != null && this._certFolderPath.Length != 0) ? this._certFolderPath : (this._certFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			}
			set
			{
				this.CheckDisposed();
				this._certFolderPath = value;
			}
		}

		public X509Certificate2 DefaultCertificate
		{
			get
			{
				this.CheckDisposed();
				return this._defaultCert;
			}
			set
			{
				this.CheckDisposed();
				this._defaultCert = value;
			}
		}

		public bool IgnoreWriteExceptions
		{
			get
			{
				this.CheckDisposed();
				return this._ignoreWriteExceptions;
			}
			set
			{
				this.CheckDisposed();
				this._ignoreWriteExceptions = value;
			}
		}

		public bool IsListening
		{
			get
			{
				return this._listening;
			}
		}

		public static bool IsSupported
		{
			get
			{
				return true;
			}
		}

		public HttpListenerPrefixCollection Prefixes
		{
			get
			{
				this.CheckDisposed();
				return this._prefixes;
			}
		}

		public string Realm
		{
			get
			{
				this.CheckDisposed();
				return (this._realm != null && this._realm.Length != 0) ? this._realm : (this._realm = "SECRET AREA");
			}
			set
			{
				this.CheckDisposed();
				this._realm = value;
			}
		}

		public bool UnsafeConnectionNtlmAuthentication
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public Func<IIdentity, NetworkCredential> UserCredentialsFinder
		{
			get
			{
				this.CheckDisposed();
				Func<IIdentity, NetworkCredential> result;
				if ((result = this._credentialsFinder) == null)
				{
					result = (this._credentialsFinder = ((IIdentity identity) => null));
				}
				return result;
			}
			set
			{
				this.CheckDisposed();
				this._credentialsFinder = value;
			}
		}

		private void cleanup(bool force)
		{
			object syncRoot = ((ICollection)this._registry).SyncRoot;
			lock (syncRoot)
			{
				if (!force)
				{
					this.sendServiceUnavailable();
				}
				this.cleanupContextRegistry();
				this.cleanupConnections();
				this.cleanupWaitQueue();
			}
		}

		private void cleanupConnections()
		{
			object syncRoot = ((ICollection)this._connections).SyncRoot;
			lock (syncRoot)
			{
				if (this._connections.Count != 0)
				{
					Dictionary<HttpConnection, HttpConnection>.KeyCollection keys = this._connections.Keys;
					HttpConnection[] array = new HttpConnection[keys.Count];
					keys.CopyTo(array, 0);
					this._connections.Clear();
					for (int i = array.Length - 1; i >= 0; i--)
					{
						array[i].Close(true);
					}
				}
			}
		}

		private void cleanupContextRegistry()
		{
			object syncRoot = ((ICollection)this._registry).SyncRoot;
			lock (syncRoot)
			{
				if (this._registry.Count != 0)
				{
					Dictionary<HttpListenerContext, HttpListenerContext>.KeyCollection keys = this._registry.Keys;
					HttpListenerContext[] array = new HttpListenerContext[keys.Count];
					keys.CopyTo(array, 0);
					this._registry.Clear();
					for (int i = array.Length - 1; i >= 0; i--)
					{
						array[i].Connection.Close(true);
					}
				}
			}
		}

		private void cleanupWaitQueue()
		{
			object syncRoot = ((ICollection)this._waitQueue).SyncRoot;
			lock (syncRoot)
			{
				if (this._waitQueue.Count != 0)
				{
					ObjectDisposedException exception = new ObjectDisposedException(base.GetType().ToString());
					foreach (ListenerAsyncResult listenerAsyncResult in this._waitQueue)
					{
						listenerAsyncResult.Complete(exception);
					}
					this._waitQueue.Clear();
				}
			}
		}

		private void close(bool force)
		{
			EndPointManager.RemoveListener(this);
			this.cleanup(force);
		}

		private HttpListenerContext getContextFromQueue()
		{
			if (this._contextQueue.Count == 0)
			{
				return null;
			}
			HttpListenerContext result = this._contextQueue[0];
			this._contextQueue.RemoveAt(0);
			return result;
		}

		private void sendServiceUnavailable()
		{
			object syncRoot = ((ICollection)this._contextQueue).SyncRoot;
			lock (syncRoot)
			{
				if (this._contextQueue.Count != 0)
				{
					HttpListenerContext[] array = this._contextQueue.ToArray();
					this._contextQueue.Clear();
					foreach (HttpListenerContext httpListenerContext in array)
					{
						HttpListenerResponse response = httpListenerContext.Response;
						response.StatusCode = 503;
						response.Close();
					}
				}
			}
		}

		internal void AddConnection(HttpConnection connection)
		{
			this._connections[connection] = connection;
		}

		internal ListenerAsyncResult BeginGetContext(ListenerAsyncResult asyncResult)
		{
			this.CheckDisposed();
			if (this._prefixes.Count == 0)
			{
				throw new InvalidOperationException("Please, call AddPrefix before using this method.");
			}
			if (!this._listening)
			{
				throw new InvalidOperationException("Please, call Start before using this method.");
			}
			object syncRoot = ((ICollection)this._waitQueue).SyncRoot;
			lock (syncRoot)
			{
				object syncRoot2 = ((ICollection)this._contextQueue).SyncRoot;
				lock (syncRoot2)
				{
					HttpListenerContext contextFromQueue = this.getContextFromQueue();
					if (contextFromQueue != null)
					{
						asyncResult.Complete(contextFromQueue, true);
						return asyncResult;
					}
				}
				this._waitQueue.Add(asyncResult);
			}
			return asyncResult;
		}

		internal void CheckDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
		}

		internal void RegisterContext(HttpListenerContext context)
		{
			object syncRoot = ((ICollection)this._registry).SyncRoot;
			lock (syncRoot)
			{
				this._registry[context] = context;
			}
			ListenerAsyncResult listenerAsyncResult = null;
			object syncRoot2 = ((ICollection)this._waitQueue).SyncRoot;
			lock (syncRoot2)
			{
				if (this._waitQueue.Count == 0)
				{
					object syncRoot3 = ((ICollection)this._contextQueue).SyncRoot;
					lock (syncRoot3)
					{
						this._contextQueue.Add(context);
					}
				}
				else
				{
					listenerAsyncResult = this._waitQueue[0];
					this._waitQueue.RemoveAt(0);
				}
			}
			if (listenerAsyncResult != null)
			{
				listenerAsyncResult.Complete(context);
			}
		}

		internal void RemoveConnection(HttpConnection connection)
		{
			this._connections.Remove(connection);
		}

		internal AuthenticationSchemes SelectAuthenticationScheme(HttpListenerContext context)
		{
			return (this.AuthenticationSchemeSelectorDelegate == null) ? this._authSchemes : this.AuthenticationSchemeSelectorDelegate(context.Request);
		}

		internal void UnregisterContext(HttpListenerContext context)
		{
			object syncRoot = ((ICollection)this._registry).SyncRoot;
			lock (syncRoot)
			{
				this._registry.Remove(context);
			}
			object syncRoot2 = ((ICollection)this._contextQueue).SyncRoot;
			lock (syncRoot2)
			{
				int num = this._contextQueue.IndexOf(context);
				if (num >= 0)
				{
					this._contextQueue.RemoveAt(num);
				}
			}
		}

		public void Abort()
		{
			if (this._disposed)
			{
				return;
			}
			this.close(true);
			this._disposed = true;
		}

		public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
		{
			return this.BeginGetContext(new ListenerAsyncResult(callback, state));
		}

		public void Close()
		{
			if (this._disposed)
			{
				return;
			}
			this.close(false);
			this._disposed = true;
		}

		public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
		{
			this.CheckDisposed();
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			ListenerAsyncResult listenerAsyncResult = asyncResult as ListenerAsyncResult;
			if (listenerAsyncResult == null)
			{
				throw new ArgumentException("Wrong IAsyncResult.", "asyncResult");
			}
			if (listenerAsyncResult.EndCalled)
			{
				throw new InvalidOperationException("Cannot reuse this IAsyncResult.");
			}
			listenerAsyncResult.EndCalled = true;
			if (!listenerAsyncResult.IsCompleted)
			{
				listenerAsyncResult.AsyncWaitHandle.WaitOne();
			}
			object syncRoot = ((ICollection)this._waitQueue).SyncRoot;
			lock (syncRoot)
			{
				int num = this._waitQueue.IndexOf(listenerAsyncResult);
				if (num >= 0)
				{
					this._waitQueue.RemoveAt(num);
				}
			}
			HttpListenerContext context = listenerAsyncResult.GetContext();
			AuthenticationSchemes authenticationSchemes = this.SelectAuthenticationScheme(context);
			if (authenticationSchemes != AuthenticationSchemes.Anonymous)
			{
				context.SetUser(authenticationSchemes, this.Realm, this.UserCredentialsFinder);
			}
			return context;
		}

		public HttpListenerContext GetContext()
		{
			ListenerAsyncResult listenerAsyncResult = this.BeginGetContext(new ListenerAsyncResult(null, null));
			listenerAsyncResult.InGet = true;
			return this.EndGetContext(listenerAsyncResult);
		}

		public void Start()
		{
			this.CheckDisposed();
			if (this._listening)
			{
				return;
			}
			EndPointManager.AddListener(this);
			this._listening = true;
		}

		public void Stop()
		{
			this.CheckDisposed();
			if (!this._listening)
			{
				return;
			}
			this._listening = false;
			EndPointManager.RemoveListener(this);
			this.sendServiceUnavailable();
		}
	}
}
