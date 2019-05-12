using System;
using UnityEngine.Csv.Internal;

namespace UnityEngine.Csv
{
	[Serializable]
	public class CsvValue
	{
		[SerializeField]
		private string _value;

		public CsvValue()
		{
			this._value = string.Empty;
		}

		public CsvValue(CsvValue csvValue)
		{
			this._value = csvValue._value;
		}

		public CsvValue(string value)
		{
			this._value = value;
		}

		public CsvValue(int value)
		{
			this._value = value.ToString();
		}

		public CsvValue(float value)
		{
			this._value = value.ToString();
		}

		public CsvValue(double value)
		{
			this._value = value.ToString();
		}

		public string stringValue
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		public int intValue
		{
			get
			{
				int result;
				if (int.TryParse(this._value, out result))
				{
					return result;
				}
				CsvGeneric.NotIntValueException(this._value);
				return 0;
			}
			set
			{
				this._value = value.ToString();
			}
		}

		public float floatValue
		{
			get
			{
				float result;
				if (float.TryParse(this._value, out result))
				{
					return result;
				}
				Debug.LogError("[ " + this._value + " ] is not Float value.");
				return 0f;
			}
			set
			{
				this._value = value.ToString();
			}
		}

		public double doubleValue
		{
			get
			{
				double result;
				if (double.TryParse(this._value, out result))
				{
					return result;
				}
				Debug.LogError("[ " + this._value + " ] is not Double value.");
				return 0.0;
			}
			set
			{
				this._value = value.ToString();
			}
		}

		public override string ToString()
		{
			return this._value;
		}

		public bool Equals(CsvValue obj)
		{
			return obj._value.Trim().Equals(this._value.Trim());
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(CsvValue a, CsvValue b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(CsvValue a, CsvValue b)
		{
			return a == b;
		}

		public static CsvValue operator +(CsvValue a, int i)
		{
			int num;
			if (int.TryParse(a._value, out num))
			{
				a.intValue = num + i;
			}
			else
			{
				CsvGeneric.NotIntValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator -(CsvValue a, int i)
		{
			return a + -i;
		}

		public static CsvValue operator *(CsvValue a, int i)
		{
			int num;
			if (int.TryParse(a._value, out num))
			{
				a.intValue = num * i;
			}
			else
			{
				CsvGeneric.NotIntValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator /(CsvValue a, int i)
		{
			int num;
			if (int.TryParse(a._value, out num))
			{
				a.intValue = num / i;
			}
			else
			{
				CsvGeneric.NotIntValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator +(CsvValue a, float i)
		{
			float num;
			if (float.TryParse(a._value, out num))
			{
				a.floatValue = num + i;
			}
			else
			{
				CsvGeneric.NotFloatValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator -(CsvValue a, float f)
		{
			return a + -f;
		}

		public static CsvValue operator *(CsvValue a, float i)
		{
			float num;
			if (float.TryParse(a._value, out num))
			{
				a.floatValue = num * i;
			}
			else
			{
				CsvGeneric.NotFloatValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator /(CsvValue a, float i)
		{
			float num;
			if (float.TryParse(a._value, out num))
			{
				a.floatValue = num / i;
			}
			else
			{
				CsvGeneric.NotFloatValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator +(CsvValue a, double d)
		{
			double num;
			if (double.TryParse(a._value, out num))
			{
				a.doubleValue = num + d;
			}
			else
			{
				CsvGeneric.NotDoubleValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator -(CsvValue a, double d)
		{
			return a + -d;
		}

		public static CsvValue operator *(CsvValue a, double d)
		{
			double num;
			if (double.TryParse(a._value, out num))
			{
				a.doubleValue = num * d;
			}
			else
			{
				CsvGeneric.NotDoubleValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator /(CsvValue a, double d)
		{
			double num;
			if (double.TryParse(a._value, out num))
			{
				a.doubleValue = num / d;
			}
			else
			{
				CsvGeneric.NotDoubleValueException(a._value);
			}
			return a;
		}

		public static CsvValue operator +(CsvValue a, string s)
		{
			a.stringValue += s;
			return a;
		}

		public static CsvValue operator -(CsvValue a, string s)
		{
			a.stringValue = a.stringValue.Replace(s, string.Empty);
			return a;
		}
	}
}
