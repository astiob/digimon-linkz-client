using System;
using UnityEngine;

public class ToleranceIcon : MonoBehaviour
{
	[SerializeField]
	private GameObject defaultIcon;

	[SerializeField]
	private GameObject strongIcon;

	[SerializeField]
	private GameObject weakIcon;

	public void SetActive(bool value)
	{
		if (base.gameObject.activeSelf != value)
		{
			NGUITools.SetActiveSelf(base.gameObject, value);
		}
	}

	public void SetToleranceIcon(Strength iconType)
	{
		this.defaultIcon.SetActive(iconType == Strength.None);
		this.strongIcon.SetActive(iconType == Strength.Strong || iconType == Strength.Invalid);
		this.weakIcon.SetActive(iconType == Strength.Weak);
	}
}
