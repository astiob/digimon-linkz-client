using System;
using UnityEngine;

namespace StatusObject
{
	public class SkillStatusObject : ScriptableObject
	{
		[SerializeField]
		private SkillStatus _skillStatus = new SkillStatus();

		public SkillStatusObject(SkillStatus item)
		{
			this._skillStatus = item;
		}

		public SkillStatus skillStatus
		{
			get
			{
				return this._skillStatus;
			}
		}
	}
}
