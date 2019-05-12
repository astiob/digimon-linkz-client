using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Base class for texture handling. Contains functionality that is common to both Texture2D and RenderTexture classes.</para>
	/// </summary>
	public class Texture : Object
	{
		public static extern int masterTextureLimit { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern AnisotropicFiltering anisotropicFiltering { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Sets Anisotropic limits.</para>
		/// </summary>
		/// <param name="forcedMin"></param>
		/// <param name="globalMax"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGlobalAnisotropicFilteringLimits(int forcedMin, int globalMax);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetWidth(Texture mono);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetHeight(Texture mono);

		/// <summary>
		///   <para>Width of the texture in pixels. (Read Only)</para>
		/// </summary>
		public virtual int width
		{
			get
			{
				return Texture.Internal_GetWidth(this);
			}
			set
			{
				throw new Exception("not implemented");
			}
		}

		/// <summary>
		///   <para>Height of the texture in pixels. (Read Only)</para>
		/// </summary>
		public virtual int height
		{
			get
			{
				return Texture.Internal_GetHeight(this);
			}
			set
			{
				throw new Exception("not implemented");
			}
		}

		/// <summary>
		///   <para>Filtering mode of the texture.</para>
		/// </summary>
		public extern FilterMode filterMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Anisotropic filtering level of the texture.</para>
		/// </summary>
		public extern int anisoLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Wrap mode (Repeat or Clamp) of the texture.</para>
		/// </summary>
		public extern TextureWrapMode wrapMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Mip map bias of the texture.</para>
		/// </summary>
		public extern float mipMapBias { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Vector2 texelSize
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_texelSize(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_texelSize(out Vector2 value);

		/// <summary>
		///   <para>Retrieve native ('hardware') pointer to a texture.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetNativeTexturePtr();

		[Obsolete("Use GetNativeTexturePtr instead.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetNativeTextureID();
	}
}
