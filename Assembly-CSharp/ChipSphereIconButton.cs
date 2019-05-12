using System;
using UnityEngine;

public sealed class ChipSphereIconButton : GUICollider
{
	private const string emptyFrame = "Chip_Sphere_Thumbnail_OFF";

	private const string putFrame = "Chip_Sphere_Thumbnail_ON";

	private ChipSphereIconButton.Parameter myParameter;

	[Header("枠のスプライト")]
	[SerializeField]
	private UISprite frameSprite;

	[SerializeField]
	[Header("LOOKのオブジェクト")]
	private GameObject lookGO;

	[SerializeField]
	[Header("Chargesのオブジェクト")]
	private GameObject chargesGO;

	[Header("選択中(Chip_choosing)")]
	[SerializeField]
	private GameObject choosingGO;

	[Header("チップの名前/拡張説明")]
	[SerializeField]
	private UILabel chipNameLabel;

	[Header("チップの説明ラベル/拡張パッチの個数")]
	[SerializeField]
	private UILabel chipDescriptLabel;

	[SerializeField]
	[Header("チップのテクスチャ/枠")]
	private UITexture chipTexture;

	[SerializeField]
	[Header("アイテムのスプライト")]
	private UISprite itemSprite;

	[Header("LOOKのスプライト")]
	[SerializeField]
	private UISprite lookSprite;

	[SerializeField]
	[Header("チップランクのスプライト")]
	private UISprite rankSprite;

	public CMD_ChipSphere cmdChipSphere { private get; set; }

	public bool isOpened
	{
		get
		{
			return this.myParameter.isOpened;
		}
		private set
		{
			this.myParameter.isOpened = value;
		}
	}

	private bool isExtendable
	{
		set
		{
			this.myParameter.isExtendable = value;
		}
	}

	public bool isPuttedChip
	{
		get
		{
			return this.myParameter.menuType == CMD_ChipSphere.MenuType.Detail;
		}
	}

	public void SetChoose()
	{
		NGUITools.SetActiveSelf(this.choosingGO, true);
	}

	public void SetUnChoose()
	{
		NGUITools.SetActiveSelf(this.choosingGO, false);
	}

	public void SetupChip(ChipSphereIconButton.Parameter parameter)
	{
		NGUITools.SetActiveSelf(this.chipNameLabel.gameObject, false);
		NGUITools.SetActiveSelf(this.chipDescriptLabel.gameObject, false);
		this.myParameter = parameter;
		switch (parameter.menuType)
		{
		case CMD_ChipSphere.MenuType.Empty:
			this.SetupEmpty();
			break;
		case CMD_ChipSphere.MenuType.Extendable:
			this.SetupExtendable();
			break;
		case CMD_ChipSphere.MenuType.NotYet:
			this.SetupNotYet();
			break;
		case CMD_ChipSphere.MenuType.Detail:
			this.SetupDetail(this.myParameter.userChipId, this.myParameter.GetChipMainData());
			break;
		default:
			global::Debug.LogError("ありえない.");
			break;
		}
	}

	public void SetupEmpty()
	{
		this.isOpened = true;
		this.frameSprite.spriteName = "Chip_Sphere_Thumbnail_OFF";
		this.myParameter.menuType = CMD_ChipSphere.MenuType.Empty;
		NGUITools.SetActiveSelf(this.rankSprite.gameObject, false);
		NGUITools.SetActiveSelf(this.chargesGO, false);
		NGUITools.SetActiveSelf(this.lookGO, false);
		NGUIUtil.ChangeUITextureFromFileASync(this.chipTexture, "ChipThumbnail/Chip_Empty", false, null);
	}

	private void SetupExtendable()
	{
		this.isOpened = false;
		NGUITools.SetActiveSelf(this.rankSprite.gameObject, false);
		this.chipNameLabel.text = this.myParameter.chipName;
		NGUITools.SetActiveSelf(this.chipDescriptLabel.gameObject, true);
		this.chipDescriptLabel.text = string.Format("x{0}", this.myParameter.itemCount);
		NGUITools.SetActiveSelf(this.chargesGO, true);
		NGUITools.SetActiveSelf(this.lookSprite.gameObject, false);
		NGUIUtil.ChangeUITextureFromFileASync(this.chipTexture, "ChipThumbnail/Chip_NotOpen", false, null);
	}

	private void SetupNotYet()
	{
		NGUITools.SetActiveSelf(this.rankSprite.gameObject, false);
		NGUIUtil.ChangeUITextureFromFileASync(this.chipTexture, "ChipThumbnail/Chip_NotOpen", false, null);
		NGUITools.SetActiveSelf(this.chargesGO, false);
		NGUITools.SetActiveSelf(this.lookGO, true);
		NGUITools.SetActiveSelf(this.chipNameLabel.gameObject, true);
		this.chipNameLabel.text = this.myParameter.chipName;
		NGUITools.SetActiveSelf(this.lookSprite.gameObject, true);
		this.lookSprite.spriteName = "Common02_Icon_KeyQ";
	}

