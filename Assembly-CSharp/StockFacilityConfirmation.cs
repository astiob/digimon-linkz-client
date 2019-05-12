using FarmData;
using System;
using System.Runtime.CompilerServices;

public sealed class StockFacilityConfirmation : FacilityConfirmation
{
	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	protected override void TaskSaveFarmObject(FarmScenery farmScenery)
	{
		int facilityID = base.GetFacilityID();
		UserFacility stockFacilityByfacilityId = Singleton<UserDataMng>.Instance.GetStockFacilityByfacilityId(facilityID);
		int userFacilityId = stockFacilityByfacilityId.userFacilityId;
		APIRequestTask apirequestTask = farmScenery.SaveStockFarmObjectPosition(userFacilityId, new Action<int>(base.OnFinishedToSave));
		TaskBase task = apirequestTask;
		if (StockFacilityConfirmation.<>f__mg$cache0 == null)
		{
			StockFacilityConfirmation.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
		}
		base.StartCoroutine(task.Run(StockFacilityConfirmation.<>f__mg$cache0, null, null));
	}

	protected override bool CheckExtendBuild()
	{
		return true;
	}

	protected override bool CanExtendBuild(int facilityId)
	{
		return Singleton<UserDataMng>.Instance.ExistInUserStockFacility(facilityId) && FarmUtility.IsExtendBuild(facilityId);
	}

	protected override void BackToUI()
	{
		GUIMain.ShowCommonDialog(null, "CMD_FacilityStock", null);
	}

	protected override void Close(bool returnHomeUI)
	{
		base.Close(returnHomeUI);
	}

	protected override void PlaySavedSE()
	{
	}
}
