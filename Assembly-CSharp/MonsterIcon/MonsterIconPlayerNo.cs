using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconPlayerNo : MonoBehaviour
	{
		[SerializeField]
		private UISprite sprite;

		[SerializeField]
		private Vector2 position;

		public Vector2 GetPosition()
		{
			return this.position;
		}

		public void SetPlayerIndex(int playerIndex)
		{
			this.sprite.enabled = true;
			if (playerIndex != 0)
			{
				if (playerIndex != 1)
				{
					if (playerIndex == 2)
					{
						this.sprite.spriteName = "MultiBattle_P3";
					}
				}
				else
				{
					this.sprite.spriteName = "MultiBattle_P2";
				}
			}
			else
			{
				this.sprite.spriteName = "MultiBattle_P1";
			}
		}

		public void ClearPlayerNo()
		{
			this.sprite.enabled = false;
		}
	}
}
