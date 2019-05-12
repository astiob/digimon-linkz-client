using System;
using System.Security.Cryptography;
using System.Text;

internal sealed class \uE009 : \uE007
{
	private static readonly string \uE000 = \uE019.\uE000(11668);

	private static readonly string \uE001 = \uE019.\uE000(11537);

	private static readonly int \uE002 = 10000;

	private new static readonly int \uE003 = 256;

	private SymmetricAlgorithm \uE004;

	internal \uE009(string \uE008, string \uE009, int \uE00A, int \uE00B)
	{
		this.\uE001(\uE008, \uE009, \uE00A, \uE00B);
	}

	internal \uE009()
	{
		this.\uE000();
	}

	internal override byte[] \uE003(byte[] \uE0D3)
	{
		return this.\uE004.CreateEncryptor().TransformFinalBlock(\uE0D3, 0, \uE0D3.Length);
	}

	internal override byte[] \uE003(byte[] \uE0D4, int \uE0D5, int \uE0D6)
	{
		return this.\uE004.CreateEncryptor().TransformFinalBlock(\uE0D4, \uE0D5, \uE0D6);
	}

	internal override byte[] \uE005(byte[] \uE0DF)
	{
		return this.\uE004.CreateDecryptor().TransformFinalBlock(\uE0DF, 0, \uE0DF.Length);
	}

	internal override byte[] \uE005(byte[] \uE0E0, int \uE0E1, int \uE0E2)
	{
		return this.\uE004.CreateDecryptor().TransformFinalBlock(\uE0E0, \uE0E1, \uE0E2);
	}

	internal override byte[] \uE006(string \uE0E5)
	{
		return this.\uE003(Encoding.UTF8.GetBytes(\uE0E5));
	}

	internal override string \uE008(byte[] \uE0EE)
	{
		return Encoding.UTF8.GetString(this.\uE005(\uE0EE));
	}

	internal override string \uE008(byte[] \uE0EF, int \uE0F0, int \uE0F1)
	{
		return Encoding.UTF8.GetString(this.\uE005(\uE0EF, \uE0F0, \uE0F1));
	}

	private void \uE000()
	{
		this.\uE001(\uE009.\uE000, \uE009.\uE001, \uE009.\uE002, \uE009.\uE003);
	}

	private void \uE001(string \uE00C, string \uE00D, int \uE00E, int \uE00F)
	{
		this.\uE004 = new AesManaged();
		this.\uE004.Mode = CipherMode.CBC;
		this.\uE004.Padding = PaddingMode.PKCS7;
		this.\uE004.KeySize = ((\uE00F <= 128) ? 128 : 256);
		this.\uE004.BlockSize = 128;
		\uE00A uE00A = new \uE00A(\uE00C, Encoding.UTF8.GetBytes(\uE00D), \uE00E);
		this.\uE004.Key = uE00A.\uE000(this.\uE004.KeySize / 8);
		uE00A.\uE001();
		this.\uE004.IV = uE00A.\uE000(this.\uE004.BlockSize / 8);
	}
}
