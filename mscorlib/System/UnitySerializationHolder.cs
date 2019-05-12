using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace System
{
	[Serializable]
	internal class UnitySerializationHolder : ISerializable, IObjectReference
	{
		private string _data;

		private UnitySerializationHolder.UnityType _unityType;

		private string _assemblyName;

		private UnitySerializationHolder(SerializationInfo info, StreamingContext ctx)
		{
			this._data = info.GetString("Data");
			this._unityType = (UnitySerializationHolder.UnityType)info.GetInt32("UnityType");
			this._assemblyName = info.GetString("AssemblyName");
		}

		public static void GetTypeData(Type instance, SerializationInfo info, StreamingContext ctx)
		{
			info.AddValue("Data", instance.FullName);
			info.AddValue("UnityType", 4);
			info.AddValue("AssemblyName", instance.Assembly.FullName);
			info.SetType(typeof(UnitySerializationHolder));
		}

		public static void GetDBNullData(DBNull instance, SerializationInfo info, StreamingContext ctx)
		{
			info.AddValue("Data", null);
			info.AddValue("UnityType", 2);
			info.AddValue("AssemblyName", instance.GetType().Assembly.FullName);
			info.SetType(typeof(UnitySerializationHolder));
		}

		public static void GetAssemblyData(Assembly instance, SerializationInfo info, StreamingContext ctx)
		{
			info.AddValue("Data", instance.FullName);
			info.AddValue("UnityType", 6);
			info.AddValue("AssemblyName", instance.FullName);
			info.SetType(typeof(UnitySerializationHolder));
		}

		public static void GetModuleData(Module instance, SerializationInfo info, StreamingContext ctx)
		{
			info.AddValue("Data", instance.ScopeName);
			info.AddValue("UnityType", 5);
			info.AddValue("AssemblyName", instance.Assembly.FullName);
			info.SetType(typeof(UnitySerializationHolder));
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException();
		}

		public virtual object GetRealObject(StreamingContext context)
		{
			switch (this._unityType)
			{
			case UnitySerializationHolder.UnityType.DBNull:
				return DBNull.Value;
			case UnitySerializationHolder.UnityType.Type:
			{
				Assembly assembly = Assembly.Load(this._assemblyName);
				return assembly.GetType(this._data);
			}
			case UnitySerializationHolder.UnityType.Module:
			{
				Assembly assembly2 = Assembly.Load(this._assemblyName);
				return assembly2.GetModule(this._data);
			}
			case UnitySerializationHolder.UnityType.Assembly:
				return Assembly.Load(this._data);
			}
			throw new NotSupportedException(Locale.GetText("UnitySerializationHolder does not support this type."));
		}

		private enum UnityType : byte
		{
			DBNull = 2,
			Type = 4,
			Module,
			Assembly
		}
	}
}
