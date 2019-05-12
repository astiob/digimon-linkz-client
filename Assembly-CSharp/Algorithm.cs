using System;
using System.Collections.Generic;
using System.Linq;

public class Algorithm
{
	public static T BinarySearch<T>(T[] Array, string KeyID, int SearchRangeStart, int SearchRangeEnd, string CompareFieldName, int BinarySearchPermissibleLength = 8)
	{
		if (Array.Length == 0)
		{
			return default(T);
		}
		Type type = Array[0].GetType();
		if (SearchRangeStart > SearchRangeEnd)
		{
			for (int i = 0; i < Array.Length; i++)
			{
				if ((string)type.GetField(CompareFieldName).GetValue(Array[i]) == KeyID)
				{
					return Array[i];
				}
			}
		}
		if (SearchRangeEnd - SearchRangeStart <= BinarySearchPermissibleLength)
		{
			for (int j = SearchRangeStart; j <= SearchRangeEnd; j++)
			{
				if ((string)type.GetField(CompareFieldName).GetValue(Array[j]) == KeyID)
				{
					return Array[j];
				}
			}
			for (int k = 0; k < Array.Length; k++)
			{
				if ((string)type.GetField(CompareFieldName).GetValue(Array[k]) == KeyID)
				{
					return Array[k];
				}
			}
			return default(T);
		}
		int num = SearchRangeStart + (SearchRangeEnd - SearchRangeStart) / 2;
		if (num % 2 != 0)
		{
			num++;
		}
		int num2 = int.Parse(KeyID);
		int num3 = int.Parse((string)type.GetField(CompareFieldName).GetValue(Array[num]));
		if (num3 > num2)
		{
			return Algorithm.BinarySearch<T>(Array, KeyID, SearchRangeStart, num - 1, CompareFieldName, BinarySearchPermissibleLength);
		}
		if (num3 < num2)
		{
			return Algorithm.BinarySearch<T>(Array, KeyID, num + 1, SearchRangeEnd, CompareFieldName, BinarySearchPermissibleLength);
		}
		return Array[num];
	}

	public static T BinarySearch<T>(List<T> ListArray, string KeyID, int SearchRangeStart, int SearchRangeEnd, string CompareFieldName, int BinarySearchPermissibleLength = 8)
	{
		if (ListArray.Count == 0)
		{
			return default(T);
		}
		T t = ListArray[0];
		Type type = t.GetType();
		if (SearchRangeStart > SearchRangeEnd)
		{
			for (int i = 0; i < ListArray.Count; i++)
			{
				if ((string)type.GetField(CompareFieldName).GetValue(ListArray[i]) == KeyID)
				{
					return ListArray[i];
				}
			}
		}
		if (SearchRangeEnd - SearchRangeStart <= BinarySearchPermissibleLength)
		{
			for (int j = SearchRangeStart; j <= SearchRangeEnd; j++)
			{
				if ((string)type.GetField(CompareFieldName).GetValue(ListArray[j]) == KeyID)
				{
					return ListArray[j];
				}
			}
			for (int k = 0; k < ListArray.Count; k++)
			{
				if ((string)type.GetField(CompareFieldName).GetValue(ListArray[k]) == KeyID)
				{
					return ListArray[k];
				}
			}
			return default(T);
		}
		int num = SearchRangeStart + (SearchRangeEnd - SearchRangeStart) / 2;
		if (num % 2 != 0)
		{
			num++;
		}
		int num2 = int.Parse(KeyID);
		int num3 = int.Parse((string)type.GetField(CompareFieldName).GetValue(ListArray[num]));
		if (num3 > num2)
		{
			return Algorithm.BinarySearch<T>(ListArray, KeyID, SearchRangeStart, num - 1, CompareFieldName, BinarySearchPermissibleLength);
		}
		if (num3 < num2)
		{
			return Algorithm.BinarySearch<T>(ListArray, KeyID, num + 1, SearchRangeEnd, CompareFieldName, BinarySearchPermissibleLength);
		}
		return ListArray[num];
	}

