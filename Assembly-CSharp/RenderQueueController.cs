using System;
using UnityEngine;

public class RenderQueueController : MonoBehaviour
{
	[SerializeField]
	private int queue;

	private Material mat;

	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			this.mat = component.sharedMaterial;
		}
	}

	private void OnWillRenderObject()
	{
		this.UpdateMaterial();
	}

	private void UpdateMaterial()
	{
		if (this.mat != null)
		{
			this.mat.renderQueue = this.queue;
		}
	}
}
