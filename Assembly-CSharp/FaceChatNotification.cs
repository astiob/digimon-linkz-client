using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceChatNotification : GUICollider
{
	[SerializeField]
	private GameObject attensionIcon;

	private bool isFirst = true;

	public GameWebAPI.RespData_ChatLastHistoryIdList apiLastHistoryList;

	private bool isNewMockBattleRequest;

	public bool[] chatTabAlertList = new bool[4];

	private bool isLock { get; set; }

	public List<FaceChatNotification.UserPrefsHistoryIdList> prefsLastHistoryList { get; set; }

	private bool isNewMultiRequestRequest { get; set; }

	protected override void Awake()
	{
		ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification = this;
		this.OnNotificationExec(false);
		this.StartGetHistoryIdList();
		base.Awake();
	}

	protected override void OnDestroy()
	{
		this.StopGetHistoryIdList();
		base.OnDestroy();
	}

	public void StartGetHistoryIdList()
	{
		if (ChatConstValue.CHAT_NOTIFICATION_INTERVAL_TIME != "0")
		{
			this.SetLock(true);
			base.InvokeRepeating("CheckBarrier", 0f, float.Parse(ChatConstValue.CHAT_NOTIFICATION_INTERVAL_TIME));
		}
	}

	public void StopGetHistoryIdList()
	{
		base.CancelInvoke("CheckBarrier");
	}

	public void StartGetHistoryIdListSingle()
	{
		if (!this.isLock)
		{
			this.GetChatLastHistoryIdList();
		}
	}

	public void CheckBarrier()
	{
		bool activeSelf = GUIManager.GetGUI("CommonDialogBarrier").gameObject.activeSelf;
		bool flag = GUIMain.IsBarrierON();
		if ((!activeSelf && !flag) || this.isFirst)
		{
			this.isFirst = false;
			this.GetChatLastHistoryIdList();
		}
		else
		{
			this.SetLock(false);
		}
	}

	private void GetChatLastHistoryIdList()
	{
		GameWebAPI.ChatLastHistoryIdList request = new GameWebAPI.ChatLastHistoryIdList
		{
			OnReceived = new Action<GameWebAPI.RespData_ChatLastHistoryIdList>(this.AfterChatLastHistoryIdList)
		};
		if (base.gameObject.activeInHierarchy)
		{
			APIRequestTask apirequestTask = new APIRequestTask(request, false);
			apirequestTask.SetAfterBehavior(TaskBase.AfterAlertClosed.RETURN);
			Func<Exception, IEnumerator> onAlert = (Exception noop) => null;
			IEnumerator routine = apirequestTask.Run(null, null, onAlert);
			base.StartCoroutine(routine);
		}
	}

	private void AfterChatLastHistoryIdList(GameWebAPI.RespData_ChatLastHistoryIdList data)
	{
		this.CheckMultiRequestNotification(data.multiRoomRequestId);
		this.CheckPvPMockBattleNotification(data.lastMockBattleRequestTime);
		if (!this.isNewMultiRequestRequest && !this.isNewMockBattleRequest)
		{
			if (data.lastHistoryIds == null)
			{
				this.ALlDeletePrefsHistoryIds();
				this.TextPlateAnimationDefault();
			}
			else
			{
				this.apiLastHistoryList = data;
				this.LoadPrefsHistoryIds();
			}
		}
	}

	private void CheckMultiRequestNotification(string multiRoomRequestId)
	{
		if (multiRoomRequestId != null)
		{
			if (PlayerPrefs.HasKey("lastMultiReqId"))
			{
				int @int = PlayerPrefs.GetInt("lastMultiReqId");
				if (@int < int.Parse(multiRoomRequestId))
				{
					this.OnNotificationExec(true);
					this.chatTabAlertList[0] = true;
					this.isNewMultiRequestRequest = true;
				}
				else
				{
					this.chatTabAlertList[0] = false;
					this.isNewMultiRequestRequest = false;
				}
			}
			else
			{
				this.OnNotificationExec(true);
				this.chatTabAlertList[0] = true;
				this.isNewMultiRequestRequest = true;
			}
		}
		else
		{
			this.chatTabAlertList[0] = false;
			this.isNewMultiRequestRequest = false;
		}
	}

	private void CheckPvPMockBattleNotification(string lastMockBattleRequestTime)
	{
		if (lastMockBattleRequestTime != null)
		{
			if (PlayerPrefs.HasKey("lastPvPMockTime"))
			{
				int @int = PlayerPrefs.GetInt("lastPvPMockTime");
				if (@int < int.Parse(lastMockBattleRequestTime))
				{
					if (!this.isNewMultiRequestRequest)
					{
						this.OnNotificationExec(true);
					}
					this.chatTabAlertList[1] = true;
					this.isNewMockBattleRequest = true;
				}
				else
				{
					this.chatTabAlertList[1] = false;
					this.isNewMockBattleRequest = false;
				}
			}
			else
			{
				if (!this.isNewMultiRequestRequest)
				{
					this.OnNotificationExec(true);
				}
				this.chatTabAlertList[1] = true;
				this.isNewMockBattleRequest = true;
			}
		}
		else
		{
			this.chatTabAlertList[1] = false;
			this.isNewMockBattleRequest = false;
		}
	}

	private void ALlDeletePrefsHistoryIds()
	{
		if (PlayerPrefs.HasKey("lastHistoryIds"))
		{
			PlayerPrefs.DeleteKey("lastHistoryIds");
		}
	}

	private void LoadPrefsHistoryIds()
	{
		if (!PlayerPrefs.HasKey("lastHistoryIds"))
		{
			this.MakePrefsHistoryIds();
			this.OnNotificationExec(true);
		}
		else
		{
			this.prefsLastHistoryList = this.GetPrefsHistoryIdList();
			this.CompHistory();
		}
	}

	private void MakePrefsHistoryIds()
	{
		if (this.apiLastHistoryList != null)
		{
			string text = string.Empty;
			foreach (GameWebAPI.RespData_ChatLastHistoryIdList.LastHistoryIds lastHistoryIds2 in this.apiLastHistoryList.lastHistoryIds)
			{
				text += string.Format("{0}:{1},", lastHistoryIds2.chatGroupId, "0");
			}
			text = text.Trim(new char[]
			{
				','
			});
			PlayerPrefs.SetString("lastHistoryIds", text);
		}
	}

	private void CompHistory()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		bool flag = false;
		foreach (GameWebAPI.RespData_ChatLastHistoryIdList.LastHistoryIds dt in this.apiLastHistoryList.lastHistoryIds)
		{
			text2 += string.Format("{0}:{1},", dt.chatGroupId, dt.chatMessageHistoryId);
			var <>__AnonType = this.prefsLastHistoryList.Where((FaceChatNotification.UserPrefsHistoryIdList item) => item.historyData.chatGroupId == dt.chatGroupId).Select((FaceChatNotification.UserPrefsHistoryIdList item) => new
			{
				item.historyData.chatGroupId,
				item.historyData.chatMessageHistoryId
			}).SingleOrDefault();
			if (<>__AnonType == null)
			{
				text += string.Format("{0}:{1},", dt.chatGroupId, "0");
				if (dt.chatMessageHistoryId != null)
				{
					flag = true;
				}
			}
			else
			{
				text += string.Format("{0}:{1},", dt.chatGroupId, <>__AnonType.chatMessageHistoryId);
				if (dt.chatMessageHistoryId != <>__AnonType.chatMessageHistoryId && dt.chatMessageHistoryId != null)
				{
					flag = true;
				}
			}
		}
		text = text.Trim(new char[]
		{
			','
		});
		text2 = text2.Trim(new char[]
		{
			','
		});
		PlayerPrefs.SetString("lastHistoryIds", text);
		if (flag)
		{
			this.OnNotificationExec(true);
		}
		else
		{
			this.OnNotificationExec(false);
		}
	}

	private void OnNotificationExec(bool active = false)
	{
		this.DispControlNewIcon(active);
		this.SetLock(false);
	}

	public void OnClickedNotificationIcon()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		List<string> worldIdList = new List<string>(ChatConstValue.WORLD_ID_LIST);
		ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(worldIdList, delegate(bool i)
		{
			CMD_ChatTop cmd_ChatTop = GUIMain.ShowCommonDialog(null, "CMD_ChatTop", null) as CMD_ChatTop;
			if (this.isNewMultiRequestRequest)
			{
				cmd_ChatTop.ForceSelectTab(CMD_ChatTop.TabType.MULTI);
			}
			else if (this.isNewMockBattleRequest)
			{
				cmd_ChatTop.ForceSelectTab(CMD_ChatTop.TabType.PVP);
			}
		});
	}

	private void DispControlNewIcon(bool flg = false)
	{
		this.attensionIcon.SetActive(flg);
	}

	private void TextPlateAnimationDefault()
	{
		this.DispControlNewIcon(false);
	}

	private void SetLock(bool flg = false)
	{
		this.isLock = flg;
	}

	public List<FaceChatNotification.UserPrefsHistoryIdList> GetPrefsHistoryIdList()
	{
		List<FaceChatNotification.UserPrefsHistoryIdList> list = new List<FaceChatNotification.UserPrefsHistoryIdList>();
		if (PlayerPrefs.HasKey("lastHistoryIds"))
		{
			string[] array = PlayerPrefs.GetString("lastHistoryIds").Split(new char[]
			{
				','
			});
			foreach (string text in array)
			{
				string[] array3 = text.Split(new char[]
				{
					':'
				});
				list.Add(new FaceChatNotification.UserPrefsHistoryIdList
				{
					historyData = new FaceChatNotification.UserPrefsHistoryIdList.HistoryData(),
					historyData = 
					{
						chatGroupId = array3[0],
						chatMessageHistoryId = array3[1]
					}
				});
			}
		}
		return list;
	}

	public class UserPrefsHistoryIdList
	{
		public FaceChatNotification.UserPrefsHistoryIdList.HistoryData historyData;

		public class HistoryData
		{
			public string chatGroupId;

			public string chatMessageHistoryId;
		}
	}
}
