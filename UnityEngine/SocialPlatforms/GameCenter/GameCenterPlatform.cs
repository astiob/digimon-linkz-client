using System;

namespace UnityEngine.SocialPlatforms.GameCenter
{
	/// <summary>
	///   <para>iOS GameCenter implementation for network services.</para>
	/// </summary>
	public class GameCenterPlatform : Local
	{
		public static void ResetAllAchievements(Action<bool> callback)
		{
			Debug.Log("ResetAllAchievements - no effect in editor");
			callback(true);
		}

		/// <summary>
		///   <para>Show the default iOS banner when achievements are completed.</para>
		/// </summary>
		/// <param name="value"></param>
		public static void ShowDefaultAchievementCompletionBanner(bool value)
		{
			Debug.Log("ShowDefaultAchievementCompletionBanner - no effect in editor");
		}

		/// <summary>
		///   <para>Show the leaderboard UI with a specific leaderboard shown initially with a specific time scope selected.</para>
		/// </summary>
		/// <param name="leaderboardID"></param>
		/// <param name="timeScope"></param>
		public static void ShowLeaderboardUI(string leaderboardID, TimeScope timeScope)
		{
			Debug.Log("ShowLeaderboardUI - no effect in editor");
		}
	}
}
