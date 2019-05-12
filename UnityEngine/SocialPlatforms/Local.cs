using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms
{
	public class Local : ISocialPlatform
	{
		private static LocalUser m_LocalUser;

		private List<UserProfile> m_Friends = new List<UserProfile>();

		private List<UserProfile> m_Users = new List<UserProfile>();

		private List<AchievementDescription> m_AchievementDescriptions = new List<AchievementDescription>();

		private List<Achievement> m_Achievements = new List<Achievement>();

		private List<Leaderboard> m_Leaderboards = new List<Leaderboard>();

		private Texture2D m_DefaultTexture;

		void ISocialPlatform.Authenticate(ILocalUser user, Action<bool> callback)
		{
			LocalUser localUser = (LocalUser)user;
			this.m_DefaultTexture = this.CreateDummyTexture(32, 32);
			this.PopulateStaticData();
			localUser.SetAuthenticated(true);
			localUser.SetUnderage(false);
			localUser.SetUserID("1000");
			localUser.SetUserName("Lerpz");
			localUser.SetImage(this.m_DefaultTexture);
			if (callback != null)
			{
				callback(true);
			}
		}

		void ISocialPlatform.LoadFriends(ILocalUser user, Action<bool> callback)
		{
			if (!this.VerifyUser())
			{
				return;
			}
			((LocalUser)user).SetFriends(this.m_Friends.ToArray());
			if (callback != null)
			{
				callback(true);
			}
		}

		void ISocialPlatform.LoadScores(ILeaderboard board, Action<bool> callback)
		{
			if (!this.VerifyUser())
			{
				return;
			}
			Leaderboard leaderboard = (Leaderboard)board;
			foreach (Leaderboard leaderboard2 in this.m_Leaderboards)
			{
				if (leaderboard2.id == leaderboard.id)
				{
					leaderboard.SetTitle(leaderboard2.title);
					leaderboard.SetScores(leaderboard2.scores);
					leaderboard.SetMaxRange((uint)leaderboard2.scores.Length);
				}
			}
			this.SortScores(leaderboard);
			this.SetLocalPlayerScore(leaderboard);
			if (callback != null)
			{
				callback(true);
			}
		}

		bool ISocialPlatform.GetLoading(ILeaderboard board)
		{
			return this.VerifyUser() && ((Leaderboard)board).loading;
		}

		public ILocalUser localUser
		{
			get
			{
				if (Local.m_LocalUser == null)
				{
					Local.m_LocalUser = new LocalUser();
				}
				return Local.m_LocalUser;
			}
		}

		public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
		{
			List<UserProfile> list = new List<UserProfile>();
			if (!this.VerifyUser())
			{
				return;
			}
			foreach (string b in userIDs)
			{
				foreach (UserProfile userProfile in this.m_Users)
				{
					if (userProfile.id == b)
					{
						list.Add(userProfile);
					}
				}
				foreach (UserProfile userProfile2 in this.m_Friends)
				{
					if (userProfile2.id == b)
					{
						list.Add(userProfile2);
					}
				}
			}
			callback(list.ToArray());
		}

		public void ReportProgress(string id, double progress, Action<bool> callback)
		{
			if (!this.VerifyUser())
			{
				return;
			}
			foreach (Achievement achievement in this.m_Achievements)
			{
				if (achievement.id == id && achievement.percentCompleted <= progress)
				{
					if (progress >= 100.0)
					{
						achievement.SetCompleted(true);
					}
					achievement.SetHidden(false);
					achievement.SetLastReportedDate(DateTime.Now);
					achievement.percentCompleted = progress;
					if (callback != null)
					{
						callback(true);
					}
					return;
				}
			}
			foreach (AchievementDescription achievementDescription in this.m_AchievementDescriptions)
			{
				if (achievementDescription.id == id)
				{
					bool completed = progress >= 100.0;
					Achievement item = new Achievement(id, progress, completed, false, DateTime.Now);
					this.m_Achievements.Add(item);
					if (callback != null)
					{
						callback(true);
					}
					return;
				}
			}
			Debug.LogError("Achievement ID not found");
			if (callback != null)
			{
				callback(false);
			}
		}

		public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
		{
			if (!this.VerifyUser())
			{
				return;
			}
			if (callback != null)
			{
				callback(this.m_AchievementDescriptions.ToArray());
			}
		}

		public void LoadAchievements(Action<IAchievement[]> callback)
		{
			if (!this.VerifyUser())
			{
				return;
			}
			if (callback != null)
			{
				callback(this.m_Achievements.ToArray());
			}
		}

		public void ReportScore(long score, string board, Action<bool> callback)
		{
			if (!this.VerifyUser())
			{
				return;
			}
			foreach (Leaderboard leaderboard in this.m_Leaderboards)
			{
				if (leaderboard.id == board)
				{
					leaderboard.SetScores(new List<Score>((Score[])leaderboard.scores)
					{
						new Score(board, score, this.localUser.id, DateTime.Now, score + " points", 0)
					}.ToArray());
					if (callback != null)
					{
						callback(true);
					}
					return;
				}
			}
			Debug.LogError("Leaderboard not found");
			if (callback != null)
			{
				callback(false);
			}
		}

		public void LoadScores(string leaderboardID, Action<IScore[]> callback)
		{
			if (!this.VerifyUser())
			{
				return;
			}
			foreach (Leaderboard leaderboard in this.m_Leaderboards)
			{
				if (leaderboard.id == leaderboardID)
				{
					this.SortScores(leaderboard);
					if (callback != null)
					{
						callback(leaderboard.scores);
					}
					return;
				}
			}
			Debug.LogError("Leaderboard not found");
			if (callback != null)
			{
				callback(new Score[0]);
			}
		}

		private void SortScores(Leaderboard board)
		{
			List<Score> list = new List<Score>((Score[])board.scores);
			list.Sort((Score s1, Score s2) => s2.value.CompareTo(s1.value));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetRank(i + 1);
			}
		}

		private void SetLocalPlayerScore(Leaderboard board)
		{
			foreach (Score score in board.scores)
			{
				if (score.userID == this.localUser.id)
				{
					board.SetLocalUserScore(score);
					break;
				}
			}
		}

		public void ShowAchievementsUI()
		{
			Debug.Log("ShowAchievementsUI not implemented");
		}

		public void ShowLeaderboardUI()
		{
			Debug.Log("ShowLeaderboardUI not implemented");
		}

		public ILeaderboard CreateLeaderboard()
		{
			return new Leaderboard();
		}

		public IAchievement CreateAchievement()
		{
			return new Achievement();
		}

		private bool VerifyUser()
		{
			if (!this.localUser.authenticated)
			{
				Debug.LogError("Must authenticate first");
				return false;
			}
			return true;
		}

		private void PopulateStaticData()
		{
			this.m_Friends.Add(new UserProfile("Fred", "1001", true, UserState.Online, this.m_DefaultTexture));
			this.m_Friends.Add(new UserProfile("Julia", "1002", true, UserState.Online, this.m_DefaultTexture));
			this.m_Friends.Add(new UserProfile("Jeff", "1003", true, UserState.Online, this.m_DefaultTexture));
			this.m_Users.Add(new UserProfile("Sam", "1004", false, UserState.Offline, this.m_DefaultTexture));
			this.m_Users.Add(new UserProfile("Max", "1005", false, UserState.Offline, this.m_DefaultTexture));
			this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement01", "First achievement", this.m_DefaultTexture, "Get first achievement", "Received first achievement", false, 10));
			this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement02", "Second achievement", this.m_DefaultTexture, "Get second achievement", "Received second achievement", false, 20));
			this.m_AchievementDescriptions.Add(new AchievementDescription("Achievement03", "Third achievement", this.m_DefaultTexture, "Get third achievement", "Received third achievement", false, 15));
			Leaderboard leaderboard = new Leaderboard();
			leaderboard.SetTitle("High Scores");
			leaderboard.id = "Leaderboard01";
			leaderboard.SetScores(new List<Score>
			{
				new Score("Leaderboard01", 300L, "1001", DateTime.Now.AddDays(-1.0), "300 points", 1),
				new Score("Leaderboard01", 255L, "1002", DateTime.Now.AddDays(-1.0), "255 points", 2),
				new Score("Leaderboard01", 55L, "1003", DateTime.Now.AddDays(-1.0), "55 points", 3),
				new Score("Leaderboard01", 10L, "1004", DateTime.Now.AddDays(-1.0), "10 points", 4)
			}.ToArray());
			this.m_Leaderboards.Add(leaderboard);
		}

		private Texture2D CreateDummyTexture(int width, int height)
		{
			Texture2D texture2D = new Texture2D(width, height);
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					Color color = ((j & i) <= 0) ? Color.gray : Color.white;
					texture2D.SetPixel(j, i, color);
				}
			}
			texture2D.Apply();
			return texture2D;
		}
	}
}
