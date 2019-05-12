using System;

namespace UnityEngine.SocialPlatforms
{
	public interface IScore
	{
		void ReportScore(Action<bool> callback);

		string leaderboardID { get; set; }

		long value { get; set; }

		DateTime date { get; }

		string formattedValue { get; }

		string userID { get; }

		int rank { get; }
	}
}
