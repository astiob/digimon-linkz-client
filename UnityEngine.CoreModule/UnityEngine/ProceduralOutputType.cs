using System;

namespace UnityEngine
{
	[Obsolete("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install Allegorithmic's external importer from the Asset Store.", false)]
	public enum ProceduralOutputType
	{
		Unknown,
		Diffuse,
		Normal,
		Height,
		Emissive,
		Specular,
		Opacity,
		Smoothness,
		AmbientOcclusion,
		DetailMask,
		Metallic,
		Roughness
	}
}
