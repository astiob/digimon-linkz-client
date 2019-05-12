using System;

namespace Neptune.Cloud
{
	public enum NpCloudHeaderType : byte
	{
		CommonMsg = 1,
		TextFile,
		BinaryFile,
		PingMsg = 255,
		PongMsg = 254
	}
}
