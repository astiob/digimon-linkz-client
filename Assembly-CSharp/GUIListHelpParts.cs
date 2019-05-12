using System;
using UnityEngine;

public class GUIListHelpParts : GUIListPartBS
{
	public GameObject helpCMD;

	public GameObject goTX_TITLE;

	private UILabel ngTX_TITLE;

	private Color defaultColor;

	private string helpCategoryId;

	[SerializeField]
	private UISprite backgroundImg;

	private GameWebAPI.RespDataMA_GetHelpCategoryM.HelpCategoryM categoryData;

	private GameWebAPI.RespDataMA_GetHelpM.HelpM listData;

	public GameWebAPI.RespDataMA_GetHelpCategoryM.HelpCategoryM CategoryData
	{
		get
		{
			return this.categoryData;
		}
		set
		{
			this.categoryData = value;
			this.ShowGUI();
		}
	}

	public GameWebAPI.RespDataMA_GetHelpM.HelpM ListData
	{
		get
		{
			return this.listData;
		}
		set
		{
			this.listData = value;
			this.ShowGUI();
		}
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ShowGUI()
	{
		if (!this.isUpdate)
		{
			base.ShowGUI();
			this.defaultColor = this.backgroundImg.color;
		}
		if (this.helpCMD.GetComponent<CMD_HelpCategory>())
		{
			this.ngTX_TITLE = this.goTX_TITLE.GetComponent<UILabel>();
			this.ngTX_TITLE.text = this.categoryData.helpCategoryName;
			this.helpCategoryId = this.categoryData.helpCategoryId;
		}
		else
		{
			this.ngTX_TITLE = this.goTX_TITLE.GetComponent<UILabel>();
			this.ngTX_TITLE.text = this.ListData.helpTitle;
		}
	}

	private void OnClickedParts()
	{
		if (this.helpCMD.GetComponent<CMD_HelpCategory>())
		{
			CMD_HelpList.Data = MasterDataMng.Instance().RespDataMA_HelpM;
			CMD_HelpList.helpCategoryId = this.helpCategoryId;
			GUIMain.ShowCommonDialog(null, "CMD_HelpList", null);
		}
		else
		{
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null);
			((CMDWebWindow)commonDialog).TitleText = this.ListData.helpTitle;
			((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_HELP_DETAIL + this.ListData.helpId;
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
		base.OnTouchBegan(touch, pos);
		GUICollider.TouchBehavior touchBehavior = this.touchBehavior;
		if (touchBehavior == GUICollider.TouchBehavior.ChangeColor)
		{
			this.backgroundImg.color = this.changeColor;
		}
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
		GUICollider.TouchBehavior touchBehavior = this.touchBehavior;
		if (touchBehavior == GUICollider.TouchBehavior.ChangeColor)
		{
			this.backgroundImg.color = this.defaultColor;
		}
	}
}
