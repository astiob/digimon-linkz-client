using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class NpCloudReceiveRoomParameter
	{
		public int result;

		public string detail;

		public uint errorCode;

		public string errorMsg;

		public string room_id;

		public int room_type;

		public List<int> member_list;

		public string room_name;

		public List<RoomCondition> room_condition;

		public int owner;

		public int member_left;

		public int member_joined;

		public int is_new_member;

		public List<RoomList> room_list;
	}
}
