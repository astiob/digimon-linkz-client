using System;
using System.Collections.Generic;
using System.Net.NetworkInformation.MacOsStructs;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class MacOsNetworkInterface : UnixNetworkInterface
	{
		private const int AF_INET = 2;

		private const int AF_INET6 = 30;

		private const int AF_LINK = 18;

		private MacOsNetworkInterface(string name) : base(name)
		{
		}

		[DllImport("libc")]
		private static extern int getifaddrs(out IntPtr ifap);

		[DllImport("libc")]
		private static extern void freeifaddrs(IntPtr ifap);

		public static NetworkInterface[] ImplGetAllNetworkInterfaces()
		{
			Dictionary<string, MacOsNetworkInterface> dictionary = new Dictionary<string, MacOsNetworkInterface>();
			IntPtr intPtr;
			if (MacOsNetworkInterface.getifaddrs(out intPtr) != 0)
			{
				throw new SystemException("getifaddrs() failed");
			}
			try
			{
				IntPtr intPtr2 = intPtr;
				while (intPtr2 != IntPtr.Zero)
				{
					ifaddrs ifaddrs = (ifaddrs)Marshal.PtrToStructure(intPtr2, typeof(ifaddrs));
					IPAddress ipaddress = IPAddress.None;
					string ifa_name = ifaddrs.ifa_name;
					int index = -1;
					byte[] array = null;
					NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Unknown;
					if (ifaddrs.ifa_addr != IntPtr.Zero)
					{
						sockaddr sockaddr = (sockaddr)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr));
						if (sockaddr.sa_family == 30)
						{
							sockaddr_in6 sockaddr_in = (sockaddr_in6)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_in6));
							ipaddress = new IPAddress(sockaddr_in.sin6_addr.u6_addr8, (long)((ulong)sockaddr_in.sin6_scope_id));
						}
						else if (sockaddr.sa_family == 2)
						{
							ipaddress = new IPAddress((long)((ulong)((sockaddr_in)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_in))).sin_addr));
						}
						else if (sockaddr.sa_family == 18)
						{
							sockaddr_dl sockaddr_dl = (sockaddr_dl)Marshal.PtrToStructure(ifaddrs.ifa_addr, typeof(sockaddr_dl));
							array = new byte[(int)sockaddr_dl.sdl_alen];
							Array.Copy(sockaddr_dl.sdl_data, (int)sockaddr_dl.sdl_nlen, array, 0, Math.Min(array.Length, sockaddr_dl.sdl_data.Length - (int)sockaddr_dl.sdl_nlen));
							index = (int)sockaddr_dl.sdl_index;
							int sdl_type = (int)sockaddr_dl.sdl_type;
							if (Enum.IsDefined(typeof(MacOsArpHardware), sdl_type))
							{
								MacOsArpHardware macOsArpHardware = (MacOsArpHardware)sdl_type;
								switch (macOsArpHardware)
								{
								case MacOsArpHardware.PPP:
									networkInterfaceType = NetworkInterfaceType.Ppp;
									break;
								case MacOsArpHardware.LOOPBACK:
									networkInterfaceType = NetworkInterfaceType.Loopback;
									array = null;
									break;
								default:
									if (macOsArpHardware != MacOsArpHardware.ETHER)
									{
										if (macOsArpHardware != MacOsArpHardware.FDDI)
										{
											if (macOsArpHardware == MacOsArpHardware.ATM)
											{
												networkInterfaceType = NetworkInterfaceType.Atm;
											}
										}
										else
										{
											networkInterfaceType = NetworkInterfaceType.Fddi;
										}
									}
									else
									{
										networkInterfaceType = NetworkInterfaceType.Ethernet;
									}
									break;
								case MacOsArpHardware.SLIP:
									networkInterfaceType = NetworkInterfaceType.Slip;
									break;
								}
							}
						}
					}
					MacOsNetworkInterface macOsNetworkInterface = null;
					if (!dictionary.TryGetValue(ifa_name, out macOsNetworkInterface))
					{
						macOsNetworkInterface = new MacOsNetworkInterface(ifa_name);
						dictionary.Add(ifa_name, macOsNetworkInterface);
					}
					if (!ipaddress.Equals(IPAddress.None))
					{
						macOsNetworkInterface.AddAddress(ipaddress);
					}
					if (array != null || networkInterfaceType == NetworkInterfaceType.Loopback)
					{
						macOsNetworkInterface.SetLinkLayerInfo(index, array, networkInterfaceType);
					}
					intPtr2 = ifaddrs.ifa_next;
				}
			}
			finally
			{
				MacOsNetworkInterface.freeifaddrs(intPtr);
			}
			NetworkInterface[] array2 = new NetworkInterface[dictionary.Count];
			int num = 0;
			foreach (NetworkInterface networkInterface in dictionary.Values)
			{
				array2[num] = networkInterface;
				num++;
			}
			return array2;
		}

		public override IPInterfaceProperties GetIPProperties()
		{
			if (this.ipproperties == null)
			{
				this.ipproperties = new MacOsIPInterfaceProperties(this, this.addresses);
			}
			return this.ipproperties;
		}

		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			if (this.ipv4stats == null)
			{
				this.ipv4stats = new MacOsIPv4InterfaceStatistics(this);
			}
			return this.ipv4stats;
		}

		public override OperationalStatus OperationalStatus
		{
			get
			{
				return OperationalStatus.Unknown;
			}
		}

		public override bool SupportsMulticast
		{
			get
			{
				return false;
			}
		}
	}
}
