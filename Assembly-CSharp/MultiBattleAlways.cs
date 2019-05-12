using System;
using UnityEngine;

public sealed class MultiBattleAlways : BattleAlways
{
	[SerializeField]
	[Header("EmotionボタンFrontのGameObject")]
	private GameObject emotionButtonFrontGO;

	[SerializeField]
	[Header("EmotionIconのButton")]
	private UIButton emotionIconButton;

	[SerializeField]
	[Header("右下のメッセージ")]
	public MultiConnetionMessage connectionMessage;

	[SerializeField]
	[Header("マルチバトルのダイアログ")]
	public MultiBattleDialog multiBattleDialog;

	[SerializeField]
	[Header("共有AP")]
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
