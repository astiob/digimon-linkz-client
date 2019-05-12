using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateCharacterRevivalFunction : BattleStateController
{
	private bool isLastFindRevivalCharacter;

	private string cameraKey = "skillA";

	public SubStateCharacterRevivalFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		this.isLastFindRevivalCharacter = false;
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(false);
		if (base.battleStateData.isFindRevivalCharacter)
		{
			base.stateManager.SetBattleScreen(BattleScreen.RevivalCharacter);
			this.SetupRevivalData();
			IEnumerator revivalCharacter = this.RevivalCharacter();
			while (revivalCharacter.MoveNext())
			{
				object obj = revivalCharacter.Current;
				yield return obj;
			}
		}
		yield break;
	}

	protected virtual void SetupRevivalData()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (base.battleStateData.isRevivalReservedCharacter[i])
			{
				list.Add(i);
			}
		}
		base.battleStateData.revivaledCharactersIndex = list.ToArray();
		base.battleStateData.isRunnedRevivalFunction = true;
	}

	protected IEnumerator RevivalCharacter()
	{
		this.isLastFindRevivalCharacter = true;
		List<CharacterStateControl> characters = new List<CharacterStateControl>();
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (base.battleStateData.isRevivalReservedCharacter[i])
			{
				characters.Add(base.battleStateData.playerCharacters[i]);
				base.battleStateData.isRevivalReservedCharacter[i] = false;
				base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.revivalReservedEffect[i], AlwaysEffectState.Out);
				int index = base.battleStateData.GetTotalCharacterIndex(base.battleStateData.playerCharacters[i]);
				base.battleStateData.isRoundStartApRevival[index] = false;
				base.battleStateData.isRoundStartHpRevival[i] = false;
			}
		}
		if (characters.Count > 1)
		{
			this.cameraKey = "skillA";
		}
		else
		{
			this.cameraKey = "skillF";
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, characters[0]);
		for (int j = 0; j < characters.Count; j++)
		{
			characters[j].Revival();
			characters[j].CharacterParams.Initialize(base.hierarchyData.cameraObject.camera3D);
			characters[j].CharacterParams.gameObject.SetActive(true);
			characters[j].CharacterParams.transform.localScale = Vector3.one;
			base.stateManager.threeDAction.PlayHitEffectAction(base.battleStateData.revivalEffect[j], characters[j]);
			characters[j].CharacterParams.transform.localScale = Vector3.zero;
			characters[j].CharacterParams.PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
		}
		base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.revivalReservedEffect[0], AlwaysEffectState.Out);
		base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.revivalEffect[0]);
		base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(true);
		IEnumerator wait = base.stateManager.threeDAction.SmoothIncreaseCharactersAction(base.stateManager.stateProperty.revivalActionWaitSecond, characters.ToArray());
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		if (this.isLastFindRevivalCharacter)
		{
			base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(false);
			base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
			base.stateManager.soundPlayer.TryStopSE(base.battleStateData.revivalReservedEffect[0]);
			base.stateManager.soundPlayer.TryStopSE(base.battleStateData.revivalEffect[0]);
			for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
			{
				base.stateManager.uiControl.ApplyCharacterHudReset(base.battleStateData.playerCharacters[i].myIndex);
			}
			base.stateManager.uiControl.ApplyBigBossCharacterHudReset();
			base.stateManager.threeDAction.StopHitEffectAction(base.battleStateData.revivalEffect);
			base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.revivalReservedEffect);
		}
		base.stateManager.waveControl.RoundCountingFunction(base.battleStateData.GetTotalCharacters(), base.battleStateData.apRevival);
		base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(true);
		base.battleStateData.turnUseDigiStoneCount = 0;
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}
}
