using System;
using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
	internal class MembersSerializationSource : SerializationSource
	{
		private string elementName;

		private bool hasWrapperElement;

		private string membersHash;

		private bool writeAccessors;

		private bool literalFormat;

		public MembersSerializationSource(string elementName, bool hasWrapperElement, XmlReflectionMember[] members, bool writeAccessors, bool literalFormat, string namspace, Type[] includedTypes) : base(namspace, includedTypes)
		{
			this.elementName = elementName;
			this.hasWrapperElement = hasWrapperElement;
			this.writeAccessors = writeAccessors;
			this.literalFormat = literalFormat;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(members.Length.ToString(CultureInfo.InvariantCulture));
			foreach (XmlReflectionMember xmlReflectionMember in members)
			{
				xmlReflectionMember.AddKeyHash(stringBuilder);
			}
			this.membersHash = stringBuilder.ToString();
		}

		public override bool Equals(object o)
		{
			MembersSerializationSource membersSerializationSource = o as MembersSerializationSource;
			return membersSerializationSource != null && !(this.literalFormat = membersSerializationSource.literalFormat) && !(this.elementName != membersSerializationSource.elementName) && this.hasWrapperElement == membersSerializationSource.hasWrapperElement && !(this.membersHash != membersSerializationSource.membersHash) && base.BaseEquals(membersSerializationSource);
		}

		public override int GetHashCode()
		{
			return this.membersHash.GetHashCode();
		}
	}
}
