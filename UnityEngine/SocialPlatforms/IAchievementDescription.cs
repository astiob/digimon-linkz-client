using System;

namespace UnityEngine.SocialPlatforms
{
	public interface IAchievementDescription
	{
		/// <summary>
		///   <para>Unique identifier for this achievement description.</para>
		/// </summary>
		string id { get; set; }

		/// <summary>
		///   <para>Human readable title.</para>
		/// </summary>
		string title { get; }

		/// <summary>
		///   <para>Image representation of the achievement.</para>
		/// </summary>
		Texture2D image { get; }

		/// <summary>
		///   <para>Description when the achivement is completed.</para>
		/// </summary>
		string achievedDescription { get; }

		/// <summary>
		///   <para>Description when the achivement has not been completed.</para>
		/// </summary>
		string unachievedDescription { get; }

		/// <summary>
		///   <para>Hidden achievement are not shown in the list until the percentCompleted has been touched (even if it's 0.0).</para>
		/// </summary>
		bool hidden { get; }

		/// <summary>
		///   <para>Point value of this achievement.</para>
		/// </summary>
		int points { get; }
	}
}
