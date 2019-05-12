using BattleStateMachineInternal;
using Master;
using System;
using UnityEngine;

public class BattleUIInitialInduction : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[SerializeField]
	[Header("Rootのオブジェクト(Dialog)")]
	private GameObject rootObject;

	[Header("ボタン")]
	[SerializeField]
	private UIButton button;

	[Header("テキスト")]
	[SerializeField]
	private UILabel text;

	[Header("モニター")]
	[SerializeField]
	private UIComponentSkinner monitorSkinner;

	[SerializeField]
	[Header("ウィンドウTween")]
	private UITweener windowTween;

	[Header("モニターTween")]
	[SerializeField]
	private UITweener monitorTween;

	[Header("テキストTween")]
	[SerializeField]
	private UITweener textTween;

	[SerializeField]
	[Header("ヒナの名前")]
	private UILabel hinaLocalize;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.hinaLocalize.text = StringMaster.GetString("SystemNavigator");
	}

	public void AddEvent(Action OnClickCloseInitialInductionButton)
	{
		BattleInputUtility.AddEvent(this.button.onClick, OnClickCloseInitialInductionButton);
	}

	public void HideRoot()
	{
		NGUITools.SetActiveSelf(this.rootObject, false);
	}

	public void ShowFace(BattleStateHierarchyData hierarchyData, int type)
	{
		this.monitorSkinner.SetSkins((int)hierarchyData.initialIntroductionMessage[type].faceType);
		this.text.text = hierarchyData.initialIntroductionMessage[type].message;
		NGUITools.SetActiveSelf(this.rootObject, true);
	}

	public void PlayTween()
	{
		this.windowTween.PlayReverse();
		this.monitorTween.PlayReverse();
		this.textTween.PlayReverse();
	}
}
