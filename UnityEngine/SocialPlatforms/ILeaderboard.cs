using System;

namespace UnityEngine.SocialPlatforms
{
	public interface ILeaderboard
	{
		/// <summary>
		///   <para>Only search for these user IDs.</para>
		/// </summary>
		/// <param name="userIDs">List of user ids.</param>
		void SetUserFilter(string[] userIDs);

		void LoadScores(Action<bool> callback);

		/// <summary>
		///   <para>The leaderboad is in the process of loading scores.</para>
		/// </summary>
		bool loading { get; }

		/// <summary>
		///   <para>Unique identifier for this leaderboard.</para>
		/// </summary>
		string id { get; set; }

		/// <summary>
		///   <para>The users scope searched by this leaderboard.</para>
		/// </summary>
		UserScope userScope { get; set; }

		/// <summary>
		///   <para>The rank range this leaderboard returns.</para>
		/// </summary>
		Range range { get; set; }

		/// <summary>
		///   <para>The time period/scope searched by this leaderboard.</para>
		/// </summary>
		TimeScope timeScope { get; set; }

		/// <summary>
		///   <para>The leaderboard score of the logged in user.</para>
		/// </summary>
		IScore localUserScore { get; }

		/// <summary>
		///   <para>The total amount of scores the leaderboard contains.</para>
		/// </summary>
		uint maxRange { get; }

		/// <summary>
		///   <para>The leaderboard scores returned by a query.</para>
		/// </summary>
		IScore[] scores { get; }

		/// <summary>
		///   <para>The human readable title of this leaderboard.</para>
		/// </summary>
		string title { get; }
	}
}
