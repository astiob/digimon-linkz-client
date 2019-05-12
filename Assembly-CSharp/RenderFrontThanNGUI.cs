using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderFrontThanNGUI : MonoBehaviour
{
	[SerializeField]
	private int rQueue = 4000;

	[SerializeField]
	private int sortOrder = 10;

	[SerializeField]
	private bool applyChildrens = true;

	private void Start()
	{
		if (!this.applyChildrens)
		{
			this.GetRenderers(base.transform);
		}
		else
		{
			this.GetRenderersChild(base.transform);
		}
	}

	private void GetRenderersChild(Transform t)
	{
		this.GetRenderers(t);
		for (int i = 0; i < t.childCount; i++)
		{
			this.GetRenderersChild(t.GetChild(i));
		}
	}

	private void GetRenderers(Transform t)
	{
		Renderer component = t.GetComponent<Renderer>();
		if (component == null)
		{
			return;
		}
		component.sortingOrder = this.sortOrder;
		component.material.renderQueue = this.rQueue;
	}

	public int GetSortOrder()
	{
		return this.sortOrder;
	}
}
