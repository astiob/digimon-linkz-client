using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerStatus : CharacterStatus
{
	[SerializeField]
	private ToleranceShifter _arousalTolerance;

	[SerializeField]
	private int _luck = 1;

	[FormerlySerializedAs("_rarity")]
	[SerializeField]
	[Range(0f, 4f)]
	private int _arousal;

	[SerializeField]
	private string[] _skillIds = new string[]
	{
		string.Empty,
		string.Empty
	};

	[SerializeField]
	private string _thumbnailId = string.Empty;

	[SerializeField]
	private Talent _talent = new Talent();

	[SerializeField]
	private FriendshipStatus _friendshipStatus;

	public PlayerStatus(string prefabId, int hp, int attackPower, int defencePower, int specialAttackPower, int specialDefencePower, int speed, int level, string toleranceId, ToleranceShifter arousalTolerance, int luck, string deathblowId, string inheritanceTechniqueId, string leaderSkillId, string thumbnailId, Talent talent, int arousal, FriendshipStatus friendshipStatus, int[] chipIds) : base(prefabId, hp, attackPower, defencePower, specialAttackPower, specialDefencePower, speed, level, toleranceId, chipIds)
	{
		this._luck = luck;
		this._arousal = arousal;
		this._skillIds = new string[]
		{
			deathblowId,
			inheritanceTechniqueId
		};
		this._leaderSkillId = leaderSkillId;
		this._thumbnailId = thumbnailId;
		this._talent = talent;
		this._arousalTolerance = arousalTolerance;
		this._friendshipStatus = friendshipStatus;
	}

	public PlayerStatus()
	{
		this._luck = 1;
		this._arousal = 0;
		this._skillIds = new string[]
		{
			string.Empty,
			string.Empty
		};
		this._leaderSkillId = string.Empty;
		this._thumbnailId = string.Empty;
		this._talent = new Talent();
		this._arousalTolerance = new ToleranceShifter();
		this._friendshipStatus = new FriendshipStatus();
	}

	public int luck
	{
		get
		{
			return this._luck;
		}
	}

	public int arousal
	{
		get
		{
			return this._arousal;
		}
	}

	public string[] skillIds
	{
		get
		{
			return this._skillIds;
		}
	}

	public string deathblowId
	{
		get
		{
			return this._skillIds[0];
		}
	}

	public string inheritanceTechniqueId
	{
		get
		{
			return this._skillIds[1];
		}
	}

	public string thumbnailId
	{
		get
		{
			return this._thumbnailId;
		}
	}

	public Talent talent
	{
		get
		{
			return this._talent;
		}
	}

	public ToleranceShifter arousalTolerance
	{
		get
		{
			return this._arousalTolerance;
		}
	}

	public FriendshipStatus friendshipStatus
	{
		get
		{
			return this._friendshipStatus;
		}
	}

	public static bool Match(CharacterStatus characterStatus)
	{
		return characterStatus.GetType() == typeof(PlayerStatus);
	}
}
