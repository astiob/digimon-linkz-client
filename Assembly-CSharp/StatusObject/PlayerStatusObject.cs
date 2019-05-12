using System;
using UnityEngine;

namespace StatusObject
{
	public class PlayerStatusObject : ScriptableObject
	{
		[SerializeField]
		private PlayerStatus _playerStatus = new PlayerStatus();

		public PlayerStatusObject(PlayerStatus item)
		{
			this._playerStatus = item;
		}

		public PlayerStatus playerStatus
		{
			get
			{
				return this._playerStatus;
			}
		}
	}
}
