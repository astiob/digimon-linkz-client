using System;
using UnityEngine;

public class PinchInOut
{
	private float pre_pinchLength;

	private float pinchLength;

	public float Value
	{
		get
		{
			this.GetPinchLength();
			return this.pinchLength - this.pre_pinchLength;
		}
	}

	private void GetPinchLength()
	{
		if (Input.touchCount >= 2)
		{
			Touch touch = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			if (touch2.phase == TouchPhase.Began)
			{
				this.pre_pinchLength = (this.pinchLength = Vector2.Distance(touch.position, touch2.position));
			}
			else if (touch.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
			{
				this.pre_pinchLength = this.pinchLength;
				this.pinchLength = Vector2.Distance(touch.position, touch2.position);
			}
		}
		else
		{
			this.pre_pinchLength = 0f;
			this.pinchLength = 0f;
		}
	}
}
