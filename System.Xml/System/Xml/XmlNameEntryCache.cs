using System;
using System.Collections;

namespace System.Xml
{
	internal class XmlNameEntryCache
	{
		private Hashtable table = new Hashtable();

		private XmlNameTable nameTable;

		private XmlNameEntry dummy = new XmlNameEntry(string.Empty, string.Empty, string.Empty);

		private char[] cacheBuffer;

		public XmlNameEntryCache(XmlNameTable nameTable)
		{
			this.nameTable = nameTable;
		}

		public string GetAtomizedPrefixedName(string prefix, string local)
		{
			if (prefix == null || prefix.Length == 0)
			{
				return local;
			}
			if (this.cacheBuffer == null)
			{
				this.cacheBuffer = new char[20];
			}
			if (this.cacheBuffer.Length < prefix.Length + local.Length + 1)
			{
				this.cacheBuffer = new char[Math.Max(prefix.Length + local.Length + 1, this.cacheBuffer.Length << 1)];
			}
			prefix.CopyTo(0, this.cacheBuffer, 0, prefix.Length);
			this.cacheBuffer[prefix.Length] = ':';
			local.CopyTo(0, this.cacheBuffer, prefix.Length + 1, local.Length);
			return this.nameTable.Add(this.cacheBuffer, 0, prefix.Length + local.Length + 1);
		}

		public XmlNameEntry Add(string prefix, string local, string ns, bool atomic)
		{
			if (!atomic)
			{
				prefix = this.nameTable.Add(prefix);
				local = this.nameTable.Add(local);
				ns = this.nameTable.Add(ns);
			}
			XmlNameEntry xmlNameEntry = this.GetInternal(prefix, local, ns, true);
			if (xmlNameEntry == null)
			{
				xmlNameEntry = new XmlNameEntry(prefix, local, ns);
				this.table[xmlNameEntry] = xmlNameEntry;
			}
			return xmlNameEntry;
		}

		public XmlNameEntry Get(string prefix, string local, string ns, bool atomic)
		{
			return this.GetInternal(prefix, local, ns, atomic);
		}

		private XmlNameEntry GetInternal(string prefix, string local, string ns, bool atomic)
		{
			if (!atomic && (this.nameTable.Get(prefix) == null || this.nameTable.Get(local) == null || this.nameTable.Get(ns) == null))
			{
				return null;
			}
			this.dummy.Update(prefix, local, ns);
			return this.table[this.dummy] as XmlNameEntry;
		}
	}
}
