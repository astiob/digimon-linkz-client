using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace Microsoft.Win32
{
	/// <summary>Represents a key-level node in the Windows registry. This class is a registry encapsulation.</summary>
	[ComVisible(true)]
	public sealed class RegistryKey : MarshalByRefObject, IDisposable
	{
		private object handle;

		private object hive;

		private readonly string qname;

		private readonly bool isRemoteRoot;

		private readonly bool isWritable;

		private static readonly IRegistryApi RegistryApi;

		internal RegistryKey(RegistryHive hiveId) : this(hiveId, new IntPtr((int)hiveId), false)
		{
		}

		internal RegistryKey(RegistryHive hiveId, IntPtr keyHandle, bool remoteRoot)
		{
			this.hive = hiveId;
			this.handle = keyHandle;
			this.qname = RegistryKey.GetHiveName(hiveId);
			this.isRemoteRoot = remoteRoot;
			this.isWritable = true;
		}

		internal RegistryKey(object data, string keyName, bool writable)
		{
			this.handle = data;
			this.qname = keyName;
			this.isWritable = writable;
		}

		static RegistryKey()
		{
			if (Path.DirectorySeparatorChar == '\\')
			{
				RegistryKey.RegistryApi = new Win32RegistryApi();
			}
			else
			{
				RegistryKey.RegistryApi = new UnixRegistryApi();
			}
		}

		/// <summary>Performs a <see cref="M:Microsoft.Win32.RegistryKey.Close" /> on the current key.</summary>
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.Close();
		}

		~RegistryKey()
		{
			this.Close();
		}

		/// <summary>Retrieves the name of the key.</summary>
		/// <returns>The absolute (qualified) name of the key.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
		public string Name
		{
			get
			{
				return this.qname;
			}
		}

		/// <summary>Writes all the attributes of the specified open registry key into the registry.</summary>
		public void Flush()
		{
			RegistryKey.RegistryApi.Flush(this);
		}

		/// <summary>Closes the key and flushes it to disk if its contents have been modified.</summary>
		public void Close()
		{
			this.Flush();
			if (!this.isRemoteRoot && this.IsRoot)
			{
				return;
			}
			RegistryKey.RegistryApi.Close(this);
			this.handle = null;
		}

		/// <summary>Retrieves the count of subkeys of the current key.</summary>
		/// <returns>The number of subkeys of the current key.</returns>
		/// <exception cref="T:System.Security.SecurityException">The user does not have read permission for the key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <exception cref="T:System.IO.IOException">A system error occurred, for example the current key has been deleted.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SubKeyCount
		{
			get
			{
				this.AssertKeyStillValid();
				return RegistryKey.RegistryApi.SubKeyCount(this);
			}
		}

		/// <summary>Retrieves the count of values in the key.</summary>
		/// <returns>The number of name/value pairs in the key.</returns>
		/// <exception cref="T:System.Security.SecurityException">The user does not have read permission for the key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <exception cref="T:System.IO.IOException">A system error occurred, for example the current key has been deleted.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ValueCount
		{
			get
			{
				this.AssertKeyStillValid();
				return RegistryKey.RegistryApi.ValueCount(this);
			}
		}

		/// <summary>Sets the specified name/value pair.</summary>
		/// <param name="name">The name of the value to store. </param>
		/// <param name="value">The data to be stored. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is an unsupported data type.-or-<paramref name="name" /> is longer than the maximum length allowed (255 characters). </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is read-only, and cannot be written to; for example, the key has not been opened with write access. -or-The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows Millennium Edition or Windows 98.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or modify registry keys. </exception>
		/// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows 2000, Windows XP, or Windows Server 2003.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void SetValue(string name, object value)
		{
			this.AssertKeyStillValid();
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name != null)
			{
				this.AssertKeyNameLength(name);
			}
			if (!this.IsWritable)
			{
				throw new UnauthorizedAccessException("Cannot write to the registry key.");
			}
			RegistryKey.RegistryApi.SetValue(this, name, value);
		}

		/// <summary>Sets the value of a name/value pair in the registry key, using the specified registry data type.</summary>
		/// <param name="name">The name of the value to be stored. </param>
		/// <param name="value">The data to be stored. </param>
		/// <param name="valueKind">The registry data type to use when storing the data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than the maximum length allowed (255 characters).-or- The type of <paramref name="value" /> did not match the registry data type specified by <paramref name="valueKind" />, therefore the data could not be converted properly. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is read-only, and cannot be written to; for example, the key has not been opened with write access.-or-The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows Millennium Edition or Windows 98. </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or modify registry keys. </exception>
		/// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows 2000, Windows XP, or Windows Server 2003.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[ComVisible(false)]
		public void SetValue(string name, object value, RegistryValueKind valueKind)
		{
			this.AssertKeyStillValid();
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name != null)
			{
				this.AssertKeyNameLength(name);
			}
			if (!this.IsWritable)
			{
				throw new UnauthorizedAccessException("Cannot write to the registry key.");
			}
			RegistryKey.RegistryApi.SetValue(this, name, value, valueKind);
		}

		/// <summary>Retrieves a subkey as read-only.</summary>
		/// <returns>The subkey requested, or null if the operation failed.</returns>
		/// <param name="name">The name or path of the subkey to open read-only. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than the maximum length allowed (255 characters). </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read the registry key. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
		/// </PermissionSet>
		public RegistryKey OpenSubKey(string name)
		{
			return this.OpenSubKey(name, false);
		}

		/// <summary>Retrieves a specified subkey.</summary>
		/// <returns>The subkey requested, or null if the operation failed.</returns>
		/// <param name="name">Name or path of the subkey to open. </param>
		/// <param name="writable">Set to true if you need write access to the key. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than the maximum length allowed (255 characters). </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to access the registry key in the specified mode. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public RegistryKey OpenSubKey(string name, bool writable)
		{
			this.AssertKeyStillValid();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.AssertKeyNameLength(name);
			return RegistryKey.RegistryApi.OpenSubKey(this, name, writable);
		}

		/// <summary>Retrieves the value associated with the specified name. Returns null if the name/value pair does not exist in the registry.</summary>
		/// <returns>The value associated with <paramref name="name" />, or null if <paramref name="name" /> is not found.</returns>
		/// <param name="name">The name of the value to retrieve. </param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has been marked for deletion. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
		/// </PermissionSet>
		public object GetValue(string name)
		{
			return this.GetValue(name, null);
		}

		/// <summary>Retrieves the value associated with the specified name. If the name is not found, returns the default value that you provide.</summary>
		/// <returns>The value associated with <paramref name="name" />, with any embedded environment variables left unexpanded, or <paramref name="defaultValue" /> if <paramref name="name" /> is not found.</returns>
		/// <param name="name">The name of the value to retrieve. </param>
		/// <param name="defaultValue">The value to return if <paramref name="name" /> does not exist. </param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has been marked for deletion. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
		/// </PermissionSet>
		public object GetValue(string name, object defaultValue)
		{
			this.AssertKeyStillValid();
			return RegistryKey.RegistryApi.GetValue(this, name, defaultValue, RegistryValueOptions.None);
		}

		/// <summary>Retrieves the value associated with the specified name and retrieval options. If the name is not found, returns the default value that you provide.</summary>
		/// <returns>The value associated with <paramref name="name" />, processed according to the specified <paramref name="options" />, or <paramref name="defaultValue" /> if <paramref name="name" /> is not found.</returns>
		/// <param name="name">The name of the value to retrieve. </param>
		/// <param name="defaultValue">The value to return if <paramref name="name" /> does not exist. </param>
		/// <param name="options">One of the <see cref="T:Microsoft.Win32.RegistryValueOptions" /> values that specifies optional processing of the retrieved value.</param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has been marked for deletion. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not a valid <see cref="T:Microsoft.Win32.RegistryValueOptions" /> value; for example, an invalid value is cast to <see cref="T:Microsoft.Win32.RegistryValueOptions" />.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
		/// </PermissionSet>
		[ComVisible(false)]
		public object GetValue(string name, object defaultValue, RegistryValueOptions options)
		{
			this.AssertKeyStillValid();
			return RegistryKey.RegistryApi.GetValue(this, name, defaultValue, options);
		}

		/// <summary>Retrieves the registry data type of the value associated with the specified name.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryValueKind" /> value representing the registry data type of the value associated with <paramref name="name" />.</returns>
		/// <param name="name">The name of the value whose registry data type is to be retrieved. </param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.IO.IOException">The subkey that contains the specified value does not exist.-or-The name/value pair specified by <paramref name="name" /> does not exist.This exception is not thrown on Windows 95, Windows 98, or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
		/// </PermissionSet>
		[ComVisible(false)]
		public RegistryValueKind GetValueKind(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>Creates a new subkey or opens an existing subkey for write access. The string <paramref name="subkey" /> is not case-sensitive.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryKey" /> object that represents the existing or newly created subkey, or null if the operation failed. If a zero-length string is specified for <paramref name="subkey" />, the current <see cref="T:Microsoft.Win32.RegistryKey" /> object is returned.</returns>
		/// <param name="subkey">The name or path of the subkey to create or open. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="subkey" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or open the registry key. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="subkey" /> is longer than the maximum length allowed (255 characters). </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> on which this method is being invoked is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> cannot be written to; for example, it was not opened as a writable key , or the user does not have the necessary access rights. </exception>
		/// <exception cref="T:System.IO.IOException">The nesting level exceeds 510.-or-A system error occurred, such as deletion of the key, or an attempt to create a key in the <see cref="F:Microsoft.Win32.Registry.LocalMachine" /> root.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public RegistryKey CreateSubKey(string subkey)
		{
			this.AssertKeyStillValid();
			this.AssertKeyNameNotNull(subkey);
			this.AssertKeyNameLength(subkey);
			if (!this.IsWritable)
			{
				throw new UnauthorizedAccessException("Cannot write to the registry key.");
			}
			return RegistryKey.RegistryApi.CreateSubKey(this, subkey);
		}

		/// <summary>Creates a new subkey or opens an existing subkey for write access, using the specified permission check option. The string <paramref name="subkey" /> is not case-sensitive.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryKey" /> object that represents the existing or newly created subkey, or null if the operation failed. If a zero-length string is specified for <paramref name="subkey" />, the current <see cref="T:Microsoft.Win32.RegistryKey" /> object is returned.</returns>
		/// <param name="subkey">The name or path of the subkey to create or open.</param>
		/// <param name="permissionCheck">One of the <see cref="T:Microsoft.Win32.RegistryKeyPermissionCheck" /> values that specifies whether the key is opened for read or read/write access.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="subkey" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or open the registry key. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="subkey" /> is longer than the maximum length allowed (255 characters). -or-<paramref name="permissionCheck" /> contains an invalid value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> on which this method is being invoked is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> cannot be written to; for example, it was not opened as a writable key, or the user does not have the necessary access rights. </exception>
		/// <exception cref="T:System.IO.IOException">The nesting level exceeds 510.-or-A system error occurred, such as deletion of the key, or an attempt to create a key in the <see cref="F:Microsoft.Win32.Registry.LocalMachine" /> root.</exception>
		[ComVisible(false)]
		public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
		{
			throw new NotImplementedException();
		}

		/// <summary>Creates a new subkey or opens an existing subkey for write access, using the specified permission check option and registry security. The string <paramref name="subkey" /> is not case-sensitive.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryKey" /> object that represents existing or the newly created subkey, or null if the operation failed. If a zero-length string is specified for <paramref name="subkey" />, the current <see cref="T:Microsoft.Win32.RegistryKey" /> object is returned.</returns>
		/// <param name="subkey">The name or path of the subkey to create or open.</param>
		/// <param name="permissionCheck">One of the <see cref="T:Microsoft.Win32.RegistryKeyPermissionCheck" /> values that specifies whether the key is opened for read or read/write access.</param>
		/// <param name="registrySecurity">A <see cref="T:System.Security.AccessControl.RegistrySecurity" />  object that specifies the access control security for the new key.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="subkey" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or open the registry key. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="subkey" /> is longer than the maximum length allowed (255 characters). -or-<paramref name="permissionCheck" /> contains an invalid value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> on which this method is being invoked is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The current <see cref="T:Microsoft.Win32.RegistryKey" /> cannot be written to; for example, it was not opened as a writable key, or the user does not have the necessary access rights.</exception>
		/// <exception cref="T:System.IO.IOException">The nesting level exceeds 510.-or-A system error occurred, such as deletion of the key, or an attempt to create a key in the <see cref="F:Microsoft.Win32.Registry.LocalMachine" /> root.</exception>
		[ComVisible(false)]
		public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
		{
			throw new NotImplementedException();
		}

		/// <summary>Deletes the specified subkey. The string <paramref name="subkey" /> is not case-sensitive.</summary>
		/// <param name="subkey">The name of the subkey to delete. </param>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="subkey" /> has child subkeys </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="subkey" /> parameter does not specify a valid registry key </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="subkey" /> is null</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void DeleteSubKey(string subkey)
		{
			this.DeleteSubKey(subkey, true);
		}

		/// <summary>Deletes the specified subkey. The string subkey is not case-sensitive.</summary>
		/// <param name="subkey">The name of the subkey to delete. </param>
		/// <param name="throwOnMissingSubKey">Indicates whether an exception should be raised if the specified subkey cannot be found. If this argument is true and the specified subkey does not exist, then an exception is raised. If this argument is false and the specified subkey does not exist, then no action is taken </param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="subkey" /> has child subkeys. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="subkey" /> does not specify a valid registry key and <paramref name="throwOnMissingSubKey" /> is true. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="subkey" /> is null.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
		{
			this.AssertKeyStillValid();
			this.AssertKeyNameNotNull(subkey);
			this.AssertKeyNameLength(subkey);
			if (!this.IsWritable)
			{
				throw new UnauthorizedAccessException("Cannot write to the registry key.");
			}
			RegistryKey registryKey = this.OpenSubKey(subkey);
			if (registryKey == null)
			{
				if (throwOnMissingSubKey)
				{
					throw new ArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
				}
				return;
			}
			else
			{
				if (registryKey.SubKeyCount > 0)
				{
					throw new InvalidOperationException("Registry key has subkeys and recursive removes are not supported by this method.");
				}
				registryKey.Close();
				RegistryKey.RegistryApi.DeleteKey(this, subkey, throwOnMissingSubKey);
				return;
			}
		}

		/// <summary>Deletes a subkey and any child subkeys recursively. The string <paramref name="subkey" /> is not case-sensitive.</summary>
		/// <param name="subkey">The subkey to delete. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="subkey" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">Deletion of a root hive is attempted.-or-<paramref name="subkey" /> does not specify a valid registry subkey. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error has occurred.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void DeleteSubKeyTree(string subkey)
		{
			this.AssertKeyStillValid();
			this.AssertKeyNameNotNull(subkey);
			this.AssertKeyNameLength(subkey);
			RegistryKey registryKey = this.OpenSubKey(subkey, true);
			if (registryKey == null)
			{
				throw new ArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
			}
			registryKey.DeleteChildKeysAndValues();
			registryKey.Close();
			this.DeleteSubKey(subkey, false);
		}

		/// <summary>Deletes the specified value from this key.</summary>
		/// <param name="name">The name of the value to delete. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid reference to a value. </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the value. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is read-only. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void DeleteValue(string name)
		{
			this.DeleteValue(name, true);
		}

		/// <summary>Deletes the specified value from this key.</summary>
		/// <param name="name">The name of the value to delete. </param>
		/// <param name="throwOnMissingValue">Indicates whether an exception should be raised if the specified value cannot be found. If this argument is true and the specified value does not exist, then an exception is raised. If this argument is false and the specified value does not exist, then no action is taken </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid reference to a value and <paramref name="throwOnMissingValue" /> is true. -or- <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the value. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is read-only. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void DeleteValue(string name, bool throwOnMissingValue)
		{
			this.AssertKeyStillValid();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!this.IsWritable)
			{
				throw new UnauthorizedAccessException("Cannot write to the registry key.");
			}
			RegistryKey.RegistryApi.DeleteValue(this, name, throwOnMissingValue);
		}

		/// <summary>Returns the access control security for the current registry key.</summary>
		/// <returns>A <see cref="T:System.Security.AccessControl.RegistrySecurity" /> object that describes the access control permissions on the registry key represented by the current <see cref="T:Microsoft.Win32.RegistryKey" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the necessary permissions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed).</exception>
		/// <exception cref="T:System.InvalidOperationException">The current key has been deleted.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public RegistrySecurity GetAccessControl()
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns the specified sections of the access control security for the current registry key.</summary>
		/// <returns>A <see cref="T:System.Security.AccessControl.RegistrySecurity" /> object that describes the access control permissions on the registry key represented by the current <see cref="T:Microsoft.Win32.RegistryKey" />.</returns>
		/// <param name="includeSections">A bitwise combination of <see cref="T:System.Security.AccessControl.AccessControlSections" /> values that specifies the type of security information to get. </param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the necessary permissions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed).</exception>
		/// <exception cref="T:System.InvalidOperationException">The current key has been deleted.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public RegistrySecurity GetAccessControl(AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves an array of strings that contains all the subkey names.</summary>
		/// <returns>An array of strings that contains the names of the subkeys for the current key.</returns>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <exception cref="T:System.IO.IOException">A system error occurred, for example the current key has been deleted.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string[] GetSubKeyNames()
		{
			this.AssertKeyStillValid();
			return RegistryKey.RegistryApi.GetSubKeyNames(this);
		}

		/// <summary>Retrieves an array of strings that contains all the value names associated with this key.</summary>
		/// <returns>An array of strings that contains the value names for the current key.</returns>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" />  being manipulated is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <exception cref="T:System.IO.IOException">A system error occurred; for example, the current key has been deleted.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string[] GetValueNames()
		{
			this.AssertKeyStillValid();
			return RegistryKey.RegistryApi.GetValueNames(this);
		}

		/// <summary>Opens a new <see cref="T:Microsoft.Win32.RegistryKey" /> that represents the requested key on a remote machine.</summary>
		/// <returns>The requested <see cref="T:Microsoft.Win32.RegistryKey" />.</returns>
		/// <param name="hKey">The HKEY to open, from the <see cref="T:Microsoft.Win32.RegistryHive" /> enumeration. </param>
		/// <param name="machineName">The remote machine. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="hKey" /> is invalid.</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="machineName" /> is not found.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="machineName" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the proper permissions to perform this operation. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[MonoTODO("Not implemented on unix")]
		public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
		{
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			return RegistryKey.RegistryApi.OpenRemoteBaseKey(hKey, machineName);
		}

		/// <summary>Retrieves the specified subkey for read or read/write access.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryKey" /> object representing the subkey requested, or null if the operation failed.</returns>
		/// <param name="name">The name or path of the subkey to create or open.</param>
		/// <param name="permissionCheck">One of the <see cref="T:Microsoft.Win32.RegistryKeyPermissionCheck" /> values that specifies whether the key is opened for read or read/write access.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than the maximum length allowed (255 characters). -or-<paramref name="permissionCheck" /> contains an invalid value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read the registry key. </exception>
		[ComVisible(false)]
		public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the specified subkey for read or read/write access, requesting the specified access rights.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.RegistryKey" /> object representing the subkey requested, or null if the operation failed.</returns>
		/// <param name="name">The name or path of the subkey to create or open.</param>
		/// <param name="permissionCheck">One of the <see cref="T:Microsoft.Win32.RegistryKeyPermissionCheck" /> values that specifies whether the key is opened for read or read/write access.</param>
		/// <param name="rights">A bitwise combination of <see cref="T:System.Security.AccessControl.RegistryRights" />  values that specifies the desired security access.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than the maximum length allowed (255 characters). -or-<paramref name="permissionCheck" /> contains an invalid value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
		/// <exception cref="T:System.Security.SecurityException">
		///   <paramref name="rights" /> includes invalid registry rights values.-or-The user does not have the requested permissions. </exception>
		[ComVisible(false)]
		public RegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
		{
			throw new NotImplementedException();
		}

		/// <summary>Applies Windows access control security to an existing registry key.</summary>
		/// <param name="registrySecurity">A <see cref="T:System.Security.AccessControl.RegistrySecurity" /> object that specifies the access control security to apply to the current subkey. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The current <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a key with access control security, and the caller does not have <see cref="F:System.Security.AccessControl.RegistryRights.ChangePermissions" /> rights.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="registrySecurity" /> is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed).</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void SetAccessControl(RegistrySecurity registrySecurity)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves a string representation of this key.</summary>
		/// <returns>A string representing the key. If the specified key is invalid (cannot be found) then null is returned.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being accessed is closed (closed keys cannot be accessed). </exception>
		public override string ToString()
		{
			this.AssertKeyStillValid();
			return RegistryKey.RegistryApi.ToString(this);
		}

		internal bool IsRoot
		{
			get
			{
				return this.hive != null;
			}
		}

		private bool IsWritable
		{
			get
			{
				return this.isWritable;
			}
		}

		internal RegistryHive Hive
		{
			get
			{
				if (!this.IsRoot)
				{
					throw new NotSupportedException();
				}
				return (RegistryHive)((int)this.hive);
			}
		}

		internal object Handle
		{
			get
			{
				return this.handle;
			}
		}

		private void AssertKeyStillValid()
		{
			if (this.handle == null)
			{
				throw new ObjectDisposedException("Microsoft.Win32.RegistryKey");
			}
		}

		private void AssertKeyNameNotNull(string subKeyName)
		{
			if (subKeyName == null)
			{
				throw new ArgumentNullException("name");
			}
		}

		private void AssertKeyNameLength(string name)
		{
			if (name.Length > 255)
			{
				throw new ArgumentException("Name of registry key cannot be greater than 255 characters");
			}
		}

		private void DeleteChildKeysAndValues()
		{
			if (this.IsRoot)
			{
				return;
			}
			string[] subKeyNames = this.GetSubKeyNames();
			foreach (string text in subKeyNames)
			{
				RegistryKey registryKey = this.OpenSubKey(text, true);
				registryKey.DeleteChildKeysAndValues();
				registryKey.Close();
				this.DeleteSubKey(text, false);
			}
			string[] valueNames = this.GetValueNames();
			foreach (string name in valueNames)
			{
				this.DeleteValue(name, false);
			}
		}

		internal static string DecodeString(byte[] data)
		{
			string text = Encoding.Unicode.GetString(data);
			int num = text.IndexOf('\0');
			if (num != -1)
			{
				text = text.TrimEnd(new char[1]);
			}
			return text;
		}

		internal static IOException CreateMarkedForDeletionException()
		{
			throw new IOException("Illegal operation attempted on a registry key that has been marked for deletion.");
		}

		private static string GetHiveName(RegistryHive hive)
		{
			switch (hive + -2147483648)
			{
			case (RegistryHive)0:
				return "HKEY_CLASSES_ROOT";
			case (RegistryHive)1:
				return "HKEY_CURRENT_USER";
			case (RegistryHive)2:
				return "HKEY_LOCAL_MACHINE";
			case (RegistryHive)3:
				return "HKEY_USERS";
			case (RegistryHive)4:
				return "HKEY_PERFORMANCE_DATA";
			case (RegistryHive)5:
				return "HKEY_CURRENT_CONFIG";
			case (RegistryHive)6:
				return "HKEY_DYN_DATA";
			default:
				throw new NotImplementedException(string.Format("Registry hive '{0}' is not implemented.", hive.ToString()));
			}
		}
	}
}
