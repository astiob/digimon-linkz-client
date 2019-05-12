using System;
using UnityExtension;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class BattleWave
	{
		public string[] useEnemiesId = new string[]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};

		public bool[] enemiesBossFlag = new bool[3];

		public bool[] enemiesInfinityApFlag = new bool[3];

		public float hpRevivalPercentage;

		public string bgmId = string.Empty;

		public float bgmChangeHpPercentage;

		public string changedBgmId = string.Empty;

		public int floorNum;

		public int floorType;

		public int cameraType;

		public CharacterStatus[] enemyStatuses;

		public bool isFindBoss
		{
			get
			{
				return !BoolExtension.AllMachValue(false, this.enemiesBossFlag);
			}
		}
	}
}
