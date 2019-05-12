using Evolution;
using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
	public sealed class UpdateMasterData
	{
		private IEnumerator GetOldMasterDataIdList(List<MasterId> updateInfoList)
		{
			UnityEngine.Object resource = AssetDataMng.Instance().LoadObject("UIPrefab/MasterDataLoadingGauge", null, true);
			GameObject go = UnityEngine.Object.Instantiate(resource) as GameObject;
			MasterDataLoadingGauge topDownload = go.GetComponent<MasterDataLoadingGauge>();
			AppCoroutine.Start(MasterDataMng.Instance().ReadMasterData(updateInfoList, new Action<int, int>(topDownload.SetLoadProgress)), false);
			yield return AppCoroutine.Start(topDownload.WaitMasterDataLoad(), false);
			resource = null;
			topDownload = null;
			UnityEngine.Object.Destroy(go);
			go = null;
			yield break;
		}

		private IEnumerator UpgradeMasterData(List<MasterId> updateInfoList)
		{
			if (0 < updateInfoList.Count)
			{
				Loading.Invisible();
				AppCoroutine.Start(MasterDataMng.Instance().UpdateLocalMasterData(updateInfoList), false);
				CMD_ShortDownload shortDownload = GUIMain.ShowCommonDialog(null, "CMD_ShortDownload") as CMD_ShortDownload;
				yield return AppCoroutine.Start(shortDownload.WaitMasterDataDownload(), false);
				shortDownload.ClosePanel(true);
				Loading.ResumeDisplay();
			}
			yield break;
		}

		private void CreateMasterDataCache()
		{
			MonsterMaster.Initialize();
			MonsterSkillData.Initialize();
			ClassSingleton<EvolutionData>.Instance.Initialize();
			VersionUpMaterialData.Initialize();
			MonsterFriendshipData.Initialize();
			MonsterGrowStepData.Initialize();
			MonsterTribeData.Initialize();
		}

		public IEnumerator UpdateData()
		{
			MasterDataMng.Instance().ClearCache();
			MasterDataMng.Instance().InitialFileIO();
			List<MasterId> updateInfoList = new List<MasterId>();
			yield return AppCoroutine.Start(this.GetOldMasterDataIdList(updateInfoList), false);
			yield return AppCoroutine.Start(MasterDataMng.Instance().GetMasterDataUpdateInfo(updateInfoList), false);
			yield return AppCoroutine.Start(this.UpgradeMasterData(updateInfoList), false);
			MasterDataMng.Instance().ReleaseFileIO();
			if (MasterDataMng.Instance().MasterDataVersion.versionManagerMaster != null)
			{
				GameWebAPI.Instance().SetMasterDataVersion(MasterDataMng.Instance().MasterDataVersion.versionManagerMaster.version);
			}
			this.CreateMasterDataCache();
			yield break;
		}
	}
}
