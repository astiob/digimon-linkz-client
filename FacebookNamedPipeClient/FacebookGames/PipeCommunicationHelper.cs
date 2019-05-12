using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookGames
{
	public class PipeCommunicationHelper
	{
		public static void SendPacket(NamedPipeStream stream, PipePacket packet)
		{
			string s = packet.Serialize();
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			stream.Write(bytes, 0, bytes.Length);
		}

		public static T ReadPacket<T>(NamedPipeStream stream) where T : PipePacket
		{
			return PipePacket.Deserialize<T>(PipeCommunicationHelper.ReadPacketMessage(stream));
		}

		public static string ReadPacketMessage(NamedPipeStream stream)
		{
			List<byte> list = new List<byte>(1024);
			byte[] array = new byte[1024];
			for (;;)
			{
				int num = stream.Read(array, 0, array.Length);
				if (num == -1)
				{
					break;
				}
				for (int i = 0; i < num; i++)
				{
					list.Add(array[i]);
				}
				if (stream.IsMessageComplete)
				{
					goto Block_3;
				}
			}
			return null;
			Block_3:
			return Encoding.UTF8.GetString(list.ToArray());
		}
	}
}
