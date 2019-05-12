using System;
using System.Collections;
using System.Reflection;

namespace System.Runtime.Serialization
{
	internal sealed class SerializationCallbacks
	{
		private const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		private readonly ArrayList onSerializingList;

		private readonly ArrayList onSerializedList;

		private readonly ArrayList onDeserializingList;

		private readonly ArrayList onDeserializedList;

		private static Hashtable cache = new Hashtable();

		private static object cache_lock = new object();

		public SerializationCallbacks(Type type)
		{
			this.onSerializingList = SerializationCallbacks.GetMethodsByAttribute(type, typeof(OnSerializingAttribute));
			this.onSerializedList = SerializationCallbacks.GetMethodsByAttribute(type, typeof(OnSerializedAttribute));
			this.onDeserializingList = SerializationCallbacks.GetMethodsByAttribute(type, typeof(OnDeserializingAttribute));
			this.onDeserializedList = SerializationCallbacks.GetMethodsByAttribute(type, typeof(OnDeserializedAttribute));
		}

		public bool HasSerializingCallbacks
		{
			get
			{
				return this.onSerializingList != null;
			}
		}

		public bool HasSerializedCallbacks
		{
			get
			{
				return this.onSerializedList != null;
			}
		}

		public bool HasDeserializingCallbacks
		{
			get
			{
				return this.onDeserializingList != null;
			}
		}

		public bool HasDeserializedCallbacks
		{
			get
			{
				return this.onDeserializedList != null;
			}
		}

		private static ArrayList GetMethodsByAttribute(Type type, Type attr)
		{
			ArrayList arrayList = new ArrayList();
			for (Type type2 = type; type2 != typeof(object); type2 = type2.BaseType)
			{
				int num = 0;
				foreach (MethodInfo methodInfo in type2.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (methodInfo.IsDefined(attr, false))
					{
						arrayList.Add(methodInfo);
						num++;
					}
				}
				if (num > 1)
				{
					throw new TypeLoadException(string.Format("Type '{0}' has more than one method with the following attribute: '{1}'.", type.AssemblyQualifiedName, attr.FullName));
				}
			}
			return (arrayList.Count != 0) ? arrayList : null;
		}

		private static void Invoke(ArrayList list, object target, StreamingContext context)
		{
			if (list == null)
			{
				return;
			}
			SerializationCallbacks.CallbackHandler callbackHandler = null;
			foreach (object obj in list)
			{
				MethodInfo method = (MethodInfo)obj;
				callbackHandler = (SerializationCallbacks.CallbackHandler)Delegate.Combine(Delegate.CreateDelegate(typeof(SerializationCallbacks.CallbackHandler), target, method), callbackHandler);
			}
			callbackHandler(context);
		}

		public void RaiseOnSerializing(object target, StreamingContext contex)
		{
			SerializationCallbacks.Invoke(this.onSerializingList, target, contex);
		}

		public void RaiseOnSerialized(object target, StreamingContext contex)
		{
			SerializationCallbacks.Invoke(this.onSerializedList, target, contex);
		}

		public void RaiseOnDeserializing(object target, StreamingContext contex)
		{
			SerializationCallbacks.Invoke(this.onDeserializingList, target, contex);
		}

		public void RaiseOnDeserialized(object target, StreamingContext contex)
		{
			SerializationCallbacks.Invoke(this.onDeserializedList, target, contex);
		}

		public static SerializationCallbacks GetSerializationCallbacks(Type t)
		{
			SerializationCallbacks serializationCallbacks = (SerializationCallbacks)SerializationCallbacks.cache[t];
			if (serializationCallbacks != null)
			{
				return serializationCallbacks;
			}
			object obj = SerializationCallbacks.cache_lock;
			SerializationCallbacks result;
			lock (obj)
			{
				serializationCallbacks = (SerializationCallbacks)SerializationCallbacks.cache[t];
				if (serializationCallbacks == null)
				{
					Hashtable hashtable = (Hashtable)SerializationCallbacks.cache.Clone();
					serializationCallbacks = new SerializationCallbacks(t);
					hashtable[t] = serializationCallbacks;
					SerializationCallbacks.cache = hashtable;
				}
				result = serializationCallbacks;
			}
			return result;
		}

		public delegate void CallbackHandler(StreamingContext context);
	}
}
