using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

internal abstract class \uE00B
{
	protected HashAlgorithm \uE000;

	internal virtual byte[] \uE002(byte[] \uE0F2)
	{
		return this.\uE000.ComputeHash(\uE0F2);
	}

	internal virtual byte[] \uE002(string \uE0F3)
	{
		return this.\uE000.ComputeHash(Encoding.UTF8.GetBytes(\uE0F3));
	}

	internal virtual byte[] \uE002(Stream \uE0F4)
	{
		return this.\uE000.ComputeHash(\uE0F4);
	}

	internal virtual byte[] \uE004(string \uE0F5)
	{
		FileStream fileStream = new FileStream(\uE0F5, FileMode.Open);
		byte[] result = this.\uE002(fileStream);
		fileStream.Close();
		return result;
	}

	internal virtual void \uE007(byte[] \uE0F6)
	{
		this.\uE000.TransformBlock(\uE0F6, 0, \uE0F6.Length, \uE0F6, 0);
	}

	internal virtual byte[] \uE009(byte[] \uE0F7)
	{
		return this.\uE000.TransformFinalBlock(\uE0F7, 0, \uE0F7.Length);
	}
}
