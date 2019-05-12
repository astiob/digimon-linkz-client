using System;
using UnityEngine;

public sealed class CMD_ResearchModalAlert : CMD_ModalMessageBtn2
{
	[SerializeField]
	private GUIMonsterIcon icon;

	protected override void Awake()
	{
		base.Awake();
	}

	public void SetDigimonIcon(MonsterData monsterData)
	{
		this.icon.Data = monsterData;
	}

	protected override void WindowClosed()
	{
		base.WindowClosed();
		UITexture[] componentsInChildren = this.icon.GetComponentsInChildren<UITexture>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].mainTexture = null;
			}
		}
	}
}
