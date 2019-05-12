using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelDropItemList : GUISelectPanelViewPartsUD
{
	public static List<GUIListDropItemParts.Data> partsDataList;

	public void AllBuild(int widthLength, Vector2 windowSize, GameWebAPI.RespDataWD_DungeonStart.Drop[] standardDrops, GameWebAPI.RespDataWD_DungeonStart.LuckDrop luckDrop, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] ownerMultiDrops, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] multiDrops, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LuckDrop[] multiLuckDrops, GameWebAPI.RespDataWD_DungeonResult.OptionDrop[] optionDrops)
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
		int num8 = (optionDrops == null) ? 0 : optionDrops.Length;
		int num9 = num3 + num4 + num5 + num6 + num7 + num8;
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
		if (num4 > 0)
		{
			GUIListDropItemParts.Data data2 = new GUIListDropItemParts.Data();
			data2.assetCategoryId = (MasterDataMng.AssetCategory)luckDrop.assetCategoryId.ToInt32();
			data2.assetNum = luckDrop.assetNum;
			data2.assetValue = luckDrop.assetValue;
			data2.dropBoxType = (GUIListDropItemParts.BoxType)luckDrop.dropBoxType;
			data2.dropType = GUIListDropItemParts.DropType.Luck;
			GUISelectPanelDropItemList.partsDataList.Add(data2);
		}
		for (int j = 0; j < num8; j++)
		{
			GUIListDropItemParts.Data data3 = new GUIListDropItemParts.Data();
			data3.assetCategoryId = (MasterDataMng.AssetCategory)optionDrops[j].assetCategoryId.ToInt32();
			data3.assetNum = optionDrops[j].assetNum.ToInt32();
			data3.assetValue = optionDrops[j].assetValue.ToInt32();
			data3.dropBoxType = (GUIListDropItemParts.BoxType)optionDrops[j].subType.ToInt32();
			data3.dropType = GUIListDropItemParts.DropType.Challenge;
			GUISelectPanelDropItemList.partsDataList.Add(data3);
		}
		for (int k = 0; k < num5; k++)
		{
			GUIListDropItemParts.Data data4 = new GUIListDropItemParts.Data();
			data4.assetCategoryId = (MasterDataMng.AssetCategory)ownerMultiDrops[k].assetCategoryId.ToInt32();
			data4.assetNum = ownerMultiDrops[k].assetNum;
			data4.assetValue = ownerMultiDrops[k].assetValue;
			data4.dropBoxType = (GUIListDropItemParts.BoxType)ownerMultiDrops[k].dropBoxType;
			data4.dropType = GUIListDropItemParts.DropType.Owner;
			GUISelectPanelDropItemList.partsDataList.Add(data4);
		}
		for (int l = 0; l < num6; l++)
		{
			GUIListDropItemParts.Data data5 = new GUIListDropItemParts.Data();
			data5.assetCategoryId = (MasterDataMng.AssetCategory)multiDrops[l].assetCategoryId.ToInt32();
			data5.assetNum = multiDrops[l].assetNum;
			data5.assetValue = multiDrops[l].assetValue;
			data5.dropBoxType = (GUIListDropItemParts.BoxType)multiDrops[l].dropBoxType;
			data5.dropType = GUIListDropItemParts.DropType.Multi;
			GUISelectPanelDropItemList.partsDataList.Add(data5);
		}
		for (int m = 0; m < num7; m++)
		{
			GUIListDropItemParts.Data data6 = new GUIListDropItemParts.Data();
			data6.assetCategoryId = (MasterDataMng.AssetCategory)multiLuckDrops[m].assetCategoryId.ToInt32();
			data6.assetNum = multiLuckDrops[m].assetNum;
			data6.assetValue = multiLuckDrops[m].assetValue;
			data6.dropBoxType = (GUIListDropItemParts.BoxType)multiLuckDrops[m].dropBoxType;
			data6.dropType = GUIListDropItemParts.DropType.LuckMulti;
			data6.multiLuckDropUserId = multiLuckDrops[m].userId;
			GUISelectPanelDropItemList.partsDataList.Add(data6);
		}
		for (int n = 0; n < num9; n++)
		{
			GUISelectPanelDropItemList.partsDataList[n].index = n;
		}
		base.initLocation = true;
		base.AllBuild(num9, true, 1f, 1f, null, null);
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
				if (!isSkip && !isEndMultiDrop && GUISelectPanelDropItemList.partsDataList[i].dropType != GUIListDropItemParts.DropType.Standard && GUISelectPanelDropItemList.partsDataList[i].dropType != GUIListDropItemParts.DropType.Luck && GUISelectPanelDropItemList.partsDataList[i].dropType != GUIListDropItemParts.DropType.Challenge)
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
