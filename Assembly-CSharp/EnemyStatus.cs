using Enemy.AI;
using Enemy.DropItem;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatus : CharacterStatus
{
	[SerializeField]
	private EnemyAIPattern _enemyAiPattern = new EnemyAIPattern();

	[SerializeField]
	private int _getChip;

	[SerializeField]
	private int _getExp;

	[SerializeField]
	private EnemyDropItemPattern _dropItemPattern = new EnemyDropItemPattern(new DropAssetPattern[0]);

	[NonSerialized]
	private List<ItemDropResult> _itemDropResult = new List<ItemDropResult>();

	public EnemyStatus(string prefabId, string groupId, int hp, int attackPower, int defencePower, int specialAttackPower, int specialDefencePower, int speed, int level, Tolerance tolerance, EnemyAIPattern enemyAiPattern, int getChip, int getExp, List<ItemDropResult> itemDropResult, string[] skillIds, int[] chipIdList, string[] monsterIntegrationIds) : base(prefabId, groupId, hp, attackPower, defencePower, specialAttackPower, specialDefencePower, speed, level, tolerance, skillIds, chipIdList, monsterIntegrationIds)
	{
		this._enemyAiPattern = enemyAiPattern;
		this._getChip = getChip;
		this._getExp = getExp;
		this._dropItemPattern = new EnemyDropItemPattern(new DropAssetPattern[0]);
		this._itemDropResult = itemDropResult;
	}

	public EnemyStatus()
	{
		this._enemyAiPattern = new EnemyAIPattern();
		this._getChip = 0;
		this._getExp = 0;
		this._dropItemPattern = new EnemyDropItemPattern(new DropAssetPattern[0]);
		base.skillIds = new string[0];
	}

	public EnemyAIPattern enemyAiPattern
	{
		get
		{
			return this._enemyAiPattern;
		}
	}

	public int getChip
	{
		get
		{
			return this._getChip;
		}
	}

	public int getExp
	{
		get
		{
			return this._getExp;
		}
	}

	public EnemyDropItemPattern dropItemPattern
	{
		get
		{
			return this._dropItemPattern;
		}
	}

	public List<ItemDropResult> itemDropResult
	{
		get
		{
			return this._itemDropResult;
		}
		set
		{
			this._itemDropResult = value;
		}
	}
}
