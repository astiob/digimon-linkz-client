using System;
using System.Collections;

namespace Mono.Security.X509
{
	public sealed class X509ExtensionCollection : CollectionBase, IEnumerable
	{
		private bool readOnly;

		public X509ExtensionCollection()
		{
		}

		public X509ExtensionCollection(ASN1 asn1) : this()
		{
			this.readOnly = true;
			if (asn1 == null)
			{
				return;
			}
			if (asn1.Tag != 48)
			{
				throw new Exception("Invalid extensions format");
			}
			for (int i = 0; i < asn1.Count; i++)
			{
				X509Extension value = new X509Extension(asn1[i]);
				base.InnerList.Add(value);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return base.InnerList.GetEnumerator();
		}

		public int Add(X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			if (this.readOnly)
			{
				throw new NotSupportedException("Extensions are read only");
			}
			return base.InnerList.Add(extension);
		}

		public void AddRange(X509Extension[] extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			if (this.readOnly)
			{
				throw new NotSupportedException("Extensions are read only");
			}
			for (int i = 0; i < extension.Length; i++)
			{
				base.InnerList.Add(extension[i]);
			}
		}

		public void AddRange(X509ExtensionCollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (this.readOnly)
			{
				throw new NotSupportedException("Extensions are read only");
			}
			for (int i = 0; i < collection.InnerList.Count; i++)
			{
				base.InnerList.Add(collection[i]);
			}
		}

		public bool Contains(X509Extension extension)
		{
			return this.IndexOf(extension) != -1;
		}

		public bool Contains(string oid)
		{
			return this.IndexOf(oid) != -1;
		}

		public void CopyTo(X509Extension[] extensions, int index)
		{
			if (extensions == null)
			{
				throw new ArgumentNullException("extensions");
			}
			base.InnerList.CopyTo(extensions, index);
		}

		public int IndexOf(X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				X509Extension x509Extension = (X509Extension)base.InnerList[i];
				if (x509Extension.Equals(extension))
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOf(string oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				X509Extension x509Extension = (X509Extension)base.InnerList[i];
				if (x509Extension.Oid == oid)
				{
					return i;
				}
			}
			return -1;
		}

		public void Insert(int index, X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			base.InnerList.Insert(index, extension);
		}

		public void Remove(X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			base.InnerList.Remove(extension);
		}

		public void Remove(string oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			int num = this.IndexOf(oid);
			if (num != -1)
			{
				base.InnerList.RemoveAt(num);
			}
		}

		public X509Extension this[int index]
		{
			get
			{
				return (X509Extension)base.InnerList[index];
			}
		}

		public X509Extension this[string oid]
		{
			get
			{
				int num = this.IndexOf(oid);
				if (num == -1)
				{
					return null;
				}
				return (X509Extension)base.InnerList[num];
			}
		}

		public byte[] GetBytes()
		{
			if (base.InnerList.Count < 1)
			{
				return null;
			}
			ASN1 asn = new ASN1(48);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				X509Extension x509Extension = (X509Extension)base.InnerList[i];
				asn.Add(x509Extension.ASN1);
			}
			return asn.GetBytes();
		}
	}
}
