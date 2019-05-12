using System;
using UnityEngine;

public class PointResultRewardInfo : MonoBehaviour
{
	[Header("報酬アイテム")]
	[SerializeField]
	private PresentBoxItem[] rewards;

	public void SetDetail(int rewardNo, string assetCategoryId, string assetValue)
	{
		this.rewards[rewardNo].SetItem(assetCategoryId, assetValue, "1", false, null);
	}
}
