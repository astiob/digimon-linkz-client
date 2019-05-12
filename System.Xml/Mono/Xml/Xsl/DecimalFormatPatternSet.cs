using System;

namespace Mono.Xml.Xsl
{
	internal class DecimalFormatPatternSet
	{
		private DecimalFormatPattern positivePattern;

		private DecimalFormatPattern negativePattern;

		public DecimalFormatPatternSet(string pattern, XslDecimalFormat decimalFormat)
		{
			this.Parse(pattern, decimalFormat);
		}

		private void Parse(string pattern, XslDecimalFormat format)
		{
			if (pattern.Length == 0)
			{
				throw new ArgumentException("Invalid number format pattern string.");
			}
			this.positivePattern = new DecimalFormatPattern();
			this.negativePattern = this.positivePattern;
			int num = this.positivePattern.ParsePattern(0, pattern, format);
			if (num < pattern.Length)
			{
				if (pattern[num] != format.PatternSeparator)
				{
					return;
				}
				num++;
				this.negativePattern = new DecimalFormatPattern();
				num = this.negativePattern.ParsePattern(num, pattern, format);
				if (num < pattern.Length)
				{
					throw new ArgumentException("Number format pattern string ends with extraneous part.");
				}
			}
		}

		public string FormatNumber(double number)
		{
			if (number >= 0.0)
			{
				return this.positivePattern.FormatNumber(number);
			}
			return this.negativePattern.FormatNumber(number);
		}
	}
}
