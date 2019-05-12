using Master;
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

		public static void ShowCommonDialogForMessage(Action<int> callback, string title, string info, string se = "SEInternal/Common/se_106")
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(callback, "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = title;
			cmd_Confirm.Info = info;
			SoundMng.Instance().PlaySE(se, 0f, false, true, null, -1, 1f);
		}

		public static void ShowCommonDialog(Action<int> callback, string titleKey, string infoKey, string se = "SEInternal/Common/se_106")
		{
			ScriptUtil.ShowCommonDialogForMessage(callback, StringMaster.GetString(titleKey), StringMaster.GetString(infoKey), se);
		}

		public static float CheckSize(int size, ref ScriptUtil.SIZE_TYPE type)
		{
			type = ScriptUtil.SIZE_TYPE.KILOBYTE;
			float num = (float)size;
			if (num > 1024f)
			{
				num /= 1024f;
			}
			if (num > 1024f)
			{
				num /= 1024f;
				type = ScriptUtil.SIZE_TYPE.MEGABYTE;
			}
			if (num > 1024f)
			{
				num /= 1024f;
				type = ScriptUtil.SIZE_TYPE.GIGABYTE;
			}
			return num;
		}

		public enum SIZE_TYPE
		{
			KILOBYTE,
			MEGABYTE,
			GIGABYTE
		}
	}
}
