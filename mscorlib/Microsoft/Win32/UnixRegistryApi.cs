using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Win32
{
	internal class UnixRegistryApi : IRegistryApi
	{
		private static string ToUnix(string keyname)
		{
			if (keyname.IndexOf('\\') != -1)
			{
				keyname = keyname.Replace('\\', '/');
			}
			return keyname.ToLower();
		}

		private static bool IsWellKnownKey(string parentKeyName, string keyname)
		{
			return (parentKeyName == Registry.CurrentUser.Name || parentKeyName == Registry.LocalMachine.Name) && 0 == string.Compare("software", keyname, true, CultureInfo.InvariantCulture);
		}

		public RegistryKey CreateSubKey(RegistryKey rkey, string keyname)
		{
			return this.CreateSubKey(rkey, keyname, true);
		}

		public RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
		{
			throw new NotImplementedException();
		}

		public RegistryKey OpenSubKey(RegistryKey rkey, string keyname, bool writable)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				return null;
			}
			RegistryKey registryKey = keyHandler.Probe(rkey, UnixRegistryApi.ToUnix(keyname), writable);
			if (registryKey == null && UnixRegistryApi.IsWellKnownKey(rkey.Name, keyname))
			{
				registryKey = this.CreateSubKey(rkey, keyname, writable);
			}
			return registryKey;
		}

		public void Flush(RegistryKey rkey)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, false);
			if (keyHandler == null)
			{
				return;
			}
			keyHandler.Flush();
		}

		public void Close(RegistryKey rkey)
		{
			KeyHandler.Drop(rkey);
		}

		public object GetValue(RegistryKey rkey, string name, object default_value, RegistryValueOptions options)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				return default_value;
			}
			if (keyHandler.ValueExists(name))
			{
				return keyHandler.GetValue(name, options);
			}
			return default_value;
		}

		public void SetValue(RegistryKey rkey, string name, object value)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			keyHandler.SetValue(name, value);
		}

		public void SetValue(RegistryKey rkey, string name, object value, RegistryValueKind valueKind)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			keyHandler.SetValue(name, value, valueKind);
		}

		public int SubKeyCount(RegistryKey rkey)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			return Directory.GetDirectories(keyHandler.Dir).Length;
		}

		public int ValueCount(RegistryKey rkey)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			return keyHandler.ValueCount;
		}

		public void DeleteValue(RegistryKey rkey, string name, bool throw_if_missing)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				return;
			}
			if (throw_if_missing && !keyHandler.ValueExists(name))
			{
				throw new ArgumentException("the given value does not exist");
			}
			keyHandler.RemoveValue(name);
		}

		public void DeleteKey(RegistryKey rkey, string keyname, bool throw_if_missing)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler != null)
			{
				string text = Path.Combine(keyHandler.Dir, UnixRegistryApi.ToUnix(keyname));
				if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
					KeyHandler.Drop(text);
				}
				else if (throw_if_missing)
				{
					throw new ArgumentException("the given value does not exist");
				}
				return;
			}
			if (!throw_if_missing)
			{
				return;
			}
			throw new ArgumentException("the given value does not exist");
		}

		public string[] GetSubKeyNames(RegistryKey rkey)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			DirectoryInfo directoryInfo = new DirectoryInfo(keyHandler.Dir);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			string[] array = new string[directories.Length];
			for (int i = 0; i < directories.Length; i++)
			{
				DirectoryInfo directoryInfo2 = directories[i];
				array[i] = directoryInfo2.Name;
			}
			return array;
		}

		public string[] GetValueNames(RegistryKey rkey)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			return keyHandler.GetValueNames();
		}

		public string ToString(RegistryKey rkey)
		{
			return rkey.Name;
		}

		private RegistryKey CreateSubKey(RegistryKey rkey, string keyname, bool writable)
		{
			KeyHandler keyHandler = KeyHandler.Lookup(rkey, true);
			if (keyHandler == null)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			return keyHandler.Ensure(rkey, UnixRegistryApi.ToUnix(keyname), writable);
		}
	}
}
