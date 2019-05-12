using System;
using UnityEngine;

[Serializable]
public class CharacterStatus
{
	[SerializeField]
	protected string _prefabId;

	[SerializeField]
	protected int _hp = 40;

	[SerializeField]
	protected int _attackPower = 1000;

	[SerializeField]
	protected int _defencePower = 1000;

	[SerializeField]
	protected int _specialAttackPower = 1000;

	[SerializeField]
	protected int _specialDefencePower = 1000;

	[SerializeField]
	protected int _speed = 100;

	[SerializeField]
	protected int _level = 1;

	[SerializeField]
	protected string _toleranceId;

	[SerializeField]
	protected string _leaderSkillId = string.Empty;

	public CharacterStatus(string prefabId, int hp, int attackPower, int defencePower, int specialAttackPower, int specialDefencePower, int speed, int level, string toleranceId, int[] chipIdList)
	{
		this._prefabId = prefabId;
		this._hp = hp;
		this._attackPower = attackPower;
		this._defencePower = defencePower;
		this._specialAttackPower = specialAttackPower;
		this._specialDefencePower = specialDefencePower;
		this._speed = speed;
		this._level = level;
		this._toleranceId = toleranceId;
		this.chipIds = chipIdList;
		Array.Sort<int>(this.chipIds);
	}

	public CharacterStatus()
	{
		this._prefabId = string.Empty;
		this._hp = 40;
		this._attackPower = 1000;
		this._defencePower = 1000;
		this._specialAttackPower = 1000;
		this._specialDefencePower = 1000;
		this._speed = 100;
		this._level = 1;
		this._attackPower = this.attackPower;
		this.chipIds = new int[0];
	}

	public CharacterStatus(CharacterStatus characterStatus)
	{
		this._prefabId = characterStatus._prefabId;
		this._hp = characterStatus._hp;
		this._attackPower = characterStatus._attackPower;
		this._defencePower = characterStatus._defencePower;
		this._specialAttackPower = characterStatus._specialAttackPower;
		this._specialDefencePower = characterStatus._specialDefencePower;
		this._speed = characterStatus._speed;
		this._level = characterStatus._level;
		this.chipIds = characterStatus.chipIds;
	}

	public string prefabId
	{
		get
		{
			return this._prefabId;
		}
	}

	public int maxHp
	{
		get
		{
			return this._hp;
		}
	}

	public int attackPower
	{
		get
		{
			return this._attackPower;
		}
	}

	public int defencePower
	{
		get
		{
			return this._defencePower;
		}
	}

	public int specialAttackPower
	{
		get
		{
			return this._specialAttackPower;
		}
	}

	public int specialDefencePower
	{
		get
		{
			return this._specialDefencePower;
		}
	}

	public int speed
	{
		get
		{
			return this._speed;
		}
	}

	public int level
	{
		get
		{
			return this._level;
		}
	}

	public string toleranceId
	{
		get
		{
			return this._toleranceId;
		}
	}

	public string leaderSkillId
	{
		get
		{
			return this._leaderSkillId;
		}
	}

	public bool isHavingLeaderSkill
	{
		get
		{
			return !this.leaderSkillId.Equals(string.Empty);
		}
	}

	public int[] chipIds { get; private set; }

	public CharacterStatus ToCharacterStatus()
	{
		return new CharacterStatus(this._prefabId, this._hp, this._attackPower, this._defencePower, this._specialAttackPower, this._specialDefencePower, this._speed, this._level, this._toleranceId, this.chipIds);
	}
}
