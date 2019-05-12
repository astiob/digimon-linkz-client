using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

internal abstract class \uE013
{
	protected HashAlgorithm \uE000;

	internal virtual byte[] \uE002(byte[] \uE000)
	{
		return this.\uE000.ComputeHash(\uE000);
	}

	internal virtual byte[] \uE002(string \uE000)
	{
		return this.\uE000.ComputeHash(Encoding.UTF8.GetBytes(\uE000));
	}

	internal virtual byte[] \uE002(Stream \uE000)
	{
		return this.\uE000.ComputeHash(\uE000);
	}

	internal virtual byte[] \uE004(string \uE000)
	{
		FileStream fileStream = new FileStream(\uE000, FileMode.Open);
		byte[] result = this.\uE002(fileStream);
		fileStream.Close();
		return result;
	}

	internal virtual void \uE007(byte[] \uE000)
	{
		this.\uE000.TransformBlock(\uE000, 0, \uE000.Length, \uE000, 0);
	}

	internal virtual byte[] \uE009(byte[] \uE000)
	{
		return this.\uE000.TransformFinalBlock(\uE000, 0, \uE000.Length);
	}
}
