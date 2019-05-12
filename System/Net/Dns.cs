using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace System.Net
{
	/// <summary>Provides simple domain name resolution functionality.</summary>
	public static class Dns
	{
		static Dns()
		{
			System.Net.Sockets.Socket.CheckProtocolSupport();
		}

		/// <summary>Begins an asynchronous request for <see cref="T:System.Net.IPHostEntry" /> information about the specified DNS host name.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that references the asynchronous request.</returns>
		/// <param name="hostName">The DNS name of the host. </param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="stateObject">A user-defined object that contains information about the operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostName" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error was encountered executing the DNS query. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use BeginGetHostEntry instead")]
		public static IAsyncResult BeginGetHostByName(string hostName, AsyncCallback requestCallback, object stateObject)
		{
			if (hostName == null)
			{
				throw new ArgumentNullException("hostName");
			}
			Dns.GetHostByNameCallback getHostByNameCallback = new Dns.GetHostByNameCallback(Dns.GetHostByName);
			return getHostByNameCallback.BeginInvoke(hostName, requestCallback, stateObject);
		}

		/// <summary>Begins an asynchronous request to resolve a DNS host name or IP address to an <see cref="T:System.Net.IPAddress" /> instance.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that references the asynchronous request.</returns>
		/// <param name="hostName">The DNS name of the host. </param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="stateObject">A user-defined object that contains information about the operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostName" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">The caller does not have permission to access DNS information. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use BeginGetHostEntry instead")]
		public static IAsyncResult BeginResolve(string hostName, AsyncCallback requestCallback, object stateObject)
		{
			if (hostName == null)
			{
				throw new ArgumentNullException("hostName");
			}
			Dns.ResolveCallback resolveCallback = new Dns.ResolveCallback(Dns.Resolve);
			return resolveCallback.BeginInvoke(hostName, requestCallback, stateObject);
		}

		/// <summary>Asynchronously returns the Internet Protocol (IP) addresses for the specified host.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that references the asynchronous request.</returns>
		/// <param name="hostNameOrAddress">The host name or IP address to resolve.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="hostNameOrAddress" /> is greater than 126 characters. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="hostNameOrAddress" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="hostNameOrAddress" /> is an invalid IP address.</exception>
		public static IAsyncResult BeginGetHostAddresses(string hostNameOrAddress, AsyncCallback requestCallback, object stateObject)
		{
			if (hostNameOrAddress == null)
			{
				throw new ArgumentNullException("hostName");
			}
			if (hostNameOrAddress == "0.0.0.0" || hostNameOrAddress == "::0")
			{
				throw new ArgumentException("Addresses 0.0.0.0 (IPv4) and ::0 (IPv6) are unspecified addresses. You cannot use them as target address.", "hostNameOrAddress");
			}
			Dns.GetHostAddressesCallback getHostAddressesCallback = new Dns.GetHostAddressesCallback(Dns.GetHostAddresses);
			return getHostAddressesCallback.BeginInvoke(hostNameOrAddress, requestCallback, stateObject);
		}

		/// <summary>Asynchronously resolves a host name or IP address to an <see cref="T:System.Net.IPHostEntry" /> instance.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that references the asynchronous request.</returns>
		/// <param name="hostNameOrAddress">The host name or IP address to resolve.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="stateObject">A user-defined object that contains information about the operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="hostNameOrAddress" /> is greater than 126 characters. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="hostNameOrAddress" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="hostNameOrAddress" /> is an invalid IP address.</exception>
		public static IAsyncResult BeginGetHostEntry(string hostNameOrAddress, AsyncCallback requestCallback, object stateObject)
		{
			if (hostNameOrAddress == null)
			{
				throw new ArgumentNullException("hostName");
			}
			if (hostNameOrAddress == "0.0.0.0" || hostNameOrAddress == "::0")
			{
				throw new ArgumentException("Addresses 0.0.0.0 (IPv4) and ::0 (IPv6) are unspecified addresses. You cannot use them as target address.", "hostNameOrAddress");
			}
			Dns.GetHostEntryNameCallback getHostEntryNameCallback = new Dns.GetHostEntryNameCallback(Dns.GetHostEntry);
			return getHostEntryNameCallback.BeginInvoke(hostNameOrAddress, requestCallback, stateObject);
		}

		/// <summary>Asynchronously resolves an IP address to an <see cref="T:System.Net.IPHostEntry" /> instance.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that references the asynchronous request.</returns>
		/// <param name="address">The IP address to resolve.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="stateObject">A user-defined object that contains information about the operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="address" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is an invalid IP address.</exception>
		public static IAsyncResult BeginGetHostEntry(IPAddress address, AsyncCallback requestCallback, object stateObject)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			Dns.GetHostEntryIPCallback getHostEntryIPCallback = new Dns.GetHostEntryIPCallback(Dns.GetHostEntry);
			return getHostEntryIPCallback.BeginInvoke(address, requestCallback, stateObject);
		}

		/// <summary>Ends an asynchronous request for DNS information.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> object that contains DNS information about a host.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance that is returned by a call to the <see cref="M:System.Net.Dns.BeginGetHostByName(System.String,System.AsyncCallback,System.Object)" /> method.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Obsolete("Use EndGetHostEntry instead")]
		public static IPHostEntry EndGetHostByName(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AsyncResult asyncResult2 = (AsyncResult)asyncResult;
			Dns.GetHostByNameCallback getHostByNameCallback = (Dns.GetHostByNameCallback)asyncResult2.AsyncDelegate;
			return getHostByNameCallback.EndInvoke(asyncResult);
		}

		/// <summary>Ends an asynchronous request for DNS information.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> object that contains DNS information about a host.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance that is returned by a call to the <see cref="M:System.Net.Dns.BeginResolve(System.String,System.AsyncCallback,System.Object)" /> method.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Obsolete("Use EndGetHostEntry instead")]
		public static IPHostEntry EndResolve(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AsyncResult asyncResult2 = (AsyncResult)asyncResult;
			Dns.ResolveCallback resolveCallback = (Dns.ResolveCallback)asyncResult2.AsyncDelegate;
			return resolveCallback.EndInvoke(asyncResult);
		}

		/// <summary>Ends an asynchronous request for DNS information.</summary>
		/// <returns>An array of type <see cref="T:System.Net.IPAddress" /> that holds the IP addresses for the host specified by the <paramref name="hostNameOrAddress" /> parameter of <see cref="M:System.Net.Dns.BeginGetHostAddresses(System.String,System.AsyncCallback,System.Object)" />.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to the <see cref="M:System.Net.Dns.BeginGetHostAddresses(System.String,System.AsyncCallback,System.Object)" /> method.</param>
		public static IPAddress[] EndGetHostAddresses(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AsyncResult asyncResult2 = (AsyncResult)asyncResult;
			Dns.GetHostAddressesCallback getHostAddressesCallback = (Dns.GetHostAddressesCallback)asyncResult2.AsyncDelegate;
			return getHostAddressesCallback.EndInvoke(asyncResult);
		}

		/// <summary>Ends an asynchronous request for DNS information.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> instance that contains address information about the host.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to an <see cref="Overload:System.Net.Dns.BeginGetHostEntry" /> method.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		public static IPHostEntry EndGetHostEntry(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AsyncResult asyncResult2 = (AsyncResult)asyncResult;
			if (asyncResult2.AsyncDelegate is Dns.GetHostEntryIPCallback)
			{
				return ((Dns.GetHostEntryIPCallback)asyncResult2.AsyncDelegate).EndInvoke(asyncResult);
			}
			Dns.GetHostEntryNameCallback getHostEntryNameCallback = (Dns.GetHostEntryNameCallback)asyncResult2.AsyncDelegate;
			return getHostEntryNameCallback.EndInvoke(asyncResult);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetHostByName_internal(string host, out string h_name, out string[] h_aliases, out string[] h_addr_list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetHostByAddr_internal(string addr, out string h_name, out string[] h_aliases, out string[] h_addr_list);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetHostName_internal(out string h_name);

		private static IPHostEntry hostent_to_IPHostEntry(string h_name, string[] h_aliases, string[] h_addrlist)
		{
			IPHostEntry iphostEntry = new IPHostEntry();
			ArrayList arrayList = new ArrayList();
			iphostEntry.HostName = h_name;
			iphostEntry.Aliases = h_aliases;
			for (int i = 0; i < h_addrlist.Length; i++)
			{
				try
				{
					IPAddress ipaddress = IPAddress.Parse(h_addrlist[i]);
					if ((System.Net.Sockets.Socket.SupportsIPv6 && ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) || (System.Net.Sockets.Socket.SupportsIPv4 && ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
					{
						arrayList.Add(ipaddress);
					}
				}
				catch (ArgumentNullException)
				{
				}
			}
			if (arrayList.Count == 0)
			{
				throw new System.Net.Sockets.SocketException(11001);
			}
			iphostEntry.AddressList = (arrayList.ToArray(typeof(IPAddress)) as IPAddress[]);
			return iphostEntry;
		}

		/// <summary>Creates an <see cref="T:System.Net.IPHostEntry" /> instance from the specified <see cref="T:System.Net.IPAddress" />.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" />.</returns>
		/// <param name="address">An <see cref="T:System.Net.IPAddress" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="address" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use GetHostEntry instead")]
		public static IPHostEntry GetHostByAddress(IPAddress address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return Dns.GetHostByAddressFromString(address.ToString(), false);
		}

		/// <summary>Creates an <see cref="T:System.Net.IPHostEntry" /> instance from an IP address.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> instance.</returns>
		/// <param name="address">An IP address. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="address" />. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="address" /> is not a valid IP address. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use GetHostEntry instead")]
		public static IPHostEntry GetHostByAddress(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return Dns.GetHostByAddressFromString(address, true);
		}

		private static IPHostEntry GetHostByAddressFromString(string address, bool parse)
		{
			if (address.Equals("0.0.0.0"))
			{
				address = "127.0.0.1";
				parse = false;
			}
			if (parse)
			{
				IPAddress.Parse(address);
			}
			string h_name;
			string[] h_aliases;
			string[] h_addrlist;
			if (!Dns.GetHostByAddr_internal(address, out h_name, out h_aliases, out h_addrlist))
			{
				throw new System.Net.Sockets.SocketException(11001);
			}
			return Dns.hostent_to_IPHostEntry(h_name, h_aliases, h_addrlist);
		}

		/// <summary>Resolves a host name or IP address to an <see cref="T:System.Net.IPHostEntry" /> instance.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> instance that contains address information about the host specified in <paramref name="hostNameOrAddress" />.</returns>
		/// <param name="hostNameOrAddress">The host name or IP address to resolve.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="hostNameOrAddress" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="hostNameOrAddress" /> parameter is greater than 126 characters. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error was encountered when resolving the <paramref name="hostNameOrAddress" /> parameter. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="hostNameOrAddress" /> parameter is an invalid IP address. </exception>
		public static IPHostEntry GetHostEntry(string hostNameOrAddress)
		{
			if (hostNameOrAddress == null)
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			if (hostNameOrAddress == "0.0.0.0" || hostNameOrAddress == "::0")
			{
				throw new ArgumentException("Addresses 0.0.0.0 (IPv4) and ::0 (IPv6) are unspecified addresses. You cannot use them as target address.", "hostNameOrAddress");
			}
			IPAddress address;
			if (hostNameOrAddress.Length > 0 && IPAddress.TryParse(hostNameOrAddress, out address))
			{
				return Dns.GetHostEntry(address);
			}
			return Dns.GetHostByName(hostNameOrAddress);
		}

		/// <summary>Resolves an IP address to an <see cref="T:System.Net.IPHostEntry" /> instance.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> instance that contains address information about the host specified in <paramref name="address" />.</returns>
		/// <param name="address">An IP address.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="address" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="address" /> is an invalid IP address.</exception>
		public static IPHostEntry GetHostEntry(IPAddress address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return Dns.GetHostByAddressFromString(address.ToString(), false);
		}

		/// <summary>Returns the Internet Protocol (IP) addresses for the specified host.</summary>
		/// <returns>An array of type <see cref="T:System.Net.IPAddress" /> that holds the IP addresses for the host that is specified by the <paramref name="hostNameOrAddress" /> parameter.</returns>
		/// <param name="hostNameOrAddress">The host name or IP address to resolve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostNameOrAddress" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="hostNameOrAddress" /> is greater than 126 characters. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="hostNameOrAddress" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="hostNameOrAddress" /> is an invalid IP address.</exception>
		public static IPAddress[] GetHostAddresses(string hostNameOrAddress)
		{
			if (hostNameOrAddress == null)
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			if (hostNameOrAddress == "0.0.0.0" || hostNameOrAddress == "::0")
			{
				throw new ArgumentException("Addresses 0.0.0.0 (IPv4) and ::0 (IPv6) are unspecified addresses. You cannot use them as target address.", "hostNameOrAddress");
			}
			IPAddress ipaddress;
			if (hostNameOrAddress.Length > 0 && IPAddress.TryParse(hostNameOrAddress, out ipaddress))
			{
				return new IPAddress[]
				{
					ipaddress
				};
			}
			return Dns.GetHostEntry(hostNameOrAddress).AddressList;
		}

		/// <summary>Gets the DNS information for the specified DNS host name.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> object that contains host information for the address specified in <paramref name="hostName" />.</returns>
		/// <param name="hostName">The DNS name of the host. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostName" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="hostName" /> is greater than 126 characters. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="hostName" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use GetHostEntry instead")]
		public static IPHostEntry GetHostByName(string hostName)
		{
			if (hostName == null)
			{
				throw new ArgumentNullException("hostName");
			}
			string h_name;
			string[] h_aliases;
			string[] h_addrlist;
			if (!Dns.GetHostByName_internal(hostName, out h_name, out h_aliases, out h_addrlist))
			{
				throw new System.Net.Sockets.SocketException(11001);
			}
			return Dns.hostent_to_IPHostEntry(h_name, h_aliases, h_addrlist);
		}

		/// <summary>Gets the host name of the local computer.</summary>
		/// <returns>A string that contains the DNS host name of the local computer.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving the local host name. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string GetHostName()
		{
			string result;
			if (!Dns.GetHostName_internal(out result))
			{
				throw new System.Net.Sockets.SocketException(11001);
			}
			return result;
		}

		/// <summary>Resolves a DNS host name or IP address to an <see cref="T:System.Net.IPHostEntry" /> instance.</summary>
		/// <returns>An <see cref="T:System.Net.IPHostEntry" /> instance that contains address information about the host specified in <paramref name="hostName" />.</returns>
		/// <param name="hostName">A DNS-style host name or IP address. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="hostName" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="hostName" /> is greater than 126 characters. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error is encountered when resolving <paramref name="hostName" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use GetHostEntry instead")]
		public static IPHostEntry Resolve(string hostName)
		{
			if (hostName == null)
			{
				throw new ArgumentNullException("hostName");
			}
			IPHostEntry iphostEntry = null;
			try
			{
				iphostEntry = Dns.GetHostByAddress(hostName);
			}
			catch
			{
			}
			if (iphostEntry == null)
			{
				iphostEntry = Dns.GetHostByName(hostName);
			}
			return iphostEntry;
		}

		private delegate IPHostEntry GetHostByNameCallback(string hostName);

		private delegate IPHostEntry ResolveCallback(string hostName);

		private delegate IPHostEntry GetHostEntryNameCallback(string hostName);

		private delegate IPHostEntry GetHostEntryIPCallback(IPAddress hostAddress);

		private delegate IPAddress[] GetHostAddressesCallback(string hostName);
	}
}
