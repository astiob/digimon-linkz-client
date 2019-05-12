using System;

namespace Neptune.Cloud
{
	public class NpJoinLeaveRoomReqParam
	{
		public NpJoinLeaveRoomReqParam.Param param = new NpJoinLeaveRoomReqParam.Param();

		public class Param
		{
			public string room_id;

			public uint user_id;
		}
	}
}
