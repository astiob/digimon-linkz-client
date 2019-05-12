using System;
using System.Collections.Generic;
using UnityEngine;

public class GUITips : MonoBehaviour
{
	private readonly float SWITCH_COMMENT_INTERVAL_TIME = 5f;

	[SerializeField]
	private UILabel commentLabel;

	[SerializeField]
	private UITexture thumbnail;

	private Dictionary<string, Texture2D> thumbnails;

	private List<CMD_Tips.TipsM.Tips> displayCommentDataList = new List<CMD_Tips.TipsM.Tips>();

	private int displayCommentDataListIndex;

	private CMD_Tips.TipsM.Tips lastTips;

	public void Init(GUITips.TIPS_DISP_TYPE dispType)
	{
		this.SetCommentData(dispType);
		this.LoadNaviThumb();
		this.DisplayComment();
	}

	private void SetCommentData(GUITips.TIPS_DISP_TYPE dispType)
	{
		List<List<CMD_Tips.TipsM.TipsManage>> list = new List<List<CMD_Tips.TipsM.TipsManage>>();
		List<CMD_Tips.TipsM.Tips> list2 = new List<CMD_Tips.TipsM.Tips>();
		foreach (CMD_Tips.TipsM.TipsManage tipsManage2 in MasterDataMng.Instance().RespDataMA_TipsM.tipsM.tipsManage)
		{
			int num = int.Parse(tipsManage2.dispType);
			if (list.Count < num)
			{
				list.Add(new List<CMD_Tips.TipsM.TipsManage>());
			}
			if (tipsManage2 != null)
			{
				list[list.Count - 1].Add(tipsManage2);
			}
		}
		foreach (CMD_Tips.TipsM.Tips tips2 in MasterDataMng.Instance().RespDataMA_TipsM.tipsM.tips)
		{
			if (tips2 != null)
			{
				list2.Add(tips2);
			}
		}
		if (list.Count <= dispType - (GUITips.TIPS_DISP_TYPE)1)
		{
			global::Debug.LogError("Tipsがありません。マスターを確認してください。");
			return;
		}
		this.displayCommentDataList.Clear();
		foreach (CMD_Tips.TipsM.TipsManage tipsManage3 in list[dispType - (GUITips.TIPS_DISP_TYPE)1])
		{
			this.displayCommentDataList.Add(Algorithm.BinarySearch<CMD_Tips.TipsM.Tips>(list2, tipsManage3.tipsId, 0, list2.Count - 1, "tipsId", 8));
		}
	}

	private void DisplayComment()
	{
		if (this.displayCommentDataList.Count == 0)
		{
			this.commentLabel.text = string.Empty;
			return;
		}
		base.InvokeRepeating("DisplayComment_", 0f, this.SWITCH_COMMENT_INTERVAL_TIME);
	}

	private void DisplayComment_()
	{
		if (this.displayCommentDataListIndex == 0)
		{
			this.displayCommentDataList = this.ShuffulList(this.displayCommentDataList, this.lastTips);
		}
		if (this.displayCommentDataList.Count > 0)
		{
			this.commentLabel.text = this.displayCommentDataList[this.displayCommentDataListIndex].message;
			Texture2D mainTexture;
			this.thumbnails.TryGetValue(this.displayCommentDataList[this.displayCommentDataListIndex].img + this.displayCommentDataList[this.displayCommentDataListIndex].icon, out mainTexture);
			this.thumbnail.mainTexture = mainTexture;
			this.lastTips = this.displayCommentDataList[this.displayCommentDataListIndex];
			this.displayCommentDataListIndex++;
		}
		if (this.displayCommentDataListIndex == this.displayCommentDataList.Count)
		{
			this.displayCommentDataListIndex = 0;
		}
	}

	private void LoadNaviThumb()
	{
		this.thumbnails = new Dictionary<string, Texture2D>();
		foreach (CMD_Tips.TipsM.Tips tips in this.displayCommentDataList)
		{
			string text = tips.img + tips.icon;
			if (!this.thumbnails.ContainsKey(text))
			{
				Texture2D value = NGUIUtil.LoadTexture("Navi/" + text);
				this.thumbnails.Add(text, value);
			}
		}
	}

	private List<CMD_Tips.TipsM.Tips> ShuffulList(List<CMD_Tips.TipsM.Tips> list, CMD_Tips.TipsM.Tips lastTips)
	{
		if (list.Count <= 1)
		{
			return list;
		}
		List<CMD_Tips.TipsM.Tips> list2;
		CMD_Tips.TipsM.Tips tips;
		do
		{
			list2 = Algorithm.ShuffuleList<CMD_Tips.TipsM.Tips>(list);
			tips = list2[0];
		}
		while (tips == lastTips);
		return list2;
	}

	public enum TIPS_DISP_TYPE
	{
		ExchangeNavi = 12,
		NoneExchangeNavi
	}
}
