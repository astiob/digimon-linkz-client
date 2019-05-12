using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>Class describing a client in a network match.</para>
	/// </summary>
	public class MatchDirectConnectInfo : ResponseBase
	{
		/// <summary>
		///   <para>NodeID of the client described in this direct connect info.</para>
		/// </summary>
		public NodeID nodeId { get; set; }

		/// <summary>
		///   <para>Public address the client described by this class provided.</para>
		/// </summary>
		public string publicAddress { get; set; }

		/// <summary>
		///   <para>Private address the client described by this class provided.</para>
		/// </summary>
		public string privateAddress { get; set; }

		public override string ToString()
		{
			return UnityString.Format("[{0}]-nodeId:{1},publicAddress:{2},privateAddress:{3}", new object[]
			{
				base.ToString(),
				this.nodeId,
				this.publicAddress,
				this.privateAddress
			});
		}

		public override void Parse(object obj)
		{
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary != null)
			{
				this.nodeId = (NodeID)base.ParseJSONUInt16("nodeId", obj, dictionary);
				this.publicAddress = base.ParseJSONString("publicAddress", obj, dictionary);
				this.privateAddress = base.ParseJSONString("privateAddress", obj, dictionary);
				return;
			}
			throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
		}
	}
}
