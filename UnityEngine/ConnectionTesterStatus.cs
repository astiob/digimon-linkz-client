using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The various test results the connection tester may return with.</para>
	/// </summary>
	public enum ConnectionTesterStatus
	{
		/// <summary>
		///   <para>Some unknown error occurred.</para>
		/// </summary>
		Error = -2,
		/// <summary>
		///   <para>Test result undetermined, still in progress.</para>
		/// </summary>
		Undetermined,
		[Obsolete("No longer returned, use newer connection tester enums instead.")]
		PrivateIPNoNATPunchthrough,
		[Obsolete("No longer returned, use newer connection tester enums instead.")]
		PrivateIPHasNATPunchThrough,
		/// <summary>
		///   <para>Public IP address detected and game listen port is accessible to the internet.</para>
		/// </summary>
		PublicIPIsConnectable,
		/// <summary>
		///   <para>Public IP address detected but the port is not connectable from the internet.</para>
		/// </summary>
		PublicIPPortBlocked,
		/// <summary>
		///   <para>Public IP address detected but server is not initialized and no port is listening.</para>
		/// </summary>
		PublicIPNoServerStarted,
		/// <summary>
		///   <para>Port-restricted NAT type, can do NAT punchthrough to everyone except symmetric.</para>
		/// </summary>
		LimitedNATPunchthroughPortRestricted,
		/// <summary>
		///   <para>Symmetric NAT type, cannot do NAT punchthrough to other symmetric types nor port restricted type.</para>
		/// </summary>
		LimitedNATPunchthroughSymmetric,
		/// <summary>
		///   <para>Full cone type, NAT punchthrough fully supported.</para>
		/// </summary>
		NATpunchthroughFullCone,
		/// <summary>
		///   <para>Address-restricted cone type, NAT punchthrough fully supported.</para>
		/// </summary>
		NATpunchthroughAddressRestrictedCone
	}
}
