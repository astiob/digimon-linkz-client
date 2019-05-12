using System;
using System.Collections;
using UnityEngine;

public class StrokeEgg : MonoBehaviour
{
	private readonly float endTime = 3f;

	private readonly float downPercent = -0.01f;

	private readonly float upPercentCoefficient = 4000f;

	[SerializeField]
	private CMD eventListener;

	[SerializeField]
	private string callMethodOnSliderMax;

	[SerializeField]
	private Animation handAnime;

	[SerializeField]
	private UISlider endPercentSlider;

	[SerializeField]
	private BoxCollider boxCollider;

	private Coroutine backToInitCoroutine;

	private void OnEnable()
	{
		this.OnStrokeEventItem();
		this.handAnime.Play();
		this.endPercentSlider.value = 0f;
	}

	private void OnPress(bool IsDown)
	{
		if (IsDown)
		{
			this.handAnime.Stop();
			if (this.backToInitCoroutine != null)
			{
				base.StopCoroutine(this.backToInitCoroutine);
				this.backToInitCoroutine = null;
			}
		}
		else if (Input.touchCount < 2)
		{
			this.backToInitCoroutine = base.StartCoroutine(this.BackToInit());
		}
	}

	private void OnDrag(Vector2 Delta)
	{
		this.endPercentSlider.value += Delta.magnitude / this.upPercentCoefficient;
		Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		this.handAnime.transform.localPosition = base.transform.InverseTransformPoint(position);
		if (this.endPercentSlider.value == 1f)
		{
			this.eventListener.SendMessage(this.callMethodOnSliderMax);
			this.OffStrokeEventItem();
		}
	}

	private IEnumerator BackToInit()
	{
		float currentTime = 0f;
		while (currentTime < this.endTime)
		{
			currentTime += Time.deltaTime;
			yield return null;
		}
		while (this.endPercentSlider.value > 0f)
		{
			this.endPercentSlider.value += this.downPercent;
			yield return null;
		}
		this.handAnime.Play();
		yield break;
	}

	private void OnStrokeEventItem()
	{
		this.handAnime.gameObject.SetActive(true);
		this.endPercentSlider.gameObject.SetActive(true);
		this.boxCollider.enabled = true;
	}

	public void OffStrokeEventItem()
	{
		this.handAnime.gameObject.SetActive(false);
		this.endPercentSlider.gameObject.SetActive(false);
		this.boxCollider.enabled = false;
	}
}
