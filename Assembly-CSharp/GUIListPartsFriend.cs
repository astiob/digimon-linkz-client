using Master;
using System;
using UnityEngine;

public class GUIListPartsFriend : GUIListPartBS
{
	public GameObject goMN_ICON;

	public GameObject goTL_ICON;

	public GameObject goTX_NICKNAME;

	public GameObject goTX_EXP;

	public GameObject goTX_LAST_LOGIN;

	[SerializeField]
	private UISprite bgSprite;

	private UILabel ngTX_NICKNAME;

	private UILabel ngTX_EXP;

	private UILabel ngTX_LAST_LOGIN;

	private GameWebAPI.FriendList data;

	private bool isTouchEndFromChild;

	public string lastLoginTime;

	private bool isUpdateMIcon;

	private GameObject goMN_ICON_2;

	private MonsterData md_favo;

	public GameWebAPI.FriendList Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngTX_NICKNAME = this.goTX_NICKNAME.GetComponent<UILabel>();
		this.ngTX_EXP = this.goTX_EXP.GetComponent<UILabel>();
		this.ngTX_LAST_LOGIN = this.goTX_LAST_LOGIN.GetComponent<UILabel>();
	}

	public void ChangeSprite(string sprName)
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		if (component != null)
		{
			component.spriteName = sprName;
			component.MakePixelPerfect();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.ShowParam();
	}

	private void ShowParam()
	{
		string str = string.Empty;
		if (!this.data.userData.loginTime.Contains(StringMaster.GetString("Friend-13")))
		{
			str = StringMaster.GetString("Friend-13");
		}
		this.ngTX_NICKNAME.text = this.data.userData.nickname;
		TitleDataMng.SetTitleIcon(this.data.userData.titleId, this.goTL_ICON.GetComponent<UITexture>());
		this.ngTX_EXP.text = this.data.userData.description;
		this.ngTX_LAST_LOGIN.text = StringMaster.GetString("Friend-12") + this.data.userData.loginTime + str;
		this.lastLoginTime = this.data.userData.loginTime;
	}

	private void actMIconLong(MonsterData md)
	{
		CMD_CharacterDetailed.DataChg = md;
		GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed");
	}

	private void UpdateMonsterIcon()
	{
		if (!this.isUpdateMIcon && this.data != null)
		{
			this.md_favo = MonsterDataMng.Instance().CreateMonsterDataByMID(this.data.monsterData.monsterId);
			GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(this.md_favo, this.goMN_ICON.transform.localScale, this.goMN_ICON.transform.localPosition, this.goMN_ICON.transform.parent, true, true);
			this.goMN_ICON_2 = guimonsterIcon.gameObject;
			this.goMN_ICON_2.SetActive(true);
			guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.actMIconLong));
			guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.actMIconLong));
			UIWidget component = this.goMN_ICON.GetComponent<UIWidget>();
			UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
			if (component != null && component2 != null)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(guimonsterIcon.transform, add);
			}
			this.goMN_ICON.SetActive(false);
			this.isUpdateMIcon = true;
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this.beganPostion = pos;
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				this.OnTouchEndedProcess();
			}
		}
	}

	protected virtual void OnTouchEndedProcess()
	{
		CMD_ProfileFriend.friendData = this.Data;
		CMD_FriendTop.instance.ListPartsOperate(this, base.IDX);
		CMD_FriendTop.instance.selectUserlastLoginTime = this.lastLoginTime;
	}

	private void OnClickedBtnSelect()
	{
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateMonsterIcon();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void ChangeSelectItem(bool isSelect)
	{
		this.bgSprite.color = ((!isSelect) ? new Color(0f, 0.392156869f, 1f, 0.784313738f) : new Color(0.392156869f, 0f, 0f, 0.784313738f));
	}

	public void ChangeUnselectItem(bool isUnselect)
	{
		this.bgSprite.color = ((!isUnselect) ? new Color(0f, 0.392156869f, 1f, 0.784313738f) : new Color(0f, 0f, 0f, 0.784313738f));
		base.GetComponent<Collider>().enabled = !isUnselect;
	}
}
