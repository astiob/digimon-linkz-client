using System;
using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a base type converter for nonfloating-point numerical types.</summary>
	public abstract class BaseNumberConverter : TypeConverter
	{
		internal Type InnerType;

		internal abstract bool SupportHex { get; }

		/// <summary>Determines if this converter can convert an object in the given source type to the native type of the converter.</summary>
		/// <returns>true if this converter can perform the operation; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type from which you want to convert. </param>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Returns a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
		/// <returns>true if this converter can perform the operation; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="t">A <see cref="T:System.Type" /> that represents the type to which you want to convert. </param>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type t)
		{
			return t.IsPrimitive || base.CanConvertTo(context, t);
		}

		/// <summary>Converts the given object to the converter's native type.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that specifies the culture to represent the number. </param>
		/// <param name="value">The object to convert. </param>
		/// <exception cref="T:System.Exception">
		///   <paramref name="value" /> is not a valid value for the target type.</exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			string text = value as string;
			if (text != null)
			{
				try
				{
					if (this.SupportHex)
					{
						if (text.Length >= 1 && text[0] == '#')
						{
							return this.ConvertFromString(text.Substring(1), 16);
						}
						if (text.StartsWith("0x") || text.StartsWith("0X"))
						{
							return this.ConvertFromString(text, 16);
						}
					}
					NumberFormatInfo format = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
					return this.ConvertFromString(text, format);
				}
				catch (Exception innerException)
				{
					throw new Exception(value.ToString() + " is not a valid value for " + this.InnerType.Name + ".", innerException);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the specified object to another type.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that specifies the culture to represent the number. </param>
		/// <param name="value">The object to convert. </param>
		/// <param name="destinationType">The type to convert the object to. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationType" /> is null.</exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			if (destinationType == typeof(string) && value is IConvertible)
			{
				return ((IConvertible)value).ToType(destinationType, culture);
			}
			if (destinationType.IsPrimitive)
			{
				return Convert.ChangeType(value, destinationType, culture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		internal abstract string ConvertToString(object value, NumberFormatInfo format);

		internal abstract object ConvertFromString(string value, NumberFormatInfo format);

		internal virtual object ConvertFromString(string value, int fromBase)
		{
			if (this.SupportHex)
			{
				throw new NotImplementedException();
			}
			throw new InvalidOperationException();
		}
	}
}
