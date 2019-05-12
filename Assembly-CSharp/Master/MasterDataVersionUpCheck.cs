using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Master
{
	public static class MasterDataVersionUpCheck
	{
		private static IEnumerator LoadLocalMasterDataVersionInfo(MasterDataVersionInfo masterDataVersion, MasterDataFileIO fileIO)
		{
			return fileIO.ReadMasterDataVersionFile(delegate(GameWebAPI.RespDataCM_MDVersion data)
			{
				masterDataVersion.localVersion = data;
			});
		}

		private static IEnumerator GetServerMasterDataVersionInfo(MasterDataVersionInfo masterDataVersion)
		{
			GameWebAPI.RequestCM_MasterDataVersion request = new GameWebAPI.RequestCM_MasterDataVersion
			{
				OnReceived = delegate(GameWebAPI.RespDataCM_MDVersion response)
				{
					masterDataVersion.serverVersion = response;
				}
			};
			return request.Run(null, null, null);
		}

		private static bool IsVersionUp(GameWebAPI.RespDataCM_MDVersion.DataVersionList[] localVersionInfoList, GameWebAPI.RespDataCM_MDVersion.DataVersionList serverVersionInfo)
		{
			bool flag = true;
			for (int i = 0; i < localVersionInfoList.Length; i++)
			{
				if (localVersionInfoList[i].tableName == serverVersionInfo.tableName)
				{
					int num = int.Parse(localVersionInfoList[i].version);
					int num2 = int.Parse(serverVersionInfo.version);
					flag = (num < num2);
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		private static List<MasterId> GetClientCoverMasterDataIdList(string tableName, MasterBase[] localMasterDataList)
		{
			List<MasterId> list = new List<MasterId>();
			for (int i = 0; i < localMasterDataList.Length; i++)
			{
				if (tableName == localMasterDataList[i].GetTableName())
				{
					list.Add(localMasterDataList[i].ID);
				}
			}
			return list;
		}

		public static IEnumerator GetMasterDataVersion(MonoBehaviour coroutineObj, MasterDataFileIO fileIO, MasterDataVersionInfo masterDataVersion)
		{
			global::Debug.Assert(null != masterDataVersion, "マスターデータのバージョン格納先がNULLです.");
			yield return coroutineObj.StartCoroutine(MasterDataVersionUpCheck.LoadLocalMasterDataVersionInfo(masterDataVersion, fileIO));
			yield return coroutineObj.StartCoroutine(MasterDataVersionUpCheck.GetServerMasterDataVersionInfo(masterDataVersion));
			yield break;
		}

		public static List<string> GetVersionUpMasterDataTableNameList(MasterDataVersionInfo masterDataVersion)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < masterDataVersion.serverVersion.dataVersionList.Length; i++)
			{
				GameWebAPI.RespDataCM_MDVersion.DataVersionList dataVersionList = masterDataVersion.serverVersion.dataVersionList[i];
				if (masterDataVersion.localVersion == null || MasterDataVersionUpCheck.IsVersionUp(masterDataVersion.localVersion.dataVersionList, dataVersionList))
				{
					list.Add(dataVersionList.tableName);
				}
			}
			return list;
		}

		public static List<MasterId> GetVersionUpMasterIdList(List<string> tableNameList, Dictionary<MasterId, MasterBase> localMasterDataTable)
		{
			List<MasterId> list = new List<MasterId>();
			MasterBase[] localMasterDataList = localMasterDataTable.Values.ToArray<MasterBase>();
			for (int i = 0; i < tableNameList.Count; i++)
			{
				list.AddRange(MasterDataVersionUpCheck.GetClientCoverMasterDataIdList(tableNameList[i], localMasterDataList));
			}
			return list;
		}

		public static void AddVersionUpMasterId(List<MasterId> versionUpMasterDataIdList, List<MasterId> destMasterIdList)
		{
			for (int i = 0; i < versionUpMasterDataIdList.Count; i++)
			{
				if (!destMasterIdList.Contains(versionUpMasterDataIdList[i]))
				{
					destMasterIdList.Add(versionUpMasterDataIdList[i]);
				}
			}
		}
	}
}
