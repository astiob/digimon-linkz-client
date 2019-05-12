using System;

public sealed class BattleDebug
{
	public static bool IsShowBattleLog = true;

	private static string GetColorTag(string color, string message)
	{
		if (color == null)
		{
			return message;
		}
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">",
			message,
			"</color>"
		});
	}

	public static void Log(string message)
	{
	}
}
