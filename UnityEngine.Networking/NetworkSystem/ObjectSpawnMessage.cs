using System;

namespace UnityEngine.Networking.NetworkSystem
{
	internal class ObjectSpawnMessage : MessageBase
	{
		public NetworkInstanceId netId;

		public NetworkHash128 assetId;

		public Vector3 position;

		public byte[] payload;

		public Quaternion rotation;

		public override void Deserialize(NetworkReader reader)
		{
			this.netId = reader.ReadNetworkId();
			this.assetId = reader.ReadNetworkHash128();
			this.position = reader.ReadVector3();
			this.payload = reader.ReadBytesAndSize();
			uint num = 16u;
			if ((long)reader.Length - (long)((ulong)reader.Position) >= (long)((ulong)num))
			{
				this.rotation = reader.ReadQuaternion();
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(this.netId);
			writer.Write(this.assetId);
			writer.Write(this.position);
			writer.WriteBytesFull(this.payload);
			writer.Write(this.rotation);
		}
	}
}
