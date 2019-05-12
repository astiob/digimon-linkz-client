using System;

namespace UnityEngine.Networking.NetworkSystem
{
	internal class CRCMessage : MessageBase
	{
		public CRCMessageEntry[] scripts;

		public override void Deserialize(NetworkReader reader)
		{
			int num = (int)reader.ReadUInt16();
			this.scripts = new CRCMessageEntry[num];
			for (int i = 0; i < this.scripts.Length; i++)
			{
				CRCMessageEntry crcmessageEntry = default(CRCMessageEntry);
				crcmessageEntry.name = reader.ReadString();
				crcmessageEntry.channel = reader.ReadByte();
				this.scripts[i] = crcmessageEntry;
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((ushort)this.scripts.Length);
			foreach (CRCMessageEntry crcmessageEntry in this.scripts)
			{
				writer.Write(crcmessageEntry.name);
				writer.Write(crcmessageEntry.channel);
			}
		}
	}
}
