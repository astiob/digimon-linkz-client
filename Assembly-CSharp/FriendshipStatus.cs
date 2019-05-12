using System;
using UnityEngine;

[Serializable]
public class FriendshipStatus
{
	public const int StatusUpInterval = 10;

	[SerializeField]
	private int _friendshipLevel;

	[SerializeField]
	private int _maxAttackPower;

	[SerializeField]
	private int _maxDefencePower;

	[SerializeField]
	private int _maxSpecialAttackPower;

	[SerializeField]
	private int _maxSpecialDefencePower;

	[SerializeField]
	private int _maxSpeed;

	public FriendshipStatus(int friendshipLevel, int maxAttackPower, int maxDefencePower, int maxSpecialAttackPower, int maxSpecialDefencePower, int maxSpeed)
	{
		this._friendshipLevel = friendshipLevel;
		this._maxAttackPower = maxAttackPower;
		this._maxSpecialAttackPower = maxDefencePower;
		this._maxSpecialAttackPower = maxSpecialAttackPower;
		this._maxSpecialDefencePower = maxSpecialDefencePower;
		this._maxSpeed = maxSpeed;
	}

	public FriendshipStatus()
	{
		this._friendshipLevel = 0;
		this._maxAttackPower = 0;
		this._maxSpecialAttackPower = 0;
		this._maxSpecialAttackPower = 0;
		this._maxSpecialDefencePower = 0;
		this._maxSpeed = 0;
	}

	public int friendshipLevel
	{
		get
		{
			return this._friendshipLevel;
		}
	}

	private int GetCalcedStatus(int based, int max)
	{
		return based;
	}

	public int GetCorrectedAttackPower(int attackPower)
	{
		return this.GetCalcedStatus(attackPower, this._maxAttackPower);
	}

	public int GetCorrectedDefencePower(int defencePower)
	{
		return this.GetCalcedStatus(defencePower, this._maxDefencePower);
	}

	public int GetCorrectedSpecialAttackPower(int specialAttackPower)
	{
		return this.GetCalcedStatus(specialAttackPower, this._maxSpecialAttackPower);
	}

	public int GetCorrectedSpecialDefencePower(int specialDefencePower)
	{
		return this.GetCalcedStatus(specialDefencePower, this._maxSpecialDefencePower);
	}

	public int GetCorrectedSpeed(int speed)
	{
		return this.GetCalcedStatus(speed, this._maxSpeed);
	}
}
