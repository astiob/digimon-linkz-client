using System;
using UnityEngine;

public class GUIMoveBanner : GUICollider
{
	public GUISelectPanelGashaEdit selectPanelGasha;

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		this.selectPanelGasha.timeCoun = this.selectPanelGasha.limitOrigin;
		base.OnTouchEnded(touch, pos, flag);
		this.selectPanelGasha.timeCoun = this.selectPanelGasha.limitOrigin;
	}
}
