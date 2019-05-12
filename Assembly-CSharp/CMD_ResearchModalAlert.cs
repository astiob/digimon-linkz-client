using Master;
using System;
using UnityEngine;

public sealed class CMD_ResearchModalAlert : CMD_ModalMessageBtn2
{
	[SerializeField]
	private GUIMonsterIcon icon;

	protected override void Awake()
	{
		base.Awake();
		base.SetTitle(StringMaster.GetString("LaboratoryResearchAlertTitle"));
		base.SetExp(StringMaster.GetString("LaboratoryResearchAlertInfo"));
		base.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		base.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
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
