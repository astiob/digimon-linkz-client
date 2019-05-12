using Master;
using Quest;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public sealed class CMDWebWindowPopup : CMDWebWindow
{
	public static readonly Dictionary<int, string> linkCategoryTypeToMethod = new Dictionary<int, string>
	{
		{
			1,
			"Quest"
		},
		{
			2,
			"Shop"
		},
		{
			3,
			"Gasha"
		}
	};

	private bool dontShowAgainFlg;

	[SerializeField]
	private GameObject checkDontShowAgain;

	[SerializeField]
	private GameObject goLinkBtn;

	[SerializeField]
	private UILabel lblLinkBtn;

	public int userInfoId;

	private int linkCategoryType;

	private string getCategoryTypeKey()
	{
		if (CMDWebWindowPopup.linkCategoryTypeToMethod.ContainsKey(this.linkCategoryType))
		{
			return CMDWebWindowPopup.linkCategoryTypeToMethod[this.linkCategoryType];
		}
		return null;
	}

	protected override void WindowOpened()
	{
		if (!string.IsNullOrEmpty(this._Url))
		{
			this.StartWebView(null);
		}
	}

	private void StartWebView(string url = null)
	{
		if (url != null)
		{
			this._Url = url;
		}
		base.WebViewOpen(this._Url);
	}

	public void setLinkCategoryType(int linkCategoryType)
	{
		this.linkCategoryType = linkCategoryType;
		string categoryTypeKey = this.getCategoryTypeKey();
		if (categoryTypeKey != null)
		{
			this.lblLinkBtn.text = StringMaster.GetString(string.Format("PopupTo{0}", categoryTypeKey));
		}
		else
		{
			this.goLinkBtn.SetActive(false);
		}
	}

	public void OnToggleDontShowAgain()
	{
		bool active = !this.checkDontShowAgain.activeSelf;
		this.checkDontShowAgain.SetActive(active);
		this.dontShowAgainFlg = active;
	}

	public void OnClickLink()
	{
		DataMng.Instance().ShowPopupInfoNum = DataMng.Instance().RespDataIN_InfoList.infoList.Length;
		string categoryTypeKey = this.getCategoryTypeKey();
		if (categoryTypeKey != null)
		{
			MethodInfo method = base.GetType().GetMethod(string.Format("OnClick{0}", categoryTypeKey));
			if (method != null)
			{
				method.Invoke(this, new object[0]);
			}
		}
	}

	public void OnClickQuest()
	{
		if (CMD_QuestSelect.instance == null)
		{
			this.finish(1);
			this.ClosePanelNotEndShow(true);
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			List<string> list = new List<string>();
			list.Add("1");
			list.Add("3");
			list.Add("8");
			ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(list, new Action<bool>(base.actCBQuest));
		}
		else
		{
			this.ClosePanel(true);
		}
	}

	public void OnClickShop()
	{
		Action actCallBack = delegate()
		{
			GUIMain.ShowCommonDialog(null, "CMD_Shop", null);
			FarmRoot instance = FarmRoot.Instance;
			if (null != instance)
			{
				instance.ClearSettingFarmObject();
			}
		};
		GUIManager.CloseAllCommonDialog(actCallBack);
	}

	public void OnClickGasha()
	{
		Action actCallBack = delegate()
		{
			GUIMain.ShowCommonDialog(null, "CMD_GashaTOP", null);
			FarmRoot instance = FarmRoot.Instance;
			if (null != instance)
			{
				instance.ClearSettingFarmObject();
			}
		};
		GUIManager.CloseAllCommonDialog(actCallBack);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.dontShowAgainFlg)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			GameWebAPI.RequestIN_disablePopup request = new GameWebAPI.RequestIN_disablePopup
			{
				SetSendData = delegate(GameWebAPI.SendDataIN_disablePopup param)
				{
					param.userInfoId = this.userInfoId;
				}
			};
			APIRequestTask task = new APIRequestTask(request, true);
			base.StartCoroutine(task.Run(delegate
			{
				RestrictionInput.EndLoad();
				this.<ClosePanel>__BaseCallProxy0(animation);
			}, null, null));
		}
		else
		{
			if (DataMng.Instance().ShowPopupInfoIds != null)
			{
				if (!DataMng.Instance().ShowPopupInfoIds.Contains(this.userInfoId))
				{
					DataMng.Instance().ShowPopupInfoIds.Enqueue(this.userInfoId);
				}
				while (DataMng.Instance().ShowPopupInfoIds.Count > DataMng.MAX_SHOW_POPUP_INFO_IDS)
				{
					DataMng.Instance().ShowPopupInfoIds.Dequeue();
				}
			}
			base.ClosePanel(animation);
		}
	}

	private enum LinkCategoryType
	{
		None,
		Quest,
		Shop,
		Gasha,
		News,
		Event
	}
}
