using System;

namespace UnityEngine.Rendering
{
	public struct RenderTargetIdentifier
	{
		private BuiltinRenderTextureType m_Type;

		private int m_NameID;

		private int m_InstanceID;

		private IntPtr m_BufferPointer;

		private int m_MipLevel;

		private CubemapFace m_CubeFace;

		private int m_DepthSlice;

		public RenderTargetIdentifier(BuiltinRenderTextureType type)
		{
			this.m_Type = type;
			this.m_NameID = -1;
			this.m_InstanceID = 0;
			this.m_BufferPointer = IntPtr.Zero;
			this.m_MipLevel = 0;
			this.m_CubeFace = CubemapFace.Unknown;
			this.m_DepthSlice = 0;
		}

		public RenderTargetIdentifier(string name)
		{
			this.m_Type = BuiltinRenderTextureType.PropertyName;
			this.m_NameID = Shader.PropertyToID(name);
			this.m_InstanceID = 0;
			this.m_BufferPointer = IntPtr.Zero;
			this.m_MipLevel = 0;
			this.m_CubeFace = CubemapFace.Unknown;
			this.m_DepthSlice = 0;
		}

		public RenderTargetIdentifier(string name, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
		{
			this.m_Type = BuiltinRenderTextureType.PropertyName;
			this.m_NameID = Shader.PropertyToID(name);
			this.m_InstanceID = 0;
			this.m_BufferPointer = IntPtr.Zero;
			this.m_MipLevel = mipLevel;
			this.m_CubeFace = cubeFace;
			this.m_DepthSlice = depthSlice;
		}

		public RenderTargetIdentifier(int nameID)
		{
			this.m_Type = BuiltinRenderTextureType.PropertyName;
			this.m_NameID = nameID;
			this.m_InstanceID = 0;
			this.m_BufferPointer = IntPtr.Zero;
			this.m_MipLevel = 0;
			this.m_CubeFace = CubemapFace.Unknown;
			this.m_DepthSlice = 0;
		}

		public RenderTargetIdentifier(int nameID, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
		{
			this.m_Type = BuiltinRenderTextureType.PropertyName;
			this.m_NameID = nameID;
			this.m_InstanceID = 0;
			this.m_BufferPointer = IntPtr.Zero;
			this.m_MipLevel = mipLevel;
			this.m_CubeFace = cubeFace;
			this.m_DepthSlice = depthSlice;
		}

		public RenderTargetIdentifier(Texture tex)
		{
			if (tex == null)
			{
				this.m_Type = BuiltinRenderTextureType.None;
				this.m_InstanceID = 0;
				this.m_BufferPointer = IntPtr.Zero;
			}
			else if (tex is RenderTexture)
			{
				this.m_Type = BuiltinRenderTextureType.BufferPtr;
				this.m_BufferPointer = ((RenderTexture)tex).colorBuffer.m_BufferPtr;
			}
			else
			{
				this.m_Type = BuiltinRenderTextureType.BindableTexture;
				this.m_BufferPointer = IntPtr.Zero;
			}
			this.m_NameID = -1;
			this.m_InstanceID = ((!tex) ? 0 : tex.GetInstanceID());
			this.m_MipLevel = 0;
			this.m_CubeFace = CubemapFace.Unknown;
			this.m_DepthSlice = 0;
		}

		public RenderTargetIdentifier(Texture tex, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
		{
			if (tex == null)
			{
				this.m_Type = BuiltinRenderTextureType.None;
				this.m_InstanceID = 0;
				this.m_BufferPointer = IntPtr.Zero;
			}
			else if (tex is RenderTexture)
			{
				this.m_Type = BuiltinRenderTextureType.BufferPtr;
				this.m_BufferPointer = ((RenderTexture)tex).colorBuffer.m_BufferPtr;
			}
			else
			{
				this.m_Type = BuiltinRenderTextureType.BindableTexture;
				this.m_BufferPointer = IntPtr.Zero;
			}
			this.m_NameID = -1;
			this.m_InstanceID = ((!tex) ? 0 : tex.GetInstanceID());
			this.m_MipLevel = mipLevel;
			this.m_CubeFace = cubeFace;
			this.m_DepthSlice = depthSlice;
		}

		public RenderTargetIdentifier(RenderBuffer buf, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
		{
			this.m_Type = BuiltinRenderTextureType.BufferPtr;
			this.m_NameID = -1;
			this.m_InstanceID = buf.m_RenderTextureInstanceID;
			this.m_BufferPointer = buf.m_BufferPtr;
			this.m_MipLevel = mipLevel;
			this.m_CubeFace = cubeFace;
			this.m_DepthSlice = depthSlice;
		}

		public static implicit operator RenderTargetIdentifier(BuiltinRenderTextureType type)
		{
			return new RenderTargetIdentifier(type);
		}

		public static implicit operator RenderTargetIdentifier(string name)
		{
			return new RenderTargetIdentifier(name);
		}

		public static implicit operator RenderTargetIdentifier(int nameID)
		{
			return new RenderTargetIdentifier(nameID);
		}

		public static implicit operator RenderTargetIdentifier(Texture tex)
		{
			return new RenderTargetIdentifier(tex);
		}

		public static implicit operator RenderTargetIdentifier(RenderBuffer buf)
		{
			return new RenderTargetIdentifier(buf, 0, CubemapFace.Unknown, 0);
		}

		public override string ToString()
		{
			return UnityString.Format("Type {0} NameID {1} InstanceID {2}", new object[]
			{
				this.m_Type,
				this.m_NameID,
				this.m_InstanceID
			});
		}

		public override int GetHashCode()
		{
			return (this.m_Type.GetHashCode() * 23 + this.m_NameID.GetHashCode()) * 23 + this.m_InstanceID.GetHashCode();
		}

		public bool Equals(RenderTargetIdentifier rhs)
		{
			return this.m_Type == rhs.m_Type && this.m_NameID == rhs.m_NameID && this.m_InstanceID == rhs.m_InstanceID && this.m_BufferPointer == rhs.m_BufferPointer && this.m_MipLevel == rhs.m_MipLevel && this.m_CubeFace == rhs.m_CubeFace && this.m_DepthSlice == rhs.m_DepthSlice;
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (!(obj is RenderTargetIdentifier))
			{
				result = false;
			}
			else
			{
				RenderTargetIdentifier rhs = (RenderTargetIdentifier)obj;
				result = this.Equals(rhs);
			}
			return result;
		}

		public static bool operator ==(RenderTargetIdentifier lhs, RenderTargetIdentifier rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(RenderTargetIdentifier lhs, RenderTargetIdentifier rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
