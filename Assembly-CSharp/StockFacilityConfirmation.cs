using FarmData;
using System;

public sealed class StockFacilityConfirmation : FacilityConfirmation
{
	protected override void TaskSaveFarmObject(FarmScenery farmScenery)
	{
		int facilityID = base.GetFacilityID();
		UserFacility stockFacilityByfacilityId = Singleton<UserDataMng>.Instance.GetStockFacilityByfacilityId(facilityID);
		int userFacilityId = stockFacilityByfacilityId.userFacilityId;
		APIRequestTask task = farmScenery.SaveStockFarmObjectPosition(userFacilityId, new Action<int>(base.OnFinishedToSave));
		base.StartCoroutine(task.Run(new Action(RestrictionInput.EndLoad), null, null));
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
