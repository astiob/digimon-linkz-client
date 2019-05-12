using System;
using System.ComponentModel;
using System.Globalization;

namespace System
{
	/// <summary>Converts a <see cref="T:System.String" /> type to a <see cref="T:System.Uri" /> type, and vice versa.</summary>
	public class UriTypeConverter : System.ComponentModel.TypeConverter
	{
		private bool CanConvert(Type type)
		{
			return type == typeof(string) || type == typeof(System.Uri);
		}

		/// <summary>Returns whether this converter can convert an object of the given type to the type of this converter.</summary>
		/// <returns>true if <paramref name="sourceType" /> is a <see cref="T:System.String" /> type or a <see cref="T:System.Uri" /> type can be assigned from <paramref name="sourceType" />; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />  that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type that you want to convert from.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sourceType" /> parameter is null.</exception>
		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return this.CanConvert(sourceType);
		}

		/// <summary>Returns whether this converter can convert the object to the specified type, using the specified context.</summary>
		/// <returns>true if <paramref name="destinationType" /> is of type <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" />, <see cref="T:System.String" />, or <see cref="T:System.Uri" />; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />  that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type that you want to convert to.</param>
		public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType != null && this.CanConvert(destinationType);
		}

		/// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />  that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.</exception>
		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!this.CanConvertFrom(context, value.GetType()))
			{
				throw new NotSupportedException(Locale.GetText("Cannot convert from value."));
			}
			if (value is System.Uri)
			{
				return value;
			}
			string text = value as string;
			if (text != null)
			{
				return new System.Uri(text, System.UriKind.RelativeOrAbsolute);
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts a given value object to the specified type, using the specified context and culture information.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />  that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" /> parameter is null.</exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.</exception>
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!this.CanConvertTo(context, destinationType))
			{
				throw new NotSupportedException(Locale.GetText("Cannot convert to destination type."));
			}
			System.Uri uri = value as System.Uri;
			if (uri != null)
			{
				if (destinationType == typeof(string))
				{
					return uri.ToString();
				}
				if (destinationType == typeof(System.Uri))
				{
					return uri;
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
