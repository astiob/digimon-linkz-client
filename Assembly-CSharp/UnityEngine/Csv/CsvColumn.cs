using System;
using System.Collections.Generic;
using UnityEngine.Csv.Internal;

namespace UnityEngine.Csv
{
	[Serializable]
	public class CsvColumn
	{
		[SerializeField]
		private string _key;

		[SerializeField]
		private List<CsvValue> _raw;

		public CsvColumn(string columnKey, params CsvValue[] value)
		{
			this._key = columnKey;
			this._raw = new List<CsvValue>();
			foreach (CsvValue item in value)
			{
				this._raw.Add(item);
			}
		}

		public CsvColumn(string columnKey, int length)
		{
			this._key = columnKey;
			this._raw = new List<CsvValue>();
			for (int i = 0; i < length; i++)
			{
				this._raw.Add(new CsvValue());
			}
		}

		public CsvColumn(CsvRow[] csvRow, int index)
		{
			this._key = csvRow[index].GetKey(index);
			this._raw = new List<CsvValue>();
			for (int i = 0; i < this._key.Length; i++)
			{
				this._raw.Add(new CsvValue());
			}
		}

		public CsvColumn(CsvColumn csvColumn)
		{
			this._key = csvColumn.key;
			this._raw = new List<CsvValue>();
			for (int i = 0; i < csvColumn.Length; i++)
			{
				this._raw.Add(new CsvValue(csvColumn[i]));
			}
		}

		public CsvValue this[int row]
		{
			get
			{
				if (row < 0 || row >= this.Length)
				{
					CsvGeneric.IndexOutOfException(row, true);
					return new CsvValue();
				}
				return this._raw[row];
			}
			set
			{
				if (row < 0 || row >= this.Length)
				{
					CsvGeneric.IndexOutOfException(row, true);
					return;
				}
				this._raw[row] = value;
			}
		}

		public void Add(CsvValue value)
		{
			this._raw.Add(value);
		}

		public void RemoveAt(int index)
		{
			if (this._raw.Count >= index || index < 0)
			{
				CsvGeneric.IndexOutOfException(index, false);
				return;
			}
			this._raw.RemoveAt(index);
		}

		public void Insert(int index, CsvValue value)
		{
			this._raw.Insert(index, value);
		}

		public string key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		public int Length
		{
			get
			{
				return this._raw.Count;
			}
		}

		public override string ToString()
		{
			string text = "KEY [ " + this.key + " ], ROW [ ";
			for (int i = 0; i < this.Length; i++)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					i,
					": ",
					this._raw[i].ToString()
				});
				if (i != this.Length - 1)
				{
					text += ", ";
				}
			}
			return text + " ]";
		}

		public CsvColumn CopyTo()
		{
			return new CsvColumn(this._key, this._raw.ToArray());
		}

		public CsvColumn Clone()
		{
			CsvColumn csvColumn = new CsvColumn(this._key, this._raw.Count);
			for (int i = 0; i < csvColumn.Length; i++)
			{
				csvColumn[i] = new CsvValue(csvColumn[i]);
			}
			return csvColumn;
		}

		public bool Equals(CsvColumn column)
		{
			if (!this.key.Equals(column.key))
			{
				return false;
			}
			for (int i = 0; i < this.Length; i++)
			{
				if (this._raw[i] != column._raw[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool operator ==(CsvColumn a, CsvColumn b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(CsvColumn a, CsvColumn b)
		{
			return !(a == b);
		}

		public static CsvColumn operator +(CsvColumn a, int length)
		{
			for (int i = 0; i < length; i++)
			{
				a.Add(new CsvValue());
			}
			return a;
		}

		public static CsvColumn operator -(CsvColumn a, int length)
		{
			for (int i = 0; i < length; i++)
			{
				a.RemoveAt(a.Length - 1);
			}
			return a;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
