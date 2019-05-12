using FarmData;
using Master;
using Neptune.Push;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNotice : MonoBehaviour, NpPush.INpPushListener
{
	private readonly string senderID = "994857230643";

	private static PushNotice instance;

	private NpPush npPush;

	private bool isRecieveGardenPushNotice;

	private List<Action<bool, string>> neptuneCallbackList = new List<Action<bool, string>>();

	private bool isRecieveStaminaMaxPushNotice;

	private bool isRecieveEndBuildingPushNotice;

	public int staminaRecovery;

	private List<UserFacility> userFacilityList = new List<UserFacility>();

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
				this.userFacilityList = new List<UserFacility>();
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

	public void OnGetDeviceToken(string DeviceToken)
	{
		foreach (Action<bool, string> action in this.neptuneCallbackList)
		{
			action(true, DeviceToken);
		}
		this.neptuneCallbackList.Clear();
	}

	public void OnPushError(string ErrorMessage)
	{
		global::Debug.LogError("Failed GetDeviceToken() : " + ErrorMessage);
		foreach (Action<bool, string> action in this.neptuneCallbackList)
		{
			action(false, ErrorMessage);
		}
		this.neptuneCallbackList.Clear();
	}

	private void Update()
	{
		if (this.isRecieveStaminaMaxPushNotice && Singleton<UserDataMng>.Instance != null && DataMng.Instance() != null && DataMng.Instance().RespDataUS_PlayerInfo != null)
		{
			TimeSpan timeSpan = ServerDateTime.Now - Singleton<UserDataMng>.Instance.playerStaminaBaseTime;
			this.staminaRecovery = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery - (int)timeSpan.TotalSeconds;
		}
		if (this.isRecieveEndBuildingPushNotice && Singleton<UserDataMng>.Instance != null && Singleton<UserDataMng>.Instance.userFacilityList != null)
		{
			this.userFacilityList = Singleton<UserDataMng>.Instance.userFacilityList;
		}
	}

	private void OnApplicationPause(bool PauseStatus)
	{
		if (PauseStatus)
		{
			this.SetLocalPushNotice();
		}
		else
		{
			NpPush.BadgeNumberChange(0);
			this.ClearStaminaLocalPushNotice();
			this.ClearBuildedLocalPushNotice();
		}
	}

	private void OnDestroy()
	{
		this.SetLocalPushNotice();
	}

	private void Initialize()
	{
		this.npPush = new NpPush(base.gameObject, this);
		NpPush.BadgeNumberChange(0);
		this.ClearStaminaLocalPushNotice();
		this.ClearBuildedLocalPushNotice();
		this.SendDeviceTokenToServer(null);
	}

	private void SetCallback(Action<bool, string> OnRecieveDeviceToken)
	{
		this.neptuneCallbackList.Add(OnRecieveDeviceToken);
	}

	private void SendDeviceTokenToServer(Action<bool> ResultGetDeviceToken = null)
	{
		this.SetCallback(delegate(bool result, string resultString)
		{
			if (result)
			{
				if (ResultGetDeviceToken != null)
				{
					this.StartCoroutine(this.SaveDeviceTokenToServer(resultString, ResultGetDeviceToken));
				}
				else
				{
					this.StartCoroutine(this.SaveDeviceTokenToServer(resultString, null));
				}
			}
			else if (ResultGetDeviceToken != null)
			{
				ResultGetDeviceToken(false);
			}
		});
		this.npPush.GetDeviceToken(this.senderID);
	}

	private IEnumerator SaveDeviceTokenToServer(string DeviceToken, Action<bool> ResultSendDeviceToken)
	{
		GameWebAPI.Request_CM_UpdateDeviceToken request = new GameWebAPI.Request_CM_UpdateDeviceToken
		{
			SetSendData = delegate(GameWebAPI.CM_Req_UpdateDeviceToken param)
			{
				param.osType = 2;
				param.deviceToken = DeviceToken;
			}
		};
		return request.Run(delegate()
		{
			if (ResultSendDeviceToken != null)
			{
				ResultSendDeviceToken(true);
			}
		}, null, null);
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
		if (!this.isRecieveStaminaMaxPushNotice || this.staminaRecovery <= 0)
		{
			return;
		}
		string @string = StringMaster.GetString("LocalNotice-01");
		NpPush.LocalPushSendRequestCode(@string, 1, this.staminaRecovery, 0);
		this.staminaRecovery = 0;
	}

	private void ClearStaminaLocalPushNotice()
	{
		NpPush.CancelLocalNotifications(0);
	}

	private void SetBuildedLocalPushNotice()
	{
		if (!this.isRecieveEndBuildingPushNotice || this.userFacilityList.Count == 0)
		{
			return;
		}
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
		this.userFacilityList = new List<UserFacility>();
	}

	private void ClearBuildedLocalPushNotice()
	{
		NpPush.CancelLocalNotifications(1);
	}

	public void SetGardenPushNotice()
	{
		if (MasterDataMng.Instance() == null || MasterDataMng.Instance().RespDataMA_CodeM == null || MasterDataMng.Instance().RespDataMA_CodeM.codeM.SEND_ANDROID_PUSH == 0)
		{
			return;
		}
		this.ResetGardenPushNotice();
	}

	public void SyncGardenPushNoticeData()
	{
		this.gardenDataList.Clear();
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList(false);
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.GROWING_IN_GARDEN);
		if (list.Count == 0)
		{
			return;
		}
		list = MonsterDataMng.Instance().SortMDList(list, false);
		foreach (MonsterData monsterData in list)
		{
			if (!(monsterData.userMonster.eggFlg == "1"))
			{
				this.gardenDataList.Add(new PushNotice.GardenPushNoticeData(monsterData.userMonster.userMonsterId, DateTime.Parse(monsterData.userMonster.growEndDate)));
			}
		}
		this.SortGardenPushNoticeData();
	}

	private void SortGardenPushNoticeData()
	{
		if (this.gardenDataList.Count == 0)
		{
			return;
		}
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

	private void ResetGardenPushNotice()
	{
		if (!this.isRecieveGardenPushNotice)
		{
			return;
		}
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
				int num = int.Parse(gardenPushNoticeData.monsterID);
				string string2 = StringMaster.GetString("LocalNotice-02");
				NpPush.LocalPushSendRequestCode(string2, 1, (int)timeSpan.TotalSeconds, 2);
				PlayerPrefs.SetString("RESERVED_GARDEN_NOTICE", gardenPushNoticeData.monsterID);
				break;
			}
		}
	}

	public void ClearGardenPushNotice()
	{
		string @string = PlayerPrefs.GetString("RESERVED_GARDEN_NOTICE", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		PlayerPrefs.DeleteKey("RESERVED_GARDEN_NOTICE");
		NpPush.CancelLocalNotifications(2);
	}

	public enum NoticeType
	{
		StaminaMax,
		EndBuilding,
		Garden
	}

	public struct GardenPushNoticeData
	{
		public string monsterID;

		public DateTime growEndDate;

		public GardenPushNoticeData(string MonsterID, DateTime GrowEndDate)
		{
			this.monsterID = MonsterID;
			this.growEndDate = GrowEndDate;
		}
	}
}
