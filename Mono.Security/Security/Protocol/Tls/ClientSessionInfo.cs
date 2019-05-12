using System;

namespace Mono.Security.Protocol.Tls
{
	internal class ClientSessionInfo : IDisposable
	{
		private const int DefaultValidityInterval = 180;

		private static readonly int ValidityInterval;

		private bool disposed;

		private DateTime validuntil;

		private string host;

		private byte[] sid;

		private byte[] masterSecret;

		public ClientSessionInfo(string hostname, byte[] id)
		{
			this.host = hostname;
			this.sid = id;
			this.KeepAlive();
		}

		static ClientSessionInfo()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("MONO_TLS_SESSION_CACHE_TIMEOUT");
			if (environmentVariable == null)
			{
				ClientSessionInfo.ValidityInterval = 180;
			}
			else
			{
				try
				{
					ClientSessionInfo.ValidityInterval = int.Parse(environmentVariable);
				}
				catch
				{
					ClientSessionInfo.ValidityInterval = 180;
				}
			}
		}

		~ClientSessionInfo()
		{
			this.Dispose(false);
		}

		public string HostName
		{
			get
			{
				return this.host;
			}
		}

		public byte[] Id
		{
			get
			{
				return this.sid;
			}
		}

		public bool Valid
		{
			get
			{
				return this.masterSecret != null && this.validuntil > DateTime.UtcNow;
			}
		}

		public void GetContext(Context context)
		{
			this.CheckDisposed();
			if (context.MasterSecret != null)
			{
				this.masterSecret = (byte[])context.MasterSecret.Clone();
			}
		}

		public void SetContext(Context context)
		{
			this.CheckDisposed();
			if (this.masterSecret != null)
			{
				context.MasterSecret = (byte[])this.masterSecret.Clone();
			}
		}

		public void KeepAlive()
		{
			this.CheckDisposed();
			this.validuntil = DateTime.UtcNow.AddSeconds((double)ClientSessionInfo.ValidityInterval);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.validuntil = DateTime.MinValue;
				this.host = null;
				this.sid = null;
				if (this.masterSecret != null)
				{
					Array.Clear(this.masterSecret, 0, this.masterSecret.Length);
					this.masterSecret = null;
				}
			}
			this.disposed = true;
		}

		private void CheckDisposed()
		{
			if (this.disposed)
			{
				string text = Locale.GetText("Cache session information were disposed.");
				throw new ObjectDisposedException(text);
			}
		}
	}
}
