using System;
using UnityEngine;

public class AnimationLunch : MonoBehaviour
{
	private Quaternion rotationHold;

	private void OnEnable()
	{
		this.rotationHold = base.transform.localRotation;
	}

	private void LateUpdate()
	{
		base.transform.localRotation = this.rotationHold;
	}
}
