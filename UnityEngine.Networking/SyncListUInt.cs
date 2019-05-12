using System;

namespace UnityEngine.Networking
{
	public class SyncListUInt : SyncList<uint>
	{
		protected override void SerializeItem(NetworkWriter writer, uint item)
		{
			writer.WritePackedUInt32(item);
		}

		protected override uint DeserializeItem(NetworkReader reader)
		{
			return reader.ReadPackedUInt32();
		}

		[Obsolete("ReadReference is now used instead")]
		public static SyncListUInt ReadInstance(NetworkReader reader)
		{
			ushort num = reader.ReadUInt16();
			SyncListUInt syncListUInt = new SyncListUInt();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncListUInt.AddInternal(reader.ReadPackedUInt32());
			}
			return syncListUInt;
		}

		public static void ReadReference(NetworkReader reader, SyncListUInt syncList)
		{
			ushort num = reader.ReadUInt16();
			syncList.Clear();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncList.AddInternal(reader.ReadPackedUInt32());
			}
		}

		public static void WriteInstance(NetworkWriter writer, SyncListUInt items)
		{
			writer.Write((ushort)items.Count);
			for (int i = 0; i < items.Count; i++)
			{
				writer.WritePackedUInt32(items[i]);
			}
		}
	}
}
