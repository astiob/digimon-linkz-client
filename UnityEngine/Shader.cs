using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Shader scripts used for all rendering.</para>
	/// </summary>
	public sealed class Shader : Object
	{
		/// <summary>
		///   <para>Finds a shader with the given name.</para>
		/// </summary>
		/// <param name="name"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Shader Find(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Shader FindBuiltin(string name);

		/// <summary>
		///   <para>Can this shader run on the end-users graphics card? (Read Only)</para>
		/// </summary>
		public extern bool isSupported { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Set a global shader keyword.</para>
		/// </summary>
		/// <param name="keyword"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EnableKeyword(string keyword);

		/// <summary>
		///   <para>Unset a global shader keyword.</para>
		/// </summary>
		/// <param name="keyword"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DisableKeyword(string keyword);

		/// <summary>
		///   <para>Is global shader keyword enabled?</para>
		/// </summary>
		/// <param name="keyword"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsKeywordEnabled(string keyword);

		/// <summary>
		///   <para>Shader LOD level for this shader.</para>
		/// </summary>
		public extern int maximumLOD { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Shader LOD level for all shaders.</para>
		/// </summary>
		public static extern int globalMaximumLOD { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Render queue of this shader. (Read Only)</para>
		/// </summary>
		public extern int renderQueue { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern DisableBatchingType disableBatching { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Sets a global color property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="color"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalColor(string propertyName, Color color)
		{
			Shader.SetGlobalColor(Shader.PropertyToID(propertyName), color);
		}

		/// <summary>
		///   <para>Sets a global color property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="color"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalColor(int nameID, Color color)
		{
			Shader.INTERNAL_CALL_SetGlobalColor(nameID, ref color);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetGlobalColor(int nameID, ref Color color);

		/// <summary>
		///   <para>Sets a global vector property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="vec"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalVector(string propertyName, Vector4 vec)
		{
			Shader.SetGlobalColor(propertyName, vec);
		}

		/// <summary>
		///   <para>Sets a global vector property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="vec"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalVector(int nameID, Vector4 vec)
		{
			Shader.SetGlobalColor(nameID, vec);
		}

		/// <summary>
		///   <para>Sets a global float property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalFloat(string propertyName, float value)
		{
			Shader.SetGlobalFloat(Shader.PropertyToID(propertyName), value);
		}

		/// <summary>
		///   <para>Sets a global float property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGlobalFloat(int nameID, float value);

		/// <summary>
		///   <para>Sets a global int property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalInt(string propertyName, int value)
		{
			Shader.SetGlobalFloat(propertyName, (float)value);
		}

		/// <summary>
		///   <para>Sets a global int property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalInt(int nameID, int value)
		{
			Shader.SetGlobalFloat(nameID, (float)value);
		}

		/// <summary>
		///   <para>Sets a global texture property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="tex"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalTexture(string propertyName, Texture tex)
		{
			Shader.SetGlobalTexture(Shader.PropertyToID(propertyName), tex);
		}

		/// <summary>
		///   <para>Sets a global texture property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="tex"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGlobalTexture(int nameID, Texture tex);

		/// <summary>
		///   <para>Sets a global matrix property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="mat"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalMatrix(string propertyName, Matrix4x4 mat)
		{
			Shader.SetGlobalMatrix(Shader.PropertyToID(propertyName), mat);
		}

		/// <summary>
		///   <para>Sets a global matrix property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="mat"></param>
		/// <param name="nameID"></param>
		public static void SetGlobalMatrix(int nameID, Matrix4x4 mat)
		{
			Shader.INTERNAL_CALL_SetGlobalMatrix(nameID, ref mat);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetGlobalMatrix(int nameID, ref Matrix4x4 mat);

		[Obsolete("SetGlobalTexGenMode is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGlobalTexGenMode(string propertyName, TexGenMode mode);

		[Obsolete("SetGlobalTextureMatrixName is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGlobalTextureMatrixName(string propertyName, string matrixName);

		/// <summary>
		///   <para>Sets a global compute buffer property for all shaders.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="buffer"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGlobalBuffer(string propertyName, ComputeBuffer buffer);

		/// <summary>
		///   <para>Gets unique identifier for a shader property name.</para>
		/// </summary>
		/// <param name="name"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int PropertyToID(string name);

		/// <summary>
		///   <para>Fully load all shaders to prevent future performance hiccups.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WarmupAllShaders();
	}
}
