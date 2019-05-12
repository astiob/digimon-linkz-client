using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class IPAddressInformationImplCollection : IPAddressInformationCollection
	{
		public static readonly IPAddressInformationImplCollection Empty = new IPAddressInformationImplCollection(true);

		private bool is_readonly;

		private IPAddressInformationImplCollection(bool isReadOnly)
		{
			this.is_readonly = isReadOnly;
		}

		public override bool IsReadOnly
		{
			get
			{
				return this.is_readonly;
			}
		}

		public static IPAddressInformationCollection Win32FromAnycast(IntPtr ptr)
		{
			IPAddressInformationImplCollection ipaddressInformationImplCollection = new IPAddressInformationImplCollection(false);
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_ANYCAST_ADDRESS win32_IP_ADAPTER_ANYCAST_ADDRESS = (Win32_IP_ADAPTER_ANYCAST_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_ANYCAST_ADDRESS));
				ipaddressInformationImplCollection.Add(new IPAddressInformationImpl(win32_IP_ADAPTER_ANYCAST_ADDRESS.Address.GetIPAddress(), win32_IP_ADAPTER_ANYCAST_ADDRESS.LengthFlags.IsDnsEligible, win32_IP_ADAPTER_ANYCAST_ADDRESS.LengthFlags.IsTransient));
				intPtr = win32_IP_ADAPTER_ANYCAST_ADDRESS.Next;
			}
			ipaddressInformationImplCollection.is_readonly = true;
			return ipaddressInformationImplCollection;
		}

		public static IPAddressInformationImplCollection LinuxFromAnycast(IList<IPAddress> addresses)
		{
			IPAddressInformationImplCollection ipaddressInformationImplCollection = new IPAddressInformationImplCollection(false);
			foreach (IPAddress address in addresses)
			{
				ipaddressInformationImplCollection.Add(new IPAddressInformationImpl(address, false, false));
			}
			ipaddressInformationImplCollection.is_readonly = true;
			return ipaddressInformationImplCollection;
		}
	}
}
