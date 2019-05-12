using System;
using UnityEngine;

namespace Network.Socket
{
	public sealed class SocketNpCloudListenerLostPacketSimulation : MonoBehaviour
	{
		[Header("パケットロスト率")]
		[Range(0f, 100f)]
		[SerializeField]
		public int packetLostRate;

		[NonSerialized]
		public SocketNpCloudListener target;

		public static void Create(SocketNpCloudListener target)
		{
		}
	}
}
