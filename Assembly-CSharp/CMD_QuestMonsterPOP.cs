using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_QuestMonsterPOP : CMD
{
	[SerializeField]
	private UILabel digimonNameLabel;

	[SerializeField]
	private UILabel digimonRaceLabel;

	[SerializeField]
	private UILabel toleranceLabel;

	[SerializeField]
	private UILabel digimonGradeLabel;

	public List<GameObject> toleranceGOList;

	[SerializeField]
	private List<GameObject> invalidToleranceGOList;

	public static int ResistanceId { private get; set; }

	public static MonsterData MonsterData { private get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.toleranceLabel.text = StringMaster.GetString("CharaStatus-22");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.ShowChgInfoUP();
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
		CMD_QuestMonsterPOP.MonsterData = null;
		base.OnDestroy();
	}

	private void ShowChgInfoUP()
	{
		this.digimonNameLabel.text = CMD_QuestMonsterPOP.MonsterData.monsterMG.monsterName;
		this.digimonGradeLabel.text = CMD_QuestMonsterPOP.MonsterData.growStepM.monsterGrowStepName;
		this.digimonRaceLabel.text = CMD_QuestMonsterPOP.MonsterData.tribeM.monsterTribeName;
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceM = CMD_QuestMonsterPOP.MonsterData.SerchResistanceById(CMD_QuestMonsterPOP.ResistanceId.ToString());
		MonsterDetailUtil.SetTolerances(resistanceM, this.toleranceGOList);
		MonsterDetailUtil.SetInvalidTolerances(resistanceM, this.invalidToleranceGOList);
	}
}
