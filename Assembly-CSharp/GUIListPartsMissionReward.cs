using System;

public sealed class GUIListPartsMissionReward : GUIListPartBS
{
	private GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward rewardData;

	public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward Data
	{
		get
		{
			return this.rewardData;
		}
		set
		{
			this.rewardData = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.SetCommonUI();
		base.ShowGUI();
	}

	private void SetCommonUI()
	{
		RewardListItem component = base.gameObject.GetComponent<RewardListItem>();
		component.SetItem(this.rewardData.assetCategoryId, this.rewardData.assetValue, this.rewardData.assetNum, false, null);
	}
}
