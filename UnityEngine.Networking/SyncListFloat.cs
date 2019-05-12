using System;

namespace UnityEngine.Networking
{
	public sealed class SyncListFloat : SyncList<float>
	{
		protected override void SerializeItem(NetworkWriter writer, float item)
		{
			writer.Write(item);
		}

		protected override float DeserializeItem(NetworkReader reader)
		{
			return reader.ReadSingle();
		}

		[Obsolete("ReadReference is now used instead")]
		public static SyncListFloat ReadInstance(NetworkReader reader)
		{
			ushort num = reader.ReadUInt16();
			SyncListFloat syncListFloat = new SyncListFloat();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncListFloat.AddInternal(reader.ReadSingle());
			}
			return syncListFloat;
		}

		public static void ReadReference(NetworkReader reader, SyncListFloat syncList)
		{
			ushort num = reader.ReadUInt16();
			syncList.Clear();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				syncList.AddInternal(reader.ReadSingle());
			}
		}

		public static void WriteInstance(NetworkWriter writer, SyncListFloat items)
		{
			writer.Write((ushort)items.Count);
			foreach (float num in items)
			{
				float value = num;
				writer.Write(value);
			}
		}
	}
}
