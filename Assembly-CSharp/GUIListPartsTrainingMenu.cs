using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIListPartsTrainingMenu : GUIListPartBS
{
	[SerializeField]
	[Header("ベースのスプライト")]
	private UISprite spBase;

	[Header("NEWを示すアイコン")]
	[SerializeField]
	private UISprite spNew;

	[SerializeField]
	[Header("ビックリマーク")]
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

	[SerializeField]
	[Header("キャンペーンラベル")]
	private UILabel lbTX_Campaign;

	private GUIListPartsTrainingMenu.PartsData data;

	public override void SetData()
	{
		this.data = CMD_Training_Menu.instance.GetData(base.IDX);
		string strTitle = this.data.strTitle;
		if (strTitle != null)
		{
			if (GUIListPartsTrainingMenu.<>f__switch$map39 == null)
			{
				GUIListPartsTrainingMenu.<>f__switch$map39 = new Dictionary<string, int>(2)
				{
					{
						"MealTitle",
						0
					},
					{
						"ChipSphereTitle",
						1
					}
				};
			}
			int num;
			if (GUIListPartsTrainingMenu.<>f__switch$map39.TryGetValue(strTitle, out num))
			{
				if (num != 0)
				{
					if (num == 1)
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
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	protected override void Awake()
	{
		base.Awake();
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	[Serializable]
	public class PartsData
	{
		public bool isNew;

		public bool isInfo;

		public string strTitle;

		public string strCampaign;

		public Color col;

		public Color labelCol;

		public Color LRCol;

		public Action actCallBack;
	}
}
