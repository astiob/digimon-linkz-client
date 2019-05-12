using System;

namespace FarmData
{
	[Serializable]
	public sealed class FacilityM
	{
		public string facilityId;

		public string facilityName;

		public string description;

		public string type;

		public string sort;

		public string openTime;

		public string closeTime;

		public string releaseId;

		public string autoBuildingFlg;

		public string shorteningFlg;

		public string movingFlg;

		public string sellFlg;

		public string sellPrice;

		public string initialX;

		public string initialY;

		public string buildingTime;

		public string maxLevel;

		public string maxNum;

		public string modelResource;

		public string iconResource;

		public string popDescription;

		public string popDetails;

		public string isWalk;

		public string isExtend;

		public string buildingAssetCategoryId1;

		public string buildingAssetValue1;

		public string buildingAssetNum1;

		public string buildingAssetCategoryId2;

		public string buildingAssetValue2;

		public string buildingAssetNum2;

		public string buildingAssetCategoryId3;

		public string buildingAssetValue3;

		public string buildingAssetNum3;

		public string shorteningAssetCategoryId1;

		public string shorteningAssetValue1;

		public string shorteningAssetNum1;

		public string shorteningAssetCategoryId2;

		public string shorteningAssetValue2;

		public string shorteningAssetNum2;

		public string shorteningAssetCategoryId3;

		public string shorteningAssetValue3;

		public string shorteningAssetNum3;

		public string stockFlg;

		public string levelUpFlg;

		public string GetIconPath()
		{
			return string.Format("Farm/Textures/{0}", this.iconResource);
		}

		public bool IsFacility()
		{
			return "1" == this.type;
		}
	}
}
