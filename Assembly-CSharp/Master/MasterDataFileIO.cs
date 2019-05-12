using Save;
using System;
using System.Collections;
using TypeSerialize;
using UnityEngine;

namespace Master
{
	public class MasterDataFileIO
	{
		private const string MASTER_DATA_VERSION_FILE_NAME = "MASTER_DATA_VERSION";

		private PersistentFile persistentFile = new PersistentFile(false);

		public IEnumerator ReadMasterDataFile(MasterBase masterData)
		{
			string fileName = this.GetFileName(masterData.ID.ToString());
			return this.persistentFile.Read(fileName, delegate(bool result, byte[] binary)
			{
				if (result)
				{
					try
					{
						masterData.BinaryToData(binary);
					}
					catch
					{
					}
				}
			});
		}

		public IEnumerator ReadMasterDataVersionFile(Action<GameWebAPI.RespDataCM_MDVersion> onRead)
		{
			string fileName = this.GetFileName("MASTER_DATA_VERSION");
			return this.persistentFile.Read(fileName, delegate(bool isSuccess, byte[] binary)
			{
				if (isSuccess)
				{
					try
					{
						if (onRead != null)
						{
							GameWebAPI.RespDataCM_MDVersion obj;
							TypeSerializeHelper.BytesToData<GameWebAPI.RespDataCM_MDVersion>(binary, out obj);
							onRead(obj);
						}
					}
					catch
					{
					}
				}
			});
		}

		public IEnumerator WriteMasterDataFile(MasterBase masterData)
		{
			byte[] array = this.MasterDataToBinary(masterData.GetData());
			if (array == null)
			{
				DataAlert dataAlert = new DataAlert();
				return dataAlert.OpenAlertJsonParseError();
			}
			string fileName = this.GetFileName(masterData.ID.ToString());
			return this.WriteBinaryFile(fileName, array);
		}

		public IEnumerator WriteMasterDataVersionFile(GameWebAPI.RespDataCM_MDVersion masterDataVersion)
		{
			byte[] array = this.MasterDataToBinary(masterDataVersion);
			if (array == null)
			{
				DataAlert dataAlert = new DataAlert();
				return dataAlert.OpenAlertJsonParseError();
			}
			string fileName = this.GetFileName("MASTER_DATA_VERSION");
			return this.WriteBinaryFile(fileName, array);
		}

		private byte[] MasterDataToBinary(WebAPI.ResponseData masterData)
		{
			byte[] result = null;
			try
			{
				TypeSerializeHelper.DataToBytes<WebAPI.ResponseData>(masterData, out result);
			}
			catch
			{
				global::Debug.LogWarning("MasterDataのSerializeでエラーが発生しました");
			}
			return result;
		}

		private IEnumerator WriteBinaryFile(string fileName, byte[] binary)
		{
			DataAlert alert = null;
			bool isSuccess = false;
			while (!isSuccess)
			{
				IEnumerator ie = this.persistentFile.Write(fileName, binary, delegate(bool result)
				{
					isSuccess = result;
				});
				while (ie.MoveNext())
				{
					object obj = ie.Current;
					yield return obj;
				}
				if (!isSuccess)
				{
					if (alert == null)
					{
						alert = new DataAlert();
					}
					ie = alert.OpenAlert(this.persistentFile.GetErrorType());
					while (ie.MoveNext())
					{
						object obj2 = ie.Current;
						yield return obj2;
					}
				}
			}
			yield break;
		}

		public string GetFileName(string idName)
		{
			return Application.persistentDataPath + "/MA_DT_" + idName + ".txt";
		}
	}
}
