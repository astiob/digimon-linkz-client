using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class TickerEffect : MonoBehaviour
{
	private UILabel uiLabel;

	private Transform uiLabelTrans;

	[SerializeField]
	private float beginWait;

	[SerializeField]
	private float finishWait;

	[SerializeField]
	private float lebelFrameWidth;

	[SerializeField]
	private float speed;

	private float bkLocalPosX;

	private void Awake()
	{
		this.uiLabel = base.GetComponent<UILabel>();
		this.uiLabelTrans = this.uiLabel.transform;
		this.bkLocalPosX = this.uiLabelTrans.localPosition.x;
	}

	private void Start()
	{
		int num = (int)this.uiLabel.printedSize.x;
		int num2 = num - (int)this.lebelFrameWidth;
		if (num2 > 0)
		{
			base.StartCoroutine(this.Run((float)num2));
		}
	}

	private IEnumerator Run(float delta)
	{
		this.uiLabelTrans.SetLocalX(this.bkLocalPosX);
		yield return new WaitForSeconds(this.beginWait);
		while (this.bkLocalPosX - delta < this.uiLabelTrans.localPosition.x)
		{
			this.uiLabelTrans.SetLocalX(this.uiLabelTrans.localPosition.x - this.speed);
			yield return null;
		}
		yield return new WaitForSeconds(this.finishWait);
		base.StartCoroutine(this.Run(delta));
		yield break;
	}
}
