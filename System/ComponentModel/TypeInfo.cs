using System;
using System.Collections;
using System.Reflection;

namespace System.ComponentModel
{
	internal class TypeInfo : Info
	{
		private EventDescriptorCollection _events;

		private PropertyDescriptorCollection _properties;

		public TypeInfo(Type t) : base(t)
		{
		}

		public override AttributeCollection GetAttributes()
		{
			return base.GetAttributes(null);
		}

		public override EventDescriptorCollection GetEvents()
		{
			if (this._events != null)
			{
				return this._events;
			}
			EventInfo[] events = base.InfoType.GetEvents();
			EventDescriptor[] array = new EventDescriptor[events.Length];
			for (int i = 0; i < events.Length; i++)
			{
				array[i] = new ReflectionEventDescriptor(events[i]);
			}
			this._events = new EventDescriptorCollection(array);
			return this._events;
		}

		public override PropertyDescriptorCollection GetProperties()
		{
			if (this._properties != null)
			{
				return this._properties;
			}
			Hashtable hashtable = new Hashtable();
			ArrayList arrayList = new ArrayList();
			Type type = base.InfoType;
			while (type != null && type != typeof(object))
			{
				PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanRead && !hashtable.ContainsKey(propertyInfo.Name))
					{
						arrayList.Add(new ReflectionPropertyDescriptor(propertyInfo));
						hashtable.Add(propertyInfo.Name, null);
					}
				}
				type = type.BaseType;
			}
			this._properties = new PropertyDescriptorCollection((PropertyDescriptor[])arrayList.ToArray(typeof(PropertyDescriptor)), true);
			return this._properties;
		}
	}
}
