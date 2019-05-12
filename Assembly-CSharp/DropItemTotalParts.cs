using System;
using UnityEngine;

public class DropItemTotalParts : MonoBehaviour
{
	[SerializeField]
	[Header("ドロップアイテム")]
	private PresentBoxItem dropItemItems;

	[SerializeField]
	[Header("ドロップ数のラベル")]
	private UILabel dropNumLabel;

	public void SetData(DropItemTotalParts.Data data)
	{
		this.dropItemItems.SetItem(data.assetCategoryId, data.objectId, "1", false, null);
		this.dropNumLabel.text = "× " + data.num;
	}

	public class Data
	{
		public string assetCategoryId = string.Empty;

		public string objectId = string.Empty;

		public int num;
	}
}
