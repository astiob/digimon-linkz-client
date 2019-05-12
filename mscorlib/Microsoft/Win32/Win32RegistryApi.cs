using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Microsoft.Win32
{
	internal class Win32RegistryApi : IRegistryApi
	{
		private const int OpenRegKeyRead = 131097;

		private const int OpenRegKeyWrite = 131078;

		private const int Int32ByteSize = 4;

		private const int BufferMaxLength = 1024;

		private readonly int NativeBytesPerCharacter = Marshal.SystemDefaultCharSize;

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegCreateKey(IntPtr keyBase, string keyName, out IntPtr keyHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegCloseKey(IntPtr keyHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegConnectRegistry(string machineName, IntPtr hKey, out IntPtr keyHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegFlushKey(IntPtr keyHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegOpenKeyEx(IntPtr keyBase, string keyName, IntPtr reserved, int access, out IntPtr keyHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegDeleteKey(IntPtr keyHandle, string valueName);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegDeleteValue(IntPtr keyHandle, string valueName);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegEnumKey(IntPtr keyBase, int index, StringBuilder nameBuffer, int bufferLength);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegEnumValue(IntPtr keyBase, int index, StringBuilder nameBuffer, ref int nameLength, IntPtr reserved, ref RegistryValueKind type, IntPtr data, IntPtr dataLength);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegSetValueEx(IntPtr keyBase, string valueName, IntPtr reserved, RegistryValueKind type, string data, int rawDataLength);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegSetValueEx(IntPtr keyBase, string valueName, IntPtr reserved, RegistryValueKind type, byte[] rawData, int rawDataLength);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegSetValueEx(IntPtr keyBase, string valueName, IntPtr reserved, RegistryValueKind type, ref int data, int rawDataLength);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegQueryValueEx(IntPtr keyBase, string valueName, IntPtr reserved, ref RegistryValueKind type, IntPtr zero, ref int dataSize);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegQueryValueEx(IntPtr keyBase, string valueName, IntPtr reserved, ref RegistryValueKind type, [Out] byte[] data, ref int dataSize);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegQueryValueEx(IntPtr keyBase, string valueName, IntPtr reserved, ref RegistryValueKind type, ref int data, ref int dataSize);

		private static IntPtr GetHandle(RegistryKey key)
		{
			return (IntPtr)key.Handle;
		}

		private static bool IsHandleValid(RegistryKey key)
		{
			return key.Handle != null;
		}

		public object GetValue(RegistryKey rkey, string name, object defaultValue, RegistryValueOptions options)
		{
			RegistryValueKind registryValueKind = RegistryValueKind.Unknown;
			int size = 0;
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int num = Win32RegistryApi.RegQueryValueEx(handle, name, IntPtr.Zero, ref registryValueKind, IntPtr.Zero, ref size);
			if (num == 2 || num == 1018)
			{
				return defaultValue;
			}
			if (num != 234 && num != 0)
			{
				this.GenerateException(num);
			}
			object obj;
			if (registryValueKind == RegistryValueKind.String)
			{
				byte[] data;
				num = this.GetBinaryValue(rkey, name, registryValueKind, out data, size);
				obj = RegistryKey.DecodeString(data);
			}
			else if (registryValueKind == RegistryValueKind.ExpandString)
			{
				byte[] data2;
				num = this.GetBinaryValue(rkey, name, registryValueKind, out data2, size);
				obj = RegistryKey.DecodeString(data2);
				if ((options & RegistryValueOptions.DoNotExpandEnvironmentNames) == RegistryValueOptions.None)
				{
					obj = Environment.ExpandEnvironmentVariables((string)obj);
				}
			}
			else if (registryValueKind == RegistryValueKind.DWord)
			{
				int num2 = 0;
				num = Win32RegistryApi.RegQueryValueEx(handle, name, IntPtr.Zero, ref registryValueKind, ref num2, ref size);
				obj = num2;
			}
			else if (registryValueKind == RegistryValueKind.Binary)
			{
				byte[] array;
				num = this.GetBinaryValue(rkey, name, registryValueKind, out array, size);
				obj = array;
			}
			else
			{
				if (registryValueKind != RegistryValueKind.MultiString)
				{
					throw new SystemException();
				}
				obj = null;
				byte[] data3;
				num = this.GetBinaryValue(rkey, name, registryValueKind, out data3, size);
				if (num == 0)
				{
					obj = RegistryKey.DecodeString(data3).Split(new char[1]);
				}
			}
			if (num != 0)
			{
				this.GenerateException(num);
			}
			return obj;
		}

		public void SetValue(RegistryKey rkey, string name, object value, RegistryValueKind valueKind)
		{
			Type type = value.GetType();
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int num2;
			if (valueKind == RegistryValueKind.DWord && type == typeof(int))
			{
				int num = (int)value;
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, RegistryValueKind.DWord, ref num, 4);
			}
			else if (valueKind == RegistryValueKind.Binary && type == typeof(byte[]))
			{
				byte[] array = (byte[])value;
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, RegistryValueKind.Binary, array, array.Length);
			}
			else if (valueKind == RegistryValueKind.MultiString && type == typeof(string[]))
			{
				string[] array2 = (string[])value;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value2 in array2)
				{
					stringBuilder.Append(value2);
					stringBuilder.Append('\0');
				}
				stringBuilder.Append('\0');
				byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, RegistryValueKind.MultiString, bytes, bytes.Length);
			}
			else if ((valueKind == RegistryValueKind.String || valueKind == RegistryValueKind.ExpandString) && type == typeof(string))
			{
				string text = string.Format("{0}{1}", value, '\0');
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, valueKind, text, text.Length * this.NativeBytesPerCharacter);
			}
			else
			{
				if (type.IsArray)
				{
					throw new ArgumentException("Only string and byte arrays can written as registry values");
				}
				throw new ArgumentException("Type does not match the valueKind");
			}
			if (num2 != 0)
			{
				this.GenerateException(num2);
			}
		}

		public void SetValue(RegistryKey rkey, string name, object value)
		{
			Type type = value.GetType();
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int num2;
			if (type == typeof(int))
			{
				int num = (int)value;
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, RegistryValueKind.DWord, ref num, 4);
			}
			else if (type == typeof(byte[]))
			{
				byte[] array = (byte[])value;
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, RegistryValueKind.Binary, array, array.Length);
			}
			else if (type == typeof(string[]))
			{
				string[] array2 = (string[])value;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value2 in array2)
				{
					stringBuilder.Append(value2);
					stringBuilder.Append('\0');
				}
				stringBuilder.Append('\0');
				byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, RegistryValueKind.MultiString, bytes, bytes.Length);
			}
			else
			{
				if (type.IsArray)
				{
					throw new ArgumentException("Only string and byte arrays can written as registry values");
				}
				string text = string.Format("{0}{1}", value, '\0');
				num2 = Win32RegistryApi.RegSetValueEx(handle, name, IntPtr.Zero, RegistryValueKind.String, text, text.Length * this.NativeBytesPerCharacter);
			}
			if (num2 == 1018)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			if (num2 != 0)
			{
				this.GenerateException(num2);
			}
		}

		private int GetBinaryValue(RegistryKey rkey, string name, RegistryValueKind type, out byte[] data, int size)
		{
			byte[] array = new byte[size];
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int result = Win32RegistryApi.RegQueryValueEx(handle, name, IntPtr.Zero, ref type, array, ref size);
			data = array;
			return result;
		}

		public int SubKeyCount(RegistryKey rkey)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int num = 0;
			for (;;)
			{
				int num2 = Win32RegistryApi.RegEnumKey(handle, num, stringBuilder, stringBuilder.Capacity);
				if (num2 == 1018)
				{
					break;
				}
				if (num2 != 0)
				{
					if (num2 == 259)
					{
						return num;
					}
					this.GenerateException(num2);
				}
				num++;
			}
			throw RegistryKey.CreateMarkedForDeletionException();
		}

		public int ValueCount(RegistryKey rkey)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int num = 0;
			for (;;)
			{
				RegistryValueKind registryValueKind = RegistryValueKind.Unknown;
				int capacity = stringBuilder.Capacity;
				int num2 = Win32RegistryApi.RegEnumValue(handle, num, stringBuilder, ref capacity, IntPtr.Zero, ref registryValueKind, IntPtr.Zero, IntPtr.Zero);
				if (num2 == 1018)
				{
					break;
				}
				if (num2 != 0 && num2 != 234)
				{
					if (num2 == 259)
					{
						return num;
					}
					this.GenerateException(num2);
				}
				num++;
			}
			throw RegistryKey.CreateMarkedForDeletionException();
		}

		public RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
		{
			IntPtr hKey2 = new IntPtr((int)hKey);
			IntPtr keyHandle;
			int num = Win32RegistryApi.RegConnectRegistry(machineName, hKey2, out keyHandle);
			if (num != 0)
			{
				this.GenerateException(num);
			}
			return new RegistryKey(hKey, keyHandle, true);
		}

		public RegistryKey OpenSubKey(RegistryKey rkey, string keyName, bool writable)
		{
			int num = 131097;
			if (writable)
			{
				num |= 131078;
			}
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			IntPtr intPtr;
			int num2 = Win32RegistryApi.RegOpenKeyEx(handle, keyName, IntPtr.Zero, num, out intPtr);
			if (num2 == 2 || num2 == 1018)
			{
				return null;
			}
			if (num2 != 0)
			{
				this.GenerateException(num2);
			}
			return new RegistryKey(intPtr, Win32RegistryApi.CombineName(rkey, keyName), writable);
		}

		public void Flush(RegistryKey rkey)
		{
			if (!Win32RegistryApi.IsHandleValid(rkey))
			{
				return;
			}
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			Win32RegistryApi.RegFlushKey(handle);
		}

		public void Close(RegistryKey rkey)
		{
			if (!Win32RegistryApi.IsHandleValid(rkey))
			{
				return;
			}
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			Win32RegistryApi.RegCloseKey(handle);
		}

		public RegistryKey CreateSubKey(RegistryKey rkey, string keyName)
		{
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			IntPtr intPtr;
			int num = Win32RegistryApi.RegCreateKey(handle, keyName, out intPtr);
			if (num == 1018)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
			if (num != 0)
			{
				this.GenerateException(num);
			}
			return new RegistryKey(intPtr, Win32RegistryApi.CombineName(rkey, keyName), true);
		}

		public void DeleteKey(RegistryKey rkey, string keyName, bool shouldThrowWhenKeyMissing)
		{
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int num = Win32RegistryApi.RegDeleteKey(handle, keyName);
			if (num != 2)
			{
				if (num != 0)
				{
					this.GenerateException(num);
				}
				return;
			}
			if (shouldThrowWhenKeyMissing)
			{
				throw new ArgumentException("key " + keyName);
			}
		}

		public void DeleteValue(RegistryKey rkey, string value, bool shouldThrowWhenKeyMissing)
		{
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			int num = Win32RegistryApi.RegDeleteValue(handle, value);
			if (num == 1018)
			{
				return;
			}
			if (num != 2)
			{
				if (num != 0)
				{
					this.GenerateException(num);
				}
				return;
			}
			if (shouldThrowWhenKeyMissing)
			{
				throw new ArgumentException("value " + value);
			}
		}

		public string[] GetSubKeyNames(RegistryKey rkey)
		{
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			StringBuilder stringBuilder = new StringBuilder(1024);
			ArrayList arrayList = new ArrayList();
			int num = 0;
			for (;;)
			{
				int num2 = Win32RegistryApi.RegEnumKey(handle, num, stringBuilder, stringBuilder.Capacity);
				if (num2 == 0)
				{
					arrayList.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				else
				{
					if (num2 == 259)
					{
						break;
					}
					this.GenerateException(num2);
				}
				num++;
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		public string[] GetValueNames(RegistryKey rkey)
		{
			IntPtr handle = Win32RegistryApi.GetHandle(rkey);
			ArrayList arrayList = new ArrayList();
			int num = 0;
			for (;;)
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				int capacity = stringBuilder.Capacity;
				RegistryValueKind registryValueKind = RegistryValueKind.Unknown;
				int num2 = Win32RegistryApi.RegEnumValue(handle, num, stringBuilder, ref capacity, IntPtr.Zero, ref registryValueKind, IntPtr.Zero, IntPtr.Zero);
				if (num2 == 0 || num2 == 234)
				{
					arrayList.Add(stringBuilder.ToString());
				}
				else
				{
					if (num2 == 259)
					{
						break;
					}
					if (num2 == 1018)
					{
						goto Block_3;
					}
					this.GenerateException(num2);
				}
				num++;
			}
			return (string[])arrayList.ToArray(typeof(string));
			Block_3:
			throw RegistryKey.CreateMarkedForDeletionException();
		}

		private void GenerateException(int errorCode)
		{
			switch (errorCode)
			{
			case 2:
				break;
			default:
				if (errorCode == 53)
				{
					throw new IOException("The network path was not found.");
				}
				if (errorCode != 87)
				{
					throw new SystemException();
				}
				break;
			case 5:
				throw new SecurityException();
			}
			throw new ArgumentException();
		}

		public string ToString(RegistryKey rkey)
		{
			return rkey.Name;
		}

		internal static string CombineName(RegistryKey rkey, string localName)
		{
			return rkey.Name + "\\" + localName;
		}
	}
}
