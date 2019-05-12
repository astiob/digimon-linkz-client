using System;

namespace UnityEngine.Networking.NetworkSystem
{
	public class ReconnectMessage : MessageBase
	{
		public int oldConnectionId;

		public short playerControllerId;

		public NetworkInstanceId netId;

		public int msgSize;

		public byte[] msgData;

		public override void Deserialize(NetworkReader reader)
		{
			this.oldConnectionId = (int)reader.ReadPackedUInt32();
			this.playerControllerId = (short)reader.ReadPackedUInt32();
			this.netId = reader.ReadNetworkId();
			this.msgData = reader.ReadBytesAndSize();
			this.msgSize = this.msgData.Length;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)this.oldConnectionId);
			writer.WritePackedUInt32((uint)this.playerControllerId);
			writer.Write(this.netId);
			writer.WriteBytesAndSize(this.msgData, this.msgSize);
		}
	}
}
