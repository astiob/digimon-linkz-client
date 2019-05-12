using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_EvolutionItemList : CMD
{
	public static CMD_EvolutionItemList instance;

	[Header("プラグインリストの親")]
	[SerializeField]
	private GameObject pluginPartsParent;

	[SerializeField]
	[Header("ソウルリストの親")]
	private GameObject soulPartsParent;

	[SerializeField]
	[Header("バージョンアップの親")]
	private GameObject verupPartsParent;

	[SerializeField]
	[Header("コアプラグインの親")]
	private GameObject corePluginPartsParent;

	[SerializeField]
	[Header("バージョンアップ耐性変化の親")]
	private GameObject verupAttrChangePartsParent;

	[Header("プラグインリストのwraper")]
	[SerializeField]
	private GameObject goWrapPlugin;

	[Header("ソウルリストのwraper")]
	[SerializeField]
	private GameObject goWrapSoul;

	[SerializeField]
	[Header("バージョンアップリストのwraper")]
	private GameObject goWrapVerup;

	[Header("コアプラグインリストのwraper")]
	[SerializeField]
	private GameObject goWrapCorePlugin;

	[Header("バージョンアップ耐性変化リストのwraper")]
	[SerializeField]
	private GameObject goWrapVerupAttrChange;

	[Header("ソウル用パーツ")]
	[SerializeField]
	private GameObject soulListParts;

	[SerializeField]
	[Header("進化素材未所持メッセージ")]
	private GameObject goNoEvolutionItemMsg;

	private GUISelectPanelEvolutionItemList csPluginPartsParent;

	private GUISelectPanelEvolutionItemList csSoulPartsParent;

	private GUISelectPanelEvolutionItemList csVerupPartsParent;

	private GUISelectPanelEvolutionItemList csCorePluginPartsParent;

	private GUISelectPanelEvolutionItemList csVerupAttrChangePartsParent;

	private GameWebAPI.UserSoulData[] userSoulData;

	private List<GameWebAPI.UserSoulData> normalPluginDataList;

	private Dictionary<CMD_EvolutionItemList.SOUL_GROUP, int> soulNumList;

	protected override void Awake()
	{
		base.Awake();
		CMD_EvolutionItemList.instance = this;
		this.userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData;
		this.soulNumList = new Dictionary<CMD_EvolutionItemList.SOUL_GROUP, int>();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.csPluginPartsParent != null && this.goWrapPlugin.activeSelf)
		{
			this.csPluginPartsParent.FadeOutAllListParts(null, false);
			this.csPluginPartsParent.SetHideScrollBarAllWays(true);
		}
		if (this.csSoulPartsParent != null && this.goWrapSoul.activeSelf)
		{
			this.csSoulPartsParent.FadeOutAllListParts(null, false);
			this.csSoulPartsParent.SetHideScrollBarAllWays(true);
		}
		if (this.csVerupPartsParent != null && this.goWrapVerup.activeSelf)
		{
			this.csVerupPartsParent.FadeOutAllListParts(null, false);
			this.csVerupPartsParent.SetHideScrollBarAllWays(true);
		}
		if (this.csCorePluginPartsParent != null && this.goWrapCorePlugin.activeSelf)
		{
			this.csCorePluginPartsParent.FadeOutAllListParts(null, false);
			this.csCorePluginPartsParent.SetHideScrollBarAllWays(true);
		}
		if (this.csVerupAttrChangePartsParent != null && this.goWrapVerupAttrChange.activeSelf)
		{
			this.csVerupAttrChangePartsParent.FadeOutAllListParts(null, false);
			this.csVerupAttrChangePartsParent.SetHideScrollBarAllWays(true);
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
		this.makeNormalPluginDataList();
		yield return null;
		this.SetCommonUI();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
		yield break;
	}

	private void SetCommonUI()
	{
		this.goWrapPlugin.SetActive(false);
		this.goWrapSoul.SetActive(false);
		this.goWrapVerup.SetActive(false);
		this.goWrapCorePlugin.SetActive(false);
		this.goWrapVerupAttrChange.SetActive(false);
		this.goNoEvolutionItemMsg.SetActive(false);
		this.csPluginPartsParent = this.pluginPartsParent.GetComponent<GUISelectPanelEvolutionItemList>();
		this.csSoulPartsParent = this.soulPartsParent.GetComponent<GUISelectPanelEvolutionItemList>();
		this.csVerupPartsParent = this.verupPartsParent.GetComponent<GUISelectPanelEvolutionItemList>();
		this.csCorePluginPartsParent = this.corePluginPartsParent.GetComponent<GUISelectPanelEvolutionItemList>();
		this.csVerupAttrChangePartsParent = this.verupAttrChangePartsParent.GetComponent<GUISelectPanelEvolutionItemList>();
		this.csPluginPartsParent.SetData(this.normalPluginDataList.ToArray());
		this.csPluginPartsParent.AllBuild(this.normalPluginDataList.Count, true, 1f, 1f, null, null);
		this.soulListParts.SetActive(false);
		this.goWrapPlugin.SetActive(true);
	}

	private void makeNormalPluginDataList()
	{
		this.normalPluginDataList = new List<GameWebAPI.UserSoulData>();
		Dictionary<GameWebAPI.UserSoulData, string> dictionary = new Dictionary<GameWebAPI.UserSoulData, string>();
		List<GameWebAPI.RespDataMA_GetSoulM.SoulM> list = MasterDataMng.Instance().RespDataMA_SoulM.soulM.Where((GameWebAPI.RespDataMA_GetSoulM.SoulM x) => int.Parse(x.soulGroup) == 0).ToList<GameWebAPI.RespDataMA_GetSoulM.SoulM>();
		GameWebAPI.RespDataMA_GetSoulM.SoulM soul;
		foreach (GameWebAPI.RespDataMA_GetSoulM.SoulM soul2 in list)
		{
			soul = soul2;
			GameWebAPI.UserSoulData userSoulData = this.userSoulData.Where((GameWebAPI.UserSoulData userSoul) => userSoul.soulId == soul.soulId).Max<GameWebAPI.UserSoulData>();
			if (userSoulData != null)
			{
				dictionary.Add(userSoulData, soul.rare);
			}
			else
			{
				dictionary.Add(new GameWebAPI.UserSoulData
				{
					soulId = soul.soulId,
					num = "0"
				}, soul.rare);
			}
		}
		IOrderedEnumerable<KeyValuePair<GameWebAPI.UserSoulData, string>> orderedEnumerable = dictionary.OrderByDescending((KeyValuePair<GameWebAPI.UserSoulData, string> y) => y.Value);
		foreach (KeyValuePair<GameWebAPI.UserSoulData, string> keyValuePair in orderedEnumerable)
		{
			this.normalPluginDataList.Add(keyValuePair.Key);
		}
	}

	public void OnTouchedBtnPlugin()
	{
		this.goNoEvolutionItemMsg.SetActive(false);
		this.goWrapPlugin.SetActive(true);
		this.goWrapSoul.SetActive(false);
		this.goWrapVerup.SetActive(false);
		this.goWrapCorePlugin.SetActive(false);
		this.goWrapVerupAttrChange.SetActive(false);
	}

	public void OnTouchedMenuBtn(CMD_EvolutionItemList.SOUL_GROUP soulGroup)
	{
		bool[] array = new bool[5];
		array[(int)soulGroup] = true;
		this.goWrapPlugin.SetActive(array[0]);
		this.goWrapSoul.SetActive(array[1]);
		if (array[1])
		{
			base.StartCoroutine(this.makeList(this.csSoulPartsParent, CMD_EvolutionItemList.SOUL_GROUP.SOUL));
		}
		this.goWrapVerup.SetActive(array[3]);
		if (array[3])
		{
			base.StartCoroutine(this.makeList(this.csVerupPartsParent, CMD_EvolutionItemList.SOUL_GROUP.VER_UP_PULGIN));
		}
		this.goWrapCorePlugin.SetActive(array[2]);
		if (array[2])
		{
			base.StartCoroutine(this.makeList(this.csCorePluginPartsParent, CMD_EvolutionItemList.SOUL_GROUP.CORE_PLGIN));
		}
		this.goWrapVerupAttrChange.SetActive(array[4]);
		if (array[4])
		{
			base.StartCoroutine(this.makeList(this.csVerupAttrChangePartsParent, CMD_EvolutionItemList.SOUL_GROUP.VER_UP_PULGIN_ATTR_CHANGE));
		}
	}

	private IEnumerator makeList(GUISelectPanelEvolutionItemList panel, CMD_EvolutionItemList.SOUL_GROUP soulGroup)
	{
		this.goNoEvolutionItemMsg.SetActive(false);
		if (!panel.isLoading() && !panel.isLoaded())
		{
			panel.setStatusLoading();
			List<GameWebAPI.UserSoulData> soulList = new List<GameWebAPI.UserSoulData>();
			foreach (GameWebAPI.UserSoulData soul in this.userSoulData)
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM sm = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(soul.soulId);
				if (int.Parse(sm.soulGroup) == (int)soulGroup)
				{
					soulList.Add(soul);
				}
			}
			this.soulNumList.Add(soulGroup, soulList.Count);
			panel.SetData(soulList.ToArray());
			panel.AllBuild(soulList.Count, true, 1f, 1f, null, null);
			panel.setStatusLoaded();
		}
		yield return null;
		if (panel.isLoaded())
		{
			int soulNum = 0;
			this.soulNumList.TryGetValue(soulGroup, out soulNum);
			if (soulNum == 0)
			{
				this.goNoEvolutionItemMsg.SetActive(true);
			}
		}
		yield break;
	}

	private void FakeMethod()
	{
	}

	public enum SOUL_GROUP
	{
		PLUGIN,
		SOUL,
		CORE_PLGIN,
		VER_UP_PULGIN,
		VER_UP_PULGIN_ATTR_CHANGE
	}
}
