using System;
using System.Collections;
using UnityEngine;

public class SubStateWaitRandomSeedSync : BattleStateController
{
	private bool isFirst = true;

	private GameObject pvpBattleSyncWaitGO;

	private Standby standby;

	public SubStateWaitRandomSeedSync(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		if (base.battleMode == BattleMode.PvP || base.battleMode == BattleMode.Multi)
		{
			base.stateManager.uiControlMultiBasic.HideLoading();
			if (base.battleStateData.IsChipSkill())
			{
				this.isFirst = false;
			}
		}
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.battleMode != BattleMode.PvP && base.battleMode != BattleMode.Multi)
		{
			yield break;
		}
		if (this.isFirst)
		{
			if (base.battleMode == BattleMode.PvP)
			{
				IEnumerator runAlreadyLoseEvent = base.stateManager.pvpFunction.CheckAlreadyLoseBeforeBattle();
				while (runAlreadyLoseEvent.MoveNext())
				{
					yield return null;
				}
				this.pvpBattleSyncWaitGO = (base.stateManager.battleUiComponents as BattleUIComponentsPvP).pvpBattleSyncWaitUi.gameObject;
				NGUITools.SetActiveSelf(this.pvpBattleSyncWaitGO, false);
				NGUITools.SetActiveSelf(this.pvpBattleSyncWaitGO, true);
				this.standby = (this.standby ?? this.pvpBattleSyncWaitGO.GetComponent<Standby>());
			}
			else if (base.battleMode == BattleMode.Multi)
			{
				base.stateManager.uiControlMultiBasic.ShowPrepareMessage();
			}
			if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
			{
				base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0007_commandCharaView", base.battleStateData.stageSpawnPoint, true);
			}
			else
			{
				base.stateManager.cameraControl.PlayCameraMotionAction("0007_commandCharaView", base.battleStateData.stageSpawnPoint, true);
			}
		}
		else if (base.battleMode == BattleMode.Multi)
		{
			base.stateManager.uiControlMulti.ShowLoading(false);
		}
		if (base.stateManager.multiBasicFunction.IsOwner)
		{
			IEnumerator wait = base.stateManager.multiBasicFunction.SendRandomSeedSync();
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
		}
		else
		{
			IEnumerator wait2 = base.stateManager.multiBasicFunction.WaitAllPlayers(TCPMessageType.RandomSeedSync);
			while (wait2.MoveNext())
			{
				object obj2 = wait2.Current;
				yield return obj2;
			}
		}
		if (this.isFirst && base.battleMode == BattleMode.PvP)
		{
			this.standby.PlayHideAnimation();
			IEnumerator asdWait = base.stateManager.time.WaitForCertainPeriodTimeAction(1.5f, null, null);
			while (asdWait.MoveNext())
			{
				yield return null;
			}
		}
		this.isFirst = false;
		yield break;
	}

	protected override void DisabledThisState()
	{
		if (base.battleMode == BattleMode.Multi)
		{
			base.stateManager.uiControlMultiBasic.HidePrepareMessage();
			base.stateManager.uiControlMulti.HideLoading();
		}
	}
}
