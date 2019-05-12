using System;
using System.Collections;
using UnityEngine;

public class GUIListEvolutionItemParts : GUIListPartBS
{
	[Header("素材用アイコンのGUICollider")]
	[SerializeField]
	private GUICollider colSoul;

	[SerializeField]
	[Header("素材用アイコンのUITexture")]
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

	private string soulId;

	private GameWebAPI.UserSoulData soulData;

	public GameWebAPI.UserSoulData SoulData
	{
		get
		{
			return this.soulData;
		}
		set
		{
			this.soulData = value;
			this.ShowGUI();
		}
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.ShowItemIcon();
	}

	private void ShowItemIcon()
	{
		if (this.soulData != null)
		{
			this.soulId = this.soulData.soulId;
			string evolveItemIconPathByID = MonsterDataMng.Instance().GetEvolveItemIconPathByID(this.soulId);
			this.lbNum.text = this.soulData.num;
			this.LoadObjectASync(this.texSoul, evolveItemIconPathByID);
		}
	}

	private void LoadObjectASync(UITexture uiTex, string path)
	{
		AssetDataMng.Instance().LoadObjectASync(path, delegate(UnityEngine.Object obj)
		{
			Texture2D mainTexture = obj as Texture2D;
			uiTex.mainTexture = mainTexture;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", 1f);
			hashtable.Add("y", 1f);
			hashtable.Add("time", 0.4f);
			hashtable.Add("delay", 0.01f);
			hashtable.Add("easetype", "spring");
			hashtable.Add("oncomplete", "ScaleEnd");
			hashtable.Add("oncompleteparams", 0);
			this.goNum.SetActive(true);
			this.goNumBG.SetActive(true);
			iTween.ScaleTo(uiTex.gameObject, hashtable);
		});
	}

	private void OnTouchedSoulIcon()
	{
		GameWebAPI.RespDataMA_GetSoulM.SoulM soulMasterBySoulId = MonsterDataMng.Instance().GetSoulMasterBySoulId(this.soulId);
		CMD_QuestItemPOP.Create(soulMasterBySoulId);
	}
}
