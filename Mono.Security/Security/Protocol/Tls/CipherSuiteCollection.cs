using System;
using System.Collections;
using System.Globalization;

namespace Mono.Security.Protocol.Tls
{
	internal sealed class CipherSuiteCollection : IEnumerable, ICollection, IList
	{
		private ArrayList cipherSuites;

		private SecurityProtocolType protocol;

		public CipherSuiteCollection(SecurityProtocolType protocol)
		{
			this.protocol = protocol;
			this.cipherSuites = new ArrayList();
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (CipherSuite)value;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return this.cipherSuites.IsSynchronized;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this.cipherSuites.SyncRoot;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.cipherSuites.GetEnumerator();
		}

		bool IList.Contains(object value)
		{
			return this.cipherSuites.Contains(value as CipherSuite);
		}

		int IList.IndexOf(object value)
		{
			return this.cipherSuites.IndexOf(value as CipherSuite);
		}

		void IList.Insert(int index, object value)
		{
			this.cipherSuites.Insert(index, value as CipherSuite);
		}

		void IList.Remove(object value)
		{
			this.cipherSuites.Remove(value as CipherSuite);
		}

		void IList.RemoveAt(int index)
		{
			this.cipherSuites.RemoveAt(index);
		}

		int IList.Add(object value)
		{
			return this.cipherSuites.Add(value as CipherSuite);
		}

		public CipherSuite this[string name]
		{
			get
			{
				return (CipherSuite)this.cipherSuites[this.IndexOf(name)];
			}
			set
			{
				this.cipherSuites[this.IndexOf(name)] = value;
			}
		}

		public CipherSuite this[int index]
		{
			get
			{
				return (CipherSuite)this.cipherSuites[index];
			}
			set
			{
				this.cipherSuites[index] = value;
			}
		}

		public CipherSuite this[short code]
		{
			get
			{
				return (CipherSuite)this.cipherSuites[this.IndexOf(code)];
			}
			set
			{
				this.cipherSuites[this.IndexOf(code)] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.cipherSuites.Count;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return this.cipherSuites.IsFixedSize;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this.cipherSuites.IsReadOnly;
			}
		}

		public void CopyTo(Array array, int index)
		{
			this.cipherSuites.CopyTo(array, index);
		}

		public void Clear()
		{
			this.cipherSuites.Clear();
		}

		public int IndexOf(string name)
		{
			int num = 0;
			foreach (object obj in this.cipherSuites)
			{
				CipherSuite cipherSuite = (CipherSuite)obj;
				if (this.cultureAwareCompare(cipherSuite.Name, name))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public int IndexOf(short code)
		{
			int num = 0;
			foreach (object obj in this.cipherSuites)
			{
				CipherSuite cipherSuite = (CipherSuite)obj;
				if (cipherSuite.Code == code)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public CipherSuite Add(short code, string name, CipherAlgorithmType cipherType, HashAlgorithmType hashType, ExchangeAlgorithmType exchangeType, bool exportable, bool blockMode, byte keyMaterialSize, byte expandedKeyMaterialSize, short effectiveKeyBytes, byte ivSize, byte blockSize)
		{
			SecurityProtocolType securityProtocolType = this.protocol;
			if (securityProtocolType != SecurityProtocolType.Default)
			{
				if (securityProtocolType != SecurityProtocolType.Ssl2)
				{
					if (securityProtocolType == SecurityProtocolType.Ssl3)
					{
						return this.add(new SslCipherSuite(code, name, cipherType, hashType, exchangeType, exportable, blockMode, keyMaterialSize, expandedKeyMaterialSize, effectiveKeyBytes, ivSize, blockSize));
					}
					if (securityProtocolType == SecurityProtocolType.Tls)
					{
						goto IL_32;
					}
				}
				throw new NotSupportedException("Unsupported security protocol type.");
			}
			IL_32:
			return this.add(new TlsCipherSuite(code, name, cipherType, hashType, exchangeType, exportable, blockMode, keyMaterialSize, expandedKeyMaterialSize, effectiveKeyBytes, ivSize, blockSize));
		}

		private TlsCipherSuite add(TlsCipherSuite cipherSuite)
		{
			this.cipherSuites.Add(cipherSuite);
			return cipherSuite;
		}

		private SslCipherSuite add(SslCipherSuite cipherSuite)
		{
			this.cipherSuites.Add(cipherSuite);
			return cipherSuite;
		}

		private bool cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0;
		}
	}
}
