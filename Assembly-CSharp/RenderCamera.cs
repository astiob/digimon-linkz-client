using System;
using UnityEngine;

public class RenderCamera : MonoBehaviour
{
	public Camera useCamera;

	public bool isVisible;

	private void OnWillRenderObject()
	{
		if (Camera.current == this.useCamera)
		{
			this.isVisible = true;
		}
		else
		{
			this.isVisible = false;
		}
	}
}
