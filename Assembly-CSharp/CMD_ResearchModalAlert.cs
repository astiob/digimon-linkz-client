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

	public void AdjustSize()
	{
		UILabel component = this.goTX_EXP.GetComponent<UILabel>();
		int num = component.text.Split(new char[]
		{
			'\n'
		}).Length;
		if (num >= 5)
		{
			int num2 = (component.fontSize + component.spacingY) * (num - 4);
			base.transform.SetLocalY(base.transform.localPosition.y - (float)(num2 / 2));
			base.GetComponent<UIWidget>().height += num2;
		}
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
