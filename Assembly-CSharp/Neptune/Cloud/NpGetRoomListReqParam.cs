using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class NpGetRoomListReqParam
	{
		public NpGetRoomListReqParam.Param param = new NpGetRoomListReqParam.Param();

		public class Param
		{
			public uint user_id;

			public int room_list_type;

			public List<RoomCondition> room_condition;
		}
	}
}
