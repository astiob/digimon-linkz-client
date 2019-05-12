using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert object references to and from other representations.</summary>
	public class ReferenceConverter : TypeConverter
	{
		private Type reference_type;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ReferenceConverter" /> class.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type to associate with this reference converter. </param>
		public ReferenceConverter(Type type)
		{
			this.reference_type = type;
		}

		/// <summary>Gets a value indicating whether this converter can convert an object in the given source type to a reference object using the specified context.</summary>
		/// <returns>true if this object can perform the conversion; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you wish to convert from. </param>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return (context != null && sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Converts the given object to the reference type.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that specifies the culture used to represent the font. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			if (context != null)
			{
				object obj = null;
				System.ComponentModel.Design.IReferenceService referenceService = context.GetService(typeof(System.ComponentModel.Design.IReferenceService)) as System.ComponentModel.Design.IReferenceService;
				if (referenceService != null)
				{
					obj = referenceService.GetReference((string)value);
				}
				if (obj == null && context.Container != null && context.Container.Components != null)
				{
					obj = context.Container.Components[(string)value];
				}
				return obj;
			}
			return null;
		}

		/// <summary>Converts the given value object to the reference type using the specified context and arguments.</summary>
		/// <returns>The converted object.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that specifies the culture used to represent the font. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The type to convert the object to. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationType" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				return "(none)";
			}
			string text = string.Empty;
			if (context != null)
			{
				System.ComponentModel.Design.IReferenceService referenceService = context.GetService(typeof(System.ComponentModel.Design.IReferenceService)) as System.ComponentModel.Design.IReferenceService;
				if (referenceService != null)
				{
					text = referenceService.GetName(value);
				}
				if ((text == null || text.Length == 0) && value is IComponent)
				{
					IComponent component = (IComponent)value;
					if (component.Site != null && component.Site.Name != null)
					{
						text = component.Site.Name;
					}
				}
			}
			return text;
		}

		/// <summary>Gets a collection of standard values for the reference data type.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that holds a standard set of valid values, or null if the data type does not support a standard set of values.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ArrayList arrayList = new ArrayList();
			if (context != null)
			{
				System.ComponentModel.Design.IReferenceService referenceService = context.GetService(typeof(System.ComponentModel.Design.IReferenceService)) as System.ComponentModel.Design.IReferenceService;
				if (referenceService != null)
				{
					foreach (object value in referenceService.GetReferences(this.reference_type))
					{
						if (this.IsValueAllowed(context, value))
						{
							arrayList.Add(value);
						}
					}
				}
				else if (context.Container != null && context.Container.Components != null)
				{
					foreach (object obj in context.Container.Components)
					{
						if (obj != null && this.IsValueAllowed(context, obj) && this.reference_type.IsInstanceOfType(obj))
						{
							arrayList.Add(obj);
						}
					}
				}
				arrayList.Add(null);
			}
			return new TypeConverter.StandardValuesCollection(arrayList);
		}

		/// <summary>Gets a value indicating whether the list of standard values returned from <see cref="M:System.ComponentModel.ReferenceConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)" /> is an exclusive list.</summary>
		/// <returns>true because the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> returned from <see cref="M:System.ComponentModel.ReferenceConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)" /> is an exhaustive list of possible values. This method never returns false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Gets a value indicating whether this object supports a standard set of values that can be picked from a list.</summary>
		/// <returns>true because <see cref="M:System.ComponentModel.ReferenceConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)" /> can be called to find a common set of values the object supports. This method never returns false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Returns a value indicating whether a particular value can be added to the standard values collection.</summary>
		/// <returns>true if the value is allowed and can be added to the standard values collection; false if the value cannot be added to the standard values collection.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides an additional context. </param>
		/// <param name="value">The value to check. </param>
		protected virtual bool IsValueAllowed(ITypeDescriptorContext context, object value)
		{
			return true;
		}
	}
}
