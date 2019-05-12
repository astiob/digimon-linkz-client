using System;
using System.Collections.Generic;
using UnityEngine.Csv.Internal;
using UnityEngine.Serialization;

namespace UnityEngine.Csv
{
	[Serializable]
	public class Csv
	{
		[SerializeField]
		private List<string> _key = new List<string>();

		[SerializeField]
		private List<CsvColumnList> _value = new List<CsvColumnList>();

		[SerializeField]
		[FormerlySerializedAs("_rowLengthCash")]
		private int _rowLengthCache;

		public Csv(int rowLength, params string[] column)
		{
			this.Initialize(rowLength, column);
		}

		public Csv(Csv csv)
		{
			this.Initialize(csv.RowLength, csv.GetKeyList());
			for (int i = 0; i < this.ColumnLength; i++)
			{
				for (int j = 0; j < this.RowLength; j++)
				{
					this[i, j] = new CsvValue(csv[i, j]);
				}
			}
		}

		public Csv(string csvValue)
		{
			this.Parse(csvValue);
		}

		public void Initialize(int rowLength, params string[] column)
		{
			this._key = new List<string>();
			this._value = new List<CsvColumnList>();
			this._rowLengthCache = rowLength;
			for (int i = 0; i < column.Length; i++)
			{
				this._key.Add(column[i]);
				this._value.Add(new CsvColumnList());
				for (int j = 0; j < this._rowLengthCache; j++)
				{
					this._value[this._value.Count - 1].value.Add(new CsvValue());
				}
			}
		}

		public void Initialize(params CsvColumn[] column)
		{
			this._key = new List<string>();
			int num = 0;
			for (int i = 0; i < column.Length; i++)
			{
				this._key.Add(column[i].key);
				num = Mathf.Max(num, column[i].Length);
			}
			this.Initialize(num, this._key.ToArray());
			for (int j = 0; j < this.ColumnLength; j++)
			{
				for (int k = 0; k < this.RowLength; k++)
				{
					if (column.Length > k)
					{
						this[j, k] = column[j][k];
					}
				}
			}
		}

		public void Initialize(params CsvRow[] row)
		{
			this._key = new List<string>();
			for (int i = 0; i < row.Length; i++)
			{
				string[] keyList = row[i].GetKeyList();
				for (int j = 0; j < keyList.Length; j++)
				{
					if (!this._key.Contains(keyList[j]))
					{
						this._key.Add(keyList[j]);
					}
				}
			}
			this.Initialize(row.Length, this._key.ToArray());
			for (int k = 0; k < row.Length; k++)
			{
				string[] keyList2 = row[k].GetKeyList();
				for (int l = 0; l < keyList2.Length; l++)
				{
					this[keyList2[l], k] = row[k][l];
				}
			}
		}

		internal int KeyToIndex(string key)
		{
			int i;
			for (i = 0; i < this._key.Count; i++)
			{
				if (this._key[i].Trim().Equals(key.Trim()))
				{
					break;
				}
			}
			if (i == this._key.Count)
			{
				return -1;
			}
			return i;
		}

		internal List<CsvValue> GetColumnList(int index)
		{
			List<CsvValue> list = new List<CsvValue>();
			for (int i = 0; i < this.RowLength; i++)
			{
				list.Add(this._value[index].value[i]);
			}
			return list;
		}

		internal List<CsvValue> GetRowList(int index)
		{
			List<CsvValue> list = new List<CsvValue>();
			for (int i = 0; i < this.ColumnLength; i++)
			{
				list.Add(this._value[i].value[index]);
			}
			return list;
		}

		internal List<CsvValue> CsvColumnToList(CsvColumn column)
		{
			List<CsvValue> list = new List<CsvValue>();
			for (int i = 0; i < column.Length; i++)
			{
				list.Add(column[i]);
			}
			return list;
		}

		internal List<CsvValue> CsvRowToList(CsvRow row)
		{
			List<CsvValue> list = new List<CsvValue>();
			for (int i = 0; i < row.Length; i++)
			{
				list.Add(row[i]);
			}
			return list;
		}

