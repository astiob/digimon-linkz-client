using System;
using System.Collections.Generic;

namespace TS
{
	public sealed class ScriptInitializer
	{
		public List<ScriptCommandData> ConvertAllStrDtsToCommands(List<string> allLineList)
		{
			List<ScriptCommandData> list = new List<ScriptCommandData>();
			bool flag = false;
			int i = 0;
			while (i < allLineList.Count)
			{
				string text = allLineList[i];
				if (text.Length <= 0)
				{
					if (flag)
					{
						text = "\u3000";
						goto IL_51;
					}
				}
				else if (text[0] != ';')
				{
					goto IL_51;
				}
				IL_10E:
				i++;
				continue;
				IL_51:
				int num = text.IndexOf(";");
				if (num != -1)
				{
					text = text.Substring(0, num).Trim();
				}
				if (text[0] != '#')
				{
					if (!flag)
					{
						Debug.LogErrorFormat("TextError : ファイル {0}行目 コマンドではありません！！", new object[]
						{
							(i + 1).ToString()
						});
					}
					else
					{
						text = "#text " + text;
					}
				}
				else if (text.StartsWith("#msgend"))
				{
					flag = false;
				}
				else if (text.StartsWith("#msg"))
				{
					flag = true;
				}
				list.Add(new ScriptCommandData
				{
					strArrange = text,
					lineNum = i + 1
				});
				goto IL_10E;
			}
			return list;
		}
	}
}
