using System;

public static class StringFormat
{
	public static string Cluster(string s)
	{
		return StringFormat.Cluster(int.Parse(s));
	}

	public static string Cluster(int value)
	{
		string result;
		if (ConstValue.MAX_CLUSTER_COUNT < value)
		{
			result = ConstValue.MAX_CLUSTER_COUNT.ToString("#,0");
		}
		else
		{
			result = value.ToString("#,0");
		}
		return result;
	}
}
