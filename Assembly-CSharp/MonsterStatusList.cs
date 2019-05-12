using Master;
using Monster;
using System;
using UnityEngine;

public class MonsterStatusList : MonoBehaviour
{
	[SerializeField]
	private UILabel hpLabelTitle;

	[SerializeField]
	protected UILabel hpLabel;

	[SerializeField]
	private UILabel attackLabelTitle;

	[SerializeField]
	protected UILabel attackLabel;

	[SerializeField]
	private UILabel defenseLabelTitle;

	[SerializeField]
	protected UILabel defenseLabel;

	[SerializeField]
	private UILabel magicAttackLabelTitle;

	[SerializeField]
	protected UILabel magicAttackLabel;

	[SerializeField]
	private UILabel magicDefenceLabelTitle;

	[SerializeField]
	protected UILabel magicDefenceLabel;

	[SerializeField]
	private UILabel speedLabelTitle;

	[SerializeField]
	protected UILabel speedLabel;

	[SerializeField]
	private UILabel luckLabelTitle;

	[SerializeField]
	protected UILabel luckLabel;

	[SerializeField]
	private UILabel friendshipLabelTitle;

	[SerializeField]
	protected UILabel friendshipLabel;

	[SerializeField]
	private bool noTitle;

	[SerializeField]
	protected bool disableFriendshipMaxValue;

	private void Start()
	{
		if (!this.noTitle)
		{
			this.hpLabelTitle.text = StringMaster.GetString("CharaStatus-10");
			this.attackLabelTitle.text = StringMaster.GetString("CharaStatus-08");
			this.defenseLabelTitle.text = StringMaster.GetString("CharaStatus-09");
			this.magicAttackLabelTitle.text = StringMaster.GetString("CharaStatus-13");
			this.magicDefenceLabelTitle.text = StringMaster.GetString("CharaStatus-14");
			this.speedLabelTitle.text = StringMaster.GetString("CharaStatus-15");
			this.luckLabelTitle.text = StringMaster.GetString("CharaStatus-16");
			this.friendshipLabelTitle.text = StringMaster.GetString("CharaStatus-17");
		}
	}

	public virtual void ClearValues()
	{
		this.hpLabel.text = "0";
		this.attackLabel.text = "0";
		this.defenseLabel.text = "0";
		this.magicAttackLabel.text = "0";
		this.magicDefenceLabel.text = "0";
		this.speedLabel.text = "0";
		this.luckLabel.text = "0";
		this.friendshipLabel.text = "0";
	}

	public void SetValues(MonsterData monsterData, bool setBaseStatus = false, bool showMaxLuck = false)
	{
		if (!setBaseStatus)
		{
			this.hpLabel.text = monsterData.userMonster.hp;
			this.attackLabel.text = monsterData.userMonster.attack;
			this.defenseLabel.text = monsterData.userMonster.defense;
			this.magicAttackLabel.text = monsterData.userMonster.spAttack;
			this.magicDefenceLabel.text = monsterData.userMonster.spDefense;
			this.speedLabel.text = monsterData.userMonster.speed;
		}
		else
		{
			StatusValue statusValue = MonsterStatusData.GetStatusValue(monsterData.userMonster.monsterId, monsterData.userMonster.level);
			this.hpLabel.text = statusValue.hp.ToString();
			this.attackLabel.text = statusValue.attack.ToString();
			this.defenseLabel.text = statusValue.defense.ToString();
			this.magicAttackLabel.text = statusValue.magicAttack.ToString();
			this.magicDefenceLabel.text = statusValue.magicDefense.ToString();
			this.speedLabel.text = statusValue.speed.ToString();
		}
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(monsterData.userMonster.monsterId).Simple;
		if (showMaxLuck)
		{
			this.luckLabel.text = string.Format(StringMaster.GetString("SystemFraction"), monsterData.userMonster.luck, simple.maxLuck);
		}
		else
		{
			this.luckLabel.text = monsterData.userMonster.luck.ToString();
		}
		if (!this.disableFriendshipMaxValue)
		{
			this.friendshipLabel.text = MonsterFriendshipData.GetMaxFriendshipFormat(monsterData.userMonster.friendship, monsterData.monsterMG.growStep);
		}
		else
		{
			this.friendshipLabel.text = monsterData.userMonster.friendship;
		}
	}

	public void ClearEggCandidateMedalValues()
	{
		this.luckLabel.text = "0";
		this.friendshipLabel.text = "0";
		this.hpLabel.text = StringMaster.GetString("CharaStatusNoneWord");
		this.attackLabel.text = StringMaster.GetString("CharaStatusNoneWord");
		this.defenseLabel.text = StringMaster.GetString("CharaStatusNoneWord");
		this.magicAttackLabel.text = StringMaster.GetString("CharaStatusNoneWord");
		this.magicDefenceLabel.text = StringMaster.GetString("CharaStatusNoneWord");
		this.speedLabel.text = StringMaster.GetString("CharaStatusNoneWord");
	}

	public void SetEggCandidateMedalValues(MonsterEggStatusInfo eggStatusInfo)
	{
		this.SetCandidateMedalText(eggStatusInfo.hpAbilityFlg, this.hpLabel);
		this.SetCandidateMedalText(eggStatusInfo.attackAbilityFlg, this.attackLabel);
		this.SetCandidateMedalText(eggStatusInfo.defenseAbilityFlg, this.defenseLabel);
		this.SetCandidateMedalText(eggStatusInfo.spAttackAbilityFlg, this.magicAttackLabel);
		this.SetCandidateMedalText(eggStatusInfo.spDefenseAbilityFlg, this.magicDefenceLabel);
		this.SetCandidateMedalText(eggStatusInfo.speedAbilityFlg, this.speedLabel);
	}

	private void SetCandidateMedalText(ConstValue.CandidateMedal candidate, UILabel label)
	{
		label.text = this.GetCandidateMedalText(candidate);
		if (string.IsNullOrEmpty(label.text))
		{
			label.gameObject.SetActive(false);
		}
		else
		{
			label.gameObject.SetActive(true);
		}
	}

	private string GetCandidateMedalText(ConstValue.CandidateMedal candidate)
	{
		string result = string.Empty;
		if (candidate == ConstValue.CandidateMedal.SILVER_OR_NONE || candidate == ConstValue.CandidateMedal.GOLD_OR_NONE || candidate == ConstValue.CandidateMedal.NONE)
		{
			result = StringMaster.GetString("CharaStatusNoneWord");
		}
		return result;
	}

	public UILabel LuckLabel
	{
		get
		{
			return this.luckLabel;
		}
	}

	public UILabel FriendshipLabel
	{
		get
		{
			return this.friendshipLabel;
		}
	}

	public StatusValue GetValues()
	{
		return new StatusValue
		{
			hp = this.hpLabel.text.ToInt32(),
			attack = this.attackLabel.text.ToInt32(),
			defense = this.defenseLabel.text.ToInt32(),
			magicAttack = this.magicAttackLabel.text.ToInt32(),
			magicDefense = this.magicDefenceLabel.text.ToInt32(),
			speed = this.speedLabel.text.ToInt32(),
			luck = this.luckLabel.text.ToInt32(),
			friendship = this.friendshipLabel.text.ToInt32()
		};
	}
}
