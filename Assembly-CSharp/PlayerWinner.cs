using System;
using UnityEngine;

public class PlayerWinner : MonoBehaviour
{
	[SerializeField]
	[Header("Nextボタン")]
	private UIButton nextButton;

	[SerializeField]
	[Header("Nextボタンのコライダー")]
	private Collider nextButtonCollider;

	[Header("NextボタンのGameObject")]
	[SerializeField]
	public GameObject nextButtonGO;

	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[Header("スピードクリア")]
	[SerializeField]
	public GameObject speedClearObject;

	public void AddEvent(Action skipWinnerAction)
	{
		BattleInputUtility.AddEvent(this.nextButton.onClick, skipWinnerAction);
	}

	public void SpeedClearObjActive(bool active)
	{
		this.speedClearObject.SetActive(active);
	}

	public void SetColliderEnabled(bool isEnabled)
	{
		this.nextButtonCollider.enabled = isEnabled;
	}
}
