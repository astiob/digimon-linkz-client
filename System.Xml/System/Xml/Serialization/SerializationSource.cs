using System;

namespace System.Xml.Serialization
{
	internal abstract class SerializationSource
	{
		private Type[] includedTypes;

		private string namspace;

		private bool canBeGenerated = true;

		public SerializationSource(string namspace, Type[] includedTypes)
		{
			this.namspace = namspace;
			this.includedTypes = includedTypes;
		}

		protected bool BaseEquals(SerializationSource other)
		{
			if (this.namspace != other.namspace)
			{
				return false;
			}
			if (this.canBeGenerated != other.canBeGenerated)
			{
				return false;
			}
			if (this.includedTypes == null)
			{
				return other.includedTypes == null;
			}
			if (other.includedTypes == null || this.includedTypes.Length != other.includedTypes.Length)
			{
				return false;
			}
			for (int i = 0; i < this.includedTypes.Length; i++)
			{
				if (!this.includedTypes[i].Equals(other.includedTypes[i]))
				{
					return false;
				}
			}
			return true;
		}

		public virtual bool CanBeGenerated
		{
			get
			{
				return this.canBeGenerated;
			}
			set
			{
				this.canBeGenerated = value;
			}
		}
	}
}
