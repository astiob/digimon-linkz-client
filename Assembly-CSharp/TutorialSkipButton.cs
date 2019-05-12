using System;
using UnityEngine;

public class TutorialSkipButton : MonoBehaviour
{
	[SerializeField]
	private GUICollider skipButton;

	private Action onPushedAction;

	public void DisplaySkipButton()
	{
		this.skipButton.gameObject.SetActive(true);
		this.skipButton.activeCollider = true;
	}

	public void InvisibleSkipButton()
	{
		this.skipButton.activeCollider = false;
		this.skipButton.gameObject.SetActive(false);
	}

	public void SetPushedAction(Action action)
	{
		this.onPushedAction = action;
	}

	private void OnPushedSkipButton()
	{
		if (this.onPushedAction != null)
		{
			this.onPushedAction();
			this.onPushedAction = null;
		}
		this.InvisibleSkipButton();
	}
}
