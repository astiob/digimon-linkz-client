using System;
using System.Collections.Generic;
using UnityEngine;

namespace Master
{
	public static class StringMaster
	{
		private static Dictionary<string, string> stringCache;

		private static StringMasterResource resourceMaster;

		public static string GetString(string key)
		{
			string empty = string.Empty;
			GameWebAPI.RespDataMA_MessageStringM respDataMA_MessageStringMaster = MasterDataMng.Instance().RespDataMA_MessageStringMaster;
			if (respDataMA_MessageStringMaster != null)
			{
				if (!StringMaster.GetStringDownloadMaster(respDataMA_MessageStringMaster, key, out empty))
				{
					bool stringResourceMaster = StringMaster.GetStringResourceMaster(key, out empty);
				}
			}
			else
			{
				bool stringResourceMaster = StringMaster.GetStringResourceMaster(key, out empty);
			}
			return empty;
		}

		private static bool GetStringDownloadMaster(GameWebAPI.RespDataMA_MessageStringM master, string key, out string text)
		{
			bool flag = StringMaster.stringCache.TryGetValue(key, out text);
			if (!flag && master.messageStringM != null)
			{
				for (int i = 0; i < master.messageStringM.Length; i++)
				{
					if (master.messageStringM[i].messageCode == key)
					{
						text = master.messageStringM[i].messageText;
						StringMaster.stringCache.Add(master.messageStringM[i].messageCode, master.messageStringM[i].messageText);
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		private static bool GetStringResourceMaster(string key, out string text)
		{
			bool flag = StringMaster.stringCache.TryGetValue(key, out text);
			if (!flag)
			{
				GameWebAPI.RespDataMA_MessageStringM.Message message = StringMaster.resourceMaster.GetMessage(key);
				if (message != null)
				{
					text = message.messageText;
					StringMaster.stringCache.Add(message.messageCode, message.messageText);
					flag = true;
				}
			}
			return flag;
		}

		public static void Initialize()
		{
			if (StringMaster.stringCache == null)
			{
				StringMaster.stringCache = new Dictionary<string, string>();
			}
			if (null == StringMaster.resourceMaster)
			{
				int num = 1;
				string path = "Master/message_string_m" + num;
				StringMaster.resourceMaster = Resources.Load<StringMasterResource>(path);
			}
		}

		public static void ClearCache()
		{
			if (StringMaster.stringCache != null)
			{
				StringMaster.stringCache.Clear();
			}
		}
	}
}
