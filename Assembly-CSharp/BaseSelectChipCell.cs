using Master;
using System;
using UnityEngine;

public sealed class BaseSelectChipCell : MonoBehaviour
{
	[Header("サムネイルのテクスチャ")]
	[SerializeField]
	private UITexture thumbnailTexture;

	[Header("レア度のスプライト")]
	[SerializeField]
	private UISprite rareSprite;

	[SerializeField]
	[Header("チップ名のラベル")]
	private UILabel chipNameLabel;

	[SerializeField]
	[Header("非装着のラベル")]
	private UILabel noChipLabel;

	public void SetupIcon(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip)
	{
		GameWebAPI.RespDataMA_ChipM.Chip chipData = ChipTools.GetChipData(equip);
		this.SetupIcon(chipData);
	}

	public void SetupIcon(int chipId)
	{
		GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(chipId.ToString());
		this.SetupIcon(chipMainData);
	}

	private void SetupIcon(GameWebAPI.RespDataMA_ChipM.Chip chipMainData)
	{
		NGUITools.SetActiveSelf(this.thumbnailTexture.gameObject, true);
		NGUIUtil.ChangeUITextureFromFileASync(this.thumbnailTexture, chipMainData.GetIconPath(), false, null);
		if (this.rareSprite != null)
		{
			this.rareSprite.spriteName = ChipTools.GetRankPath(chipMainData.rank);
			NGUITools.SetActiveSelf(this.rareSprite.gameObject, true);
		}
		if (this.noChipLabel != null)
		{
			NGUITools.SetActiveSelf(this.noChipLabel.gameObject, false);
		}
		if (this.chipNameLabel != null)
		{
			NGUITools.SetActiveSelf(this.chipNameLabel.gameObject, true);
			this.chipNameLabel.text = chipMainData.name;
		}
	}

	public void SetupEmptyIcon()
	{
		NGUIUtil.ChangeUITextureFromFileASync(this.thumbnailTexture, "ChipThumbnail/Chip_NotOpen", false, null);
		if (this.chipNameLabel != null)
		{
			NGUITools.SetActiveSelf(this.chipNameLabel.gameObject, false);
		}
		if (this.rareSprite != null)
		{
			NGUITools.SetActiveSelf(this.rareSprite.gameObject, false);
		}
		if (this.noChipLabel != null)
		{
			NGUITools.SetActiveSelf(this.noChipLabel.gameObject, true);
			this.noChipLabel.text = StringMaster.GetString("ChipInstalling-13");
		}
		if (this.thumbnailTexture != null)
		{
			NGUITools.SetActiveSelf(this.thumbnailTexture.gameObject, false);
		}
	}
}
