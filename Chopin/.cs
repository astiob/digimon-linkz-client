using System;
using System.Reflection;

[DefaultMember("Item")]
internal class \uE01C
{
	private int[] \uE004;

	private int \uE005;

	public int \uE000
	{
		get
		{
			return this.\uE005;
		}
	}

	public int \uE001
	{
		get
		{
			return this.\uE004[index];
		}
	}

	public \uE01C()
	{
		this.\uE004 = new int[16];
	}

	public int \uE000(int \uE0A4)
	{
		if (this.\uE005 == this.\uE004.Length)
		{
			int[] array = new int[this.\uE005 * 2];
			Array.Copy(this.\uE004, 0, array, 0, this.\uE005);
			this.\uE004 = array;
		}
		this.\uE004[this.\uE005] = \uE0A4;
		int uE = this.\uE005;
		this.\uE005 = uE + 1;
		return uE;
	}

	public int[] \uE001()
	{
		int[] array = new int[this.\uE005];
		Array.Copy(this.\uE004, 0, array, 0, this.\uE005);
		return array;
	}
}
