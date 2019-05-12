using LitJson;
using System;
using TypeSerialize;

namespace Master
{
	public abstract class MasterBaseData<MasterDataT> : MasterBase where MasterDataT : WebAPI.ResponseData
	{
		protected MasterDataT data;

		public override void JsonToData(string json)
		{
			this.PrepareData(JsonMapper.ToObject<MasterDataT>(json));
		}

		public override void BinaryToData(byte[] binary)
		{
			MasterDataT src;
			TypeSerializeHelper.BytesToData<MasterDataT>(binary, out src);
			this.PrepareData(src);
		}

		public void SetResponse(MasterDataT response)
		{
			this.PrepareData(response);
		}

		protected virtual void PrepareData(MasterDataT src)
		{
			this.data = src;
		}

		public override WebAPI.ResponseData GetData()
		{
			return this.data;
		}

		public MasterDataT GetMasterData()
		{
			return this.data;
		}

		public override bool IsLocalSaveData()
		{
			return true;
		}

		public override void ClearData()
		{
			this.data = (MasterDataT)((object)null);
		}
	}
}
