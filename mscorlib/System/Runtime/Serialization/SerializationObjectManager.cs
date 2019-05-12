using System;
using System.Collections;

namespace System.Runtime.Serialization
{
	/// <summary>Manages serialization processes at run time. This class cannot be inherited.</summary>
	public sealed class SerializationObjectManager
	{
		private readonly StreamingContext context;

		private readonly Hashtable seen = new Hashtable();

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.SerializationObjectManager" /> class. </summary>
		/// <param name="context">An instance of the <see cref="T:System.Runtime.Serialization.StreamingContext" /> class that contains information about the current serialization operation.</param>
		public SerializationObjectManager(StreamingContext context)
		{
			this.context = context;
		}

		private event SerializationCallbacks.CallbackHandler callbacks;

		/// <summary>Registers the object upon which events will be raised.</summary>
		/// <param name="obj">The object to register.</param>
		public void RegisterObject(object obj)
		{
			if (this.seen.Contains(obj))
			{
				return;
			}
			SerializationCallbacks sc = SerializationCallbacks.GetSerializationCallbacks(obj.GetType());
			this.seen[obj] = 1;
			sc.RaiseOnSerializing(obj, this.context);
			if (sc.HasSerializedCallbacks)
			{
				this.callbacks = (SerializationCallbacks.CallbackHandler)Delegate.Combine(this.callbacks, new SerializationCallbacks.CallbackHandler(delegate(StreamingContext ctx)
				{
					sc.RaiseOnSerialized(obj, ctx);
				}));
			}
		}

		/// <summary>Invokes the OnSerializing callback event if the type of the object has one; and registers the object for raising the OnSerialized event if the type of the object has one.</summary>
		public void RaiseOnSerializedEvent()
		{
			if (this.callbacks != null)
			{
				this.callbacks(this.context);
			}
		}
	}
}
