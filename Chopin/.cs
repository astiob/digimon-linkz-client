using System;
using System.Security.Cryptography;

internal class \uE009
{
	private DeriveBytes \uE000;

	internal \uE009(string \uE000, byte[] \uE001, int \uE002)
	{
		this.\uE000 = new Rfc2898DeriveBytes(\uE000, \uE001, \uE002);
	}

	internal byte[] \uE000(int \uE000)
	{
		return this.\uE000.GetBytes(\uE000);
	}

	internal void Reset()
	{
		this.\uE000.Reset();
	}
}
