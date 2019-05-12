using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_ChipReinforcement : CMD
{
	private const string CMD_NAME = "CMD_ChipReinforced";

	[SerializeField]
	private GUICollider sortButton;

	[SerializeField]
	private UILabel sortButtonLabel;

	[SerializeField]
	private UILabel sortNameLabel;

	[SerializeField]
	private UILabel messageLabel;

	[SerializeField]
	private UILabel listCountLabel;

	private ChipList chipList;

	public static CMD_ChipReinforcement Create(Action<int> callback = null)
	{
		return GUIMain.ShowCommonDialog(callback, "CMD_ChipReinforced", null) as CMD_ChipReinforcement;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		ChipTutoial.Start("second_tutorial_chip_reinforcement", delegate
		{
			GUICollider.EnableAllCollider("CMD_ChipReinforced");
		});
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		GUICollider.DisableAllCollider("CMD_ChipReinforced");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] array = this.ConvertChipList(ChipDataMng.userChipData);
		this.chipList = new ChipList(this.goEFC_FOOTER, 8, new Vector2(960f, 450f), array, false);
		this.chipList.SetPosition(new Vector3(-40f, -70f, 0f));
		this.chipList.SetScrollBarPosX(520f);
		this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
		this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
		this.chipList.AddWidgetDepth(base.gameObject.GetComponent<UIWidget>().depth);
		base.PartsTitle.SetTitle(StringMaster.GetString("ChipReinforcement-01"));
		this.sortButtonLabel.text = StringMaster.GetString("SystemSortButton");
		this.sortNameLabel.text = CMD_ChipSortModal.GetSortName();
		this.sortButton.CallBackClass = base.gameObject;
		this.sortButton.MethodToInvoke = "OnSortButton";
		this.messageLabel.gameObject.SetActive(array.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>() == 0);
		this.messageLabel.text = StringMaster.GetString("ChipAdministration-01");
		int num = 0;
		if (ChipDataMng.userChipData != null && ChipDataMng.userChipData.userChipList != null)
		{
			num = ChipDataMng.userChipData.userChipList.Length;
		}
		this.listCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), num, DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax);
		base.ShowDLG();
		base.SetTutorialAnyTime("anytime_second_tutorial_chip_reinforcement");
		base.Show(closeEvent, sizeX, sizeY, showAnimationTime);
		RestrictionInput.EndLoad();
	}

	private void OnShortTouchChip(GUIListChipParts.Data data)
	{
		Action<int> callback = delegate(int result)
		{
			if (result > 0)
			{
				this.Send(data.userChip);
			}
		};
		CMD_ChipReinforcementModal.Create(data.userChip, callback);
	}

	private void Send(GameWebAPI.RespDataCS_ChipListLogic.UserChipList baseChip)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		int userChipId = baseChip.userChipId;
		GameWebAPI.RespDataMA_ChipM.Chip baseMaterChip = ChipDataMng.GetChipMainData(baseChip);
		int num = baseMaterChip.needChip.ToInt32();
		int[] array = null;
		if (num > 0)
		{
			array = new int[num];
			int num2 = 0;
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in ChipDataMng.userChipData.userChipList)
			{
				if (userChipId != userChipList2.userChipId && userChipList2.chipId == baseChip.chipId && userChipList2.userMonsterId == 0)
				{
					array[num2] = userChipList2.userChipId;
					num2++;
					if (num2 >= array.Length)
					{
						break;
					}
				}
			}
		}
		Action callback = delegate()
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] array2 = this.ConvertChipList(ChipDataMng.userChipData);
			this.chipList.ReAllBuild(array2, false);
			this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
			this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
			this.messageLabel.gameObject.SetActive(array2.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>() == 0);
			this.listCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), ChipDataMng.userChipData.userChipList.Length, DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax);
			GameWebAPI.RespDataMA_ChipM.Chip chipEnhancedData = ChipDataMng.GetChipEnhancedData(baseMaterChip.chipId);
			CMD_ChipReinforcementAnimation.Create(this.gameObject, chipEnhancedData, null);
		};
		int resultCode = 0;
		APIRequestTask task = ChipDataMng.RequestAPIChipFusion(baseChip.userChipId, array, delegate(int res)
		{
			resultCode = res;
		});
		AppCoroutine.Start(task.Run(delegate
		{
			if (resultCode == 1)
			{
				callback();
			}
			else
			{
				string @string = StringMaster.GetString("SystemDataMismatchTitle");
				string message = string.Format(StringMaster.GetString("ChipDataMismatchMesage"), resultCode);
				AlertManager.ShowModalMessage(delegate(int modal)
				{
				}, @string, message, AlertManager.ButtonActionType.Close, false);
			}
			RestrictionInput.EndLoad();
		}, null, null), false);
	}

	private void OnLongTouchChip(GUIListChipParts.Data data)
	{
		global::Debug.LogWarning("LongTouch " + data.userChip.userChipId);
		CMD_QuestItemPOP.Create(data.masterChip);
	}

	private void OnSortButton()
	{
		Action<int> callback = delegate(int result)
		{
			global::Debug.Log("result " + result);
			if (result > 0)
			{
				this.sortNameLabel.text = CMD_ChipSortModal.GetSortName();
				GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] array = this.ConvertChipList(ChipDataMng.userChipData);
				this.chipList.ReAllBuild(array, false);
				this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
				this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
				this.messageLabel.gameObject.SetActive(array.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>() == 0);
			}
		};
		CMD_ChipSortModal.Create(callback);
	}

	private GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] ConvertChipList(GameWebAPI.RespDataCS_ChipListLogic chipListLogic)
	{
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		if (chipListLogic != null && chipListLogic.userChipList != null)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in chipListLogic.userChipList)
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipEnhancedData = ChipDataMng.GetChipEnhancedData(userChipList2.chipId.ToString());
				if (chipEnhancedData != null)
				{
					list.Add(userChipList2);
				}
			}
			CMD_ChipSortModal.UpdateSortedUserChipList(list.ToArray());
			return CMD_ChipSortModal.sortedUserChipList;
		}
		return list.ToArray();
	}
}
