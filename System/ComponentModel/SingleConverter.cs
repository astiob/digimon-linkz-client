using System;
using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert single-precision, floating point number objects to and from various other representations.</summary>
	public class SingleConverter : BaseNumberConverter
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.SingleConverter" /> class. </summary>
		public SingleConverter()
		{
			this.InnerType = typeof(float);
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
			return ((float)value).ToString("R", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return float.Parse(value, NumberStyles.Float, format);
		}
	}
}
