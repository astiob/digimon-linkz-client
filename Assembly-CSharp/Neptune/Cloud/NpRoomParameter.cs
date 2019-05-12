using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class NpRoomParameter
	{
		public NpRoomParameter()
		{
			this.RoomJoinType = RoomJoinTypeE.None;
			this.RoomId = string.Empty;
			this.RoomName = string.Empty;
			this.RoomType = RoomType.Global;
			this.MemberJoined = 0;
			this.IsNewMember = false;
			this.Owner = 0;
			this.MemberList = new List<int>();
			this.RoomCondition = new List<RoomCondition>();
			this.ResponseTime = default(DateTime);
		}

		public NpRoomParameter(RoomJoinTypeE roomJoinType, string roomId, string roomName, RoomType roomType, int memberJoined, bool isNewMember, int owner, List<int> memberList, List<RoomCondition> roomCondition, DateTime responseTime)
		{
			this.RoomJoinType = roomJoinType;
			this.RoomId = roomId;
			this.RoomName = roomName;
			this.RoomType = roomType;
			this.MemberJoined = memberJoined;
			this.IsNewMember = isNewMember;
			this.Owner = owner;
			this.MemberList = memberList;
			this.RoomCondition = roomCondition;
			this.ResponseTime = responseTime;
		}

		public NpRoomParameter(NpRoomParameter data)
		{
			this.RoomJoinType = data.RoomJoinType;
			this.RoomId = data.RoomId;
			this.RoomName = data.RoomName;
			this.RoomType = data.RoomType;
			this.MemberJoined = data.MemberJoined;
			this.IsNewMember = data.IsNewMember;
			this.Owner = data.Owner;
			this.MemberList = data.MemberList;
			this.RoomCondition = data.RoomCondition;
			this.ResponseTime = data.ResponseTime;
		}

		public RoomJoinTypeE RoomJoinType { get; set; }

		public string RoomId { get; set; }

		public string RoomName { get; set; }

		public RoomType RoomType { get; set; }

		public int Owner { get; set; }

		public int MemberJoined { get; set; }

		public bool IsNewMember { get; set; }

		public List<int> MemberList { get; set; }

		public List<RoomCondition> RoomCondition { get; set; }

		public DateTime ResponseTime { get; set; }

		public void PrintLog()
		{
			Debug.Log(string.Format("NpRoomParameter：RoomJoinType={0}, RoomId={1}, RoomName={2}, RoomType={3}, Owner={4}, MemberJoined={5}, IsNewMember={6}, MemberList={7}, RoomCondition={8}, ResponseTime={9}", new object[]
			{
				this.RoomJoinType,
				this.RoomId,
				this.RoomName,
				this.RoomType,
				this.Owner,
				this.MemberJoined,
				this.IsNewMember,
				this.MemberList,
				this.RoomCondition,
				this.ResponseTime
			}));
		}
	}
}
