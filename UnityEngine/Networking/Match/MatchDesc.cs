using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>A member contained in a ListMatchResponse.matches list. Each element describes an individual match.</para>
	/// </summary>
	public class MatchDesc : ResponseBase
	{
		/// <summary>
		///   <para>NetworkID of the match.</para>
		/// </summary>
		public NetworkID networkId { get; set; }

		/// <summary>
		///   <para>Name of the match.</para>
		/// </summary>
		public string name { get; set; }

		/// <summary>
		///   <para>The optional game defined Elo score for the match as a whole. The Elo score is averaged against all clients in a match and that value is used to produce better search results when listing available matches.
		/// If the Elo is provided the result set will be ordered according to the magnitude of the absoloute value of the difference of the a client searching for a match and the network average for all clients in each match. If the Elo score is not provided (and therefore 0 for all matches) the Elo score will not affect the search results.
		/// Each game can calculate this value as they wish according to whatever scale is best for that game.</para>
		/// </summary>
		public int averageEloScore { get; set; }

		/// <summary>
		///   <para>Max number of users that may connect to a match.</para>
		/// </summary>
		public int maxSize { get; set; }

		/// <summary>
		///   <para>Current number of users connected to a match.</para>
		/// </summary>
		public int currentSize { get; set; }

		/// <summary>
		///   <para>Describes if this match is considered private.</para>
		/// </summary>
		public bool isPrivate { get; set; }

		/// <summary>
		///   <para>Match attributes describing game specific features for this match. Each attribute is a key/value pair of a string key with a long value. Each match may have up to 10 of these values.
		/// The game is free to use this as desired to assist in finding better match results when clients search for matches to join.</para>
		/// </summary>
		public Dictionary<string, long> matchAttributes { get; set; }

		/// <summary>
		///   <para>The NodeID of the host in a matchmaker match.</para>
		/// </summary>
		public NodeID hostNodeId { get; set; }

		/// <summary>
		///   <para>Direct connection info for network games; This is not required for games utilizing matchmaker.</para>
		/// </summary>
		public List<MatchDirectConnectInfo> directConnectInfos { get; set; }

		/// <summary>
		///   <para>Provides string description of current class data.</para>
		/// </summary>
		public override string ToString()
		{
			return UnityString.Format("[{0}]-networkId:0x{1},name:{2},averageEloScore:{3},maxSize:{4},currentSize:{5},isPrivate:{6},matchAttributes.Count:{7},directConnectInfos.Count:{8}", new object[]
			{
				base.ToString(),
				this.networkId.ToString("X"),
				this.name,
				this.averageEloScore,
				this.maxSize,
				this.currentSize,
				this.isPrivate,
				(this.matchAttributes != null) ? this.matchAttributes.Count : 0,
				this.directConnectInfos.Count
			});
		}

		public override void Parse(object obj)
		{
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary != null)
			{
				this.networkId = (NetworkID)base.ParseJSONUInt64("networkId", obj, dictionary);
				this.name = base.ParseJSONString("name", obj, dictionary);
				this.maxSize = base.ParseJSONInt32("maxSize", obj, dictionary);
				this.currentSize = base.ParseJSONInt32("currentSize", obj, dictionary);
				this.isPrivate = base.ParseJSONBool("isPrivate", obj, dictionary);
				this.directConnectInfos = base.ParseJSONList<MatchDirectConnectInfo>("directConnectInfos", obj, dictionary);
				return;
			}
			throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
		}
	}
}
