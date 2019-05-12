using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>JSON response for a CreateMatchRequest. It contains all information necessdary to continue joining a match.</para>
	/// </summary>
	public class CreateMatchResponse : BasicResponse
	{
		/// <summary>
		///   <para>Network address to connect to in order to join the match.</para>
		/// </summary>
		public string address { get; set; }

		/// <summary>
		///   <para>Network port to connect to in order to join the match.</para>
		/// </summary>
		public int port { get; set; }

		/// <summary>
		///   <para>The network id for the match created.</para>
		/// </summary>
		public NetworkID networkId { get; set; }

		/// <summary>
		///   <para>JSON encoding for the binary access token this client uses to authenticate its session for future commands.</para>
		/// </summary>
		public string accessTokenString { get; set; }

		/// <summary>
		///   <para>NodeId for the requesting client in the created match.</para>
		/// </summary>
		public NodeID nodeId { get; set; }

		/// <summary>
		///   <para>If the match is hosted by a relay server.</para>
		/// </summary>
		public bool usingRelay { get; set; }

		/// <summary>
		///   <para>Provides string description of current class data.</para>
		/// </summary>
		public override string ToString()
		{
			return UnityString.Format("[{0}]-address:{1},port:{2},networkId:0x{3},nodeId:0x{4},usingRelay:{5}", new object[]
			{
				base.ToString(),
				this.address,
				this.port,
				this.networkId.ToString("X"),
				this.nodeId.ToString("X"),
				this.usingRelay
			});
		}

		public override void Parse(object obj)
		{
			base.Parse(obj);
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary != null)
			{
				this.address = base.ParseJSONString("address", obj, dictionary);
				this.port = base.ParseJSONInt32("port", obj, dictionary);
				this.networkId = (NetworkID)base.ParseJSONUInt64("networkId", obj, dictionary);
				this.accessTokenString = base.ParseJSONString("accessTokenString", obj, dictionary);
				this.nodeId = (NodeID)base.ParseJSONUInt16("nodeId", obj, dictionary);
				this.usingRelay = base.ParseJSONBool("usingRelay", obj, dictionary);
				return;
			}
			throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
		}
	}
}
