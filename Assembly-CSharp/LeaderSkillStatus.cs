using System;
using UnityEngine;

[Serializable]
public class LeaderSkillStatus
{
	[SerializeField]
	private string _name;

	[Multiline(1)]
	[SerializeField]
	private string _description;

	[SerializeField]
	private LeaderSkillType _type;

	[SerializeField]
	private float[] _floatValue;

	[SerializeField]
	private int[] _addTolerances;

	private bool _isHaving;

	public LeaderSkillStatus()
	{
		float[] array = new float[2];
		array[0] = 1f;
		this._floatValue = array;
		this._isHaving = true;
		base..ctor();
		this._name = string.Empty;
		this._description = string.Empty;
		this._type = LeaderSkillType.HpFollowingDamageUp;
		this._floatValue[0] = 0f;
		this._floatValue[1] = 0f;
		this._addTolerances = null;
		this._isHaving = true;
	}

	public LeaderSkillStatus(string name, string description, LeaderSkillType type, float hpFollowingPercent, float upPercent, Tolerance tolerance)
	{
		float[] array = new float[2];
		array[0] = 1f;
		this._floatValue = array;
		this._isHaving = true;
		base..ctor();
		this._name = name;
		this._description = description;
		this._type = type;
		this._floatValue[0] = Mathf.Clamp01(hpFollowingPercent);
		this._floatValue[1] = Mathf.Clamp01(upPercent);
		this._addTolerances = this.addTolerances;
		this._isHaving = (name != null);
	}

	public static LeaderSkillStatus GetUnHavingLeaderSkill()
	{
		return new LeaderSkillStatus(null, null, LeaderSkillType.HpFollowingAttackUp, 0f, 0f, null);
	}

	public string name
	{
		get
		{
			return this._name;
		}
	}

	public string description
	{
		get
		{
			return this._description;
		}
	}

	public LeaderSkillType type
	{
		get
		{
			return this._type;
		}
	}

	public int[] addTolerances
	{
		get
		{
			return this._addTolerances;
		}
	}

	public float hpFollowingPercent
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float upPercent
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public bool isHaving
	{
		get
		{
			return this._isHaving;
		}
	}
}
