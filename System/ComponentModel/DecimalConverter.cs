using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Decimal" /> objects to and from various other representations.</summary>
	public class DecimalConverter : BaseNumberConverter
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DecimalConverter" /> class. </summary>
		public DecimalConverter()
		{
			this.InnerType = typeof(decimal);
		}

		internal override bool SupportHex
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you wish to convert to. </param>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given value object to a <see cref="T:System.Decimal" /> using the arguments.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo" />. If not supplied, the current culture is assumed. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value to. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) && value is decimal)
			{
				decimal d = (decimal)value;
				ConstructorInfo constructor = typeof(decimal).GetConstructor(new Type[]
				{
					typeof(int[])
				});
				return new System.ComponentModel.Design.Serialization.InstanceDescriptor(constructor, new object[]
				{
					decimal.GetBits(d)
				});
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((decimal)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return decimal.Parse(value, NumberStyles.Float, format);
		}
	}
}
