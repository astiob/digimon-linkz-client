using System;
using System.Collections.Generic;

namespace Network.Socket
{
	public sealed class SynchronizeInfo
	{
		public string hash;

		public Dictionary<string, object> packet;

		public int count;

		public float interval;

		public float sendTime;

		public List<int> toUserIdList;

		public Func<int, bool> onResend;

		public Action<bool> onSynchronized;

		public bool delete;
	}
}
