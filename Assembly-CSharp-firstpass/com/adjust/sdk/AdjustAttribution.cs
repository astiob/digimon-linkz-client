using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public class AdjustAttribution
	{
		public AdjustAttribution()
		{
		}

		public AdjustAttribution(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.trackerName = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTrackerName);
			this.trackerToken = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTrackerToken);
			this.network = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyNetwork);
			this.campaign = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyCampaign);
			this.adgroup = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdgroup);
			this.creative = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyCreative);
			this.clickLabel = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyClickLabel);
			this.adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
		}

		public AdjustAttribution(Dictionary<string, string> dicAttributionData)
		{
			if (dicAttributionData == null)
			{
				return;
			}
			this.trackerName = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerName);
			this.trackerToken = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerToken);
			this.network = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyNetwork);
			this.campaign = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyCampaign);
			this.adgroup = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyAdgroup);
			this.creative = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyCreative);
			this.clickLabel = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyClickLabel);
			this.adid = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyAdid);
		}

		public string adid { get; set; }

		public string network { get; set; }

		public string adgroup { get; set; }

		public string campaign { get; set; }

		public string creative { get; set; }

		public string clickLabel { get; set; }

		public string trackerName { get; set; }

		public string trackerToken { get; set; }

		private static string TryGetValue(Dictionary<string, string> dic, string key)
		{
			string result;
			if (dic.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}
	}
}
