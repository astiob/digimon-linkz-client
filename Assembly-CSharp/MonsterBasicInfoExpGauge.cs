using Master;
using System;
using UnityEngine;

public sealed class MonsterBasicInfoExpGauge : MonsterBasicInfo
{
	[SerializeField]
	private UIProgressBar expGauge;

	[SerializeField]
	private UILabel nextLvRestExp;

	private void SetLevelInfo(DataMng.ExperienceInfo expInfo)
	{
		this.nextLvRestExp.text = string.Format(StringMaster.GetString("CharaDetailsExp"), expInfo.expLevNext);
		float num = (float)expInfo.expLev;
		float num2 = (float)expInfo.expLevAll;
		this.expGauge.value = num / num2;
	}

	private void SetMaxLevelInfo()
	{
		this.nextLvRestExp.text = string.Format(StringMaster.GetString("BattleResult-10"), StringMaster.GetString("CharaStatus-12"));
		this.expGauge.value = 1f;
	}

	public void SetMonsterData(MonsterData monsterData, DataMng.ExperienceInfo expInfo)
	{
		base.SetMonsterData(monsterData);
		this.UpdateExpGauge(monsterData, expInfo);
	}

	public new void SetEggData(string eggName, string rare)
	{
		base.SetEggData(eggName, rare);
		this.nextLvRestExp.text = string.Empty;
		this.expGauge.value = 0f;
	}

	public override void ClearMonsterData()
	{
		base.ClearMonsterData();
		this.nextLvRestExp.text = string.Empty;
		this.expGauge.value = 0f;
	}

	public void UpdateExpGauge(MonsterData monsterData, DataMng.ExperienceInfo expInfo)
	{
		if (monsterData.monsterM.maxLevel.ToInt32() <= expInfo.lev)
		{
			this.SetMaxLevelInfo();
		}
		else
		{
			this.SetLevelInfo(expInfo);
		}
	}
}
