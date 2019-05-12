using System;
using UnityEngine;

public class LeaderSkillResult
{
	private CharacterDatas _leaderCharacterData;

	private CharacterStateControl _characterStateControl;

	private LeaderSkillStatus _leaderSkillStatus;

	public LeaderSkillResult(CharacterStateControl characterStateControl, LeaderSkillStatus leaderSkillStatus = null, CharacterDatas leaderCharacterData = null)
	{
		this._characterStateControl = characterStateControl;
		this._leaderSkillStatus = leaderSkillStatus;
		this._leaderCharacterData = leaderCharacterData;
		if (this._leaderSkillStatus != null && !this._leaderSkillStatus.isHaving)
		{
			this._leaderSkillStatus = null;
		}
	}

	private bool isHpFollowing
	{
		get
		{
			return this._characterStateControl.hp <= Mathf.FloorToInt((float)this._characterStateControl.maxHp * this._leaderSkillStatus.hpFollowingPercent);
		}
	}

	private bool isHpMax
	{
		get
		{
			return this._characterStateControl.hp == this._characterStateControl.maxHp;
		}
	}

	private bool isSpeciesMach
	{
		get
		{
			return this._characterStateControl.characterDatas.tribe == this._leaderCharacterData.tribe;
		}
	}

	public float attackUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingAttackUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxAttackUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachAttackUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.AttackUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float defenceUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingDefenceUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxDefenceUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachDefenceUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.DefenceUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float specialAttackUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingSpecialAttackUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxSpecialAttackUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachSpecialAttackUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpecialAttackUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float specialDefenceUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingSpecialDefenceUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxSpecialDefenceUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachSpecialDefenceUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpecialDefenceUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float speedUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingSpeedUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxSpeedUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachSpeedUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeedUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float hitRateUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingHitRateUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxHitRateUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachHitRateUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HitRateUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float satisfactionRateUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingSatisfactionRateUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxMachSatisfactionRateUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachSatisfactionRateUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SatisfactionRateUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float hpUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachHpUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public float damageUpPercent
	{
		get
		{
			if (this._leaderSkillStatus != null)
			{
				if (this._leaderSkillStatus.type == LeaderSkillType.HpFollowingDamageUp && this.isHpFollowing)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.HpMaxDamageUp && this.isHpMax)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.SpeciesMachDamageUp && this.isSpeciesMach)
				{
					return this._leaderSkillStatus.upPercent;
				}
				if (this._leaderSkillStatus.type == LeaderSkillType.DamageUp)
				{
					return this._leaderSkillStatus.upPercent;
				}
			}
			return 0f;
		}
	}

	public int[] addTolerances
	{
		get
		{
			if (this._leaderSkillStatus != null && this._leaderSkillStatus.type == LeaderSkillType.ToleranceUp)
			{
				return this._leaderSkillStatus.addTolerances;
			}
			return null;
		}
	}
}
