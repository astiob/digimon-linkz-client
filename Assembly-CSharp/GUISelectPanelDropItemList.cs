using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelDropItemList : GUISelectPanelViewPartsUD
{
	public static List<GUIListDropItemParts.Data> partsDataList;

	public void AllBuild(int widthLength, Vector2 windowSize, GameWebAPI.RespDataWD_DungeonStart.Drop[] standardDrops, GameWebAPI.RespDataWD_DungeonStart.LuckDrop luckDrop, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] ownerMultiDrops, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] multiDrops, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LuckDrop[] multiLuckDrops)
	{
		Rect listWindowViewRect = default(Rect);
		float num = windowSize.x * 0.5f;
		float num2 = windowSize.y * 0.5f;
		listWindowViewRect.xMin = -num;
		listWindowViewRect.xMax = num;
		listWindowViewRect.yMin = -num2;
		listWindowViewRect.yMax = num2;
		base.ListWindowViewRect = listWindowViewRect;
		this.fRecycleViewMaxY = num2 * 1.5f;
		this.fRecycleViewMinY = -num2 * 1.5f;
		this.PARTS_CT_MN = widthLength;
		this.RecycleViewSectorSize = 4;
		int num3 = (standardDrops == null) ? 0 : standardDrops.Length;
		int num4 = (luckDrop == null) ? 0 : 1;
		int num5 = (ownerMultiDrops == null) ? 0 : ownerMultiDrops.Length;
		int num6 = (multiDrops == null) ? 0 : multiDrops.Length;
		int num7 = (multiLuckDrops == null) ? 0 : multiLuckDrops.Length;
		int num8 = num3 + num4 + num5 + num6 + num7;
		GUISelectPanelDropItemList.partsDataList = new List<GUIListDropItemParts.Data>();
		for (int i = 0; i < num3; i++)
		{
			GUIListDropItemParts.Data data = new GUIListDropItemParts.Data();
			data.assetCategoryId = (MasterDataMng.AssetCategory)standardDrops[i].assetCategoryId.ToInt32();
			data.assetNum = standardDrops[i].assetNum;
			data.assetValue = standardDrops[i].assetValue;
			data.dropBoxType = (GUIListDropItemParts.BoxType)standardDrops[i].dropBoxType;
			data.dropType = GUIListDropItemParts.DropType.Standard;
			GUISelectPanelDropItemList.partsDataList.Add(data);
		}
		for (int j = 0; j < num5; j++)
		{
			GUIListDropItemParts.Data data2 = new GUIListDropItemParts.Data();
			data2.assetCategoryId = (MasterDataMng.AssetCategory)ownerMultiDrops[j].assetCategoryId.ToInt32();
			data2.assetNum = ownerMultiDrops[j].assetNum;
			data2.assetValue = ownerMultiDrops[j].assetValue;
			data2.dropBoxType = (GUIListDropItemParts.BoxType)ownerMultiDrops[j].dropBoxType;
			data2.dropType = GUIListDropItemParts.DropType.Owner;
			GUISelectPanelDropItemList.partsDataList.Add(data2);
		}
		for (int k = 0; k < num6; k++)
		{
			GUIListDropItemParts.Data data3 = new GUIListDropItemParts.Data();
			data3.assetCategoryId = (MasterDataMng.AssetCategory)multiDrops[k].assetCategoryId.ToInt32();
			data3.assetNum = multiDrops[k].assetNum;
			data3.assetValue = multiDrops[k].assetValue;
			data3.dropBoxType = (GUIListDropItemParts.BoxType)multiDrops[k].dropBoxType;
			data3.dropType = GUIListDropItemParts.DropType.Multi;
			GUISelectPanelDropItemList.partsDataList.Add(data3);
		}
		if (num4 > 0)
		{
			GUIListDropItemParts.Data data4 = new GUIListDropItemParts.Data();
			data4.assetCategoryId = (MasterDataMng.AssetCategory)luckDrop.assetCategoryId.ToInt32();
			data4.assetNum = luckDrop.assetNum;
			data4.assetValue = luckDrop.assetValue;
			data4.dropBoxType = (GUIListDropItemParts.BoxType)luckDrop.dropBoxType;
			data4.dropType = GUIListDropItemParts.DropType.Luck;
			GUISelectPanelDropItemList.partsDataList.Add(data4);
		}
		for (int l = 0; l < num7; l++)
		{
			GUIListDropItemParts.Data data5 = new GUIListDropItemParts.Data();
			data5.assetCategoryId = (MasterDataMng.AssetCategory)multiLuckDrops[l].assetCategoryId.ToInt32();
			data5.assetNum = multiLuckDrops[l].assetNum;
			data5.assetValue = multiLuckDrops[l].assetValue;
			data5.dropBoxType = (GUIListDropItemParts.BoxType)multiLuckDrops[l].dropBoxType;
			data5.dropType = GUIListDropItemParts.DropType.LuckMulti;
			data5.multiLuckDropUserId = multiLuckDrops[l].userId;
			GUISelectPanelDropItemList.partsDataList.Add(data5);
		}
		for (int m = 0; m < num8; m++)
		{
			GUISelectPanelDropItemList.partsDataList[m].index = m;
		}
		base.initLocation = true;
		base.AllBuild(num8, true, 1f, 1f, null, null);
		if (base.scrollBar.activeInHierarchy)
		{
			this.boxCollider.size = new Vector3(this.boxCollider.size.x, this.boxCollider.size.y, 40f);
		}
	}

	public IEnumerator SetDrops(bool isSkip = false, Action callBack = null)
	{
		this.boxCollider.enabled = false;
		List<GUIListDropItemParts> list = new List<GUIListDropItemParts>();
		bool isEndMultiDrop = false;
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIListDropItemParts parts = this.partObjs[i].csParts.GetComponent<GUIListDropItemParts>();
			IEnumerator setDrops = parts.SetDrops(isSkip, null);
			while (setDrops.MoveNext())
			{
				if (!isSkip && !isEndMultiDrop && GUISelectPanelDropItemList.partsDataList[i].dropType != GUIListDropItemParts.DropType.Standard && GUISelectPanelDropItemList.partsDataList[i].dropType != GUIListDropItemParts.DropType.Luck)
				{
					isEndMultiDrop = true;
					this.PlayMultiClearRewardAnimation();
					yield return new WaitForSeconds(0.5f);
				}
				if (!isSkip)
				{
					yield return null;
				}
			}
			parts.SetupDropIcon();
			list.Add(parts);
			bool isMaxShow = list.Count >= 20;
			bool isEndPage = i == this.partObjs.Count - 1;
			if (isMaxShow || isEndPage)
			{
				if (!isSkip)
				{
					yield return new WaitForSeconds(0.5f);
				}
				int endDrawDropIconCount = 0;
				foreach (GUIListDropItemParts temp in list)
				{
					temp.DrawDropIcon(isSkip, delegate
					{
						endDrawDropIconCount++;
					});
				}
				while (list.Count != endDrawDropIconCount)
				{
					if (!isSkip)
					{
						yield return null;
					}
				}
				if (isMaxShow && !isEndPage)
				{
					int index = (i + 1) / 10;
					base.InitMinMaxLocation(index, 0f);
					this.Update();
				}
				list.Clear();
			}
		}
		if (callBack != null)
		{
			callBack();
		}
		this.boxCollider.enabled = true;
		yield break;
	}

	private void PlayMultiClearRewardAnimation()
	{
		string path = "UICommon/BattleResult/MultiClearReward";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Transform transform = gameObject.transform;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		Animation component = gameObject.GetComponent<Animation>();
		component["MultiClearReward"].time = 0f;
		component.Play("MultiClearReward");
		gameObject.AddComponent<DepthController>();
	}
}
