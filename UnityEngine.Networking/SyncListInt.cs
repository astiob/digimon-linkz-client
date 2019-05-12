using System;

namespace UnityEngine.Networking
{
	public class SyncListInt : SyncList<int>
	{
		protected override void SerializeItem(NetworkWriter writer, int item)
		{
			writer.WritePackedUInt32((uint)item);
		}

		protected override int DeserializeItem(NetworkReader reader)
		{
			return (int)reader.ReadPackedUInt32();
		}

		[Obsolete("ReadReference is now used instead")]
		public static SyncListInt ReadInstance(NetworkReader reader)
		{
			ushort num = reader.ReadUInt16();
			SyncListInt syncListInt = new SyncListInt();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncListInt.AddInternal((int)reader.ReadPackedUInt32());
			}
			return syncListInt;
		}

		public static void ReadReference(NetworkReader reader, SyncListInt syncList)
		{
			ushort num = reader.ReadUInt16();
			syncList.Clear();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncList.AddInternal((int)reader.ReadPackedUInt32());
			}
		}

		public static void WriteInstance(NetworkWriter writer, SyncListInt items)
		{
			writer.Write((ushort)items.Count);
			foreach (int value in items)
			{
				writer.WritePackedUInt32((uint)value);
			}
		}
	}
}
