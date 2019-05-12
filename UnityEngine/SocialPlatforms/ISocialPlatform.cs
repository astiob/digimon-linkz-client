using System;

namespace UnityEngine.SocialPlatforms
{
	public interface ISocialPlatform
	{
		/// <summary>
		///   <para>See Social.localUser.</para>
		/// </summary>
		ILocalUser localUser { get; }

		void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback);

		void ReportProgress(string achievementID, double progress, Action<bool> callback);

		void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback);

		void LoadAchievements(Action<IAchievement[]> callback);

		/// <summary>
		///   <para>See Social.CreateAchievement..</para>
		/// </summary>
		IAchievement CreateAchievement();

		void ReportScore(long score, string board, Action<bool> callback);

		void LoadScores(string leaderboardID, Action<IScore[]> callback);

		/// <summary>
		///   <para>See Social.CreateLeaderboard.</para>
		/// </summary>
		ILeaderboard CreateLeaderboard();

		/// <summary>
		///   <para>See Social.ShowAchievementsUI.</para>
		/// </summary>
		void ShowAchievementsUI();

		/// <summary>
		///   <para>See Social.ShowLeaderboardUI.</para>
		/// </summary>
		void ShowLeaderboardUI();

		void Authenticate(ILocalUser user, Action<bool> callback);

		void LoadFriends(ILocalUser user, Action<bool> callback);

		void LoadScores(ILeaderboard board, Action<bool> callback);

		bool GetLoading(ILeaderboard board);
	}
}
