using System;

namespace UnityEngine.SocialPlatforms
{
	public interface IAchievement
	{
		void ReportProgress(Action<bool> callback);

		string id { get; set; }

		double percentCompleted { get; set; }

		bool completed { get; }

		bool hidden { get; }

		DateTime lastReportedDate { get; }
	}
}
