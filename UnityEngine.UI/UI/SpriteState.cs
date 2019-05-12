using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[Serializable]
	public struct SpriteState : IEquatable<SpriteState>
	{
		[FormerlySerializedAs("m_SelectedSprite")]
		[SerializeField]
		[FormerlySerializedAs("highlightedSprite")]
		private Sprite m_HighlightedSprite;

		[SerializeField]
		[FormerlySerializedAs("pressedSprite")]
		private Sprite m_PressedSprite;

		[FormerlySerializedAs("disabledSprite")]
		[SerializeField]
		private Sprite m_DisabledSprite;

		public Sprite highlightedSprite
		{
			get
			{
				return this.m_HighlightedSprite;
			}
			set
			{
				this.m_HighlightedSprite = value;
			}
		}

		public Sprite pressedSprite
		{
			get
			{
				return this.m_PressedSprite;
			}
			set
			{
				this.m_PressedSprite = value;
			}
		}

		public Sprite disabledSprite
		{
			get
			{
				return this.m_DisabledSprite;
			}
			set
			{
				this.m_DisabledSprite = value;
			}
		}

		public bool Equals(SpriteState other)
		{
			return this.highlightedSprite == other.highlightedSprite && this.pressedSprite == other.pressedSprite && this.disabledSprite == other.disabledSprite;
		}
	}
}
