using System;
using UnityEngine;

[RequireComponent(typeof(UIRoot))]
public class UIRootReposition : MonoBehaviour
{
	private const float worldYPosition = 10f;

	private void Start()
	{
		base.transform.position = new Vector3(base.transform.position.x, 10f, base.transform.position.z);
	}
}
