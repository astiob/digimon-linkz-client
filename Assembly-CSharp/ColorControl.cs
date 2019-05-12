using System;
using UnityEngine;

public class ColorControl : MonoBehaviour
{
	private MeshRenderer meshRender;

	[SerializeField]
	private int offFlame;

	[SerializeField]
	private int onFlame;

	private int frameCT;

	private void Start()
	{
		this.meshRender = base.gameObject.GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		if (this.offFlame == this.frameCT)
		{
			this.meshRender.enabled = false;
		}
		if (this.onFlame == this.frameCT)
		{
			this.meshRender.enabled = true;
		}
		this.frameCT++;
	}
}
