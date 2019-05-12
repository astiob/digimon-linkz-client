using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerStatus : CharacterStatus
{
	[SerializeField]
	private int _luck = 1;

	[SerializeField]
	[FormerlySerializedAs("_rarity")]
	private int _arousal;

	[SerializeField]
	private string _thumbnailId = string.Empty;

	[SerializeField]
	private Talent _talent = new Talent();

	[SerializeField]
	private FriendshipStatus _friendshipStatus;

	public PlayerStatus(string userMonsterId, string prefabId, string groupId, int hp, int attackPower, int defencePower, int specialAttackPower, int specialDefencePower, int speed, int level, Tolerance tolerance, int luck, string leaderSkillId, string thumbnailId, Talent talent, int arousal, FriendshipStatus friendshipStatus, string[] skillIds, int[] chipIds, string[] monsterIntegrationIds) : base(prefabId, groupId, hp, attackPower, defencePower, specialAttackPower, specialDefencePower, speed, level, tolerance, skillIds, chipIds, monsterIntegrationIds)
	{
		this.userMonsterId = userMonsterId;
		this._luck = luck;
		this._arousal = arousal;
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

	public string userMonsterId { get; private set; }

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
