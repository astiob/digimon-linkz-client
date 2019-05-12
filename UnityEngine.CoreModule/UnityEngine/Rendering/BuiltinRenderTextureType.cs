using System;

namespace UnityEngine.Rendering
{
	public enum BuiltinRenderTextureType
	{
		PropertyName = -3,
		BufferPtr,
		BindableTexture,
		None,
		CurrentActive,
		CameraTarget,
		Depth,
		DepthNormals,
		ResolvedDepth,
		PrepassNormalsSpec = 7,
		PrepassLight,
		PrepassLightSpec,
		GBuffer0,
		GBuffer1,
		GBuffer2,
		GBuffer3,
		Reflections,
		MotionVectors,
		GBuffer4,
		GBuffer5,
		GBuffer6,
		GBuffer7
	}
}
