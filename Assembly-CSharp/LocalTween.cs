using System;
using UnityEngine;

public class LocalTween : MonoBehaviour
{
	private LocalTween.TWEEN_TYPE nowType;

	private float deltaX;

	private float deltaY;

	private float addCT;

	private Action<int> actCB;

	private int actRET;

	private bool isBusy;

	private float FRAME_PER_SEC = 30f;

	public void KickTween(LocalTween.TWEEN_TYPE type, Vector3 valueTo, float time, Action<int> act = null, int actR = 0)
	{
		if (this.isBusy)
		{
			if (this.nowType != type)
			{
				global::Debug.LogError("==================================== LocalTween called during other type busy / GO NAME = " + base.gameObject.name);
			}
			else
			{
				global::Debug.LogWarning("==================================== LocalTween called during busy / GO NAME = " + base.gameObject.name);
			}
		}
		this.isBusy = true;
		this.nowType = type;
		this.actCB = act;
		this.actRET = actR;
		Vector3 vector = Vector3.one;
		LocalTween.TWEEN_TYPE tween_TYPE = this.nowType;
		if (tween_TYPE != LocalTween.TWEEN_TYPE.LOCAL_POS)
		{
			if (tween_TYPE == LocalTween.TWEEN_TYPE.LOCAL_SCALE)
			{
				vector = base.gameObject.transform.localScale;
			}
		}
		else
		{
			vector = base.gameObject.transform.localPosition;
		}
		float num = this.FRAME_PER_SEC * time;
		this.addCT = Mathf.Round(num);
		num = valueTo.x - vector.x;
		this.deltaX = num / this.addCT;
		num = valueTo.y - vector.y;
		this.deltaY = num / this.addCT;
	}

	private void LateUpdate()
	{
		Vector3 vector = Vector3.one;
		if (this.isBusy)
		{
			LocalTween.TWEEN_TYPE tween_TYPE = this.nowType;
			if (tween_TYPE != LocalTween.TWEEN_TYPE.LOCAL_POS)
			{
				if (tween_TYPE == LocalTween.TWEEN_TYPE.LOCAL_SCALE)
				{
					vector = base.gameObject.transform.localScale;
					vector.x += this.deltaX;
					vector.y += this.deltaY;
					base.gameObject.transform.localScale = vector;
				}
			}
			else
			{
				vector = base.gameObject.transform.localPosition;
				vector.x += this.deltaX;
				vector.y += this.deltaY;
				base.gameObject.transform.localPosition = vector;
			}
			this.addCT -= 1f;
			if (this.addCT <= 0f)
			{
				this.isBusy = false;
				if (this.actCB != null)
				{
					this.actCB(this.actRET);
				}
			}
		}
	}

	public enum TWEEN_TYPE
	{
		LOCAL_POS,
		LOCAL_SCALE
	}
}
