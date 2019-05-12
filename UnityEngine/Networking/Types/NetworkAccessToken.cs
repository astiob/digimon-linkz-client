using System;

namespace UnityEngine.Networking.Types
{
	public class NetworkAccessToken
	{
		private const int NETWORK_ACCESS_TOKEN_SIZE = 64;

		public byte[] array;

		public NetworkAccessToken()
		{
			this.array = new byte[64];
		}

		public NetworkAccessToken(byte[] array)
		{
			this.array = array;
		}

		public NetworkAccessToken(string strArray)
		{
			this.array = Convert.FromBase64String(strArray);
		}

		public string GetByteString()
		{
			return Convert.ToBase64String(this.array);
		}

		public bool IsValid()
		{
			if (this.array == null || this.array.Length != 64)
			{
				return false;
			}
			bool result = false;
			foreach (byte b in this.array)
			{
				if (b != 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
