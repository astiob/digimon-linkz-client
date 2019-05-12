using System;
using System.Collections;

namespace Title
{
	public sealed class ConfirmGDPR_Network
	{
		private GameWebAPI.ResponseGdprInfo gdprInfo;

		public bool Exists()
		{
			bool result = false;
			if (this.gdprInfo != null && this.gdprInfo.gdprList != null)
			{
				result = (0 < this.gdprInfo.gdprList.Length);
			}
			return result;
		}

		public GameWebAPI.ResponseGdprInfo.Details[] Details
		{
			get
			{
				return this.gdprInfo.gdprList;
			}
		}

		public ConfirmGDPR_Network.GDPRWebPageType GetWebPageType(GameWebAPI.ResponseGdprInfo.Details details)
		{
			ConfirmGDPR_Network.GDPRWebPageType result = ConfirmGDPR_Network.GDPRWebPageType.NONE;
			int type = details.type;
			if (type != 1)
			{
				if (type != 2)
				{
					if (type == 3)
					{
						result = ConfirmGDPR_Network.GDPRWebPageType.ANALYTICS;
					}
				}
				else
				{
					result = ConfirmGDPR_Network.GDPRWebPageType.AD_TARGET;
				}
			}
			else
			{
				result = ConfirmGDPR_Network.GDPRWebPageType.TOP_PAGE;
			}
			return result;
		}

		public bool IsConfirmWebPage(ConfirmGDPR_Network.GDPRWebPageType pageType)
		{
			return pageType == ConfirmGDPR_Network.GDPRWebPageType.AD_TARGET || ConfirmGDPR_Network.GDPRWebPageType.ANALYTICS == pageType;
		}

		public string GetWebPageURL(ConfirmGDPR_Network.GDPRWebPageType type)
		{
			string result = string.Empty;
			if (this.gdprInfo != null)
			{
				for (int i = 0; i < this.gdprInfo.gdprList.Length; i++)
				{
					if (type == this.GetWebPageType(this.gdprInfo.gdprList[i]))
					{
						result = this.gdprInfo.gdprList[i].url;
						break;
					}
				}
			}
			return result;
		}

		public APIRequestTask GetRequestInfo()
		{
			GameWebAPI.Request_GdprInfo request = new GameWebAPI.Request_GdprInfo
			{
				OnReceived = delegate(GameWebAPI.ResponseGdprInfo response)
				{
					this.gdprInfo = response;
				}
			};
			return new APIRequestTask(request, true);
		}

		public APIRequestTask GetRequestConfirmed()
		{
			GameWebAPI.Request_GdprConfirmed request = new GameWebAPI.Request_GdprConfirmed();
			return new APIRequestTask(request, true);
		}

		public IEnumerator Send(APIRequestTask request, Action completed)
		{
			return request.Run(completed, null, null);
		}

		public IEnumerator Send(APIRequestTask request, Action completed, Func<Exception, APIRequestTask, IEnumerator> alert)
		{
			return request.Run(completed, null, (Exception ex) => alert(ex, request));
		}

		public enum GDPRWebPageType
		{
			NONE,
			TOP_PAGE,
			AD_TARGET,
			ANALYTICS
		}
	}
}
