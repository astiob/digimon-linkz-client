using System;

namespace Neptune.Cloud
{
	public class NpCloudHeader
	{
		public const uint venusRequestDataCapacity = 4294967295u;

		public const int vensuHeaderLengthSize = 4;

		public const int venusHeaderMsgTypeSize = 1;

		public const int venusHeaderMsgTypeIndex = 4;

		public const int venusHeaderReserveSize = 5;

		public const int memoryStreamSize = 8192;

		public const int venusHeaderSize = 10;
	}
}
