using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	public abstract class MD2 : HashAlgorithm
	{
		protected MD2()
		{
			this.HashSizeValue = 128;
		}

		public new static MD2 Create()
		{
			return MD2.Create("MD2");
		}

		public new static MD2 Create(string hashName)
		{
			object obj = CryptoConfig.CreateFromName(hashName);
			if (obj == null)
			{
				obj = new MD2Managed();
			}
			return (MD2)obj;
		}
	}
}
