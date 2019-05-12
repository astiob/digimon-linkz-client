using System;
using UnityEngine;

public class PartsGaugePoint : MonoBehaviour
{
	[SerializeField]
	private UISprite digimonIcon1;

	[SerializeField]
	private UISprite digimonIcon2;

	[SerializeField]
	private UISprite digimonIcon3;

	private PartsGaugePoint.Step newStep;

	private PartsGaugePoint.Step nowStep;

	private bool activeIcon1;

	private bool activeIcon2;

	private bool activeIcon3;

	public void SetGaugeValue(float value)
	{
		if (0.33f >= value)
		{
			this.newStep = PartsGaugePoint.Step.GAUGE_LEVEL_1;
			this.activeIcon1 = true;
			this.activeIcon2 = false;
			this.activeIcon3 = false;
		}
		else if (0.66f >= value)
		{
			this.newStep = PartsGaugePoint.Step.GAUGE_LEVEL_2;
			this.activeIcon1 = false;
			this.activeIcon2 = true;
			this.activeIcon3 = false;
		}
		else
		{
			this.newStep = PartsGaugePoint.Step.GAUGE_LEVEL_3;
			this.activeIcon1 = false;
			this.activeIcon2 = false;
			this.activeIcon3 = true;
		}
	}

	public void SetDepthDigimonIcons(int depth)
	{
		this.digimonIcon1.depth = depth;
		this.digimonIcon2.depth = depth;
		this.digimonIcon3.depth = depth;
	}

	private void Update()
	{
		if (this.newStep != this.nowStep)
		{
			this.nowStep = this.newStep;
			this.SetIcon(this.digimonIcon1.gameObject, this.activeIcon1);
			this.SetIcon(this.digimonIcon2.gameObject, this.activeIcon2);
			this.SetIcon(this.digimonIcon3.gameObject, this.activeIcon3);
		}
	}

	private void SetIcon(GameObject icon, bool isActive)
	{
		if (icon.activeSelf != isActive)
		{
			icon.SetActive(isActive);
		}
	}

	private enum Step
	{
		GAUGE_LEVEL_1,
		GAUGE_LEVEL_2,
		GAUGE_LEVEL_3
	}
}
