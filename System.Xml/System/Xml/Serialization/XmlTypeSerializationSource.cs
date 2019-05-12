using System;
using System.Text;

namespace System.Xml.Serialization
{
	internal class XmlTypeSerializationSource : SerializationSource
	{
		private string attributeOverridesHash;

		private Type type;

		private string rootHash;

		public XmlTypeSerializationSource(Type type, XmlRootAttribute root, XmlAttributeOverrides attributeOverrides, string namspace, Type[] includedTypes) : base(namspace, includedTypes)
		{
			if (attributeOverrides != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				attributeOverrides.AddKeyHash(stringBuilder);
				this.attributeOverridesHash = stringBuilder.ToString();
			}
			if (root != null)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				root.AddKeyHash(stringBuilder2);
				this.rootHash = stringBuilder2.ToString();
			}
			this.type = type;
		}

		public override bool Equals(object o)
		{
			XmlTypeSerializationSource xmlTypeSerializationSource = o as XmlTypeSerializationSource;
			return xmlTypeSerializationSource != null && this.type.Equals(xmlTypeSerializationSource.type) && !(this.rootHash != xmlTypeSerializationSource.rootHash) && !(this.attributeOverridesHash != xmlTypeSerializationSource.attributeOverridesHash) && base.BaseEquals(xmlTypeSerializationSource);
		}

		public override int GetHashCode()
		{
			return this.type.GetHashCode();
		}
	}
}
