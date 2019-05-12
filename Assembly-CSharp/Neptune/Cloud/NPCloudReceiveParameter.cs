using System;

namespace Neptune.Cloud
{
	public class NPCloudReceiveParameter<T>
	{
		public T body;

		public NpCloudReceiveOptionParameter option;

		public string resultCode;

		public string resultMsg;

		public long resTime;

		public string command;

		public uint sender;
	}
}
