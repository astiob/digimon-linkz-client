using System;
using UnityEngine;

public sealed class MultiBattleAlways : BattleAlways
{
	[Header("ガード用コライダーGameObject")]
	[SerializeField]
	private GameObject gueadColliderGO;

	[Header("EmotionボタンFrontのGameObject")]
	[SerializeField]
	private GameObject emotionButtonFrontGO;

	[SerializeField]
	[Header("RemainingTurnRightのGameObject")]
	private GameObject remainingTurnRightDownGO;

	[SerializeField]
	[Header("RemainingTurnMiddleのGameObject")]
	private GameObject remainingTurnMiddleGO;

	[SerializeField]
	[Header("MultiBattleSpeedAlertのGameObject")]
	private GameObject multiBattleSpeedAlertGO;

	[SerializeField]
	[Header("SharedApGaugeのGameObject")]
	private GameObject sharedApGaugeGO;

	[Header("EmotionIconのButton")]
	[SerializeField]
	private UIButton emotionIconButton;

	[Header("右下のメッセージ")]
	[SerializeField]
	public MultiConnetionMessage connectionMessage;

	[SerializeField]
	[Header("マルチバトルのダイアログ")]
	public MultiBattleDialog multiBattleDialog;

	[SerializeField]
	[Header("共有AP")]
	public SharedAPMulti sharedAPMulti;

	[Header("残りターン(右下)")]
	[SerializeField]
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
		BattleMonsterButton[] monsterButton = current.battleUiComponentsMulti.monsterButton;
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
