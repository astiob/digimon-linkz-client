using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>The material class.</para>
	/// </summary>
	public class Material : Object
	{
		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="contents"></param>
		[Obsolete("Creating materials from shader source string will be removed in the future. Use Shader assets instead.")]
		public Material(string contents)
		{
			Material.Internal_CreateWithString(this, contents);
		}

		/// <summary>
		///   <para>Create a temporary Material.</para>
		/// </summary>
		/// <param name="shader">Create a material with a given Shader.</param>
		/// <param name="source">Create a material by copying all properties from another material.</param>
		public Material(Shader shader)
		{
			Material.Internal_CreateWithShader(this, shader);
		}

		/// <summary>
		///   <para>Create a temporary Material.</para>
		/// </summary>
		/// <param name="shader">Create a material with a given Shader.</param>
		/// <param name="source">Create a material by copying all properties from another material.</param>
		public Material(Material source)
		{
			Material.Internal_CreateWithMaterial(this, source);
		}

		/// <summary>
		///   <para>The shader used by the material.</para>
		/// </summary>
		public extern Shader shader { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The main material's color.</para>
		/// </summary>
		public Color color
		{
			get
			{
				return this.GetColor("_Color");
			}
			set
			{
				this.SetColor("_Color", value);
			}
		}

		/// <summary>
		///   <para>The material's texture.</para>
		/// </summary>
		public Texture mainTexture
		{
			get
			{
				return this.GetTexture("_MainTex");
			}
			set
			{
				this.SetTexture("_MainTex", value);
			}
		}

		/// <summary>
		///   <para>The texture offset of the main texture.</para>
		/// </summary>
		public Vector2 mainTextureOffset
		{
			get
			{
				return this.GetTextureOffset("_MainTex");
			}
			set
			{
				this.SetTextureOffset("_MainTex", value);
			}
		}

		/// <summary>
		///   <para>The texture scale of the main texture.</para>
		/// </summary>
		public Vector2 mainTextureScale
		{
			get
			{
				return this.GetTextureScale("_MainTex");
			}
			set
			{
				this.SetTextureScale("_MainTex", value);
			}
		}

		/// <summary>
		///   <para>Set a named color value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="color"></param>
		/// <param name="nameID"></param>
		public void SetColor(string propertyName, Color color)
		{
			this.SetColor(Shader.PropertyToID(propertyName), color);
		}

		/// <summary>
		///   <para>Set a named color value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="color"></param>
		/// <param name="nameID"></param>
		public void SetColor(int nameID, Color color)
		{
			Material.INTERNAL_CALL_SetColor(this, nameID, ref color);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetColor(Material self, int nameID, ref Color color);

		/// <summary>
		///   <para>Get a named color value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public Color GetColor(string propertyName)
		{
			return this.GetColor(Shader.PropertyToID(propertyName));
		}

		/// <summary>
		///   <para>Get a named color value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color GetColor(int nameID);

		/// <summary>
		///   <para>Set a named vector value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="vector"></param>
		/// <param name="nameID"></param>
		public void SetVector(string propertyName, Vector4 vector)
		{
			this.SetColor(propertyName, new Color(vector.x, vector.y, vector.z, vector.w));
		}

		/// <summary>
		///   <para>Set a named vector value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="vector"></param>
		/// <param name="nameID"></param>
		public void SetVector(int nameID, Vector4 vector)
		{
			this.SetColor(nameID, new Color(vector.x, vector.y, vector.z, vector.w));
		}

		/// <summary>
		///   <para>Get a named vector value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public Vector4 GetVector(string propertyName)
		{
			Color color = this.GetColor(propertyName);
			return new Vector4(color.r, color.g, color.b, color.a);
		}

		/// <summary>
		///   <para>Get a named vector value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public Vector4 GetVector(int nameID)
		{
			Color color = this.GetColor(nameID);
			return new Vector4(color.r, color.g, color.b, color.a);
		}

		/// <summary>
		///   <para>Set a named texture.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="texture"></param>
		/// <param name="nameID"></param>
		public void SetTexture(string propertyName, Texture texture)
		{
			this.SetTexture(Shader.PropertyToID(propertyName), texture);
		}

		/// <summary>
		///   <para>Set a named texture.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="texture"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTexture(int nameID, Texture texture);

		/// <summary>
		///   <para>Get a named texture.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public Texture GetTexture(string propertyName)
		{
			return this.GetTexture(Shader.PropertyToID(propertyName));
		}

		/// <summary>
		///   <para>Get a named texture.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture GetTexture(int nameID);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetTextureOffset(Material mat, string name, out Vector2 output);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetTextureScale(Material mat, string name, out Vector2 output);

		/// <summary>
		///   <para>Sets the placement offset of texture propertyName.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="offset"></param>
		public void SetTextureOffset(string propertyName, Vector2 offset)
		{
			Material.INTERNAL_CALL_SetTextureOffset(this, propertyName, ref offset);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetTextureOffset(Material self, string propertyName, ref Vector2 offset);

		/// <summary>
		///   <para>Gets the placement offset of texture propertyName.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		public Vector2 GetTextureOffset(string propertyName)
		{
			Vector2 result;
			Material.Internal_GetTextureOffset(this, propertyName, out result);
			return result;
		}

		/// <summary>
		///   <para>Sets the placement scale of texture propertyName.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="scale"></param>
		public void SetTextureScale(string propertyName, Vector2 scale)
		{
			Material.INTERNAL_CALL_SetTextureScale(this, propertyName, ref scale);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetTextureScale(Material self, string propertyName, ref Vector2 scale);

		/// <summary>
		///   <para>Gets the placement scale of texture propertyName.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		public Vector2 GetTextureScale(string propertyName)
		{
			Vector2 result;
			Material.Internal_GetTextureScale(this, propertyName, out result);
			return result;
		}

		/// <summary>
		///   <para>Set a named matrix for the shader.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="matrix"></param>
		/// <param name="nameID"></param>
		public void SetMatrix(string propertyName, Matrix4x4 matrix)
		{
			this.SetMatrix(Shader.PropertyToID(propertyName), matrix);
		}

		/// <summary>
		///   <para>Set a named matrix for the shader.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="matrix"></param>
		/// <param name="nameID"></param>
		public void SetMatrix(int nameID, Matrix4x4 matrix)
		{
			Material.INTERNAL_CALL_SetMatrix(this, nameID, ref matrix);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetMatrix(Material self, int nameID, ref Matrix4x4 matrix);

		/// <summary>
		///   <para>Get a named matrix value from the shader.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public Matrix4x4 GetMatrix(string propertyName)
		{
			return this.GetMatrix(Shader.PropertyToID(propertyName));
		}

		/// <summary>
		///   <para>Get a named matrix value from the shader.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Matrix4x4 GetMatrix(int nameID);

		/// <summary>
		///   <para>Set a named float value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetFloat(string propertyName, float value)
		{
			this.SetFloat(Shader.PropertyToID(propertyName), value);
		}

		/// <summary>
		///   <para>Set a named float value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFloat(int nameID, float value);

		/// <summary>
		///   <para>Get a named float value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public float GetFloat(string propertyName)
		{
			return this.GetFloat(Shader.PropertyToID(propertyName));
		}

		/// <summary>
		///   <para>Get a named float value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetFloat(int nameID);

		/// <summary>
		///   <para>Set a named integer value.
		///
		/// When setting values on materials using the Standard Shader, you should be aware that you may need to use EnableKeyword to enable features of the shader that were not previously in use. For more detail, read wiki:
		/// MaterialsAccessingViaScript|Accessing Materials via Script.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetInt(string propertyName, int value)
		{
			this.SetFloat(propertyName, (float)value);
		}

		/// <summary>
		///   <para>Set a named integer value.
		///
		/// When setting values on materials using the Standard Shader, you should be aware that you may need to use EnableKeyword to enable features of the shader that were not previously in use. For more detail, read wiki:
		/// MaterialsAccessingViaScript|Accessing Materials via Script.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="nameID"></param>
		public void SetInt(int nameID, int value)
		{
			this.SetFloat(nameID, (float)value);
		}

		/// <summary>
		///   <para>Get a named integer value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public int GetInt(string propertyName)
		{
			return (int)this.GetFloat(propertyName);
		}

		/// <summary>
		///   <para>Get a named integer value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public int GetInt(int nameID)
		{
			return (int)this.GetFloat(nameID);
		}

		/// <summary>
		///   <para>Set a ComputeBuffer value.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="buffer"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBuffer(string propertyName, ComputeBuffer buffer);

		/// <summary>
		///   <para>Checks if material's shader has a property of a given name.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		public bool HasProperty(string propertyName)
		{
			return this.HasProperty(Shader.PropertyToID(propertyName));
		}

		/// <summary>
		///   <para>Checks if material's shader has a property of a given name.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="nameID"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasProperty(int nameID);

		/// <summary>
		///   <para>Get the value of material's shader tag.</para>
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="searchFallbacks"></param>
		/// <param name="defaultValue"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetTag(string tag, bool searchFallbacks, [DefaultValue("\"\"")] string defaultValue);

		/// <summary>
		///   <para>Get the value of material's shader tag.</para>
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="searchFallbacks"></param>
		/// <param name="defaultValue"></param>
		[ExcludeFromDocs]
		public string GetTag(string tag, bool searchFallbacks)
		{
			string empty = string.Empty;
			return this.GetTag(tag, searchFallbacks, empty);
		}

		/// <summary>
		///   <para>Sets an override tag/value on the material.</para>
		/// </summary>
		/// <param name="tag">Name of the tag to set.</param>
		/// <param name="val">Name of the value to set. Empty string to clear the override flag.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetOverrideTag(string tag, string val);

		/// <summary>
		///   <para>Interpolate properties between two materials.</para>
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="t"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Lerp(Material start, Material end, float t);

		/// <summary>
		///   <para>How many passes are in this material (Read Only).</para>
		/// </summary>
		public extern int passCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Activate the given pass for rendering.</para>
		/// </summary>
		/// <param name="pass">Shader pass number to setup.</param>
		/// <returns>
		///   <para>If false is returned, no rendering should be done.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetPass(int pass);

		/// <summary>
		///   <para>Render queue of this material.</para>
		/// </summary>
		public extern int renderQueue { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Creating materials from shader source string will be removed in the future. Use Shader assets instead.")]
		public static Material Create(string scriptContents)
		{
			return new Material(scriptContents);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateWithString([Writable] Material mono, string contents);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateWithShader([Writable] Material mono, Shader shader);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateWithMaterial([Writable] Material mono, Material source);

		/// <summary>
		///   <para>Copy properties from other material into this material.</para>
		/// </summary>
		/// <param name="mat"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CopyPropertiesFromMaterial(Material mat);

		/// <summary>
		///   <para>Set a shader keyword that is enabled by this material.</para>
		/// </summary>
		/// <param name="keyword"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void EnableKeyword(string keyword);

		/// <summary>
		///   <para>Unset a shader keyword.</para>
		/// </summary>
		/// <param name="keyword"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DisableKeyword(string keyword);

		/// <summary>
		///   <para>Is the shader keyword enabled on this material?</para>
		/// </summary>
		/// <param name="keyword"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsKeywordEnabled(string keyword);

		/// <summary>
		///   <para>Additional shader keywords set by this material.</para>
		/// </summary>
		public extern string[] shaderKeywords { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Defines how the material should interact with lightmaps and lightprobes.</para>
		/// </summary>
		public extern MaterialGlobalIlluminationFlags globalIlluminationFlags { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
