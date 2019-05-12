using System;
using UnityEngine;

public sealed class BattleCameraCallMonoCodes : MonoBehaviour
{
	[HideInInspector]
	public BattleStateManager stateManager;

	private void OnPreRender()
	{
		if (!this.stateManager)
		{
			return;
		}
		for (int i = 0; i < this.stateManager.onPreRenderJustOnce.Count; i++)
		{
			this.stateManager.onPreRenderJustOnce[i]();
		}
		this.stateManager.onPreRenderJustOnce.Clear();
	}
}
