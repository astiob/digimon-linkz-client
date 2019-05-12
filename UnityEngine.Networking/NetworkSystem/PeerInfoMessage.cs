using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.NetworkSystem
{
	public class PeerInfoMessage : MessageBase
	{
		public int connectionId;

		public string address;

		public int port;

		public bool isHost;

		public bool isYou;

		public PeerInfoPlayer[] playerIds;

		public override void Deserialize(NetworkReader reader)
		{
			this.connectionId = (int)reader.ReadPackedUInt32();
			this.address = reader.ReadString();
			this.port = (int)reader.ReadPackedUInt32();
			this.isHost = reader.ReadBoolean();
			this.isYou = reader.ReadBoolean();
			uint num = reader.ReadPackedUInt32();
			if (num > 0u)
			{
				List<PeerInfoPlayer> list = new List<PeerInfoPlayer>();
				for (uint num2 = 0u; num2 < num; num2 += 1u)
				{
					PeerInfoPlayer item;
					item.netId = reader.ReadNetworkId();
					item.playerControllerId = (short)reader.ReadPackedUInt32();
					list.Add(item);
				}
				this.playerIds = list.ToArray();
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)this.connectionId);
			writer.Write(this.address);
			writer.WritePackedUInt32((uint)this.port);
			writer.Write(this.isHost);
			writer.Write(this.isYou);
			if (this.playerIds == null)
			{
				writer.WritePackedUInt32(0u);
			}
			else
			{
				writer.WritePackedUInt32((uint)this.playerIds.Length);
				for (int i = 0; i < this.playerIds.Length; i++)
				{
					writer.Write(this.playerIds[i].netId);
					writer.WritePackedUInt32((uint)this.playerIds[i].playerControllerId);
				}
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"PeerInfo conn:",
				this.connectionId,
				" addr:",
				this.address,
				":",
				this.port,
				" host:",
				this.isHost,
				" isYou:",
				this.isYou
			});
		}
	}
}
