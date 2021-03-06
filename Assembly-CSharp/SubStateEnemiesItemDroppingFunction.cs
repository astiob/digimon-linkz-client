﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateEnemiesItemDroppingFunction : BattleStateBase
{
	private Dictionary<int, bool> isEffectPlaying = new Dictionary<int, bool>();

	private CharacterStateControl[] currentDeathCharacters;

	public SubStateEnemiesItemDroppingFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	public void Init(CharacterStateControl[] deads)
	{
		this.currentDeathCharacters = deads;
	}

	protected override void EnabledThisState()
	{
	}

	protected override IEnumerator MainRoutine()
	{
		if (this.currentDeathCharacters == null || this.currentDeathCharacters.Length <= 0)
		{
			yield break;
		}
		base.stateManager.deadOrAlive.AfterEnemyDeadFunction(this.currentDeathCharacters);
		int dropCount = 0;
		int rareCount = 0;
		int normalCount = 0;
		this.isEffectPlaying.Clear();
		Dictionary<CharacterStateControl, Dictionary<int, SubStateEnemiesItemDroppingFunction.ItemDropResultData>> droppedCharacterData = new Dictionary<CharacterStateControl, Dictionary<int, SubStateEnemiesItemDroppingFunction.ItemDropResultData>>();
		foreach (CharacterStateControl characterStateControl in this.currentDeathCharacters)
		{
			if (!characterStateControl.isEnemy)
			{
				yield break;
			}
			Dictionary<int, SubStateEnemiesItemDroppingFunction.ItemDropResultData> dictionary = new Dictionary<int, SubStateEnemiesItemDroppingFunction.ItemDropResultData>();
			foreach (ItemDropResult itemDropResult in characterStateControl.itemDropResult)
			{
				if (itemDropResult.isDropped)
				{
					int effectIndex;
					if (itemDropResult.isRare)
					{
						effectIndex = rareCount;
						rareCount++;
					}
					else
					{
						effectIndex = normalCount;
						normalCount++;
					}
					SubStateEnemiesItemDroppingFunction.ItemDropResultData value = new SubStateEnemiesItemDroppingFunction.ItemDropResultData(itemDropResult, effectIndex);
					dictionary.Add(dropCount, value);
					this.isEffectPlaying[dropCount] = true;
					dropCount++;
					base.battleStateData.itemDropResults.Add(itemDropResult);
				}
			}
			if (dictionary.Count > 0)
			{
				droppedCharacterData.Add(characterStateControl, dictionary);
			}
		}
		if (droppedCharacterData.Count == 0)
		{
			yield break;
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.droppingItemActionStartWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		foreach (CharacterStateControl characterStateControl2 in droppedCharacterData.Keys)
		{
			base.StartCoroutine(this.PlayEffect(characterStateControl2, droppedCharacterData[characterStateControl2]));
		}
		while (this.isPlay())
		{
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.stateManager.uiControl.ApplyDroppedItemHide();
	}

	protected override void GetEventThisState(EventState eventState)
	{
		base.stateManager.uiControl.ApplyDroppedItemHide();
	}

	private IEnumerator PlayEffect(CharacterStateControl droppedCharacter, Dictionary<int, SubStateEnemiesItemDroppingFunction.ItemDropResultData> dropList)
	{
		int count = 0;
		float waitTime = 0.2f;
		foreach (int indexKey in dropList.Keys)
		{
			if (count > 0)
			{
				yield return new WaitForSeconds(waitTime);
			}
			base.StartCoroutine(this.PlayEffect(droppedCharacter, dropList[indexKey], indexKey));
			count++;
		}
		yield break;
	}

	private IEnumerator PlayEffect(CharacterStateControl droppedCharacter, SubStateEnemiesItemDroppingFunction.ItemDropResultData drop, int indexKey)
	{
		AlwaysEffectParams effectParams = null;
		if (!drop.itemDropResult.isRare)
		{
			effectParams = base.battleStateData.droppingItemNormalEffect[drop.effectIndex];
			base.battleStateData.currentDroppedNormalItem++;
		}
		else
		{
			effectParams = base.battleStateData.droppingItemRareEffect[drop.effectIndex];
			base.battleStateData.currentDroppedRareItem++;
		}
		Vector3[] offsetList = new Vector3[]
		{
			Vector3.zero,
			new Vector3(-0.5f, 0f, 0f),
			new Vector3(0.5f, 0f, 0f),
			new Vector3(0f, 0f, -0.5f),
			new Vector3(0f, 0f, 0.5f),
			new Vector3(-0.25f, 0f, -0.25f),
			new Vector3(0.25f, 0f, 0.25f),
			new Vector3(0.25f, 0f, -0.25f),
			new Vector3(-0.25f, 0f, 0.25f)
		};
		base.stateManager.threeDAction.SetPositionAlwaysEffectAction(effectParams, droppedCharacter, new Vector3?(offsetList[indexKey % offsetList.Length]));
		base.stateManager.threeDAction.PlayAlwaysEffectAction(effectParams, AlwaysEffectState.In);
		if (drop.itemDropResult.isRare)
		{
			base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.droppingItemRareEffect[0], AlwaysEffectState.In);
		}
		else
		{
			base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.droppingItemNormalEffect[0], AlwaysEffectState.In);
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.droppingItemActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.stateManager.threeDAction.PlayAlwaysEffectAction(effectParams, AlwaysEffectState.Out);
		Vector3 currentShowEffectPosition = base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(effectParams.targetPosition.position);
		if (drop.itemDropResult.isRare)
		{
			base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.droppingItemRareEffect[0], AlwaysEffectState.Out);
		}
		else
		{
			base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.droppingItemNormalEffect[0], AlwaysEffectState.Out);
		}
		base.stateManager.uiControl.ApplyDroppedItemNumber(base.battleStateData.currentDroppedNormalItem, base.battleStateData.currentDroppedRareItem);
		IEnumerator moveUIAction = base.stateManager.uiControl.ApplyDroppedItemMove(currentShowEffectPosition, drop.itemDropResult.isRare, indexKey);
		Action MoveUIAction = delegate()
		{
			moveUIAction.MoveNext();
		};
		wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.droppingItemHiddenActionWaitSecond, MoveUIAction, null);
		while (wait.MoveNext())
		{
			object obj2 = wait.Current;
			yield return obj2;
		}
		this.isEffectPlaying[indexKey] = false;
		base.stateManager.threeDAction.StopAlwaysEffectAction(new AlwaysEffectParams[]
		{
			effectParams
		});
		yield break;
	}

	private bool isPlay()
	{
		foreach (int key in this.isEffectPlaying.Keys)
		{
			if (this.isEffectPlaying[key])
			{
				return true;
			}
		}
		return false;
	}

	public class ItemDropResultData
	{
		public ItemDropResultData(ItemDropResult _itemDropResult, int _effectIndex)
		{
			this.itemDropResult = _itemDropResult;
			this.effectIndex = _effectIndex;
		}

		public ItemDropResult itemDropResult { get; private set; }

		public int effectIndex { get; private set; }
	}
}