		public CsvValue this[string columnKey, int rowIndex]
		{
			get
			{
				int num = this.KeyToIndex(columnKey);
				if (num == -1)
				{
					CsvGeneric.KeyNotFoundException(columnKey);
					return new CsvValue();
				}
				return this[num, rowIndex];
			}
			set
			{
				int num = this.KeyToIndex(columnKey);
				if (num == -1)
				{
					CsvGeneric.KeyNotFoundException(columnKey);
					return;
				}
				this[num, rowIndex] = value;
			}
		}

		public CsvValue this[int columnIndex, int rowIndex]
		{
			get
			{
				if (columnIndex < 0 || columnIndex >= this.ColumnLength)
				{
					CsvGeneric.IndexOutOfException(columnIndex, rowIndex);
					return new CsvValue();
				}
				if (rowIndex < 0 || rowIndex >= this.RowLength)
				{
					CsvGeneric.IndexOutOfException(columnIndex, rowIndex);
					return new CsvValue();
				}
				return this._value[columnIndex].value[rowIndex];
			}
			set
			{
				if (columnIndex < 0 || columnIndex >= this.ColumnLength)
				{
					CsvGeneric.IndexOutOfException(columnIndex, rowIndex);
					return;
				}
				if (rowIndex < 0 || rowIndex >= this.RowLength)
				{
					CsvGeneric.IndexOutOfException(columnIndex, rowIndex);
					return;
				}
				this._value[columnIndex].value[rowIndex] = value;
			}
		}

		public CsvColumn GetColumn(string column)
		{
			int num = this.IndexOfColumn(column);
			if (num == -1)
			{
				CsvGeneric.KeyNotFoundException(column);
				return null;
			}
			return this.GetColumn(num);
		}

		public CsvColumn GetColumn(int columnIndex)
		{
			if (columnIndex < 0 || columnIndex >= this.ColumnLength)
			{
				CsvGeneric.IndexOutOfException(columnIndex, false);
				return null;
			}
			return new CsvColumn(this._key[columnIndex], this.GetColumnList(columnIndex).ToArray());
		}

		public void AddColumn(string column)
		{
			if (this.KeyToIndex(column) != -1)
			{
				CsvGeneric.KeyOverlapException(column);
				return;
			}
			this._key.Add(column);
			this._value.Add(new CsvColumnList());
			for (int i = 0; i < this.RowLength; i++)
			{
				this._value[this.ColumnLength - 1].value.Add(new CsvValue());
			}
		}

		public void AddColumn(CsvColumn column)
		{
			this.InsertColumn(this.ColumnLength, column);
		}

		public void RemoveColumn(string column)
		{
			int num = this.KeyToIndex(column);
			if (num == -1)
			{
				CsvGeneric.KeyNotFoundException(column);
				return;
			}
			this._value.RemoveAt(num);
		}

		public void RemoveAtColumn(int columnIndex)
		{
			if (columnIndex >= this.ColumnLength || columnIndex < 0)
			{
				CsvGeneric.IndexOutOfException(columnIndex, false);
				return;
			}
			this._value.RemoveAt(columnIndex);
		}

		public void MoveColumn(int columnIndexFrom, int columnIndexTo)
		{
			if (this.ColumnLength <= columnIndexFrom || columnIndexFrom < 0)
			{
				CsvGeneric.FromToIndexOutOfException(columnIndexFrom, false, false);
				return;
			}
			if (this.ColumnLength <= columnIndexTo || columnIndexTo < 0)
			{
				CsvGeneric.FromToIndexOutOfException(columnIndexTo, true, false);
				return;
			}
			if (columnIndexFrom == columnIndexTo)
			{
				return;
			}
			string item = this._key[columnIndexFrom];
			List<CsvValue> value = this._value[columnIndexFrom].value;
			this._key.RemoveAt(columnIndexFrom);
			this._value.RemoveAt(columnIndexFrom);
			this._key.Insert(columnIndexTo, item);
			CsvColumnList csvColumnList = new CsvColumnList();
			csvColumnList.value = value;
			this._value.Insert(columnIndexTo, csvColumnList);
		}

