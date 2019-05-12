using System;
using System.Collections.Generic;

namespace Neptune.Cloud.Core
{
	public class NpCloudRoomSystem : INpCloudManagerSystem
	{
		private Dictionary<string, List<int>> mUserList;

		private Dictionary<string, NpRoomParameter> mJoinRoomList;

		private Action<NpRoomParameter> mJoinRoom;

		private Action<NpLeaveParameter> mLeaveRoom;

		private Action<NpCloudErrorData> mRequestException;

		public NpCloudRoomSystem()
		{
			this.mUserList = new Dictionary<string, List<int>>();
			this.mJoinRoomList = new Dictionary<string, NpRoomParameter>();
		}

		public void ClearData()
		{
			this.mUserList.Clear();
			this.mJoinRoomList.Clear();
		}

		public Action<NpRoomParameter> JoinRoom
		{
			get
			{
				return this.mJoinRoom;
			}
			set
			{
				this.mJoinRoom = value;
			}
		}

		public Action<NpLeaveParameter> LeaveRoom
		{
			get
			{
				return this.mLeaveRoom;
			}
			set
			{
				this.mLeaveRoom = value;
			}
		}

		public Action<NpCloudErrorData> RequestException
		{
			get
			{
				return this.mRequestException;
			}
			set
			{
				this.mRequestException = value;
			}
		}

		public IList<int> GetUserList(string roomId)
		{
			if (!this.IsRoom(roomId))
			{
				throw new NpCloudException(770, "ルームIDが異常です");
			}
			List<int> list = new List<int>(this.mUserList[roomId]);
			return list.AsReadOnly();
		}

		public bool IsRoom(string roomId)
		{
			return this.mJoinRoomList.ContainsKey(roomId);
		}

		public void Leave(string roomId)
		{
			this.RemoveRoomData(roomId);
			this.mLeaveRoom(new NpLeaveParameter());
		}

		private void RemoveRoomData(string roomId)
		{
			this.mUserList.Remove(roomId);
			this.mJoinRoomList.Remove(roomId);
		}

		public void Update()
		{
		}

		public void CloudExit(int exitCode, string message)
		{
		}

		public bool ReceiveException(NpCloudErrorData error)
		{
			if (error.command.Equals("9100") || error.command.Equals("9102") || error.command.Equals("9105") || error.command.Equals("9101") || error.command.Equals("9106"))
			{
				this.mRequestException(error);
				return true;
			}
			return false;
		}

		public bool Receive(string command, object param, long resTime)
		{
			if (param is NpCloudReceiveRoomParameter)
			{
				NpCloudReceiveRoomParameter response = param as NpCloudReceiveRoomParameter;
				return this.Join(command, response, resTime) || this.Leave(command, response, resTime) || this.RemoveRoom(command, response, resTime) || this.ForceLeave(command, response, resTime);
			}
			return false;
		}

		private bool RemoveRoom(string command, NpCloudReceiveRoomParameter response, long resTime)
		{
			if (command.Equals("9101"))
			{
				if (this.mJoinRoomList.ContainsKey(response.room_id))
				{
					this.mJoinRoomList.Remove(response.room_id);
				}
				if (this.mUserList.ContainsKey(response.room_id))
				{
					this.mUserList.Remove(response.room_id);
				}
				NpLeaveParameter obj = new NpLeaveParameter(LeaveTypeE.DeleteRoom, response.room_id, 0, 0, new List<int>(), NpUtil.MsTimestampToDateTime(resTime));
				this.mLeaveRoom(obj);
				return true;
			}
			return false;
		}

		private bool Join(string command, NpCloudReceiveRoomParameter response, long resTime)
		{
			if (command.Equals("9100"))
			{
				bool isNewMember = true;
				NpRoomParameter npRoomParameter = new NpRoomParameter(RoomJoinTypeE.Create, response.room_id, response.room_name, (RoomType)response.room_type, (int)NpCloudManager.GetInstance().UserID, isNewMember, response.owner, response.member_list, new List<RoomCondition>(), NpUtil.MsTimestampToDateTime(resTime));
				if (response.room_condition != null)
				{
					npRoomParameter.RoomCondition = response.room_condition;
				}
				List<int> member_list = response.member_list;
				if (!this.mJoinRoomList.ContainsKey(npRoomParameter.RoomId))
				{
					this.mJoinRoomList.Add(npRoomParameter.RoomId, npRoomParameter);
					this.mUserList.Add(npRoomParameter.RoomId, member_list);
				}
				else
				{
					this.mUserList[npRoomParameter.RoomId] = member_list;
					this.mJoinRoomList[npRoomParameter.RoomId] = npRoomParameter;
				}
				this.mJoinRoom(npRoomParameter);
				return true;
			}
			if (command.Equals("9102"))
			{
				NpRoomParameter npRoomParameter2 = new NpRoomParameter(RoomJoinTypeE.RoomJoin, response.room_id, response.room_name, (RoomType)response.room_type, response.member_joined, response.is_new_member != 0, response.owner, response.member_list, new List<RoomCondition>(), NpUtil.MsTimestampToDateTime(resTime));
				if (response.room_condition != null)
				{
					npRoomParameter2.RoomCondition = response.room_condition;
				}
				List<int> member_list2 = response.member_list;
				if (!this.mJoinRoomList.ContainsKey(npRoomParameter2.RoomId))
				{
					this.mJoinRoomList.Add(npRoomParameter2.RoomId, npRoomParameter2);
					this.mUserList.Add(npRoomParameter2.RoomId, member_list2);
				}
				else
				{
					this.mUserList[npRoomParameter2.RoomId] = member_list2;
					this.mJoinRoomList[npRoomParameter2.RoomId] = npRoomParameter2;
				}
				this.mJoinRoom(npRoomParameter2);
				return true;
			}
			return false;
		}

		private bool Leave(string command, NpCloudReceiveRoomParameter response, long resTime)
		{
			if (command.Equals("9105"))
			{
				if (response.member_list == null)
				{
					response.member_list = new List<int>();
				}
				this.mUserList[response.room_id] = response.member_list;
				this.mJoinRoomList[response.room_id].MemberList = response.member_list;
				this.mJoinRoomList[response.room_id].Owner = response.owner;
				NpLeaveParameter obj = new NpLeaveParameter(LeaveTypeE.Leave, response.room_id, response.member_left, response.owner, response.member_list, NpUtil.MsTimestampToDateTime(resTime));
				this.mLeaveRoom(obj);
				return true;
			}
			return false;
		}

		private bool ForceLeave(string command, NpCloudReceiveRoomParameter response, long resTime)
		{
			if (!command.Equals("9106"))
			{
				return false;
			}
			if (response.member_list == null)
			{
				return true;
			}
			this.mUserList[response.room_id] = response.member_list;
			this.mJoinRoomList[response.room_id].MemberList = response.member_list;
			NpLeaveParameter obj = new NpLeaveParameter(LeaveTypeE.ForceLeave, response.room_id, response.member_left, response.owner, response.member_list, NpUtil.MsTimestampToDateTime(resTime));
			this.mLeaveRoom(obj);
			return true;
		}
	}
}
