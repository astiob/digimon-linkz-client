using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>BillboardAsset describes how a billboard is rendered.</para>
	/// </summary>
	public sealed class BillboardAsset : Object
	{
		/// <summary>
		///   <para>Width of the billboard.</para>
		/// </summary>
		public extern float width { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Height of the billboard.</para>
		/// </summary>
		public extern float height { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Height of the billboard that is below ground.</para>
		/// </summary>
		public extern float bottom { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Number of pre-baked images that can be switched when the billboard is viewed from different angles.</para>
		/// </summary>
		public extern int imageCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Number of vertices in the billboard mesh. The mesh is not necessarily a quad. It can be a more complex shape which fits the actual image more precisely.</para>
		/// </summary>
		public extern int vertexCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Number of indices in the billboard mesh. The mesh is not necessarily a quad. It can be a more complex shape which fits the actual image more precisely.</para>
		/// </summary>
		public extern int indexCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The material used for rendering.</para>
		/// </summary>
		public extern Material material { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MakeRenderMesh(Mesh mesh, float widthScale, float heightScale, float rotation);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MakeMaterialProperties(MaterialPropertyBlock properties, Camera camera);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MakePreviewMesh(Mesh mesh);
	}
}
