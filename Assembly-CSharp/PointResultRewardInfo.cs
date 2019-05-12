using System;
using UnityEngine;

public class PointResultRewardInfo : MonoBehaviour
{
	[SerializeField]
	[Header("報酬アイテム")]
	private PresentBoxItem[] rewards;

	public void SetDetail(int rewardNo, string assetCategoryId, string assetValue)
	{
		this.rewards[rewardNo].SetItem(assetCategoryId, assetValue, "1", false, null);
	}
}
