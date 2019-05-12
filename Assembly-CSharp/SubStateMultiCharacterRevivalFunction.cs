using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;

public class SubStateMultiCharacterRevivalFunction : SubStateCharacterRevivalFunction
{
	public SubStateMultiCharacterRevivalFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(false);
		while (base.stateManager.multiFunction.IsDisconnected)
		{
			yield return null;
		}
		IEnumerator revivalSync = this.RevivalSync();
		while (revivalSync.MoveNext())
		{
			object obj = revivalSync.Current;
			yield return obj;
		}
		if (base.battleStateData.isFindRevivalCharacter)
		{
			base.stateManager.SetBattleScreen(BattleScreen.RevivalCharacter);
			IEnumerator revivalCancel = this.RevivalCancel();
			while (revivalCancel.MoveNext())
			{
				object obj2 = revivalCancel.Current;
				yield return obj2;
			}
			if (!base.battleStateData.isFindRevivalCharacter)
			{
				yield break;
			}
			this.SetupRevivalData();
			IEnumerator revivalCharacter = base.RevivalCharacter();
			while (revivalCharacter.MoveNext())
			{
				object obj3 = revivalCharacter.Current;
				yield return obj3;
			}
		}
		yield break;
	}

	private IEnumerator RevivalSync()
	{
		base.stateManager.uiControlMulti.ShowLoading(false);
		base.stateManager.uiControl.SetTouchEnable(false);
		while (base.stateManager.multiFunction.isSendCharacterRevival)
		{
			yield return null;
		}
		if (base.stateManager.multiFunction.IsOwner)
		{
			EnemyTurnSyncData message = new EnemyTurnSyncData
			{
				playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.EnemyTurnSync, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None)
			};
			IEnumerator wait = base.stateManager.multiBasicFunction.SendMessageInsistently<EnemyTurnSyncData>(TCPMessageType.EnemyTurnSync, message, 1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
		}
		else
		{
			IEnumerator waitAllPlayers = base.stateManager.multiFunction.WaitAllPlayers(TCPMessageType.EnemyTurnSync);
			while (waitAllPlayers.MoveNext())
			{
				object obj2 = waitAllPlayers.Current;
				yield return obj2;
			}
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControlMulti.HideLoading();
		yield break;
	}

	private IEnumerator RevivalCancel()
	{
		List<string> revivalCancelUserIdList = new List<string>();
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (base.stateManager.multiFunction.IsRevivalCancel(i))
			{
				string userId = base.stateManager.multiFunction.GetRevivalUserId(i);
				revivalCancelUserIdList.Add(userId);
			}
		}
		base.stateManager.uiControlMulti.ShowLoading(false);
		base.stateManager.uiControl.SetTouchEnable(false);
		if (base.stateManager.multiFunction.GetReservedCount() != 3)
		{
			if (base.stateManager.multiFunction.IsOwner)
			{
				IEnumerator sendCancelCharacterRevival = base.stateManager.multiFunction.SendCancelCharacterRevival(revivalCancelUserIdList.ToArray());
				while (sendCancelCharacterRevival.MoveNext())
				{
					object obj = sendCancelCharacterRevival.Current;
					yield return obj;
				}
			}
			else
			{
				IEnumerator waitAllPlayers = base.stateManager.multiFunction.WaitAllPlayers(TCPMessageType.RevivalCancel);
				while (waitAllPlayers.MoveNext())
				{
					object obj2 = waitAllPlayers.Current;
					yield return obj2;
				}
			}
		}
		else
		{
			foreach (string revivalCancelUserId in revivalCancelUserIdList)
			{
				base.stateManager.multiFunction.RemoveRevivalData(revivalCancelUserId);
			}
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControlMulti.HideLoading();
		yield break;
	}

	protected override void SetupRevivalData()
	{
		int reservedCount = base.stateManager.multiFunction.GetReservedCount();
		List<int> list = new List<int>();
		if (base.stateManager.multiFunction.IsOwner || (!base.stateManager.multiFunction.IsOwner && reservedCount < 3))
		{
			for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
			{
				if (base.battleStateData.isRevivalReservedCharacter[i] && base.stateManager.multiFunction.IsMe(i))
				{
					base.battleStateData.isRunnedRevivalFunction = true;
					list.Add(i);
				}
				else if (base.stateManager.multiFunction.IsOwner && reservedCount >= 3)
				{
					list.Add(i);
				}
			}
		}
		base.battleStateData.revivaledCharactersIndex = list.ToArray();
	}
}
