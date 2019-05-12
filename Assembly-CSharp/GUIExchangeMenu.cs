using ExchangeData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIExchangeMenu : MonoBehaviour
{
	public static GUIExchangeMenu instance;

	[SerializeField]
	private GUISelectPanelExcangeMenu csSelectPanelExchangeMenu;

	[SerializeField]
	private GameObject goListParts;

	[SerializeField]
	private GameObject goNoneExchangeWindow;

	[SerializeField]
	private GameObject goScrollBar;

	[SerializeField]
	private GameObject goScrollBarBg;

	private List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result> exchangeResultInfoList;

	private Dictionary<string, bool> exchangeUpdateDict = new Dictionary<string, bool>();

	private List<ExchangeMenuItem> exchangeMenuItemList = new List<ExchangeMenuItem>();

	private string prefsKeyString = "ExchangeNew_";

	private void Awake()
	{
		GUIExchangeMenu.instance = this;
	}

	public void Init()
	{
		GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] eventExchangeInfoLogicData = ClassSingleton<ExchangeWebAPI>.Instance.EventExchangeInfoLogicData;
		bool flag = eventExchangeInfoLogicData != null && eventExchangeInfoLogicData.Length > 0 && eventExchangeInfoLogicData[0] != null;
		this.exchangeResultInfoList = ((!flag) ? null : new List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result>(eventExchangeInfoLogicData));
		this.exchangeUpdateDict = new Dictionary<string, bool>();
		if (flag)
		{
			this.CheckExchangeUpdate();
			this.csSelectPanelExchangeMenu.selectParts = this.goListParts;
			this.csSelectPanelExchangeMenu.ListWindowViewRect = ConstValue.GetRectWindow3();
			this.csSelectPanelExchangeMenu.initLocation = true;
			this.exchangeMenuItemList = this.csSelectPanelExchangeMenu.AllBuild(this.exchangeResultInfoList);
		}
		else
		{
			this.csSelectPanelExchangeMenu.gameObject.SetActive(false);
			this.goScrollBar.SetActive(false);
			this.goScrollBarBg.SetActive(false);
			this.goNoneExchangeWindow.SetActive(true);
		}
		this.goListParts.SetActive(false);
	}

	private void CheckExchangeUpdate()
	{
		bool flag = false;
		for (int i = 0; i < this.exchangeResultInfoList.Count; i++)
		{
			string eventExchangeId = this.exchangeResultInfoList[i].eventExchangeId;
			string @string = PlayerPrefs.GetString(this.prefsKeyString + eventExchangeId, string.Empty);
			string updateTime = this.exchangeResultInfoList[i].updateTime;
			if (@string.StartsWith("NEW"))
			{
				this.exchangeUpdateDict.Add(eventExchangeId, true);
			}
			else if (string.IsNullOrEmpty(@string) || @string != updateTime)
			{
				this.exchangeUpdateDict.Add(eventExchangeId, true);
				PlayerPrefs.SetString(this.prefsKeyString + eventExchangeId, "NEW" + updateTime);
				flag = true;
			}
			else
			{
				this.exchangeUpdateDict.Add(eventExchangeId, false);
			}
		}
		if (flag)
		{
			PlayerPrefs.Save();
		}
	}

	public bool IsNewExchange(string id)
	{
		return this.exchangeUpdateDict[id];
	}

	public void VisitExchange(string id)
	{
		string @string = PlayerPrefs.GetString(this.prefsKeyString + id, string.Empty);
		if (@string.StartsWith("NEW"))
		{
			PlayerPrefs.SetString(this.prefsKeyString + id, @string.Replace("NEW", string.Empty));
			PlayerPrefs.Save();
		}
	}

	public void ReloadResultInfo()
	{
		GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] eventExchangeInfoLogicData = ClassSingleton<ExchangeWebAPI>.Instance.EventExchangeInfoLogicData;
		bool flag = eventExchangeInfoLogicData != null && eventExchangeInfoLogicData.Length > 0 && eventExchangeInfoLogicData[0] != null;
		this.exchangeResultInfoList = ((!flag) ? null : new List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result>(eventExchangeInfoLogicData));
		if (this.exchangeResultInfoList == null)
		{
			return;
		}
		for (int i = 0; i < this.exchangeResultInfoList.Count; i++)
		{
			int count = this.exchangeResultInfoList[i].detail[0].item.count;
			bool flag2 = false;
			foreach (GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail detail2 in this.exchangeResultInfoList[i].detail)
			{
				if (int.Parse(detail2.needNum) <= count && detail2.remainCount > 0)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				this.exchangeMenuItemList[i].OffAvailableMark();
			}
			this.exchangeMenuItemList[i].ReloadInfo(this.exchangeResultInfoList[i]);
		}
	}
}
