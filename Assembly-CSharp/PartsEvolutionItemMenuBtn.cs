using System;
using UnityEngine;

public class PartsEvolutionItemMenuBtn : GUICollider
{
	[SerializeField]
	private PanelEvolutionItemMenu parent;

	[SerializeField]
	private UISprite spBtn;

	[SerializeField]
	private UILabelEx lbBtn;

	public PartsEvolutionItemMenuBtn.TYPE type;

	private void OnTouchedMenuBtn()
	{
		switch (this.type)
		{
		case PartsEvolutionItemMenuBtn.TYPE.PLUGIN:
			CMD_EvolutionItemList.instance.OnTouchedBtnPlugin();
			break;
		case PartsEvolutionItemMenuBtn.TYPE.SOUL:
			CMD_EvolutionItemList.instance.OnTouchedMenuBtn(CMD_EvolutionItemList.SOUL_GROUP.SOUL);
			break;
		case PartsEvolutionItemMenuBtn.TYPE.VER_UP:
			CMD_EvolutionItemList.instance.OnTouchedMenuBtn(CMD_EvolutionItemList.SOUL_GROUP.VER_UP_PULGIN);
			break;
		case PartsEvolutionItemMenuBtn.TYPE.CORE_PLG:
			CMD_EvolutionItemList.instance.OnTouchedMenuBtn(CMD_EvolutionItemList.SOUL_GROUP.CORE_PLGIN);
			break;
		case PartsEvolutionItemMenuBtn.TYPE.VER_UP_AT_CHANGE:
			CMD_EvolutionItemList.instance.OnTouchedMenuBtn(CMD_EvolutionItemList.SOUL_GROUP.VER_UP_PULGIN_ATTR_CHANGE);
			break;
		}
		this.ChangeSelected(true);
		this.parent.OnTouchedMenuBtn(this.type);
	}

	public void ChangeSelected(bool isSelected)
	{
		if (isSelected)
		{
			this.spBtn.spriteName = "Common02_Btn_SupportRed";
			this.lbBtn.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
		else
		{
			this.spBtn.spriteName = "Common02_Btn_SupportWhite";
			this.lbBtn.color = ConstValue.DEACTIVE_BUTTON_LABEL;
		}
	}

	public enum TYPE
	{
		ALL,
		PLUGIN,
		SOUL,
		VER_UP,
		CORE_PLG,
		VER_UP_AT_CHANGE
	}
}
