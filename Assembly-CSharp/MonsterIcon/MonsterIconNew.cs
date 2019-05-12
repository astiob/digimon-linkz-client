using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconNew : MonoBehaviour
	{
		[SerializeField]
		private UISprite sprite;

		[SerializeField]
		private UISprite anime;

		[SerializeField]
		private Vector2 position;

		[SerializeField]
		private Vector3 rotation;

		public Vector2 GetPosition()
		{
			return this.position;
		}

		public Quaternion GetRotation()
		{
			return Quaternion.Euler(this.rotation);
		}

		public void SetNew()
		{
			this.sprite.enabled = true;
			this.anime.enabled = true;
		}

		public void ClearNew()
		{
			this.sprite.enabled = false;
			this.anime.enabled = false;
		}
	}
}
