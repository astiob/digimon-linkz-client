using System;

namespace System.Xml
{
	internal class XmlNameEntry
	{
		public string Prefix;

		public string LocalName;

		public string NS;

		public int Hash;

		private string prefixed_name_cache;

		public XmlNameEntry(string prefix, string local, string ns)
		{
			this.Update(prefix, local, ns);
		}

		public void Update(string prefix, string local, string ns)
		{
			this.Prefix = prefix;
			this.LocalName = local;
			this.NS = ns;
			this.Hash = local.GetHashCode() + ((prefix.Length <= 0) ? 0 : prefix.GetHashCode());
		}

		public override bool Equals(object other)
		{
			XmlNameEntry xmlNameEntry = other as XmlNameEntry;
			return xmlNameEntry != null && xmlNameEntry.Hash == this.Hash && object.ReferenceEquals(xmlNameEntry.LocalName, this.LocalName) && object.ReferenceEquals(xmlNameEntry.NS, this.NS) && object.ReferenceEquals(xmlNameEntry.Prefix, this.Prefix);
		}

		public override int GetHashCode()
		{
			return this.Hash;
		}

		public string GetPrefixedName(XmlNameEntryCache owner)
		{
			if (this.prefixed_name_cache == null)
			{
				this.prefixed_name_cache = owner.GetAtomizedPrefixedName(this.Prefix, this.LocalName);
			}
			return this.prefixed_name_cache;
		}
	}
}
