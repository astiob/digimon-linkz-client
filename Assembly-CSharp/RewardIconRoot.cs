using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardIconRoot : MonoBehaviour
{
	[SerializeField]
	private List<PresentBoxItem> rewardIconList = new List<PresentBoxItem>();

	[SerializeField]
	private List<UILabel> numLabelList = new List<UILabel>();

	public string itemName { get; private set; }

	public void SetRewardList(List<GameWebAPI.RespDataWD_DungeonResult.DungeonReward> rewardList)
	{
		base.gameObject.SetActive(true);
		int num = 0;
		while (num < this.rewardIconList.Count && num < rewardList.Count)
		{
			this.rewardIconList[num].SetItem(rewardList[num].assetCategoryId, rewardList[num].assetValue.ToString(), "1", false, null);
			this.itemName += string.Format(StringMaster.GetString("BattleResult-09"), this.rewardIconList[num].itemName, rewardList[num].assetNum);
			num++;
		}
	}

	public void SetRewardList(List<GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward> rewardList, Color iconColor)
	{
		base.gameObject.SetActive(true);
		int num = 0;
		while (num < this.rewardIconList.Count && num < rewardList.Count)
		{
			this.rewardIconList[num].SetItem(rewardList[num].assetCategoryId, rewardList[num].assetValue.ToString(), "1", false, null);
			this.itemName += string.Format(StringMaster.GetString("BattleResult-09"), this.rewardIconList[num].itemName, rewardList[num].assetNum);
			this.rewardIconList[num].SetColor(iconColor);
			if (this.numLabelList != null && this.numLabelList.Count > num && this.numLabelList[num] != null && int.Parse(rewardList[num].assetNum) > 1)
			{
				this.numLabelList[num].gameObject.SetActive(true);
				this.numLabelList[num].text = string.Format(StringMaster.GetString("SystemItemCount2"), rewardList[num].assetNum);
			}
			num++;
		}
	}

	public void SetRewardList(GameWebAPI.ColosseumReward[] rewardList)
	{
		base.gameObject.SetActive(true);
		int num = 0;
		while (num < this.rewardIconList.Count && num < rewardList.Length)
		{
			this.rewardIconList[num].SetItem(rewardList[num].assetCategoryId, rewardList[num].assetValue.ToString(), "1", false, null);
			this.itemName += string.Format(StringMaster.GetString("BattleResult-09"), this.rewardIconList[num].itemName, rewardList[num].assetNum);
			num++;
		}
	}
}
