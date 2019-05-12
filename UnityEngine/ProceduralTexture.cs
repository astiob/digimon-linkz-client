using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class for ProceduralTexture handling.</para>
	/// </summary>
	public sealed class ProceduralTexture : Texture
	{
		/// <summary>
		///   <para>The output type of this ProceduralTexture.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ProceduralOutputType GetProceduralOutputType();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern ProceduralMaterial GetProceduralMaterial();

		/// <summary>
		///   <para>Check whether the ProceduralMaterial that generates this ProceduralTexture is set to an output format with an alpha channel.</para>
		/// </summary>
		public extern bool hasAlpha { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasBeenGenerated();

		/// <summary>
		///   <para>The format of the pixel data in the texture (Read Only).</para>
		/// </summary>
		public extern TextureFormat format { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Grab pixel values from a ProceduralTexture.
		/// </para>
		/// </summary>
		/// <param name="x">X-coord of the top-left corner of the rectangle to grab.</param>
		/// <param name="y">Y-coord of the top-left corner of the rectangle to grab.</param>
		/// <param name="blockWidth">Width of rectangle to grab.</param>
		/// <param name="blockHeight">Height of the rectangle to grab.
		/// Get the pixel values from a rectangular area of a ProceduralTexture into an array.
		/// The block is specified by its x,y offset in the texture and by its width and height. The block is "flattened" into the array by scanning the pixel values across rows one by one.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32(int x, int y, int blockWidth, int blockHeight);
	}
}
