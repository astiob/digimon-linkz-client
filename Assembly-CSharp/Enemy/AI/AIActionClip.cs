using System;
using UnityEngine;

namespace Enemy.AI
{
	[Serializable]
	public class AIActionClip
	{
		[SerializeField]
		private TargetSelectReference _targetSelectRerefence;

		[SerializeField]
		private SelectingOrder _selectingOrder;

		[SerializeField]
		private float _minValue;

		[SerializeField]
		private float _maxValue;

		[SerializeField]
		private float _minRange;

		[SerializeField]
		private float _maxRange;

		[SerializeField]
		private string _useSkillId = string.Empty;

		public AIActionClip()
		{
			this._targetSelectRerefence = TargetSelectReference.Hp;
			this._selectingOrder = SelectingOrder.HighAndHave;
			this._minValue = 0f;
			this._maxValue = 1f;
			this._minRange = 0f;
			this._maxRange = 1f;
			this._useSkillId = string.Empty;
		}

		public AIActionClip(TargetSelectReference targetSelectRefelence, SelectingOrder selectingOrder, float minValue, float maxValue, float minRange, float maxRange, string useSkillId)
		{
			this._targetSelectRerefence = targetSelectRefelence;
			this._selectingOrder = selectingOrder;
			this._minValue = minValue;
			this._maxValue = maxValue;
			this._minRange = minRange;
			this._maxRange = maxRange;
			this._useSkillId = useSkillId;
		}

		public TargetSelectReference targetSelectRerefence
		{
			get
			{
				return this._targetSelectRerefence;
			}
			set
			{
				this._targetSelectRerefence = value;
			}
		}

		public SelectingOrder selectingOrder
		{
			get
			{
				return this._selectingOrder;
			}
			set
			{
				this._selectingOrder = value;
			}
		}

		public float minValue
		{
			get
			{
				return this._minValue;
			}
			set
			{
				this._minValue = value;
			}
		}

		public float maxValue
		{
			get
			{
				return this._maxValue;
			}
			set
			{
				this._maxValue = value;
			}
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
				this._minRange = value;
			}
		}

		public string useSkillId
		{
			get
			{
				return this._useSkillId;
			}
			private set
			{
				this._useSkillId = value;
			}
		}

		public bool IsFindMachConditionTargets(params CharacterStateControl[] targets)
		{
			foreach (CharacterStateControl characterStateControl in targets)
			{
				TargetSelectReference targetSelectRerefence = this.targetSelectRerefence;
				switch (targetSelectRerefence)
				{
				case TargetSelectReference.AttributeRed:
					foreach (SkillStatus skillStatus2 in characterStateControl.skillStatus)
					{
						if (skillStatus2.AttributeMachLevel(global::Attribute.Red) > 0)
						{
							return true;
						}
					}
					break;
				case TargetSelectReference.AttributeBlue:
					foreach (SkillStatus skillStatus4 in characterStateControl.skillStatus)
					{
						if (skillStatus4.AttributeMachLevel(global::Attribute.Blue) > 0)
						{
							return true;
						}
					}
					break;
				case TargetSelectReference.AttributeYellow:
					foreach (SkillStatus skillStatus6 in characterStateControl.skillStatus)
					{
						if (skillStatus6.AttributeMachLevel(global::Attribute.Yellow) > 0)
						{
							return true;
						}
					}
					break;
				case TargetSelectReference.AttributeGreen:
					foreach (SkillStatus skillStatus8 in characterStateControl.skillStatus)
					{
						if (skillStatus8.AttributeMachLevel(global::Attribute.Green) > 0)
						{
							return true;
						}
					}
					break;
				case TargetSelectReference.AttributeWhite:
					foreach (SkillStatus skillStatus10 in characterStateControl.skillStatus)
					{
						if (skillStatus10.AttributeMachLevel(global::Attribute.White) > 0)
						{
							return true;
						}
					}
					break;
				case TargetSelectReference.AttributeBlack:
					foreach (SkillStatus skillStatus12 in characterStateControl.skillStatus)
					{
						if (skillStatus12.AttributeMachLevel(global::Attribute.Black) > 0)
						{
							return true;
						}
					}
					break;
				default:
					if (targetSelectRerefence != TargetSelectReference.Hp)
					{
						return true;
					}
					if (characterStateControl.GetHpRemainingAmoutRange(this.minValue, this.maxValue))
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("[AIActionClip: targetSelectRerefence={0}, selectingOrder={1}, minValue={2}, maxValue={3}, minRange={4}, maxRange={5}, useSkillId={6}]", new object[]
			{
				this.targetSelectRerefence,
				this.selectingOrder,
				this.minValue,
				this.maxValue,
				this.minRange,
				this.maxRange,
				this.useSkillId
			});
		}
	}
}
