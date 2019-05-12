using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class NpRoomRequestParameter
	{
		public NpRoomRequestParameter.Param param = new NpRoomRequestParameter.Param();

		public class Param
		{
			public int room_type = 4;

			public uint user_id;

			public string room_name;

			public List<RoomCondition> room_condition;
		}
	}
}
