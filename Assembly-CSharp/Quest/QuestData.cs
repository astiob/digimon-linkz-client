using PersistentData;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeSerialize;
using UnityEngine;
using WebAPIRequest;

namespace Quest
{
	public sealed class QuestData : ClassSingleton<QuestData>
	{
		private QuestEventInfoList eventInfoList;

		private CryptoHelper cryptoHelper = new CryptoHelper(null, null);

		private Dictionary<string, GameWebAPI.RespDataWD_GetDungeonInfo> dngDataCacheList = new Dictionary<string, GameWebAPI.RespDataWD_GetDungeonInfo>();

		private List<QuestData.WorldAreaData> worldAreaMList;

		private List<QuestData.WD_DngInfoData> WD_DngInfoDataList;

		private int WD_DngInfoStatusTutorial;

		private GameWebAPI.RespDataWD_DungeonStart resp_data_wd_dungeonstart;

		private GameWebAPI.RespDataWD_DungeonResult resp_data_wd_dungeonresult;

		private GameWebAPI.RespData_WorldMultiResultInfoLogic resp_data_world_multi_result_info_logic;

		private GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo wdi_bk;

		public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM SelectDungeon { get; set; }

		private QuestEventInfoList GetEventInfo()
		{
			if (null == this.eventInfoList)
			{
				this.eventInfoList = (AssetDataMng.Instance().LoadObject("Quest/List/QuestEventInfoList", null, false) as QuestEventInfoList);
			}
			return this.eventInfoList;
		}

		private string GetEncryptoDungeonId(string dungeonId)
		{
			byte[] array;
			TypeSerializeHelper.DataToBytes<string>(dungeonId, out array);
			array = this.cryptoHelper.EncryptWithDES(array);
			return Convert.ToBase64String(array);
		}

		public bool ExistEvent(string dungeonId)
		{
			bool result = false;
			QuestEventInfoList eventInfo = this.GetEventInfo();
			if (null != eventInfo)
			{
				string encryptoDungeonId = this.GetEncryptoDungeonId(dungeonId);
				result = eventInfo.ExistEvent(encryptoDungeonId);
			}
			return result;
		}

		public QuestEventInfoList.EventInfo GetEventInfo(string dungeonId)
		{
			QuestEventInfoList.EventInfo result = null;
			QuestEventInfoList eventInfo = this.GetEventInfo();
			if (null != eventInfo)
			{
				string encryptoDungeonId = this.GetEncryptoDungeonId(dungeonId);
				result = eventInfo.GetEventInfo(encryptoDungeonId);
			}
			return result;
		}

		public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM GetWorldDungeonMaster(string worldDungeonId)
		{
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM result = null;
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
			for (int i = 0; i < worldDungeonM.Length; i++)
			{
				if (worldDungeonId == worldDungeonM[i].worldDungeonId)
				{
					result = worldDungeonM[i];
					break;
				}
			}
			return result;
		}

		public List<GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy> GetBossMonsterList(GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy[] enemyList)
		{
			List<GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy> list = new List<GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy>();
			if (enemyList != null)
			{
				for (int i = 0; i < enemyList.Length; i++)
				{
					if (enemyList[i].type == 2 || enemyList[i].type == 4)
					{
						list.Add(enemyList[i]);
					}
				}
			}
			return list;
		}

