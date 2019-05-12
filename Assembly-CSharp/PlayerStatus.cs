using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerStatus : CharacterStatus
{
	[SerializeField]
	private int _luck = 1;

	[FormerlySerializedAs("_rarity")]
	[SerializeField]
	private int _arousal;

	[SerializeField]
	private string _thumbnailId = string.Empty;

	[SerializeField]
	private Talent _talent = new Talent();

	[SerializeField]
	private FriendshipStatus _friendshipStatus;

	public PlayerStatus(string prefabId, int hp, int attackPower, int defencePower, int specialAttackPower, int specialDefencePower, int speed, int level, string toleranceId, Tolerance tolerance, int luck, string deathblowId, string inheritanceTechniqueId, string inheritanceTechniqueId2, string leaderSkillId, string thumbnailId, Talent talent, int arousal, FriendshipStatus friendshipStatus, int[] chipIds) : base(prefabId, hp, attackPower, defencePower, specialAttackPower, specialDefencePower, speed, level, toleranceId, tolerance, chipIds)
	{
		this._luck = luck;
		this._arousal = arousal;
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(deathblowId))
		{
			list.Add(deathblowId);
		}
		if (!string.IsNullOrEmpty(inheritanceTechniqueId))
		{
			list.Add(inheritanceTechniqueId);
		}
		if (!string.IsNullOrEmpty(inheritanceTechniqueId2) && inheritanceTechniqueId2 != "0")
		{
			list.Add(inheritanceTechniqueId2);
		}
		base.skillIds = list.ToArray();
		this._leaderSkillId = leaderSkillId;
		this._thumbnailId = thumbnailId;
		this._talent = talent;
		this._friendshipStatus = friendshipStatus;
	}

	public PlayerStatus()
	{
		this._luck = 1;
		this._arousal = 0;
		base.skillIds = new string[]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};
		this._leaderSkillId = string.Empty;
		this._thumbnailId = string.Empty;
		this._talent = new Talent();
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

	public string deathblowId
	{
		get
		{
			return base.skillIds[0];
		}
	}

	public string inheritanceTechniqueId
	{
		get
		{
			return base.skillIds[1];
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