	public void SetupOnlyDetailParams(int userChipId, GameWebAPI.RespDataMA_ChipM.Chip chipData)
	{
		this.myParameter.menuType = CMD_ChipSphere.MenuType.Detail;
		this.myParameter.chipName = chipData.name;
		this.myParameter.chipDetail = chipData.detail;
		this.myParameter.chipRank = chipData.rank;
		this.myParameter.chipIconPath = chipData.GetIconPath();
		this.myParameter.userChipId = userChipId;
	}

	public void SetupDetail(int userChipId, GameWebAPI.RespDataMA_ChipM.Chip chipData)
	{
		this.isOpened = true;
		this.SetupOnlyDetailParams(userChipId, chipData);
		this.frameSprite.spriteName = "Chip_Sphere_Thumbnail_ON";
		NGUITools.SetActiveSelf(this.chargesGO, false);
		NGUITools.SetActiveSelf(this.lookGO, false);
		NGUITools.SetActiveSelf(this.chipNameLabel.gameObject, true);
		NGUITools.SetActiveSelf(this.chipDescriptLabel.gameObject, true);
		this.chipNameLabel.text = this.myParameter.chipName;
		this.chipDescriptLabel.text = this.myParameter.chipDetail;
		NGUIUtil.ChangeUITextureFromFileASync(this.chipTexture, chipData.GetIconPath(), false, null);
		NGUITools.SetActiveSelf(this.rankSprite.gameObject, true);
		this.rankSprite.spriteName = ChipTools.GetRankPath(chipData.rank);
	}

	public void RefreshItemCountColor(int itemCount)
	{
		if (this.myParameter.menuType == CMD_ChipSphere.MenuType.Extendable)
		{
			bool flag = this.myParameter.itemCount <= itemCount;
			this.chipDescriptLabel.color = ((!flag) ? Color.red : Color.white);
		}
	}

	public void OnTouch()
	{
		this.cmdChipSphere.OnTouchChipIcon(this.myParameter);
	}

	public void SetChipColor(bool isExtendable)
	{
		Color color = (!isExtendable) ? Color.gray : Color.white;
		this.isExtendable = isExtendable;
		this.itemSprite.color = color;
		this.rankSprite.color = color;
		this.frameSprite.color = color;
	}

	public struct Parameter
	{
		public bool isOpened;

		public bool isExtendable;

		public int buttonNo;

		public int type;

		public CMD_ChipSphere.MenuType menuType;

		public int itemCount;

		public string chipRank;

		public string chipName;

		public string chipDetail;

		public string chipIconPath;

		public int userChipId;

		public GameWebAPI.RespDataMA_ChipM.Chip GetChipMainData()
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipDataByUserChipId = ChipDataMng.GetUserChipDataByUserChipId(this.userChipId);
			return ChipDataMng.GetChipMainData(userChipDataByUserChipId);
		}

		public string ConvertChipGroupId()
		{
			return this.GetChipMainData().chipGroupId;
		}

		public string ConvertChipId()
		{
			return this.GetChipMainData().chipId;
		}

		public int ConvertDispNum()
		{
			if (this.type == 0)
			{
				return this.buttonNo;
			}
			if (this.type == 1)
			{
				return this.buttonNo - 5;
			}
			global::Debug.LogError("ありえない.");
			return this.buttonNo;
		}

		public int ConvertButtonIndex()
		{
			return this.buttonNo - 1;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is ChipSphereIconButton.Parameter))
			{
				return false;
			}
			ChipSphereIconButton.Parameter rhs = (ChipSphereIconButton.Parameter)obj;
			return this == rhs;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(ChipSphereIconButton.Parameter lhs, ChipSphereIconButton.Parameter rhs)
		{
			return lhs.isOpened == rhs.isOpened && lhs.buttonNo == rhs.buttonNo && lhs.type == rhs.type && lhs.menuType == rhs.menuType && lhs.itemCount == rhs.itemCount && lhs.chipRank == rhs.chipRank && lhs.chipName == rhs.chipName && lhs.chipDetail == rhs.chipDetail && lhs.chipIconPath == rhs.chipIconPath && lhs.userChipId == rhs.userChipId;
		}

		public static bool operator !=(ChipSphereIconButton.Parameter lhs, ChipSphereIconButton.Parameter rhs)
		{
			return !(lhs == rhs);
		}
	}
}
