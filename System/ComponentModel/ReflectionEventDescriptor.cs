using System;
using System.Reflection;

namespace System.ComponentModel
{
	internal class ReflectionEventDescriptor : EventDescriptor
	{
		private Type _eventType;

		private Type _componentType;

		private EventInfo _eventInfo;

		private MethodInfo add_method;

		private MethodInfo remove_method;

		public ReflectionEventDescriptor(EventInfo eventInfo) : base(eventInfo.Name, (Attribute[])eventInfo.GetCustomAttributes(true))
		{
			this._eventInfo = eventInfo;
			this._componentType = eventInfo.DeclaringType;
			this._eventType = eventInfo.EventHandlerType;
			this.add_method = eventInfo.GetAddMethod();
			this.remove_method = eventInfo.GetRemoveMethod();
		}

		public ReflectionEventDescriptor(Type componentType, EventDescriptor oldEventDescriptor, Attribute[] attrs) : base(oldEventDescriptor, attrs)
		{
			this._componentType = componentType;
			this._eventType = oldEventDescriptor.EventType;
			EventInfo @event = componentType.GetEvent(oldEventDescriptor.Name);
			this.add_method = @event.GetAddMethod();
			this.remove_method = @event.GetRemoveMethod();
		}

		public ReflectionEventDescriptor(Type componentType, string name, Type type, Attribute[] attrs) : base(name, attrs)
		{
			this._componentType = componentType;
			this._eventType = type;
			EventInfo @event = componentType.GetEvent(name);
			this.add_method = @event.GetAddMethod();
			this.remove_method = @event.GetRemoveMethod();
		}

		private EventInfo GetEventInfo()
		{
			if (this._eventInfo == null)
			{
				this._eventInfo = this._componentType.GetEvent(this.Name);
				if (this._eventInfo == null)
				{
					throw new ArgumentException("Accessor methods for the " + this.Name + " event are missing");
				}
			}
			return this._eventInfo;
		}

		public override void AddEventHandler(object component, Delegate value)
		{
			this.add_method.Invoke(component, new object[]
			{
				value
			});
		}

		public override void RemoveEventHandler(object component, Delegate value)
		{
			this.remove_method.Invoke(component, new object[]
			{
				value
			});
		}

		public override Type ComponentType
		{
			get
			{
				return this._componentType;
			}
		}

		public override Type EventType
		{
			get
			{
				return this._eventType;
			}
		}

		public override bool IsMulticast
		{
			get
			{
				return this.GetEventInfo().IsMulticast;
			}
		}
	}
}
