using Master;
using System;
using UnityEngine;

public sealed class GUIPlayerStatus : MonoBehaviour
{
	private static GUIPlayerStatus instance;

	[SerializeField]
	private UILabel userName;

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

	private void Awake()
	{
		GUIPlayerStatus.instance = this;
		Singleton<GUIManager>.Instance.ActCallBackCloseAllCMD += GUIPlayerStatus.RefreshParamsForce_S;
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
		Singleton<GUIManager>.Instance.ActCallBackCloseAllCMD -= GUIPlayerStatus.RefreshParamsForce_S;
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
		GUIMain.ShowCommonDialog(null, "CMD_Shop");
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			farmRoot.ClearSettingFarmObject();
		}
	}
}
