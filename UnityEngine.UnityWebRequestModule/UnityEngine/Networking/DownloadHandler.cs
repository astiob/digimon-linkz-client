using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandler.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class DownloadHandler : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		internal DownloadHandler()
		{
		}

		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Release();

		~DownloadHandler()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				this.Release();
				this.m_Ptr = IntPtr.Zero;
			}
		}

		public bool isDone
		{
			get
			{
				return this.IsDone();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsDone();

		public byte[] data
		{
			get
			{
				return this.GetData();
			}
		}

		public string text
		{
			get
			{
				return this.GetText();
			}
		}

		protected virtual byte[] GetData()
		{
			return null;
		}

		protected virtual string GetText()
		{
			byte[] data = this.GetData();
			string result;
			if (data != null && data.Length > 0)
			{
				result = this.GetTextEncoder().GetString(data, 0, data.Length);
			}
			else
			{
				result = "";
			}
			return result;
		}

		private Encoding GetTextEncoder()
		{
			string contentType = this.GetContentType();
			if (!string.IsNullOrEmpty(contentType))
			{
				int num = contentType.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
				if (num > -1)
				{
					int num2 = contentType.IndexOf('=', num);
					if (num2 > -1)
					{
						string text = contentType.Substring(num2 + 1).Trim().Trim(new char[]
						{
							'\'',
							'"'
						}).Trim();
						int num3 = text.IndexOf(';');
						if (num3 > -1)
						{
							text = text.Substring(0, num3);
						}
						try
						{
							return Encoding.GetEncoding(text);
						}
						catch (ArgumentException ex)
						{
							Debug.LogWarning(string.Format("Unsupported encoding '{0}': {1}", text, ex.Message));
						}
					}
				}
			}
			return Encoding.UTF8;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetContentType();

		[UsedByNativeCode]
		protected virtual bool ReceiveData(byte[] data, int dataLength)
		{
			return true;
		}

		[UsedByNativeCode]
		protected virtual void ReceiveContentLength(int contentLength)
		{
		}

		[UsedByNativeCode]
		protected virtual void CompleteContent()
		{
		}

		[UsedByNativeCode]
		protected virtual float GetProgress()
		{
			return 0f;
		}

		protected static T GetCheckedDownloader<T>(UnityWebRequest www) where T : DownloadHandler
		{
			if (www == null)
			{
				throw new NullReferenceException("Cannot get content from a null UnityWebRequest object");
			}
			if (!www.isDone)
			{
				throw new InvalidOperationException("Cannot get content from an unfinished UnityWebRequest object");
			}
			if (www.isNetworkError)
			{
				throw new InvalidOperationException(www.error);
			}
			return (T)((object)www.downloadHandler);
		}

		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] InternalGetByteArray(DownloadHandler dh);
	}
}
