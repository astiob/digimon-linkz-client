using System;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>JSON object to request a UNET match destruction.</para>
	/// </summary>
	public class DestroyMatchRequest : Request
	{
		/// <summary>
		///   <para>NetworkID of the match to destroy.</para>
		/// </summary>
		public NetworkID networkId { get; set; }

		/// <summary>
		///   <para>Provides string description of current class data.</para>
		/// </summary>
		public override string ToString()
		{
			return UnityString.Format("[{0}]-networkId:0x{1}", new object[]
			{
				base.ToString(),
				this.networkId.ToString("X")
			});
		}

		/// <summary>
		///   <para>Accessor to verify if the contained data is a valid request with respect to initialized variables and accepted parameters.</para>
		/// </summary>
		public override bool IsValid()
		{
			return base.IsValid() && this.networkId != NetworkID.Invalid;
		}
	}
}
