using System;
using UnityEngine;

public class PlayerWinnerController : MonoBehaviour
{
	[SerializeField]
	[Header("サークルオブジェクト")]
	private Transform[] circles = new Transform[0];

	[SerializeField]
	[Header("サークルの回転制御")]
	private float[] circleRotate = new float[0];

	private int circleIndex;

	private Vector3 circteRotateCache = Vector3.zero;

	private void Start()
	{
		this.circleIndex = this.circles.Length;
	}

	private void Update()
	{
		for (int i = 0; i < this.circleIndex; i++)
		{
			this.circteRotateCache.Set(0f, 0f, this.circleRotate[i]);
			this.circles[i].Rotate(this.circteRotateCache);
		}
	}
}
