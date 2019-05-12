using System;
using System.Reflection;

[DefaultMember("Item")]
internal class \uE015
{
	private int[] \uE000;

	private int \uE001;

	public int \uE000
	{
		get
		{
			return this.\uE001;
		}
	}

	public int \uE001
	{
		get
		{
			return this.\uE000[index];
		}
	}

	public \uE015()
	{
		this.\uE000 = new int[16];
	}

	public int \uE000(int \uE000)
	{
		if (this.\uE001 == this.\uE000.Length)
		{
			int[] array = new int[this.\uE001 * 2];
			Array.Copy(this.\uE000, 0, array, 0, this.\uE001);
			this.\uE000 = array;
		}
		this.\uE000[this.\uE001] = \uE000;
		int uE = this.\uE001;
		this.\uE001 = uE + 1;
		return uE;
	}

	public int[] \uE001()
	{
		int[] array = new int[this.\uE001];
		Array.Copy(this.\uE000, 0, array, 0, this.\uE001);
		return array;
	}
}
