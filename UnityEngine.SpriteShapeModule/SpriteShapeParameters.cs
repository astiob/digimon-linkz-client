using System;

namespace UnityEngine.Experimental.U2D
{
	public struct SpriteShapeParameters
	{
		public Matrix4x4 transform;

		public Texture2D fillTexture;

		public uint fillScale;

		public uint splineDetail;

		public float angleThreshold;

		public float borderPivot;

		public float bevelCutoff;

		public float bevelSize;

		public bool carpet;

		public bool smartSprite;

		public bool adaptiveUV;

		public bool spriteBorders;
	}
}
