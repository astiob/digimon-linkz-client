using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_ChipSortModal : CMD
{
	[SerializeField]
	private CMD_ChipSortModal.SortInfo sortInfo;

	[SerializeField]
	private CMD_ChipSortModal.RefineInfo refineInfo;

	[SerializeField]
	private CMD_ChipSortModal.ButtonInfo reset;

	[SerializeField]
	private CMD_ChipSortModal.ButtonInfo decision;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel currentSortNameLabel;

	[SerializeField]
	private UILabel currentSortCountLabel;

	[SerializeField]
	private GUICollider closeButton;

	private static CMD_ChipSortModal.Data data = new CMD_ChipSortModal.Data();

	private CMD_ChipSortModal.Data tempData = new CMD_ChipSortModal.Data();

	private static GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] baseUserChipList = new GameWebAPI.RespDataCS_ChipListLogic.UserChipList[0];

	public static GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] sortedUserChipList = new GameWebAPI.RespDataCS_ChipListLogic.UserChipList[0];

	private static CMD_ChipSortModal.RefineType[] rankRefineTypes = new CMD_ChipSortModal.RefineType[]
	{
		CMD_ChipSortModal.RefineType.RankG,
		CMD_ChipSortModal.RefineType.RankF,
		CMD_ChipSortModal.RefineType.RankE,
		CMD_ChipSortModal.RefineType.RankD,
		CMD_ChipSortModal.RefineType.RankC,
		CMD_ChipSortModal.RefineType.RankB,
		CMD_ChipSortModal.RefineType.RankA,
		CMD_ChipSortModal.RefineType.RankS,
		CMD_ChipSortModal.RefineType.RankSS,
		CMD_ChipSortModal.RefineType.RankLS
	};

	private static CMD_ChipSortModal.RefineType[] tribeRefineTypes = new CMD_ChipSortModal.RefineType[]
	{
		CMD_ChipSortModal.RefineType.TribeHeatHaze,
		CMD_ChipSortModal.RefineType.TribeEarth,
		CMD_ChipSortModal.RefineType.TribeShaftOfLight,
		CMD_ChipSortModal.RefineType.TribeAbyss,
		CMD_ChipSortModal.RefineType.TribeElectromagnetic,
		CMD_ChipSortModal.RefineType.TribeGlacier,
		CMD_ChipSortModal.RefineType.TribePhantomStudents
	};

	public static CMD_ChipSortModal Create(Action<int> callback = null)
	{
		return GUIMain.ShowCommonDialog(callback, "CMD_ChipModalSort") as CMD_ChipSortModal;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		this.tempData.Copy(CMD_ChipSortModal.data);
		this.SetupSortUI();
		this.SetupRefineUI();
		this.decision.label.text = StringMaster.GetString("ChipSortModal-02");
		this.decision.button.CallBackClass = base.gameObject;
		this.decision.button.MethodToInvoke = "OnDecisionButton";
		this.closeButton.CallBackClass = base.gameObject;
		this.closeButton.MethodToInvoke = "OnCloseButton";
		this.titleLabel.text = StringMaster.GetString("ChipSortModal-01");
		this.UpdateCurrentSortCountLabel();
	}

	private void UpdateCurrentSortCountLabel()
	{
		this.currentSortCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), CMD_ChipSortModal.sortedUserChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(), CMD_ChipSortModal.baseUserChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>());
	}

	private void SetupSortUI()
	{
		this.sortInfo.orderByAsc.label.text = StringMaster.GetString("ChipSortModal-05");
		this.sortInfo.orderByAsc.button.CallBackClass = base.gameObject;
		this.sortInfo.orderByAsc.button.MethodToInvoke = "OnOrderByAsc";
		this.sortInfo.orderByAsc.SetSelect(this.tempData.isOrderByAsc);
		this.sortInfo.orderByDesc.label.text = StringMaster.GetString("ChipSortModal-06");
		this.sortInfo.orderByDesc.button.CallBackClass = base.gameObject;
		this.sortInfo.orderByDesc.button.MethodToInvoke = "OnOrderByDesc";
		this.sortInfo.orderByDesc.SetSelect(!this.tempData.isOrderByAsc);
		CMD_ChipSortModal.SortButtonInfo[] sortContents = this.sortInfo.sortContents;
		for (int i = 0; i < sortContents.Length; i++)
		{
			CMD_ChipSortModal.SortButtonInfo sortButtonInfo = sortContents[i];
			CMD_ChipSortModal.SortType sortType = sortButtonInfo.sortType;
			sortButtonInfo.label.text = CMD_ChipSortModal.GetSortName(sortType);
			sortButtonInfo.button.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnSortContentButton(sortType);
			};
			sortButtonInfo.SetSelect(sortType == this.tempData.sortType);
		}
		this.currentSortNameLabel.text = string.Format(StringMaster.GetString("Sort-01"), CMD_ChipSortModal.GetSortName(this.tempData.sortType));
	}

	private void SetupRefineUI()
	{
		this.refineInfo.allRank.button.CallBackClass = base.gameObject;
		this.refineInfo.allRank.button.MethodToInvoke = "OnAllRankButton";
		this.refineInfo.allRank.label.text = StringMaster.GetString("ChipSortModal-04");
		this.refineInfo.allRank.SetSelect(this.tempData.isAllRank);
		this.refineInfo.allTribe.button.CallBackClass = base.gameObject;
		this.refineInfo.allTribe.button.MethodToInvoke = "OnAllTribeButton";
		this.refineInfo.allTribe.label.text = StringMaster.GetString("ChipSortModal-04");
		this.refineInfo.allTribe.SetSelect(this.tempData.isAllTribe);
		CMD_ChipSortModal.RefineButtonInfo[] refineContents = this.refineInfo.refineContents;
		for (int i = 0; i < refineContents.Length; i++)
		{
			CMD_ChipSortModal.RefineButtonInfo refineButtonInfo = refineContents[i];
			CMD_ChipSortModal.RefineType refineType = refineButtonInfo.refineType;
			refineButtonInfo.button.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				this.OnRefineContentButton(refineType);
			};
			refineButtonInfo.SetSelect((this.tempData.refineTypeList & (int)refineType) > 0);
		}
		this.reset.label.text = StringMaster.GetString("ChipSortModal-03");
		this.reset.button.CallBackClass = base.gameObject;
		this.reset.button.MethodToInvoke = "OnResetButton";
		bool value = this.CheckEnableResetButton();
		this.EnableResetButton(value);
	}

	private void OnSortContentButton(CMD_ChipSortModal.SortType sortType)
	{
		this.tempData.sortType = sortType;
		foreach (CMD_ChipSortModal.SortButtonInfo sortButtonInfo in this.sortInfo.sortContents)
		{
			sortButtonInfo.SetSelect(sortButtonInfo.sortType == this.tempData.sortType);
		}
		this.currentSortNameLabel.text = string.Format(StringMaster.GetString("Sort-01"), CMD_ChipSortModal.GetSortName(this.tempData.sortType));
	}

	private void OnOrderByAsc()
	{
		this.tempData.isOrderByAsc = true;
		this.sortInfo.orderByAsc.SetSelect(this.tempData.isOrderByAsc);
		this.sortInfo.orderByDesc.SetSelect(!this.tempData.isOrderByAsc);
	}

	private void OnOrderByDesc()
	{
		this.tempData.isOrderByAsc = false;
		this.sortInfo.orderByAsc.SetSelect(this.tempData.isOrderByAsc);
		this.sortInfo.orderByDesc.SetSelect(!this.tempData.isOrderByAsc);
	}

	private void OnRefineContentButton(CMD_ChipSortModal.RefineType refineType)
	{
		Action action = delegate()
		{
			this.SetSelectRefine((this.tempData.refineTypeList & (int)refineType) <= 0, refineType);
			CMD_ChipSortModal.sortedUserChipList = CMD_ChipSortModal.GetRefineSortList(CMD_ChipSortModal.baseUserChipList, this.tempData);
			bool value = this.CheckEnableResetButton();
			this.EnableResetButton(value);
			this.tempData.isAllRank = this.CheckAllButton(CMD_ChipSortModal.rankRefineTypes);
			this.refineInfo.allRank.SetSelect(this.tempData.isAllRank);
			this.tempData.isAllTribe = this.CheckAllButton(CMD_ChipSortModal.tribeRefineTypes);
			this.refineInfo.allTribe.SetSelect(this.tempData.isAllTribe);
			this.UpdateCurrentSortCountLabel();
		};
		AppCoroutine.Start(this.Load(action), false);
	}

	private void OnAllRankButton()
	{
		Action action = delegate()
		{
			this.SetSelectRefine(true, CMD_ChipSortModal.rankRefineTypes);
			CMD_ChipSortModal.sortedUserChipList = CMD_ChipSortModal.GetRefineSortList(CMD_ChipSortModal.baseUserChipList, this.tempData);
			this.tempData.isAllRank = true;
			this.refineInfo.allRank.SetSelect(this.tempData.isAllRank);
			bool value = this.CheckEnableResetButton();
			this.EnableResetButton(value);
			this.UpdateCurrentSortCountLabel();
		};
		AppCoroutine.Start(this.Load(action), false);
	}

	private void OnAllTribeButton()
	{
		Action action = delegate()
		{
			this.SetSelectRefine(true, CMD_ChipSortModal.tribeRefineTypes);
			CMD_ChipSortModal.sortedUserChipList = CMD_ChipSortModal.GetRefineSortList(CMD_ChipSortModal.baseUserChipList, this.tempData);
			this.tempData.isAllTribe = true;
			this.refineInfo.allTribe.SetSelect(this.tempData.isAllTribe);
			bool value = this.CheckEnableResetButton();
			this.EnableResetButton(value);
			this.UpdateCurrentSortCountLabel();
		};
		AppCoroutine.Start(this.Load(action), false);
	}

	private void SetSelectRefine(bool value, CMD_ChipSortModal.RefineType[] refineTypes)
	{
		foreach (CMD_ChipSortModal.RefineType refineType in refineTypes)
		{
			this.SetSelectRefine(value, refineType);
		}
	}

	private void SetSelectRefine(bool value, CMD_ChipSortModal.RefineType refineType)
	{
		if (value)
		{
			this.tempData.refineTypeList |= (int)refineType;
		}
		else
		{
			this.tempData.refineTypeList |= (int)refineType;
			this.tempData.refineTypeList ^= (int)refineType;
		}
		foreach (CMD_ChipSortModal.RefineButtonInfo refineButtonInfo in this.refineInfo.refineContents)
		{
			if (refineButtonInfo.refineType == refineType)
			{
				refineButtonInfo.SetSelect(value);
				break;
			}
		}
	}

	private void OnDecisionButton()
	{
		CMD_ChipSortModal.data.Copy(this.tempData);
		base.SetForceReturnValue(1);
		this.ClosePanel(true);
	}

	public void OnResetButton()
	{
		Action action = delegate()
		{
			foreach (object obj in Enum.GetValues(typeof(CMD_ChipSortModal.RefineType)))
			{
				this.SetSelectRefine(true, (CMD_ChipSortModal.RefineType)((int)obj));
			}
			CMD_ChipSortModal.sortedUserChipList = CMD_ChipSortModal.GetRefineSortList(CMD_ChipSortModal.baseUserChipList, this.tempData);
			this.tempData.isAllRank = true;
			this.refineInfo.allRank.SetSelect(this.tempData.isAllRank);
			this.tempData.isAllTribe = true;
			this.refineInfo.allTribe.SetSelect(this.tempData.isAllTribe);
			this.EnableResetButton(false);
			this.UpdateCurrentSortCountLabel();
		};
		AppCoroutine.Start(this.Load(action), false);
	}

	private void OnCloseButton()
	{
		base.SetForceReturnValue(0);
		this.ClosePanel(true);
	}

	private bool CheckEnableResetButton()
	{
		return this.tempData.refineTypeList > 0;
	}

	private void EnableResetButton(bool value)
	{
		UISprite component = this.reset.button.GetComponent<UISprite>();
		component.GetComponent<BoxCollider>().enabled = value;
		component.spriteName = ((!value) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON");
		this.reset.label.color = ((!value) ? ConstValue.DEACTIVE_BUTTON_LABEL : Color.white);
	}

	private bool CheckAllButton(CMD_ChipSortModal.RefineType[] refineTypes)
	{
		foreach (CMD_ChipSortModal.RefineType refineType in refineTypes)
		{
			if ((this.tempData.refineTypeList & (int)refineType) <= 0)
			{
				return false;
			}
		}
		return true;
	}

	private static string GetSortName(CMD_ChipSortModal.SortType sortType)
	{
		if (sortType == CMD_ChipSortModal.SortType.Rarity)
		{
			return StringMaster.GetString("ChipSortModal-07");
		}
		if (sortType != CMD_ChipSortModal.SortType.Time)
		{
			return string.Empty;
		}
		return StringMaster.GetString("ChipSortModal-08");
	}

	public static string GetSortName()
	{
		return CMD_ChipSortModal.GetSortName(CMD_ChipSortModal.data.sortType);
	}

	private static GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] GetRefineSortList(GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList, CMD_ChipSortModal.Data sortData)
	{
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		if (userChipList == null)
		{
			return list.ToArray();
		}
		int num = 0;
		foreach (CMD_ChipSortModal.RefineType refineType in CMD_ChipSortModal.tribeRefineTypes)
		{
			if ((sortData.refineTypeList & (int)refineType) > 0)
			{
				num++;
			}
		}
		bool flag = CMD_ChipSortModal.tribeRefineTypes.Length == num || num == 0;
		int refineTypeList = sortData.refineTypeList;
		foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in userChipList)
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(userChipList2);
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(userChipList2);
			int num2 = 0;
			string text = chipMainData.rank;
			switch (text)
			{
			case "1":
				num2 |= 1;
				break;
			case "2":
				num2 |= 2;
				break;
			case "3":
				num2 |= 4;
				break;
			case "4":
				num2 |= 8;
				break;
			case "5":
				num2 |= 16;
				break;
			case "6":
				num2 |= 32;
				break;
			case "7":
				num2 |= 64;
				break;
			case "8":
				num2 |= 128;
				break;
			case "9":
				num2 |= 256;
				break;
			case "10":
				num2 |= 512;
				break;
			}
			bool flag2 = (refineTypeList & num2) > 0;
			int num4 = 0;
			foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
			{
				if (chipEffect.targetSubType == "2")
				{
					text = chipEffect.targetValue;
					switch (text)
					{
					case "1":
						num4 |= 65536;
						break;
					case "2":
						num4 |= 1024;
						break;
					case "3":
						num4 |= 32768;
						break;
					case "4":
						num4 |= 16384;
						break;
					case "5":
						num4 |= 2048;
						break;
					case "6":
						num4 |= 4096;
						break;
					case "7":
						num4 |= 8192;
						break;
					}
				}
			}
			bool flag3 = (refineTypeList & num4) > 0;
			if (num4 == 0 && flag)
			{
				flag3 = true;
			}
			if (flag2 && flag3)
			{
				list.Add(userChipList2);
			}
		}
		if (sortData.sortType == CMD_ChipSortModal.SortType.Time)
		{
			if (sortData.isOrderByAsc)
			{
				list.Sort(new Comparison<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(CMD_ChipSortModal.ComparerTimeOrderByAsc));
			}
			else
			{
				list.Sort(new Comparison<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(CMD_ChipSortModal.ComparerTimeOrderByDesc));
			}
		}
		else if (sortData.sortType == CMD_ChipSortModal.SortType.Rarity)
		{
			if (sortData.isOrderByAsc)
			{
				list.Sort(new Comparison<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(CMD_ChipSortModal.ComparerRarityOrderByAsc));
			}
			else
			{
				list.Sort(new Comparison<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(CMD_ChipSortModal.ComparerRarityOrderByDesc));
			}
		}
		return list.ToArray();
	}

	private static int ComparerTimeOrderByAsc(GameWebAPI.RespDataCS_ChipListLogic.UserChipList x, GameWebAPI.RespDataCS_ChipListLogic.UserChipList y)
	{
		return x.userChipId - y.userChipId;
	}

	private static int ComparerTimeOrderByDesc(GameWebAPI.RespDataCS_ChipListLogic.UserChipList x, GameWebAPI.RespDataCS_ChipListLogic.UserChipList y)
	{
		return y.userChipId - x.userChipId;
	}

	private static int ComparerRarityOrderByAsc(GameWebAPI.RespDataCS_ChipListLogic.UserChipList x, GameWebAPI.RespDataCS_ChipListLogic.UserChipList y)
	{
		GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(x);
		GameWebAPI.RespDataMA_ChipM.Chip chipMainData2 = ChipDataMng.GetChipMainData(y);
		int num = chipMainData.rank.ToInt32();
		int num2 = chipMainData2.rank.ToInt32();
		int num3 = num - num2;
		return (num3 != 0) ? num3 : (x.chipId - y.chipId);
	}

	private static int ComparerRarityOrderByDesc(GameWebAPI.RespDataCS_ChipListLogic.UserChipList x, GameWebAPI.RespDataCS_ChipListLogic.UserChipList y)
	{
		GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(x);
		GameWebAPI.RespDataMA_ChipM.Chip chipMainData2 = ChipDataMng.GetChipMainData(y);
		int num = chipMainData.rank.ToInt32();
		int num2 = chipMainData2.rank.ToInt32();
		int num3 = num2 - num;
		return (num3 != 0) ? num3 : (y.chipId - x.chipId);
	}

	public static void UpdateSortedUserChipList(GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] list)
	{
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list2 = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		if (list != null)
		{
			for (int i = 0; i < list.Length; i++)
			{
				list2.Add(list[i]);
			}
		}
		CMD_ChipSortModal.baseUserChipList = list2.ToArray();
		CMD_ChipSortModal.sortedUserChipList = CMD_ChipSortModal.GetRefineSortList(CMD_ChipSortModal.baseUserChipList, CMD_ChipSortModal.data);
	}

	private IEnumerator Load(Action action)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		action();
		RestrictionInput.EndLoad();
		yield return null;
		yield break;
	}

	public enum SortType
	{
		Rarity,
		Time
	}

	[Flags]
	public enum RefineType
	{
		None = 0,
		RankG = 1,
		RankF = 2,
		RankE = 4,
		RankD = 8,
		RankC = 16,
		RankB = 32,
		RankA = 64,
		RankS = 128,
		RankSS = 256,
		RankLS = 512,
		TribeHeatHaze = 1024,
		TribeEarth = 2048,
		TribeShaftOfLight = 4096,
		TribeAbyss = 8192,
		TribeElectromagnetic = 16384,
		TribeGlacier = 32768,
		TribePhantomStudents = 65536
	}

	public class Data
	{
		public bool isOrderByAsc = true;

		public CMD_ChipSortModal.SortType sortType;

		public int refineTypeList;

		public bool isAllRank = true;

		public bool isAllTribe = true;

		public Data()
		{
			foreach (object obj in Enum.GetValues(typeof(CMD_ChipSortModal.RefineType)))
			{
				this.refineTypeList |= (int)obj;
			}
		}

		public void Copy(CMD_ChipSortModal.Data data)
		{
			this.isOrderByAsc = data.isOrderByAsc;
			this.sortType = data.sortType;
			this.refineTypeList = data.refineTypeList;
			this.isAllRank = data.isAllRank;
			this.isAllTribe = data.isAllTribe;
		}
	}

	[Serializable]
	public class ButtonInfo
	{
		public GUICollider button;

		public UILabel label;

		public void SetSelect(bool isSelect)
		{
			string spriteName = string.Empty;
			Color color = Color.white;
			if (isSelect)
			{
				spriteName = "Common02_Btn_SupportRed";
				color = Color.white;
			}
			else
			{
				spriteName = "Common02_Btn_SupportWhite";
				color = Color.black;
			}
			if (this.button != null)
			{
				UISprite component = this.button.GetComponent<UISprite>();
				if (component != null)
				{
					component.spriteName = spriteName;
				}
			}
			if (this.label != null)
			{
				this.label.color = color;
			}
		}

		public void SetEnable(bool enable)
		{
			string spriteName = string.Empty;
			Color white = Color.white;
			bool activeCollider;
			if (enable)
			{
				spriteName = "Common02_Btn_BaseON";
				white = Color.white;
				activeCollider = true;
			}
			else
			{
				spriteName = "Common02_Btn_BaseG";
				white = Color.white;
				activeCollider = false;
			}
			if (this.button != null)
			{
				UISprite component = this.button.GetComponent<UISprite>();
				if (component != null)
				{
					component.spriteName = spriteName;
				}
				this.button.activeCollider = activeCollider;
			}
			if (this.label != null)
			{
				this.label.color = white;
			}
		}
	}

	[Serializable]
	public class SortButtonInfo : CMD_ChipSortModal.ButtonInfo
	{
		public CMD_ChipSortModal.SortType sortType;
	}

	[Serializable]
	public class RefineButtonInfo : CMD_ChipSortModal.ButtonInfo
	{
		public CMD_ChipSortModal.RefineType refineType;
	}

	[Serializable]
	public class SortInfo
	{
		public CMD_ChipSortModal.ButtonInfo orderByAsc;

		public CMD_ChipSortModal.ButtonInfo orderByDesc;

		public CMD_ChipSortModal.SortButtonInfo[] sortContents;
	}

	[Serializable]
	public class RefineInfo
	{
		public CMD_ChipSortModal.ButtonInfo allRank;

		public CMD_ChipSortModal.ButtonInfo allTribe;

		public CMD_ChipSortModal.RefineButtonInfo[] refineContents;
	}
}
