using FarmData;
using Master;
using Neptune.Push;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PushNotice : MonoBehaviour, NpPush.INpPushListener
{
	private const string SENDER_ID = "994857230643";

	private static PushNotice instance;

	private NpPush npPush;

	private bool isRecieveGardenPushNotice;

	private bool isRecieveStaminaMaxPushNotice;

	private bool isRecieveEndBuildingPushNotice;

	public int staminaRecovery;

	private List<UserFacility> userFacilityList;

	private List<PushNotice.GardenPushNoticeData> gardenDataList = new List<PushNotice.GardenPushNoticeData>();

	private PushNotice()
	{
	}

	public static PushNotice Instance
	{
		get
		{
			if (PushNotice.instance == null)
			{
				GameObject gameObject = new GameObject("PushNotice");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				PushNotice.instance = gameObject.AddComponent<PushNotice>();
				PushNotice.instance.Initialize();
			}
			return PushNotice.instance;
		}
	}

	public bool IsRecieveStaminaMaxPushNotice
	{
		set
		{
			this.isRecieveStaminaMaxPushNotice = value;
			if (!value)
			{
				this.staminaRecovery = 0;
			}
		}
	}

	public bool IsRecieveEndBuildingPushNotice
	{
		set
		{
			this.isRecieveEndBuildingPushNotice = value;
			if (!value)
			{
				this.userFacilityList = null;
			}
		}
	}

	public bool IsRecieveGardenPushNotice
	{
		set
		{
			this.isRecieveGardenPushNotice = value;
			if (!value)
			{
				this.ClearGardenPushNotice();
			}
			else
			{
				this.SetGardenPushNotice();
			}
		}
	}

	public void OnGetDeviceToken(string deviceToken)
	{
		base.StartCoroutine(this.SaveDeviceTokenToServer(deviceToken));
	}

	public void OnPushError(string errorMessage)
	{
		global::Debug.LogError("Failed GetDeviceToken() : " + errorMessage);
	}

	private void Update()
	{
		if (Singleton<UserDataMng>.Instance != null)
		{
			if (this.isRecieveStaminaMaxPushNotice && DataMng.Instance() != null && DataMng.Instance().RespDataUS_PlayerInfo != null)
			{
				TimeSpan timeSpan = ServerDateTime.Now - Singleton<UserDataMng>.Instance.playerStaminaBaseTime;
				this.staminaRecovery = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery - (int)timeSpan.TotalSeconds;
			}
			if (this.isRecieveEndBuildingPushNotice && Singleton<UserDataMng>.Instance.userFacilityList != null)
			{
				this.userFacilityList = Singleton<UserDataMng>.Instance.userFacilityList;
			}
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			this.SetLocalPushNotice();
		}
		else
		{
			this.DeleteLocalPushNotice();
		}
	}

	private void OnDestroy()
	{
		this.SetLocalPushNotice();
	}

	private void DeleteLocalPushNotice()
	{
		NpPush.BadgeNumberChange(0);
		this.ClearStaminaLocalPushNotice();
		this.ClearBuildedLocalPushNotice();
	}

	private void Initialize()
	{
		this.npPush = new NpPush(base.gameObject, this);
		this.DeleteLocalPushNotice();
		this.npPush.GetDeviceToken("994857230643");
	}

	private IEnumerator SaveDeviceTokenToServer(string deviceToken)
	{
		GameWebAPI.Request_CM_UpdateDeviceToken request = new GameWebAPI.Request_CM_UpdateDeviceToken
		{
			SetSendData = delegate(GameWebAPI.CM_Req_UpdateDeviceToken param)
			{
				param.osType = 2;
				param.deviceToken = deviceToken;
			}
		};
		return request.Run(new Action(this.OnEndSaveDeviceToken), null, null);
	}

	private void OnEndSaveDeviceToken()
	{
	}

	private void SetLocalPushNotice()
	{
		if (MasterDataMng.Instance() == null || MasterDataMng.Instance().RespDataMA_CodeM == null || MasterDataMng.Instance().RespDataMA_CodeM.codeM.SEND_ANDROID_PUSH == 0)
		{
			return;
		}
		this.SetStaminaLocalPushNotice();
		this.SetBuildedLocalPushNotice();
		this.ResetGardenPushNotice();
	}

	private void SetStaminaLocalPushNotice()
	{
		if (this.isRecieveStaminaMaxPushNotice && 0 < this.staminaRecovery)
		{
			string @string = StringMaster.GetString("LocalNotice-01");
			NpPush.LocalPushSendRequestCode(@string, 1, this.staminaRecovery, 0);
			this.staminaRecovery = 0;
		}
	}

	private void ClearStaminaLocalPushNotice()
	{
		NpPush.CancelLocalNotifications(0);
	}

	private void SetBuildedLocalPushNotice()
	{
		if (this.isRecieveEndBuildingPushNotice && this.userFacilityList != null && 0 < this.userFacilityList.Count)
		{
			List<TimeSpan> list = new List<TimeSpan>();
			foreach (UserFacility userFacility in this.userFacilityList)
			{
				if (!string.IsNullOrEmpty(userFacility.completeTime))
				{
					DateTime d = DateTime.Parse(userFacility.completeTime);
					TimeSpan item = d - ServerDateTime.Now;
					if (item.TotalSeconds > 0.0)
					{
						list.Add(item);
					}
				}
			}
			if (list.Count > 0)
			{
				if (list.Count > 1)
				{
					list.Sort((TimeSpan a, TimeSpan b) => (int)(a.TotalSeconds - b.TotalSeconds));
				}
				string @string = StringMaster.GetString("LocalNotice-03");
				NpPush.LocalPushSendRequestCode(@string, 1, (int)list[0].TotalSeconds, 1);
			}
			this.userFacilityList = null;
		}
	}

	private void ClearBuildedLocalPushNotice()
	{
		NpPush.CancelLocalNotifications(1);
	}

	private void ResetGardenPushNotice()
	{
		if (this.isRecieveGardenPushNotice)
		{
			string @string = PlayerPrefs.GetString("RESERVED_GARDEN_NOTICE", string.Empty);
			if (string.IsNullOrEmpty(@string))
			{
				if (this.gardenDataList.Count == 0)
				{
					return;
				}
			}
			else
			{
				if (this.gardenDataList.Count == 0)
				{
					PlayerPrefs.DeleteKey("RESERVED_GARDEN_NOTICE");
					return;
				}
				if (@string != this.gardenDataList[0].monsterID)
				{
					PlayerPrefs.DeleteKey("RESERVED_GARDEN_NOTICE");
				}
				else
				{
					if ((this.gardenDataList[0].growEndDate - ServerDateTime.Now).TotalSeconds > 0.0)
					{
						return;
					}
					PlayerPrefs.DeleteKey("RESERVED_GARDEN_NOTICE");
				}
			}
			foreach (PushNotice.GardenPushNoticeData gardenPushNoticeData in this.gardenDataList)
			{
				TimeSpan timeSpan = gardenPushNoticeData.growEndDate - ServerDateTime.Now;
				if (timeSpan.TotalSeconds > 0.0)
				{
					string string2 = StringMaster.GetString("LocalNotice-02");
					NpPush.LocalPushSendRequestCode(string2, 1, (int)timeSpan.TotalSeconds, 2);
					PlayerPrefs.SetString("RESERVED_GARDEN_NOTICE", gardenPushNoticeData.monsterID);
					break;
				}
			}
		}
	}

	private void SortGardenPushNoticeData()
	{
		if (0 < this.gardenDataList.Count)
		{
			this.gardenDataList.Sort(delegate(PushNotice.GardenPushNoticeData a, PushNotice.GardenPushNoticeData b)
			{
				double totalSeconds = (a.growEndDate - b.growEndDate).TotalSeconds;
				if (totalSeconds < 0.0)
				{
					return -1;
				}
				if (totalSeconds > 0.0)
				{
					return 1;
				}
				int num = int.Parse(a.monsterID);
				int num2 = int.Parse(b.monsterID);
				return num - num2;
			});
		}
	}

	public void SetGardenPushNotice()
	{
		if (MasterDataMng.Instance() == null || MasterDataMng.Instance().RespDataMA_CodeM == null || MasterDataMng.Instance().RespDataMA_CodeM.codeM.SEND_ANDROID_PUSH == 0)
		{
			return;
		}
		this.ResetGardenPushNotice();
	}

	public void ClearGardenPushNotice()
	{
		string @string = PlayerPrefs.GetString("RESERVED_GARDEN_NOTICE", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			PlayerPrefs.DeleteKey("RESERVED_GARDEN_NOTICE");
			NpPush.CancelLocalNotifications(2);
		}
	}

	public void SyncGardenPushNoticeData(List<MonsterData> mds)
	{
		this.gardenDataList.Clear();
		if (0 < mds.Count)
		{
			foreach (MonsterData monsterData in mds)
			{
				if (!monsterData.userMonster.IsEgg())
				{
					PushNotice.GardenPushNoticeData item = new PushNotice.GardenPushNoticeData(monsterData.userMonster.userMonsterId, DateTime.Parse(monsterData.userMonster.growEndDate));
					this.gardenDataList.Add(item);
				}
			}
			this.SortGardenPushNoticeData();
		}
	}

	private enum NoticeType
	{
		STAMINA_MAX,
		END_BUILDING,
		GARDEN_EVOLUVE
	}

	private struct GardenPushNoticeData
	{
		public string monsterID;

		public DateTime growEndDate;

		public GardenPushNoticeData(string monsterID, DateTime growEndDate)
		{
			this.monsterID = monsterID;
			this.growEndDate = growEndDate;
		}
	}
}
