using FarmData;
using System;
using System.Collections;
using System.Collections.Generic;
using TutorialRequestHeader;
using UnityEngine;

public sealed class TutorialFirstFinishRequest
{
	private IEnumerator RequestUserFacility()
	{
		List<UserFacility> userFacilityList = new List<UserFacility>();
		GameWebAPI.RequestFA_UserFacilityList requestFA_UserFacilityList = new GameWebAPI.RequestFA_UserFacilityList();
		requestFA_UserFacilityList.SetSendData = delegate(GameWebAPI.FA_Req_RequestFA_UserFacilityList param)
		{
			param.userId = DataMng.Instance().RespDataCM_Login.playerInfo.UserId;
		};
		requestFA_UserFacilityList.OnReceived = delegate(GameWebAPI.RespDataFA_GetFacilityList response)
		{
			for (int i = 0; i < response.userFacilityList.Length; i++)
			{
				UserFacility userFacility = response.userFacilityList[i];
				if (userFacility.facilityId == 6)
				{
					userFacilityList.Add(userFacility);
				}
			}
		};
		GameWebAPI.RequestFA_UserFacilityList request = requestFA_UserFacilityList;
		yield return AppCoroutine.Start(request.Run(null, null, null), false);
		Singleton<UserDataMng>.Instance.userFacilityList = userFacilityList;
		yield break;
	}

	private IEnumerator RequestBuildMeatFarm(Action<int> completed)
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(1);
		int userFacilityId = 0;
		RequestFA_FacilityBuild request = new RequestFA_FacilityBuild
		{
			SetSendData = delegate(FacilityBuild param)
			{
				param.facilityId = 1;
				param.positionX = facilityMaster.initialX.ToInt32();
				param.positionY = facilityMaster.initialX.ToInt32();
			},
			OnReceived = delegate(FacilityBuildResult response)
			{
				userFacilityId = response.userFacilityId;
			}
		};
		yield return AppCoroutine.Start(request.Run(null, null, null), false);
		if (completed != null)
		{
			completed(userFacilityId);
		}
		yield break;
	}

	private IEnumerator RequestBuildCompletedMeatFarm(int meatFarmUserFacilityId)
	{
		FacilityBuildCompleteResult.resultCode resultCode = FacilityBuildCompleteResult.resultCode.FAILED;
		while (resultCode == FacilityBuildCompleteResult.resultCode.FAILED)
		{
			RequestFA_FacilityBuildComplete request = new RequestFA_FacilityBuildComplete
			{
				SetSendData = delegate(FacilityBuildComplete param)
				{
					param.userFacilityId = meatFarmUserFacilityId;
				},
				OnReceived = delegate(FacilityBuildCompleteResult param)
				{
					resultCode = (FacilityBuildCompleteResult.resultCode)param.result;
				}
			};
			bool isClose = false;
			yield return AppCoroutine.Start(request.Run(delegate()
			{
				if (resultCode == FacilityBuildCompleteResult.resultCode.FAILED)
				{
					string errorCode = AlertManager.GetErrorCode(WWWResponse.LocalErrorStatus.LOCAL_ERROR_WWW);
					AlertManager.ShowAlertDialog(delegate(int nop)
					{
						isClose = true;
					}, errorCode);
				}
				else
				{
					isClose = true;
				}
			}, null, null), false);
			while (!isClose)
			{
				yield return null;
			}
		}
		yield break;
	}

	public IEnumerator RequestFirstTutorialFinish()
	{
		yield return AppCoroutine.Start(this.RequestUserFacility(), false);
		int meatFarmUserFacilityId = -1;
		for (int i = 0; i < Singleton<UserDataMng>.Instance.userFacilityList.Count; i++)
		{
			UserFacility facility = Singleton<UserDataMng>.Instance.userFacilityList[i];
			if (facility.facilityId == 1)
			{
				meatFarmUserFacilityId = facility.userFacilityId;
				break;
			}
		}
		if (meatFarmUserFacilityId == -1)
		{
			yield return AppCoroutine.Start(this.RequestBuildMeatFarm(delegate(int id)
			{
				meatFarmUserFacilityId = id;
			}), false);
		}
		yield return new WaitForSeconds(1f);
		yield return AppCoroutine.Start(this.RequestBuildCompletedMeatFarm(meatFarmUserFacilityId), false);
		TutorialEnd request = new TutorialEnd();
		yield return AppCoroutine.Start(request.Run(null, null, null), false);
		yield break;
	}
}
