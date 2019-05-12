using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class MonsterMedalList : MonoBehaviour
{
	[SerializeField]
	private UISprite hpIcon;

	[SerializeField]
	private UISprite attackIcon;

	[SerializeField]
	private UISprite defenseIcon;

	[SerializeField]
	private UISprite magicAttackIcon;

	[SerializeField]
	private UISprite magicDefenseIcon;

	[SerializeField]
	private UISprite speedIcon;

	private Dictionary<string, string> medalLevelTable;

	private void Awake()
	{
		this.SetMedalTable();
	}

	private void SetMedalTable()
	{
		this.medalLevelTable = new Dictionary<string, string>();
		this.medalLevelTable.Add("5", "1");
		this.medalLevelTable.Add("6", "2");
		this.medalLevelTable.Add("7", "3");
		this.medalLevelTable.Add("8", "4");
		this.medalLevelTable.Add("9", "5");
		this.medalLevelTable.Add("10", "6");
		this.medalLevelTable.Add("15", "1");
		this.medalLevelTable.Add("16", "2");
		this.medalLevelTable.Add("17", "3");
		this.medalLevelTable.Add("18", "4");
		this.medalLevelTable.Add("19", "5");
		this.medalLevelTable.Add("20", "6");
	}

	public void SetActive(bool isActive)
	{
		this.hpIcon.gameObject.SetActive(isActive);
		this.attackIcon.gameObject.SetActive(isActive);
		this.defenseIcon.gameObject.SetActive(isActive);
		this.magicAttackIcon.gameObject.SetActive(isActive);
		this.magicDefenseIcon.gameObject.SetActive(isActive);
		this.speedIcon.gameObject.SetActive(isActive);
		if (isActive)
		{
			this.ResetMedalColor(this.hpIcon);
			this.ResetMedalColor(this.attackIcon);
			this.ResetMedalColor(this.defenseIcon);
			this.ResetMedalColor(this.magicAttackIcon);
			this.ResetMedalColor(this.magicDefenseIcon);
			this.ResetMedalColor(this.speedIcon);
		}
	}

	private void ResetMedalColor(UIWidget widget)
	{
		Color color = widget.color;
		color.a = 1f;
		widget.color = color;
	}

	public void SetValues(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterData)
	{
		this.SetMedalIcon(userMonsterData.hpAbilityFlg, userMonsterData.hpAbility, this.hpIcon);
		this.SetMedalIcon(userMonsterData.attackAbilityFlg, userMonsterData.attackAbility, this.attackIcon);
		this.SetMedalIcon(userMonsterData.defenseAbilityFlg, userMonsterData.defenseAbility, this.defenseIcon);
		this.SetMedalIcon(userMonsterData.spAttackAbilityFlg, userMonsterData.spAttackAbility, this.magicAttackIcon);
		this.SetMedalIcon(userMonsterData.spDefenseAbilityFlg, userMonsterData.spDefenseAbility, this.magicDefenseIcon);
		this.SetMedalIcon(userMonsterData.speedAbilityFlg, userMonsterData.speedAbility, this.speedIcon);
	}

	public void SetValues(Talent monsterMedale)
	{
		this.SetMedalIcon(monsterMedale.hpAbilityFlg, monsterMedale.hpAbility, this.hpIcon);
		this.SetMedalIcon(monsterMedale.attackAbilityFlg, monsterMedale.attackAbility, this.attackIcon);
		this.SetMedalIcon(monsterMedale.defenseAbilityFlg, monsterMedale.defenseAbility, this.defenseIcon);
		this.SetMedalIcon(monsterMedale.spAttackAbilityFlg, monsterMedale.spAttackAbility, this.magicAttackIcon);
		this.SetMedalIcon(monsterMedale.spDefenseAbilityFlg, monsterMedale.spDefenseAbility, this.magicDefenseIcon);
		this.SetMedalIcon(monsterMedale.speedAbilityFlg, monsterMedale.speedAbility, this.speedIcon);
	}

	private void SetMedalIcon(string medalType, string medalPercentage, UISprite iconSprite)
	{
		string empty = string.Empty;
		if (medalPercentage == null)
		{
			medalPercentage = string.Empty;
		}
		if (this.medalLevelTable == null)
		{
			this.SetMedalTable();
		}
		this.medalLevelTable.TryGetValue(medalPercentage, out empty);
		iconSprite.spriteName = MonsterDetailUtil.GetMedalSpriteName(medalType);
		if (string.IsNullOrEmpty(iconSprite.spriteName))
		{
			iconSprite.gameObject.SetActive(false);
		}
		else
		{
			iconSprite.spriteName += empty;
			iconSprite.gameObject.SetActive(true);
		}
	}

	public UISprite HpIcon
	{
		get
		{
			return this.hpIcon;
		}
	}

	public UISprite AttackIcon
	{
		get
		{
			return this.attackIcon;
		}
	}

	public UISprite DefenseIcon
	{
		get
		{
			return this.defenseIcon;
		}
	}

	public UISprite MagicAttackIcon
	{
		get
		{
			return this.magicAttackIcon;
		}
	}

	public UISprite MagicDefenseIcon
	{
		get
		{
			return this.magicDefenseIcon;
		}
	}

	public UISprite SpeedIcon
	{
		get
		{
			return this.speedIcon;
		}
	}
}
