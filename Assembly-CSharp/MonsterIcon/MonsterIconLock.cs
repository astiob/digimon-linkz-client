using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconLock : MonoBehaviour
	{
		[SerializeField]
		private UISprite sprite;

		[SerializeField]
		private Vector2 position;

		public Vector2 GetPosition()
		{
			return this.position;
		}

		public void SetLock()
		{
			this.sprite.enabled = true;
		}

		public void SetUnlock()
		{
			this.sprite.enabled = false;
		}
	}
}
