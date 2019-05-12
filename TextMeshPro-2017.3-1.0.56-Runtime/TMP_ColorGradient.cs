using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_ColorGradient : ScriptableObject
	{
		public Color topLeft;

		public Color topRight;

		public Color bottomLeft;

		public Color bottomRight;

		private static Color k_defaultColor = Color.white;

		public TMP_ColorGradient()
		{
			this.topLeft = TMP_ColorGradient.k_defaultColor;
			this.topRight = TMP_ColorGradient.k_defaultColor;
			this.bottomLeft = TMP_ColorGradient.k_defaultColor;
			this.bottomRight = TMP_ColorGradient.k_defaultColor;
		}

		public TMP_ColorGradient(Color color)
		{
			this.topLeft = color;
			this.topRight = color;
			this.bottomLeft = color;
			this.bottomRight = color;
		}

		public TMP_ColorGradient(Color color0, Color color1, Color color2, Color color3)
		{
			this.topLeft = color0;
			this.topRight = color1;
			this.bottomLeft = color2;
			this.bottomRight = color3;
		}
	}
}
