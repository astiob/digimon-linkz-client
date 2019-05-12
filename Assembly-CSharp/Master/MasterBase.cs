using System;
using WebAPIRequest;

namespace Master
{
	public abstract class MasterBase
	{
		public MasterId ID { get; set; }

		public int DataVolume { get; set; }

		public abstract RequestBase CreateRequest();

		public abstract void JsonToData(string json);

		public abstract void BinaryToData(byte[] binary);

		public abstract WebAPI.ResponseData GetData();

		public abstract bool IsLocalSaveData();

		public abstract string GetTableName();

		public abstract void ClearData();
	}
}
