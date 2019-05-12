using System;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>Abstract base for requests, which includes common info in all requests.</para>
	/// </summary>
	public abstract class Request
	{
		/// <summary>
		///   <para>Matchmaker protocol version info.</para>
		/// </summary>
		public int version = 2;

		/// <summary>
		///   <para>SourceID for the current client, required in every request. This is generated from the Cloud API.</para>
		/// </summary>
		public SourceID sourceId { get; set; }

		/// <summary>
		///   <para>The Cloud Project Id for this game, required in every request. This is used to match games of the same type.</para>
		/// </summary>
		public string projectId { get; set; }

		/// <summary>
		///   <para>AppID for the current game, required in every request. This is generated from the Cloud API.</para>
		/// </summary>
		public AppID appId { get; set; }

		/// <summary>
		///   <para>The JSON encoded binary access token this client uses to authenticate its session for future commands.</para>
		/// </summary>
		public string accessTokenString { get; set; }

		/// <summary>
		///   <para>Domain for the request. All commands will be sandboxed to their own domain; For example no clients with domain 1 will see matches with domain 2. This can be used to prevent incompatible client versions from communicating.</para>
		/// </summary>
		public int domain { get; set; }

		/// <summary>
		///   <para>Accessor to verify if the contained data is a valid request with respect to initialized variables and accepted parameters.</para>
		/// </summary>
		public virtual bool IsValid()
		{
			return this.appId != AppID.Invalid && this.sourceId != SourceID.Invalid;
		}

		/// <summary>
		///   <para>Provides string description of current class data.</para>
		/// </summary>
		public override string ToString()
		{
			return UnityString.Format("[{0}]-SourceID:0x{1},AppID:0x{2},domain:{3}", new object[]
			{
				base.ToString(),
				this.sourceId.ToString("X"),
				this.appId.ToString("X"),
				this.domain
			});
		}
	}
}
