using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.AI
{
	[Serializable]
	public class AIActionPattern
	{
		[SerializeField]
		private float _minRange;

		[SerializeField]
		private float _maxRange;

		[SerializeField]
		private List<AIActionClip> _aiActionClip = new List<AIActionClip>();

		public AIActionPattern()
		{
			this._minRange = 0f;
			this._maxRange = 1f;
			this._aiActionClip = new List<AIActionClip>();
		}

		public AIActionPattern(float minRange, float maxRange, params AIActionClip[] aiActionClip)
		{
			this._minRange = minRange;
			this._maxRange = maxRange;
			this._aiActionClip = new List<AIActionClip>(aiActionClip);
		}

		public float minRange
		{
			get
			{
				return this._minRange;
			}
			private set
			{
				this._minRange = value;
			}
		}

		public float maxRange
		{
			get
			{
				return this._maxRange;
			}
			private set
			{
				this._maxRange = value;
			}
		}

		public List<AIActionClip> aiActionClip
		{
			get
			{
				return this._aiActionClip;
			}
			private set
			{
				this._aiActionClip = value;
			}
		}

		public string[] GetAllSkillID()
		{
			List<string> list = new List<string>();
			foreach (AIActionClip aiactionClip in this._aiActionClip)
			{
				if (!list.Contains(aiactionClip.useSkillId))
				{
					list.Add(aiactionClip.useSkillId);
				}
			}
			return list.ToArray();
		}

		public AIActionClip GetRandomActionClip()
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			for (int i = 0; i < this.aiActionClip.Count; i++)
			{
				if (this.aiActionClip[i].minRange != this.aiActionClip[i].maxRange)
				{
					if (this.aiActionClip[i].minRange < num && this.aiActionClip[i].maxRange >= num)
					{
						return this.aiActionClip[i];
					}
				}
			}
			return this.aiActionClip[this.aiActionClip.Count - 1];
		}

		public override string ToString()
		{
			string text = string.Format("[AIActionPattern: minRange={0}, maxRange={1}]", this.minRange, this.maxRange) + "\n";
			for (int i = 0; i < this._aiActionClip.Count; i++)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"+++",
					i.ToString(),
					" : ",
					this._aiActionClip[i].ToString(),
					"\n"
				});
			}
			return text;
		}
	}
}
