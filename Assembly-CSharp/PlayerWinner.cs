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

	[SerializeField]
	[Header("NextボタンのGameObject")]
	public GameObject nextButtonGO;

	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	public void AddEvent(Action skipWinnerAction)
	{
		BattleInputUtility.AddEvent(this.nextButton.onClick, skipWinnerAction);
	}

	public void SetColliderEnabled(bool isEnabled)
	{
		this.nextButtonCollider.enabled = isEnabled;
	}
}
