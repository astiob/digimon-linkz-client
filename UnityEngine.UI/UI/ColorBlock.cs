using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[Serializable]
	public struct ColorBlock
	{
		[FormerlySerializedAs("normalColor")]
		[SerializeField]
		private Color m_NormalColor;

		[SerializeField]
		[FormerlySerializedAs("m_SelectedColor")]
		[FormerlySerializedAs("highlightedColor")]
		private Color m_HighlightedColor;

		[FormerlySerializedAs("pressedColor")]
		[SerializeField]
		private Color m_PressedColor;

		[FormerlySerializedAs("disabledColor")]
		[SerializeField]
		private Color m_DisabledColor;

		[Range(1f, 5f)]
		[SerializeField]
		private float m_ColorMultiplier;

		[FormerlySerializedAs("fadeDuration")]
		[SerializeField]
		private float m_FadeDuration;

		public Color normalColor
		{
			get
			{
				return this.m_NormalColor;
			}
			set
			{
				this.m_NormalColor = value;
			}
		}

		public Color highlightedColor
		{
			get
			{
				return this.m_HighlightedColor;
			}
			set
			{
				this.m_HighlightedColor = value;
			}
		}

		public Color pressedColor
		{
			get
			{
				return this.m_PressedColor;
			}
			set
			{
				this.m_PressedColor = value;
			}
		}

		public Color disabledColor
		{
			get
			{
				return this.m_DisabledColor;
			}
			set
			{
				this.m_DisabledColor = value;
			}
		}

		public float colorMultiplier
		{
			get
			{
				return this.m_ColorMultiplier;
			}
			set
			{
				this.m_ColorMultiplier = value;
			}
		}

		public float fadeDuration
		{
			get
			{
				return this.m_FadeDuration;
			}
			set
			{
				this.m_FadeDuration = value;
			}
		}

		public static ColorBlock defaultColorBlock
		{
			get
			{
				return new ColorBlock
				{
					m_NormalColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
					m_HighlightedColor = new Color32(245, 245, 245, byte.MaxValue),
					m_PressedColor = new Color32(200, 200, 200, byte.MaxValue),
					m_DisabledColor = new Color32(200, 200, 200, 128),
					colorMultiplier = 1f,
					fadeDuration = 0.1f
				};
			}
		}
	}
}
