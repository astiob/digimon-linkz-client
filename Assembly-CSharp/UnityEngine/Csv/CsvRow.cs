using System;
using System.Collections.Generic;
using UnityEngine.Csv.Internal;

namespace UnityEngine.Csv
{
	[Serializable]
	public class CsvRow
	{
		[SerializeField]
		private string[] _key;

		[SerializeField]
		private List<CsvValue> _column;

		public CsvRow()
		{
			this._key = new string[0];
			this._column = new List<CsvValue>();
		}

		public CsvRow(Csv csv)
		{
			this._key = new string[csv.ColumnLength];
			string[] keyList = csv.GetKeyList();
			this._column = new List<CsvValue>();
			for (int i = 0; i < csv.ColumnLength; i++)
			{
				this._key[i] = keyList[i];
				this._column.Add(new CsvValue());
			}
		}

		public CsvRow(int columnLength)
		{
			this._key = new string[columnLength];
			this._column = new List<CsvValue>();
			for (int i = 0; i < columnLength; i++)
			{
				this._column.Add(new CsvValue());
			}
		}

		public CsvRow(params string[] column)
		{
			this._key = column;
			this._column = new List<CsvValue>();
			for (int i = 0; i < column.Length; i++)
			{
				this._column.Add(new CsvValue());
			}
		}

		public CsvRow(string[] column, CsvValue[] value)
		{
			this._key = column;
			this._column = new List<CsvValue>(value);
		}

		public CsvRow(CsvColumn[] csvColumn, int index)
		{
			this._key = new string[csvColumn.Length];
			this._column = new List<CsvValue>();
			for (int i = 0; i < csvColumn.Length; i++)
			{
				this._key[i] = csvColumn[i].key;
				this._column.Add(csvColumn[i][index]);
			}
		}

		public CsvRow(CsvRow csvRow)
		{
			this._key = new string[csvRow.Length];
			this._column = new List<CsvValue>();
			for (int i = 0; i < this._key.Length; i++)
			{
				this._key[i] = csvRow.GetKey(i);
				this._column.Add(new CsvValue());
			}
		}

		internal int KeyToIndex(string key)
		{
			int i;
			for (i = 0; i < this._key.Length; i++)
			{
				if (this._key[i].Trim().Equals(key.Trim()))
				{
					break;
				}
			}
			if (i == this._key.Length)
			{
				return -1;
			}
			return i;
		}

		public int Length
		{
			get
			{
				return this._key.Length;
			}
		}

		public string[] GetKeyList()
		{
			return this._key;
		}

		public string GetKey(int index)
		{
			if (index < 0 || index >= this._key.Length)
			{
				CsvGeneric.IndexOutOfException(index, false);
				return null;
			}
			return this._key[index];
		}

		public void SetKey(int index, string key)
		{
			if (index < 0 || index >= this._key.Length)
			{
				CsvGeneric.IndexOutOfException(index, false);
				return;
			}
			this._key[index] = key;
		}

		public void ReplaceKey(string currentKey, string replaceKey)
		{
			int num = this.KeyToIndex(currentKey);
			if (num == -1)
			{
				CsvGeneric.KeyNotFoundException(currentKey);
				return;
			}
			this._key[num] = replaceKey;
		}

		public CsvValue this[string key]
		{
			get
			{
				int num = this.KeyToIndex(key);
				if (num == -1)
				{
					CsvGeneric.KeyNotFoundException(key);
					return new CsvValue();
				}
				return this[num];
			}
			set
			{
				int num = this.KeyToIndex(key);
				if (num == -1)
				{
					CsvGeneric.KeyNotFoundException(key);
					return;
				}
				this[num] = value;
			}
		}

		public CsvValue this[int columnIndex]
		{
			get
			{
				if (columnIndex < 0 || columnIndex >= this._key.Length)
				{
					CsvGeneric.IndexOutOfException(columnIndex, false);
					return null;
				}
				return this._column[columnIndex];
			}
			set
			{
				if (columnIndex < 0 || columnIndex >= this._key.Length)
				{
					CsvGeneric.IndexOutOfException(columnIndex, false);
					return;
				}
				this._column[columnIndex] = value;
			}
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < this.Length; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					this._key[i],
					": ",
					this._column[i]
				});
				if (i < this.Length - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		public CsvRow CopyTo()
		{
			CsvRow csvRow = new CsvRow(this);
			for (int i = 0; i < csvRow.Length; i++)
			{
				csvRow[i] = this[i];
			}
			return csvRow;
		}

		public CsvRow Clone()
		{
			CsvRow csvRow = new CsvRow(this);
			for (int i = 0; i < csvRow.Length; i++)
			{
				csvRow[i] = new CsvValue(this[i]);
			}
			return csvRow;
		}

		public bool Equals(CsvRow row)
		{
			if (this._key.Length != row._key.Length)
			{
				return false;
			}
			for (int i = 0; i < this._key.Length; i++)
			{
				if (!this[i].Equals(row._key[i]))
				{
					return false;
				}
				if (!this._column[i].Equals(row._column[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(CsvRow a, CsvRow b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(CsvRow a, CsvRow b)
		{
			return a == b;
		}
	}
}
