using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropItemResult : ResultBase
{
	public const int MAX_SCROLL_WIDTH = 10;

	public const int MAX_SCROLL_HEIGHT = 2;

	[Header("Winのロゴ")]
	[SerializeField]
	private GameObject winLogo;

	[Header("スキップ用Winのロゴ")]
	[SerializeField]
	private GameObject winLogoForSkip;

	[Header("エリア名とステージ名が入ってるGameObject")]
	[SerializeField]
	private GameObject titleGO;

	[Header("エリア名")]
	[SerializeField]
	private UILabel areaName;

	[Header("ステージ名")]
	[SerializeField]
	private UILabel stageName;

	[Header("ライン達")]
	[SerializeField]
	private GameObject[] lines;

	[Header("クリッピングテクスチャ")]
	[SerializeField]
	private UITexture[] clipingTextures;

	private DropItemResult.SkipCount skipCount;

	private Coroutine coroutineDropInfo;

	private DropItemList dropItemList;

	private DropItemTotalList dropItemTotalList;

	public bool isUserMonsterRefresh { get; private set; }

	public bool isChipMonsterRefresh { get; private set; }

	public override void Init()
	{
		base.Init();
		NGUITools.SetActiveSelf(this.winLogo, false);
		NGUITools.SetActiveSelf(this.winLogoForSkip, false);
		NGUITools.SetActiveSelf(this.titleGO, false);
		if (this.lines != null)
		{
			foreach (GameObject gameObject in this.lines)
			{
				gameObject.SetActive(false);
			}
		}
		string worldDungeonId = string.Empty;
		GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
		if (respDataWD_DungeonStart != null)
		{
			worldDungeonId = respDataWD_DungeonStart.worldDungeonId;
		}
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		if (respData_WorldMultiStartInfo != null)
		{
			worldDungeonId = respData_WorldMultiStartInfo.worldDungeonId;
		}
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM masterDungeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == worldDungeonId);
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == masterDungeon.worldStageId);
		string name = worldStageM2.name;
		this.areaName.text = name;
		string name2 = masterDungeon.name;
		string @string = StringMaster.GetString("BattleResult-01");
		this.stageName.text = string.Format(@string, name2);
	}

	public override void Show()
	{
		this.RunWinAnimation();
	}

	private void RunWinAnimation()
	{
		this.skipCount = DropItemResult.SkipCount.WinAnimation;
		NGUITools.SetActiveSelf(this.winLogo, true);
	}

	public void FinishWinAnimation()
	{
		if (this.skipCount == DropItemResult.SkipCount.WinAnimation)
		{
			this.RunTitleAnimation();
		}
	}

	private void RunTitleAnimation()
	{
		this.skipCount = DropItemResult.SkipCount.TitleAnimation;
		NGUITools.SetActiveSelf(this.titleGO, true);
	}

	public void FinishTitleAnimation()
	{
		if (this.skipCount == DropItemResult.SkipCount.TitleAnimation)
		{
			this.ShowDrops();
		}
	}

	private void ShowDrops()
	{
		this.skipCount = DropItemResult.SkipCount.Drops;
		GameWebAPI.RespDataWD_DungeonStart.Drop[] array = null;
		GameWebAPI.RespDataWD_DungeonStart.LuckDrop luckDrop = null;
		GameWebAPI.RespDataWD_DungeonResult.OptionDrop[] array2 = null;
		GameWebAPI.RespDataWD_DungeonResult.EventChipReward[] array3 = null;
		GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] array4 = null;
		GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] array5 = null;
		GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LuckDrop[] array6 = null;
		GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
		if (respDataWD_DungeonStart != null)
		{
			array = respDataWD_DungeonStart.dungeonFloor.Where((GameWebAPI.RespDataWD_DungeonStart.DungeonFloor x) => null != x.enemy).SelectMany((GameWebAPI.RespDataWD_DungeonStart.DungeonFloor x) => x.enemy.Where((GameWebAPI.RespDataWD_DungeonStart.Enemy e) => null != e.drop).Select((GameWebAPI.RespDataWD_DungeonStart.Enemy e) => e.drop)).ToArray<GameWebAPI.RespDataWD_DungeonStart.Drop[]>().SelectMany((GameWebAPI.RespDataWD_DungeonStart.Drop[] x) => x).ToArray<GameWebAPI.RespDataWD_DungeonStart.Drop>();
			luckDrop = respDataWD_DungeonStart.luckDrop;
			if (ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult != null)
			{
				array2 = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult.optionDrop;
				array3 = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult.eventChipReward;
			}
		}
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		if (respData_WorldMultiStartInfo != null)
		{
			array = null;
			luckDrop = null;
			array = respData_WorldMultiStartInfo.dungeonFloor.Where((GameWebAPI.RespDataWD_DungeonStart.DungeonFloor x) => null != x.enemy).SelectMany((GameWebAPI.RespDataWD_DungeonStart.DungeonFloor x) => x.enemy.Where((GameWebAPI.RespDataWD_DungeonStart.Enemy e) => null != e.drop).Select((GameWebAPI.RespDataWD_DungeonStart.Enemy e) => e.drop)).ToArray<GameWebAPI.RespDataWD_DungeonStart.Drop[]>().SelectMany((GameWebAPI.RespDataWD_DungeonStart.Drop[] x) => x).ToArray<GameWebAPI.RespDataWD_DungeonStart.Drop>();
			GameWebAPI.RespData_WorldMultiResultInfoLogic respData_WorldMultiResultInfoLogic = ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic;
			if (respData_WorldMultiResultInfoLogic.dungeonReward != null)
			{
				array6 = respData_WorldMultiResultInfoLogic.dungeonReward.luckDrop;
				GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo2 = DataMng.Instance().RespData_WorldMultiStartInfo;
				bool flag = respData_WorldMultiStartInfo2.party[0].userId == DataMng.Instance().RespDataCM_Login.playerInfo.userId;
				if (flag)
				{
					array4 = respData_WorldMultiResultInfoLogic.dungeonReward.ownerDropReward;
				}
				array5 = respData_WorldMultiResultInfoLogic.dungeonReward.multiReward;
			}
			array2 = respData_WorldMultiResultInfoLogic.optionDrop;
			array3 = respData_WorldMultiResultInfoLogic.eventChipReward;
		}
		List<DropItemTotalParts.Data> list = new List<DropItemTotalParts.Data>();
		if (array != null)
		{
			foreach (GameWebAPI.RespDataWD_DungeonStart.Drop drop in array)
			{
				list.Add(new DropItemTotalParts.Data
				{
					assetCategoryId = drop.assetCategoryId,
					objectId = drop.assetValue.ToString(),
					num = drop.assetNum
				});
			}
		}
		if (luckDrop != null)
		{
			list.Add(new DropItemTotalParts.Data
			{
				assetCategoryId = luckDrop.assetCategoryId,
				objectId = luckDrop.assetValue.ToString(),
				num = luckDrop.assetNum
			});
		}
		if (array4 != null)
		{
			foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward dropReward in array4)
			{
				list.Add(new DropItemTotalParts.Data
				{
					assetCategoryId = dropReward.assetCategoryId,
					objectId = dropReward.assetValue.ToString(),
					num = dropReward.assetNum
				});
			}
		}
		if (array5 != null)
		{
			foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward dropReward2 in array5)
			{
				list.Add(new DropItemTotalParts.Data
				{
					assetCategoryId = dropReward2.assetCategoryId,
					objectId = dropReward2.assetValue.ToString(),
					num = dropReward2.assetNum
				});
			}
		}
		if (array6 != null)
		{
			foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LuckDrop luckDrop2 in array6)
			{
				list.Add(new DropItemTotalParts.Data
				{
					assetCategoryId = luckDrop2.assetCategoryId,
					objectId = luckDrop2.assetValue.ToString(),
					num = luckDrop2.assetNum
				});
			}
		}
		if (array2 != null)
		{
			foreach (GameWebAPI.RespDataWD_DungeonResult.OptionDrop optionDrop in array2)
			{
				list.Add(new DropItemTotalParts.Data
				{
					assetCategoryId = optionDrop.assetCategoryId,
					objectId = optionDrop.assetValue.ToString(),
					num = optionDrop.assetNum.ToInt32()
				});
			}
		}
		if (array3 != null)
		{
			foreach (GameWebAPI.RespDataWD_DungeonResult.EventChipReward eventChipReward in array3)
			{
				list.Add(new DropItemTotalParts.Data
				{
					assetCategoryId = eventChipReward.assetCategoryId,
					objectId = eventChipReward.assetValue.ToString(),
					num = eventChipReward.assetNum.ToInt32()
				});
			}
		}
		List<DropItemTotalParts.Data> list2 = new List<DropItemTotalParts.Data>();
		while (list.Count > 0)
		{
			DropItemTotalParts.Data data = list[0];
			int num = 0;
			foreach (DropItemTotalParts.Data data2 in list)
			{
				if (data.assetCategoryId == data2.assetCategoryId && data.objectId == data2.objectId)
				{
					num += data2.num;
				}
			}
			DropItemTotalParts.Data newData = new DropItemTotalParts.Data();
			newData.assetCategoryId = data.assetCategoryId;
			newData.objectId = data.objectId;
			newData.num = num;
			list2.Add(newData);
			list = list.Where((DropItemTotalParts.Data x) => x.assetCategoryId != newData.assetCategoryId || x.objectId != newData.objectId).ToList<DropItemTotalParts.Data>();
		}
		this.dropItemTotalList = new DropItemTotalList(base.gameObject, list2.ToArray());
		this.dropItemTotalList.SetActive(false);
		this.dropItemList = new DropItemList(base.gameObject, 10, new Vector2(890f, 250f), array, luckDrop, array4, array5, array6, array2, array3);
		this.dropItemList.SetScrollBarPosX(550f);
		this.dropItemList.SetPosition(new Vector3(0f, 40f, 100f));
		if (this.clipingTextures != null)
		{
			foreach (UITexture uitexture in this.clipingTextures)
			{
				uitexture.depth = 0;
			}
		}
		IEnumerator routine = this.dropItemList.SetDrops(false, new Action(this.SetDropCallBack));
		this.coroutineDropInfo = AppCoroutine.Start(routine, false);
		if (this.lines != null)
		{
			foreach (GameObject gameObject in this.lines)
			{
				gameObject.SetActive(true);
			}
		}
		this.isUserMonsterRefresh = false;
		this.isChipMonsterRefresh = false;
		foreach (DropItemTotalParts.Data data3 in list2)
		{
			if (data3.assetCategoryId.ToInt32() == 1)
			{
				this.isUserMonsterRefresh = true;
			}
			if (data3.assetCategoryId.ToInt32() == 17)
			{
				this.isChipMonsterRefresh = true;
			}
		}
	}

	private void SetDropCallBack()
	{
		this.skipCount = DropItemResult.SkipCount.Finish;
		this.dropItemTotalList.SetActive(true);
		base.ShowNextTap();
	}

	public override void OnTapped()
	{
		switch (this.skipCount)
		{
		case DropItemResult.SkipCount.WinAnimation:
			this.SkipWinAnimation();
			break;
		case DropItemResult.SkipCount.TitleAnimation:
			this.SkipTitleAnimation();
			break;
		case DropItemResult.SkipCount.Drops:
			this.SkipDrops();
			break;
		case DropItemResult.SkipCount.Finish:
			if (base.isShowTapNext)
			{
				base.HideNextTap();
				base.isEnd = true;
			}
			break;
		}
	}

	private void SkipWinAnimation()
	{
		NGUITools.SetActiveSelf(this.winLogo, false);
		NGUITools.SetActiveSelf(this.winLogoForSkip, true);
		this.RunTitleAnimation();
	}

	private void SkipTitleAnimation()
	{
		base.ResetTweenAlpha(this.titleGO);
		this.ShowDrops();
	}

	private void SkipDrops()
	{
		if (this.coroutineDropInfo != null)
		{
			AppCoroutine.Stop(this.coroutineDropInfo, false);
			this.coroutineDropInfo = null;
		}
		IEnumerator enumerator = this.dropItemList.SetDrops(true, new Action(this.SetDropCallBack));
		while (enumerator.MoveNext())
		{
		}
	}

	private enum SkipCount
	{
		None,
		WinAnimation,
		TitleAnimation,
		Drops,
		Finish
	}
}
