using Evolution;
using System;
using UnityEngine;

public class GUIListEvolutionItemParts : GUIListPartBS
{
	[Header("素材用アイコンのGUICollider")]
	[SerializeField]
	private GUICollider colSoul;

	[Header("素材用アイコンのUITexture")]
	[SerializeField]
	private UITexture texSoul;

	[Header("所持数のGameObject")]
	[SerializeField]
	private GameObject goNum;

	[Header("所持数のUIlabel")]
	[SerializeField]
	private UILabel lbNum;

	[Header("所持数背景のGameObject")]
	[SerializeField]
	private GameObject goNumBG;

	[Header("親のパネル")]
	[SerializeField]
	private GUISelectPanelEvolutionItemList parentPanel;

	private string soulId;

	private GameWebAPI.UserSoulData data;

	public override void SetData()
	{
		this.data = this.parentPanel.partsDataList[base.IDX];
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.ShowData();
		base.ShowGUI();
	}

	private void ShowData()
	{
		this.ShowItemIcon();
	}

	private void ShowItemIcon()
	{
		if (this.data != null)
		{
			this.soulId = this.data.soulId;
			string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(this.soulId);
			this.lbNum.text = this.data.num;
			NGUIUtil.LoadTextureAsync(this.texSoul, evolveItemIconPathByID, new Action(this.DispItemNum));
		}
	}

	private void DispItemNum()
	{
		if (this.goNum != null && this.goNumBG != null)
		{
			this.goNum.SetActive(true);
			this.goNumBG.SetActive(true);
		}
	}

	private void OnTouchedSoulIcon()
	{
		GameWebAPI.RespDataMA_GetSoulM.SoulM soulMaster = ClassSingleton<EvolutionData>.Instance.GetSoulMaster(this.soulId);
		CMD_QuestItemPOP.Create(soulMaster);
	}
}
