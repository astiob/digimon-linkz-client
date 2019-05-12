using System;
using UnityEngine;

public class GUIListMultiRecruitFriendParts : GUIListPartBS
{
	[SerializeField]
	private UILabel lbUserName;

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

	public bool isSelected;

	public GameWebAPI.FriendList friendData;

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
		base.ShowGUI();
		this.SetInitLabel();
	}

	private void SetInitLabel()
	{
		this.lbUserName.text = this.friendData.userData.nickname;
		this.lbUserComment.text = this.friendData.userData.description;
		this.lbLastLogin.text = this.friendData.userData.loginTime;
		MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.friendData.monsterData.monsterId);
		if (monsterData != null)
		{
			GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(monsterData, this.goMonsterIcon.transform.localScale, this.goMonsterIcon.transform.localPosition, this.goMonsterIcon.transform.parent, true, false);
			if (guimonsterIcon.transform.parent != null)
			{
				UIWidget component = guimonsterIcon.transform.parent.gameObject.GetComponent<UIWidget>();
				if (component != null)
				{
					DepthController.SetWidgetDepth_2(guimonsterIcon.transform, component.depth + 2);
				}
			}
		}
	}

	private void OnCrickedInfo()
	{
		this.isSelected = !this.isSelected;
		if (this.isSelected)
		{
			this.spBgPlate.color = this.activeListColor;
		}
		else
		{
			this.spBgPlate.color = this.defaultListColor;
		}
		CMD_MultiRecruitFriend.Instance.CheckEnableBtnRecruit();
	}

	public void ForceSelect(bool isSelect)
	{
		this.isSelected = isSelect;
		this.spBgPlate.color = ((!isSelect) ? this.defaultListColor : this.activeListColor);
	}
}
