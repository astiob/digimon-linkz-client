using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class RoomList
	{
		public string room_id;

		public string room_name;

		public List<int> member_list;

		public int owner;

		public List<RoomCondition> room_condition;
	}
}
