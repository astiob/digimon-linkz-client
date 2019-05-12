using System;
using System.Text;

namespace System.Xml.Serialization
{
	internal class SoapTypeSerializationSource : SerializationSource
	{
		private string attributeOverridesHash;

		private Type type;

		public SoapTypeSerializationSource(Type type, SoapAttributeOverrides attributeOverrides, string namspace, Type[] includedTypes) : base(namspace, includedTypes)
		{
			if (attributeOverrides != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				attributeOverrides.AddKeyHash(stringBuilder);
				this.attributeOverridesHash = stringBuilder.ToString();
			}
			this.type = type;
		}

		public override bool Equals(object o)
		{
			SoapTypeSerializationSource soapTypeSerializationSource = o as SoapTypeSerializationSource;
			return soapTypeSerializationSource != null && this.type.Equals(soapTypeSerializationSource.type) && !(this.attributeOverridesHash != soapTypeSerializationSource.attributeOverridesHash) && base.BaseEquals(soapTypeSerializationSource);
		}

		public override int GetHashCode()
		{
			return this.type.GetHashCode();
		}
	}
}
