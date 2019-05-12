using System;

namespace System.Xml.Schema
{
	internal struct StringArrayValueType
	{
		private string[] value;

		public StringArrayValueType(string[] value)
		{
			this.value = value;
		}

		public string[] Value
		{
			get
			{
				return this.value;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is StringArrayValueType && (StringArrayValueType)obj == this;
		}

		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		public static bool operator ==(StringArrayValueType v1, StringArrayValueType v2)
		{
			return v1.Value == v2.Value;
		}

		public static bool operator !=(StringArrayValueType v1, StringArrayValueType v2)
		{
			return v1.Value != v2.Value;
		}
	}
}
