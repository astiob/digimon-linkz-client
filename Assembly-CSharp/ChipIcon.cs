using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipIcon : MonoBehaviour
{
	[Header("装備アイコン")]
	[SerializeField]
	private UISprite equipment;

	[Header("選択中のメッセージラベル")]
	[SerializeField]
	private UILabel dimMessage;

	[SerializeField]
	[Header("チップアイコン")]
	private UITexture iconTexture;

	[Header("ランクアイコン")]
	[SerializeField]
	private UISprite rankSprite;

	[SerializeField]
	[Header("複数選択時のメッセージラベル")]
	private UILabel selectMessage;

	private string texturePath = string.Empty;

	private int defaultTexSizeWidth;

	private int defaultTexSizeHeight;

	private void Awake()
	{
		this.iconTexture.mainTexture = null;
		this.defaultTexSizeWidth = this.iconTexture.width;
		this.defaultTexSizeHeight = this.iconTexture.height;
		this.rankSprite.spriteName = string.Empty;
		this.SetEquipmentIcon(false);
		this.SetSelectMessage(string.Empty);
	}

	public void SetData(GameWebAPI.RespDataMA_ChipM.Chip data = null, int texSizeWidth = -1, int texSizeHeight = -1)
	{
		string text = (data == null) ? string.Empty : ("Chip_Lv" + data.rank);
		if (this.rankSprite.spriteName != text)
		{
			this.rankSprite.spriteName = text;
		}
		string texname = (data == null) ? "ChipThumbnail/Chip_Empty" : data.GetIconPath();
		NGUIUtil.LoadTextureAsync(this.iconTexture, texname, delegate
		{
			if (this != null)
			{
				this.transform.localScale = Vector3.one;
				if (texSizeWidth > 0 && texSizeHeight > 0)
				{
					float x = (float)texSizeWidth / (float)this.defaultTexSizeWidth;
					float y = (float)texSizeHeight / (float)this.defaultTexSizeHeight;
					this.transform.localScale = new Vector3(x, y, 1f);
				}
			}
		});
		if (this.dimMessage != null)
		{
			this.SetNowSelectMessage(false);
		}
		this.SetEquipmentIcon(false);
		this.SetSelectMessage(string.Empty);
	}

	public void SetSelectColor(bool isSelect)
	{
		this.iconTexture.color = ((!isSelect) ? Color.white : ConstValue.DEACTIVE_BUTTON_LABEL);
	}

	public void SetSelectRankColor(bool isSelect)
	{
		this.rankSprite.color = ((!isSelect) ? Color.white : ConstValue.DEACTIVE_BUTTON_LABEL);
	}

	public void SetActive(bool value)
	{
		base.gameObject.SetActive(value);
	}

	public void SetEquipmentIcon(bool value)
	{
		if (this.equipment != null)
		{
			this.equipment.gameObject.SetActive(value);
		}
	}

	public bool SetEquipmentIconWithDim(bool isAlreadyAttached, List<string> myDigimonChipGroupIds, int chipId)
	{
		this.SetEquipmentIcon(isAlreadyAttached);
		this.iconTexture.color = ((!isAlreadyAttached) ? Color.white : ConstValue.DEACTIVE_BUTTON_LABEL);
		this.rankSprite.color = ((!isAlreadyAttached) ? Color.white : ConstValue.DEACTIVE_BUTTON_LABEL);
		bool flag = this.IsAlreadySameGroupIdChipAttached(myDigimonChipGroupIds, chipId);
		this.SetDoubleChipMessage(flag);
		return isAlreadyAttached || flag;
	}

	private bool IsAlreadySameGroupIdChipAttached(List<string> myDigimonChipGroupIds, int chipId)
	{
		if (myDigimonChipGroupIds == null)
		{
			return false;
		}
		string strChipId = chipId.ToString();
		GameWebAPI.RespDataMA_ChipM.Chip chip = MasterDataMng.Instance().RespDataMA_ChipMaster.chipM.Where((GameWebAPI.RespDataMA_ChipM.Chip c) => c.chipId == strChipId).SingleOrDefault<GameWebAPI.RespDataMA_ChipM.Chip>();
		if (chip == null)
		{
			global::Debug.LogError("なぜかchipIdがおかしい.");
			return true;
		}
		return myDigimonChipGroupIds.Contains(chip.chipGroupId);
	}

	private void SetDoubleChipMessage(bool isAlready)
	{
		if (isAlready)
		{
			this.iconTexture.color = ConstValue.DEACTIVE_BUTTON_LABEL;
			this.rankSprite.color = ConstValue.DEACTIVE_BUTTON_LABEL;
		}
	}

	public void SetNowSelectMessage(bool isActive)
	{
		if (this.dimMessage != null)
		{
			if (isActive)
			{
				this.dimMessage.text = StringMaster.GetString("SystemSelect");
			}
			else
			{
				this.dimMessage.text = string.Empty;
			}
		}
	}

	public void SetSelectMessage(string value)
	{
		if (this.selectMessage != null)
		{
			this.selectMessage.text = value;
		}
	}
}
