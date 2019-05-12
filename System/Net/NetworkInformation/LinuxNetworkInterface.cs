using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class LinuxNetworkInterface : UnixNetworkInterface
	{
		private const int AF_INET = 2;

		private const int AF_INET6 = 10;

		private const int AF_PACKET = 17;

		private NetworkInterfaceType type;

		private string iface_path;

		private string iface_operstate_path;

		private string iface_flags_path;

		private LinuxNetworkInterface(string name) : base(name)
		{
			this.iface_path = "/sys/class/net/" + name + "/";
			this.iface_operstate_path = this.iface_path + "operstate";
			this.iface_flags_path = this.iface_path + "flags";
		}

		static LinuxNetworkInterface()
		{
			LinuxNetworkInterface.InitializeInterfaceAddresses();
		}

		internal string IfacePath
		{
			get
			{
				return this.iface_path;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitializeInterfaceAddresses();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetInterfaceAddresses(out IntPtr ifap);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FreeInterfaceAddresses(IntPtr ifap);

		public static NetworkInterface[] ImplGetAllNetworkInterfaces()
		{
			Dictionary<string, LinuxNetworkInterface> dictionary = new Dictionary<string, LinuxNetworkInterface>();
			IntPtr intPtr;
			if (LinuxNetworkInterface.GetInterfaceAddresses(out intPtr) != 0)
			{
				throw new SystemException("getifaddrs() failed");
			}
			try
			{
				IntPtr intPtr2 = intPtr;
				int num = 0;
				while (intPtr2 != IntPtr.Zero)
				{
					ifaddrs ifaddrs = (ifaddrs)Marshal.PtrToStructure(intPtr2, typeof(ifaddrs));
					IPAddress ipaddress = IPAddress.None;
					string text = ifaddrs.ifa_name;
					int index = -1;
					byte[] array = null;
					NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Unknown;
					if (ifaddrs.ifa_addr != IntPtr.Zero)
					{
						sockaddr_in sockaddr_in = (sockaddr_in)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_in));
						if (sockaddr_in.sin_family == 10)
						{
							sockaddr_in6 sockaddr_in2 = (sockaddr_in6)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_in6));
							ipaddress = new IPAddress(sockaddr_in2.sin6_addr.u6_addr8, (long)((ulong)sockaddr_in2.sin6_scope_id));
						}
						else if (sockaddr_in.sin_family == 2)
						{
							ipaddress = new IPAddress((long)((ulong)sockaddr_in.sin_addr));
						}
						else if (sockaddr_in.sin_family == 17)
						{
							sockaddr_ll sockaddr_ll = (sockaddr_ll)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_ll));
							if ((int)sockaddr_ll.sll_halen > sockaddr_ll.sll_addr.Length)
							{
								Console.Error.WriteLine("Got a bad hardware address length for an AF_PACKET {0} {1}", sockaddr_ll.sll_halen, sockaddr_ll.sll_addr.Length);
								intPtr2 = ifaddrs.ifa_next;
								continue;
							}
							array = new byte[(int)sockaddr_ll.sll_halen];
							Array.Copy(sockaddr_ll.sll_addr, 0, array, 0, array.Length);
							index = sockaddr_ll.sll_ifindex;
							int sll_hatype = (int)sockaddr_ll.sll_hatype;
							if (Enum.IsDefined(typeof(LinuxArpHardware), sll_hatype))
							{
								LinuxArpHardware linuxArpHardware = (LinuxArpHardware)sll_hatype;
								switch (linuxArpHardware)
								{
								case LinuxArpHardware.TUNNEL:
									break;
								case LinuxArpHardware.TUNNEL6:
									break;
								default:
									switch (linuxArpHardware)
									{
									case LinuxArpHardware.ETHER:
										break;
									case LinuxArpHardware.EETHER:
										break;
									default:
										if (linuxArpHardware == LinuxArpHardware.ATM)
										{
											networkInterfaceType = NetworkInterfaceType.Atm;
											goto IL_27D;
										}
										if (linuxArpHardware == LinuxArpHardware.SLIP)
										{
											networkInterfaceType = NetworkInterfaceType.Slip;
											goto IL_27D;
										}
										if (linuxArpHardware != LinuxArpHardware.PPP)
										{
											goto IL_27D;
										}
										networkInterfaceType = NetworkInterfaceType.Ppp;
										goto IL_27D;
									case LinuxArpHardware.PRONET:
										networkInterfaceType = NetworkInterfaceType.TokenRing;
										goto IL_27D;
									}
									networkInterfaceType = NetworkInterfaceType.Ethernet;
									goto IL_27D;
								case LinuxArpHardware.LOOPBACK:
									networkInterfaceType = NetworkInterfaceType.Loopback;
									array = null;
									goto IL_27D;
								case LinuxArpHardware.FDDI:
									networkInterfaceType = NetworkInterfaceType.Fddi;
									goto IL_27D;
								}
								networkInterfaceType = NetworkInterfaceType.Tunnel;
							}
						}
					}
					IL_27D:
					LinuxNetworkInterface linuxNetworkInterface = null;
					if (string.IsNullOrEmpty(text))
					{
						string str = "\0";
						int num2;
						num = (num2 = num + 1);
						text = str + num2.ToString();
					}
					if (!dictionary.TryGetValue(text, out linuxNetworkInterface))
					{
						linuxNetworkInterface = new LinuxNetworkInterface(text);
						dictionary.Add(text, linuxNetworkInterface);
					}
					if (!ipaddress.Equals(IPAddress.None))
					{
						linuxNetworkInterface.AddAddress(ipaddress);
					}
					if (array != null || networkInterfaceType == NetworkInterfaceType.Loopback)
					{
						if (networkInterfaceType == NetworkInterfaceType.Ethernet && Directory.Exists(linuxNetworkInterface.IfacePath + "wireless"))
						{
							networkInterfaceType = NetworkInterfaceType.Wireless80211;
						}
						linuxNetworkInterface.SetLinkLayerInfo(index, array, networkInterfaceType);
					}
					intPtr2 = ifaddrs.ifa_next;
				}
			}
			finally
			{
				LinuxNetworkInterface.FreeInterfaceAddresses(intPtr);
			}
			NetworkInterface[] array2 = new NetworkInterface[dictionary.Count];
			int num3 = 0;
			foreach (NetworkInterface networkInterface in dictionary.Values)
			{
				array2[num3] = networkInterface;
				num3++;
			}
			return array2;
		}

		public override IPInterfaceProperties GetIPProperties()
		{
			if (this.ipproperties == null)
			{
				this.ipproperties = new LinuxIPInterfaceProperties(this, this.addresses);
			}
			return this.ipproperties;
		}

		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			if (this.ipv4stats == null)
			{
				this.ipv4stats = new LinuxIPv4InterfaceStatistics(this);
			}
			return this.ipv4stats;
		}

		public override OperationalStatus OperationalStatus
		{
			get
			{
				if (!Directory.Exists(this.iface_path))
				{
					return OperationalStatus.Unknown;
				}
				try
				{
					string text = NetworkInterface.ReadLine(this.iface_operstate_path);
					string text2 = text;
					switch (text2)
					{
					case "unknown":
						return OperationalStatus.Unknown;
					case "notpresent":
						return OperationalStatus.NotPresent;
					case "down":
						return OperationalStatus.Down;
					case "lowerlayerdown":
						return OperationalStatus.LowerLayerDown;
					case "testing":
						return OperationalStatus.Testing;
					case "dormant":
						return OperationalStatus.Dormant;
					case "up":
						return OperationalStatus.Up;
					}
				}
				catch
				{
				}
				return OperationalStatus.Unknown;
			}
		}

		public override bool SupportsMulticast
		{
			get
			{
				if (!Directory.Exists(this.iface_path))
				{
					return false;
				}
				bool result;
				try
				{
					string text = NetworkInterface.ReadLine(this.iface_flags_path);
					if (text.Length > 2 && text[0] == '0' && text[1] == 'x')
					{
						text = text.Substring(2);
					}
					ulong num = ulong.Parse(text, NumberStyles.HexNumber);
					result = ((num & 4096UL) == 4096UL);
				}
				catch
				{
					result = false;
				}
				return result;
			}
		}
	}
}
