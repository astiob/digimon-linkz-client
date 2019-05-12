using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace System
{
	[Serializable]
	internal class DelegateSerializationHolder : ISerializable, IObjectReference
	{
		private Delegate _delegate;

		private DelegateSerializationHolder(SerializationInfo info, StreamingContext ctx)
		{
			DelegateSerializationHolder.DelegateEntry delegateEntry = (DelegateSerializationHolder.DelegateEntry)info.GetValue("Delegate", typeof(DelegateSerializationHolder.DelegateEntry));
			int num = 0;
			DelegateSerializationHolder.DelegateEntry delegateEntry2 = delegateEntry;
			while (delegateEntry2 != null)
			{
				delegateEntry2 = delegateEntry2.delegateEntry;
				num++;
			}
			if (num == 1)
			{
				this._delegate = delegateEntry.DeserializeDelegate(info);
			}
			else
			{
				Delegate[] array = new Delegate[num];
				delegateEntry2 = delegateEntry;
				for (int i = 0; i < num; i++)
				{
					array[i] = delegateEntry2.DeserializeDelegate(info);
					delegateEntry2 = delegateEntry2.delegateEntry;
				}
				this._delegate = Delegate.Combine(array);
			}
		}

		public static void GetDelegateData(Delegate instance, SerializationInfo info, StreamingContext ctx)
		{
			Delegate[] invocationList = instance.GetInvocationList();
			DelegateSerializationHolder.DelegateEntry delegateEntry = null;
			for (int i = 0; i < invocationList.Length; i++)
			{
				Delegate @delegate = invocationList[i];
				string text = (@delegate.Target == null) ? null : ("target" + i);
				DelegateSerializationHolder.DelegateEntry delegateEntry2 = new DelegateSerializationHolder.DelegateEntry(@delegate, text);
				if (delegateEntry == null)
				{
					info.AddValue("Delegate", delegateEntry2);
				}
				else
				{
					delegateEntry.delegateEntry = delegateEntry2;
				}
				delegateEntry = delegateEntry2;
				if (@delegate.Target != null)
				{
					info.AddValue(text, @delegate.Target);
				}
			}
			info.SetType(typeof(DelegateSerializationHolder));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException();
		}

		public object GetRealObject(StreamingContext context)
		{
			return this._delegate;
		}

		[Serializable]
		private class DelegateEntry
		{
			private string type;

			private string assembly;

			public object target;

			private string targetTypeAssembly;

			private string targetTypeName;

			private string methodName;

			public DelegateSerializationHolder.DelegateEntry delegateEntry;

			public DelegateEntry(Delegate del, string targetLabel)
			{
				this.type = del.GetType().FullName;
				this.assembly = del.GetType().Assembly.FullName;
				this.target = targetLabel;
				this.targetTypeAssembly = del.Method.DeclaringType.Assembly.FullName;
				this.targetTypeName = del.Method.DeclaringType.FullName;
				this.methodName = del.Method.Name;
			}

			public Delegate DeserializeDelegate(SerializationInfo info)
			{
				object obj = null;
				if (this.target != null)
				{
					obj = info.GetValue(this.target.ToString(), typeof(object));
				}
				Assembly assembly = Assembly.Load(this.assembly);
				Type type = assembly.GetType(this.type);
				Delegate result;
				if (obj != null)
				{
					if (RemotingServices.IsTransparentProxy(obj))
					{
						Assembly assembly2 = Assembly.Load(this.targetTypeAssembly);
						Type type2 = assembly2.GetType(this.targetTypeName);
						if (!type2.IsInstanceOfType(obj))
						{
							throw new RemotingException("Unexpected proxy type.");
						}
					}
					result = Delegate.CreateDelegate(type, obj, this.methodName);
				}
				else
				{
					Assembly assembly3 = Assembly.Load(this.targetTypeAssembly);
					Type type3 = assembly3.GetType(this.targetTypeName);
					result = Delegate.CreateDelegate(type, type3, this.methodName);
				}
				return result;
			}
		}
	}
}
