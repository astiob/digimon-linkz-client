using System;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class UserInfoEvent : AnalyticsEvent
	{
		public const string kEventUserInfo = "userInfo";

		public const string kCustomUserId = "custom_userid";

		public const string kGender = "sex";

		public const string kBirthYear = "birth_year";

		public UserInfoEvent() : base("userInfo", CloudEventFlags.CacheImmediately)
		{
		}

		public void SetCustomUserId(string userId)
		{
			base.SetParameter("custom_userid", userId);
		}

		public void SetUserGender(string gender)
		{
			base.SetParameter("sex", gender);
		}

		public void SetUserBirthYear(int birthyear)
		{
			base.SetParameter("birth_year", birthyear);
		}
	}
}
