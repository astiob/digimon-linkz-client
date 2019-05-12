using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Master
{
	public static class AlertMaster
	{
		private static Dictionary<string, GameWebAPI.RespDataMA_MessageM.MessageM> alertCache;

		private static AlertMasterResource resourceMaster;

		public static GameWebAPI.RespDataMA_MessageM.MessageM GetAlert(string errorCode)
		{
			GameWebAPI.RespDataMA_MessageM.MessageM result = null;
			GameWebAPI.RespDataMA_MessageM respDataMA_MessageM = MasterDataMng.Instance().RespDataMA_MessageM;
			if (respDataMA_MessageM != null)
			{
				if (!AlertMaster.GetAlertDownloadMaster(respDataMA_MessageM, errorCode, out result))
				{
					bool alertResourceMaster = AlertMaster.GetAlertResourceMaster(errorCode, out result);
				}
			}
			else
			{
				bool alertResourceMaster = AlertMaster.GetAlertResourceMaster(errorCode, out result);
			}
			return result;
		}

		private static bool GetAlertDownloadMaster(GameWebAPI.RespDataMA_MessageM master, string errorCode, out GameWebAPI.RespDataMA_MessageM.MessageM alert)
		{
			bool flag = AlertMaster.alertCache.TryGetValue(errorCode, out alert);
			if (!flag && master.messageM != null)
			{
				for (int i = 0; i < master.messageM.Length; i++)
				{
					if (master.messageM[i].messageCode == errorCode)
					{
						alert = master.messageM[i];
						AlertMaster.alertCache.Add(master.messageM[i].messageCode, master.messageM[i]);
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		private static bool GetAlertResourceMaster(string errorCode, out GameWebAPI.RespDataMA_MessageM.MessageM alert)
		{
			bool flag = AlertMaster.alertCache.TryGetValue(errorCode, out alert);
			if (!flag)
			{
				GameWebAPI.RespDataMA_MessageM.MessageM alert2 = AlertMaster.resourceMaster.GetAlert(errorCode);
				if (alert2 != null)
				{
					alert = alert2;
					AlertMaster.alertCache.Add(alert.messageCode, alert);
					flag = true;
				}
			}
			return flag;
		}

		public static void Initialize()
		{
			if (AlertMaster.alertCache == null)
			{
				AlertMaster.alertCache = new Dictionary<string, GameWebAPI.RespDataMA_MessageM.MessageM>();
			}
			if (null == AlertMaster.resourceMaster)
			{
				int num = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				string path = "Master/message_m" + num;
				AlertMaster.resourceMaster = Resources.Load<AlertMasterResource>(path);
			}
		}

		public static void ClearCache()
		{
			if (AlertMaster.alertCache != null)
			{
				AlertMaster.alertCache.Clear();
			}
		}

		public static void Reload()
		{
			AlertMaster.ClearCache();
			File.Delete(Application.persistentDataPath + "/MA_DT_" + MasterId.MESSAGE_MASTER.ToString() + ".txt");
			int num = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			AlertMaster.resourceMaster = Resources.Load<AlertMasterResource>("Master/message_m" + num);
		}
	}
}
