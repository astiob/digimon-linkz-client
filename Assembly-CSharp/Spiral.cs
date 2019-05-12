using System;
using UnityEngine;

public class Spiral : MonoBehaviour
{
	private void Update()
	{
		base.transform.RotateAround(base.gameObject.transform.position, Vector3.up, 540f * Time.deltaTime);
	}
}
