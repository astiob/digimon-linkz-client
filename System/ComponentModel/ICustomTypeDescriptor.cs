using System;

namespace System.ComponentModel
{
	/// <summary>Provides an interface that supplies dynamic custom type information for an object.</summary>
	public interface ICustomTypeDescriptor
	{
		/// <summary>Returns a collection of custom attributes for this instance of a component.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.AttributeCollection" /> containing the attributes for this object.</returns>
		AttributeCollection GetAttributes();

		/// <summary>Returns the class name of this instance of a component.</summary>
		/// <returns>The class name of the object, or null if the class does not have a name.</returns>
		string GetClassName();

		/// <summary>Returns the name of this instance of a component.</summary>
		/// <returns>The name of the object, or null if the object does not have a name.</returns>
		string GetComponentName();

		/// <summary>Returns a type converter for this instance of a component.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter" /> that is the converter for this object, or null if there is no <see cref="T:System.ComponentModel.TypeConverter" /> for this object.</returns>
		TypeConverter GetConverter();

		/// <summary>Returns the default event for this instance of a component.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventDescriptor" /> that represents the default event for this object, or null if this object does not have events.</returns>
		EventDescriptor GetDefaultEvent();

		/// <summary>Returns the default property for this instance of a component.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the default property for this object, or null if this object does not have properties.</returns>
		PropertyDescriptor GetDefaultProperty();

		/// <summary>Returns an editor of the specified type for this instance of a component.</summary>
		/// <returns>An <see cref="T:System.Object" /> of the specified type that is the editor for this object, or null if the editor cannot be found.</returns>
		/// <param name="editorBaseType">A <see cref="T:System.Type" /> that represents the editor for this object. </param>
		object GetEditor(Type editorBaseType);

		/// <summary>Returns the events for this instance of a component.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the events for this component instance.</returns>
		EventDescriptorCollection GetEvents();

		/// <summary>Returns the events for this instance of a component using the specified attribute array as a filter.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventDescriptorCollection" /> that represents the filtered events for this component instance.</returns>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter. </param>
		EventDescriptorCollection GetEvents(Attribute[] arr);

		/// <summary>Returns the properties for this instance of a component.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the properties for this component instance.</returns>
		PropertyDescriptorCollection GetProperties();

		/// <summary>Returns the properties for this instance of a component using the attribute array as a filter.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the filtered properties for this component instance.</returns>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter. </param>
		PropertyDescriptorCollection GetProperties(Attribute[] arr);

		/// <summary>Returns an object that contains the property described by the specified property descriptor.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the owner of the specified property.</returns>
		/// <param name="pd">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the property whose owner is to be found. </param>
		object GetPropertyOwner(PropertyDescriptor pd);
	}
}
