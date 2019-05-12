using System;
using UnityEngine;

public class AlwaysPvP : BattleAlways
{
	[SerializeField]
	[Header("エモーションを開くボタン")]
	private UIButton openEmotionButton;

	[Header("ガードのオブジェクト/未使用")]
	[SerializeField]
	public GameObject guardObject;

	[SerializeField]
	[Header("右下のメッセージ")]
	public MultiConnetionMessage connetionMessage;

	[SerializeField]
	[Header("ダイアログ")]
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
