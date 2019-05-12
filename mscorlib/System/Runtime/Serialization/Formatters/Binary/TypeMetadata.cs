using System;
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal abstract class TypeMetadata
	{
		public string TypeAssemblyName;

		public string InstanceTypeName;

		public abstract void WriteAssemblies(ObjectWriter ow, BinaryWriter writer);

		public abstract void WriteTypeData(ObjectWriter ow, BinaryWriter writer, bool writeTypes);

		public abstract void WriteObjectData(ObjectWriter ow, BinaryWriter writer, object data);

		public virtual bool IsCompatible(TypeMetadata other)
		{
			return true;
		}

		public abstract bool RequiresTypes { get; }
	}
}
