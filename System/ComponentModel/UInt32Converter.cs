using System;
using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert 32-bit unsigned integer objects to and from various other representations.</summary>
	public class UInt32Converter : BaseNumberConverter
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.UInt32Converter" /> class. </summary>
		public UInt32Converter()
		{
			this.InnerType = typeof(uint);
		}

		internal override bool SupportHex
		{
			get
			{
				return true;
			}
		}

		internal override string ConvertToString(object value, NumberFormatInfo format)
		{
			return ((uint)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return uint.Parse(value, NumberStyles.Integer, format);
		}

		internal override object ConvertFromString(string value, int fromBase)
		{
			return Convert.ToUInt32(value, fromBase);
		}
	}
}
