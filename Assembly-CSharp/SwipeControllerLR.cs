using System;
using UnityEngine;

public class SwipeControllerLR : MonoBehaviour
{
	[SerializeField]
	private float threshold;

	private Action actOnLeft;

	private Action actOnRight;

	private Vector2 vPosBegan;

	public void SetThreshold(float _threshold)
	{
		this.threshold = _threshold;
	}

	public void SetActionSwipe(Action onLeft, Action onRight)
	{
		this.actOnLeft = onLeft;
		this.actOnRight = onRight;
	}

	private void Update()
	{
		this.UpdateTouch();
	}

	private void UpdateTouch()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			this.CheckSwipe(touch);
		}
	}

	private void CheckSwipe(Touch touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			this.vPosBegan = touch.position;
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			float num = (touch.position - this.vPosBegan).x * (1136f / (float)Screen.width);
			if (num >= this.threshold)
			{
				if (this.actOnRight != null)
				{
					this.actOnRight();
				}
			}
			else if (num <= -this.threshold && this.actOnLeft != null)
			{
				this.actOnLeft();
			}
		}
	}
}
