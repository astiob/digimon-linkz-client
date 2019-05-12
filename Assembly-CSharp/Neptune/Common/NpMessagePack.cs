using CROOZ.Chopin.Core;
using System;
using System.IO;

namespace Neptune.Common
{
	public static class NpMessagePack
	{
		public static void Pack(object data, string stream, NpPackType type)
		{
			CpInazumaMsgPack.PackMode = (CpCustomDataPackMode)type;
			CpInazumaMsgPack.Pack(data, stream);
		}

		public static void Pack(object data, Stream stream, NpPackType type)
		{
			CpInazumaMsgPack.PackMode = (CpCustomDataPackMode)type;
			CpInazumaMsgPack.Pack(data, stream);
		}

		public static byte[] Pack<T>(T data)
		{
			MemoryStream memoryStream = new MemoryStream();
			CpInazumaMsgPack.Pack(data, memoryStream);
			return memoryStream.ToArray();
		}

		public static void Pack<T>(T data, MemoryStream ms)
		{
			CpInazumaMsgPack.Pack(data, ms);
		}

		public static T Unpack<T>(byte[] data)
		{
			return CpInazumaMsgPack.Unpack<T>(data);
		}

		public static T Unpack<T>(string data)
		{
			return CpInazumaMsgPack.Unpack<T>(data);
		}

		public static T Unpack<T>(Stream data)
		{
			return CpInazumaMsgPack.Unpack<T>(data);
		}

		public static string Dump(byte[] dumpByte)
		{
			return CpInazumaMsgPack.Dump(dumpByte);
		}

		public static string Dump(Stream stream)
		{
			return CpInazumaMsgPack.Dump(stream);
		}
	}
}
