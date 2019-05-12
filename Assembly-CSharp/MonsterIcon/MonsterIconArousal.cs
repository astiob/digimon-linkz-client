using Monster;
using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconArousal : MonoBehaviour
	{
		[SerializeField]
		private UISprite sprite;

		[SerializeField]
		private Vector2 position;

		public Vector2 GetPosition()
		{
			return this.position;
		}

		public void SetArousal(string arousal)
		{
			string spriteName = MonsterArousalData.GetSpriteName(arousal);
			if (!string.IsNullOrEmpty(spriteName))
			{
				if (!this.sprite.enabled)
				{
					this.sprite.enabled = true;
				}
				this.sprite.spriteName = spriteName;
			}
			else
			{
				this.sprite.enabled = false;
			}
		}

		public void ClearArousal()
		{
			this.sprite.enabled = false;
		}
	}
}
