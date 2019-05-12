using System;
using System.Collections;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	internal class GeneralNames
	{
		private ArrayList rfc822Name;

		private ArrayList dnsName;

		private ArrayList directoryNames;

		private ArrayList uris;

		private ArrayList ipAddr;

		private ASN1 asn;

		public GeneralNames()
		{
		}

		public GeneralNames(string[] rfc822s, string[] dnsNames, string[] ipAddresses, string[] uris)
		{
			this.asn = new ASN1(48);
			if (rfc822s != null)
			{
				this.rfc822Name = new ArrayList();
				foreach (string s in rfc822s)
				{
					this.asn.Add(new ASN1(129, Encoding.ASCII.GetBytes(s)));
					this.rfc822Name.Add(rfc822s);
				}
			}
			if (dnsNames != null)
			{
				this.dnsName = new ArrayList();
				foreach (string text in dnsNames)
				{
					this.asn.Add(new ASN1(130, Encoding.ASCII.GetBytes(text)));
					this.dnsName.Add(text);
				}
			}
			if (ipAddresses != null)
			{
				this.ipAddr = new ArrayList();
				foreach (string text2 in ipAddresses)
				{
					string[] array = text2.Split(new char[]
					{
						'.',
						':'
					});
					byte[] array2 = new byte[array.Length];
					for (int l = 0; l < array.Length; l++)
					{
						array2[l] = byte.Parse(array[l]);
					}
					this.asn.Add(new ASN1(135, array2));
					this.ipAddr.Add(text2);
				}
			}
			if (uris != null)
			{
				this.uris = new ArrayList();
				foreach (string text3 in uris)
				{
					this.asn.Add(new ASN1(134, Encoding.ASCII.GetBytes(text3)));
					this.uris.Add(text3);
				}
			}
		}

		public GeneralNames(ASN1 sequence)
		{
			int i = 0;
			while (i < sequence.Count)
			{
				byte tag = sequence[i].Tag;
				switch (tag)
				{
				case 129:
					if (this.rfc822Name == null)
					{
						this.rfc822Name = new ArrayList();
					}
					this.rfc822Name.Add(Encoding.ASCII.GetString(sequence[i].Value));
					break;
				case 130:
					if (this.dnsName == null)
					{
						this.dnsName = new ArrayList();
					}
					this.dnsName.Add(Encoding.ASCII.GetString(sequence[i].Value));
					break;
				default:
					if (tag == 164)
					{
						goto IL_CF;
					}
					break;
				case 132:
					goto IL_CF;
				case 134:
					if (this.uris == null)
					{
						this.uris = new ArrayList();
					}
					this.uris.Add(Encoding.ASCII.GetString(sequence[i].Value));
					break;
				case 135:
				{
					if (this.ipAddr == null)
					{
						this.ipAddr = new ArrayList();
					}
					byte[] value = sequence[i].Value;
					string value2 = (value.Length != 4) ? ":" : ".";
					StringBuilder stringBuilder = new StringBuilder();
					for (int j = 0; j < value.Length; j++)
					{
						stringBuilder.Append(value[j].ToString());
						if (j < value.Length - 1)
						{
							stringBuilder.Append(value2);
						}
					}
					this.ipAddr.Add(stringBuilder.ToString());
					if (this.ipAddr == null)
					{
						this.ipAddr = new ArrayList();
					}
					break;
				}
				}
				IL_1F9:
				i++;
				continue;
				IL_CF:
				if (this.directoryNames == null)
				{
					this.directoryNames = new ArrayList();
				}
				this.directoryNames.Add(X501.ToString(sequence[i][0]));
				goto IL_1F9;
			}
		}

		public string[] RFC822
		{
			get
			{
				if (this.rfc822Name == null)
				{
					return new string[0];
				}
				return (string[])this.rfc822Name.ToArray(typeof(string));
			}
		}

		public string[] DirectoryNames
		{
			get
			{
				if (this.directoryNames == null)
				{
					return new string[0];
				}
				return (string[])this.directoryNames.ToArray(typeof(string));
			}
		}

		public string[] DNSNames
		{
			get
			{
				if (this.dnsName == null)
				{
					return new string[0];
				}
				return (string[])this.dnsName.ToArray(typeof(string));
			}
		}

		public string[] UniformResourceIdentifiers
		{
			get
			{
				if (this.uris == null)
				{
					return new string[0];
				}
				return (string[])this.uris.ToArray(typeof(string));
			}
		}

		public string[] IPAddresses
		{
			get
			{
				if (this.ipAddr == null)
				{
					return new string[0];
				}
				return (string[])this.ipAddr.ToArray(typeof(string));
			}
		}

		public byte[] GetBytes()
		{
			return this.asn.GetBytes();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.rfc822Name != null)
			{
				foreach (object obj in this.rfc822Name)
				{
					string value = (string)obj;
					stringBuilder.Append("RFC822 Name=");
					stringBuilder.Append(value);
					stringBuilder.Append(Environment.NewLine);
				}
			}
			if (this.dnsName != null)
			{
				foreach (object obj2 in this.dnsName)
				{
					string value2 = (string)obj2;
					stringBuilder.Append("DNS Name=");
					stringBuilder.Append(value2);
					stringBuilder.Append(Environment.NewLine);
				}
			}
			if (this.directoryNames != null)
			{
				foreach (object obj3 in this.directoryNames)
				{
					string value3 = (string)obj3;
					stringBuilder.Append("Directory Address: ");
					stringBuilder.Append(value3);
					stringBuilder.Append(Environment.NewLine);
				}
			}
			if (this.uris != null)
			{
				foreach (object obj4 in this.uris)
				{
					string value4 = (string)obj4;
					stringBuilder.Append("URL=");
					stringBuilder.Append(value4);
					stringBuilder.Append(Environment.NewLine);
				}
			}
			if (this.ipAddr != null)
			{
				foreach (object obj5 in this.ipAddr)
				{
					string value5 = (string)obj5;
					stringBuilder.Append("IP Address=");
					stringBuilder.Append(value5);
					stringBuilder.Append(Environment.NewLine);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
