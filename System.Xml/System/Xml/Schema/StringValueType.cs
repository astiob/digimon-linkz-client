using System;

namespace System.Xml.Schema
{
	internal struct StringValueType
	{
		private string value;

		public StringValueType(string value)
		{
			this.value = value;
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is StringValueType && (StringValueType)obj == this;
		}

		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		public static bool operator ==(StringValueType v1, StringValueType v2)
		{
			return v1.Value == v2.Value;
		}

		public static bool operator !=(StringValueType v1, StringValueType v2)
		{
			return v1.Value != v2.Value;
		}
	}
}
