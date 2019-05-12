using Master;
using System;
using UnityEngine;

public sealed class PartyEditPartyInfo : MonoBehaviour
{
	[SerializeField]
	private UILabel partyNo;

	[SerializeField]
	private GameObject favoriteButton;

	[SerializeField]
	private UILabel favoriteButtonLabel;

	[SerializeField]
	private Color favoriteButtonLabelColor;

	private GUICollider favoriteButtonCollider;

	private UISprite favoriteButtonBackground;

	private int favoriteDeckNo;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private GameObject statusChangeButton;

	[SerializeField]
	private UISprite nameUnderLine;

	[SerializeField]
	private UISprite infoUnderLine;

	[SerializeField]
	private PartyEditPartyInfo.ViewInfo partyEditViewInfo;

	private void InitializeFavoriteButton(CMD_PartyEdit.MODE_TYPE type)
	{
		string favoriteDeckNum = DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum;
		this.favoriteDeckNo = int.Parse(favoriteDeckNum);
		this.favoriteButtonCollider = this.favoriteButton.GetComponent<GUICollider>();
		this.favoriteButtonBackground = this.favoriteButton.GetComponent<UISprite>();
		if (type != CMD_PartyEdit.MODE_TYPE.MULTI && type != CMD_PartyEdit.MODE_TYPE.SELECT)
		{
			if (!this.favoriteButton.activeSelf)
			{
				this.favoriteButton.SetActive(true);
			}
		}
		else
		{
			this.favoriteButton.SetActive(false);
		}
	}

	private void SetFavoriteButton(bool enable)
	{
		if (this.favoriteButton.activeSelf)
		{
			this.favoriteButtonCollider.activeCollider = !enable;
			if (enable)
			{
				this.favoriteButtonBackground.spriteName = "Common02_Btn_SupportRed";
				this.favoriteButtonLabel.color = Color.white;
			}
			else
			{
				this.favoriteButtonBackground.spriteName = "Common02_Btn_SupportWhite";
				this.favoriteButtonLabel.color = this.favoriteButtonLabelColor;
			}
		}
	}

	public void SetView(CMD_PartyEdit.MODE_TYPE type)
	{
		if (type == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			Transform transform = this.statusChangeButton.transform;
			transform.localPosition = new Vector3(this.partyEditViewInfo.statusChangeButton.x, this.partyEditViewInfo.statusChangeButton.y, transform.localPosition.z);
			this.nameUnderLine.width = this.partyEditViewInfo.nameUnderLine;
			this.infoUnderLine.width = this.partyEditViewInfo.infoUnderLine;
		}
		this.InitializeFavoriteButton(type);
	}

	public void SetPartyInfo(int viewPartyNo, MonsterData monsterData)
	{
		this.partyNo.text = string.Format(StringMaster.GetString("PartyNumber"), viewPartyNo);
		this.monsterLeaderSkill.SetSkill(monsterData);
	}

	public void SetFavoriteDeckNo(int partyNo)
	{
		this.favoriteDeckNo = partyNo;
		this.SetFavoriteButton(true);
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		if (cmd_ModalMessage != null)
		{
			cmd_ModalMessage.Title = StringMaster.GetString("PartyFavoriteTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("PartyFavoriteInfo");
		}
	}

	public int GetFavoriteDeckNo()
	{
		return this.favoriteDeckNo;
	}

	public void EnableFavoriteButton(bool enable)
	{
		this.SetFavoriteButton(enable);
	}

	[Serializable]
	private sealed class ViewInfo
	{
		[SerializeField]
		public Vector2 statusChangeButton;

		[SerializeField]
		public int nameUnderLine;

		[SerializeField]
		public int infoUnderLine;
	}
}
