using Master;
using System;
using System.Linq;
using UnityEngine;

public sealed class CMD_ChipInstalling : CMD
{
	[SerializeField]
	[Header("左下のチップアイコン")]
	private ChipIcon chipIcon;

	[SerializeField]
	[Header("左下のチップ名ラベル")]
	private UILabel chipNameLabel;

	[Header("左下のチップの説明ラベル")]
	[SerializeField]
	private UILabel chipDetailLabel;

	[SerializeField]
	[Header("右下の装着するボタンラベル")]
	private UILabel executeButtonLabel;

	[Header("右下の表示切替ボタンラベル")]
	[SerializeField]
	private UILabel changeViewLabel;

	[Header("右下の内容ラベル")]
	[SerializeField]
	private UILabel changeViewContentLabel;

	[SerializeField]
	[Header("右下の装着するボタンスプライト")]
	private UISprite executeButtonSprite;

	[SerializeField]
	[Header("右下の装着するボタンコライダー")]
	private BoxCollider executeButtonCollider;

	[SerializeField]
	[Header("右下の表示切替ボタンスプライト")]
	private UISprite changeViewButtonSprite;

	[SerializeField]
	[Header("チップ一覧用親")]
	private GameObject partsSortListBase;

	[SerializeField]
	[Header("チップ空メッセージ")]
	private UILabel emptyChipLabel;

	private int selectedUserChipId;

	private ChipList chipList;

	private GameWebAPI.RespDataMA_ChipM.Chip selectedChip;

	private GameWebAPI.ReqDataCS_ChipEquipLogic equip { get; set; }

	private Action<int, GameWebAPI.RespDataMA_ChipM.Chip> successChipCallback { get; set; }

	public static CMD_ChipInstalling Create(GameWebAPI.ReqDataCS_ChipEquipLogic equip, Action<int, GameWebAPI.RespDataMA_ChipM.Chip> successChipCallback)
	{
		CMD_ChipInstalling cmd_ChipInstalling = GUIMain.ShowCommonDialog(null, "CMD_ChipInstalling") as CMD_ChipInstalling;
		cmd_ChipInstalling.equip = equip;
		cmd_ChipInstalling.successChipCallback = successChipCallback;
		return cmd_ChipInstalling;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.SetupLocalize();
		this.SetupChips(ChipDataMng.userChipData);
		this.SetupChipDetail();
		this.SetupAttackButton();
		this.changeViewContentLabel.text = CMD_ChipSortModal.GetSortName();
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
	}

	private void SetupLocalize()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("ChipSphereTitle"));
		this.emptyChipLabel.text = StringMaster.GetString("ChipAdministration-01");
		this.changeViewLabel.text = StringMaster.GetString("SystemSortButton");
	}

	private void SetupChipDetail()
	{
		if (this.selectedChip == null)
		{
			this.chipNameLabel.text = StringMaster.GetString("SystemNoneHyphen");
			this.chipDetailLabel.text = StringMaster.GetString("SystemNoneHyphen");
		}
		else
		{
			this.chipNameLabel.text = this.selectedChip.name;
			this.chipDetailLabel.text = this.selectedChip.detail;
		}
		this.chipIcon.SetData(this.selectedChip, -1, -1);
	}

	private GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] GetDataList(GameWebAPI.RespDataCS_ChipListLogic data)
	{
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] list = data.userChipList.ToArray<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		CMD_ChipSortModal.UpdateSortedUserChipList(list);
		return CMD_ChipSortModal.sortedUserChipList;
	}

	private void SetupChips(GameWebAPI.RespDataCS_ChipListLogic data)
	{
		int widthLength = 8;
		if (data.userChipList != null)
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] dataList = this.GetDataList(data);
			this.chipList = new ChipList(this.partsSortListBase, widthLength, new Vector2(960f, 350f), dataList, true);
			this.chipList.SetPosition(new Vector3(-110f, 0f, 0f));
			this.chipList.SetScrollBarPosX(510f);
			this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
			this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
			this.chipList.AddWidgetDepth(this.partsSortListBase.GetComponent<UIWidget>().depth);
			if (dataList.Length <= 0)
			{
				this.emptyChipLabel.gameObject.SetActive(true);
			}
			else
			{
				this.emptyChipLabel.gameObject.SetActive(false);
			}
		}
		else
		{
			this.emptyChipLabel.gameObject.SetActive(true);
		}
	}

	private void OnShortTouchChip(GUIListChipParts.Data data)
	{
		this.chipList.SetSelectColor(this.selectedUserChipId, false);
		this.chipList.SetNowSelectMessage(this.selectedUserChipId, false);
		int userChipId = data.userChip.userChipId;
		this.chipList.SetSelectColor(userChipId, true);
		this.chipList.SetNowSelectMessage(userChipId, true);
		this.selectedUserChipId = data.userChip.userChipId;
		this.selectedChip = data.masterChip;
		global::Debug.LogWarning("ShortTouch " + data.userChip.userChipId);
		this.SetupAttackButton();
		this.SetupChipDetail();
	}

	private void OnLongTouchChip(GUIListChipParts.Data data)
	{
		CMD_QuestItemPOP.Create(data.masterChip);
	}

	private void SetupAttackButton()
	{
		bool flag = this.selectedChip != null;
		this.executeButtonLabel.color = ((!flag) ? ConstValue.DEACTIVE_BUTTON_LABEL : Color.white);
		this.executeButtonCollider.enabled = flag;
		this.executeButtonSprite.spriteName = ((!flag) ? "Common02_Btn_Gray" : "Common02_Btn_Red");
	}

	private void OnTouchDecide()
	{
		this.equip.userChipId = this.selectedUserChipId;
		CMD_InstallingPOP.Create(this.equip, this.selectedChip, delegate
		{
			this.successChipCallback(this.equip.userChipId, this.selectedChip);
			base.ClosePanel(true);
		});
	}

	private void OnTouchChangeView()
	{
		Action<int> callback = delegate(int result)
		{
			global::Debug.Log("result " + result);
			if (result > 0)
			{
				this.changeViewContentLabel.text = CMD_ChipSortModal.GetSortName();
				GameWebAPI.RespDataCS_ChipListLogic userChipData = ChipDataMng.userChipData;
				if (userChipData.userChipList != null)
				{
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] dataList = this.GetDataList(userChipData);
					this.chipList.ReAllBuild(dataList, true);
					this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
					this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
					this.chipList.SetSelectColor(this.selectedUserChipId, true);
				}
				if (CMD_ChipSortModal.sortedUserChipList.Length > 0)
				{
					NGUITools.SetActiveSelf(this.emptyChipLabel.gameObject, false);
				}
				else
				{
					NGUITools.SetActiveSelf(this.emptyChipLabel.gameObject, true);
				}
			}
		};
		CMD_ChipSortModal.Create(callback);
	}
}
