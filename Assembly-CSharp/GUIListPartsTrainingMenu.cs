using Master;
using System;
using UnityEngine;

public class GUIListPartsTrainingMenu : GUIListPartBS
{
	[Header("ベースのスプライト")]
	[SerializeField]
	private UISprite spBase;

	[Header("NEWを示すアイコン")]
	[SerializeField]
	private UISprite spNew;

	[Header("ビックリマーク")]
	[SerializeField]
	private UISprite spInfo;

	[Header("タイトルラベル")]
	[SerializeField]
	private UILabel lbTX_Title;

	[Header("タイトル左")]
	[SerializeField]
	private UISprite spL_Title;

	[Header("タイトル右")]
	[SerializeField]
	private UISprite spR_Title;

	[Header("キャンペーンラベル")]
	[SerializeField]
	private UILabel lbTX_Campaign;

	private GUIListPartsTrainingMenu.PartsData data;

	public override void SetData()
	{
		this.data = CMD_Training_Menu.instance.GetData(base.IDX);
		string strTitle = this.data.strTitle;
		if (strTitle != null)
		{
			if (!(strTitle == "MealTitle"))
			{
				if (strTitle == "ChipSphereTitle")
				{
					TutorialEmphasizeUI tutorialEmphasizeUI = base.gameObject.AddComponent<TutorialEmphasizeUI>();
					tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.CHIP_INSTALLING;
				}
			}
			else
			{
				TutorialEmphasizeUI tutorialEmphasizeUI = base.gameObject.AddComponent<TutorialEmphasizeUI>();
				tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.MEAL;
			}
		}
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	public override void ShowGUI()
	{
		this.ShowData();
		base.ShowGUI();
	}

	private void ShowData()
	{
		this.spBase.color = this.data.col;
		this.spNew.gameObject.SetActive(this.data.isNew);
		this.spInfo.gameObject.SetActive(this.data.isInfo);
		this.lbTX_Title.text = StringMaster.GetString(this.data.strTitle);
		this.lbTX_Title.color = this.data.labelCol;
		this.spL_Title.color = this.data.LRCol;
		this.spR_Title.color = this.data.LRCol;
		this.lbTX_Campaign.text = this.data.strCampaign;
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
			if (magnitude < 40f && this.data.actCallBack != null)
			{
				this.data.actCallBack();
			}
		}
	}

	[Serializable]
	public class PartsData
	{
		public bool isNew;

		public bool isInfo;

		public string strTitle;

		public string strCampaign;

		public Color col;

		[NonSerialized]
		public Color labelCol;

		[NonSerialized]
		public Color LRCol;

		public Action actCallBack;
	}
}
