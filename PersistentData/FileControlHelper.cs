using System;
using System.Collections;

namespace PersistentData
{
	public sealed class FileControlHelper
	{
		private byte[] \uE000;

		private byte[] \uE001;

		public void SetCryptoData(byte[] key, byte[] iv)
		{
			this.\uE000 = key;
			this.\uE001 = iv;
		}

		private CryptoHelper \uE000()
		{
			return new CryptoHelper(this.\uE000, this.\uE001);
		}

		public IEnumerator Read(string filePath, Action<Type> callbackException, Action<byte[]> callbackData)
		{
			FileControlHelper.\uE002 uE = new FileControlHelper.\uE002(0);
			uE.<>4__this = this;
			uE.filePath = filePath;
			uE.callbackException = callbackException;
			uE.callbackData = callbackData;
			return uE;
		}

		public IEnumerator Write(string filePath, byte[] binary, Action<Type> onCompleted)
		{
			FileControlHelper.\uE003 uE = new FileControlHelper.\uE003(0);
			uE.<>4__this = this;
			uE.filePath = filePath;
			uE.binary = binary;
			uE.onCompleted = onCompleted;
			return uE;
		}

		public IEnumerator Decrypt(byte[] bytes, Action<byte[]> onCompleted)
		{
			FileControlHelper.\uE009 uE = new FileControlHelper.\uE009(0);
			uE.<>4__this = this;
			uE.bytes = bytes;
			uE.onCompleted = onCompleted;
			return uE;
		}
	}
}
