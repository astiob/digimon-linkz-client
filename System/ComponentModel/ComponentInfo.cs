using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;

namespace System.ComponentModel
{
	internal class ComponentInfo : Info
	{
		private IComponent _component;

		private EventDescriptorCollection _events;

		private PropertyDescriptorCollection _properties;

		public ComponentInfo(IComponent component) : base(component.GetType())
		{
			this._component = component;
		}

		public override AttributeCollection GetAttributes()
		{
			return base.GetAttributes(this._component);
		}

		public override EventDescriptorCollection GetEvents()
		{
			if (this._events != null)
			{
				return this._events;
			}
			bool flag = true;
			EventInfo[] events = this._component.GetType().GetEvents();
			Hashtable hashtable = new Hashtable();
			foreach (EventInfo eventInfo in events)
			{
				hashtable[eventInfo.Name] = new ReflectionEventDescriptor(eventInfo);
			}
			if (this._component.Site != null)
			{
				System.ComponentModel.Design.ITypeDescriptorFilterService typeDescriptorFilterService = (System.ComponentModel.Design.ITypeDescriptorFilterService)this._component.Site.GetService(typeof(System.ComponentModel.Design.ITypeDescriptorFilterService));
				if (typeDescriptorFilterService != null)
				{
					flag = typeDescriptorFilterService.FilterEvents(this._component, hashtable);
				}
			}
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(hashtable.Values);
			EventDescriptorCollection eventDescriptorCollection = new EventDescriptorCollection(arrayList);
			if (flag)
			{
				this._events = eventDescriptorCollection;
			}
			return eventDescriptorCollection;
		}

		public override PropertyDescriptorCollection GetProperties()
		{
			if (this._properties != null)
			{
				return this._properties;
			}
			bool flag = true;
			PropertyInfo[] properties = this._component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			Hashtable hashtable = new Hashtable();
			for (int i = properties.Length - 1; i >= 0; i--)
			{
				hashtable[properties[i].Name] = new ReflectionPropertyDescriptor(properties[i]);
			}
			if (this._component.Site != null)
			{
				System.ComponentModel.Design.ITypeDescriptorFilterService typeDescriptorFilterService = (System.ComponentModel.Design.ITypeDescriptorFilterService)this._component.Site.GetService(typeof(System.ComponentModel.Design.ITypeDescriptorFilterService));
				if (typeDescriptorFilterService != null)
				{
					flag = typeDescriptorFilterService.FilterProperties(this._component, hashtable);
				}
			}
			PropertyDescriptor[] array = new PropertyDescriptor[hashtable.Values.Count];
			hashtable.Values.CopyTo(array, 0);
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(array, true);
			if (flag)
			{
				this._properties = propertyDescriptorCollection;
			}
			return propertyDescriptorCollection;
		}
	}
}
