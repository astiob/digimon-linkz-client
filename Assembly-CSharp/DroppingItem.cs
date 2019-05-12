using System;
using UnityEngine;

public class DroppingItem : MonoBehaviour
{
	[SerializeField]
	private UITweener tween;

	[SerializeField]
	private GameObject defaultItem;

	[SerializeField]
	private GameObject rareItem;

	public void SetActive(bool value)
	{
		NGUITools.SetActiveSelf(base.gameObject, value);
	}

	public void SetRare(bool isRare)
	{
		this.defaultItem.SetActive(!isRare);
		this.rareItem.SetActive(isRare);
	}

	public void PlayForward()
	{
		this.tween.PlayForward();
	}

	public void ResetToBeginning()
	{
		this.tween.enabled = false;
		this.tween.ResetToBeginning();
	}
}
