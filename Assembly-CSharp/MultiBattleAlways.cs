using System;
using UnityEngine;

public sealed class MultiBattleAlways : BattleAlways
{
	[Header("EmotionボタンFrontのGameObject")]
	[SerializeField]
	private GameObject emotionButtonFrontGO;

	[Header("EmotionIconのButton")]
	[SerializeField]
	private UIButton emotionIconButton;

	[Header("右下のメッセージ")]
	[SerializeField]
	public MultiConnetionMessage connectionMessage;

	[SerializeField]
	[Header("マルチバトルのダイアログ")]
	public MultiBattleDialog multiBattleDialog;

	[Header("共有AP")]
	[SerializeField]
	public BattleUIMultiSharedAP sharedAP;

	[SerializeField]
	[Header("残りターン(右下)")]
	public RemainingTurn remainingTurn;

	public void ShowWinnerUI()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
		}
		this.multiBattleDialog.gameObject.SetActive(true);
		this.emotionButtonFrontGO.SetActive(true);
		BattleStateManager current = BattleStateManager.current;
		current.battleUiComponentsMulti.skillSelectUi.gameObject.SetActive(true);
		current.uiControlMulti.ApplySkillSelectUI(false);
		BattleMonsterButton[] monsterButton = current.battleUiComponentsMulti.skillSelectUi.monsterButton;
		foreach (BattleMonsterButton battleMonsterButton in monsterButton)
		{
			Collider[] componentsInChildren = battleMonsterButton.GetComponentsInChildren<Collider>();
			if (componentsInChildren != null)
			{
				foreach (Collider collider in componentsInChildren)
				{
					collider.enabled = false;
				}
			}
		}
	}

	public void HideMenu()
	{
		NGUITools.SetActiveSelf(this.menuCollider.gameObject, false);
	}

	public void HideAutoPlay()
	{
		NGUITools.SetActiveSelf(this.autoCollider.gameObject, false);
	}

	public void SetupMenuButton(bool isOwner)
	{
		NGUITools.SetActiveSelf(this.menuCollider.gameObject, isOwner);
	}

	public void SetupEmotion(EmotionSenderMulti emotionSenderMulti, Action<int> sendEmotionAction)
	{
		emotionSenderMulti.Initialize(this.emotionIconButton, sendEmotionAction);
	}

	public void SetEmotionButton(bool isEnabled)
	{
		NGUITools.SetActiveSelf(this.emotionIconButton.gameObject, isEnabled);
	}
}
