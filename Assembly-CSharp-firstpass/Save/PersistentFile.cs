using PersistentData;
using System;
using System.Collections;
using System.IO;
using System.Text;
using TypeSerialize;
using UnityEngine;

namespace Save
{
	public sealed class PersistentFile
	{
		private FileControlHelper fileControlHelper;

		private bool isException;

		private Type exceptionType;

		private bool iCloudBackup;

		public PersistentFile(bool iCloudBackup)
		{
			this.fileControlHelper = new FileControlHelper();
			if (PlayerPrefs.HasKey("uuid"))
			{
				string @string = PlayerPrefs.GetString("uuid");
				int num = 8;
				if (num <= @string.Length)
				{
					byte[] array = new byte[num];
					byte[] array2 = new byte[num];
					Encoding.ASCII.GetBytes(@string, 0, num, array, 0);
					Encoding.ASCII.GetBytes(@string, @string.Length - num, num, array2, 0);
					this.fileControlHelper.SetCryptoData(array, array2);
				}
			}
			else
			{
				this.fileControlHelper.SetCryptoData(null, null);
			}
			this.iCloudBackup = iCloudBackup;
		}

		public IEnumerator Write(string filePath, string jsonText, Action<bool> onCompleted)
		{
			this.isException = false;
			byte[] binary = null;
			try
			{
				TypeSerializeHelper.DataToBytes<string>(jsonText, out binary);
			}
			catch
			{
			}
			return this.fileControlHelper.Write(filePath, binary, delegate(Type exceptionType)
			{
				if (exceptionType != null)
				{
					this.isException = true;
					this.exceptionType = exceptionType;
				}
				if (!this.iCloudBackup)
				{
				}
				if (onCompleted != null)
				{
					onCompleted(!this.isException);
				}
			});
		}

		public IEnumerator Write(string filePath, byte[] binary, Action<bool> onCompleted)
		{
			this.isException = false;
			return this.fileControlHelper.Write(filePath, binary, delegate(Type exceptionType)
			{
				if (exceptionType != null)
				{
					this.isException = true;
					this.exceptionType = exceptionType;
				}
				if (!this.iCloudBackup)
				{
				}
				if (onCompleted != null)
				{
					onCompleted(!this.isException);
				}
			});
		}

		public IEnumerator Read(string filePath, Action<bool, string> onCompleted)
		{
			this.isException = false;
			return this.fileControlHelper.Read(filePath, new Action<Type>(this.OnFailedFileRead), delegate(byte[] loadData)
			{
				if (onCompleted != null)
				{
					string empty = string.Empty;
					try
					{
						TypeSerializeHelper.BytesToData<string>(loadData, out empty);
					}
					catch (Exception ex)
					{
						this.OnFailedFileRead(ex.GetType());
					}
					onCompleted(false == this.isException, empty);
				}
			});
		}

		public IEnumerator Read(string filePath, Action<bool, byte[]> onCompleted)
		{
			this.isException = false;
			return this.fileControlHelper.Read(filePath, new Action<Type>(this.OnFailedFileRead), delegate(byte[] loadData)
			{
				if (onCompleted != null)
				{
					onCompleted(false == this.isException, loadData);
				}
			});
		}

		private void OnFailedFileRead(Type exceptionType)
		{
			if (exceptionType != null)
			{
				this.isException = true;
				this.exceptionType = exceptionType;
			}
		}

		public PersistentFile.ErrorType GetErrorType()
		{
			Type type = null;
			if (this.isException)
			{
				type = this.exceptionType;
			}
			PersistentFile.ErrorType result = PersistentFile.ErrorType.NONE;
			if (typeof(IOException) == type)
			{
				result = PersistentFile.ErrorType.IO;
			}
			else if (typeof(Exception) == type)
			{
				result = PersistentFile.ErrorType.OTHER;
			}
			else if (type != null)
			{
				result = PersistentFile.ErrorType.SECURITY;
			}
			return result;
		}

		public enum ErrorType
		{
			NONE,
			IO,
			SECURITY,
			OTHER
		}
	}
}
