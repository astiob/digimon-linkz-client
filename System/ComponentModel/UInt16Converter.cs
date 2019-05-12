using System;
using System.Globalization;

namespace System.ComponentModel
{
	/// <summary>Provides a type converter to convert 16-bit unsigned integer objects to and from other representations.</summary>
	public class UInt16Converter : BaseNumberConverter
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.UInt16Converter" /> class. </summary>
		public UInt16Converter()
		{
			this.InnerType = typeof(ushort);
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
			return ((ushort)value).ToString("G", format);
		}

		internal override object ConvertFromString(string value, NumberFormatInfo format)
		{
			return ushort.Parse(value, NumberStyles.Integer, format);
		}

		internal override object ConvertFromString(string value, int fromBase)
		{
			return Convert.ToUInt16(value, fromBase);
		}
	}
}
