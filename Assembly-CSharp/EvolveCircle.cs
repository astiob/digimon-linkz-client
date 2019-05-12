using System;
using UnityEngine;

public class EvolveCircle : MonoBehaviour
{
	[SerializeField]
	private Transform bg1Trans;

	[SerializeField]
	private Transform bg2Trans;

	[SerializeField]
	private float rotateSpeed;

	private void Update()
	{
		this.bg1Trans.Rotate(this.rotateSpeed * Vector3.up, Space.World);
		this.bg2Trans.Rotate(-this.rotateSpeed * Vector3.up, Space.World);
	}
}
