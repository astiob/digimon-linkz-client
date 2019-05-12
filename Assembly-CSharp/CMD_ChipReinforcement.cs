using Master;
using System;
using System.Collections;
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

	private Action fusionSuccessCallback;

	public static CMD_ChipReinforcement Create(Action<int> callback = null)
	{
		return GUIMain.ShowCommonDialog(callback, "CMD_ChipReinforced") as CMD_ChipReinforcement;
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

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		AppCoroutine.Start(this.Init(f, sizeX, sizeY, aT), false);
	}

	private IEnumerator Init(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_ChipReinforced");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList = this.ConvertChipList(ChipDataMng.userChipData);
		this.chipList = new ChipList(this.goEFC_FOOTER, 8, new Vector2(960f, 450f), userChipList, false);
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
		this.messageLabel.gameObject.SetActive(userChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>() == 0);
		this.messageLabel.text = StringMaster.GetString("ChipAdministration-01");
		int currentListNum = 0;
		if (ChipDataMng.userChipData != null && ChipDataMng.userChipData.userChipList != null)
		{
			currentListNum = ChipDataMng.userChipData.userChipList.Length;
		}
		this.listCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), currentListNum, DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax);
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
		yield return null;
		yield break;
	}

	private void OnShortTouchChip(GUIListChipParts.Data data)
	{
		global::Debug.LogWarning("ShortTouch " + data.userChip.userChipId);
		Action<int> callback = delegate(int result)
		{
			if (result > 0)
			{
				AppCoroutine.Start(this.Send(data.userChip), false);
			}
		};
		CMD_ChipReinforcementModal.Create(data.userChip, callback);
	}

	private IEnumerator Send(GameWebAPI.RespDataCS_ChipListLogic.UserChipList baseChip)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		int baseChipId = baseChip.userChipId;
		GameWebAPI.RespDataMA_ChipM.Chip baseMaterChip = ChipDataMng.GetChipMainData(baseChip);
		int needCount = baseMaterChip.needChip.ToInt32();
		int[] needChips = null;
		if (needCount > 0)
		{
			needChips = new int[needCount];
			int i = 0;
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip in ChipDataMng.userChipData.userChipList)
			{
				if (baseChipId != userChip.userChipId && userChip.chipId == baseChip.chipId && userChip.userMonsterId == 0)
				{
					needChips[i] = userChip.userChipId;
					i++;
					if (i >= needChips.Length)
					{
						break;
					}
				}
			}
		}
		Action callback = delegate()
		{
			RestrictionInput.EndLoad();
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] array = this.ConvertChipList(ChipDataMng.userChipData);
			this.chipList.ReAllBuild(array, false);
			this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
			this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
			this.messageLabel.gameObject.SetActive(array.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>() == 0);
			this.listCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), ChipDataMng.userChipData.userChipList.Length, DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax);
			GameWebAPI.RespDataMA_ChipM.Chip chipEnhancedData = ChipDataMng.GetChipEnhancedData(baseMaterChip.chipId);
			CMD_ChipReinforcementAnimation.Create(base.gameObject, chipEnhancedData, null);
		};
		this.fusionSuccessCallback = callback;
		GameWebAPI.ChipFusionLogic logic = ChipDataMng.RequestAPIChipFusion(baseChip.userChipId, needChips, new Action<int, GameWebAPI.RequestMonsterList>(this.EndChipFusion));
		IEnumerator run = logic.Run(null, null, null);
		while (run.MoveNext())
		{
			object obj = run.Current;
			yield return obj;
		}
		yield break;
	}

	private void EndChipFusion(int resultCode, GameWebAPI.RequestMonsterList subRequest)
	{
		if (resultCode == 1)
		{
			if (subRequest != null)
			{
				base.StartCoroutine(subRequest.Run(delegate()
				{
					this.fusionSuccessCallback();
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
					GUIMain.BarrierOFF();
				}, null));
			}
			else
			{
				this.fusionSuccessCallback();
			}
		}
		else
		{
			RestrictionInput.EndLoad();
			ChipTools.CheckResultCode(resultCode);
			string @string = StringMaster.GetString("SystemDataMismatchTitle");
			string message = string.Format(StringMaster.GetString("ChipDataMismatchMesage"), resultCode);
			AlertManager.ShowModalMessage(delegate(int modal)
			{
			}, @string, message, AlertManager.ButtonActionType.Close, false);
		}
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
