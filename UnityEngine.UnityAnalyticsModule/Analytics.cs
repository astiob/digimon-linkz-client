﻿using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	public static class Analytics
	{
		private static UnityAnalyticsHandler s_UnityAnalyticsHandler;

		internal static UnityAnalyticsHandler GetUnityAnalyticsHandler()
		{
			if (Analytics.s_UnityAnalyticsHandler == null)
			{
				Analytics.s_UnityAnalyticsHandler = new UnityAnalyticsHandler();
			}
			return Analytics.s_UnityAnalyticsHandler;
		}

		public static bool limitUserTracking
		{
			get
			{
				return UnityAnalyticsHandler.limitUserTracking;
			}
			set
			{
				UnityAnalyticsHandler.limitUserTracking = value;
			}
		}

		public static bool deviceStatsEnabled
		{
			get
			{
				return UnityAnalyticsHandler.deviceStatsEnabled;
			}
			set
			{
				UnityAnalyticsHandler.deviceStatsEnabled = value;
			}
		}

		public static bool enabled
		{
			get
			{
				UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
				return unityAnalyticsHandler != null && unityAnalyticsHandler.enabled;
			}
			set
			{
				UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
				if (unityAnalyticsHandler != null)
				{
					unityAnalyticsHandler.enabled = value;
				}
			}
		}

		public static AnalyticsResult FlushEvents()
		{
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.FlushEvents();
			}
			return result;
		}

		public static AnalyticsResult SetUserId(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException("Cannot set userId to an empty or null string");
			}
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.SetUserId(userId);
			}
			return result;
		}

		public static AnalyticsResult SetUserGender(Gender gender)
		{
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.SetUserGender(gender);
			}
			return result;
		}

		public static AnalyticsResult SetUserBirthYear(int birthYear)
		{
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (Analytics.s_UnityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.SetUserBirthYear(birthYear);
			}
			return result;
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency)
		{
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.Transaction(productId, Convert.ToDouble(amount), currency, null, null);
			}
			return result;
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
		{
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.Transaction(productId, Convert.ToDouble(amount), currency, receiptPurchaseData, signature);
			}
			return result;
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature, bool usingIAPService)
		{
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.Transaction(productId, Convert.ToDouble(amount), currency, receiptPurchaseData, signature, usingIAPService);
			}
			return result;
		}

		public static AnalyticsResult CustomEvent(string customEventName)
		{
			if (string.IsNullOrEmpty(customEventName))
			{
				throw new ArgumentException("Cannot set custom event name to an empty or null string");
			}
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				result = unityAnalyticsHandler.CustomEvent(customEventName);
			}
			return result;
		}

		public static AnalyticsResult CustomEvent(string customEventName, Vector3 position)
		{
			if (string.IsNullOrEmpty(customEventName))
			{
				throw new ArgumentException("Cannot set custom event name to an empty or null string");
			}
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else
			{
				CustomEventData customEventData = new CustomEventData(customEventName);
				customEventData.Add("x", (double)Convert.ToDecimal(position.x));
				customEventData.Add("y", (double)Convert.ToDecimal(position.y));
				customEventData.Add("z", (double)Convert.ToDecimal(position.z));
				result = unityAnalyticsHandler.CustomEvent(customEventData);
			}
			return result;
		}

		public static AnalyticsResult CustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			if (string.IsNullOrEmpty(customEventName))
			{
				throw new ArgumentException("Cannot set custom event name to an empty or null string");
			}
			UnityAnalyticsHandler unityAnalyticsHandler = Analytics.GetUnityAnalyticsHandler();
			AnalyticsResult result;
			if (unityAnalyticsHandler == null)
			{
				result = AnalyticsResult.NotInitialized;
			}
			else if (eventData == null)
			{
				result = unityAnalyticsHandler.CustomEvent(customEventName);
			}
			else
			{
				CustomEventData customEventData = new CustomEventData(customEventName);
				customEventData.Add(eventData);
				result = unityAnalyticsHandler.CustomEvent(customEventData);
			}
			return result;
		}
	}
}
