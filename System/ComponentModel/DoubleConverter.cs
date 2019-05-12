using System;
using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert double-precision, floating point number objects to and from various other representations.</summary>
	public class DoubleConverter : BaseNumberConverter
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DoubleConverter" /> class. </summary>
		public DoubleConverter()
		{
			this.InnerType = typeof(double);
		}

		internal override bool SupportHex
		{
			get
			{
				return false;
			}
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((double)value).ToString("R", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return double.Parse(value, NumberStyles.Float, format);
		}
	}
}
