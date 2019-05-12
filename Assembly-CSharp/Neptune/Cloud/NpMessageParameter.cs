using System;

namespace Neptune.Cloud
{
	public class NpMessageParameter
	{
		public NpMessageParameter()
		{
			this.RoomId = string.Empty;
			this.UserId = 0;
			this.Message = string.Empty;
			this.ResponseTime = default(DateTime);
		}

		public NpMessageParameter(string roomId, int userId, string message, DateTime responseTime)
		{
			this.RoomId = roomId;
			this.UserId = userId;
			this.Message = message;
			this.ResponseTime = responseTime;
		}

		public NpMessageParameter(NpMessageParameter data)
		{
			this.RoomId = data.RoomId;
			this.UserId = data.UserId;
			this.Message = data.Message;
			this.ResponseTime = data.ResponseTime;
		}

		public string RoomId { get; set; }

		public int UserId { get; set; }

		public string Message { get; set; }

		public DateTime ResponseTime { get; set; }

		public void PrintLog()
		{
			Debug.Log(string.Format("NpMessageParameter : RoomId={0}, UserId={1}, Message={2}, ResponseTime={3}", new object[]
			{
				this.RoomId,
				this.UserId,
				this.Message,
				this.ResponseTime
			}));
		}
	}
}