		public void InsertColumn(int columnIndex, CsvColumn csvColumn)
		{
			if (columnIndex < 0 || columnIndex > this.ColumnLength)
			{
				CsvGeneric.IndexOutOfException(columnIndex, false);
				return;
			}
			List<CsvValue> list = this.CsvColumnToList(csvColumn);
			if (this.RowLength != list.Count)
			{
				while (this.RowLength != list.Count)
				{
					if (this.RowLength > list.Count)
					{
						list.Add(new CsvValue());
					}
					else
					{
						this.AddRow();
					}
				}
			}
			CsvColumnList csvColumnList = new CsvColumnList();
			csvColumnList.value = this.CsvColumnToList(csvColumn);
			if (!this.ContainsColumn(csvColumn, false))
			{
				this._key.Insert(columnIndex, csvColumn.key);
				this._value.Insert(columnIndex, csvColumnList);
			}
			else
			{
				this._key.Add(csvColumn.key);
				this._value.Add(csvColumnList);
			}
		}

		public bool ContainsColumn(string column)
		{
			return this.IndexOfColumn(column) != -1;
		}

		public bool ContainsColumn(CsvColumn csvColumn, bool isIgnoreRow = false)
		{
			if (isIgnoreRow)
			{
				return this.ContainsColumn(csvColumn.key);
			}
			return this.IndexOfColumn(csvColumn, isIgnoreRow) != -1;
		}

		public int IndexOfColumn(string column)
		{
			return this.KeyToIndex(column);
		}

		public int IndexOfColumn(CsvColumn csvColumn, bool isIgnoreRow = false)
		{
			if (isIgnoreRow)
			{
				return this.IndexOfColumn(csvColumn, false);
			}
			int num = this.KeyToIndex(csvColumn.key);
			if (num == -1)
			{
				return -1;
			}
			if (csvColumn.Length != this.RowLength)
			{
				return -1;
			}
			for (int i = 0; i < this.RowLength; i++)
			{
				if (!csvColumn[i].Equals(this._value[num].value[i]))
				{
					return -1;
				}
			}
			return num;
		}

		public string[] GetKeyList()
		{
			return this._key.ToArray();
		}

		public string[] GetMargedKeyList(params string[] keyList)
		{
			List<string> list = new List<string>(this.GetKeyList());
			for (int i = 0; i < keyList.Length; i++)
			{
				if (!list.Contains(keyList[i]))
				{
					list.Add(keyList[i]);
				}
			}
			return list.ToArray();
		}

		public int ColumnLength
		{
			get
			{
				return this._key.Count;
			}
		}

		public bool CheckMachKey(params string[] key)
		{
			if (key.Length != this.ColumnLength)
			{
				return false;
			}
			for (int i = 0; i < this.ColumnLength; i++)
			{
				if (!this._key[i].Trim().Equals(key[i].Trim()))
				{
					return false;
				}
			}
			return true;
		}

		public bool CheckMachKey(CsvRow csvRow)
		{
			return this.CheckMachKey(csvRow.GetKeyList());
		}

		public bool CheckMachKey(Csv csv)
		{
			return this.CheckMachKey(csv.GetKeyList());
		}

		public CsvRow GetRow(int rowIndex)
		{
			if (rowIndex < 0 || rowIndex >= this.RowLength)
			{
				CsvGeneric.IndexOutOfException(rowIndex, true);
				return null;
			}
			return new CsvRow(this._key.ToArray(), this.GetRowList(rowIndex).ToArray());
		}

		public void AddRow()
		{
			for (int i = 0; i < this.ColumnLength; i++)
			{
				this._value[i].value.Add(new CsvValue());
			}
			this._rowLengthCache++;
		}

		public void AddRow(CsvRow csvRow)
		{
			if (!this.CheckMachKey(csvRow))
			{
				CsvGeneric.KeyNotMachingException();
				return;
			}
			this.AddRow();
			this.SetRow(this.RowLength - 1, csvRow);
		}

