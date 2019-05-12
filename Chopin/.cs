using System;
using System.Security.Cryptography;
using System.Text;

internal sealed class \uE008 : \uE007
{
	private static readonly string \uE000 = \uE019.\uE000(11668);

	private static readonly string \uE001 = \uE019.\uE000(11537);

	private static readonly int \uE002 = 10000;

	private new static readonly int \uE003 = 256;

	private SymmetricAlgorithm \uE004;

	internal \uE008(string \uE000, string \uE001, int \uE002, int \uE003)
	{
		this.\uE001(\uE000, \uE001, \uE002, \uE003);
	}

	internal \uE008()
	{
		this.\uE000();
	}

	internal override byte[] \uE003(byte[] \uE0CF)
	{
		return this.\uE004.CreateEncryptor().TransformFinalBlock(\uE0CF, 0, \uE0CF.Length);
	}

	internal override byte[] \uE003(byte[] \uE0D0, int \uE0D1, int \uE0D2)
	{
		return this.\uE004.CreateEncryptor().TransformFinalBlock(\uE0D0, \uE0D1, \uE0D2);
	}

	internal override byte[] \uE005(byte[] \uE0DB)
	{
		return this.\uE004.CreateDecryptor().TransformFinalBlock(\uE0DB, 0, \uE0DB.Length);
	}

	internal override byte[] \uE005(byte[] \uE0DC, int \uE0DD, int \uE0DE)
	{
		return this.\uE004.CreateDecryptor().TransformFinalBlock(\uE0DC, \uE0DD, \uE0DE);
	}

	internal override byte[] \uE006(string \uE0E4)
	{
		return this.\uE003(Encoding.UTF8.GetBytes(\uE0E4));
	}

	internal override string \uE008(byte[] \uE0EA)
	{
		return Encoding.UTF8.GetString(this.\uE005(\uE0EA));
	}

	internal override string \uE008(byte[] \uE0EB, int \uE0EC, int \uE0ED)
	{
		return Encoding.UTF8.GetString(this.\uE005(\uE0EB, \uE0EC, \uE0ED));
	}

	private void \uE000()
	{
		this.\uE001(global::\uE008.\uE000, global::\uE008.\uE001, global::\uE008.\uE002, global::\uE008.\uE003);
	}

	private void \uE001(string \uE004, string \uE005, int \uE006, int \uE007)
	{
		this.\uE004 = new AesManaged();
		this.\uE004.Mode = CipherMode.CBC;
		this.\uE004.Padding = PaddingMode.PKCS7;
		this.\uE004.KeySize = ((\uE007 <= 128) ? 128 : 256);
		this.\uE004.BlockSize = 128;
		\uE00A uE00A = new \uE00A(\uE004, Encoding.UTF8.GetBytes(\uE005), \uE006);
		this.\uE004.Key = uE00A.\uE000(this.\uE004.KeySize / 8);
		this.\uE004.IV = uE00A.\uE000(this.\uE004.BlockSize / 8);
	}
}
