using System;
using UnityEngine;

public class CameraAnimatorManager : MonoBehaviour
{
	private void LateUpdate()
	{
		CameraAnimator.UpdateCameraPosition();
	}
}
