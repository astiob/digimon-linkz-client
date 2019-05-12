using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.AI
{
	[Serializable]
	public class EnemyAIPattern
	{
		[SerializeField]
		private AICycle _aiCycle;

		[SerializeField]
		private List<AIActionPattern> _aiActionPattern = new List<AIActionPattern>();

		public EnemyAIPattern()
		{
			this._aiActionPattern = new List<AIActionPattern>();
			this._aiActionPattern.Add(new AIActionPattern());
		}

		public EnemyAIPattern(AICycle aiCycle, params AIActionPattern[] aiActionPattern)
		{
			this._aiCycle = aiCycle;
			this._aiActionPattern = new List<AIActionPattern>(aiActionPattern);
			float minRange = 0f;
			for (int i = 0; i < this._aiActionPattern.Count; i++)
			{
				this._aiActionPattern[i] = new AIActionPattern(minRange, this._aiActionPattern[i].maxRange, this._aiActionPattern[i].aiActionClip.ToArray());
				minRange = this._aiActionPattern[i].maxRange;
			}
		}

		public AICycle aiCycle
		{
			get
			{
				return this._aiCycle;
			}
			private set
			{
				this._aiCycle = value;
			}
		}

		public List<AIActionPattern> aiActionPattern
		{
			get
			{
				return this._aiActionPattern;
			}
			private set
			{
				this._aiActionPattern = value;
			}
		}

		public string[] GetAllSkillID()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this._aiActionPattern.Count; i++)
			{
				string[] allSkillID = this._aiActionPattern[i].GetAllSkillID();
				foreach (string item in allSkillID)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			return list.ToArray();
		}

		public AIActionPattern GetCurrentActionPattern(CharacterStateControl characterState, int currentRound)
		{
			if (this.aiCycle == AICycle.fixableRotation)
			{
				return this.aiActionPattern[currentRound % this.aiActionPattern.Count];
			}
			float num = (float)(characterState.hp / characterState.maxHp);
			for (int i = 0; i < this.aiActionPattern.Count; i++)
			{
				if (num >= this.aiActionPattern[i].minRange && num < this.aiActionPattern[i].maxRange)
				{
					return this.aiActionPattern[i];
				}
			}
			return this.aiActionPattern[this.aiActionPattern.Count - 1];
		}

		public override string ToString()
		{
			string text = string.Format("[EnemyAIPattern: aiCycle={0}]", this.aiCycle);
			for (int i = 0; i < this._aiActionPattern.Count; i++)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"++",
					i.ToString(),
					" : ",
					this._aiActionPattern[i].ToString(),
					"\n"
				});
			}
			return text;
		}
	}
}