		public void RemoveRow(CsvRow csvRow)
		{
			if (!this.ContainsRow(csvRow))
			{
				CsvGeneric.RowNotFoundException(csvRow);
				return;
			}
			int rowIndex = this.RowIndexOf(csvRow);
			this.RemoveAtRow(rowIndex);
		}

		public void RemoveAtRow(int rowIndex)
		{
			for (int i = 0; i < this.ColumnLength; i++)
			{
				if (this.RowLength <= rowIndex || rowIndex < 0)
				{
					CsvGeneric.IndexOutOfException(rowIndex, true);
					return;
				}
				this._value[i].value.RemoveAt(rowIndex);
			}
			this._rowLengthCache--;
		}

		public void MoveRaw(int rowIndexFrom, int rowIndexTo)
		{
			if (this.RowLength <= rowIndexFrom || rowIndexFrom < 0)
			{
				CsvGeneric.FromToIndexOutOfException(rowIndexFrom, false, true);
				return;
			}
			if (this.RowLength <= rowIndexTo || rowIndexTo < 0)
			{
				CsvGeneric.FromToIndexOutOfException(rowIndexTo, true, true);
				return;
			}
			if (rowIndexFrom == rowIndexTo)
			{
				return;
			}
			List<CsvValue> list = new List<CsvValue>();
			for (int i = 0; i < this.ColumnLength; i++)
			{
				list.Add(this[i, rowIndexFrom]);
				this._value[i].value.RemoveAt(rowIndexFrom);
			}
			for (int j = 0; j < this.ColumnLength; j++)
			{
				this._value[j].value.Insert(rowIndexTo, list[j]);
			}
		}

		public void InsertRaw(int rowIndex, CsvRow csvRow)
		{
			if (this.RowLength < rowIndex || rowIndex < 0)
			{
				CsvGeneric.IndexOutOfException(rowIndex, true);
				return;
			}
			if (rowIndex == this.RowLength)
			{
				this.AddRow(csvRow);
				return;
			}
			for (int i = 0; i < this.ColumnLength; i++)
			{
				this._value[i].value.Insert(rowIndex, new CsvValue());
				this[csvRow.GetKey(i), rowIndex] = csvRow[i];
			}
		}

		public void SetRow(int rowIndex, CsvRow csvRow)
		{
			if (this.RowLength <= rowIndex || rowIndex < 0)
			{
				CsvGeneric.IndexOutOfException(rowIndex, true);
				return;
			}
			for (int i = 0; i < this.ColumnLength; i++)
			{
				this[i, rowIndex] = csvRow[i];
			}
		}

		public bool ContainsRow(CsvRow csvRow)
		{
			return this.RowIndexOf(csvRow) != -1;
		}

		public int RowIndexOf(CsvRow csvRow)
		{
			if (!this.CheckMachKey(csvRow))
			{
				CsvGeneric.KeyNotMachingException();
				return -1;
			}
			for (int i = 0; i < this.ColumnLength; i++)
			{
				bool flag = true;
				for (int j = 0; j < this.RowLength; j++)
				{
					if (this[i, j] != csvRow[i])
					{
						flag = false;
					}
				}
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		public int RowLength
		{
			get
			{
				return this._rowLengthCache;
			}
		}

		public void Parse(string value)
		{
			string[] array = value.Split(new char[]
			{
				'\n'
			});
			string[] column = array[0].Split(new char[]
			{
				','
			});
			this.Initialize(array.Length - 1, column);
			for (int i = 1; i < this.RowLength + 1; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					','
				});
				for (int j = 0; j < this.ColumnLength; j++)
				{
					string text = string.Empty;
					if (j < array2.Length)
					{
						text = array2[j];
					}
					text = text.Replace("\r", string.Empty);
					text = text.Replace("\t", string.Empty);
					text = text.Replace("\b", string.Empty);
					text = text.Replace("\0", string.Empty);
					this[j, i - 1].stringValue = text;
				}
			}
		}

