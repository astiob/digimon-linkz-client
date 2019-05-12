using BattleStateMachineInternal;
using Master;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class BattleSimulator : MonoBehaviour
{
	[SerializeField]
	private string _areaName = string.Empty;

	[SerializeField]
	protected string[] _usePlayerCharactersId = new string[]
	{
		string.Empty,
		string.Empty,
		string.Empty
	};

	[SerializeField]
	protected BattleWave[] _battleWaves = new BattleWave[]
	{
		new BattleWave()
	};

	[SerializeField]
	private string _useStageId;

	[SerializeField]
	private int _digiStoneNumber = 5;

	[SerializeField]
	private int _limitRound = -1;

	private void Awake()
	{
		BattleStateManager battleStateManager = UnityEngine.Object.FindObjectOfType<BattleStateManager>();
		if (BattleStateManager.onAutoServerConnect)
		{
			battleStateManager.BattleTrigger();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		StringMaster.Initialize();
		GameObject gameObject = new GameObject("MasterDataMng");
		gameObject.AddComponent<MasterDataMng>();
		this.ApplyBattleHierarchy(battleStateManager.hierarchyData, false, 10);
		battleStateManager.BattleTrigger();
	}

	protected virtual void ApplyBattleHierarchy(BattleStateHierarchyData hierarchyData, bool onAutoPlay = false, int continueWaitSecond = 10)
	{
		hierarchyData.usePlayerCharactersId = this._usePlayerCharactersId;
		hierarchyData.batteWaves = this._battleWaves;
		hierarchyData.digiStoneNumber = this._digiStoneNumber;
		hierarchyData.useStageId = this._useStageId;
		hierarchyData.areaName = this._areaName;
		hierarchyData.leaderCharacter = 0;
		hierarchyData.limitRound = this._limitRound;
		int onAutoPlay2 = 0;
		if (onAutoPlay)
		{
			onAutoPlay2 = 2;
		}
		hierarchyData.onAutoPlay = onAutoPlay2;
	}
}
