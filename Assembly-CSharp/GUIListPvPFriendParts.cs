using System;
using UnityEngine;

public class GUIListPvPFriendParts : GUIListPartBS
{
	[SerializeField]
	private UILabel lbUserName;

	[SerializeField]
	private GameObject goTitleIcon;

	[SerializeField]
	private UILabel lbUserComment;

	[SerializeField]
	private UILabel lbLastLogin;

	[SerializeField]
	private GameObject goMonsterIcon;

	[SerializeField]
	private UISprite spBgPlate;

	[SerializeField]
	private Color activeListColor;

	[SerializeField]
	private Color defaultListColor;

	private bool isSelected;

	private GameWebAPI.FriendList friendData;

	public bool IsSelected
	{
		get
		{
			return this.isSelected;
		}
	}

	public GameWebAPI.FriendList FriendData
	{
		get
		{
			return this.friendData;
		}
		set
		{
			this.friendData = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.SetInitLabel();
		base.ShowGUI();
	}

	private void SetInitLabel()
	{
		this.lbUserName.text = this.friendData.userData.nickname;
		this.lbUserComment.text = this.friendData.userData.description;
		this.lbLastLogin.text = this.friendData.userData.loginTime;
		TitleDataMng.SetTitleIcon(this.friendData.userData.titleId, this.goTitleIcon.GetComponent<UITexture>());
		MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.friendData.monsterData.monsterId);
		if (monsterData != null)
		{
			GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, this.goMonsterIcon.transform.localScale, this.goMonsterIcon.transform.localPosition, this.goMonsterIcon.transform.parent, true, false);
			DepthController depthController = guimonsterIcon.GetDepthController();
			depthController.AddWidgetDepth(guimonsterIcon.transform, 1800);
		}
	}

	private void UpdateSelectStatus()
	{
		if (this.isSelected)
		{
			this.spBgPlate.color = this.activeListColor;
		}
		else
		{
			this.spBgPlate.color = this.defaultListColor;
		}
	}

	public void ForceRelease()
	{
		this.isSelected = false;
		this.UpdateSelectStatus();
	}

	private void OnClickedInfo()
	{
		this.isSelected = !this.isSelected;
		this.UpdateSelectStatus();
		if (this.isSelected)
		{
			CMD_PvPFriend.Instance.UnSelectedAnother(this);
		}
		CMD_PvPFriend.Instance.CheckEnableBtnRecruit();
	}
}
