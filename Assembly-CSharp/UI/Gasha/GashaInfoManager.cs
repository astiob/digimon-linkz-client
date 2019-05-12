using System;
using System.Collections.Generic;

namespace UI.Gasha
{
	public sealed class GashaInfoManager
	{
		private List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaInfoList;

		public GashaInfoManager(GameWebAPI.RespDataGA_GetGachaInfo responseGashaInfo)
		{
			Debug.Assert(null != responseGashaInfo, "ガシャ情報を未取得.");
			this.gashaInfoList = new List<GameWebAPI.RespDataGA_GetGachaInfo.Result>(responseGashaInfo.result);
		}

		private int SortGashaInfo(GameWebAPI.RespDataGA_GetGachaInfo.Result x, GameWebAPI.RespDataGA_GetGachaInfo.Result y)
		{
			int num = int.Parse(x.dispNum);
			int num2 = int.Parse(y.dispNum);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			string startTime = x.startTime;
			string startTime2 = y.startTime;
			num = startTime.CompareTo(startTime2);
			num2 = startTime2.CompareTo(startTime);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			num = int.Parse(x.gachaId);
			num2 = int.Parse(y.gachaId);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return 0;
		}

		private int SortGashaDetail(GameWebAPI.RespDataGA_GetGachaInfo.Detail x, GameWebAPI.RespDataGA_GetGachaInfo.Detail y)
		{
			int num = int.Parse(x.count);
			int num2 = int.Parse(y.count);
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return 0;
		}

		public void SortInfo()
		{
			this.gashaInfoList.Sort(new Comparison<GameWebAPI.RespDataGA_GetGachaInfo.Result>(this.SortGashaInfo));
			for (int i = 0; i < this.gashaInfoList.Count; i++)
			{
				Array.Sort<GameWebAPI.RespDataGA_GetGachaInfo.Detail>(this.gashaInfoList[i].details, new Comparison<GameWebAPI.RespDataGA_GetGachaInfo.Detail>(this.SortGashaDetail));
			}
		}

		public GameWebAPI.RespDataGA_GetGachaInfo.Result GetInfo(int index)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Result result = null;
			if (index < this.gashaInfoList.Count)
			{
				result = this.gashaInfoList[index];
			}
			return result;
		}

		public List<GameWebAPI.RespDataGA_GetGachaInfo.Result> GetInfoList()
		{
			return this.gashaInfoList;
		}

		public void RemoveChipGasha()
		{
			int i = 0;
			while (i < this.gashaInfoList.Count)
			{
				if (this.gashaInfoList[i].GetPrizeAssetsCategory() == MasterDataMng.AssetCategory.CHIP)
				{
					this.gashaInfoList.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		public List<string> GetEndTimeList()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.gashaInfoList.Count; i++)
			{
				list.Add(this.gashaInfoList[i].endTime);
			}
			return list;
		}

		public List<int> RemoveExcessGasha()
		{
			List<int> list = new List<int>();
			for (int i = this.gashaInfoList.Count - 1; i >= 0; i--)
			{
				int num = int.Parse(this.gashaInfoList[i].totalPlayLimitCount);
				int num2 = int.Parse(this.gashaInfoList[i].totalPlayCount);
				if (0 < num && num <= num2)
				{
					list.Add(i);
					this.gashaInfoList.RemoveAt(i);
				}
			}
			return list;
		}
	}
}
