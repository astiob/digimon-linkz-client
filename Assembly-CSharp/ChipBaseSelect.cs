using Master;
using System;
using UnityEngine;

public sealed class ChipBaseSelect : MonoBehaviour
{
	public const int defaultMaxChipSlot = 5;

	private const string ChipOn = "Chip_Slot_ON";

	private const string ChipOff = "Chip_Slot_OFF";

	[Header("消すべきオブジェクト")]
	[SerializeField]
	private GameObject[] mustHideObjects;

	[SerializeField]
	[Header("PartsStatusChipのオブジェクト")]
	private GameObject partsStatusChipGO;

	[SerializeField]
	[Header("チップの□スロット")]
	private UISprite[] chipSlots;

	[SerializeField]
	[Header("チップのセル")]
	private BaseSelectChipCell[] chipCells;

	[SerializeField]
	[Header("チップのラベル")]
	private UILabel chipLabel;

	[SerializeField]
	[Header("チップがないメッセージ")]
	private UILabel noChipLabel;

	[SerializeField]
	[Header("分数ラベル(パーティ編成限定)")]
	private UILabel fractionLabel;

	private int myMaxChipSlot;

	private void Awake()
	{
		if (this.chipLabel != null)
		{
			GameWebAPI.RespDataMA_GetAssetCategoryM respDataMA_AssetCategoryM = MasterDataMng.Instance().RespDataMA_AssetCategoryM;
			string categoryID = 17.ToString();
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = respDataMA_AssetCategoryM.GetAssetCategory(categoryID);
			if (assetCategory != null)
			{
				this.chipLabel.text = assetCategory.assetTitle;
			}
		}
	}

	public void InitBaseSelect()
	{
		this.HideObjecs();
		NGUITools.SetActive(this.partsStatusChipGO, true);
	}

	public void OnTouchDecide(Action callback)
	{
		GUIMain.ShowCommonDialog(delegate(int i)
		{
			this.SetSelectedCharChg(CMD_BaseSelect.DataChg);
			callback();
		}, "CMD_ChipSphere");
	}

	private void HideObjecs()
	{
		foreach (GameObject go in this.mustHideObjects)
		{
			NGUITools.SetActive(go, false);
		}
	}

	private void SetNumber(int fraction, int denominator)
	{
		if (this.fractionLabel != null)
		{
			this.fractionLabel.text = string.Format(StringMaster.GetString("SystemFraction"), fraction, denominator);
		}
	}

	public void SetSelectedCharChg(int[] chipIds, bool isShowFractionLabel = false, int maxSlotNum = 0)
	{
		NGUITools.SetActiveSelf(this.fractionLabel.gameObject, isShowFractionLabel);
		if (isShowFractionLabel)
		{
			this.SetNumber(chipIds.Length, maxSlotNum);
		}
		if (chipIds.Length == 0)
		{
			this.SetNoChipMessage(true);
		}
		else
		{
			this.SetNoChipMessage(false);
		}
		for (int i = 0; i < this.chipCells.Length; i++)
		{
			if (chipIds.Length > i)
			{
				this.chipCells[i].SetupIcon(chipIds[i]);
				NGUITools.SetActiveSelf(this.chipCells[i].gameObject, true);
			}
			else
			{
				NGUITools.SetActiveSelf(this.chipCells[i].gameObject, false);
			}
		}
	}

	public void SetSelectedCharChg(MonsterData monsterData)
	{
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo userMonsterSlotInfo = monsterData.userMonsterSlotInfo;
		int num = 0;
		if (userMonsterSlotInfo == null)
		{
			global::Debug.Log("userMonsterSlotInfo == null");
			this.myMaxChipSlot = 0;
			this.SetNumber(0, this.myMaxChipSlot);
			if (this.chipCells != null)
			{
				for (int i = 0; i < this.chipCells.Length; i++)
				{
					NGUITools.SetActiveSelf(this.chipCells[i].gameObject, false);
					global::Debug.Log("ikikioki");
				}
			}
			this.SetNoChipMessage(true);
		}
		else
		{
			GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage manage = userMonsterSlotInfo.manage;
			this.myMaxChipSlot = manage.maxSlotNum + manage.maxExtraSlotNum;
			if (this.myMaxChipSlot != 5 && this.myMaxChipSlot != 10)
			{
				global::Debug.LogErrorFormat("スロット数がありえない. myMaxChipSlot:{0} [{1}, {2}]", new object[]
				{
					this.myMaxChipSlot,
					manage.maxSlotNum,
					manage.maxExtraSlotNum
				});
				this.myMaxChipSlot = 10;
			}
			if (userMonsterSlotInfo.equip != null)
			{
				num = userMonsterSlotInfo.equip.Length;
			}
			this.SetNoChipMessage(num == 0);
			this.SetupChipCells(userMonsterSlotInfo.equip);
			this.SetupChipSlotsAndCells();
			this.SetNumber(num, this.myMaxChipSlot);
			this.SetupChipIcons(num, userMonsterSlotInfo);
		}
	}

