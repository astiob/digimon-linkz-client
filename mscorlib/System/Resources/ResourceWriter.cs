using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace System.Resources
{
	/// <summary>Writes resources in the system-default format to an output file or an output stream. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class ResourceWriter : IDisposable, IResourceWriter
	{
		private SortedList resources = new SortedList(StringComparer.OrdinalIgnoreCase);

		private Stream stream;

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResourceWriter" /> class that writes the resources to the provided stream.</summary>
		/// <param name="stream">The output stream. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="stream" /> parameter is not writable. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stream" /> parameter is null. </exception>
		public ResourceWriter(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException("Stream was not writable.");
			}
			this.stream = stream;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResourceWriter" /> class that writes the resources to the specified file.</summary>
		/// <param name="fileName">The output file name. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="fileName" /> parameter is null. </exception>
		public ResourceWriter(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
		}

		/// <summary>Adds a named resource specified as a byte array to the list of resources to be written.</summary>
		/// <param name="name">The name of the resource. </param>
		/// <param name="value">Value of the resource as an 8-bit unsigned integer array. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> (or a name that varies only by capitalization) has already been added to this <see cref="T:System.Resources.ResourceWriter" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Resources.ResourceWriter" /> has been closed and its <see cref="T:System.Collections.Hashtable" /> is unavailable. </exception>
		public void AddResource(string name, byte[] value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.resources == null)
			{
				throw new InvalidOperationException("The resource writer has already been closed and cannot be edited");
			}
			if (this.resources[name] != null)
			{
				throw new ArgumentException("Resource already present: " + name);
			}
			this.resources.Add(name, value);
		}

		/// <summary>Adds a named resource specified as an object to the list of resources to be written.</summary>
		/// <param name="name">The name of the resource. </param>
		/// <param name="value">The value of the resource. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> (or a name that varies only by capitalization) has already been added to this <see cref="T:System.Resources.ResourceWriter" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Resources.ResourceWriter" /> has been closed and its <see cref="T:System.Collections.Hashtable" /> is unavailable. </exception>
		public void AddResource(string name, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.resources == null)
			{
				throw new InvalidOperationException("The resource writer has already been closed and cannot be edited");
			}
			if (this.resources[name] != null)
			{
				throw new ArgumentException("Resource already present: " + name);
			}
			this.resources.Add(name, value);
		}

		/// <summary>Adds a <see cref="T:System.String" /> resource to the list of resources to be written.</summary>
		/// <param name="name">The name of the resource. </param>
		/// <param name="value">The value of the resource. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> (or a name that varies only by capitalization) has already been added to this ResourceWriter. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Resources.ResourceWriter" /> has been closed and its <see cref="T:System.Collections.Hashtable" /> is unavailable. </exception>
		public void AddResource(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.resources == null)
			{
				throw new InvalidOperationException("The resource writer has already been closed and cannot be edited");
			}
			if (this.resources[name] != null)
			{
				throw new ArgumentException("Resource already present: " + name);
			}
			this.resources.Add(name, value);
		}

		/// <summary>Saves the resources to the output stream and then closes it.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error has occurred. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization of the object. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void Close()
		{
			this.Dispose(true);
		}

		/// <summary>Allows users to close the resource file or stream, explicitly releasing resources.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error has occurred. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization of the object. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void Dispose()
		{
			this.Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.resources != null)
				{
					this.Generate();
				}
				if (this.stream != null)
				{
					this.stream.Close();
				}
				GC.SuppressFinalize(this);
			}
			this.resources = null;
			this.stream = null;
		}

		/// <summary>Adds a unit of data as a resource to the list of resources to be written. </summary>
		/// <param name="name">A name that identifies the resource that contains the added data.</param>
		/// <param name="typeName">The type name of the added data.</param>
		/// <param name="serializedData">A byte array that contains the binary representation of the added data.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" />, <paramref name="typeName" />, or <paramref name="serializedData" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> (or a name that varies only by capitalization) has already been added to this <see cref="T:System.Resources.ResourceWriter" /> object. </exception>
		/// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Resources.ResourceWriter" /> object is not initialized. The probable cause is that the <see cref="T:System.Resources.ResourceWriter" /> object is closed.</exception>
		public void AddResourceData(string name, string typeName, byte[] serializedData)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (serializedData == null)
			{
				throw new ArgumentNullException("serializedData");
			}
			this.AddResource(name, new ResourceWriter.TypeByNameObject(typeName, serializedData));
		}

		/// <summary>Saves all resources to the output stream in the system default format.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An error occurred during serialization of the object. </exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Resources.ResourceWriter" /> has been closed and its <see cref="T:System.Collections.Hashtable" /> is unavailable. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void Generate()
		{
			if (this.resources == null)
			{
				throw new InvalidOperationException("The resource writer has already been closed and cannot be edited");
			}
			BinaryWriter binaryWriter = new BinaryWriter(this.stream, Encoding.UTF8);
			IFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence));
			binaryWriter.Write(ResourceManager.MagicNumber);
			binaryWriter.Write(ResourceManager.HeaderVersionNumber);
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter2 = new BinaryWriter(memoryStream, Encoding.UTF8);
			binaryWriter2.Write(typeof(ResourceReader).AssemblyQualifiedName);
			binaryWriter2.Write(typeof(RuntimeResourceSet).FullName);
			int num = (int)memoryStream.Length;
			binaryWriter.Write(num);
			binaryWriter.Write(memoryStream.GetBuffer(), 0, num);
			MemoryStream memoryStream2 = new MemoryStream();
			BinaryWriter binaryWriter3 = new BinaryWriter(memoryStream2, Encoding.Unicode);
			MemoryStream memoryStream3 = new MemoryStream();
			BinaryWriter binaryWriter4 = new BinaryWriter(memoryStream3, Encoding.UTF8);
			ArrayList arrayList = new ArrayList();
			int[] array = new int[this.resources.Count];
			int[] array2 = new int[this.resources.Count];
			int num2 = 0;
			IDictionaryEnumerator enumerator = this.resources.GetEnumerator();
			while (enumerator.MoveNext())
			{
				array[num2] = this.GetHash((string)enumerator.Key);
				array2[num2] = (int)binaryWriter3.BaseStream.Position;
				binaryWriter3.Write((string)enumerator.Key);
				binaryWriter3.Write((int)binaryWriter4.BaseStream.Position);
				if (enumerator.Value == null)
				{
					this.Write7BitEncodedInt(binaryWriter4, -1);
					num2++;
				}
				else
				{
					ResourceWriter.TypeByNameObject typeByNameObject = enumerator.Value as ResourceWriter.TypeByNameObject;
					Type type = (typeByNameObject == null) ? enumerator.Value.GetType() : null;
					object obj = (typeByNameObject == null) ? type : typeByNameObject.TypeName;
					switch ((type == null || type.IsEnum) ? TypeCode.Empty : Type.GetTypeCode(type))
					{
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
					case TypeCode.DateTime:
					case TypeCode.String:
						break;
					case (TypeCode)17:
						goto IL_22E;
					default:
						goto IL_22E;
					}
					IL_2A1:
					if (typeByNameObject != null)
					{
						binaryWriter4.Write(typeByNameObject.Value);
					}
					else if (type == typeof(byte))
					{
						binaryWriter4.Write(4);
						binaryWriter4.Write((byte)enumerator.Value);
					}
					else if (type == typeof(decimal))
					{
						binaryWriter4.Write(14);
						binaryWriter4.Write((decimal)enumerator.Value);
					}
					else if (type == typeof(DateTime))
					{
						binaryWriter4.Write(15);
						binaryWriter4.Write(((DateTime)enumerator.Value).Ticks);
					}
					else if (type == typeof(double))
					{
						binaryWriter4.Write(13);
						binaryWriter4.Write((double)enumerator.Value);
					}
					else if (type == typeof(short))
					{
						binaryWriter4.Write(6);
						binaryWriter4.Write((short)enumerator.Value);
					}
					else if (type == typeof(int))
					{
						binaryWriter4.Write(8);
						binaryWriter4.Write((int)enumerator.Value);
					}
					else if (type == typeof(long))
					{
						binaryWriter4.Write(10);
						binaryWriter4.Write((long)enumerator.Value);
					}
					else if (type == typeof(sbyte))
					{
						binaryWriter4.Write(5);
						binaryWriter4.Write((sbyte)enumerator.Value);
					}
					else if (type == typeof(float))
					{
						binaryWriter4.Write(12);
						binaryWriter4.Write((float)enumerator.Value);
					}
					else if (type == typeof(string))
					{
						binaryWriter4.Write(1);
						binaryWriter4.Write((string)enumerator.Value);
					}
					else if (type == typeof(TimeSpan))
					{
						binaryWriter4.Write(16);
						binaryWriter4.Write(((TimeSpan)enumerator.Value).Ticks);
					}
					else if (type == typeof(ushort))
					{
						binaryWriter4.Write(7);
						binaryWriter4.Write((ushort)enumerator.Value);
					}
					else if (type == typeof(uint))
					{
						binaryWriter4.Write(9);
						binaryWriter4.Write((uint)enumerator.Value);
					}
					else if (type == typeof(ulong))
					{
						binaryWriter4.Write(11);
						binaryWriter4.Write((ulong)enumerator.Value);
					}
					else if (type == typeof(byte[]))
					{
						binaryWriter4.Write(32);
						byte[] array3 = (byte[])enumerator.Value;
						binaryWriter4.Write((uint)array3.Length);
						binaryWriter4.Write(array3, 0, array3.Length);
					}
					else if (type == typeof(MemoryStream))
					{
						binaryWriter4.Write(33);
						byte[] array4 = ((MemoryStream)enumerator.Value).ToArray();
						binaryWriter4.Write((uint)array4.Length);
						binaryWriter4.Write(array4, 0, array4.Length);
					}
					else
					{
						formatter.Serialize(binaryWriter4.BaseStream, enumerator.Value);
					}
					num2++;
					continue;
					IL_22E:
					if (type == typeof(TimeSpan))
					{
						goto IL_2A1;
					}
					if (type == typeof(MemoryStream))
					{
						goto IL_2A1;
					}
					if (type == typeof(byte[]))
					{
						goto IL_2A1;
					}
					if (!arrayList.Contains(obj))
					{
						arrayList.Add(obj);
					}
					this.Write7BitEncodedInt(binaryWriter4, 64 + arrayList.IndexOf(obj));
					goto IL_2A1;
				}
			}
			Array.Sort<int, int>(array, array2);
			binaryWriter.Write(2);
			binaryWriter.Write(this.resources.Count);
			binaryWriter.Write(arrayList.Count);
			foreach (object obj2 in arrayList)
			{
				if (obj2 is Type)
				{
					binaryWriter.Write(((Type)obj2).AssemblyQualifiedName);
				}
				else
				{
					binaryWriter.Write((string)obj2);
				}
			}
			int num3 = (int)(binaryWriter.BaseStream.Position & 7L);
			int num4 = 0;
			if (num3 != 0)
			{
				num4 = 8 - num3;
			}
			for (int i = 0; i < num4; i++)
			{
				binaryWriter.Write((byte)"PAD"[i % 3]);
			}
			for (int j = 0; j < this.resources.Count; j++)
			{
				binaryWriter.Write(array[j]);
			}
			for (int k = 0; k < this.resources.Count; k++)
			{
				binaryWriter.Write(array2[k]);
			}
			int value = (int)binaryWriter.BaseStream.Position + (int)memoryStream2.Length + 4;
			binaryWriter.Write(value);
			binaryWriter.Write(memoryStream2.GetBuffer(), 0, (int)memoryStream2.Length);
			binaryWriter.Write(memoryStream3.GetBuffer(), 0, (int)memoryStream3.Length);
			binaryWriter3.Close();
			binaryWriter4.Close();
			binaryWriter.Flush();
			this.resources = null;
		}

		private int GetHash(string name)
		{
			uint num = 5381u;
			for (int i = 0; i < name.Length; i++)
			{
				num = ((num << 5) + num ^ (uint)name[i]);
			}
			return (int)num;
		}

		private void Write7BitEncodedInt(BinaryWriter writer, int value)
		{
			do
			{
				int num = value >> 7 & 33554431;
				byte b = (byte)(value & 127);
				if (num != 0)
				{
					b |= 128;
				}
				writer.Write(b);
				value = num;
			}
			while (value != 0);
		}

		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		private class TypeByNameObject
		{
			public readonly string TypeName;

			public readonly byte[] Value;

			public TypeByNameObject(string typeName, byte[] value)
			{
				this.TypeName = typeName;
				this.Value = (byte[])value.Clone();
			}
		}
	}
}
