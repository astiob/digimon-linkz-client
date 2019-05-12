using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	public static class AnalyticsEvent
	{
		private static readonly string k_SdkVersion = "1.0.10";

		private static readonly Dictionary<string, object> m_EventData = new Dictionary<string, object>();

		private static bool _debugMode = false;

		private static Dictionary<string, string> enumRenameTable = new Dictionary<string, string>
		{
			{
				"RewardedAd",
				"rewarded_ad"
			},
			{
				"TimedReward",
				"timed_reward"
			},
			{
				"SocialReward",
				"social_reward"
			},
			{
				"MainMenu",
				"main_menu"
			},
			{
				"IAPPromo",
				"iap_promo"
			},
			{
				"CrossPromo",
				"cross_promo"
			},
			{
				"FeaturePromo",
				"feature_promo"
			},
			{
				"TextOnly",
				"text_only"
			}
		};

		public static string sdkVersion
		{
			get
			{
				return AnalyticsEvent.k_SdkVersion;
			}
		}

		public static bool debugMode
		{
			get
			{
				return AnalyticsEvent._debugMode;
			}
			set
			{
				AnalyticsEvent._debugMode = value;
			}
		}

		private static void OnValidationFailed(string message)
		{
			throw new ArgumentException(message);
		}

		private static void AddCustomEventData(IDictionary<string, object> eventData)
		{
			if (eventData != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in eventData)
				{
					if (!AnalyticsEvent.m_EventData.ContainsKey(keyValuePair.Key))
					{
						AnalyticsEvent.m_EventData.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}

		public static AnalyticsResult Custom(string eventName, IDictionary<string, object> eventData = null)
		{
			AnalyticsResult analyticsResult = AnalyticsResult.UnsupportedPlatform;
			string text = string.Empty;
			if (string.IsNullOrEmpty(eventName))
			{
				AnalyticsEvent.OnValidationFailed("Custom event name cannot be set to null or an empty string.");
			}
			if (eventData == null)
			{
				analyticsResult = Analytics.CustomEvent(eventName);
			}
			else
			{
				analyticsResult = Analytics.CustomEvent(eventName, eventData);
			}
			if (AnalyticsEvent.debugMode)
			{
				if (eventData == null)
				{
					text += "\n  Event Data (null)";
				}
				else
				{
					text += string.Format("\n  Event Data ({0} params):", eventData.Count);
					foreach (KeyValuePair<string, object> keyValuePair in eventData)
					{
						text += string.Format("\n    Key: '{0}';  Value: '{1}'", keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
			switch (analyticsResult)
			{
			case AnalyticsResult.Ok:
				if (AnalyticsEvent.debugMode)
				{
					Debug.LogFormat("Successfully sent '{0}' event (Result: '{1}').{2}", new object[]
					{
						eventName,
						analyticsResult,
						text
					});
				}
				return analyticsResult;
			default:
				if (analyticsResult != AnalyticsResult.InvalidData)
				{
					Debug.LogWarningFormat("Unable to send '{0}' event (Result: '{1}').{2}", new object[]
					{
						eventName,
						analyticsResult,
						text
					});
					return analyticsResult;
				}
				break;
			case AnalyticsResult.TooManyItems:
				break;
			}
			Debug.LogErrorFormat("Failed to send '{0}' event (Result: '{1}').{2}", new object[]
			{
				eventName,
				analyticsResult,
				text
			});
			return analyticsResult;
		}

		public static AnalyticsResult AchievementStep(int stepIndex, string achievementId, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("step_index", stepIndex);
			if (string.IsNullOrEmpty(achievementId))
			{
				throw new ArgumentException(achievementId);
			}
			AnalyticsEvent.m_EventData.Add("achievement_id", achievementId);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("achievement_step", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AchievementUnlocked(string achievementId, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(achievementId))
			{
				throw new ArgumentException(achievementId);
			}
			AnalyticsEvent.m_EventData.Add("achievement_id", achievementId);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("achievement_unlocked", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdComplete(bool rewarded, AdvertisingNetwork network, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			AnalyticsEvent.m_EventData.Add("network", AnalyticsEvent.RenameEnum(network.ToString()));
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_complete", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdComplete(bool rewarded, string network = null, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			if (!string.IsNullOrEmpty(network))
			{
				AnalyticsEvent.m_EventData.Add("network", network);
			}
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_complete", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdOffer(bool rewarded, AdvertisingNetwork network, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			AnalyticsEvent.m_EventData.Add("network", AnalyticsEvent.RenameEnum(network.ToString()));
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_offer", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdOffer(bool rewarded, string network = null, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			if (!string.IsNullOrEmpty(network))
			{
				AnalyticsEvent.m_EventData.Add("network", network);
			}
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_offer", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdSkip(bool rewarded, AdvertisingNetwork network, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			AnalyticsEvent.m_EventData.Add("network", AnalyticsEvent.RenameEnum(network.ToString()));
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_skip", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdSkip(bool rewarded, string network = null, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			if (!string.IsNullOrEmpty(network))
			{
				AnalyticsEvent.m_EventData.Add("network", network);
			}
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_skip", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdStart(bool rewarded, AdvertisingNetwork network, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			AnalyticsEvent.m_EventData.Add("network", AnalyticsEvent.RenameEnum(network.ToString()));
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult AdStart(bool rewarded, string network = null, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			if (!string.IsNullOrEmpty(network))
			{
				AnalyticsEvent.m_EventData.Add("network", network);
			}
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("ad_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult ChatMessageSent(IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("chat_message_sent", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult CustomEvent(IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult CutsceneSkip(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("scene_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("cutscene_skip", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult CutsceneStart(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("scene_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("cutscene_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult FirstInteraction(string actionId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (!string.IsNullOrEmpty(actionId))
			{
				AnalyticsEvent.m_EventData.Add("action_id", actionId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("first_interaction", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult GameOver(int index, string name = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("level_index", index);
			if (!string.IsNullOrEmpty(name))
			{
				AnalyticsEvent.m_EventData.Add("level_name", name);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("game_over", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult GameOver(string name = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (!string.IsNullOrEmpty(name))
			{
				AnalyticsEvent.m_EventData.Add("level_name", name);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("game_over", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult GameStart(IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("game_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult IAPTransaction(string transactionContext, float price, string itemId, string itemType = null, string level = null, string transactionId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(transactionContext))
			{
				throw new ArgumentException(transactionContext);
			}
			AnalyticsEvent.m_EventData.Add("transaction_context", transactionContext);
			AnalyticsEvent.m_EventData.Add("price", price);
			if (string.IsNullOrEmpty(itemId))
			{
				throw new ArgumentException(itemId);
			}
			AnalyticsEvent.m_EventData.Add("item_id", itemId);
			if (!string.IsNullOrEmpty(itemType))
			{
				AnalyticsEvent.m_EventData.Add("item_type", itemType);
			}
			if (!string.IsNullOrEmpty(level))
			{
				AnalyticsEvent.m_EventData.Add("level", level);
			}
			if (!string.IsNullOrEmpty(transactionId))
			{
				AnalyticsEvent.m_EventData.Add("transaction_id", transactionId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("iap_transaction", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult ItemAcquired(AcquisitionType currencyType, string transactionContext, float amount, string itemId, float balance, string itemType = null, string level = null, string transactionId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("currency_type", AnalyticsEvent.RenameEnum(currencyType.ToString()));
			if (string.IsNullOrEmpty(transactionContext))
			{
				throw new ArgumentException(transactionContext);
			}
			AnalyticsEvent.m_EventData.Add("transaction_context", transactionContext);
			AnalyticsEvent.m_EventData.Add("amount", amount);
			if (string.IsNullOrEmpty(itemId))
			{
				throw new ArgumentException(itemId);
			}
			AnalyticsEvent.m_EventData.Add("item_id", itemId);
			AnalyticsEvent.m_EventData.Add("balance", balance);
			if (!string.IsNullOrEmpty(itemType))
			{
				AnalyticsEvent.m_EventData.Add("item_type", itemType);
			}
			if (!string.IsNullOrEmpty(level))
			{
				AnalyticsEvent.m_EventData.Add("level", level);
			}
			if (!string.IsNullOrEmpty(transactionId))
			{
				AnalyticsEvent.m_EventData.Add("transaction_id", transactionId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("item_acquired", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult ItemAcquired(AcquisitionType currencyType, string transactionContext, float amount, string itemId, string itemType = null, string level = null, string transactionId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("currency_type", AnalyticsEvent.RenameEnum(currencyType.ToString()));
			if (string.IsNullOrEmpty(transactionContext))
			{
				throw new ArgumentException(transactionContext);
			}
			AnalyticsEvent.m_EventData.Add("transaction_context", transactionContext);
			AnalyticsEvent.m_EventData.Add("amount", amount);
			if (string.IsNullOrEmpty(itemId))
			{
				throw new ArgumentException(itemId);
			}
			AnalyticsEvent.m_EventData.Add("item_id", itemId);
			if (!string.IsNullOrEmpty(itemType))
			{
				AnalyticsEvent.m_EventData.Add("item_type", itemType);
			}
			if (!string.IsNullOrEmpty(level))
			{
				AnalyticsEvent.m_EventData.Add("level", level);
			}
			if (!string.IsNullOrEmpty(transactionId))
			{
				AnalyticsEvent.m_EventData.Add("transaction_id", transactionId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("item_acquired", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult ItemSpent(AcquisitionType currencyType, string transactionContext, float amount, string itemId, float balance, string itemType = null, string level = null, string transactionId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("currency_type", AnalyticsEvent.RenameEnum(currencyType.ToString()));
			if (string.IsNullOrEmpty(transactionContext))
			{
				throw new ArgumentException(transactionContext);
			}
			AnalyticsEvent.m_EventData.Add("transaction_context", transactionContext);
			AnalyticsEvent.m_EventData.Add("amount", amount);
			if (string.IsNullOrEmpty(itemId))
			{
				throw new ArgumentException(itemId);
			}
			AnalyticsEvent.m_EventData.Add("item_id", itemId);
			AnalyticsEvent.m_EventData.Add("balance", balance);
			if (!string.IsNullOrEmpty(itemType))
			{
				AnalyticsEvent.m_EventData.Add("item_type", itemType);
			}
			if (!string.IsNullOrEmpty(level))
			{
				AnalyticsEvent.m_EventData.Add("level", level);
			}
			if (!string.IsNullOrEmpty(transactionId))
			{
				AnalyticsEvent.m_EventData.Add("transaction_id", transactionId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("item_spent", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult ItemSpent(AcquisitionType currencyType, string transactionContext, float amount, string itemId, string itemType = null, string level = null, string transactionId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("currency_type", AnalyticsEvent.RenameEnum(currencyType.ToString()));
			if (string.IsNullOrEmpty(transactionContext))
			{
				throw new ArgumentException(transactionContext);
			}
			AnalyticsEvent.m_EventData.Add("transaction_context", transactionContext);
			AnalyticsEvent.m_EventData.Add("amount", amount);
			if (string.IsNullOrEmpty(itemId))
			{
				throw new ArgumentException(itemId);
			}
			AnalyticsEvent.m_EventData.Add("item_id", itemId);
			if (!string.IsNullOrEmpty(itemType))
			{
				AnalyticsEvent.m_EventData.Add("item_type", itemType);
			}
			if (!string.IsNullOrEmpty(level))
			{
				AnalyticsEvent.m_EventData.Add("level", level);
			}
			if (!string.IsNullOrEmpty(transactionId))
			{
				AnalyticsEvent.m_EventData.Add("transaction_id", transactionId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("item_spent", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelComplete(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_complete", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelComplete(int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_complete", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelComplete(string name, int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_complete", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelFail(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_fail", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelFail(int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_fail", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelFail(string name, int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_fail", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelQuit(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_quit", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelQuit(int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_quit", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelQuit(string name, int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_quit", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelSkip(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_skip", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelSkip(int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_skip", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelSkip(string name, int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_skip", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelStart(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelStart(int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelStart(string name, int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("level_name", name);
			AnalyticsEvent.m_EventData.Add("level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelUp(string name, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("new_level_name", name);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_up", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelUp(int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("new_level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_up", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult LevelUp(string name, int index, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(name);
			}
			AnalyticsEvent.m_EventData.Add("new_level_name", name);
			AnalyticsEvent.m_EventData.Add("new_level_index", index);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("level_up", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult PostAdAction(bool rewarded, AdvertisingNetwork network, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			AnalyticsEvent.m_EventData.Add("network", AnalyticsEvent.RenameEnum(network.ToString()));
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("post_ad_action", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult PostAdAction(bool rewarded, string network = null, string placementId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("rewarded", rewarded);
			if (!string.IsNullOrEmpty(network))
			{
				AnalyticsEvent.m_EventData.Add("network", network);
			}
			if (!string.IsNullOrEmpty(placementId))
			{
				AnalyticsEvent.m_EventData.Add("placement_id", placementId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("post_ad_action", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult PushNotificationClick(string message_id, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(message_id))
			{
				throw new ArgumentException(message_id);
			}
			AnalyticsEvent.m_EventData.Add("message_id", message_id);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("push_notification_click", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult PushNotificationEnable(IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("push_notification_enable", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult ScreenVisit(ScreenName screenName, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("screen_name", AnalyticsEvent.RenameEnum(screenName.ToString()));
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("screen_visit", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult ScreenVisit(string screenName, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(screenName))
			{
				throw new ArgumentException(screenName);
			}
			AnalyticsEvent.m_EventData.Add("screen_name", screenName);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("screen_visit", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShare(ShareType shareType, SocialNetwork socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("share_type", AnalyticsEvent.RenameEnum(shareType.ToString()));
			AnalyticsEvent.m_EventData.Add("social_network", AnalyticsEvent.RenameEnum(socialNetwork.ToString()));
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShare(ShareType shareType, string socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("share_type", AnalyticsEvent.RenameEnum(shareType.ToString()));
			if (string.IsNullOrEmpty(socialNetwork))
			{
				throw new ArgumentException(socialNetwork);
			}
			AnalyticsEvent.m_EventData.Add("social_network", socialNetwork);
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShare(string shareType, SocialNetwork socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(shareType))
			{
				throw new ArgumentException(shareType);
			}
			AnalyticsEvent.m_EventData.Add("share_type", shareType);
			AnalyticsEvent.m_EventData.Add("social_network", AnalyticsEvent.RenameEnum(socialNetwork.ToString()));
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShare(string shareType, string socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(shareType))
			{
				throw new ArgumentException(shareType);
			}
			AnalyticsEvent.m_EventData.Add("share_type", shareType);
			if (string.IsNullOrEmpty(socialNetwork))
			{
				throw new ArgumentException(socialNetwork);
			}
			AnalyticsEvent.m_EventData.Add("social_network", socialNetwork);
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShareAccept(ShareType shareType, SocialNetwork socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("share_type", AnalyticsEvent.RenameEnum(shareType.ToString()));
			AnalyticsEvent.m_EventData.Add("social_network", AnalyticsEvent.RenameEnum(socialNetwork.ToString()));
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share_accept", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShareAccept(ShareType shareType, string socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("share_type", AnalyticsEvent.RenameEnum(shareType.ToString()));
			if (string.IsNullOrEmpty(socialNetwork))
			{
				throw new ArgumentException(socialNetwork);
			}
			AnalyticsEvent.m_EventData.Add("social_network", socialNetwork);
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share_accept", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShareAccept(string shareType, SocialNetwork socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(shareType))
			{
				throw new ArgumentException(shareType);
			}
			AnalyticsEvent.m_EventData.Add("share_type", shareType);
			AnalyticsEvent.m_EventData.Add("social_network", AnalyticsEvent.RenameEnum(socialNetwork.ToString()));
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share_accept", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult SocialShareAccept(string shareType, string socialNetwork, string senderId = null, string recipientId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(shareType))
			{
				throw new ArgumentException(shareType);
			}
			AnalyticsEvent.m_EventData.Add("share_type", shareType);
			if (string.IsNullOrEmpty(socialNetwork))
			{
				throw new ArgumentException(socialNetwork);
			}
			AnalyticsEvent.m_EventData.Add("social_network", socialNetwork);
			if (!string.IsNullOrEmpty(senderId))
			{
				AnalyticsEvent.m_EventData.Add("sender_id", senderId);
			}
			if (!string.IsNullOrEmpty(recipientId))
			{
				AnalyticsEvent.m_EventData.Add("recipient_id", recipientId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("social_share_accept", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult StoreItemClick(StoreType storeType, string itemId, string itemName = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("type", AnalyticsEvent.RenameEnum(storeType.ToString()));
			if (string.IsNullOrEmpty(itemId))
			{
				throw new ArgumentException(itemId);
			}
			AnalyticsEvent.m_EventData.Add("item_id", itemId);
			if (!string.IsNullOrEmpty(itemName))
			{
				AnalyticsEvent.m_EventData.Add("item_name", itemName);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("store_item_click", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult StoreOpened(StoreType storeType, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("type", AnalyticsEvent.RenameEnum(storeType.ToString()));
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("store_opened", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult TutorialComplete(string tutorialId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (!string.IsNullOrEmpty(tutorialId))
			{
				AnalyticsEvent.m_EventData.Add("tutorial_id", tutorialId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("tutorial_complete", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult TutorialSkip(string tutorialId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (!string.IsNullOrEmpty(tutorialId))
			{
				AnalyticsEvent.m_EventData.Add("tutorial_id", tutorialId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("tutorial_skip", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult TutorialStart(string tutorialId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (!string.IsNullOrEmpty(tutorialId))
			{
				AnalyticsEvent.m_EventData.Add("tutorial_id", tutorialId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("tutorial_start", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult TutorialStep(int stepIndex, string tutorialId = null, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("step_index", stepIndex);
			if (!string.IsNullOrEmpty(tutorialId))
			{
				AnalyticsEvent.m_EventData.Add("tutorial_id", tutorialId);
			}
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("tutorial_step", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult UserSignup(AuthorizationNetwork authorizationNetwork, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			AnalyticsEvent.m_EventData.Add("authorization_network", AnalyticsEvent.RenameEnum(authorizationNetwork.ToString()));
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("user_signup", AnalyticsEvent.m_EventData);
		}

		public static AnalyticsResult UserSignup(string authorizationNetwork, IDictionary<string, object> eventData = null)
		{
			AnalyticsEvent.m_EventData.Clear();
			if (string.IsNullOrEmpty(authorizationNetwork))
			{
				throw new ArgumentException(authorizationNetwork);
			}
			AnalyticsEvent.m_EventData.Add("authorization_network", authorizationNetwork);
			AnalyticsEvent.AddCustomEventData(eventData);
			return AnalyticsEvent.Custom("user_signup", AnalyticsEvent.m_EventData);
		}

		private static string RenameEnum(string enumName)
		{
			return (!AnalyticsEvent.enumRenameTable.ContainsKey(enumName)) ? enumName.ToLower() : AnalyticsEvent.enumRenameTable[enumName];
		}
	}
}
