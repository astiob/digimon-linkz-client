using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes network reachability options.</para>
	/// </summary>
	public enum NetworkReachability
	{
		/// <summary>
		///   <para>Network is not reachable.</para>
		/// </summary>
		NotReachable,
		/// <summary>
		///   <para>Network is reachable via carrier data network.</para>
		/// </summary>
		ReachableViaCarrierDataNetwork,
		/// <summary>
		///   <para>Network is reachable via WiFi or cable.</para>
		/// </summary>
		ReachableViaLocalAreaNetwork
	}
}
