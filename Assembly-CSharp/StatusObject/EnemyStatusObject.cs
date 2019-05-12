using System;
using UnityEngine;

namespace StatusObject
{
	public class EnemyStatusObject : ScriptableObject
	{
		[SerializeField]
		private EnemyStatus _enemyStatus = new EnemyStatus();

		public EnemyStatusObject(EnemyStatus item)
		{
			this._enemyStatus = item;
		}

		public EnemyStatus enemyStatus
		{
			get
			{
				return this._enemyStatus;
			}
		}
	}
}
