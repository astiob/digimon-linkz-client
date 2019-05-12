using System;

namespace Neptune.Cloud
{
	public class NpForceLeaveRoomReqParam
	{
		public NpForceLeaveRoomReqParam.Param param = new NpForceLeaveRoomReqParam.Param();

		public class Param
		{
			public string room_id;

			public uint user_id;
		}
	}
}