	public void ClearChipIcons()
	{
		this.myMaxChipSlot = 0;
		this.SetupChipSlotsAndCells();
		this.SetupChipIcons(0, null);
		this.SetNoChipMessage(true);
	}

	private void SetNoChipMessage(bool isEnabled)
	{
		if (this.noChipLabel != null)
		{
			if (isEnabled)
			{
				this.noChipLabel.text = StringMaster.GetString("MultiRecruit-16");
				NGUITools.SetActiveSelf(this.noChipLabel.gameObject, true);
			}
			else
			{
				NGUITools.SetActiveSelf(this.noChipLabel.gameObject, false);
			}
		}
	}

	private void SetupChipSlotsAndCells()
	{
		if (this.chipSlots == null || this.chipSlots.Length == 0)
		{
			if (this.chipCells != null && this.chipCells.Length != 0)
			{
				for (int i = this.myMaxChipSlot; i < this.chipCells.Length; i++)
				{
					NGUITools.SetActiveSelf(this.chipCells[i].gameObject, false);
				}
			}
			return;
		}
		for (int j = 0; j < this.chipSlots.Length; j++)
		{
			if (j < this.myMaxChipSlot)
			{
				if (this.chipSlots[j] == null)
				{
					break;
				}
				if (this.chipSlots != null && this.chipSlots[j].gameObject != null)
				{
					NGUITools.SetActiveSelf(this.chipSlots[j].gameObject, true);
				}
				if (this.chipCells != null && this.chipCells.Length > 0)
				{
					NGUITools.SetActiveSelf(this.chipCells[j].gameObject, true);
				}
			}
			else
			{
				NGUITools.SetActiveSelf(this.chipSlots[j].gameObject, false);
				if (this.chipCells != null && this.chipCells.Length > 0)
				{
					NGUITools.SetActiveSelf(this.chipCells[j].gameObject, false);
				}
			}
		}
	}

	private void SetupChipIcons(int attachedLength, GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo userMonsterSlotInfo)
	{
		int num = 0;
		if (userMonsterSlotInfo == null)
		{
			global::Debug.Log("DataChg.userMonsterSlotInfo == null");
		}
		else if (userMonsterSlotInfo.manage == null)
		{
			global::Debug.Log("DataChg.userMonsterSlotInfo.manage == null");
		}
		else
		{
			GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage manage = userMonsterSlotInfo.manage;
			num = manage.slotNum + manage.extraSlotNum - attachedLength;
			global::Debug.LogFormat("openedSlotNum:{0}", new object[]
			{
				num
			});
			global::Debug.LogFormat("空きスロット数:{0}, 課金空きスロット数:{1}", new object[]
			{
				manage.slotNum,
				manage.extraSlotNum
			});
			global::Debug.LogFormat("attachedLength:{0}", new object[]
			{
				attachedLength
			});
		}
		for (int i = 0; i < this.chipSlots.Length; i++)
		{
			if (i < attachedLength)
			{
				this.chipSlots[i].spriteName = "Chip_Slot_ON";
				this.chipSlots[i].color = Color.white;
			}
			else
			{
				this.chipSlots[i].spriteName = "Chip_Slot_OFF";
				if (num > 0)
				{
					this.chipSlots[i].color = Color.white;
				}
				else
				{
					this.chipSlots[i].color = Color.gray;
				}
				num--;
			}
		}
	}

	private void SetupChipCells(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip[] equips)
	{
		if (this.chipCells == null || this.chipCells.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < this.myMaxChipSlot; i++)
		{
			if (this.chipCells[i] == null)
			{
				break;
			}
			NGUITools.SetActiveSelf(this.chipCells[i].gameObject, true);
			if (equips != null && i < equips.Length)
			{
				this.chipCells[i].SetupIcon(equips[i]);
			}
			else
			{
				this.chipCells[i].SetupEmptyIcon();
			}
		}
	}
}
