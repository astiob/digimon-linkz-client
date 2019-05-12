using Master;
using Monster;
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
	private UILabel leaderSkillDescription;

	[SerializeField]
	[Header("特化型")]
	private UILabel specificType;

	[Header("固有技")]
	[SerializeField]
	private BattleDigimonStatus.Skill deathblow;

	[SerializeField]
	[Header("継承技1")]
	private BattleDigimonStatus.Skill inheritance1;

	[SerializeField]
	[Header("継承技2")]
	private BattleDigimonStatus.Skill inheritance2;

	[SerializeField]
	private UISprite arousalIcon;

	[SerializeField]
	[Header("各才能メダルの表示切り替え")]
	private MonsterMedalList MonsterMedalList;

	[Header("Lvローカライズ")]
	[SerializeField]
	private UILabel lvLocalize;

	[SerializeField]
	[Header("HPローカライズ")]
	private UILabel hpLocalize;

	[Header("友情度ローカライズ")]
	[SerializeField]
	private UILabel friendLocalize;

	[SerializeField]
	[Header("ATKローカライズ")]
	private UILabel atkLocalize;

	[SerializeField]
	[Header("DEFローカライズ")]
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

	[SerializeField]
	[Header("Luckローカライズ")]
	private UILabel luckLocalize;

	[Header("リーダースキルローカライズ")]
	[SerializeField]
	private UILabel lSkillLocalize;

	[SerializeField]
	[Header("装着チップ")]
	private ChipIcon[] chipIcons;

	private void Awake()
	{
		this.SetupLocalize();
	}

	protected override void SetupLocalize()
	{
		base.SetupLocalize();
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
		this.deathblow.title.text = StringMaster.GetString("CharaStatus-19");
		this.inheritance1.title.text = StringMaster.GetString("SkillInheritTitle1");
		this.inheritance2.title.text = StringMaster.GetString("SkillInheritTitle2");
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
		num += this.ApplyValue(characterStatus.maxHp, characterStatus.extraMaxHp, this.hp, this.hpUpObject, this.hpDownObject);
		num += this.ApplyValue(characterStatus.attackPower, characterStatus.extraAttackPower, this.atk, this.atkUpObject, this.atkDownObject);
		num += this.ApplyValue(characterStatus.defencePower, characterStatus.extraDefencePower, this.def, this.defUpObject, this.defDownObject);
		num += this.ApplyValue(characterStatus.specialAttackPower, characterStatus.extraSpecialAttackPower, this.satk, this.satkUpObject, this.satkDownObject);
		num += this.ApplyValue(characterStatus.specialDefencePower, characterStatus.extraSpecialDefencePower, this.sdef, this.sdefUpObject, this.sdefDownObject);
		num += this.ApplyValue(characterStatus.speed, characterStatus.extraSpeed, this.speed, this.speedUpObject, this.speedDownObject);
		this.luck.text = characterStatus.luck.ToString();
		if (MonsterDataMng.Instance() != null)
		{
			this.friendshipLevel.text = MonsterFriendshipData.GetMaxFriendshipFormat(characterStatus.friendshipLevel.ToString(), MonsterGrowStepData.ToGrowStepString(characterStatus.characterDatas.growStep));
		}
		if (characterStatus.isHavingLeaderSkill)
		{
			this.leaderSkillName.text = characterStatus.leaderSkillStatus.name;
			this.leaderSkillDescription.text = characterStatus.leaderSkillStatus.description;
		}
		else
		{
			this.leaderSkillName.text = StringMaster.GetString("SystemNone");
			this.leaderSkillDescription.text = string.Empty;
		}
		if (characterStatus.skillStatus.Length > 1)
		{
			this.deathblow.name.text = characterStatus.skillStatus[1].name;
			this.deathblow.description.text = characterStatus.skillStatus[1].description;
			int powerFirst = characterStatus.skillStatus[1].GetPowerFirst(characterStatus);
			int extraValue = powerFirst;
			AffectEffectProperty affectEffectFirst = characterStatus.skillStatus[1].GetAffectEffectFirst();
			if (affectEffectFirst != null && AffectEffectProperty.IsDamage(affectEffectFirst.type))
			{
				extraValue = characterStatus.extraDeathblowSkillPower;
			}
			num += this.ApplyValue(powerFirst, extraValue, null, this.deathblow.upObject, this.deathblow.downObject);
		}
		else
		{
			this.deathblow.name.text = StringMaster.GetString("SystemNone");
			this.deathblow.description.text = "-";
			this.ApplyValue(0, 0, null, this.deathblow.upObject, this.deathblow.downObject);
		}
		if (characterStatus.skillStatus.Length > 2)
		{
			this.inheritance1.name.text = characterStatus.skillStatus[2].name;
			this.inheritance1.description.text = characterStatus.skillStatus[2].description;
			int powerFirst2 = characterStatus.skillStatus[2].GetPowerFirst(characterStatus);
			int extraValue2 = powerFirst2;
			AffectEffectProperty affectEffectFirst2 = characterStatus.skillStatus[2].GetAffectEffectFirst();
			if (affectEffectFirst2 != null && AffectEffectProperty.IsDamage(affectEffectFirst2.type))
			{
				extraValue2 = characterStatus.extraInheritanceSkillPower;
			}
			num += this.ApplyValue(powerFirst2, extraValue2, null, this.inheritance1.upObject, this.inheritance1.downObject);
		}
		else
		{
			this.inheritance1.name.text = StringMaster.GetString("SystemNone");
			this.inheritance1.description.text = "-";
			this.ApplyValue(0, 0, null, this.inheritance1.upObject, this.inheritance1.downObject);
		}
		if (characterStatus.isVersionUp)
		{
			this.inheritance2.root.SetActive(true);
		}
		else
		{
			this.inheritance2.root.SetActive(false);
		}
		if (characterStatus.skillStatus.Length > 3)
		{
			this.inheritance2.name.text = characterStatus.skillStatus[3].name;
			this.inheritance2.description.text = characterStatus.skillStatus[3].description;
			int powerFirst3 = characterStatus.skillStatus[3].GetPowerFirst(characterStatus);
			int extraValue3 = powerFirst3;
			AffectEffectProperty affectEffectFirst3 = characterStatus.skillStatus[3].GetAffectEffectFirst();
			if (affectEffectFirst3 != null && AffectEffectProperty.IsDamage(affectEffectFirst3.type))
			{
				extraValue3 = characterStatus.extraInheritanceSkillPower2;
			}
			num += this.ApplyValue(powerFirst3, extraValue3, null, this.inheritance2.upObject, this.inheritance2.downObject);
		}
		else
		{
			this.inheritance2.name.text = StringMaster.GetString("SystemNone");
			this.inheritance2.description.text = "-";
			this.ApplyValue(0, 0, null, this.inheritance2.upObject, this.inheritance2.downObject);
		}
		this.stageGimmickObject.SetActive(num > 0 || characterStatus.isChipServerAddValue || characterStatus.IsPoint());
		this.SetArousal(characterStatus.arousal);
		base.SetupTolerance(characterStatus);
		this.MonsterMedalList.SetValues(characterStatus.talent);
		for (int i = 0; i < this.chipIcons.Length; i++)
		{
			if (i < characterStatus.chipIds.Length)
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(characterStatus.chipIds[i].ToString());
				this.chipIcons[i].SetData(chipMainData, -1, -1);
				this.chipIcons[i].SetActive(true);
				this.chipIcons[i].transform.localScale = new Vector3(0.6f, 0.6f, 1f);
			}
			else
			{
				this.chipIcons[i].SetActive(false);
			}
		}
		if (MonsterDataMng.Instance() != null)
		{
			string monsterStatusId = characterStatus.characterDatas.monsterStatusId;
			string specificTypeName = MonsterSpecificTypeData.GetSpecificTypeName(monsterStatusId);
			this.specificType.text = specificTypeName;
		}
		else
		{
			this.specificType.text = "-";
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
		}
		if (text != null)
		{
			text.text = text2;
			text.effectColor = effectColor;
		}
		return result;
	}

	private void SetArousal(int arousal)
	{
		this.arousalIcon.spriteName = "Common02_Arousal_" + arousal;
		this.arousalIcon.MakePixelPerfect();
	}

	[Serializable]
	private class Skill
	{
		public GameObject root;

		public UILabel title;

		public UILabel name;

		public UILabel description;

		public GameObject upObject;

		public GameObject downObject;
	}
}
