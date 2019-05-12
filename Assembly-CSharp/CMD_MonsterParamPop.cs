using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_MonsterParamPop : CMD
{
	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterBasicInfoExpGauge monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistance;

	[SerializeField]
	private MonsterLeaderSkill leaderSkill;

	[SerializeField]
	private MonsterLearnSkill learnSkill1;

	[SerializeField]
	private MonsterLearnSkill learnSkill2;

	[SerializeField]
	private MonsterLeaderSkill ver2LeaderSkill;

	[SerializeField]
	private MonsterLearnSkill ver2LearnSkill1;

	[SerializeField]
	private MonsterLearnSkill ver2LearnSkill2;

	[SerializeField]
	private MonsterLearnSkill ver2LearnSkill3;

	[SerializeField]
	private MonsterStatusChangeValueList statusChangeValue;

	[SerializeField]
	private MonsterMedalList monsterMedal;

	[SerializeField]
	private List<GameObject> chipObjList;

	[SerializeField]
	private GameObject statusPage;

	[SerializeField]
	private GameObject skillPage;

	[SerializeField]
	private GameObject extraSkillPage;

	[SerializeField]
	private GameObject chipPage;

	[SerializeField]
	private UILabel hpUpLabel;

	[SerializeField]
	private UILabel attackUpLabel;

	[SerializeField]
	private UILabel defenseUpLabel;

	[SerializeField]
	private UILabel spAttackUpLabel;

	[SerializeField]
	private UILabel spDefenseUpLabel;

	[SerializeField]
	private UILabel speedUpLabel;

	private int pageCnt;

	private bool viewExtraSkillPage;

	public void MonsterDataSet(MonsterData mData, DataMng.ExperienceInfo experienceInfo, int chipSlotNum)
	{
		this.viewExtraSkillPage = MonsterStatusData.IsVersionUp(mData.GetMonsterMaster().Simple.rare);
		this.monsterStatusList.ClearValues();
		this.monsterStatusList.SetValues(mData, true);
		this.monsterBasicInfo.ClearMonsterData();
		this.monsterBasicInfo.SetMonsterData(mData, experienceInfo);
		this.monsterResistance.ClearValues();
		this.monsterResistance.SetValues(mData);
		if (this.viewExtraSkillPage)
		{
			this.ver2LeaderSkill.ClearSkill();
			this.ver2LeaderSkill.SetSkill(mData);
			this.ver2LearnSkill1.ClearSkill();
			this.ver2LearnSkill1.SetSkill(mData);
			this.ver2LearnSkill2.ClearSkill();
			this.ver2LearnSkill2.SetSkill(mData);
			this.ver2LearnSkill3.ClearSkill();
			this.ver2LearnSkill3.SetSkill(mData);
		}
		else
		{
			this.leaderSkill.ClearSkill();
			this.leaderSkill.SetSkill(mData);
			this.learnSkill1.ClearSkill();
			this.learnSkill1.SetSkill(mData);
			this.learnSkill2.ClearSkill();
			this.learnSkill2.SetSkill(mData);
		}
		StatusValue statusValue = MonsterStatusData.GetStatusValue(mData.userMonster.monsterId, mData.userMonster.level);
		this.SetMedalParameter(this.hpUpLabel, mData.userMonster.hpAbility, (float)statusValue.hp);
		this.SetMedalParameter(this.attackUpLabel, mData.userMonster.attackAbility, (float)statusValue.attack);
		this.SetMedalParameter(this.defenseUpLabel, mData.userMonster.defenseAbility, (float)statusValue.defense);
		this.SetMedalParameter(this.spAttackUpLabel, mData.userMonster.spAttackAbility, (float)statusValue.magicAttack);
		this.SetMedalParameter(this.spDefenseUpLabel, mData.userMonster.spDefenseAbility, (float)statusValue.magicDefense);
		this.SetMedalParameter(this.speedUpLabel, mData.userMonster.speedAbility, (float)statusValue.speed);
		this.monsterMedal.SetActive(true);
		this.monsterMedal.SetValues(mData.userMonster);
		chipSlotNum += 5;
		for (int i = 0; i < this.chipObjList.Count; i++)
		{
			this.chipObjList[i].SetActive(false);
		}
		for (int j = 0; j < chipSlotNum; j++)
		{
			if (j >= this.chipObjList.Count)
			{
				break;
			}
			this.chipObjList[j].SetActive(true);
		}
	}

	private void SetMedalParameter(UILabel label, string ability, float baseStatus)
	{
		int num = 0;
		int.TryParse(ability, out num);
		float num2 = (float)num / 100f;
		int num3 = Mathf.FloorToInt(baseStatus * num2);
		if (num3 > 0)
		{
			label.text = num3.ToString("+0;-0");
		}
	}

	public void PageChange()
	{
		this.pageCnt++;
		if (this.pageCnt > 2)
		{
			this.pageCnt = 0;
		}
		this.statusPage.SetActive(false);
		this.skillPage.SetActive(false);
		this.extraSkillPage.SetActive(false);
		this.chipPage.SetActive(false);
		switch (this.pageCnt)
		{
		case 0:
			this.statusPage.SetActive(true);
			break;
		case 1:
			if (this.viewExtraSkillPage)
			{
				this.extraSkillPage.SetActive(true);
			}
			else
			{
				this.skillPage.SetActive(true);
			}
			break;
		case 2:
			this.chipPage.SetActive(true);
			break;
		}
	}

	public void ClosePopup()
	{
		this.ClosePanel(true);
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private enum PageType
	{
		STATUS,
		SKILL,
		CHIP
	}
}
