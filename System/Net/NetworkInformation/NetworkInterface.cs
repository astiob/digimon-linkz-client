using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides configuration and statistical information for a network interface.</summary>
	public abstract class NetworkInterface
	{
		private static Version windowsVer51 = new Version(5, 1);

		internal static readonly bool runningOnUnix = Environment.OSVersion.Platform == PlatformID.Unix;

		[DllImport("libc")]
		private static extern int uname(IntPtr buf);

		/// <summary>Returns objects that describe the network interfaces on the local computer.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.NetworkInterface" /> array that contains objects that describe the available network interfaces, or an empty array if no interfaces are detected.</returns>
		/// <exception cref="T:System.Net.NetworkInformation.NetworkInformationException">A Windows system function call failed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		///   <IPermission class="System.Net.NetworkInformation.NetworkInformationPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Access="Read" />
		/// </PermissionSet>
		[MonoTODO("Only works on Linux and Windows")]
		public static NetworkInterface[] GetAllNetworkInterfaces()
		{
			if (NetworkInterface.runningOnUnix)
			{
				bool flag = false;
				IntPtr intPtr = Marshal.AllocHGlobal(8192);
				if (NetworkInterface.uname(intPtr) == 0)
				{
					string a = Marshal.PtrToStringAnsi(intPtr);
					if (a == "Darwin")
					{
						flag = true;
					}
				}
				Marshal.FreeHGlobal(intPtr);
				try
				{
					if (flag)
					{
						return MacOsNetworkInterface.ImplGetAllNetworkInterfaces();
					}
					return LinuxNetworkInterface.ImplGetAllNetworkInterfaces();
				}
				catch (SystemException ex)
				{
					throw ex;
				}
				catch
				{
					return new NetworkInterface[0];
				}
			}
			if (Environment.OSVersion.Version >= NetworkInterface.windowsVer51)
			{
				return Win32NetworkInterface2.ImplGetAllNetworkInterfaces();
			}
			return new NetworkInterface[0];
		}

		/// <summary>Indicates whether any network connection is available.</summary>
		/// <returns>true if a network connection is available; otherwise, false.</returns>
		[MonoTODO("Always returns true")]
		public static bool GetIsNetworkAvailable()
		{
			return true;
		}

		internal static string ReadLine(string path)
		{
			string result;
			using (FileStream fileStream = File.OpenRead(path))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					result = streamReader.ReadLine();
				}
			}
			return result;
		}

		/// <summary>Gets the index of the IPv4 loopback interface.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that contains the index for the IPv4 loopback interface.</returns>
		/// <exception cref="T:System.Net.NetworkInformation.NetworkInformationException">This property is not valid on computers running only Ipv6.</exception>
		[MonoTODO("Only works on Linux. Returns 0 on other systems.")]
		public static int LoopbackInterfaceIndex
		{
			get
			{
				if (NetworkInterface.runningOnUnix)
				{
					try
					{
						return UnixNetworkInterface.IfNameToIndex("lo");
					}
					catch
					{
						return 0;
					}
					return 0;
				}
				return 0;
			}
		}

		/// <summary>Returns an object that describes the configuration of this network interface.</summary>
		/// <returns>An <see cref="T:System.Net.NetworkInformation.IPInterfaceProperties" /> object that describes this network interface.</returns>
		public abstract IPInterfaceProperties GetIPProperties();

		/// <summary>Gets the IPv4 statistics.</summary>
		/// <returns>An <see cref="T:System.Net.NetworkInformation.IPv4InterfaceStatistics" /> object.</returns>
		public abstract IPv4InterfaceStatistics GetIPv4Statistics();

		/// <summary>Returns the Media Access Control (MAC) or physical address for this adapter.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkInformation.PhysicalAddress" /> object that contains the physical address.</returns>
		public abstract PhysicalAddress GetPhysicalAddress();

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the interface supports the specified protocol.</summary>
		/// <returns>true if the specified protocol is supported; otherwise, false.</returns>
		/// <param name="networkInterfaceComponent">A <see cref="T:System.Net.NetworkInformation.NetworkInterfaceComponent" /> value.</param>
		public abstract bool Supports(NetworkInterfaceComponent networkInterfaceComponent);

		/// <summary>Gets the description of the interface.</summary>
		/// <returns>A <see cref="T:System.String" /> that describes this interface.</returns>
		public abstract string Description { get; }

		/// <summary>Gets the identifier of the network adapter.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the identifier.</returns>
		public abstract string Id { get; }

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the network interface is set to only receive data packets.</summary>
		/// <returns>true if the interface only receives network traffic; otherwise, false.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">This property is not valid on computers running operating systems earlier than Windows XP. </exception>
		public abstract bool IsReceiveOnly { get; }

		/// <summary>Gets the name of the network adapter.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the adapter name.</returns>
		public abstract string Name { get; }

		/// <summary>Gets the interface type.</summary>
		/// <returns>An <see cref="T:System.Net.NetworkInformation.NetworkInterfaceType" /> value that specifies the network interface type.</returns>
		public abstract NetworkInterfaceType NetworkInterfaceType { get; }

		/// <summary>Gets the current operational state of the network connection.</summary>
		/// <returns>One of the <see cref="T:System.Net.NetworkInformation.OperationalStatus" /> values.</returns>
		public abstract OperationalStatus OperationalStatus { get; }

		/// <summary>Gets the speed of the network interface.</summary>
		/// <returns>A <see cref="T:System.Int64" /> value that specifies the speed in bits per second.</returns>
		public abstract long Speed { get; }

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the network interface is enabled to receive multicast packets.</summary>
		/// <returns>true if the interface receives multicast packets; otherwise, false.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">This property is not valid on computers running operating systems earlier than Windows XP. </exception>
		public abstract bool SupportsMulticast { get; }
	}
}
