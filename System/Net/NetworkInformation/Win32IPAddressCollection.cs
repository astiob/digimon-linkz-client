using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class Win32IPAddressCollection : IPAddressCollection
	{
		public static readonly Win32IPAddressCollection Empty = new Win32IPAddressCollection(new IntPtr[]
		{
			IntPtr.Zero
		});

		private bool is_readonly;

		private Win32IPAddressCollection()
		{
		}

		public Win32IPAddressCollection(params IntPtr[] heads)
		{
			foreach (IntPtr head in heads)
			{
				this.AddSubsequentlyString(head);
			}
			this.is_readonly = true;
		}

		public Win32IPAddressCollection(params Win32_IP_ADDR_STRING[] al)
		{
			foreach (Win32_IP_ADDR_STRING win32_IP_ADDR_STRING in al)
			{
				if (!string.IsNullOrEmpty(win32_IP_ADDR_STRING.IpAddress))
				{
					this.Add(IPAddress.Parse(win32_IP_ADDR_STRING.IpAddress));
					this.AddSubsequentlyString(win32_IP_ADDR_STRING.Next);
				}
			}
			this.is_readonly = true;
		}

		public static Win32IPAddressCollection FromAnycast(IntPtr ptr)
		{
			Win32IPAddressCollection win32IPAddressCollection = new Win32IPAddressCollection();
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_ANYCAST_ADDRESS win32_IP_ADAPTER_ANYCAST_ADDRESS = (Win32_IP_ADAPTER_ANYCAST_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_ANYCAST_ADDRESS));
				win32IPAddressCollection.Add(win32_IP_ADAPTER_ANYCAST_ADDRESS.Address.GetIPAddress());
				intPtr = win32_IP_ADAPTER_ANYCAST_ADDRESS.Next;
			}
			win32IPAddressCollection.is_readonly = true;
			return win32IPAddressCollection;
		}

		public static Win32IPAddressCollection FromDnsServer(IntPtr ptr)
		{
			Win32IPAddressCollection win32IPAddressCollection = new Win32IPAddressCollection();
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_DNS_SERVER_ADDRESS win32_IP_ADAPTER_DNS_SERVER_ADDRESS = (Win32_IP_ADAPTER_DNS_SERVER_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_DNS_SERVER_ADDRESS));
				win32IPAddressCollection.Add(win32_IP_ADAPTER_DNS_SERVER_ADDRESS.Address.GetIPAddress());
				intPtr = win32_IP_ADAPTER_DNS_SERVER_ADDRESS.Next;
			}
			win32IPAddressCollection.is_readonly = true;
			return win32IPAddressCollection;
		}

		private void AddSubsequentlyString(IntPtr head)
		{
			IntPtr intPtr = head;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADDR_STRING win32_IP_ADDR_STRING = (Win32_IP_ADDR_STRING)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADDR_STRING));
				this.Add(IPAddress.Parse(win32_IP_ADDR_STRING.IpAddress));
				intPtr = win32_IP_ADDR_STRING.Next;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return this.is_readonly;
			}
		}
	}
}
