using System;

namespace TS
{
	public sealed class ScriptUtil
	{
		public static string[] SplitByWhiteSpace(string str)
		{
			return str.Split(new char[]
			{
				' ',
				'\t'
			}, StringSplitOptions.RemoveEmptyEntries);
		}

		public static int GetInt(string str)
		{
			int result = 0;
			if (!int.TryParse(str, out result))
			{
				Debug.LogError("数値じゃない");
			}
			return result;
		}

		public static float GetFloat(string str)
		{
			float result = 0f;
			if (!float.TryParse(str, out result))
			{
				Debug.LogError("数値じゃない");
			}
			return result;
		}

		public static int GetIndex(string[] keys, string str)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (str == keys[i])
				{
					return i;
				}
			}
			Debug.LogError("キーが無い");
			return 0;
		}

		public static bool GetIndex(string[] keys, string str, out int value)
		{
			bool result = false;
			value = 0;
			for (int i = 0; i < keys.Length; i++)
			{
				if (str == keys[i])
				{
					value = i;
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
