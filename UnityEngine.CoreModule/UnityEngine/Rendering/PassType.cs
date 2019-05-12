using System;

namespace UnityEngine.Rendering
{
	public enum PassType
	{
		Normal,
		Vertex,
		VertexLM,
		[Obsolete("VertexLMRGBM PassType is obsolete. Please use VertexLM PassType together with DecodeLightmap shader function.")]
		VertexLMRGBM,
		ForwardBase,
		ForwardAdd,
		LightPrePassBase,
		LightPrePassFinal,
		ShadowCaster,
		Deferred = 10,
		Meta,
		MotionVectors
	}
}
