using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	[NativeHeader("Modules/UnityWebRequestTexture/Public/DownloadHandlerTexture.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerTexture : DownloadHandler
	{
		private Texture2D mTexture;

		private bool mHasTexture;

		private bool mNonReadable;

		public DownloadHandlerTexture()
		{
			this.InternalCreateTexture(true);
		}

		public DownloadHandlerTexture(bool readable)
		{
			this.InternalCreateTexture(readable);
			this.mNonReadable = !readable;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerTexture obj, bool readable);

		private void InternalCreateTexture(bool readable)
		{
			this.m_Ptr = DownloadHandlerTexture.Create(this, readable);
		}

		protected override byte[] GetData()
		{
			return DownloadHandler.InternalGetByteArray(this);
		}

		public Texture2D texture
		{
			get
			{
				return this.InternalGetTexture();
			}
		}

		private Texture2D InternalGetTexture()
		{
			if (this.mHasTexture)
			{
				if (this.mTexture == null)
				{
					this.mTexture = new Texture2D(2, 2);
					this.mTexture.LoadImage(this.GetData(), this.mNonReadable);
				}
			}
			else if (this.mTexture == null)
			{
				this.mTexture = this.InternalGetTextureNative();
				this.mHasTexture = true;
			}
			return this.mTexture;
		}

		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D InternalGetTextureNative();

		public static Texture2D GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerTexture>(www).texture;
		}
	}
}
