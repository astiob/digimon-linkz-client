using System;
using UnityEngine;

public class TutorialImageWindowArrow : MonoBehaviour
{
	[SerializeField]
	private GameObject arrowLeft;

	[SerializeField]
	private GameObject arrowRight;

	[SerializeField]
	private int displayTweenGroup;

	[SerializeField]
	private int invisibleTweenGroup;

	public void Initialize(int pageNum)
	{
		if (1 < pageNum)
		{
			this.arrowLeft.SetActive(true);
			this.arrowRight.SetActive(true);
			this.StartArrowAnimation(this.arrowRight, true);
		}
	}

	public void StartDisplay(int newPage, int pageNum)
	{
		if (0 < newPage)
		{
			this.StartArrowAnimation(this.arrowLeft, true);
		}
		if (newPage < pageNum - 1)
		{
			this.StartArrowAnimation(this.arrowRight, true);
		}
	}

	public void StartInvisible()
	{
		BoxCollider component = this.arrowLeft.GetComponent<BoxCollider>();
		if (component.enabled)
		{
			this.StartArrowAnimation(this.arrowLeft, false);
		}
		component = this.arrowRight.GetComponent<BoxCollider>();
		if (component.enabled)
		{
			this.StartArrowAnimation(this.arrowRight, false);
		}
	}

	public void EnableButton(BoxCollider collider)
	{
		collider.enabled = true;
	}

	private void StartArrowAnimation(GameObject arrow, bool displayAnimation)
	{
		UIPlayTween component = arrow.GetComponent<UIPlayTween>();
		if (displayAnimation)
		{
			component.tweenGroup = this.displayTweenGroup;
			TweenPosition component2 = arrow.GetComponent<TweenPosition>();
			component2.tweenFactor = 0.5f;
			component2.PlayForward();
		}
		else
		{
			component.tweenGroup = this.invisibleTweenGroup;
			TweenPosition component3 = arrow.GetComponent<TweenPosition>();
			component3.enabled = false;
			BoxCollider component4 = arrow.GetComponent<BoxCollider>();
			component4.enabled = false;
		}
		component.Play(true);
	}

	public void DeactiveArrows()
	{
		this.arrowLeft.SetActive(false);
		this.arrowRight.SetActive(false);
	}
}
