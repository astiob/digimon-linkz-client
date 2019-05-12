using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	[Serializable]
	internal class MemberInfoSerializationHolder : ISerializable, IObjectReference
	{
		private const BindingFlags DefaultBinding = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		private readonly string _memberName;

		private readonly string _memberSignature;

		private readonly MemberTypes _memberType;

		private readonly Type _reflectedType;

		private readonly Type[] _genericArguments;

		private MemberInfoSerializationHolder(SerializationInfo info, StreamingContext ctx)
		{
			string @string = info.GetString("AssemblyName");
			string string2 = info.GetString("ClassName");
			this._memberName = info.GetString("Name");
			this._memberSignature = info.GetString("Signature");
			this._memberType = (MemberTypes)info.GetInt32("MemberType");
			try
			{
				this._genericArguments = null;
			}
			catch (SerializationException)
			{
			}
			Assembly assembly = Assembly.Load(@string);
			this._reflectedType = assembly.GetType(string2, true, true);
		}

		public static void Serialize(SerializationInfo info, string name, Type klass, string signature, MemberTypes type)
		{
			MemberInfoSerializationHolder.Serialize(info, name, klass, signature, type, null);
		}

		public static void Serialize(SerializationInfo info, string name, Type klass, string signature, MemberTypes type, Type[] genericArguments)
		{
			info.SetType(typeof(MemberInfoSerializationHolder));
			info.AddValue("AssemblyName", klass.Module.Assembly.FullName, typeof(string));
			info.AddValue("ClassName", klass.FullName, typeof(string));
			info.AddValue("Name", name, typeof(string));
			info.AddValue("Signature", signature, typeof(string));
			info.AddValue("MemberType", (int)type);
			info.AddValue("GenericArguments", genericArguments, typeof(Type[]));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException();
		}

		public object GetRealObject(StreamingContext context)
		{
			MemberTypes memberType = this._memberType;
			switch (memberType)
			{
			case MemberTypes.Constructor:
			{
				ConstructorInfo[] constructors = this._reflectedType.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < constructors.Length; i++)
				{
					if (constructors[i].ToString().Equals(this._memberSignature))
					{
						return constructors[i];
					}
				}
				throw new SerializationException(string.Format("Could not find constructor '{0}' in type '{1}'", this._memberSignature, this._reflectedType));
			}
			case MemberTypes.Event:
			{
				EventInfo @event = this._reflectedType.GetEvent(this._memberName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (@event != null)
				{
					return @event;
				}
				throw new SerializationException(string.Format("Could not find event '{0}' in type '{1}'", this._memberName, this._reflectedType));
			}
			default:
			{
				if (memberType != MemberTypes.Property)
				{
					throw new SerializationException(string.Format("Unhandled MemberType {0}", this._memberType));
				}
				PropertyInfo property = this._reflectedType.GetProperty(this._memberName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					return property;
				}
				throw new SerializationException(string.Format("Could not find property '{0}' in type '{1}'", this._memberName, this._reflectedType));
			}
			case MemberTypes.Field:
			{
				FieldInfo field = this._reflectedType.GetField(this._memberName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					return field;
				}
				throw new SerializationException(string.Format("Could not find field '{0}' in type '{1}'", this._memberName, this._reflectedType));
			}
			case MemberTypes.Method:
			{
				MethodInfo[] methods = this._reflectedType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (int j = 0; j < methods.Length; j++)
				{
					if (methods[j].ToString().Equals(this._memberSignature))
					{
						return methods[j];
					}
					if (this._genericArguments != null && methods[j].IsGenericMethod && methods[j].GetGenericArguments().Length == this._genericArguments.Length)
					{
						MethodInfo methodInfo = methods[j].MakeGenericMethod(this._genericArguments);
						if (methodInfo.ToString() == this._memberSignature)
						{
							return methodInfo;
						}
					}
				}
				throw new SerializationException(string.Format("Could not find method '{0}' in type '{1}'", this._memberSignature, this._reflectedType));
			}
			}
		}
	}
}
