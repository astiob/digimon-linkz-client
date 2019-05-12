using System;

namespace System.Xml.Schema
{
	internal struct QNameValueType
	{
		private XmlQualifiedName value;

		public QNameValueType(XmlQualifiedName value)
		{
			this.value = value;
		}

		public XmlQualifiedName Value
		{
			get
			{
				return this.value;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is QNameValueType && (QNameValueType)obj == this;
		}

		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		public static bool operator ==(QNameValueType v1, QNameValueType v2)
		{
			return v1.Value == v2.Value;
		}

		public static bool operator !=(QNameValueType v1, QNameValueType v2)
		{
			return v1.Value != v2.Value;
		}
	}
}
