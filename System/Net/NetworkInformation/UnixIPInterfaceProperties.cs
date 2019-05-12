using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace System.Net.NetworkInformation
{
	internal abstract class UnixIPInterfaceProperties : IPInterfaceProperties
	{
		protected IPv4InterfaceProperties ipv4iface_properties;

		protected UnixNetworkInterface iface;

		private List<IPAddress> addresses;

		private IPAddressCollection dns_servers;

		private IPAddressCollection gateways;

		private string dns_suffix;

		private DateTime last_parse;

		private static System.Text.RegularExpressions.Regex ns = new System.Text.RegularExpressions.Regex("\\s*nameserver\\s+(?<address>.*)");

		private static System.Text.RegularExpressions.Regex search = new System.Text.RegularExpressions.Regex("\\s*search\\s+(?<domain>.*)");

		public UnixIPInterfaceProperties(UnixNetworkInterface iface, List<IPAddress> addresses)
		{
			this.iface = iface;
			this.addresses = addresses;
		}

		public override IPv6InterfaceProperties GetIPv6Properties()
		{
			throw new NotImplementedException();
		}

		private void ParseRouteInfo(string iface)
		{
			try
			{
				this.gateways = new IPAddressCollection();
				using (StreamReader streamReader = new StreamReader("/proc/net/route"))
				{
					streamReader.ReadLine();
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						text = text.Trim();
						if (text.Length != 0)
						{
							string[] array = text.Split(new char[]
							{
								'\t'
							});
							if (array.Length >= 3)
							{
								string text2 = array[2].Trim();
								byte[] array2 = new byte[4];
								if (text2.Length == 8 && iface.Equals(array[0], StringComparison.OrdinalIgnoreCase))
								{
									for (int i = 0; i < 4; i++)
									{
										if (!byte.TryParse(text2.Substring(i * 2, 2), NumberStyles.HexNumber, null, out array2[3 - i]))
										{
										}
									}
									IPAddress ipaddress = new IPAddress(array2);
									if (!ipaddress.Equals(IPAddress.Any))
									{
										this.gateways.Add(ipaddress);
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		private void ParseResolvConf()
		{
			try
			{
				DateTime lastWriteTime = File.GetLastWriteTime("/etc/resolv.conf");
				if (!(lastWriteTime <= this.last_parse))
				{
					this.last_parse = lastWriteTime;
					this.dns_suffix = string.Empty;
					this.dns_servers = new IPAddressCollection();
					using (StreamReader streamReader = new StreamReader("/etc/resolv.conf"))
					{
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							text = text.Trim();
							if (text.Length != 0 && text[0] != '#')
							{
								System.Text.RegularExpressions.Match match = UnixIPInterfaceProperties.ns.Match(text);
								if (match.Success)
								{
									try
									{
										string text2 = match.Groups["address"].Value;
										text2 = text2.Trim();
										this.dns_servers.Add(IPAddress.Parse(text2));
									}
									catch
									{
									}
								}
								else
								{
									match = UnixIPInterfaceProperties.search.Match(text);
									if (match.Success)
									{
										string text2 = match.Groups["domain"].Value;
										string[] array = text2.Split(new char[]
										{
											','
										});
										this.dns_suffix = array[0].Trim();
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				this.dns_servers.SetReadOnly();
			}
		}

		public override IPAddressInformationCollection AnycastAddresses
		{
			get
			{
				List<IPAddress> list = new List<IPAddress>();
				return IPAddressInformationImplCollection.LinuxFromAnycast(list);
			}
		}

		[MonoTODO("Always returns an empty collection.")]
		public override IPAddressCollection DhcpServerAddresses
		{
			get
			{
				IPAddressCollection ipaddressCollection = new IPAddressCollection();
				ipaddressCollection.SetReadOnly();
				return ipaddressCollection;
			}
		}

		public override IPAddressCollection DnsAddresses
		{
			get
			{
				this.ParseResolvConf();
				return this.dns_servers;
			}
		}

		public override string DnsSuffix
		{
			get
			{
				this.ParseResolvConf();
				return this.dns_suffix;
			}
		}

		public override GatewayIPAddressInformationCollection GatewayAddresses
		{
			get
			{
				this.ParseRouteInfo(this.iface.Name.ToString());
				if (this.gateways.Count > 0)
				{
					return new LinuxGatewayIPAddressInformationCollection(this.gateways);
				}
				return LinuxGatewayIPAddressInformationCollection.Empty;
			}
		}

		[MonoTODO("Always returns true")]
		public override bool IsDnsEnabled
		{
			get
			{
				return true;
			}
		}

		[MonoTODO("Always returns false")]
		public override bool IsDynamicDnsEnabled
		{
			get
			{
				return false;
			}
		}

		public override MulticastIPAddressInformationCollection MulticastAddresses
		{
			get
			{
				List<IPAddress> list = new List<IPAddress>();
				foreach (IPAddress ipaddress in this.addresses)
				{
					byte[] addressBytes = ipaddress.GetAddressBytes();
					if (addressBytes[0] >= 224 && addressBytes[0] <= 239)
					{
						list.Add(ipaddress);
					}
				}
				return MulticastIPAddressInformationImplCollection.LinuxFromList(list);
			}
		}

		public override UnicastIPAddressInformationCollection UnicastAddresses
		{
			get
			{
				List<IPAddress> list = new List<IPAddress>();
				foreach (IPAddress ipaddress in this.addresses)
				{
					System.Net.Sockets.AddressFamily addressFamily = ipaddress.AddressFamily;
					if (addressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
					{
						if (addressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
						{
							if (!ipaddress.IsIPv6Multicast)
							{
								list.Add(ipaddress);
							}
						}
					}
					else
					{
						byte b = ipaddress.GetAddressBytes()[0];
						if (b < 224 || b > 239)
						{
							list.Add(ipaddress);
						}
					}
				}
				return UnicastIPAddressInformationImplCollection.LinuxFromList(list);
			}
		}

		[MonoTODO("Always returns an empty collection.")]
		public override IPAddressCollection WinsServersAddresses
		{
			get
			{
				return new IPAddressCollection();
			}
		}
	}
}
