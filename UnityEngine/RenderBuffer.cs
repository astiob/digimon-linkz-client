using System;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>Color or depth buffer part of a RenderTexture.</para>
	/// </summary>
	public struct RenderBuffer
	{
		internal int m_RenderTextureInstanceID;

		internal IntPtr m_BufferPtr;

		internal void SetLoadAction(RenderBufferLoadAction action)
		{
			RenderBufferHelper.SetLoadAction(out this, (int)action);
		}

		internal void SetStoreAction(RenderBufferStoreAction action)
		{
			RenderBufferHelper.SetStoreAction(out this, (int)action);
		}

		internal RenderBufferLoadAction loadAction
		{
			get
			{
				return (RenderBufferLoadAction)RenderBufferHelper.GetLoadAction(out this);
			}
			set
			{
				this.SetLoadAction(value);
			}
		}

		internal RenderBufferStoreAction storeAction
		{
			get
			{
				return (RenderBufferStoreAction)RenderBufferHelper.GetStoreAction(out this);
			}
			set
			{
				this.SetStoreAction(value);
			}
		}

		/// <summary>
		///   <para>Returns native RenderBuffer. Be warned this is not native Texture, but rather pointer to unity struct that can be used with native unity API. Currently such API exists only on iOS.</para>
		/// </summary>
		public IntPtr GetNativeRenderBufferPtr()
		{
			return this.m_BufferPtr;
		}
	}
}
