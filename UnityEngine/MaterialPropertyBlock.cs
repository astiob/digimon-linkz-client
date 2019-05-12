using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A block of material values to apply.</para>
	/// </summary>
	public sealed class MaterialPropertyBlock
	{
		internal IntPtr m_Ptr;

		public MaterialPropertyBlock()
		{
			this.InitBlock();
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InitBlock();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void DestroyBlock();

		~MaterialPropertyBlock()
		{
			this.DestroyBlock();
		}

		/// <summary>
		///   <para>Is the material property block empty? (Read Only)</para>
		/// </summary>
		public extern bool isEmpty { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Set a float property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetFloat(string name, float value)
		{
			this.SetFloat(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Set a float property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFloat(int nameID, float value);

		/// <summary>
		///   <para>Set a vector property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetVector(string name, Vector4 value)
		{
			this.SetVector(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Set a vector property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetVector(int nameID, Vector4 value)
		{
			MaterialPropertyBlock.INTERNAL_CALL_SetVector(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetVector(MaterialPropertyBlock self, int nameID, ref Vector4 value);

		/// <summary>
		///   <para>Set a color property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetColor(string name, Color value)
		{
			this.SetColor(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Set a color property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetColor(int nameID, Color value)
		{
			MaterialPropertyBlock.INTERNAL_CALL_SetColor(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetColor(MaterialPropertyBlock self, int nameID, ref Color value);

		/// <summary>
		///   <para>Set a matrix property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetMatrix(string name, Matrix4x4 value)
		{
			this.SetMatrix(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Set a matrix property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetMatrix(int nameID, Matrix4x4 value)
		{
			MaterialPropertyBlock.INTERNAL_CALL_SetMatrix(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetMatrix(MaterialPropertyBlock self, int nameID, ref Matrix4x4 value);

		/// <summary>
		///   <para>Set a texture property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetTexture(string name, Texture value)
		{
			this.SetTexture(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Set a texture property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTexture(int nameID, Texture value);

		/// <summary>
		///   <para>Add a float material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddFloat(string name, float value)
		{
			this.AddFloat(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a float material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddFloat(int nameID, float value);

		/// <summary>
		///   <para>Add a vector material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddVector(string name, Vector4 value)
		{
			this.AddVector(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a vector material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddVector(int nameID, Vector4 value)
		{
			MaterialPropertyBlock.INTERNAL_CALL_AddVector(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_AddVector(MaterialPropertyBlock self, int nameID, ref Vector4 value);

		/// <summary>
		///   <para>Add a color material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddColor(string name, Color value)
		{
			this.AddColor(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a color material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddColor(int nameID, Color value)
		{
			MaterialPropertyBlock.INTERNAL_CALL_AddColor(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_AddColor(MaterialPropertyBlock self, int nameID, ref Color value);

		/// <summary>
		///   <para>Add a matrix material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddMatrix(string name, Matrix4x4 value)
		{
			this.AddMatrix(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a matrix material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddMatrix(int nameID, Matrix4x4 value)
		{
			MaterialPropertyBlock.INTERNAL_CALL_AddMatrix(this, nameID, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_AddMatrix(MaterialPropertyBlock self, int nameID, ref Matrix4x4 value);

		/// <summary>
		///   <para>Add a texture material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void AddTexture(string name, Texture value)
		{
			this.AddTexture(Shader.PropertyToID(name), value);
		}

		/// <summary>
		///   <para>Add a texture material property.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddTexture(int nameID, Texture value);

		/// <summary>
		///   <para>Get a float from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		public float GetFloat(string name)
		{
			return this.GetFloat(Shader.PropertyToID(name));
		}

		/// <summary>
		///   <para>Get a float from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetFloat(int nameID);

		/// <summary>
		///   <para>Get a vector from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		public Vector4 GetVector(string name)
		{
			return this.GetVector(Shader.PropertyToID(name));
		}

		/// <summary>
		///   <para>Get a vector from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Vector4 GetVector(int nameID);

		/// <summary>
		///   <para>Get a matrix from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		public Matrix4x4 GetMatrix(string name)
		{
			return this.GetMatrix(Shader.PropertyToID(name));
		}

		/// <summary>
		///   <para>Get a matrix from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Matrix4x4 GetMatrix(int nameID);

		/// <summary>
		///   <para>Get a texture from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		public Texture GetTexture(string name)
		{
			return this.GetTexture(Shader.PropertyToID(name));
		}

		/// <summary>
		///   <para>Get a texture from the property block.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture GetTexture(int nameID);

		/// <summary>
		///   <para>Clear material property values.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Clear();
	}
}
