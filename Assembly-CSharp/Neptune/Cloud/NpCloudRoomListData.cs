using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class NpCloudRoomListData
	{
		public string url;

		public uint userId;

		public RoomListType roomListType;

		public RoomType roomType;

		public int page;

		public int limit;

		public List<RoomCondition> roomConditionList;
	}
}
