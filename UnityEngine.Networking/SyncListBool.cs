using System;

namespace UnityEngine.Networking
{
	public class SyncListBool : SyncList<bool>
	{
		protected override void SerializeItem(NetworkWriter writer, bool item)
		{
			writer.Write(item);
		}

		protected override bool DeserializeItem(NetworkReader reader)
		{
			return reader.ReadBoolean();
		}

		[Obsolete("ReadReference is now used instead")]
		public static SyncListBool ReadInstance(NetworkReader reader)
		{
			ushort num = reader.ReadUInt16();
			SyncListBool syncListBool = new SyncListBool();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncListBool.AddInternal(reader.ReadBoolean());
			}
			return syncListBool;
		}

		public static void ReadReference(NetworkReader reader, SyncListBool syncList)
		{
			ushort num = reader.ReadUInt16();
			syncList.Clear();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncList.AddInternal(reader.ReadBoolean());
			}
		}

		public static void WriteInstance(NetworkWriter writer, SyncListBool items)
		{
			writer.Write((ushort)items.Count);
			foreach (bool value in items)
			{
				writer.Write(value);
			}
		}
	}
}
