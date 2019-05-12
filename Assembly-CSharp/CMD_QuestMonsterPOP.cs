using Monster;
using System;
using UnityEngine;

public class CMD_QuestMonsterPOP : CMD
{
	[SerializeField]
	private UILabel digimonNameLabel;

	[SerializeField]
	private UILabel tribeLabel;

	[SerializeField]
	private UILabel growStepLabel;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterResistanceList monsterInvalidResistanceList;

	public void SetBossDetails(MonsterData monsterData, int resistanceId)
	{
		this.digimonNameLabel.text = monsterData.monsterMG.monsterName;
		this.tribeLabel.text = MonsterTribeData.GetTribeName(monsterData.monsterMG.tribe);
		this.growStepLabel.text = MonsterGrowStepData.GetGrowStepName(monsterData.monsterMG.growStep);
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = MonsterData.SerchResistanceById(resistanceId.ToString());
		this.monsterResistanceList.SetValues(monsterResistanceM);
		this.monsterInvalidResistanceList.SetInvalid(monsterResistanceM);
	}

	public void SetBossDetails(int monsterId, int resistanceId)
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(monsterId.ToString());
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterMasterByMonsterId.monsterGroupId);
		this.digimonNameLabel.text = monsterGroupMasterByMonsterGroupId.monsterName;
		this.tribeLabel.text = MonsterTribeData.GetTribeName(monsterGroupMasterByMonsterGroupId.tribe);
		this.growStepLabel.text = MonsterGrowStepData.GetGrowStepName(monsterGroupMasterByMonsterGroupId.growStep);
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = null;
		string b = resistanceId.ToString();
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM2 = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
		for (int i = 0; i < monsterResistanceM2.Length; i++)
		{
			if (monsterResistanceM2[i].monsterResistanceId == b)
			{
				monsterResistanceM = monsterResistanceM2[i];
				break;
			}
		}
		this.monsterResistanceList.SetValues(monsterResistanceM);
		this.monsterInvalidResistanceList.SetInvalid(monsterResistanceM);
	}
}