	public static T BinarySearch<T>(T[] Array, string KeyID_1, string KeyID_2, int SearchRangeStart, int SearchRangeEnd, string CompareFieldName_1, string CompareFieldName_2, int BinarySearchPermissibleLength = 8)
	{
		if (Array.Length == 0)
		{
			return default(T);
		}
		Type type = Array[0].GetType();
		if (SearchRangeStart > SearchRangeEnd)
		{
			for (int i = 0; i < Array.Length; i++)
			{
				if ((string)type.GetField(CompareFieldName_1).GetValue(Array[i]) == KeyID_1 && (string)type.GetField(CompareFieldName_2).GetValue(Array[i]) == KeyID_2)
				{
					return Array[i];
				}
			}
		}
		if (SearchRangeEnd - SearchRangeStart <= BinarySearchPermissibleLength)
		{
			for (int j = SearchRangeStart; j <= SearchRangeEnd; j++)
			{
				if ((string)type.GetField(CompareFieldName_1).GetValue(Array[j]) == KeyID_1 && (string)type.GetField(CompareFieldName_2).GetValue(Array[j]) == KeyID_2)
				{
					return Array[j];
				}
			}
		}
		else
		{
			int num = SearchRangeStart + (SearchRangeEnd - SearchRangeStart) / 2;
			if (num % 2 != 0)
			{
				num++;
			}
			int num2 = int.Parse(KeyID_1);
			int num3 = int.Parse((string)type.GetField(CompareFieldName_1).GetValue(Array[num]));
			if (num3 > num2)
			{
				return Algorithm.BinarySearch<T>(Array, KeyID_1, KeyID_2, SearchRangeStart, num - 1, CompareFieldName_1, CompareFieldName_2, BinarySearchPermissibleLength);
			}
			if (num3 < num2)
			{
				return Algorithm.BinarySearch<T>(Array, KeyID_1, KeyID_2, num + 1, SearchRangeEnd, CompareFieldName_1, CompareFieldName_2, BinarySearchPermissibleLength);
			}
			if ((string)type.GetField(CompareFieldName_2).GetValue(Array[num]) == KeyID_2)
			{
				return Array[num];
			}
		}
		for (int k = 0; k < Array.Length; k++)
		{
			if ((string)type.GetField(CompareFieldName_1).GetValue(Array[k]) == KeyID_1 && (string)type.GetField(CompareFieldName_2).GetValue(Array[k]) == KeyID_2)
			{
				return Array[k];
			}
		}
		return default(T);
	}

	public static T BinarySearch<T>(List<T> ListArray, string KeyID, int SearchRangeStart, int SearchRangeEnd, string CompareFieldName, string CompareParentFieldName, int BinarySearchPermissibleLength = 8)
	{
		if (ListArray.Count == 0)
		{
			return default(T);
		}
		T t = ListArray[0];
		Type type = t.GetType();
		Type type2;
		if (SearchRangeStart > SearchRangeEnd)
		{
			for (int i = 0; i < ListArray.Count; i++)
			{
				type2 = type.GetField(CompareParentFieldName).GetValue(ListArray[i]).GetType();
				if ((string)type2.GetField(CompareFieldName).GetValue(type.GetField(CompareParentFieldName).GetValue(ListArray[i])) == KeyID)
				{
					return ListArray[i];
				}
			}
		}
		if (SearchRangeEnd - SearchRangeStart <= BinarySearchPermissibleLength)
		{
			for (int j = SearchRangeStart; j <= SearchRangeEnd; j++)
			{
				type2 = type.GetField(CompareParentFieldName).GetValue(ListArray[j]).GetType();
				if ((string)type2.GetField(CompareFieldName).GetValue(type.GetField(CompareParentFieldName).GetValue(ListArray[j])) == KeyID)
				{
					return ListArray[j];
				}
			}
			for (int k = 0; k < ListArray.Count; k++)
			{
				type2 = type.GetField(CompareParentFieldName).GetValue(ListArray[k]).GetType();
				if ((string)type2.GetField(CompareFieldName).GetValue(type.GetField(CompareParentFieldName).GetValue(ListArray[k])) == KeyID)
				{
					return ListArray[k];
				}
			}
			return default(T);
		}
		int num = SearchRangeStart + (SearchRangeEnd - SearchRangeStart) / 2;
		if (num % 2 != 0)
		{
			num++;
		}
		int num2 = int.Parse(KeyID);
		type2 = type.GetField(CompareParentFieldName).GetValue(ListArray[num]).GetType();
		int num3 = int.Parse((string)type2.GetField(CompareFieldName).GetValue(type.GetField(CompareParentFieldName).GetValue(ListArray[num])));
		if (num3 > num2)
		{
			return Algorithm.BinarySearch<T>(ListArray, KeyID, SearchRangeStart, num - 1, CompareFieldName, CompareParentFieldName, BinarySearchPermissibleLength);
		}
		if (num3 < num2)
		{
			return Algorithm.BinarySearch<T>(ListArray, KeyID, num + 1, SearchRangeEnd, CompareFieldName, CompareParentFieldName, BinarySearchPermissibleLength);
		}
		return ListArray[num];
	}

	public static List<T> ShuffuleList<T>(List<T> ListArray)
	{
		T[] source = ListArray.ToArray();
		return source.OrderBy((T i) => Guid.NewGuid()).ToList<T>();
	}
}
