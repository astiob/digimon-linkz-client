using System;
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal class SerializableTypeMetadata : TypeMetadata
	{
		private Type[] types;

		private string[] names;

		public SerializableTypeMetadata(Type itype, SerializationInfo info)
		{
			this.types = new Type[info.MemberCount];
			this.names = new string[info.MemberCount];
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				this.types[num] = enumerator.ObjectType;
				this.names[num] = enumerator.Name;
				num++;
			}
			this.TypeAssemblyName = info.AssemblyName;
			this.InstanceTypeName = info.FullTypeName;
		}

		public override bool IsCompatible(TypeMetadata other)
		{
			if (!(other is SerializableTypeMetadata))
			{
				return false;
			}
			SerializableTypeMetadata serializableTypeMetadata = (SerializableTypeMetadata)other;
			if (this.types.Length != serializableTypeMetadata.types.Length)
			{
				return false;
			}
			if (this.TypeAssemblyName != serializableTypeMetadata.TypeAssemblyName)
			{
				return false;
			}
			if (this.InstanceTypeName != serializableTypeMetadata.InstanceTypeName)
			{
				return false;
			}
			for (int i = 0; i < this.types.Length; i++)
			{
				if (this.types[i] != serializableTypeMetadata.types[i])
				{
					return false;
				}
				if (this.names[i] != serializableTypeMetadata.names[i])
				{
					return false;
				}
			}
			return true;
		}

		public override void WriteAssemblies(ObjectWriter ow, BinaryWriter writer)
		{
			foreach (Type type in this.types)
			{
				Type type2 = type;
				while (type2.IsArray)
				{
					type2 = type2.GetElementType();
				}
				ow.WriteAssembly(writer, type2.Assembly);
			}
		}

		public override void WriteTypeData(ObjectWriter ow, BinaryWriter writer, bool writeTypes)
		{
			writer.Write(this.types.Length);
			foreach (string value in this.names)
			{
				writer.Write(value);
			}
			foreach (Type type in this.types)
			{
				ObjectWriter.WriteTypeCode(writer, type);
			}
			foreach (Type type2 in this.types)
			{
				ow.WriteTypeSpec(writer, type2);
			}
		}

		public override void WriteObjectData(ObjectWriter ow, BinaryWriter writer, object data)
		{
			SerializationInfo serializationInfo = (SerializationInfo)data;
			SerializationInfoEnumerator enumerator = serializationInfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ow.WriteValue(writer, enumerator.ObjectType, enumerator.Value);
			}
		}

		public override bool RequiresTypes
		{
			get
			{
				return true;
			}
		}
	}
}
