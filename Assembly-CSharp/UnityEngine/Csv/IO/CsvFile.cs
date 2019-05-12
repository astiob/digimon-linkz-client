using System;
using System.IO;
using System.Text;
using UnityEngine.Csv.Internal;

namespace UnityEngine.Csv.IO
{
	public class CsvFile
	{
		public static void Export(string path, string fileName, Csv csv, bool onOverWrite = true)
		{
			string path2 = path + "/" + fileName + ".csv";
			StreamWriter streamWriter = null;
			try
			{
				if (File.Exists(path2) && !onOverWrite)
				{
					File.Delete(path2);
				}
				else
				{
					streamWriter = new StreamWriter(path2, false, Encoding.GetEncoding("shift_jis"));
					streamWriter.AutoFlush = true;
					streamWriter.Write(csv.ToString());
				}
			}
			finally
			{
				if (streamWriter != null)
				{
					streamWriter.Close();
				}
			}
		}

		public static Csv Import(string path)
		{
			StreamReader streamReader = null;
			Csv result = null;
			try
			{
				if (!File.Exists(path))
				{
					CsvGeneric.NotFoundFileException(path);
					return null;
				}
				streamReader = new StreamReader(path);
				string text = streamReader.ReadToEnd();
				text = text.Trim(new char[]
				{
					'\n',
					' '
				});
				result = new Csv(text);
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			return result;
		}

		public static Csv Import(string directory, string fileName)
		{
			string path = directory + "/" + fileName + ".csv";
			return CsvFile.Import(path);
		}
	}
}
