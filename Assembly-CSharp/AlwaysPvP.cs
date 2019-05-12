using System;
using UnityEngine;

public class AlwaysPvP : BattleAlways
{
	[SerializeField]
	[Header("エモーションを開くボタン")]
	private UIButton openEmotionButton;

	[SerializeField]
	[Header("ガードのオブジェクト/未使用")]
	public GameObject guardObject;

	[Header("右下のメッセージ")]
	[SerializeField]
	public MultiConnetionMessage connetionMessage;

	[Header("ダイアログ")]
	[SerializeField]
	public MultiBattleDialog dialog;

	public void Initialize(BattleUIComponentsPvP ui, Action<UIButton> sendEmotionAction)
	{
		ui.emotionSenderMulti.Initialize(this.openEmotionButton, sendEmotionAction);
	}

	public void ApplySetAlwaysUIObject(bool isEnable)
	{
		this.menuCollider.gameObject.SetActive(isEnable);
	}

	public void ApplySetAlwaysUIColliders(bool isEnable)
	{
		this.menuCollider.enabled = isEnable;
	}

	public void ShowEmotionButton()
	{
		NGUITools.SetActiveSelf(this.openEmotionButton.gameObject, true);
	}

	public void HideEmotionButton()
	{
		NGUITools.SetActiveSelf(this.openEmotionButton.gameObject, false);
	}
}
