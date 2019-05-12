using System;
using System.Security.Cryptography;
using System.Text;

internal sealed class \uE002 : \uE00A
{
	private static readonly string \uE000 = \uE01B.\uE000(11406);

	private static readonly string \uE001 = \uE01B.\uE000(11275);

	private static readonly int \uE002 = 10000;

	private new static readonly int \uE003 = 256;

	private SymmetricAlgorithm \uE004;

	internal \uE002(string \uE000, string \uE001, int \uE002, int \uE003)
	{
		this.\uE001(\uE000, \uE001, \uE002, \uE003);
	}

	internal \uE002()
	{
		this.\uE000();
	}

	internal override byte[] \uE003(byte[] \uE000)
	{
		return this.\uE004.CreateEncryptor().TransformFinalBlock(\uE000, 0, \uE000.Length);
	}

	internal override byte[] \uE003(byte[] \uE000, int \uE001, int \uE002)
	{
		return this.\uE004.CreateEncryptor().TransformFinalBlock(\uE000, \uE001, \uE002);
	}

	internal override byte[] \uE005(byte[] \uE000)
	{
		return this.\uE004.CreateDecryptor().TransformFinalBlock(\uE000, 0, \uE000.Length);
	}

	internal override byte[] \uE005(byte[] \uE000, int \uE001, int \uE002)
	{
		return this.\uE004.CreateDecryptor().TransformFinalBlock(\uE000, \uE001, \uE002);
	}

	internal override byte[] \uE006(string \uE000)
	{
		return this.\uE003(Encoding.UTF8.GetBytes(\uE000));
	}

	internal override string \uE008(byte[] \uE000)
	{
		return Encoding.UTF8.GetString(this.\uE005(\uE000));
	}

	internal override string \uE008(byte[] \uE000, int \uE001, int \uE002)
	{
		return Encoding.UTF8.GetString(this.\uE005(\uE000, \uE001, \uE002));
	}

	private void \uE000()
	{
		this.\uE001(global::\uE002.\uE000, global::\uE002.\uE001, global::\uE002.\uE002, global::\uE002.\uE003);
	}

	private void \uE001(string \uE000, string \uE001, int \uE002, int \uE003)
	{
		this.\uE004 = new AesManaged();
		this.\uE004.Mode = CipherMode.CBC;
		this.\uE004.Padding = PaddingMode.PKCS7;
		this.\uE004.KeySize = ((\uE003 > 128) ? 256 : 128);
		this.\uE004.BlockSize = 128;
		\uE009 uE = new \uE009(\uE000, Encoding.UTF8.GetBytes(\uE001), \uE002);
		this.\uE004.Key = uE.\uE000(this.\uE004.KeySize / 8);
		this.\uE004.IV = uE.\uE000(this.\uE004.BlockSize / 8);
	}
}
