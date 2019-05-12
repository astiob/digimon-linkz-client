using System;
using UnityEngine;

public class AutoSetLayer : MonoBehaviour
{
	[SerializeField]
	private string _layerName = "Default";

	[SerializeField]
	private bool _applyChildrens;

	private void Awake()
	{
		if (this._applyChildrens)
		{
			this.SetLayers(base.transform);
		}
		else
		{
			this.SetLayer(base.transform);
		}
	}

	private void SetLayer(Transform t)
	{
		t.gameObject.layer = LayerMask.NameToLayer(this._layerName);
	}

	private void SetLayers(Transform t)
	{
		this.SetLayer(t);
		for (int i = 0; i < t.childCount; i++)
		{
			this.SetLayers(t.GetChild(i));
		}
	}
}
