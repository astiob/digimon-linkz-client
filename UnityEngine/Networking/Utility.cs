using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
	public class Utility
	{
		private static Random s_randomGenerator = new Random(Environment.TickCount);

		private static bool s_useRandomSourceID = false;

		private static int s_randomSourceComponent = 0;

		private static AppID s_programAppID = AppID.Invalid;

		private static Dictionary<NetworkID, NetworkAccessToken> s_dictTokens = new Dictionary<NetworkID, NetworkAccessToken>();

		private Utility()
		{
		}

		public static bool useRandomSourceID
		{
			get
			{
				return Utility.s_useRandomSourceID;
			}
			set
			{
				Utility.SetUseRandomSourceID(value);
			}
		}

		public static SourceID GetSourceID()
		{
			return (SourceID)((long)(SystemInfo.deviceUniqueIdentifier + Utility.s_randomSourceComponent).GetHashCode());
		}

		private static void SetUseRandomSourceID(bool useRandomSourceID)
		{
			if (useRandomSourceID && !Utility.s_useRandomSourceID)
			{
				Utility.s_randomSourceComponent = Utility.s_randomGenerator.Next(int.MaxValue);
			}
			else if (!useRandomSourceID && Utility.s_useRandomSourceID)
			{
				Utility.s_randomSourceComponent = 0;
			}
			Utility.s_useRandomSourceID = useRandomSourceID;
		}

		public static void SetAppID(AppID newAppID)
		{
			Utility.s_programAppID = newAppID;
		}

		public static AppID GetAppID()
		{
			return Utility.s_programAppID;
		}

		public static void SetAccessTokenForNetwork(NetworkID netId, NetworkAccessToken accessToken)
		{
			Utility.s_dictTokens.Add(netId, accessToken);
		}

		public static NetworkAccessToken GetAccessTokenForNetwork(NetworkID netId)
		{
			NetworkAccessToken result;
			if (!Utility.s_dictTokens.TryGetValue(netId, out result))
			{
				result = new NetworkAccessToken();
			}
			return result;
		}
	}
}
