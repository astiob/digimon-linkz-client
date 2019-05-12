using System;
using UnityEngine;

public class BattleWarningController : MonoBehaviour
{
	[SerializeField]
	[Header("サブサークルオブジェクト")]
	private GameObject[] Subcircles = new GameObject[0];

	[Header("サブサークルの回転制御")]
	[SerializeField]
	private float[] SubcircleRotate = new float[0];

	private int circleIndex;

	private void Start()
	{
		this.circleIndex = this.Subcircles.Length;
	}

	private void Update()
	{
		for (int i = 0; i <= this.circleIndex - 1; i++)
		{
			this.Subcircles[i].transform.Rotate(new Vector3(0f, 0f, this.SubcircleRotate[i]));
		}
	}
}
