using System;
using UnityEngine;

namespace StatusObject
{
	public class LeaderSkillStatusObject : ScriptableObject
	{
		[SerializeField]
		private LeaderSkillStatus _leaderSkillStatus = new LeaderSkillStatus();

		public LeaderSkillStatusObject(LeaderSkillStatus leaderSkillStatus)
		{
			this._leaderSkillStatus = leaderSkillStatus;
		}

		public LeaderSkillStatus leaderSkillStatus
		{
			get
			{
				return this._leaderSkillStatus;
			}
		}
	}
}
