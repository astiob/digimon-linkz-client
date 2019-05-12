using System;
using System.Collections;

namespace Mono.Security.X509
{
	[Serializable]
	public class X509CertificateCollection : CollectionBase, IEnumerable
	{
		public X509CertificateCollection()
		{
		}

		public X509CertificateCollection(X509Certificate[] value)
		{
			this.AddRange(value);
		}

		public X509CertificateCollection(X509CertificateCollection value)
		{
			this.AddRange(value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return base.InnerList.GetEnumerator();
		}

		public X509Certificate this[int index]
		{
			get
			{
				return (X509Certificate)base.InnerList[index];
			}
			set
			{
				base.InnerList[index] = value;
			}
		}

		public int Add(X509Certificate value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return base.InnerList.Add(value);
		}

		public void AddRange(X509Certificate[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				base.InnerList.Add(value[i]);
			}
		}

		public void AddRange(X509CertificateCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.InnerList.Count; i++)
			{
				base.InnerList.Add(value[i]);
			}
		}

		public bool Contains(X509Certificate value)
		{
			return this.IndexOf(value) != -1;
		}

		public void CopyTo(X509Certificate[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		public new X509CertificateCollection.X509CertificateEnumerator GetEnumerator()
		{
			return new X509CertificateCollection.X509CertificateEnumerator(this);
		}

		public override int GetHashCode()
		{
			return base.InnerList.GetHashCode();
		}

		public int IndexOf(X509Certificate value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			byte[] hash = value.Hash;
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				X509Certificate x509Certificate = (X509Certificate)base.InnerList[i];
				if (this.Compare(x509Certificate.Hash, hash))
				{
					return i;
				}
			}
			return -1;
		}

		public void Insert(int index, X509Certificate value)
		{
			base.InnerList.Insert(index, value);
		}

		public void Remove(X509Certificate value)
		{
			base.InnerList.Remove(value);
		}

		private bool Compare(byte[] array1, byte[] array2)
		{
			if (array1 == null && array2 == null)
			{
				return true;
			}
			if (array1 == null || array2 == null)
			{
				return false;
			}
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		public class X509CertificateEnumerator : IEnumerator
		{
			private IEnumerator enumerator;

			public X509CertificateEnumerator(X509CertificateCollection mappings)
			{
				this.enumerator = ((IEnumerable)mappings).GetEnumerator();
			}

			object IEnumerator.Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			bool IEnumerator.MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			void IEnumerator.Reset()
			{
				this.enumerator.Reset();
			}

			public X509Certificate Current
			{
				get
				{
					return (X509Certificate)this.enumerator.Current;
				}
			}

			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			public void Reset()
			{
				this.enumerator.Reset();
			}
		}
	}
}
