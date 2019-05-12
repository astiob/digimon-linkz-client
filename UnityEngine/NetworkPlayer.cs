using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The NetworkPlayer is a data structure with which you can locate another player over the network.</para>
	/// </summary>
	public struct NetworkPlayer
	{
		internal int index;

		public NetworkPlayer(string ip, int port)
		{
			Debug.LogError("Not yet implemented");
			this.index = 0;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Internal_GetIPAddress(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetPort(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Internal_GetExternalIP();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetExternalPort();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Internal_GetLocalIP();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetLocalPort();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetPlayerIndex();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Internal_GetGUID(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Internal_GetLocalGUID();

		public override int GetHashCode()
		{
			return this.index.GetHashCode();
		}

		public override bool Equals(object other)
		{
			return other is NetworkPlayer && ((NetworkPlayer)other).index == this.index;
		}

		/// <summary>
		///   <para>The IP address of this player.</para>
		/// </summary>
		public string ipAddress
		{
			get
			{
				if (this.index == NetworkPlayer.Internal_GetPlayerIndex())
				{
					return NetworkPlayer.Internal_GetLocalIP();
				}
				return NetworkPlayer.Internal_GetIPAddress(this.index);
			}
		}

		/// <summary>
		///   <para>The port of this player.</para>
		/// </summary>
		public int port
		{
			get
			{
				if (this.index == NetworkPlayer.Internal_GetPlayerIndex())
				{
					return NetworkPlayer.Internal_GetLocalPort();
				}
				return NetworkPlayer.Internal_GetPort(this.index);
			}
		}

		/// <summary>
		///   <para>The GUID for this player, used when connecting with NAT punchthrough.</para>
		/// </summary>
		public string guid
		{
			get
			{
				if (this.index == NetworkPlayer.Internal_GetPlayerIndex())
				{
					return NetworkPlayer.Internal_GetLocalGUID();
				}
				return NetworkPlayer.Internal_GetGUID(this.index);
			}
		}

		/// <summary>
		///   <para>Returns the index number for this network player.</para>
		/// </summary>
		public override string ToString()
		{
			return this.index.ToString();
		}

		/// <summary>
		///   <para>Returns the external IP address of the network interface.</para>
		/// </summary>
		public string externalIP
		{
			get
			{
				return NetworkPlayer.Internal_GetExternalIP();
			}
		}

		/// <summary>
		///   <para>Returns the external port of the network interface.</para>
		/// </summary>
		public int externalPort
		{
			get
			{
				return NetworkPlayer.Internal_GetExternalPort();
			}
		}

		internal static NetworkPlayer unassigned
		{
			get
			{
				NetworkPlayer result;
				result.index = -1;
				return result;
			}
		}

		public static bool operator ==(NetworkPlayer lhs, NetworkPlayer rhs)
		{
			return lhs.index == rhs.index;
		}

		public static bool operator !=(NetworkPlayer lhs, NetworkPlayer rhs)
		{
			return lhs.index != rhs.index;
		}
	}
}
