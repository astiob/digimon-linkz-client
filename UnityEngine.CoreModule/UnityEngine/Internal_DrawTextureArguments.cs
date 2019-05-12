using System;

namespace UnityEngine
{
	internal struct Internal_DrawTextureArguments
	{
		public Rect screenRect;

		public Rect sourceRect;

		public int leftBorder;

		public int rightBorder;

		public int topBorder;

		public int bottomBorder;

		public Color32 color;

		public Vector4 borderWidths;

		public Vector4 cornerRadiuses;

		public int pass;

		public Texture texture;

		public Material mat;
	}
}