		public bool ExistSortieLimit(int worldDungeonId)
		{
			bool result = false;
			GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit[] worldDungeonSortieLimitM = MasterDataMng.Instance().WorldDungeonSortieLimitMaster.worldDungeonSortieLimitM;
			string b = worldDungeonId.ToString();
			for (int i = 0; i < worldDungeonSortieLimitM.Length; i++)
			{
				if (worldDungeonSortieLimitM[i].worldDungeonId == b)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public bool CheckSortieLimit(List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList, string tribe, string growStep)
		{
			bool result = true;
			if (limitList != null && 0 < limitList.Count)
			{
				bool flag = false;
				for (int i = 0; i < limitList.Count; i++)
				{
					if (limitList[i].tribe == tribe && limitList[i].growStep == growStep)
					{
						flag = true;
						break;
					}
				}
				result = flag;
			}
			return result;
		}

		public void ClearWorldAreaMList()
		{
			this.worldAreaMList = null;
		}

		public List<QuestData.WorldAreaData> GetWorldAreaM_Normal()
		{
			if (this.worldAreaMList == null)
			{
				this.worldAreaMList = this.GetWorldAreaM("2", false);
			}
			return this.worldAreaMList;
		}

		public List<QuestData.WorldAreaData> GetWorldAreaM(string excludeType, bool includeTimeOut = false)
		{
			List<QuestData.WorldAreaData> list = new List<QuestData.WorldAreaData>();
			GameWebAPI.RespDataMA_GetWorldAreaM respDataMA_WorldAreaM = MasterDataMng.Instance().RespDataMA_WorldAreaM;
			for (int i = 0; i < respDataMA_WorldAreaM.worldAreaM.Length; i++)
			{
				if (respDataMA_WorldAreaM.worldAreaM[i].type != excludeType)
				{
					list.Add(new QuestData.WorldAreaData
					{
						data = respDataMA_WorldAreaM.worldAreaM[i],
						isActive = true
					});
				}
			}
			list.Sort(new Comparison<QuestData.WorldAreaData>(this.CompareTime));
			return list;
		}

		private int CompareTime(QuestData.WorldAreaData mx, QuestData.WorldAreaData my)
		{
			int num = int.Parse(mx.data.priority);
			int num2 = int.Parse(my.data.priority);
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

		public void InitWD_DngInfoDataList()
		{
			this.WD_DngInfoDataList = new List<QuestData.WD_DngInfoData>();
		}

		public bool IsWD_DngInfoDataExist()
		{
			return this.WD_DngInfoDataList != null && this.WD_DngInfoDataList.Count > 0;
		}

		public void ClearDNGDataCache()
		{
			this.dngDataCacheList.Clear();
		}

		public QuestData.WorldAreaData GetWorldAreaM_NormalByAreaId(string areaId)
		{
			List<QuestData.WorldAreaData> worldAreaM_Normal = this.GetWorldAreaM_Normal();
			for (int i = 0; i < worldAreaM_Normal.Count; i++)
			{
				if (worldAreaM_Normal[i].data.worldAreaId == areaId)
				{
					return worldAreaM_Normal[i];
				}
			}
			return null;
		}

		public void GetWorldDungeonInfo(List<string> worldIdList, Action<bool> completed)
		{
			if (this.worldAreaMList == null)
			{
				this.worldAreaMList = this.GetWorldAreaM("2", false);
			}
			this.InitWD_DngInfoDataList();
			List<RequestBase> list = new List<RequestBase>();
			for (int i = 0; i < worldIdList.Count; i++)
			{
				string worldId = worldIdList[i];
				if (worldId != null)
				{
					if (!(worldId == "1"))
					{
						if (!(worldId == "3"))
						{
							if (worldId == "8")
							{
								this.ResetWorldAreaActiveFlg("8");
								RequestBase item = new GameWebAPI.RequestWD_WorldTicketDungeonInfo
								{
									OnReceived = delegate(GameWebAPI.RespDataWD_GetDungeonInfo response)
									{
										this.AddWD_DngInfoDataList(worldId, response);
									}
								};
								list.Add(item);
							}
						}
						else
						{
							RequestBase item = new GameWebAPI.RequestWD_WorldEventDungeonInfo
							{
								OnReceived = delegate(GameWebAPI.RespDataWD_GetDungeonInfo response)
								{
									this.AddWD_DngInfoDataList(worldId, response);
								}
							};
							list.Add(item);
						}
					}
					else if (this.dngDataCacheList.ContainsKey(worldIdList[i]))
					{
						this.AddWD_DngInfoDataList(worldIdList[i], this.dngDataCacheList[worldIdList[i]]);
					}
					else
					{
						RequestBase item = new GameWebAPI.RequestWD_WorldDungeonInfo
						{
							SetSendData = delegate(GameWebAPI.WD_Req_DngInfo param)
							{
								param.worldId = worldId;
							},
							OnReceived = delegate(GameWebAPI.RespDataWD_GetDungeonInfo response)
							{
								this.AddWD_DngInfoDataList(worldId, response);
								this.dngDataCacheList.Add(worldId, response);
							}
						};
						list.Add(item);
					}
				}
			}
			if (list.Count > 0)
			{
				this.Request(list, 0, completed);
			}
			else
			{
				completed(false);
			}
		}

		private void Request(List<RequestBase> list, int index, Action<bool> completed)
		{
			if (list.Count - 1 == index)
			{
				this.Request(list[index], completed);
			}
			else
			{
				this.Request(list[index], delegate(bool flg)
				{
					if (flg)
					{
						this.Request(list, ++index, completed);
					}
					else
					{
						completed(false);
					}
				});
			}
		}

		private void Request(RequestBase request, Action<bool> completed)
		{
			AppCoroutine.Start(request.RunOneTime(delegate()
			{
				completed(true);
			}, delegate(Exception noop)
			{
				completed(false);
			}, null), false);
		}

		public void AddWD_DngInfoDataList(string worldId, GameWebAPI.RespDataWD_GetDungeonInfo dngInfo)
		{
			if (worldId == "1")
			{
				QuestData.WD_DngInfoData wd_DngInfoData = new QuestData.WD_DngInfoData();
				wd_DngInfoData.worldAreaId = "1";
				wd_DngInfoData.dngInfo = dngInfo;
				this.WD_DngInfoDataList.Add(wd_DngInfoData);
				for (int i = 0; i < wd_DngInfoData.dngInfo.worldDungeonInfo.Length; i++)
				{
					wd_DngInfoData.dngInfo.worldDungeonInfo[i].isEvent = false;
				}
			}
			else if (worldId == "3")
			{
				List<QuestData.WorldAreaData> wam = this.GetWorldAreaM_Normal();
				GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
				List<QuestData.WorldAreaData> list = new List<QuestData.WorldAreaData>();
				int z;
				for (z = 0; z < wam.Count; z++)
				{
					if (!(wam[z].data.worldAreaId == "1") && !(wam[z].data.worldAreaId == "8"))
					{
						string worldAreaId = wam[z].data.worldAreaId;
						QuestData.WD_DngInfoData wd_DngInfoData2 = new QuestData.WD_DngInfoData();
						wd_DngInfoData2.worldAreaId = worldAreaId;
						wd_DngInfoData2.dngInfo = new GameWebAPI.RespDataWD_GetDungeonInfo();
						List<GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo> list2 = new List<GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo>();
						for (int j = 0; j < dngInfo.worldDungeonInfo.Length; j++)
						{
							string b = dngInfo.worldDungeonInfo[j].worldStageId.ToString();
							for (int k = 0; k < worldStageM.Length; k++)
							{
								if (worldStageM[k].worldStageId == b && worldStageM[k].worldAreaId == wd_DngInfoData2.worldAreaId)
								{
									list2.Add(dngInfo.worldDungeonInfo[j]);
								}
							}
						}
						wd_DngInfoData2.dngInfo.worldDungeonInfo = new GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo[list2.Count];
						for (int j = 0; j < list2.Count; j++)
						{
							wd_DngInfoData2.dngInfo.worldDungeonInfo[j] = list2[j];
						}
						this.WD_DngInfoDataList.Add(wd_DngInfoData2);
						bool flag = false;
						if (list2.Count > 0)
						{
							wam[z].isActive = true;
							if (list2[0].timeLeft <= 0)
							{
								flag = true;
							}
						}
						else
						{
							wam[z].isActive = false;
							flag = true;
						}
						if (flag)
						{
							GameWebAPI.RespDataMA_WorldEventAreaMaster respDataMA_WorldEventAreaMaster = MasterDataMng.Instance().RespDataMA_WorldEventAreaMaster;
							GameWebAPI.RespDataMA_WorldEventMaster respDataMA_WorldEventMaster = MasterDataMng.Instance().RespDataMA_WorldEventMaster;
							if (wam[z].data.type == "3")
							{
								GameWebAPI.RespDataMA_WorldEventAreaMaster.WorldEventAreaM areaMaster = respDataMA_WorldEventAreaMaster.worldEventAreaM.SingleOrDefault((GameWebAPI.RespDataMA_WorldEventAreaMaster.WorldEventAreaM x) => x.worldAreaId == wam[z].data.worldAreaId);
								GameWebAPI.RespDataMA_WorldEventMaster.WorldEventM worldEventM = respDataMA_WorldEventMaster.worldEventM.Where((GameWebAPI.RespDataMA_WorldEventMaster.WorldEventM v) => v.worldEventId == areaMaster.worldEventId).First<GameWebAPI.RespDataMA_WorldEventMaster.WorldEventM>();
								if (worldEventM != null)
								{
									wam[z].isActive = (DateTime.Parse(worldEventM.receiveEndTime) > ServerDateTime.Now);
									if (wam[z].isActive)
									{
										list.Add(wam[z]);
									}
								}
							}
						}
						for (int l = 0; l < wd_DngInfoData2.dngInfo.worldDungeonInfo.Length; l++)
						{
							wd_DngInfoData2.dngInfo.worldDungeonInfo[l].isEvent = true;
						}
					}
				}
				foreach (QuestData.WorldAreaData item in list)
				{
					wam.Remove(item);
					wam.Add(item);
				}
				list.Clear();
			}
			else if (worldId == "8")
			{
				string text = "8";
				List<QuestData.WorldAreaData> worldAreaM_Normal = this.GetWorldAreaM_Normal();
				int m;
				for (m = 0; m < worldAreaM_Normal.Count; m++)
				{
					if (worldAreaM_Normal[m].data.worldAreaId == text)
					{
						break;
					}
				}
				if (m == worldAreaM_Normal.Count)
				{
					worldAreaM_Normal.Add(new QuestData.WorldAreaData
					{
						data = new GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM(),
						data = 
						{
							worldAreaId = text
						}
					});
				}
				QuestData.WD_DngInfoData wd_DngInfoData3 = new QuestData.WD_DngInfoData();
				wd_DngInfoData3.worldAreaId = text;
				wd_DngInfoData3.dngInfo = new GameWebAPI.RespDataWD_GetDungeonInfo();
				wd_DngInfoData3.dngInfo.worldDungeonInfo = new GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo[dngInfo.worldDungeonInfo.Length];
				for (int n = 0; n < dngInfo.worldDungeonInfo.Length; n++)
				{
					GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo worldDungeonInfo = dngInfo.worldDungeonInfo[n];
					worldDungeonInfo.sortIdx = n;
					worldDungeonInfo.totalTicketNum = 0;
					for (int num = 0; num < worldDungeonInfo.dungeons.Length; num++)
					{
						if (!string.IsNullOrEmpty(worldDungeonInfo.dungeons[num].dungeonTicketNum))
						{
							worldDungeonInfo.totalTicketNum += int.Parse(worldDungeonInfo.dungeons[num].dungeonTicketNum);
						}
					}
					wd_DngInfoData3.dngInfo.worldDungeonInfo[n] = worldDungeonInfo;
				}
				if (dngInfo.worldDungeonInfo.Length > 0)
				{
					worldAreaM_Normal[m].isActive = true;
				}
				else
				{
					worldAreaM_Normal[m].isActive = false;
				}
				this.WD_DngInfoDataList.Add(wd_DngInfoData3);
				for (int num2 = 0; num2 < wd_DngInfoData3.dngInfo.worldDungeonInfo.Length; num2++)
				{
					wd_DngInfoData3.dngInfo.worldDungeonInfo[num2].isEvent = false;
				}
			}
			else if (worldId == "4")
			{
				this.WD_DngInfoStatusTutorial = 3;
				if (dngInfo.worldDungeonInfo != null)
				{
					for (int num3 = 0; num3 < dngInfo.worldDungeonInfo.Length; num3++)
					{
						GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] dungeons = dngInfo.worldDungeonInfo[num3].dungeons;
						if (dungeons != null)
						{
							for (int num4 = 0; num4 < dungeons.Length; num4++)
							{
								if (dungeons[num4].worldDungeonId == 9001)
								{
									this.WD_DngInfoStatusTutorial = dungeons[num4].status;
									break;
								}
							}
							break;
						}
					}
				}
			}
			else
			{
				global::Debug.LogError("======================================================= AddWD_DngInfoDataList INVALIT WORLD ID");
			}
		}

		public void ResetWorldAreaActiveFlg(string areaID)
		{
			List<QuestData.WorldAreaData> worldAreaM_Normal = this.GetWorldAreaM_Normal();
			for (int i = 0; i < worldAreaM_Normal.Count; i++)
			{
				if (worldAreaM_Normal[i].data.worldAreaId == areaID)
				{
					worldAreaM_Normal[i].isActive = false;
				}
			}
		}

		public List<QuestData.WD_DngInfoData> GetWD_DngInfoDataList()
		{
			return this.WD_DngInfoDataList;
		}

		public bool IsTutorialDungeonClear()
		{
			return this.WD_DngInfoStatusTutorial == 4;
		}

		public GameWebAPI.RespDataWD_GetDungeonInfo GetDngeonInfoByWorldAreaId(string worldAreaId)
		{
			if (this.WD_DngInfoDataList == null)
			{
				return null;
			}
			for (int i = 0; i < this.WD_DngInfoDataList.Count; i++)
			{
				if (this.WD_DngInfoDataList[i].worldAreaId == worldAreaId)
				{
					return this.WD_DngInfoDataList[i].dngInfo;
				}
			}
			return null;
		}

		public GameWebAPI.RespDataWD_DungeonStart RespDataWD_DungeonStart
		{
			get
			{
				return this.resp_data_wd_dungeonstart;
			}
			set
			{
				if (value != null)
				{
					this.resp_data_wd_dungeonstart = value;
				}
			}
		}

		public GameWebAPI.RespDataWD_DungeonResult RespDataWD_DungeonResult
		{
			get
			{
				return this.resp_data_wd_dungeonresult;
			}
			set
			{
				this.resp_data_wd_dungeonresult = value;
			}
		}

		public GameWebAPI.RespData_WorldMultiResultInfoLogic RespData_WorldMultiResultInfoLogic
		{
			get
			{
				return this.resp_data_world_multi_result_info_logic;
			}
			set
			{
				this.resp_data_world_multi_result_info_logic = value;
			}
		}

		public List<QuestData.WorldStageData> GetWorldStageData_ByAreaID(string worldAreaId)
		{
			List<QuestData.WorldStageData> list = new List<QuestData.WorldStageData>();
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldAreaId = this.GetDngeonInfoByWorldAreaId(worldAreaId);
			for (int i = 0; i < worldStageM.Length; i++)
			{
				if (worldStageM[i].worldAreaId == worldAreaId)
				{
					int num = 0;
					List<QuestData.WorldDungeonData> worldDungeonData_ByAreaIdStageId;
					do
					{
						worldDungeonData_ByAreaIdStageId = this.GetWorldDungeonData_ByAreaIdStageId(worldAreaId, worldStageM[i].worldStageId, dngeonInfoByWorldAreaId, num, true, false);
						num++;
						if (worldDungeonData_ByAreaIdStageId != null && worldDungeonData_ByAreaIdStageId.Count > 0)
						{
							int num2 = (int)this.CheckStageStatus(worldDungeonData_ByAreaIdStageId);
							bool flag = this.ExistsCondition(worldDungeonData_ByAreaIdStageId);
							if (num2 > 1 || flag)
							{
								QuestData.WorldStageData worldStageData = new QuestData.WorldStageData();
								worldStageData.worldStageM = worldStageM[i];
								worldStageData.wddL = new List<QuestData.WorldDungeonData>();
								for (int j = 0; j < worldDungeonData_ByAreaIdStageId.Count; j++)
								{
									if (worldDungeonData_ByAreaIdStageId[j].status > 1 || (worldDungeonData_ByAreaIdStageId[j].wdscMList != null && worldDungeonData_ByAreaIdStageId[j].wdscMList.Count > 0))
									{
										worldStageData.wddL.Add(worldDungeonData_ByAreaIdStageId[j]);
									}
								}
								worldStageData.wdi = this.GetLastWorldDungeonInfo;
								worldStageData.status = num2;
								worldStageData.worldStageM.closeTime = ServerDateTime.Now.AddSeconds((double)worldDungeonData_ByAreaIdStageId[0].remainingTime).ToString();
								worldStageData.dngClearCount = 0;
								for (int k = 0; k < worldDungeonData_ByAreaIdStageId.Count; k++)
								{
									if (worldDungeonData_ByAreaIdStageId[k].status == 4)
									{
										worldStageData.dngClearCount++;
									}
								}
								worldStageData.dngCount = worldDungeonData_ByAreaIdStageId.Count;
								worldStageData.isViewRanking = worldDungeonData_ByAreaIdStageId[0].isViewRanking;
								worldStageData.isCounting = worldDungeonData_ByAreaIdStageId[0].isCounting;
								list.Add(worldStageData);
							}
						}
					}
					while (worldDungeonData_ByAreaIdStageId != null);
				}
			}
			if (worldAreaId == "8")
			{
				list.Sort(new Comparison<QuestData.WorldStageData>(this.CompareAccountTicketIdx));
			}
			else
			{
				list.Sort(new Comparison<QuestData.WorldStageData>(this.CompareAccountStageId));
			}
			return list;
		}

		private int CompareAccountStageId(QuestData.WorldStageData a, QuestData.WorldStageData b)
		{
			int num = int.Parse(a.worldStageM.worldStageId);
			int num2 = int.Parse(b.worldStageM.worldStageId);
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

		private int CompareAccountTicketIdx(QuestData.WorldStageData a, QuestData.WorldStageData b)
		{
			int sortIdx = a.wdi.sortIdx;
			int sortIdx2 = b.wdi.sortIdx;
			if (sortIdx < sortIdx2)
			{
				return -1;
			}
			if (sortIdx > sortIdx2)
			{
				return 1;
			}
			return 0;
		}

		public bool IsUnlockWorldStage(QuestData.WorldStageData wsd)
		{
			bool result = false;
			List<QuestData.WorldDungeonData> wddL = wsd.wddL;
			if (wddL != null)
			{
				result = (this.CheckStageStatus(wddL) > QuestData.WORLD_STATUS.LOCK);
			}
			return result;
		}

		private QuestData.WORLD_STATUS CheckStageStatus(List<QuestData.WorldDungeonData> wddL)
		{
			if (wddL.Count <= 0)
			{
				return QuestData.WORLD_STATUS.LOCK;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < wddL.Count; i++)
			{
				switch (wddL[i].status)
				{
				case 1:
					num++;
					break;
				case 2:
					num2++;
					break;
				case 3:
					num3++;
					break;
				case 4:
					num4++;
					break;
				}
			}
			if (num >= wddL.Count)
			{
				return QuestData.WORLD_STATUS.LOCK;
			}
			if (num4 >= wddL.Count)
			{
				return QuestData.WORLD_STATUS.UNLOCK_CLEAR;
			}
			if (num2 >= 1)
			{
				return QuestData.WORLD_STATUS.UNLOCK_NEW;
			}
			return QuestData.WORLD_STATUS.UNLOCK;
		}

		private bool ExistsCondition(List<QuestData.WorldDungeonData> worldDungeons)
		{
			bool result = false;
			foreach (QuestData.WorldDungeonData worldDungeonData in worldDungeons)
			{
				if (worldDungeonData.wdscMList != null && 0 < worldDungeonData.wdscMList.Count)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public QuestData.WORLD_STATUS CheckAreaStatus(List<QuestData.WorldStageData> wsdL, string AreaID)
		{
			if (wsdL.Count <= 0)
			{
				return QuestData.WORLD_STATUS.LOCK;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < wsdL.Count; i++)
			{
				switch (wsdL[i].status)
				{
				case 1:
					num++;
					break;
				case 2:
					num2++;
					break;
				case 3:
					num3++;
					break;
				case 4:
					num4++;
					break;
				}
			}
			if (num >= wsdL.Count)
			{
				return QuestData.WORLD_STATUS.LOCK;
			}
			if (num4 >= wsdL.Count)
			{
				return QuestData.WORLD_STATUS.UNLOCK_CLEAR;
			}
			if (num2 >= 1)
			{
				return QuestData.WORLD_STATUS.UNLOCK_NEW;
			}
			return QuestData.WORLD_STATUS.UNLOCK;
		}

		private GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo GetLastWorldDungeonInfo
		{
			get
			{
				return this.wdi_bk;
			}
		}

		public List<QuestData.WorldDungeonData> GetWorldDungeonData_ByAreaIdStageId(string worldAreaId, string worldStageId, GameWebAPI.RespDataWD_GetDungeonInfo dungeonInfo, int targetCount = 0, bool addLock = false, bool addEmpty = false)
		{
			List<QuestData.WorldDungeonData> list = new List<QuestData.WorldDungeonData>();
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
			List<GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM> list2 = new List<GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM>();
			if (dungeonInfo == null)
			{
				return null;
			}
			for (int i = 0; i < worldDungeonM.Length; i++)
			{
				if (worldDungeonM[i].worldStageId == worldStageId)
				{
					list2.Add(worldDungeonM[i]);
				}
			}
			if (dungeonInfo.worldDungeonInfo == null)
			{
				return list;
			}
			GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] array = null;
			GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo worldDungeonInfo = null;
			this.wdi_bk = null;
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			for (int j = 0; j < dungeonInfo.worldDungeonInfo.Length; j++)
			{
				if (dungeonInfo.worldDungeonInfo[j].worldStageId == int.Parse(worldStageId))
				{
					if (targetCount == num)
					{
						worldDungeonInfo = dungeonInfo.worldDungeonInfo[j];
						this.wdi_bk = worldDungeonInfo;
						array = worldDungeonInfo.dungeons;
						if (array == null && worldDungeonInfo.timeLeft == 0)
						{
							flag = true;
						}
						flag2 = true;
						break;
					}
					num++;
				}
			}
			if (flag2)
			{
				if (flag)
				{
					QuestData.WorldDungeonData item = new QuestData.WorldDungeonData
					{
						stageId = worldDungeonInfo.worldStageId,
						worldDungeonM = null,
						dungeon = null,
						wdscMList = null,
						isViewRanking = true,
						isCounting = (1 == worldDungeonInfo.isCounting)
					};
					list.Add(item);
				}
				else
				{
					for (int k = 0; k < list2.Count; k++)
					{
						for (int l = 0; l < array.Length; l++)
						{
							if (int.Parse(list2[k].worldDungeonId) == array[l].worldDungeonId && (addEmpty || !this.IsEmptyDng(array[l], worldAreaId)) && (addLock || array[l].status > 1))
							{
								list.Add(new QuestData.WorldDungeonData
								{
									stageId = worldDungeonInfo.worldStageId,
									worldDungeonM = list2[k],
									dungeon = array[l],
									status = array[l].status,
									wdscMList = this.GetWorldDungeonStartConditionM(array[l].worldDungeonId.ToString()),
									remainingTime = worldDungeonInfo.timeLeft
								});
							}
						}
					}
				}
				list.Sort(new Comparison<QuestData.WorldDungeonData>(this.CompareAccountDungeonId));
				return list;
			}
			return null;
		}

		private int CompareAccountDungeonId(QuestData.WorldDungeonData a, QuestData.WorldDungeonData b)
		{
			int num = int.Parse(a.worldDungeonM.worldDungeonId);
			int num2 = int.Parse(b.worldDungeonM.worldDungeonId);
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

		public bool IsEmptyDng(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng, string worldAreaId)
		{
			if (worldAreaId == "8")
			{
				int num = int.Parse(dng.dungeonTicketNum);
				if (num <= 0)
				{
					return true;
				}
			}
			else if (dng.playLimit != null)
			{
				int num = int.Parse(dng.playLimit.restCount);
				if (num <= 0 && dng.playLimit.dailyResetFlg == "0" && dng.playLimit.recoveryFlg == "0")
				{
					return true;
				}
			}
			return false;
		}

		public GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons GetTicketQuestDungeonByTicketID(string id)
		{
			GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldAreaId = this.GetDngeonInfoByWorldAreaId("8");
			if (dngeonInfoByWorldAreaId == null)
			{
				return null;
			}
			for (int i = 0; i < dngeonInfoByWorldAreaId.worldDungeonInfo.Length; i++)
			{
				GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] dungeons = dngeonInfoByWorldAreaId.worldDungeonInfo[i].dungeons;
				for (int j = 0; j < dungeons.Length; j++)
				{
					if (!string.IsNullOrEmpty(dungeons[j].userDungeonTicketId) && dungeons[j].userDungeonTicketId == id)
					{
						return dungeons[j];
					}
				}
			}
			return null;
		}

		public List<GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM> GetWorldDungeonStartConditionM(string worldDungeonId)
		{
			List<GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM> list = new List<GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM>();
			GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM[] worldDungeonStartConditionM = MasterDataMng.Instance().RespDataMA_WorldDungeonStartCondition.worldDungeonStartConditionM;
			for (int i = 0; i < worldDungeonStartConditionM.Length; i++)
			{
				if (worldDungeonStartConditionM[i].worldDungeonId == worldDungeonId)
				{
					list.Add(worldDungeonStartConditionM[i]);
				}
			}
			return list;
		}

		public List<QuestData.DSCondition_AND_Data> GetDngStartCondition_OR_DataList(List<GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM> wscList)
		{
			List<QuestData.DSCondition_AND_Data> list = new List<QuestData.DSCondition_AND_Data>();
			for (int i = 0; i < wscList.Count; i++)
			{
				QuestData.DSCondition_AND_Data dscondition_AND_Data = new QuestData.DSCondition_AND_Data();
				dscondition_AND_Data.DSCondition_AND_List = new List<QuestData.DSConditionData>();
				string[] array = new string[3];
				string[] array2 = new string[3];
				array[0] = wscList[i].preDungeonId1;
				array[1] = wscList[i].preDungeonId2;
				array[2] = wscList[i].preDungeonId3;
				array2[0] = wscList[i].preDungeonCount1;
				array2[1] = wscList[i].preDungeonCount2;
				array2[2] = wscList[i].preDungeonCount3;
				for (int j = 0; j < 3; j++)
				{
					if (!string.IsNullOrEmpty(array[j]) && !string.IsNullOrEmpty(array2[j]) && array[j] != "0")
					{
						QuestData.DSConditionData dsconditionData = new QuestData.DSConditionData();
						dsconditionData.preDngID = array[j];
						GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
						for (int k = 0; k < worldDungeonM.Length; k++)
						{
							if (worldDungeonM[k].worldDungeonId == dsconditionData.preDngID)
							{
								dsconditionData.dngM = worldDungeonM[k];
							}
						}
						dsconditionData.clearCT = int.Parse(array2[j]);
						dscondition_AND_Data.DSCondition_AND_List.Add(dsconditionData);
					}
				}
				list.Add(dscondition_AND_Data);
			}
			return list;
		}

		public Coroutine DungeonContinue(int QuestStartID, int FloorNum, int RoundNum, int[] ContinueMonsterIDs, Action<bool> Result)
		{
			bool isSuccess = false;
			GameWebAPI.RequestWD_Continue request = new GameWebAPI.RequestWD_Continue
			{
				SetSendData = delegate(GameWebAPI.WD_Req_Continue param)
				{
					param.startId = QuestStartID;
					param.floorNum = FloorNum;
					param.roundNum = RoundNum;
					param.userMonsterId = ContinueMonsterIDs;
				},
				OnReceived = delegate(GameWebAPI.RespDataWD_Continue response)
				{
					isSuccess = response.IsSuccess();
				}
			};
			return AppCoroutine.Start(request.Run(delegate()
			{
				if (Result != null)
				{
					Result(isSuccess);
				}
			}, null, null), false);
		}

		public Coroutine DungeonContinueMulti(int QuestStartID, int FloorNum, int RoundNum, int[] ContinueMonsterIDs, Action<bool> Result)
		{
			bool isSuccess = false;
			GameWebAPI.WorldMultiDungeonContinueLogic request = new GameWebAPI.WorldMultiDungeonContinueLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_WorldMultiDungeonContinueLogic param)
				{
					param.startId = QuestStartID;
					param.floorNum = FloorNum;
					param.roundNum = RoundNum;
					param.userMonsterId = ContinueMonsterIDs;
				},
				OnReceived = delegate(GameWebAPI.RespData_WorldMultiDungeonContinueLogic response)
				{
					isSuccess = response.IsSuccess();
				}
			};
			return AppCoroutine.Start(request.Run(delegate()
			{
				if (Result != null)
				{
					Result(isSuccess);
				}
			}, null, null), false);
		}

		public APIRequestTask RequestPointQuestInfo(string worldAreaId, Action<GameWebAPI.RespDataWD_PointQuestInfo> onResponse, bool requestRetry = true)
		{
			GameWebAPI.PointQuestInfo request = new GameWebAPI.PointQuestInfo
			{
				SetSendData = delegate(GameWebAPI.ReqDataWD_PointQuestInfo param)
				{
					param.worldAreaId = int.Parse(worldAreaId);
				},
				OnReceived = onResponse
			};
			return new APIRequestTask(request, requestRetry);
		}

		public static void CreateDropAssetList(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons questStage, List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset> dropAssetList)
		{
			int i;
			for (i = 0; i < questStage.dropAssets.Length; i++)
			{
				int assetCategoryId = questStage.dropAssets[i].assetCategoryId;
				if (assetCategoryId != 4 && assetCategoryId != 5 && !dropAssetList.Any((GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset x) => x.assetCategoryId == assetCategoryId && x.assetValue == questStage.dropAssets[i].assetValue))
				{
					dropAssetList.Add(questStage.dropAssets[i]);
				}
			}
		}

		public static GameWebAPI.ResponseWorldStageForceOpenMaster.ForceOpen GetQuestForceOpen(int worldStageId)
		{
			GameWebAPI.ResponseWorldStageForceOpenMaster.ForceOpen result = null;
			GameWebAPI.ResponseWorldStageForceOpenMaster worldStageForceOpenMaster = MasterDataMng.Instance().WorldStageForceOpenMaster;
			for (int i = 0; i < worldStageForceOpenMaster.worldStageForceOpenM.Length; i++)
			{
				if (worldStageForceOpenMaster.worldStageForceOpenM[i].worldStageId == worldStageId)
				{
					result = worldStageForceOpenMaster.worldStageForceOpenM[i];
					break;
				}
			}
			return result;
		}

		public static bool IsTicketQuest(string worldAreaId)
		{
			return "8" == worldAreaId;
		}

		public enum WORLD_STATUS
		{
			LOCK = 1,
			UNLOCK_NEW,
			UNLOCK,
			UNLOCK_CLEAR
		}

		public class WorldAreaData
		{
			public GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM data;

			public bool isActive;
		}

		public class WD_DngInfoData
		{
			public string worldAreaId;

			public GameWebAPI.RespDataWD_GetDungeonInfo dngInfo;
		}

		public class WorldStageData
		{
			public GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM;

			public GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo wdi;

			public List<QuestData.WorldDungeonData> wddL;

			public int status;

			public int dngCount;

			public int dngClearCount;

			public string closeTime;

			public bool isViewRanking;

			public bool isCounting;
		}

		public class WorldDungeonData
		{
			public int stageId;

			public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM;

			public GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon;

			public int status;

			public List<GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM> wdscMList;

			public int remainingTime;

			public bool isViewRanking;

			public bool isCounting;
		}

		public class DSConditionData
		{
			public string preDngID;

			public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM dngM;

			public int clearCT;
		}

		public class DSCondition_AND_Data
		{
			public List<QuestData.DSConditionData> DSCondition_AND_List;
		}
	}
}
