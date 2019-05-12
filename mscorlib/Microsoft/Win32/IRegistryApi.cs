using System;

namespace Microsoft.Win32
{
	internal interface IRegistryApi
	{
		RegistryKey CreateSubKey(RegistryKey rkey, string keyname);

		RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName);

		RegistryKey OpenSubKey(RegistryKey rkey, string keyname, bool writtable);

		void Flush(RegistryKey rkey);

		void Close(RegistryKey rkey);

		object GetValue(RegistryKey rkey, string name, object default_value, RegistryValueOptions options);

		void SetValue(RegistryKey rkey, string name, object value);

		int SubKeyCount(RegistryKey rkey);

		int ValueCount(RegistryKey rkey);

		void DeleteValue(RegistryKey rkey, string value, bool throw_if_missing);

		void DeleteKey(RegistryKey rkey, string keyName, bool throw_if_missing);

		string[] GetSubKeyNames(RegistryKey rkey);

		string[] GetValueNames(RegistryKey rkey);

		string ToString(RegistryKey rkey);

		void SetValue(RegistryKey rkey, string name, object value, RegistryValueKind valueKind);
	}
}
