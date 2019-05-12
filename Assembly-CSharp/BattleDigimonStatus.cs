using Master;
using System;
using UnityEngine;

public sealed class BattleDigimonStatus : BattleDigimonStatusBase
{
	[SerializeField]
	private GameObject stageGimmickObject;

	[SerializeField]
	private UILabel level;

	[SerializeField]
	private UILabel hp;

	[SerializeField]
	private GameObject hpUpObject;

	[SerializeField]
	private GameObject hpDownObject;

	[SerializeField]
	private UILabel atk;

	[SerializeField]
	private GameObject atkUpObject;

	[SerializeField]
	private GameObject atkDownObject;

	[SerializeField]
	private UILabel def;

	[SerializeField]
	private GameObject defUpObject;

	[SerializeField]
	private GameObject defDownObject;

	[SerializeField]
	private UILabel satk;

	[SerializeField]
	private GameObject satkUpObject;

	[SerializeField]
	private GameObject satkDownObject;

	[SerializeField]
	private UILabel sdef;

	[SerializeField]
	private GameObject sdefUpObject;

	[SerializeField]
	private GameObject sdefDownObject;

	[SerializeField]
	private UILabel speed;

	[SerializeField]
	private GameObject speedUpObject;

	[SerializeField]
	private GameObject speedDownObject;

	[SerializeField]
	private UILabel luck;

	[SerializeField]
	private UILabel leaderSkillName;

	[SerializeField]
	private UILabel friendshipLevel;

	[SerializeField]
	private UILabel deathblowName;

	[SerializeField]
	private UILabel inheritanceName;

	[SerializeField]
	private UILabel leaderSkillDescription;

	[SerializeField]
	private UILabel deathblowDescription;

	[SerializeField]
	private GameObject deathblowUpObject;

	[SerializeField]
	private GameObject deathblowDownObject;

	[SerializeField]
	private UILabel inheritanceDescription;

	[SerializeField]
	private GameObject inheritanceUpObject;

	[SerializeField]
	private GameObject inheritanceDownObject;

	[SerializeField]
	private UIComponentSkinner arousal;

	[SerializeField]
	private UIComponentSkinner talentHP;

	[SerializeField]
	private UIComponentSkinner talentATK;

	[SerializeField]
	private UIComponentSkinner talentDEF;

	[SerializeField]
	private UIComponentSkinner talentSATK;

	[SerializeField]
	private UIComponentSkinner talentSDEF;

	[SerializeField]
	private UIComponentSkinner talentSPD;

	[SerializeField]
	[Header("Lvローカライズ")]
	private UILabel lvLocalize;

	[Header("HPローカライズ")]
	[SerializeField]
	private UILabel hpLocalize;

	[Header("友情度ローカライズ")]
	[SerializeField]
	private UILabel friendLocalize;

	[SerializeField]
	[Header("ATKローカライズ")]
	private UILabel atkLocalize;

	[Header("DEFローカライズ")]
	[SerializeField]
	private UILabel defLocalize;

	[Header("SATKローカライズ")]
	[SerializeField]
	private UILabel satkLocalize;

	[Header("SDEFローカライズ")]
	[SerializeField]
	private UILabel sdefLocalize;

	[Header("SPDローカライズ")]
	[SerializeField]
	private UILabel spdLocalize;

	[Header("Luckローカライズ")]
	[SerializeField]
	private UILabel luckLocalize;

	[SerializeField]
	[Header("リーダースキルローカライズ")]
	private UILabel lSkillLocalize;

	[Header("固有技ローカライズ")]
	[SerializeField]
	private UILabel skill1Localize;

	[Header("継承技ローカライズ")]
	[SerializeField]
	private UILabel skill2Localize;

	[SerializeField]
	[Header("装着チップ")]
	private ChipIcon[] chipIcons;

	private void Awake()
	{
		this.SetupLocalize();
	}

	protected override void SetupLocalize()
	{
		this.lvLocalize.text = StringMaster.GetString("CharaStatus-11");
		this.hpLocalize.text = StringMaster.GetString("CharaStatus-10");
		this.friendLocalize.text = StringMaster.GetString("CharaStatus-17");
		this.atkLocalize.text = StringMaster.GetString("CharaStatus-08");
		this.defLocalize.text = StringMaster.GetString("CharaStatus-09");
		this.satkLocalize.text = StringMaster.GetString("CharaStatus-13");
		this.sdefLocalize.text = StringMaster.GetString("CharaStatus-14");
		this.spdLocalize.text = StringMaster.GetString("CharaStatus-15");
		this.luckLocalize.text = StringMaster.GetString("CharaStatus-16");
		this.lSkillLocalize.text = StringMaster.GetString("CharaStatus-21");
		this.skill1Localize.text = StringMaster.GetString("CharaStatus-19");
		this.skill2Localize.text = StringMaster.GetString("CharaStatus-20");
	}

