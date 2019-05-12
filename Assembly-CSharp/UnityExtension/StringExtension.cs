using System;

namespace UnityExtension
{
	public static class StringExtension
	{
		public static string CreatePath(params string[] directory)
		{
			string text = string.Empty;
			for (int i = 0; i < directory.Length; i++)
			{
				text += directory[i];
				if (i < directory.Length - 1)
				{
					text += "/";
				}
			}
			return text;
		}
	}
}
