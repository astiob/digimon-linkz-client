using System;
using System.Collections.Generic;
using UnityEngine;

namespace MultiBattle.Tools
{
	public sealed class MultiTools
	{
		public static MonsterData MakeAndSetMonster(GameWebAPI.Common_MonsterData monsterData)
		{
			return monsterData.ToMonsterData();
		}

		public static void DispLoading(bool isLoading, RestrictionInput.LoadType loadType = RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON)
		{
			if (isLoading)
			{
				if (!Loading.IsShow())
				{
					RestrictionInput.StartLoad(loadType);
				}
			}
			else
			{
				RestrictionInput.EndLoad();
			}
		}

		public static T GetValueByKey<T>(object messageObj, string key)
		{
			Dictionary<object, object> dictionary = (Dictionary<object, object>)messageObj;
			foreach (KeyValuePair<object, object> keyValuePair in dictionary)
			{
				if (keyValuePair.Key.ToString() == key)
				{
					global::Debug.LogFormat("{0}キー発見.", new object[]
					{
						key
					});
					return MultiTools.ConvertValue<T>(keyValuePair.Value);
				}
			}
			return default(T);
		}

		public static void Dump(object messageObj)
		{
			MultiTools.DumpRecursively(messageObj, 0, 0);
		}

		public static void DumpWarning(object messageObj)
		{
			MultiTools.DumpRecursively(messageObj, 1, 0);
		}

		public static void DumpError(object messageObj)
		{
			MultiTools.DumpRecursively(messageObj, 2, 0);
		}

		private static void DumpRecursively(object messageObj, int logLevel, int treeLevel)
		{
			Dictionary<object, object> dictionary = null;
			try
			{
				dictionary = (Dictionary<object, object>)messageObj;
			}
			catch (Exception ex)
			{
				global::Debug.LogFormat("Dumpメソッドのキャストが失敗. {0}", new object[]
				{
					ex
				});
				return;
			}
			foreach (KeyValuePair<object, object> keyValuePair in dictionary)
			{
				switch (logLevel)
				{
				case 0:
					global::Debug.LogFormat("{0}{1}:{2}", new object[]
					{
						new string('-', treeLevel),
						keyValuePair.Key,
						keyValuePair.Value
					});
					break;
				case 1:
					global::Debug.LogWarningFormat("{0}{1}:{2}", new object[]
					{
						new string('-', treeLevel),
						keyValuePair.Key,
						keyValuePair.Value
					});
					break;
				case 2:
					global::Debug.LogErrorFormat("{0}{1}:{2}", new object[]
					{
						new string('-', treeLevel),
						keyValuePair.Key,
						keyValuePair.Value
					});
					break;
				default:
					global::Debug.LogFormat("{0}{1}:{2}", new object[]
					{
						new string('-', treeLevel),
						keyValuePair.Key,
						keyValuePair.Value
					});
					break;
				}
				MultiTools.DumpRecursively(keyValuePair.Value, logLevel, treeLevel + 1);
			}
		}

		public static T ConvertValue<T>(object value)
		{
			return (T)((object)Convert.ChangeType(value, typeof(T)));
		}

		public static T StringToEnum<T>(string enumString)
		{
			return (T)((object)Enum.Parse(typeof(T), enumString));
		}

		public static string GetPvPRankSpriteName(int colosseumRankId)
		{
			return "Rank_" + colosseumRankId.ToString();
		}

		public static Rect MakeRecruitListRectWindow()
		{
			return new Rect
			{
				xMin = -280f,
				xMax = 280f,
				yMin = -342f - GUIMain.VerticalSpaceSize,
				yMax = 250f + GUIMain.VerticalSpaceSize
			};
		}

		public static string AddAreaNameZero(string word)
		{
			if (word.Length < 2)
			{
				return "0" + word + " ";
			}
			return word + " ";
		}
	}
}
