using System;
using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Array" /> objects to and from various other representations.</summary>
	public class ArrayConverter : CollectionConverter
	{
		/// <summary>Converts the given value object to the specified destination type.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The culture into which <paramref name="value" /> will be converted.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value to. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationType" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is Array)
			{
				return value.GetType().Name + " Array";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Gets a collection of properties for the type of array specified by the value parameter.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that are exposed for an array, or null if there are no properties.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="value">An <see cref="T:System.Object" /> that specifies the type of array to get the properties for. </param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that will be used as a filter. </param>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (value == null)
			{
				throw new NullReferenceException();
			}
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
			if (value is Array)
			{
				Array array = (Array)value;
				for (int i = 0; i < array.Length; i++)
				{
					propertyDescriptorCollection.Add(new ArrayConverter.ArrayPropertyDescriptor(i, array.GetType()));
				}
			}
			return propertyDescriptorCollection;
		}

		/// <summary>Gets a value indicating whether this object supports properties.</summary>
		/// <returns>true because <see cref="M:System.ComponentModel.ArrayConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext,System.Object,System.Attribute[])" /> should be called to find the properties of this object. This method never returns false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		internal class ArrayPropertyDescriptor : PropertyDescriptor
		{
			private int index;

			private Type array_type;

			public ArrayPropertyDescriptor(int index, Type array_type) : base(string.Format("[{0}]", index), null)
			{
				this.index = index;
				this.array_type = array_type;
			}

			public override Type ComponentType
			{
				get
				{
					return this.array_type;
				}
			}

			public override Type PropertyType
			{
				get
				{
					return this.array_type.GetElementType();
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			public override object GetValue(object component)
			{
				if (component == null)
				{
					return null;
				}
				return ((Array)component).GetValue(this.index);
			}

			public override void SetValue(object component, object value)
			{
				if (component == null)
				{
					return;
				}
				((Array)component).SetValue(value, this.index);
			}

			public override void ResetValue(object component)
			{
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}
	}
}
