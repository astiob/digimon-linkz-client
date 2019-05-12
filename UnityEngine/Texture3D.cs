using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class for handling 3D Textures, Use this to create.</para>
	/// </summary>
	public sealed class Texture3D : Texture
	{
		/// <summary>
		///   <para>Create a new empty 3D Texture.</para>
		/// </summary>
		/// <param name="width">Width of texture in pixels.</param>
		/// <param name="height">Height of texture in pixels.</param>
		/// <param name="depth">Depth of texture in pixels.</param>
		/// <param name="format">Texture data format.</param>
		/// <param name="mipmap">Should the texture have mipmaps?</param>
		public Texture3D(int width, int height, int depth, TextureFormat format, bool mipmap)
		{
			Texture3D.Internal_Create(this, width, height, depth, format, mipmap);
		}

		/// <summary>
		///   <para>The depth of the texture (Read Only).</para>
		/// </summary>
		public extern int depth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns an array of pixel colors representing one mip level of the 3D texture.</para>
		/// </summary>
		/// <param name="miplevel"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels([DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels()
		{
			int miplevel = 0;
			return this.GetPixels(miplevel);
		}

		/// <summary>
		///   <para>Returns an array of pixel colors representing one mip level of the 3D texture.</para>
		/// </summary>
		/// <param name="miplevel"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32([DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			int miplevel = 0;
			return this.GetPixels32(miplevel);
		}

		/// <summary>
		///   <para>Sets pixel colors of a 3D texture.</para>
		/// </summary>
		/// <param name="colors">The colors to set the pixels to.</param>
		/// <param name="miplevel">The mipmap level to be affected by the new colors.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors)
		{
			int miplevel = 0;
			this.SetPixels(colors, miplevel);
		}

		/// <summary>
		///   <para>Sets pixel colors of a 3D texture.</para>
		/// </summary>
		/// <param name="colors">The colors to set the pixels to.</param>
		/// <param name="miplevel">The mipmap level to be affected by the new colors.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors)
		{
			int miplevel = 0;
			this.SetPixels32(colors, miplevel);
		}

		/// <summary>
		///   <para>Actually apply all previous SetPixels changes.</para>
		/// </summary>
		/// <param name="updateMipmaps"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Apply([DefaultValue("true")] bool updateMipmaps);

		[ExcludeFromDocs]
		public void Apply()
		{
			bool updateMipmaps = true;
			this.Apply(updateMipmaps);
		}

		/// <summary>
		///   <para>The format of the pixel data in the texture (Read Only).</para>
		/// </summary>
		public extern TextureFormat format { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] Texture3D mono, int width, int height, int depth, TextureFormat format, bool mipmap);
	}
}
