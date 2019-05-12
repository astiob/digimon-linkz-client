using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_EvolutionItemList : CMD
{
	public static CMD_EvolutionItemList instance;

	[Header("通常プラグイン用parent")]
	[SerializeField]
	private GameObject pluginPartsParent;

	[SerializeField]
	[Header("通常プラグイン用parts")]
	private GameObject pluginListParts;

	[Header("究極体ソウル用parent")]
	[SerializeField]
	private GameObject soulPartsParent;

	[Header("究極体ソウル用parts")]
	[SerializeField]
	private GameObject soulListParts;

	[Header("究極体ソウル用wrap")]
	[SerializeField]
	private GameObject goWrapSoul;

	[Header("通常プラグイン用wrap")]
	[SerializeField]
	private GameObject goWrapPlugin;

	[SerializeField]
	[Header("究極体進化素材未所持メッセージGameObject")]
	private GameObject goNoEvolutionItem;

	[Header("究極体進化素材未所持メッセージLabel")]
	[SerializeField]
	private UILabel lbNoEvolutionItem;

	private GUISelectPanelEvolutionItemList csPluginPartParent;

	private GUISelectPanelEvolutionItemList csSoulPartParent;

	private GameWebAPI.UserSoulData[] userSoulData;

	private GameWebAPI.RespDataMA_GetSoulM soulM;

	private int initFocusTabNum = 1;

	protected override void Awake()
	{
		base.Awake();
		CMD_EvolutionItemList.instance = this;
		this.userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData;
		this.soulM = MasterDataMng.Instance().RespDataMA_SoulM;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.csPluginPartParent != null && this.goWrapPlugin.activeSelf)
		{
			this.csPluginPartParent.FadeOutAllListParts(null, false);
			this.csPluginPartParent.SetHideScrollBarAllWays(true);
		}
		if (this.csSoulPartParent != null && this.goWrapSoul.activeSelf)
		{
			this.csSoulPartParent.FadeOutAllListParts(null, false);
			this.csSoulPartParent.SetHideScrollBarAllWays(true);
		}
		base.ClosePanel(true);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.StartCoroutine(this.ShowGUI(f, sizeX, sizeY, aT));
	}

	private IEnumerator ShowGUI(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.PartsTitle.SetTitle(StringMaster.GetString("EvolutionMaterialList"));
		this.goWrapSoul.SetActive(false);
		this.goWrapSoul.SetActive(false);
		yield return null;
		this.SetCommonUI();
		this.setTabView();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
		yield break;
	}

	private void setTabView()
	{
		base.MultiTab.InitMultiTab(new List<Action<int>>
		{
			new Action<int>(this.OnTouchedTabPlugin),
			new Action<int>(this.OnTouchedTabSoul)
		}, new List<string>
		{
			StringMaster.GetString("EvolutionMaterialName"),
			StringMaster.GetString("EvolutionUltimateMaterialName")
		});
		base.MultiTab.SetOnOffColor(ConstValue.TAB_YELLOW, Color.white);
		base.MultiTab.SetFocus(this.initFocusTabNum);
	}

	private void SetCommonUI()
	{
		this.csPluginPartParent = this.pluginPartsParent.GetComponent<GUISelectPanelEvolutionItemList>();
		this.csPluginPartParent.AllBuildPlugin(this.userSoulData, this.soulM);
		this.csSoulPartParent = this.soulPartsParent.GetComponent<GUISelectPanelEvolutionItemList>();
		this.csSoulPartParent.AllBuildSoul(this.userSoulData);
		this.pluginListParts.SetActive(false);
		this.soulListParts.SetActive(false);
	}

	private void OnTouchedTabPlugin(int i)
	{
		this.goWrapSoul.SetActive(false);
		this.goWrapPlugin.SetActive(true);
	}

	private void OnTouchedTabSoul(int i)
	{
		this.goWrapSoul.SetActive(true);
		this.goWrapPlugin.SetActive(false);
	}

	public void DispNoEvolutionItemMessage()
	{
		this.lbNoEvolutionItem.text = StringMaster.GetString("EvolutionMaterialNothing");
		this.goNoEvolutionItem.SetActive(true);
	}

	private void FakeMethod()
	{
	}
}
