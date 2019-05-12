using System;
using UnityEngine;

public class FacilityShopButton : GUICollider
{
	[SerializeField]
	private GameObject alertIcon;

	public void SetBadge(bool isDisplay)
	{
		this.alertIcon.SetActive(isDisplay);
	}
}