	public void ApplyMonsterDescription(bool isShow, CharacterStateControl characterStatus)
	{
		NGUITools.SetActiveSelf(base.gameObject, isShow);
		if (!isShow)
		{
			return;
		}
		this.monsterName.text = characterStatus.name;
		this.level.text = characterStatus.level.ToString();
		base.SetupEvolutionStep(characterStatus);
		base.SetupSpecies(characterStatus);
		int num = 0;
		num += this.ApplyValue(characterStatus.defaultExtraMaxHp, characterStatus.extraMaxHp, this.hp, this.hpUpObject, this.hpDownObject);
		num += this.ApplyValue(characterStatus.playerStatus.attackPower + characterStatus.chipAddAttackPower, characterStatus.defaultExtraAttackPower, this.atk, this.atkUpObject, this.atkDownObject);
		num += this.ApplyValue(characterStatus.playerStatus.defencePower + characterStatus.chipAddDefencePower, characterStatus.defaultExtraDefencePower, this.def, this.defUpObject, this.defDownObject);
		num += this.ApplyValue(characterStatus.playerStatus.specialAttackPower + characterStatus.chipAddSpecialAttackPower, characterStatus.defaultExtraSpecialAttackPower, this.satk, this.satkUpObject, this.satkDownObject);
		num += this.ApplyValue(characterStatus.playerStatus.specialDefencePower + characterStatus.chipAddSpecialDefencePower, characterStatus.defaultExtraSpecialDefencePower, this.sdef, this.sdefUpObject, this.sdefDownObject);
		num += this.ApplyValue(characterStatus.playerStatus.speed + characterStatus.chipAddSpeed, characterStatus.defaultExtraSpeed, this.speed, this.speedUpObject, this.speedDownObject);
		this.luck.text = characterStatus.luck.ToString();
		if (characterStatus.isHavingLeaderSkill)
		{
			this.leaderSkillName.text = characterStatus.leaderSkillStatus.name;
		}
		else
		{
			this.leaderSkillName.text = StringMaster.GetString("SystemNone");
		}
		this.friendshipLevel.text = string.Format(StringMaster.GetString("SystemFraction"), characterStatus.friendshipLevel, characterStatus.maxFriendshipLevel);
		this.deathblowName.text = characterStatus.skillStatus[1].name;
		this.inheritanceName.text = characterStatus.skillStatus[2].name;
		this.leaderSkillDescription.text = characterStatus.leaderSkillStatus.description;
		this.deathblowDescription.text = characterStatus.skillStatus[1].description;
		this.inheritanceDescription.text = characterStatus.skillStatus[2].description;
		if (characterStatus.skillStatus.Length > 1)
		{
			int extraValue = characterStatus.skillStatus[1].power;
			AffectEffectProperty affectEffectFirst = characterStatus.skillStatus[1].GetAffectEffectFirst();
			if (affectEffectFirst != null && affectEffectFirst.type == AffectEffect.Damage)
			{
				extraValue = characterStatus.extraDeathblowSkillPower;
			}
			num += this.ApplyValue(characterStatus.skillStatus[1].power, extraValue, null, this.deathblowUpObject, this.deathblowDownObject);
		}
		else
		{
			this.ApplyValue(0, 0, null, this.deathblowUpObject, this.deathblowDownObject);
		}
		if (characterStatus.skillStatus.Length > 2)
		{
			int extraValue2 = characterStatus.skillStatus[2].power;
			AffectEffectProperty affectEffectFirst2 = characterStatus.skillStatus[2].GetAffectEffectFirst();
			if (affectEffectFirst2 != null && affectEffectFirst2.type == AffectEffect.Damage)
			{
				extraValue2 = characterStatus.extraInheritanceSkillPower;
			}
			num += this.ApplyValue(characterStatus.skillStatus[2].power, extraValue2, null, this.inheritanceUpObject, this.inheritanceDownObject);
		}
		else
		{
			this.ApplyValue(0, 0, null, this.inheritanceUpObject, this.inheritanceDownObject);
		}
		this.stageGimmickObject.SetActive(num > 0);
		this.arousal.SetSkins(characterStatus.arousal);
		base.SetupTolerance(characterStatus);
		int[] talentSetSkin = BattleUIControlBasic.GetTalentSetSkin(characterStatus.talent);
		this.talentHP.SetSkins(talentSetSkin[0]);
		this.talentATK.SetSkins(talentSetSkin[1]);
		this.talentDEF.SetSkins(talentSetSkin[2]);
		this.talentSATK.SetSkins(talentSetSkin[3]);
		this.talentSDEF.SetSkins(talentSetSkin[4]);
		this.talentSPD.SetSkins(talentSetSkin[5]);
		for (int i = 0; i < this.chipIcons.Length; i++)
		{
			if (i < characterStatus.chipIds.Length)
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(characterStatus.chipIds[i].ToString());
				this.chipIcons[i].SetData(chipMainData, -1, -1);
				this.chipIcons[i].SetActive(true);
			}
			else
			{
				this.chipIcons[i].SetData(null, -1, -1);
				this.chipIcons[i].SetActive(false);
			}
		}
	}

	private int ApplyValue(int defaultValue, int extraValue, UILabel text, GameObject upObject, GameObject downObject)
	{
		int result = 0;
		upObject.SetActive(false);
		downObject.SetActive(false);
		string text2 = defaultValue.ToString();
		Color effectColor = new Color32(0, 130, byte.MaxValue, byte.MaxValue);
		if (defaultValue < extraValue)
		{
			text2 = extraValue.ToString();
			effectColor = new Color32(0, 160, 0, byte.MaxValue);
			upObject.SetActive(true);
			result = 1;
		}
		else if (defaultValue > extraValue)
		{
			text2 = extraValue.ToString();
			effectColor = new Color32(0, 70, 240, byte.MaxValue);
			downObject.SetActive(true);
			result = 1;
		}
		if (text != null)
		{
			text.text = text2;
			text.effectColor = effectColor;
		}
		return result;
	}
}
