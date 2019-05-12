using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Tips : CMD
{
	private readonly float switchTipsIntervalTime = 4f;

	[SerializeField]
	private UILabel text;

	[SerializeField]
	private UITexture thumbnail;

	[Header("このダイアログのZ値(dont manage zpos が起っていること)")]
	[SerializeField]
	private float zPos;

	private static CMD_Tips.DISPLAY_PLACE displayPlace = CMD_Tips.DISPLAY_PLACE.TitleToFarm;

	private List<CMD_Tips.TipsM.Tips> displayTipsDataList = new List<CMD_Tips.TipsM.Tips>();

	private int displayTipsDataListIndex;

	private Dictionary<string, Texture2D> thumbnails;

	public static CMD_Tips.DISPLAY_PLACE DisPlayPlace
	{
		set
		{
			CMD_Tips.displayPlace = value;
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.Initialize(delegate
		{
			this.Show(f, sizeX, sizeY, aT);
		});
	}

	protected override void Awake()
	{
		base.Awake();
		base.SetOriginalPos(new Vector3(0f, 0f, this.zPos));
	}

	protected override void MakeAnimation(bool open, float atime, iTween.EaseType type = iTween.EaseType.linear)
	{
		if (open)
		{
			return;
		}
		if (!this.useCMDAnim)
		{
			base.MakeAnimation(open, atime, type);
		}
		else if (atime <= 0f)
		{
			base.MakeAnimation(open, atime, type);
		}
		else
		{
			base.SetScaleFlg(false);
		}
	}

	public override void ClosePanel(bool animation = false)
	{
		base.CancelInvoke();
		base.ClosePanel(false);
	}

	protected virtual void Initialize(Action OnInitialized)
	{
		this.displayTipsDataList = CMD_Tips.GetDisplayTipsData(CMD_Tips.displayPlace);
		this.LoadNaviThumb();
		this.DisplayTips();
		OnInitialized();
	}

	public static List<CMD_Tips.TipsM.Tips> GetDisplayTipsData(CMD_Tips.DISPLAY_PLACE DisplayPlace)
	{
		CMD_Tips.TipsM.TipsManage[] tipsManage = MasterDataMng.Instance().RespDataMA_TipsM.tipsM.tipsManage;
		CMD_Tips.TipsM.Tips[] tips = MasterDataMng.Instance().RespDataMA_TipsM.tipsM.tips;
		int num = tips.Length;
		int num2 = (int)DisplayPlace;
		string a = num2.ToString();
		List<CMD_Tips.TipsM.Tips> list = new List<CMD_Tips.TipsM.Tips>();
		foreach (CMD_Tips.TipsM.TipsManage tipsManage2 in tipsManage)
		{
			if (a == tipsManage2.dispType)
			{
				CMD_Tips.TipsM.Tips tips2 = Algorithm.BinarySearch<CMD_Tips.TipsM.Tips>(tips, tipsManage2.tipsId, 0, num - 1, "tipsId", 8);
				if (tips2 != null)
				{
					list.Add(tips2);
				}
			}
		}
		return list;
	}

	public static CMD_Tips.TipsM.Tips GetDisplayTips(List<CMD_Tips.TipsM.Tips> dtL, string tipsId)
	{
		for (int i = 0; i < dtL.Count; i++)
		{
			if (dtL[i].tipsId == tipsId)
			{
				return dtL[i];
			}
		}
		return null;
	}

	private void DisplayTips()
	{
		base.InvokeRepeating("DisplayTips_", 0f, this.switchTipsIntervalTime);
	}

	private void DisplayTips_()
	{
		if (this.displayTipsDataListIndex == 0)
		{
			this.displayTipsDataList = Algorithm.ShuffuleList<CMD_Tips.TipsM.Tips>(this.displayTipsDataList);
		}
		if (this.displayTipsDataList.Count > 0)
		{
			this.text.text = this.displayTipsDataList[this.displayTipsDataListIndex].message;
			Texture2D mainTexture;
			this.thumbnails.TryGetValue(this.displayTipsDataList[this.displayTipsDataListIndex].img + this.displayTipsDataList[this.displayTipsDataListIndex].icon, out mainTexture);
			this.thumbnail.mainTexture = mainTexture;
			this.displayTipsDataListIndex++;
		}
		if (this.displayTipsDataListIndex == this.displayTipsDataList.Count)
		{
			this.displayTipsDataListIndex = 0;
		}
	}

	private void LoadNaviThumb()
	{
		this.thumbnails = new Dictionary<string, Texture2D>();
		foreach (CMD_Tips.TipsM.Tips tips in this.displayTipsDataList)
		{
			string text = tips.img + tips.icon;
			if (!this.thumbnails.ContainsKey(text))
			{
				Texture2D value = NGUIUtil.LoadTexture("Navi/" + text);
				this.thumbnails.Add(text, value);
			}
		}
	}

	public enum DISPLAY_PLACE
	{
		TitleToFarm = 1,
		QuestToSoloBattle,
		BattleToFarm = 4,
		TutorialToFarm,
		TutorialToBattle,
		QuestToMultiBattle = 14,
		QuestSelect_NORMAL = 16,
		QuestSelect_DESCENT,
		QuestSelect_EVENT
	}

	[Serializable]
	public class TipsM
	{
		public CMD_Tips.TipsM.Tips[] tips;

		public CMD_Tips.TipsM.TipsManage[] tipsManage;

		[Serializable]
		public class Tips
		{
			public string tipsId;

			public string message;

			public string icon;

			public string img;
		}

		[Serializable]
		public class TipsManage
		{
			public string tipsManageId;

			public string tipsId;

			public string dispType;
		}
	}
}
