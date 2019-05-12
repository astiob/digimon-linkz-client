using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal class ObjectWriter
	{
		private ObjectIDGenerator _idGenerator = new ObjectIDGenerator();

		private Hashtable _cachedMetadata = new Hashtable();

		private Queue _pendingObjects = new Queue();

		private Hashtable _assemblyCache = new Hashtable();

		private static Hashtable _cachedTypes = new Hashtable();

		internal static Assembly CorlibAssembly = typeof(string).Assembly;

		internal static string CorlibAssemblyName = typeof(string).Assembly.FullName;

		private ISurrogateSelector _surrogateSelector;

		private StreamingContext _context;

		private FormatterAssemblyStyle _assemblyFormat;

		private FormatterTypeStyle _typeFormat;

		private byte[] arrayBuffer;

		private int ArrayBufferLength = 4096;

		private SerializationObjectManager _manager;

		public ObjectWriter(ISurrogateSelector surrogateSelector, StreamingContext context, FormatterAssemblyStyle assemblyFormat, FormatterTypeStyle typeFormat)
		{
			this._surrogateSelector = surrogateSelector;
			this._context = context;
			this._assemblyFormat = assemblyFormat;
			this._typeFormat = typeFormat;
			this._manager = new SerializationObjectManager(context);
		}

		public void WriteObjectGraph(BinaryWriter writer, object obj, Header[] headers)
		{
			this._pendingObjects.Clear();
			if (headers != null)
			{
				this.QueueObject(headers);
			}
			this.QueueObject(obj);
			this.WriteQueuedObjects(writer);
			ObjectWriter.WriteSerializationEnd(writer);
			this._manager.RaiseOnSerializedEvent();
		}

		public void QueueObject(object obj)
		{
			this._pendingObjects.Enqueue(obj);
		}

		public void WriteQueuedObjects(BinaryWriter writer)
		{
			while (this._pendingObjects.Count > 0)
			{
				this.WriteObjectInstance(writer, this._pendingObjects.Dequeue(), false);
			}
		}

		public void WriteObjectInstance(BinaryWriter writer, object obj, bool isValueObject)
		{
			long id;
			if (isValueObject)
			{
				id = this._idGenerator.NextId;
			}
			else
			{
				bool flag;
				id = this._idGenerator.GetId(obj, out flag);
			}
			if (obj is string)
			{
				this.WriteString(writer, id, (string)obj);
			}
			else if (obj is Array)
			{
				this.WriteArray(writer, id, (Array)obj);
			}
			else
			{
				this.WriteObject(writer, id, obj);
			}
		}

		public static void WriteSerializationEnd(BinaryWriter writer)
		{
			writer.Write(11);
		}

		private void WriteObject(BinaryWriter writer, long id, object obj)
		{
			TypeMetadata typeMetadata;
			object data;
			this.GetObjectData(obj, out typeMetadata, out data);
			ObjectWriter.MetadataReference metadataReference = (ObjectWriter.MetadataReference)this._cachedMetadata[typeMetadata.InstanceTypeName];
			if (metadataReference != null && typeMetadata.IsCompatible(metadataReference.Metadata))
			{
				writer.Write(1);
				writer.Write((int)id);
				writer.Write((int)metadataReference.ObjectID);
				typeMetadata.WriteObjectData(this, writer, data);
				return;
			}
			if (metadataReference == null)
			{
				metadataReference = new ObjectWriter.MetadataReference(typeMetadata, id);
				this._cachedMetadata[typeMetadata.InstanceTypeName] = metadataReference;
			}
			bool flag = typeMetadata.RequiresTypes || this._typeFormat == FormatterTypeStyle.TypesAlways;
			BinaryElement value;
			int num;
			if (typeMetadata.TypeAssemblyName == ObjectWriter.CorlibAssemblyName)
			{
				value = ((!flag) ? BinaryElement.UntypedRuntimeObject : BinaryElement.RuntimeObject);
				num = -1;
			}
			else
			{
				value = ((!flag) ? BinaryElement.UntypedExternalObject : BinaryElement.ExternalObject);
				num = this.WriteAssemblyName(writer, typeMetadata.TypeAssemblyName);
			}
			typeMetadata.WriteAssemblies(this, writer);
			writer.Write((byte)value);
			writer.Write((int)id);
			writer.Write(typeMetadata.InstanceTypeName);
			typeMetadata.WriteTypeData(this, writer, flag);
			if (num != -1)
			{
				writer.Write(num);
			}
			typeMetadata.WriteObjectData(this, writer, data);
		}

		private void GetObjectData(object obj, out TypeMetadata metadata, out object data)
		{
			Type type = obj.GetType();
			if (this._surrogateSelector != null)
			{
				ISurrogateSelector surrogateSelector;
				ISerializationSurrogate surrogate = this._surrogateSelector.GetSurrogate(type, this._context, out surrogateSelector);
				if (surrogate != null)
				{
					SerializationInfo serializationInfo = new SerializationInfo(type, new FormatterConverter());
					surrogate.GetObjectData(obj, serializationInfo, this._context);
					metadata = new SerializableTypeMetadata(type, serializationInfo);
					data = serializationInfo;
					return;
				}
			}
			BinaryCommon.CheckSerializable(type, this._surrogateSelector, this._context);
			this._manager.RegisterObject(obj);
			ISerializable serializable = obj as ISerializable;
			if (serializable != null)
			{
				SerializationInfo serializationInfo2 = new SerializationInfo(type, new FormatterConverter());
				serializable.GetObjectData(serializationInfo2, this._context);
				metadata = new SerializableTypeMetadata(type, serializationInfo2);
				data = serializationInfo2;
			}
			else
			{
				data = obj;
				if (this._context.Context != null)
				{
					metadata = new MemberTypeMetadata(type, this._context);
					return;
				}
				bool flag = false;
				Hashtable cachedTypes = ObjectWriter._cachedTypes;
				Hashtable hashtable;
				lock (cachedTypes)
				{
					hashtable = (Hashtable)ObjectWriter._cachedTypes[this._context.State];
					if (hashtable == null)
					{
						hashtable = new Hashtable();
						ObjectWriter._cachedTypes[this._context.State] = hashtable;
						flag = true;
					}
				}
				metadata = null;
				Hashtable obj2 = hashtable;
				lock (obj2)
				{
					if (!flag)
					{
						metadata = (TypeMetadata)hashtable[type];
					}
					if (metadata == null)
					{
						metadata = this.CreateMemberTypeMetadata(type);
					}
					hashtable[type] = metadata;
				}
			}
		}

		private TypeMetadata CreateMemberTypeMetadata(Type type)
		{
			if (!BinaryCommon.UseReflectionSerialization)
			{
				Type type2 = CodeGenerator.GenerateMetadataType(type, this._context);
				return (TypeMetadata)Activator.CreateInstance(type2);
			}
			return new MemberTypeMetadata(type, this._context);
		}

		private void WriteArray(BinaryWriter writer, long id, Array array)
		{
			Type elementType = array.GetType().GetElementType();
			if (elementType == typeof(object) && array.Rank == 1)
			{
				this.WriteObjectArray(writer, id, array);
			}
			else if (elementType == typeof(string) && array.Rank == 1)
			{
				this.WriteStringArray(writer, id, array);
			}
			else if (BinaryCommon.IsPrimitive(elementType) && array.Rank == 1)
			{
				this.WritePrimitiveTypeArray(writer, id, array);
			}
			else
			{
				this.WriteGenericArray(writer, id, array);
			}
		}

		private void WriteGenericArray(BinaryWriter writer, long id, Array array)
		{
			Type elementType = array.GetType().GetElementType();
			if (!elementType.IsArray)
			{
				this.WriteAssembly(writer, elementType.Assembly);
			}
			writer.Write(7);
			writer.Write((int)id);
			if (elementType.IsArray)
			{
				writer.Write(1);
			}
			else if (array.Rank == 1)
			{
				writer.Write(0);
			}
			else
			{
				writer.Write(2);
			}
			writer.Write(array.Rank);
			for (int i = 0; i < array.Rank; i++)
			{
				writer.Write(array.GetUpperBound(i) + 1);
			}
			ObjectWriter.WriteTypeCode(writer, elementType);
			this.WriteTypeSpec(writer, elementType);
			if (array.Rank == 1 && !elementType.IsValueType)
			{
				this.WriteSingleDimensionArrayElements(writer, array, elementType);
			}
			else
			{
				foreach (object val in array)
				{
					this.WriteValue(writer, elementType, val);
				}
			}
		}

		private void WriteObjectArray(BinaryWriter writer, long id, Array array)
		{
			writer.Write(16);
			writer.Write((int)id);
			writer.Write(array.Length);
			this.WriteSingleDimensionArrayElements(writer, array, typeof(object));
		}

		private void WriteStringArray(BinaryWriter writer, long id, Array array)
		{
			writer.Write(17);
			writer.Write((int)id);
			writer.Write(array.Length);
			this.WriteSingleDimensionArrayElements(writer, array, typeof(string));
		}

		private void WritePrimitiveTypeArray(BinaryWriter writer, long id, Array array)
		{
			writer.Write(15);
			writer.Write((int)id);
			writer.Write(array.Length);
			Type elementType = array.GetType().GetElementType();
			this.WriteTypeSpec(writer, elementType);
			switch (Type.GetTypeCode(elementType))
			{
			case TypeCode.Boolean:
				foreach (bool value in (bool[])array)
				{
					writer.Write(value);
				}
				return;
			case TypeCode.Char:
				writer.Write((char[])array);
				return;
			case TypeCode.SByte:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 1);
				}
				else
				{
					foreach (sbyte value2 in (sbyte[])array)
					{
						writer.Write(value2);
					}
				}
				return;
			case TypeCode.Byte:
				writer.Write((byte[])array);
				return;
			case TypeCode.Int16:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 2);
				}
				else
				{
					foreach (short value3 in (short[])array)
					{
						writer.Write(value3);
					}
				}
				return;
			case TypeCode.UInt16:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 2);
				}
				else
				{
					foreach (ushort value4 in (ushort[])array)
					{
						writer.Write(value4);
					}
				}
				return;
			case TypeCode.Int32:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 4);
				}
				else
				{
					foreach (int value5 in (int[])array)
					{
						writer.Write(value5);
					}
				}
				return;
			case TypeCode.UInt32:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 4);
				}
				else
				{
					foreach (uint value6 in (uint[])array)
					{
						writer.Write(value6);
					}
				}
				return;
			case TypeCode.Int64:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 8);
				}
				else
				{
					foreach (long value7 in (long[])array)
					{
						writer.Write(value7);
					}
				}
				return;
			case TypeCode.UInt64:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 8);
				}
				else
				{
					foreach (ulong value8 in (ulong[])array)
					{
						writer.Write(value8);
					}
				}
				return;
			case TypeCode.Single:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 4);
				}
				else
				{
					foreach (float value9 in (float[])array)
					{
						writer.Write(value9);
					}
				}
				return;
			case TypeCode.Double:
				if (array.Length > 2)
				{
					this.BlockWrite(writer, array, 8);
				}
				else
				{
					foreach (double value10 in (double[])array)
					{
						writer.Write(value10);
					}
				}
				return;
			case TypeCode.Decimal:
				foreach (decimal value11 in (decimal[])array)
				{
					writer.Write(value11);
				}
				return;
			case TypeCode.DateTime:
				foreach (DateTime dateTime in (DateTime[])array)
				{
					writer.Write(dateTime.ToBinary());
				}
				return;
			case TypeCode.String:
				foreach (string value12 in (string[])array)
				{
					writer.Write(value12);
				}
				return;
			}
			if (elementType != typeof(TimeSpan))
			{
				throw new NotSupportedException("Unsupported primitive type: " + elementType.FullName);
			}
			foreach (TimeSpan timeSpan in (TimeSpan[])array)
			{
				writer.Write(timeSpan.Ticks);
			}
		}

		private void BlockWrite(BinaryWriter writer, Array array, int dataSize)
		{
			int i = Buffer.ByteLength(array);
			if (this.arrayBuffer == null || (i > this.arrayBuffer.Length && this.arrayBuffer.Length != this.ArrayBufferLength))
			{
				this.arrayBuffer = new byte[(i > this.ArrayBufferLength) ? this.ArrayBufferLength : i];
			}
			int num = 0;
			while (i > 0)
			{
				int num2 = (i >= this.arrayBuffer.Length) ? this.arrayBuffer.Length : i;
				Buffer.BlockCopy(array, num, this.arrayBuffer, 0, num2);
				if (!BitConverter.IsLittleEndian && dataSize > 1)
				{
					BinaryCommon.SwapBytes(this.arrayBuffer, num2, dataSize);
				}
				writer.Write(this.arrayBuffer, 0, num2);
				i -= num2;
				num += num2;
			}
		}

		private void WriteSingleDimensionArrayElements(BinaryWriter writer, Array array, Type elementType)
		{
			int num = 0;
			foreach (object obj in array)
			{
				if (obj != null && num > 0)
				{
					this.WriteNullFiller(writer, num);
					this.WriteValue(writer, elementType, obj);
					num = 0;
				}
				else if (obj == null)
				{
					num++;
				}
				else
				{
					this.WriteValue(writer, elementType, obj);
				}
			}
			if (num > 0)
			{
				this.WriteNullFiller(writer, num);
			}
		}

		private void WriteNullFiller(BinaryWriter writer, int numNulls)
		{
			if (numNulls == 1)
			{
				writer.Write(10);
			}
			else if (numNulls == 2)
			{
				writer.Write(10);
				writer.Write(10);
			}
			else if (numNulls <= 255)
			{
				writer.Write(13);
				writer.Write((byte)numNulls);
			}
			else
			{
				writer.Write(14);
				writer.Write(numNulls);
			}
		}

		private void WriteObjectReference(BinaryWriter writer, long id)
		{
			writer.Write(9);
			writer.Write((int)id);
		}

		public void WriteValue(BinaryWriter writer, Type valueType, object val)
		{
			if (val == null)
			{
				BinaryCommon.CheckSerializable(valueType, this._surrogateSelector, this._context);
				writer.Write(10);
			}
			else if (BinaryCommon.IsPrimitive(val.GetType()))
			{
				if (!BinaryCommon.IsPrimitive(valueType))
				{
					writer.Write(8);
					this.WriteTypeSpec(writer, val.GetType());
				}
				ObjectWriter.WritePrimitiveValue(writer, val);
			}
			else if (valueType.IsValueType)
			{
				this.WriteObjectInstance(writer, val, true);
			}
			else if (val is string)
			{
				bool flag;
				long id = this._idGenerator.GetId(val, out flag);
				if (flag)
				{
					this.WriteObjectInstance(writer, val, false);
				}
				else
				{
					this.WriteObjectReference(writer, id);
				}
			}
			else
			{
				bool flag2;
				long id2 = this._idGenerator.GetId(val, out flag2);
				if (flag2)
				{
					this._pendingObjects.Enqueue(val);
				}
				this.WriteObjectReference(writer, id2);
			}
		}

		private void WriteString(BinaryWriter writer, long id, string str)
		{
			writer.Write(6);
			writer.Write((int)id);
			writer.Write(str);
		}

		public int WriteAssembly(BinaryWriter writer, Assembly assembly)
		{
			return this.WriteAssemblyName(writer, assembly.FullName);
		}

		public int WriteAssemblyName(BinaryWriter writer, string assembly)
		{
			if (assembly == ObjectWriter.CorlibAssemblyName)
			{
				return -1;
			}
			bool flag;
			int num = this.RegisterAssembly(assembly, out flag);
			if (!flag)
			{
				return num;
			}
			writer.Write(12);
			writer.Write(num);
			if (this._assemblyFormat == FormatterAssemblyStyle.Full)
			{
				writer.Write(assembly);
			}
			else
			{
				int num2 = assembly.IndexOf(',');
				if (num2 != -1)
				{
					assembly = assembly.Substring(0, num2);
				}
				writer.Write(assembly);
			}
			return num;
		}

		public int GetAssemblyId(Assembly assembly)
		{
			return this.GetAssemblyNameId(assembly.FullName);
		}

		public int GetAssemblyNameId(string assembly)
		{
			return (int)this._assemblyCache[assembly];
		}

		private int RegisterAssembly(string assembly, out bool firstTime)
		{
			if (this._assemblyCache.ContainsKey(assembly))
			{
				firstTime = false;
				return (int)this._assemblyCache[assembly];
			}
			int num = (int)this._idGenerator.GetId(0, out firstTime);
			this._assemblyCache.Add(assembly, num);
			return num;
		}

		public static void WritePrimitiveValue(BinaryWriter writer, object value)
		{
			Type type = value.GetType();
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				writer.Write((bool)value);
				return;
			case TypeCode.Char:
				writer.Write((char)value);
				return;
			case TypeCode.SByte:
				writer.Write((sbyte)value);
				return;
			case TypeCode.Byte:
				writer.Write((byte)value);
				return;
			case TypeCode.Int16:
				writer.Write((short)value);
				return;
			case TypeCode.UInt16:
				writer.Write((ushort)value);
				return;
			case TypeCode.Int32:
				writer.Write((int)value);
				return;
			case TypeCode.UInt32:
				writer.Write((uint)value);
				return;
			case TypeCode.Int64:
				writer.Write((long)value);
				return;
			case TypeCode.UInt64:
				writer.Write((ulong)value);
				return;
			case TypeCode.Single:
				writer.Write((float)value);
				return;
			case TypeCode.Double:
				writer.Write((double)value);
				return;
			case TypeCode.Decimal:
				writer.Write(((decimal)value).ToString(CultureInfo.InvariantCulture));
				return;
			case TypeCode.DateTime:
				writer.Write(((DateTime)value).ToBinary());
				return;
			case TypeCode.String:
				writer.Write((string)value);
				return;
			}
			if (type != typeof(TimeSpan))
			{
				throw new NotSupportedException("Unsupported primitive type: " + value.GetType().FullName);
			}
			writer.Write(((TimeSpan)value).Ticks);
		}

		public static void WriteTypeCode(BinaryWriter writer, Type type)
		{
			writer.Write((byte)ObjectWriter.GetTypeTag(type));
		}

		public static TypeTag GetTypeTag(Type type)
		{
			if (type == typeof(string))
			{
				return TypeTag.String;
			}
			if (BinaryCommon.IsPrimitive(type))
			{
				return TypeTag.PrimitiveType;
			}
			if (type == typeof(object))
			{
				return TypeTag.ObjectType;
			}
			if (type.IsArray && type.GetArrayRank() == 1 && type.GetElementType() == typeof(object))
			{
				return TypeTag.ArrayOfObject;
			}
			if (type.IsArray && type.GetArrayRank() == 1 && type.GetElementType() == typeof(string))
			{
				return TypeTag.ArrayOfString;
			}
			if (type.IsArray && type.GetArrayRank() == 1 && BinaryCommon.IsPrimitive(type.GetElementType()))
			{
				return TypeTag.ArrayOfPrimitiveType;
			}
			if (type.Assembly == ObjectWriter.CorlibAssembly)
			{
				return TypeTag.RuntimeType;
			}
			return TypeTag.GenericType;
		}

		public void WriteTypeSpec(BinaryWriter writer, Type type)
		{
			switch (ObjectWriter.GetTypeTag(type))
			{
			case TypeTag.PrimitiveType:
				writer.Write(BinaryCommon.GetTypeCode(type));
				break;
			case TypeTag.RuntimeType:
			{
				string value = type.FullName;
				if (this._context.State == StreamingContextStates.Remoting)
				{
					if (type == typeof(MonoType))
					{
						value = "System.RuntimeType";
					}
					else if (type == typeof(MonoType[]))
					{
						value = "System.RuntimeType[]";
					}
				}
				writer.Write(value);
				break;
			}
			case TypeTag.GenericType:
				writer.Write(type.FullName);
				writer.Write(this.GetAssemblyId(type.Assembly));
				break;
			case TypeTag.ArrayOfPrimitiveType:
				writer.Write(BinaryCommon.GetTypeCode(type.GetElementType()));
				break;
			}
		}

		private class MetadataReference
		{
			public TypeMetadata Metadata;

			public long ObjectID;

			public MetadataReference(TypeMetadata metadata, long id)
			{
				this.Metadata = metadata;
				this.ObjectID = id;
			}
		}
	}
}
