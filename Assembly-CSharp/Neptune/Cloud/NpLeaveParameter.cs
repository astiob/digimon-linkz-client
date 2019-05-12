using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class NpLeaveParameter
	{
		public NpLeaveParameter()
		{
			this.LeaveType = LeaveTypeE.None;
			this.RoomId = string.Empty;
			this.MemberLeft = 0;
			this.Owner = 0;
			this.MemberList = new List<int>();
			this.ResponseTime = default(DateTime);
		}

		public NpLeaveParameter(LeaveTypeE leaveType, string roomId, int memberLeft, int owner, List<int> memberList, DateTime responseTime)
		{
			this.LeaveType = leaveType;
			this.RoomId = roomId;
			this.MemberLeft = memberLeft;
			this.Owner = owner;
			this.MemberList = memberList;
			this.ResponseTime = responseTime;
		}

		public LeaveTypeE LeaveType { get; set; }

		public string RoomId { get; set; }

		public int MemberLeft { get; set; }

		public int Owner { get; set; }

		public List<int> MemberList { get; set; }

		public DateTime ResponseTime { get; set; }

		public void PrintLog()
		{
			Debug.Log(string.Format("NpLeaveParameter：LeaveType={0}, RoomId={1}, MemberLeft={2}, Owner={3}, MemberList={4}, ResponseTime={5}", new object[]
			{
				this.LeaveType,
				this.RoomId,
				this.MemberLeft,
				this.Owner,
				this.MemberList,
				this.ResponseTime
			}));
		}
	}
}
