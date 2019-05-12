using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PartyBossRegistance : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> toleranceGOs;

	[SerializeField]
	private List<GameObject> invalidToleranceGOList;

	[SerializeField]
	private GUIMonsterIcon guiMonsterIcon;

	public void SetIcon(MonsterData monsterData)
	{
		this.guiMonsterIcon.SetTouchAct_S(null);
		this.guiMonsterIcon.SetTouchAct_L(null);
		this.guiMonsterIcon.Data = monsterData;
		this.guiMonsterIcon.ShowGUI();
	}

	public void SetTolerances(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceM)
	{
		MonsterDetailUtil.SetTolerances(resistanceM, this.toleranceGOs);
		MonsterDetailUtil.SetInvalidTolerances(resistanceM, this.invalidToleranceGOList);
	}
}
