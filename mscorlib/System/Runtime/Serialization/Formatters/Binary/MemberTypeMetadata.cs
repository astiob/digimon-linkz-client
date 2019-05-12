using System;
using System.IO;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal class MemberTypeMetadata : ClrTypeMetadata
	{
		private MemberInfo[] members;

		public MemberTypeMetadata(Type type, StreamingContext context) : base(type)
		{
			this.members = FormatterServices.GetSerializableMembers(type, context);
		}

		public override void WriteAssemblies(ObjectWriter ow, BinaryWriter writer)
		{
			foreach (FieldInfo fieldInfo in this.members)
			{
				Type type = fieldInfo.FieldType;
				while (type.IsArray)
				{
					type = type.GetElementType();
				}
				ow.WriteAssembly(writer, type.Assembly);
			}
		}

		public override void WriteTypeData(ObjectWriter ow, BinaryWriter writer, bool writeTypes)
		{
			writer.Write(this.members.Length);
			foreach (FieldInfo fieldInfo in this.members)
			{
				writer.Write(fieldInfo.Name);
			}
			if (writeTypes)
			{
				foreach (FieldInfo fieldInfo2 in this.members)
				{
					ObjectWriter.WriteTypeCode(writer, fieldInfo2.FieldType);
				}
				foreach (FieldInfo fieldInfo3 in this.members)
				{
					ow.WriteTypeSpec(writer, fieldInfo3.FieldType);
				}
			}
		}

		public override void WriteObjectData(ObjectWriter ow, BinaryWriter writer, object data)
		{
			object[] objectData = FormatterServices.GetObjectData(data, this.members);
			for (int i = 0; i < objectData.Length; i++)
			{
				ow.WriteValue(writer, ((FieldInfo)this.members[i]).FieldType, objectData[i]);
			}
		}
	}
}
