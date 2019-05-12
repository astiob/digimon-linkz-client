using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowTargetCamera : MonoBehaviour
{
	private static List<Camera> cameras = new List<Camera>();

	private void Awake()
	{
		Camera component = base.GetComponent<Camera>();
		if (null != component && !FollowTargetCamera.cameras.Contains(component))
		{
			FollowTargetCamera.cameras.Add(component);
		}
	}

	public static bool IsVisible()
	{
		for (int i = 0; i < FollowTargetCamera.cameras.Count; i++)
		{
			if (Camera.current == FollowTargetCamera.cameras[i])
			{
				return true;
			}
		}
		return false;
	}

	public void AddCamera(Camera camera3D)
	{
		if (!FollowTargetCamera.cameras.Contains(camera3D))
		{
			FollowTargetCamera.cameras.Add(camera3D);
		}
	}

	public void RemoveCamera(Camera camera3D)
	{
		if (FollowTargetCamera.cameras.Contains(camera3D))
		{
			FollowTargetCamera.cameras.Remove(camera3D);
		}
	}
}
