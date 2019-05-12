using System;
using UnityEngine;

[Serializable]
public class Talent
{
	[SerializeField]
	private TalentLevel _hp;

	[SerializeField]
	private TalentLevel _attack;

	[SerializeField]
	private TalentLevel _defence;

	[SerializeField]
	private TalentLevel _specialAttack;

	[SerializeField]
	private TalentLevel _specialDefence;

	[SerializeField]
	private TalentLevel _speed;

	public Talent()
	{
		this._hp = TalentLevel.None;
		this._attack = TalentLevel.None;
		this._defence = TalentLevel.None;
		this._specialAttack = TalentLevel.None;
		this._specialDefence = TalentLevel.None;
		this._speed = TalentLevel.None;
	}

	public Talent(TalentLevel hp, TalentLevel attack, TalentLevel defence, TalentLevel specialAttack, TalentLevel specialDefence, TalentLevel speed)
	{
		this._hp = hp;
		this._attack = attack;
		this._defence = defence;
		this._specialAttack = specialAttack;
		this._specialDefence = specialDefence;
		this._speed = speed;
	}

	public TalentLevel hp
	{
		get
		{
			return this._hp;
		}
	}

	public TalentLevel attack
	{
		get
		{
			return this._attack;
		}
	}

	public TalentLevel defence
	{
		get
		{
			return this._defence;
		}
	}

	public TalentLevel specialAttack
	{
		get
		{
			return this._specialAttack;
		}
	}

	public TalentLevel specialDefence
	{
		get
		{
			return this._specialDefence;
		}
	}

	public TalentLevel speed
	{
		get
		{
			return this._speed;
		}
	}
}
