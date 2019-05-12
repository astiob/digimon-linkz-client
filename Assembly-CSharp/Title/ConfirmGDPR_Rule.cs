using Master;
using System;

namespace Title
{
	public sealed class ConfirmGDPR_Rule
	{
		public string webTitle;

		public string url;

		public string adjustEventKey;

		public ConfirmGDPR_Rule(ConfirmGDPR_Network.GDPRWebPageType pageType, string url)
		{
			if (pageType != ConfirmGDPR_Network.GDPRWebPageType.AD_TARGET)
			{
				if (pageType == ConfirmGDPR_Network.GDPRWebPageType.ANALYTICS)
				{
					this.webTitle = StringMaster.GetString("GDPR_Analytics");
					this.adjustEventKey = "analytics_flg";
				}
			}
			else
			{
				this.webTitle = StringMaster.GetString("GDPR_AdTargeting");
				this.adjustEventKey = "ad_targeting_flg";
			}
			this.url = url;
		}
	}
}
