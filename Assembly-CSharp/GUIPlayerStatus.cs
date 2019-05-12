using Master;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class GUIPlayerStatus : MonoBehaviour
{
	private static GUIPlayerStatus instance;

	[SerializeField]
	private UILabel userName;

	[SerializeField]
	private UITexture titleIcon;

	[SerializeField]
	private UILabel userDigiStone;

	[SerializeField]
	private UILabel userCluster;

	[SerializeField]
	private UILabel userMeat;

	[SerializeField]
	private GUICollider stoneShopButton;

	[SerializeField]
	private PrefabFolder userStaminaPrefabFolder;

	private bool isUpdateParams;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	private void Awake()
	{
		GUIPlayerStatus.instance = this;
		GUIManager guimanager = Singleton<GUIManager>.Instance;
		if (GUIPlayerStatus.<>f__mg$cache0 == null)
		{
			GUIPlayerStatus.<>f__mg$cache0 = new Action(GUIPlayerStatus.RefreshParamsForce_S);
		}
		guimanager.ActCallBackCloseAllCMD += GUIPlayerStatus.<>f__mg$cache0;
	}

	private void Update()
	{
		if (this.isUpdateParams)
		{
			this.SetParams();
			this.isUpdateParams = false;
		}
	}

	private void OnDestroy()
	{
		GUIPlayerStatus.instance = null;
		GUIManager guimanager = Singleton<GUIManager>.Instance;
		if (GUIPlayerStatus.<>f__mg$cache1 == null)
		{
			GUIPlayerStatus.<>f__mg$cache1 = new Action(GUIPlayerStatus.RefreshParamsForce_S);
		}
		guimanager.ActCallBackCloseAllCMD -= GUIPlayerStatus.<>f__mg$cache1;
	}

	public static void RefreshParams_S(bool isForce = false)
	{
		if (GUIPlayerStatus.instance != null)
		{
			GUIPlayerStatus.instance.RefreshParams(isForce);
		}
	}

	public static void RefreshParamsForce_S()
	{
		if (GUIPlayerStatus.instance != null)
		{
			GUIPlayerStatus.instance.RefreshParams(true);
		}
	}

	private void RefreshParams(bool isForce = false)
	{
		if (!isForce)
		{
			this.isUpdateParams = true;
		}
		else
		{
			this.SetParams();
		}
	}

	private void SetParams()
	{
		if (DataMng.Instance().RespDataUS_PlayerInfo == null || DataMng.Instance().RespDataUS_PlayerInfo.playerInfo == null)
		{
			return;
		}
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		this.userName.text = playerInfo.nickname;
		TitleDataMng.SetTitleIcon(playerInfo.titleId, this.titleIcon);
		UserStamina componentInChildren = this.userStaminaPrefabFolder.Target.GetComponentInChildren<UserStamina>();
		if (null != componentInChildren)
		{
			componentInChildren.RefreshParams();
		}
		this.userDigiStone.text = playerInfo.point.ToString();
		this.userCluster.text = StringFormat.Cluster(playerInfo.gamemoney);
		this.userMeat.text = string.Format(StringMaster.GetString("SystemFraction"), playerInfo.meatNum, playerInfo.meatLimitMax);
		this.stoneShopButton.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
		{
			if (flag)
			{
				this.OpenStoneShop();
			}
		};
	}

	private void OpenStoneShop()
	{
		GUIMain.ShowCommonDialog(null, "CMD_Shop", null);
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			farmRoot.ClearSettingFarmObject();
		}
	}
}
