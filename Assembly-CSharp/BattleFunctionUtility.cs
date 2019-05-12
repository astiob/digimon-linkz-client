using System;

public static class BattleFunctionUtility
{
	public const string EmptyPath = "";

	public static bool IsEmptyPath(string path)
	{
		return string.IsNullOrEmpty(path);
	}
}
