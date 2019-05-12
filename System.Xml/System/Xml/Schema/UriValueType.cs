using Mono.Xml.Schema;
using System;

namespace System.Xml.Schema
{
	internal struct UriValueType
	{
		private XmlSchemaUri value;

		public UriValueType(XmlSchemaUri value)
		{
			this.value = value;
		}

		public XmlSchemaUri Value
		{
			get
			{
				return this.value;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is UriValueType && (UriValueType)obj == this;
		}

		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		public static bool operator ==(UriValueType v1, UriValueType v2)
		{
			return v1.Value == v2.Value;
		}

		public static bool operator !=(UriValueType v1, UriValueType v2)
		{
			return v1.Value != v2.Value;
		}
	}
}
