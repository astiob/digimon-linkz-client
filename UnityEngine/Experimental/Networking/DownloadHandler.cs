using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.Experimental.Networking
{
	/// <summary>
	///   <para>Manage and process HTTP response body data received from a remote server.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class DownloadHandler : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		internal DownloadHandler()
		{
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalDestroy();

		~DownloadHandler()
		{
			this.InternalDestroy();
		}

		/// <summary>
		///   <para>Signals that this [DownloadHandler] is no longer being used, and should clean up any resources it is using.</para>
		/// </summary>
		public void Dispose()
		{
			this.InternalDestroy();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///   <para>Returns the raw bytes downloaded from the remote server, or null. (Read Only)</para>
		/// </summary>
		public byte[] data
		{
			get
			{
				return this.GetData();
			}
		}

		/// <summary>
		///   <para>Convenience property. Returns the bytes from data interpreted as a UTF8 string. (Read Only)</para>
		/// </summary>
		public string text
		{
			get
			{
				return this.GetText();
			}
		}

		/// <summary>
		///   <para>Callback, invoked when the data property is accessed.</para>
		/// </summary>
		/// <returns>
		///   <para>Byte array to return as the value of the data property.</para>
		/// </returns>
		protected virtual byte[] GetData()
		{
			return null;
		}

		/// <summary>
		///   <para>Callback, invoked when the text property is accessed.</para>
		/// </summary>
		/// <returns>
		///   <para>String to return as the return value of the text property.</para>
		/// </returns>
		protected virtual string GetText()
		{
			byte[] data = this.GetData();
			if (data != null && data.Length > 0)
			{
				return Encoding.UTF8.GetString(data, 0, data.Length);
			}
			return string.Empty;
		}

		/// <summary>
		///   <para>Callback, invoked as data is received from the remote server.</para>
		/// </summary>
		/// <param name="data">A buffer containing unprocessed data, received from the remote server.</param>
		/// <param name="dataLength">The number of bytes in data which are new.</param>
		/// <returns>
		///   <para>True if the download should continue, false to abort.</para>
		/// </returns>
		protected virtual bool ReceiveData(byte[] data, int dataLength)
		{
			return true;
		}

		/// <summary>
		///   <para>Callback, invoked with a Content-Length header is received.</para>
		/// </summary>
		/// <param name="contentLength">The value of the received Content-Length header.</param>
		protected virtual void ReceiveContentLength(int contentLength)
		{
		}

		/// <summary>
		///   <para>Callback, invoked when all data has been received from the remote server.</para>
		/// </summary>
		protected virtual void CompleteContent()
		{
		}

		/// <summary>
		///   <para>Callback, invoked when UnityWebRequest.downloadProgress is accessed.</para>
		/// </summary>
		/// <returns>
		///   <para>The return value for UnityWebRequest.downloadProgress.</para>
		/// </returns>
		protected virtual float GetProgress()
		{
			return 0.5f;
		}
	}
}
