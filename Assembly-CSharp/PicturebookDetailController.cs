using System;
using UnityEngine;

public class PicturebookDetailController : MonoBehaviour
{
	[SerializeField]
	private CMD eventListener;

	[SerializeField]
	private string callMethodOnDrag;

	[SerializeField]
	private string callMethodOnClick;

	public CMD EventListener
	{
		set
		{
			this.eventListener = value;
		}
	}

	public string CallMethodOnDrag
	{
		set
		{
			this.callMethodOnDrag = value;
		}
	}

	public string CallMethodOnClick
	{
		set
		{
			this.callMethodOnClick = value;
		}
	}

	private void OnDrag(Vector2 Delta)
	{
		if (Input.touchCount >= 2)
		{
			return;
		}
		if (this.eventListener != null && !string.IsNullOrEmpty(this.callMethodOnDrag))
		{
			this.eventListener.SendMessage(this.callMethodOnDrag, Delta);
		}
	}

	private void OnClick()
	{
		if (this.eventListener != null && !string.IsNullOrEmpty(this.callMethodOnClick))
		{
			this.eventListener.SendMessage(this.callMethodOnClick);
		}
	}
}
