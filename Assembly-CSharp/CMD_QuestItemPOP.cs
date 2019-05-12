using Evolution;
using FarmData;
using Master;
using System;
using UnityEngine;

public class CMD_QuestItemPOP : CMD
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel textLabel;

	[SerializeField]
	private UITexture iconTexture;

	[SerializeField]
	private UISprite iconSprite;

	public static CMD_QuestItemPOP Create(GameWebAPI.RespDataMA_GetSoulM.SoulM data)
	{
		CMD_QuestItemPOP cmd_QuestItemPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestItemPOP", null) as CMD_QuestItemPOP;
		cmd_QuestItemPOP.SetParam(data);
		return cmd_QuestItemPOP;
	}

	public static CMD_QuestItemPOP Create(GameWebAPI.RespDataMA_ChipM.Chip data)
	{
		CMD_QuestItemPOP cmd_QuestItemPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestItemPOP", null) as CMD_QuestItemPOP;
		cmd_QuestItemPOP.SetParam(data);
		return cmd_QuestItemPOP;
	}

	public static CMD_QuestItemPOP Create(GameWebAPI.RespDataMA_GetItemM.ItemM data)
	{
		CMD_QuestItemPOP cmd_QuestItemPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestItemPOP", null) as CMD_QuestItemPOP;
		cmd_QuestItemPOP.SetParam(data);
		return cmd_QuestItemPOP;
	}

	public static CMD_QuestItemPOP Create(FacilityConditionM data, string assetValue, FacilityM faciltyData)
	{
		CMD_QuestItemPOP cmd_QuestItemPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestItemPOP", null) as CMD_QuestItemPOP;
		cmd_QuestItemPOP.SetParam(data, assetValue, faciltyData);
		return cmd_QuestItemPOP;
	}

	public static CMD_QuestItemPOP Create(GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM data)
	{
		CMD_QuestItemPOP cmd_QuestItemPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestItemPOP", null) as CMD_QuestItemPOP;
		cmd_QuestItemPOP.SetParam(data);
		return cmd_QuestItemPOP;
	}

	public static CMD_QuestItemPOP Create(GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM data)
	{
		CMD_QuestItemPOP cmd_QuestItemPOP = GUIMain.ShowCommonDialog(null, "CMD_QuestItemPOP", null) as CMD_QuestItemPOP;
		cmd_QuestItemPOP.SetParam(data);
		return cmd_QuestItemPOP;
	}

	public void SetParam(GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM data)
	{
		switch (int.Parse(data.assetCategoryId))
		{
		case 2:
			this.titleLabel.text = data.assetTitle;
			this.textLabel.text = StringMaster.GetString("DigistoneDetileText");
			this.iconSprite.spriteName = "Common02_LB_Stone";
			break;
		case 3:
			this.titleLabel.text = data.assetTitle;
			this.textLabel.text = StringMaster.GetString("LinkPointDetileText");
			this.iconSprite.spriteName = "Common02_LB_Link";
			break;
		case 4:
			this.titleLabel.text = data.assetTitle;
			this.textLabel.text = StringMaster.GetString("TipDetileText");
			this.iconSprite.spriteName = "Common02_LB_Chip";
			break;
		}
		this.iconSprite.depth = this.iconTexture.depth;
		this.iconSprite.gameObject.SetActive(true);
		this.iconTexture.gameObject.SetActive(false);
	}

	public void SetParam(FacilityConditionM data, string assetValue, FacilityM faciltyData)
	{
		FacilityKeyM facilityKeyMaster = FarmDataManager.GetFacilityKeyMaster(assetValue);
		if (facilityKeyMaster != null)
		{
			this.titleLabel.text = facilityKeyMaster.facilityKeyName;
		}
		this.textLabel.text = faciltyData.description;
		FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(data.releaseId);
		string iconPath = facilityMasterByReleaseId.GetIconPath();
		this.iconSprite.gameObject.SetActive(false);
		this.iconTexture.gameObject.SetActive(true);
		NGUIUtil.ChangeUITextureFromFile(this.iconTexture, iconPath, false);
	}

	public void SetParam(GameWebAPI.RespDataMA_GetItemM.ItemM data)
	{
		this.titleLabel.text = data.name;
		this.textLabel.text = data.description;
		string largeImagePath = data.GetLargeImagePath();
		this.iconSprite.gameObject.SetActive(false);
		this.iconTexture.gameObject.SetActive(true);
		NGUIUtil.ChangeUITextureFromFile(this.iconTexture, largeImagePath, false);
	}

	public void SetParam(GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM data)
	{
		NGUIUtil.ChangeUITextureFromFile(this.iconTexture, data.img, false);
		this.titleLabel.text = data.name;
		this.titleLabel.text = data.name;
		this.textLabel.text = data.description;
		this.iconSprite.gameObject.SetActive(false);
		this.iconTexture.gameObject.SetActive(true);
	}

	public void SetParam(GameWebAPI.RespDataMA_GetSoulM.SoulM data)
	{
		this.titleLabel.text = data.soulName;
		this.textLabel.text = data.description;
		string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(data.soulId.ToString());
		Action<UnityEngine.Object> actEnd = delegate(UnityEngine.Object obj)
		{
			this.iconTexture.mainTexture = (obj as Texture2D);
		};
		AssetDataMng.Instance().LoadObjectASync(evolveItemIconPathByID, actEnd);
		this.iconSprite.gameObject.SetActive(false);
		this.iconTexture.gameObject.SetActive(true);
	}

	public void SetParam(GameWebAPI.RespDataMA_ChipM.Chip data)
	{
		this.titleLabel.text = data.name;
		this.textLabel.text = data.detail;
		this.iconSprite.gameObject.SetActive(false);
		this.iconTexture.gameObject.SetActive(true);
		Action<UnityEngine.Object> actEnd = delegate(UnityEngine.Object obj)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
			gameObject.transform.SetParent(this.iconTexture.transform.parent);
			gameObject.transform.localPosition = this.iconTexture.transform.localPosition;
			gameObject.transform.localScale = Vector3.one;
			DepthController component = gameObject.GetComponent<DepthController>();
			component.AddWidgetDepth(this.iconTexture.depth);
			ChipIcon component2 = gameObject.GetComponent<ChipIcon>();
			component2.SetData(data, -1, -1);
			gameObject.GetComponent<BoxCollider>().enabled = false;
		};
		AssetDataMng.Instance().LoadObjectASync("UICommon/ListParts/ListPartsChip", actEnd);
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
