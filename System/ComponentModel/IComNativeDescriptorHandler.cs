using System;

namespace System.ComponentModel
{
	/// <summary>Top level mapping layer between a COM object and TypeDescriptor.</summary>
	[Obsolete("Use TypeDescriptionProvider and TypeDescriptor.ComObjectType instead")]
	public interface IComNativeDescriptorHandler
	{
		AttributeCollection GetAttributes(object component);

		string GetClassName(object component);

		TypeConverter GetConverter(object component);

		EventDescriptor GetDefaultEvent(object component);

		PropertyDescriptor GetDefaultProperty(object component);

		object GetEditor(object component, Type baseEditorType);

		EventDescriptorCollection GetEvents(object component);

		EventDescriptorCollection GetEvents(object component, Attribute[] attributes);

		string GetName(object component);

		PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes);

		/// <summary>Retrieves the value of the property that has the specified dispatch identifier.</summary>
		/// <param name="component">The object to which the property belongs.</param>
		/// <param name="dispid">The dispatch identifier.</param>
		/// <param name="success">A <see cref="T:System.Boolean" />, passed by reference, that represents whether or not the property was retrieved. </param>
		object GetPropertyValue(object component, int dispid, ref bool success);

		/// <summary>Retrieves the value of the property that has the specified name.</summary>
		/// <param name="component">The object to which the property belongs.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="success">A <see cref="T:System.Boolean" />, passed by reference, that represents whether or not the property was retrieved. </param>
		object GetPropertyValue(object component, string propertyName, ref bool success);
	}
}
