using System;
using System.Security.Cryptography;

internal class \uE00A
{
	private DeriveBytes \uE000;

	internal \uE00A(string \uE010, byte[] \uE011, int \uE012)
	{
		this.\uE000 = new Rfc2898DeriveBytes(\uE010, \uE011, \uE012);
	}

	internal byte[] \uE000(int \uE013)
	{
		return this.\uE000.GetBytes(\uE013);
	}

	internal void \uE001()
	{
		this.\uE000.Reset();
	}
}
