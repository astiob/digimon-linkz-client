using System;
using UnityEngine;

public sealed class SignalColosseumEvent : MonoBehaviour
{
	private ConstructionName namePlate;

	private bool isDisplay;

	public void SetNamePlate(ConstructionName parentNamePlate)
	{
		this.namePlate = parentNamePlate;
	}

	public void SetDisplay(bool display)
	{
		this.isDisplay = display;
		this.Enable(display);
	}

	public void SetActiveIcon(bool active)
	{
		if (this.isDisplay)
		{
			this.Enable(active);
		}
	}

	private void Enable(bool enable)
	{
		base.gameObject.SetActive(enable);
		this.namePlate.SetDisplayNamePlate(!enable);
	}
}
