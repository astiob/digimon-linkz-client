using System;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>Details about a UNET Matchmaker match.</para>
	/// </summary>
	public class MatchInfo
	{
		public MatchInfo(CreateMatchResponse matchResponse)
		{
			this.address = matchResponse.address;
			this.port = matchResponse.port;
			this.networkId = matchResponse.networkId;
			this.accessToken = new NetworkAccessToken(matchResponse.accessTokenString);
			this.nodeId = matchResponse.nodeId;
			this.usingRelay = matchResponse.usingRelay;
		}

		public MatchInfo(JoinMatchResponse matchResponse)
		{
			this.address = matchResponse.address;
			this.port = matchResponse.port;
			this.networkId = matchResponse.networkId;
			this.accessToken = new NetworkAccessToken(matchResponse.accessTokenString);
			this.nodeId = matchResponse.nodeId;
			this.usingRelay = matchResponse.usingRelay;
		}

		/// <summary>
		///   <para>IP address of the host of the match,.</para>
		/// </summary>
		public string address { get; private set; }

		/// <summary>
		///   <para>Port of the host of the match.</para>
		/// </summary>
		public int port { get; private set; }

		/// <summary>
		///   <para>The unique ID of this match.</para>
		/// </summary>
		public NetworkID networkId { get; private set; }

		/// <summary>
		///   <para>The binary access token this client uses to authenticate its session for future commands.</para>
		/// </summary>
		public NetworkAccessToken accessToken { get; private set; }

		/// <summary>
		///   <para>NodeID for this member client in the match.</para>
		/// </summary>
		public NodeID nodeId { get; private set; }

		/// <summary>
		///   <para>Flag to say if the math uses a relay server.</para>
		/// </summary>
		public bool usingRelay { get; private set; }

		public override string ToString()
		{
			return UnityString.Format("{0} @ {1}:{2} [{3},{4}]", new object[]
			{
				this.networkId,
				this.address,
				this.port,
				this.nodeId,
				this.usingRelay
			});
		}
	}
}
