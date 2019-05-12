using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[Obsolete("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install Allegorithmic's external importer from the Asset Store.", false)]
	public sealed class ProceduralTexture : Texture
	{
		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ProceduralOutputType GetProceduralOutputType();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern ProceduralMaterial GetProceduralMaterial();

		public extern bool hasAlpha { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasBeenGenerated();

		public extern TextureFormat format { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32(int x, int y, int blockWidth, int blockHeight);
	}
}
