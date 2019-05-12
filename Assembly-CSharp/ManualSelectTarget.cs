using System;
using UnityEngine;

public class ManualSelectTarget : MonoBehaviour
{
	[SerializeField]
	private GameObject defaultTarget;

	[SerializeField]
	private GameObject strongTarget;

	[SerializeField]
	private GameObject weakTarget;

	public void SetActive(bool value)
	{
		if (base.gameObject.activeSelf != value)
		{
			NGUITools.SetActiveSelf(base.gameObject, value);
		}
	}

	public void SetToleranceTarget(Strength iconType)
	{
		this.defaultTarget.SetActive(iconType == Strength.None);
		this.strongTarget.SetActive(iconType == Strength.Strong || iconType == Strength.Invalid);
		this.weakTarget.SetActive(iconType == Strength.Weak);
	}
}
