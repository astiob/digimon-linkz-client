using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>List of graphics commands to execute.</para>
	/// </summary>
	public sealed class CommandBuffer : IDisposable
	{
		internal IntPtr m_Ptr;

		/// <summary>
		///   <para>Create a new empty command buffer.</para>
		/// </summary>
		public CommandBuffer()
		{
			this.m_Ptr = IntPtr.Zero;
			CommandBuffer.InitBuffer(this);
		}

		~CommandBuffer()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			this.ReleaseBuffer();
			this.m_Ptr = IntPtr.Zero;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitBuffer(CommandBuffer buf);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseBuffer();

		public void Release()
		{
			this.Dispose();
		}

		/// <summary>
		///   <para>Name of this command buffer.</para>
		/// </summary>
		public extern string name { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Size of this command buffer in bytes (Read Only).</para>
		/// </summary>
		public extern int sizeInBytes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Clear all commands in the buffer.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Clear();

		/// <summary>
		///   <para>Add a "draw mesh" command.</para>
		/// </summary>
		/// <param name="mesh">Mesh to draw.</param>
		/// <param name="matrix">Transformation matrix to use.</param>
		/// <param name="material">Material to use.</param>
		/// <param name="submeshIndex">Which subset of the mesh to render.</param>
		/// <param name="shaderPass">Which pass of the shader to use (default is -1, which renders all passes).</param>
		/// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			CommandBuffer.INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
		}

		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass)
		{
			MaterialPropertyBlock properties = null;
			CommandBuffer.INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
		}

		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex)
		{
			MaterialPropertyBlock properties = null;
			int shaderPass = -1;
			CommandBuffer.INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
		}

		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material)
		{
			MaterialPropertyBlock properties = null;
			int shaderPass = -1;
			int submeshIndex = 0;
			CommandBuffer.INTERNAL_CALL_DrawMesh(this, mesh, ref matrix, material, submeshIndex, shaderPass, properties);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawMesh(CommandBuffer self, Mesh mesh, ref Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties);

		/// <summary>
		///   <para>Add a "draw renderer" command.</para>
		/// </summary>
		/// <param name="renderer">Renderer to draw.</param>
		/// <param name="material">Material to use.</param>
		/// <param name="submeshIndex">Which subset of the mesh to render.</param>
		/// <param name="shaderPass">Which pass of the shader to use (default is -1, which renders all passes).</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DrawRenderer(Renderer renderer, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass);

		[ExcludeFromDocs]
		public void DrawRenderer(Renderer renderer, Material material, int submeshIndex)
		{
			int shaderPass = -1;
			this.DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		[ExcludeFromDocs]
		public void DrawRenderer(Renderer renderer, Material material)
		{
			int shaderPass = -1;
			int submeshIndex = 0;
			this.DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		/// <summary>
		///   <para>Add a "draw procedural geometry" command.</para>
		/// </summary>
		/// <param name="matrix">Transformation matrix to use.</param>
		/// <param name="material">Material to use.</param>
		/// <param name="shaderPass">Which pass of the shader to use (or -1 for all passes).</param>
		/// <param name="topology">Topology of the procedural geometry.</param>
		/// <param name="vertexCount">Vertex count to render.</param>
		/// <param name="instanceCount">Instance count to render.</param>
		/// <param name="properties">Additional material properties to apply just before rendering. See MaterialPropertyBlock.</param>
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, [DefaultValue("1")] int instanceCount, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			CommandBuffer.INTERNAL_CALL_DrawProcedural(this, ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		[ExcludeFromDocs]
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount)
		{
			MaterialPropertyBlock properties = null;
			CommandBuffer.INTERNAL_CALL_DrawProcedural(this, ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		[ExcludeFromDocs]
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount)
		{
			MaterialPropertyBlock properties = null;
			int instanceCount = 1;
			CommandBuffer.INTERNAL_CALL_DrawProcedural(this, ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawProcedural(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties);

		/// <summary>
		///   <para>Add a "draw procedural geometry" command.</para>
		/// </summary>
		/// <param name="matrix">Transformation matrix to use.</param>
		/// <param name="material">Material to use.</param>
		/// <param name="shaderPass">Which pass of the shader to use (or -1 for all passes).</param>
		/// <param name="topology">Topology of the procedural geometry.</param>
		/// <param name="properties">Additional material properties to apply just before rendering. See MaterialPropertyBlock.</param>
		/// <param name="bufferWithArgs">Buffer with draw arguments.</param>
		/// <param name="argsOffset">Offset where in the buffer the draw arguments are.</param>
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			CommandBuffer.INTERNAL_CALL_DrawProceduralIndirect(this, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		[ExcludeFromDocs]
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset)
		{
			MaterialPropertyBlock properties = null;
			CommandBuffer.INTERNAL_CALL_DrawProceduralIndirect(this, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		[ExcludeFromDocs]
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs)
		{
			MaterialPropertyBlock properties = null;
			int argsOffset = 0;
			CommandBuffer.INTERNAL_CALL_DrawProceduralIndirect(this, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawProceduralIndirect(CommandBuffer self, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		/// <summary>
		///   <para>Add a "set active render target" command.</para>
		/// </summary>
		/// <param name="rt">Render target to set for both color &amp; depth buffers.</param>
		/// <param name="color">Render target to set as a color buffer.</param>
		/// <param name="colors">Render targets to set as color buffers (MRT).</param>
		/// <param name="depth">Render target to set as a depth buffer.</param>
		/// <param name="mipLevel">The mip level of the render target to render into.</param>
		/// <param name="cubemapFace">The cubemap face of a cubemap render target to render into.</param>
		public void SetRenderTarget(RenderTargetIdentifier rt)
		{
			this.SetRenderTarget_Single(ref rt, 0, CubemapFace.Unknown);
		}

		/// <summary>
		///   <para>Add a "set active render target" command.</para>
		/// </summary>
		/// <param name="rt">Render target to set for both color &amp; depth buffers.</param>
		/// <param name="color">Render target to set as a color buffer.</param>
		/// <param name="colors">Render targets to set as color buffers (MRT).</param>
		/// <param name="depth">Render target to set as a depth buffer.</param>
		/// <param name="mipLevel">The mip level of the render target to render into.</param>
		/// <param name="cubemapFace">The cubemap face of a cubemap render target to render into.</param>
		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel)
		{
			this.SetRenderTarget_Single(ref rt, mipLevel, CubemapFace.Unknown);
		}

		/// <summary>
		///   <para>Add a "set active render target" command.</para>
		/// </summary>
		/// <param name="rt">Render target to set for both color &amp; depth buffers.</param>
		/// <param name="color">Render target to set as a color buffer.</param>
		/// <param name="colors">Render targets to set as color buffers (MRT).</param>
		/// <param name="depth">Render target to set as a depth buffer.</param>
		/// <param name="mipLevel">The mip level of the render target to render into.</param>
		/// <param name="cubemapFace">The cubemap face of a cubemap render target to render into.</param>
		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace)
		{
			this.SetRenderTarget_Single(ref rt, mipLevel, cubemapFace);
		}

		/// <summary>
		///   <para>Add a "set active render target" command.</para>
		/// </summary>
		/// <param name="rt">Render target to set for both color &amp; depth buffers.</param>
		/// <param name="color">Render target to set as a color buffer.</param>
		/// <param name="colors">Render targets to set as color buffers (MRT).</param>
		/// <param name="depth">Render target to set as a depth buffer.</param>
		/// <param name="mipLevel">The mip level of the render target to render into.</param>
		/// <param name="cubemapFace">The cubemap face of a cubemap render target to render into.</param>
		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth)
		{
			this.SetRenderTarget_ColDepth(ref color, ref depth, 0, CubemapFace.Unknown);
		}

		/// <summary>
		///   <para>Add a "set active render target" command.</para>
		/// </summary>
		/// <param name="rt">Render target to set for both color &amp; depth buffers.</param>
		/// <param name="color">Render target to set as a color buffer.</param>
		/// <param name="colors">Render targets to set as color buffers (MRT).</param>
		/// <param name="depth">Render target to set as a depth buffer.</param>
		/// <param name="mipLevel">The mip level of the render target to render into.</param>
		/// <param name="cubemapFace">The cubemap face of a cubemap render target to render into.</param>
		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel)
		{
			this.SetRenderTarget_ColDepth(ref color, ref depth, mipLevel, CubemapFace.Unknown);
		}

		/// <summary>
		///   <para>Add a "set active render target" command.</para>
		/// </summary>
		/// <param name="rt">Render target to set for both color &amp; depth buffers.</param>
		/// <param name="color">Render target to set as a color buffer.</param>
		/// <param name="colors">Render targets to set as color buffers (MRT).</param>
		/// <param name="depth">Render target to set as a depth buffer.</param>
		/// <param name="mipLevel">The mip level of the render target to render into.</param>
		/// <param name="cubemapFace">The cubemap face of a cubemap render target to render into.</param>
		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace)
		{
			this.SetRenderTarget_ColDepth(ref color, ref depth, mipLevel, cubemapFace);
		}

		/// <summary>
		///   <para>Add a "set active render target" command.</para>
		/// </summary>
		/// <param name="rt">Render target to set for both color &amp; depth buffers.</param>
		/// <param name="color">Render target to set as a color buffer.</param>
		/// <param name="colors">Render targets to set as color buffers (MRT).</param>
		/// <param name="depth">Render target to set as a depth buffer.</param>
		/// <param name="mipLevel">The mip level of the render target to render into.</param>
		/// <param name="cubemapFace">The cubemap face of a cubemap render target to render into.</param>
		public void SetRenderTarget(RenderTargetIdentifier[] colors, RenderTargetIdentifier depth)
		{
			this.SetRenderTarget_Multiple(colors, ref depth);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTarget_Single(ref RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTarget_ColDepth(ref RenderTargetIdentifier color, ref RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTarget_Multiple(RenderTargetIdentifier[] color, ref RenderTargetIdentifier depth);

		/// <summary>
		///   <para>Add a "blit into a render texture" command.</para>
		/// </summary>
		/// <param name="source">Source texture or render target to blit from.</param>
		/// <param name="dest">Destination to blit into.</param>
		/// <param name="mat">Material to use.</param>
		/// <param name="pass">Shader pass to use (default is -1, meaning "all passes").</param>
		public void Blit(Texture source, RenderTargetIdentifier dest)
		{
			this.Blit_Texture(source, ref dest, null, -1);
		}

		/// <summary>
		///   <para>Add a "blit into a render texture" command.</para>
		/// </summary>
		/// <param name="source">Source texture or render target to blit from.</param>
		/// <param name="dest">Destination to blit into.</param>
		/// <param name="mat">Material to use.</param>
		/// <param name="pass">Shader pass to use (default is -1, meaning "all passes").</param>
		public void Blit(Texture source, RenderTargetIdentifier dest, Material mat)
		{
			this.Blit_Texture(source, ref dest, mat, -1);
		}

		/// <summary>
		///   <para>Add a "blit into a render texture" command.</para>
		/// </summary>
		/// <param name="source">Source texture or render target to blit from.</param>
		/// <param name="dest">Destination to blit into.</param>
		/// <param name="mat">Material to use.</param>
		/// <param name="pass">Shader pass to use (default is -1, meaning "all passes").</param>
		public void Blit(Texture source, RenderTargetIdentifier dest, Material mat, int pass)
		{
			this.Blit_Texture(source, ref dest, mat, pass);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Blit_Texture(Texture source, ref RenderTargetIdentifier dest, Material mat, int pass);

		/// <summary>
		///   <para>Add a "blit into a render texture" command.</para>
		/// </summary>
		/// <param name="source">Source texture or render target to blit from.</param>
		/// <param name="dest">Destination to blit into.</param>
		/// <param name="mat">Material to use.</param>
		/// <param name="pass">Shader pass to use (default is -1, meaning "all passes").</param>
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest)
		{
			this.Blit_Identifier(ref source, ref dest, null, -1);
		}

		/// <summary>
		///   <para>Add a "blit into a render texture" command.</para>
		/// </summary>
		/// <param name="source">Source texture or render target to blit from.</param>
		/// <param name="dest">Destination to blit into.</param>
		/// <param name="mat">Material to use.</param>
		/// <param name="pass">Shader pass to use (default is -1, meaning "all passes").</param>
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat)
		{
			this.Blit_Identifier(ref source, ref dest, mat, -1);
		}

		/// <summary>
		///   <para>Add a "blit into a render texture" command.</para>
		/// </summary>
		/// <param name="source">Source texture or render target to blit from.</param>
		/// <param name="dest">Destination to blit into.</param>
		/// <param name="mat">Material to use.</param>
		/// <param name="pass">Shader pass to use (default is -1, meaning "all passes").</param>
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass)
		{
			this.Blit_Identifier(ref source, ref dest, mat, pass);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, [DefaultValue("null")] Material mat, [DefaultValue("-1")] int pass);

		[ExcludeFromDocs]
		private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, Material mat)
		{
			int pass = -1;
			this.Blit_Identifier(ref source, ref dest, mat, pass);
		}

		[ExcludeFromDocs]
		private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest)
		{
			int pass = -1;
			Material mat = null;
			this.Blit_Identifier(ref source, ref dest, mat, pass);
		}

		/// <summary>
		///   <para>Add a "get a temporary render texture" command.</para>
		/// </summary>
		/// <param name="nameID">Shader property name for this texture.</param>
		/// <param name="width">Width in pixels, or -1 for "camera pixel width".</param>
		/// <param name="height">Height in pixels, or -1 for "camera pixel height".</param>
		/// <param name="depthBuffer">Depth buffer bits (0, 16 or 24).</param>
		/// <param name="filter">Texture filtering mode (default is Point).</param>
		/// <param name="format">Format of the render texture (default is ARGB32).</param>
		/// <param name="readWrite">Color space.</param>
		/// <param name="antiAliasing">Anti-aliasing (default is no anti-aliasing).</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetTemporaryRT(int nameID, int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("FilterMode.Point")] FilterMode filter, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing);

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			int antiAliasing = 1;
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			FilterMode filter = FilterMode.Point;
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public void GetTemporaryRT(int nameID, int width, int height)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			FilterMode filter = FilterMode.Point;
			int depthBuffer = 0;
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing);
		}

		/// <summary>
		///   <para>Add a "release a temporary render texture" command.</para>
		/// </summary>
		/// <param name="nameID">Shader property name for this texture.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ReleaseTemporaryRT(int nameID);

		/// <summary>
		///   <para>Adds a "clear render target" command.</para>
		/// </summary>
		/// <param name="clearDepth">Should clear depth buffer?</param>
		/// <param name="clearColor">Should clear color buffer?</param>
		/// <param name="backgroundColor">Color to clear with.</param>
		/// <param name="depth">Depth to clear with (default is 1.0).</param>
		public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
		{
			CommandBuffer.INTERNAL_CALL_ClearRenderTarget(this, clearDepth, clearColor, ref backgroundColor, depth);
		}

		[ExcludeFromDocs]
		public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			float depth = 1f;
			CommandBuffer.INTERNAL_CALL_ClearRenderTarget(this, clearDepth, clearColor, ref backgroundColor, depth);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ClearRenderTarget(CommandBuffer self, bool clearDepth, bool clearColor, ref Color backgroundColor, float depth);

		/// <summary>
		///   <para>Add a "set global shader float property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalFloat(string name, float value)
		{
			this.SetGlobalFloat(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a "set global shader float property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalFloat(int nameID, float value);

		/// <summary>
		///   <para>Add a "set global shader vector property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalVector(string name, Vector4 value)
		{
			this.SetGlobalVector(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a "set global shader vector property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalVector(int nameID, Vector4 value)
		{
			CommandBuffer.INTERNAL_CALL_SetGlobalVector(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetGlobalVector(CommandBuffer self, int nameID, ref Vector4 value);

		/// <summary>
		///   <para>Add a "set global shader color property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalColor(string name, Color value)
		{
			this.SetGlobalColor(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a "set global shader color property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalColor(int nameID, Color value)
		{
			CommandBuffer.INTERNAL_CALL_SetGlobalColor(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetGlobalColor(CommandBuffer self, int nameID, ref Color value);

		/// <summary>
		///   <para>Add a "set global shader matrix property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalMatrix(string name, Matrix4x4 value)
		{
			this.SetGlobalMatrix(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a "set global shader matrix property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalMatrix(int nameID, Matrix4x4 value)
		{
			CommandBuffer.INTERNAL_CALL_SetGlobalMatrix(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetGlobalMatrix(CommandBuffer self, int nameID, ref Matrix4x4 value);

		/// <summary>
		///   <para>Add a "set global shader texture property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalTexture(string name, RenderTargetIdentifier value)
		{
			this.SetGlobalTexture(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a "set global shader texture property" command.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetGlobalTexture(int nameID, RenderTargetIdentifier value)
		{
			this.SetGlobalTexture_Impl(nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalTexture_Impl(int nameID, ref RenderTargetIdentifier rt);

		/// <summary>
		///   <para>Send a user-defined event to a native code plugin.</para>
		/// </summary>
		/// <param name="callback">Native code callback to queue for Unity's renderer to invoke.</param>
		/// <param name="eventID">User defined id to send to the callback.</param>
		public void IssuePluginEvent(IntPtr callback, int eventID)
		{
			if (callback == IntPtr.Zero)
			{
				throw new ArgumentException("Null callback specified.");
			}
			this.IssuePluginEventInternal(callback, eventID);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void IssuePluginEventInternal(IntPtr callback, int eventID);
	}
}