		public void Parse(TextAsset textAsset)
		{
			if (textAsset == null)
			{
				CsvGeneric.NullReferenceException();
				return;
			}
			this.Parse(textAsset.text);
		}

		public void CopyTo(Csv csv)
		{
			this.Initialize(csv.RowLength, csv.GetKeyList());
			for (int i = 0; i < csv.RowLength; i++)
			{
				for (int j = 0; j < csv.ColumnLength; j++)
				{
					this[j, i] = new CsvValue(csv[j, i]);
				}
			}
		}

		public override string ToString()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (string str in this.GetKeyList())
			{
				text2 += str;
				text2 += ",";
			}
			text2 = text2.Trim(new char[]
			{
				',',
				' ',
				'\n'
			});
			text = text + text2 + "\n";
			for (int j = 0; j < this.RowLength; j++)
			{
				string text3 = string.Empty;
				for (int k = 0; k < this.ColumnLength; k++)
				{
					text3 += this[k, j].stringValue;
					text3 += ",";
				}
				text3 = text3.Trim(new char[]
				{
					',',
					' ',
					'\n'
				});
				text = text + text3 + "\n";
			}
			return text.Trim(new char[]
			{
				',',
				' ',
				'\n'
			});
		}

		public bool Equals(Csv csv)
		{
			for (int i = 0; i < this.ColumnLength; i++)
			{
				for (int j = 0; j < this.RowLength; j++)
				{
					if (this[i, j] != csv[i, j])
					{
						return false;
					}
				}
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() == typeof(Csv))
			{
				return this.Equals(obj as Csv);
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public CsvColumn[] ToColumn()
		{
			List<CsvColumn> list = new List<CsvColumn>();
			for (int i = 0; i < this.ColumnLength; i++)
			{
				list.Add(this.GetColumn(i));
			}
			return list.ToArray();
		}

		public CsvRow[] ToRow()
		{
			List<CsvRow> list = new List<CsvRow>();
			for (int i = 0; i < this.RowLength; i++)
			{
				list.Add(this.GetRow(i));
			}
			return list.ToArray();
		}

		public static bool operator ==(Csv a, Csv b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Csv a, Csv b)
		{
			return !(a == b);
		}

		public static Csv operator +(Csv a, CsvRow b)
		{
			a.AddRow(b);
			return a;
		}

		public static Csv operator +(CsvRow a, Csv b)
		{
			b.AddRow(a);
			return b;
		}

		public static Csv operator +(Csv a, Csv b)
		{
			string[] keyList = b.GetKeyList();
			if (a.CheckMachKey(keyList))
			{
				for (int i = 0; i < b.RowLength; i++)
				{
					a.AddRow(b.GetRow(i));
				}
			}
			else
			{
				string[] margedKeyList = a.GetMargedKeyList(b.GetKeyList());
				int num = 0;
				for (int j = 0; j < margedKeyList.Length; j++)
				{
					if (!a.ContainsColumn(margedKeyList[j]))
					{
						a.AddColumn(b.GetColumn(margedKeyList[j]));
						num++;
					}
				}
				for (int k = 0; k < num; k++)
				{
					a.AddRow();
				}
				for (int l = 0; l < a.ColumnLength; l++)
				{
					if (b.ContainsColumn(margedKeyList[l]))
					{
						for (int m = b.RowLength; m < a.RowLength; m++)
						{
							a[l, m] = b[l, m];
						}
					}
				}
			}
			return a;
		}

		public static Csv operator -(Csv a, CsvRow b)
		{
			if (!a.ContainsRow(b))
			{
				return a;
			}
			a.RemoveRow(b);
			return a;
		}

		public static Csv operator -(CsvRow a, Csv b)
		{
			if (!b.ContainsRow(a))
			{
				return b;
			}
			b.RemoveRow(a);
			return b;
		}

		public static Csv operator -(Csv a, Csv b)
		{
			string[] keyList = b.GetKeyList();
			for (int i = 0; i < keyList.Length; i++)
			{
				if (a.ContainsColumn(keyList[i]))
				{
					a.RemoveColumn(keyList[i]);
				}
			}
			return a;
		}
	}
}
