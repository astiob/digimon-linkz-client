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
			switch (playerIndex)
			{
			case 0:
				this.sprite.spriteName = "MultiBattle_P1";
				break;
			case 1:
				this.sprite.spriteName = "MultiBattle_P2";
				break;
			case 2:
				this.sprite.spriteName = "MultiBattle_P3";
				break;
			}
		}

		public void ClearPlayerNo()
		{
			this.sprite.enabled = false;
		}
	}
}
