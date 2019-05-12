using System;
using UnityEngine;

namespace Neptune.GameService
{
	public class NpGameServiceAndroid
	{
		public static int GetResolveRetryMax()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<int>("getResolveRetryMax", new object[0]);
				}
			}
			return result;
		}

		public static void SetResolveRetryMax(int resolveRetry)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("setResolveRetryMax", new object[]
					{
						resolveRetry
					});
				}
			}
		}

		public static void EnableDebugLog(bool isDebug)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("enableDebugLog", new object[]
					{
						isDebug
					});
				}
			}
		}

		public static void SignedIn(NpGameServiceAndroid.CLIENT_TYPE clientsToUse)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
					{
						using (AndroidJavaObject androidJavaObject = androidJavaClass2.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
						{
							androidJavaObject.Call("signedIn", new object[]
							{
								@static,
								(int)clientsToUse
							});
						}
					}
				}
			}
		}

		public static bool IsSignedIn()
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<bool>("isSignedIn", new object[0]);
				}
			}
			return result;
		}

		public static void SignOut()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("signOut", new object[0]);
				}
			}
		}

		public static void DataSave(string key, string value)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("savedGamesUpdate", new object[]
					{
						key,
						value
					});
				}
			}
		}

		public static void DataLoad(string key)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("savedGamesLoad", new object[]
					{
						key
					});
				}
			}
		}

		public static void UnlockAchievements(string achievementId)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("unlockAchievements", new object[]
					{
						achievementId
					});
				}
			}
		}

		public static void IncrementAchievements(string achievementId, int value)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("incrementAchievements", new object[]
					{
						achievementId,
						value
					});
				}
			}
		}

		public static void ShowAchievements()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.googlegames.NpGoogleGames"))
					{
						using (AndroidJavaObject androidJavaObject = androidJavaClass2.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
						{
							androidJavaObject.Call("showAchievements", new object[]
							{
								@static
							});
						}
					}
				}
			}
		}

		public enum CLIENT_TYPE
		{
			CLIENT_NONE,
			CLIENT_GAMES,
			CLIENT_PLUS,
			CLIENT_SNAPSHOT = 8
		}
	}
}
