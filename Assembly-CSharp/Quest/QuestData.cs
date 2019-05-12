using PersistentData;
using System;
using System.Collections.Generic;
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
				this.worldAreaMList = this.GetWorldAreaM("2", true, false);
			}
			return this.worldAreaMList;
		}

		public List<QuestData.WorldAreaData> GetWorldAreaM(string type, bool omit = true, bool includeTimeOut = false)
		{
			List<QuestData.WorldAreaData> list = new List<QuestData.WorldAreaData>();
			GameWebAPI.RespDataMA_GetWorldAreaM respDataMA_WorldAreaM = MasterDataMng.Instance().RespDataMA_WorldAreaM;
			for (int i = 0; i < respDataMA_WorldAreaM.worldAreaM.Length; i++)
			{
				if ((!omit && respDataMA_WorldAreaM.worldAreaM[i].type == type) || (omit && respDataMA_WorldAreaM.worldAreaM[i].type != type))
				{
					if (includeTimeOut)
					{
						list.Add(new QuestData.WorldAreaData
						{
							data = respDataMA_WorldAreaM.worldAreaM[i],
							isActive = true
						});
					}
					else
					{
						DateTime dateTime = DateTime.Parse(respDataMA_WorldAreaM.worldAreaM[i].openTime);
						DateTime value = DateTime.Parse(respDataMA_WorldAreaM.worldAreaM[i].closeTime);
						DateTime now = ServerDateTime.Now;
						if (dateTime.CompareTo(now) <= 0 && now.CompareTo(value) <= 0)
						{
							list.Add(new QuestData.WorldAreaData
							{
								data = respDataMA_WorldAreaM.worldAreaM[i],
								isActive = true
							});
						}
					}
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
				this.worldAreaMList = this.GetWorldAreaM("2", true, false);
			}
			this.InitWD_DngInfoDataList();
			RequestList requestList = new RequestList();
			for (int i = 0; i < worldIdList.Count; i++)
			{
				string worldId = worldIdList[i];
				string worldId2 = worldId;
				switch (worldId2)
				{
				case "1":
					if (this.dngDataCacheList.ContainsKey(worldIdList[i]))
					{
						this.AddWD_DngInfoDataList(worldIdList[i], this.dngDataCacheList[worldIdList[i]]);
					}
					else
					{
						RequestBase addRequest = new GameWebAPI.RequestWD_WorldDungeonInfo
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
						requestList.AddRequest(addRequest);
					}
					break;
				case "3":
				{
					RequestBase addRequest = new GameWebAPI.RequestWD_WorldEventDungeonInfo
					{
						OnReceived = delegate(GameWebAPI.RespDataWD_GetDungeonInfo response)
						{
							this.AddWD_DngInfoDataList(worldId, response);
						}
					};
					requestList.AddRequest(addRequest);
					break;
				}
				case "8":
				{
					this.ResetWorldAreaActiveFlg("8");
					RequestBase addRequest = new GameWebAPI.RequestWD_WorldTicketDungeonInfo
					{
						OnReceived = delegate(GameWebAPI.RespDataWD_GetDungeonInfo response)
						{
							this.AddWD_DngInfoDataList(worldId, response);
						}
					};
					requestList.AddRequest(addRequest);
					break;
				}
				}
			}
			AppCoroutine.Start(requestList.RunOneTime(delegate()
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
				wd_DngInfoData.worldId = worldId;
				wd_DngInfoData.dngInfo = dngInfo;
				this.WD_DngInfoDataList.Add(wd_DngInfoData);
				for (int i = 0; i < wd_DngInfoData.dngInfo.worldDungeonInfo.Length; i++)
				{
					wd_DngInfoData.dngInfo.worldDungeonInfo[i].isEvent = false;
				}
			}
			else if (worldId == "3")
			{
				List<QuestData.WorldAreaData> worldAreaM_Normal = this.GetWorldAreaM_Normal();
				GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
				for (int j = 0; j < worldAreaM_Normal.Count; j++)
				{
					if (!(worldAreaM_Normal[j].data.worldAreaId == "1") && !(worldAreaM_Normal[j].data.worldAreaId == "8"))
					{
						string worldAreaId = worldAreaM_Normal[j].data.worldAreaId;
						QuestData.WD_DngInfoData wd_DngInfoData2 = new QuestData.WD_DngInfoData();
						wd_DngInfoData2.worldId = worldAreaId;
						wd_DngInfoData2.dngInfo = new GameWebAPI.RespDataWD_GetDungeonInfo();
						List<GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo> list = new List<GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo>();
						for (int k = 0; k < dngInfo.worldDungeonInfo.Length; k++)
						{
							for (int l = 0; l < worldStageM.Length; l++)
							{
								if (worldStageM[l].worldStageId == dngInfo.worldDungeonInfo[k].worldStageId.ToString() && worldStageM[l].worldAreaId == wd_DngInfoData2.worldId)
								{
									list.Add(dngInfo.worldDungeonInfo[k]);
								}
							}
						}
						wd_DngInfoData2.dngInfo.worldDungeonInfo = new GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo[list.Count];
						for (int k = 0; k < list.Count; k++)
						{
							wd_DngInfoData2.dngInfo.worldDungeonInfo[k] = list[k];
						}
						this.WD_DngInfoDataList.Add(wd_DngInfoData2);
						if (list.Count > 0)
						{
							worldAreaM_Normal[j].isActive = true;
						}
						else
						{
							worldAreaM_Normal[j].isActive = false;
						}
						for (int m = 0; m < wd_DngInfoData2.dngInfo.worldDungeonInfo.Length; m++)
						{
							wd_DngInfoData2.dngInfo.worldDungeonInfo[m].isEvent = true;
						}
					}
				}
			}
			else if (worldId == "8")
			{
				string text = "8";
				List<QuestData.WorldAreaData> worldAreaM_Normal2 = this.GetWorldAreaM_Normal();
				int n;
				for (n = 0; n < worldAreaM_Normal2.Count; n++)
				{
					if (worldAreaM_Normal2[n].data.worldAreaId == text)
					{
						break;
					}
				}
				if (n == worldAreaM_Normal2.Count)
				{
					worldAreaM_Normal2.Add(new QuestData.WorldAreaData
					{
						data = new GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM(),
						data = 
						{
							worldAreaId = text
						}
					});
				}
				QuestData.WD_DngInfoData wd_DngInfoData3 = new QuestData.WD_DngInfoData();
				wd_DngInfoData3.worldId = text;
				wd_DngInfoData3.dngInfo = new GameWebAPI.RespDataWD_GetDungeonInfo();
				wd_DngInfoData3.dngInfo.worldDungeonInfo = new GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo[dngInfo.worldDungeonInfo.Length];
				for (int num = 0; num < dngInfo.worldDungeonInfo.Length; num++)
				{
					GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo worldDungeonInfo = dngInfo.worldDungeonInfo[num];
					worldDungeonInfo.sortIdx = num;
					worldDungeonInfo.totalTicketNum = 0;
					for (int num2 = 0; num2 < worldDungeonInfo.dungeons.Length; num2++)
					{
						if (!string.IsNullOrEmpty(worldDungeonInfo.dungeons[num2].dungeonTicketNum))
						{
							worldDungeonInfo.totalTicketNum += int.Parse(worldDungeonInfo.dungeons[num2].dungeonTicketNum);
						}
					}
					wd_DngInfoData3.dngInfo.worldDungeonInfo[num] = worldDungeonInfo;
				}
				if (dngInfo.worldDungeonInfo.Length > 0)
				{
					worldAreaM_Normal2[n].isActive = true;
				}
				else
				{
					worldAreaM_Normal2[n].isActive = false;
				}
				this.WD_DngInfoDataList.Add(wd_DngInfoData3);
				for (int num3 = 0; num3 < wd_DngInfoData3.dngInfo.worldDungeonInfo.Length; num3++)
				{
					wd_DngInfoData3.dngInfo.worldDungeonInfo[num3].isEvent = false;
				}
			}
			else if (worldId == "4")
			{
				this.WD_DngInfoStatusTutorial = 3;
				if (dngInfo.worldDungeonInfo != null)
				{
					for (int num4 = 0; num4 < dngInfo.worldDungeonInfo.Length; num4++)
					{
						GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] dungeons = dngInfo.worldDungeonInfo[num4].dungeons;
						if (dungeons != null)
						{
							for (int num5 = 0; num5 < dungeons.Length; num5++)
							{
								if (dungeons[num5].worldDungeonId == 9001)
								{
									this.WD_DngInfoStatusTutorial = dungeons[num5].status;
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

		public GameWebAPI.RespDataWD_GetDungeonInfo GetDngeonInfoByWorldId(string worldId)
		{
			if (this.WD_DngInfoDataList == null)
			{
				return null;
			}
			for (int i = 0; i < this.WD_DngInfoDataList.Count; i++)
			{
				if (this.WD_DngInfoDataList[i].worldId == worldId)
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

		public List<QuestData.WorldStageData> GetWorldStageData_ByAreaID(string id, bool addLock = false)
		{
			List<QuestData.WorldStageData> list = new List<QuestData.WorldStageData>();
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			for (int i = 0; i < worldStageM.Length; i++)
			{
				if (worldStageM[i].worldAreaId == id)
				{
					int num = 0;
					List<QuestData.WorldDungeonData> worldDungeonData_ByAreaIdStageId;
					do
					{
						worldDungeonData_ByAreaIdStageId = this.GetWorldDungeonData_ByAreaIdStageId(id, worldStageM[i].worldStageId, num, true, false);
						num++;
						if (worldDungeonData_ByAreaIdStageId != null && worldDungeonData_ByAreaIdStageId.Count > 0)
						{
							int num2 = (int)this.CheckStageStatus(worldDungeonData_ByAreaIdStageId);
							int stageStartConditionCount = this.GetStageStartConditionCount(worldDungeonData_ByAreaIdStageId);
							if (addLock || num2 > 1 || stageStartConditionCount > 0)
							{
								QuestData.WorldStageData worldStageData = new QuestData.WorldStageData();
								worldStageData.worldStageM = worldStageM[i];
								worldStageData.wddL = new List<QuestData.WorldDungeonData>();
								for (int j = 0; j < worldDungeonData_ByAreaIdStageId.Count; j++)
								{
									if (worldDungeonData_ByAreaIdStageId[j].status > 1 || worldDungeonData_ByAreaIdStageId[j].wdscMList.Count > 0)
									{
										worldStageData.wddL.Add(worldDungeonData_ByAreaIdStageId[j]);
									}
								}
								worldStageData.wdi = this.GetLastWorldDungeonInfo;
								worldStageData.status = num2;
								if (worldDungeonData_ByAreaIdStageId.Count > 0)
								{
									worldStageData.worldStageM.closeTime = ServerDateTime.Now.AddSeconds((double)worldDungeonData_ByAreaIdStageId[0].remainingTime).ToString();
								}
								worldStageData.dngClearCount = 0;
								for (int k = 0; k < worldDungeonData_ByAreaIdStageId.Count; k++)
								{
									if (worldDungeonData_ByAreaIdStageId[k].status == 4)
									{
										worldStageData.dngClearCount++;
									}
								}
								worldStageData.dngCount = worldDungeonData_ByAreaIdStageId.Count;
								list.Add(worldStageData);
							}
						}
					}
					while (worldDungeonData_ByAreaIdStageId != null);
				}
			}
			if (id == "8")
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

		private int GetStageStartConditionCount(List<QuestData.WorldDungeonData> wddL)
		{
			int num = 0;
			for (int i = 0; i < wddL.Count; i++)
			{
				num += wddL[i].wdscMList.Count;
			}
			return num;
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

		public List<QuestData.WorldDungeonData> GetWorldDungeonData_ByAreaIdStageId(string aid, string sid, int idx = 0, bool addLock = false, bool addEmpty = false)
		{
			List<QuestData.WorldDungeonData> list = new List<QuestData.WorldDungeonData>();
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
			List<GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM> list2 = new List<GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM>();
			GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldId = this.GetDngeonInfoByWorldId(aid);
			if (dngeonInfoByWorldId == null)
			{
				return null;
			}
			for (int i = 0; i < worldDungeonM.Length; i++)
			{
				if (worldDungeonM[i].worldStageId == sid)
				{
					list2.Add(worldDungeonM[i]);
				}
			}
			if (dngeonInfoByWorldId.worldDungeonInfo == null)
			{
				return list;
			}
			GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] array = null;
			GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo worldDungeonInfo = null;
			this.wdi_bk = null;
			int num = 0;
			int j;
			for (j = 0; j < dngeonInfoByWorldId.worldDungeonInfo.Length; j++)
			{
				if (dngeonInfoByWorldId.worldDungeonInfo[j].worldStageId == int.Parse(sid))
				{
					if (idx == num)
					{
						worldDungeonInfo = dngeonInfoByWorldId.worldDungeonInfo[j];
						this.wdi_bk = worldDungeonInfo;
						array = worldDungeonInfo.dungeons;
						break;
					}
					num++;
				}
			}
			if (j < dngeonInfoByWorldId.worldDungeonInfo.Length)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					for (j = 0; j < array.Length; j++)
					{
						if (int.Parse(list2[i].worldDungeonId) == array[j].worldDungeonId && (addEmpty || !this.IsEmptyDng(array[j], aid)) && (addLock || array[j].status > 1))
						{
							list.Add(new QuestData.WorldDungeonData
							{
								stageId = worldDungeonInfo.worldStageId,
								worldDungeonM = list2[i],
								dungeon = array[j],
								status = array[j].status,
								wdscMList = this.GetWorldDungeonStartConditionM(array[j].worldDungeonId.ToString()),
								remainingTime = worldDungeonInfo.timeLeft
							});
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

		public bool IsEmptyDng(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng, string aid)
		{
			if (aid == "8")
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
			GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldId = this.GetDngeonInfoByWorldId("8");
			if (dngeonInfoByWorldId == null)
			{
				return null;
			}
			for (int i = 0; i < dngeonInfoByWorldId.worldDungeonInfo.Length; i++)
			{
				GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] dungeons = dngeonInfoByWorldId.worldDungeonInfo[i].dungeons;
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
			public string worldId;

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
		}

		public class WorldDungeonData
		{
			public int stageId;

			public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM;

			public GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon;

			public int status;

			public List<GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM> wdscMList;

			public int remainingTime;
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
