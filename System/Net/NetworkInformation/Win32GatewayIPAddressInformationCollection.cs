using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class Win32GatewayIPAddressInformationCollection : GatewayIPAddressInformationCollection
	{
		public static readonly Win32GatewayIPAddressInformationCollection Empty = new Win32GatewayIPAddressInformationCollection(true);

		private bool is_readonly;

		private Win32GatewayIPAddressInformationCollection(bool isReadOnly)
		{
			this.is_readonly = isReadOnly;
		}

		public Win32GatewayIPAddressInformationCollection(params Win32_IP_ADDR_STRING[] al)
		{
			foreach (Win32_IP_ADDR_STRING win32_IP_ADDR_STRING in al)
			{
				if (!string.IsNullOrEmpty(win32_IP_ADDR_STRING.IpAddress))
				{
					this.Add(new GatewayIPAddressInformationImpl(IPAddress.Parse(win32_IP_ADDR_STRING.IpAddress)));
					this.AddSubsequently(win32_IP_ADDR_STRING.Next);
				}
			}
			this.is_readonly = true;
		}

		private void AddSubsequently(IntPtr head)
		{
			IntPtr intPtr = head;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADDR_STRING win32_IP_ADDR_STRING = (Win32_IP_ADDR_STRING)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADDR_STRING));
				this.Add(new GatewayIPAddressInformationImpl(IPAddress.Parse(win32_IP_ADDR_STRING.IpAddress)));
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
