using System;

namespace UnityEngine.Csv.Internal
{
	public struct CsvGeneric
	{
		public static void IndexOutOfException(int index, bool onRow = false)
		{
			if (!onRow)
			{
				Debug.LogError("Index Out Of Range Exception [Column: " + index + "]");
			}
			else
			{
				Debug.LogError("Index Out Of Range Exception [Row: " + index + "]");
			}
		}

		public static void IndexOutOfException(int columnIndex, int rowIndex)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Index Out Of Range Exception [Column: ",
				columnIndex,
				" / Row: ",
				rowIndex,
				"]"
			}));
		}

		public static void FromToIndexOutOfException(int index, bool onTo = false, bool onRow = false)
		{
			string text = "Row";
			if (!onRow)
			{
				text = "Column";
			}
			if (!onTo)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"From Index Out Of Range Exception [",
					text,
					": ",
					index,
					"]"
				}));
			}
			else
			{
				Debug.LogError(string.Concat(new object[]
				{
					"To Index Out Of Range Exception [",
					text,
					": ",
					index,
					"]"
				}));
			}
		}

		public static void RowNotFoundException(CsvRow csvRow)
		{
			Debug.LogError("Row Not Fount Exception [Key: " + csvRow.ToString() + "]");
		}

		public static void KeyNotFoundException(string key)
		{
			Debug.LogError("Key Not Fount Exception [Key: " + key + "]");
		}

		public static void KeyOverlapException(string key)
		{
			Debug.LogError("Key Overlap Exception [Key: " + key + "]");
		}

		public static void KeyNotMachingException()
		{
			Debug.LogError("Key Not Maching Exception");
		}

		public static void NullReferenceException()
		{
			Debug.LogError("Null Reference Exception");
		}

		public static void NotIntValueException(string value)
		{
			Debug.LogError("[ " + value + " ] is not Int value.");
		}

		public static void NotDoubleValueException(string value)
		{
			Debug.LogError("[ " + value + " ] is not Double value.");
		}

		public static void NotFloatValueException(string value)
		{
			Debug.LogError("[ " + value + " ] is not Float value.");
		}

		public static void NotFoundFileException(string path)
		{
			Debug.LogError("File Not Fount Exception [Path: " + path.ToString() + "]");
		}
	}
}
