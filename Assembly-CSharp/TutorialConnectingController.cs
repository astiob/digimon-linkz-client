using System;
using UnityEngine;

public class TutorialConnectingController : MonoBehaviour
{
	[SerializeField]
	[Header("回転するオブジェクト")]
	private Transform[] RotationObjects = new Transform[0];

	[Header("回転オブジェクトの回転制御")]
	[SerializeField]
	private float[] RotationSpeed = new float[0];

	private int ObjectIndex;

	private void Start()
	{
		this.ObjectIndex = this.RotationObjects.Length;
	}

	private void Update()
	{
		for (int i = 0; i <= this.ObjectIndex - 1; i++)
		{
			this.RotationObjects[i].transform.Rotate(new Vector3(0f, 0f, this.RotationSpeed[i]));
		}
	}
}
