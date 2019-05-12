using System;
using UnityEngine;

public sealed class StatusGimmickEffectArrow : MonoBehaviour
{
	[SerializeField]
	private Color gimmickUpColor = new Color(0f, 0.8274f, 0.1411f, 1f);

	[SerializeField]
	private Color gimmickDownColor = new Color(0f, 0.4156f, 0.8274f, 1f);

	[SerializeField]
	private UISprite arrowTop;

	[SerializeField]
	private UISprite arrowMiddle;

	[SerializeField]
	private UISprite arrowBottom;

	public void Reset()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void DisplayUpArrow()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		base.transform.localRotation = Quaternion.identity;
		this.arrowTop.color = this.gimmickUpColor;
		this.arrowMiddle.color = this.gimmickUpColor;
		this.arrowBottom.color = this.gimmickUpColor;
	}

	public void DisplayBottomArrow()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		base.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
		this.arrowTop.color = this.gimmickDownColor;
		this.arrowMiddle.color = this.gimmickDownColor;
		this.arrowBottom.color = this.gimmickDownColor;
	}
}
