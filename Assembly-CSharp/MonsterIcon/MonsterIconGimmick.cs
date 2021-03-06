﻿using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconGimmick : MonoBehaviour
	{
		[SerializeField]
		private UISprite sprite;

		[SerializeField]
		private UILabel label;

		[SerializeField]
		private Vector2 position;

		public Vector2 GetPosition()
		{
			return this.position;
		}

		public void SetGimmickIcon()
		{
			this.sprite.enabled = true;
			this.label.enabled = true;
		}

		public void ClearGimmickIcon()
		{
			this.sprite.enabled = false;
			this.label.enabled = false;
		}
	}
}
