using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Globalization
{
	/// <summary>Defines how numeric values are formatted and displayed, depending on the culture.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class NumberFormatInfo : ICloneable, IFormatProvider
	{
		private bool isReadOnly;

		private string decimalFormats;

		private string currencyFormats;

		private string percentFormats;

		private string digitPattern = "#";

		private string zeroPattern = "0";

		private int currencyDecimalDigits;

		private string currencyDecimalSeparator;

		private string currencyGroupSeparator;

		private int[] currencyGroupSizes;

		private int currencyNegativePattern;

		private int currencyPositivePattern;

		private string currencySymbol;

		private string nanSymbol;

		private string negativeInfinitySymbol;

		private string negativeSign;

		private int numberDecimalDigits;

		private string numberDecimalSeparator;

		private string numberGroupSeparator;

		private int[] numberGroupSizes;

		private int numberNegativePattern;

		private int percentDecimalDigits;

		private string percentDecimalSeparator;

		private string percentGroupSeparator;

		private int[] percentGroupSizes;

		private int percentNegativePattern;

		private int percentPositivePattern;

		private string percentSymbol;

		private string perMilleSymbol;

		private string positiveInfinitySymbol;

		private string positiveSign;

		private string ansiCurrencySymbol;

		private int m_dataItem;

		private bool m_useUserOverride;

		private bool validForParseAsNumber;

		private bool validForParseAsCurrency;

		private string[] nativeDigits = NumberFormatInfo.invariantNativeDigits;

		private int digitSubstitution = 1;

		private static readonly string[] invariantNativeDigits = new string[]
		{
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9"
		};

		internal NumberFormatInfo(int lcid, bool read_only)
		{
			this.isReadOnly = read_only;
			if (lcid != 127)
			{
				lcid = 127;
			}
			int num = lcid;
			if (num == 127)
			{
				this.isReadOnly = false;
				this.currencyDecimalDigits = 2;
				this.currencyDecimalSeparator = ".";
				this.currencyGroupSeparator = ",";
				this.currencyGroupSizes = new int[]
				{
					3
				};
				this.currencyNegativePattern = 0;
				this.currencyPositivePattern = 0;
				this.currencySymbol = "$";
				this.nanSymbol = "NaN";
				this.negativeInfinitySymbol = "-Infinity";
				this.negativeSign = "-";
				this.numberDecimalDigits = 2;
				this.numberDecimalSeparator = ".";
				this.numberGroupSeparator = ",";
				this.numberGroupSizes = new int[]
				{
					3
				};
				this.numberNegativePattern = 1;
				this.percentDecimalDigits = 2;
				this.percentDecimalSeparator = ".";
				this.percentGroupSeparator = ",";
				this.percentGroupSizes = new int[]
				{
					3
				};
				this.percentNegativePattern = 0;
				this.percentPositivePattern = 0;
				this.percentSymbol = "%";
				this.perMilleSymbol = "‰";
				this.positiveInfinitySymbol = "Infinity";
				this.positiveSign = "+";
			}
		}

		internal NumberFormatInfo(bool read_only) : this(127, read_only)
		{
		}

		/// <summary>Initializes a new writable instance of the <see cref="T:System.Globalization.NumberFormatInfo" /> class that is culture-independent (invariant).</summary>
		public NumberFormatInfo() : this(false)
		{
		}

		private void InitPatterns()
		{
			string[] array = this.decimalFormats.Split(new char[]
			{
				';'
			}, 2);
			string[] array2;
			if (array.Length == 2)
			{
				array2 = array[0].Split(new char[]
				{
					'.'
				}, 2);
				if (array2.Length == 2)
				{
					this.numberDecimalDigits = 0;
					for (int i = 0; i < array2[1].Length; i++)
					{
						if (array2[1][i] != this.digitPattern[0])
						{
							break;
						}
						this.numberDecimalDigits++;
					}
					string[] array3 = array2[0].Split(new char[]
					{
						','
					});
					if (array3.Length > 1)
					{
						this.numberGroupSizes = new int[array3.Length - 1];
						for (int j = 0; j < this.numberGroupSizes.Length; j++)
						{
							string text = array3[j + 1];
							this.numberGroupSizes[j] = text.Length;
						}
					}
					else
					{
						this.numberGroupSizes = new int[1];
					}
					if (array[1].StartsWith("(") && array[1].EndsWith(")"))
					{
						this.numberNegativePattern = 0;
					}
					else if (array[1].StartsWith("- "))
					{
						this.numberNegativePattern = 2;
					}
					else if (array[1].StartsWith("-"))
					{
						this.numberNegativePattern = 1;
					}
					else if (array[1].EndsWith(" -"))
					{
						this.numberNegativePattern = 4;
					}
					else if (array[1].EndsWith("-"))
					{
						this.numberNegativePattern = 3;
					}
					else
					{
						this.numberNegativePattern = 1;
					}
				}
			}
			array = this.currencyFormats.Split(new char[]
			{
				';'
			}, 2);
			if (array.Length == 2)
			{
				array2 = array[0].Split(new char[]
				{
					'.'
				}, 2);
				if (array2.Length == 2)
				{
					this.currencyDecimalDigits = 0;
					for (int k = 0; k < array2[1].Length; k++)
					{
						if (array2[1][k] != this.zeroPattern[0])
						{
							break;
						}
						this.currencyDecimalDigits++;
					}
					string[] array3 = array2[0].Split(new char[]
					{
						','
					});
					if (array3.Length > 1)
					{
						this.currencyGroupSizes = new int[array3.Length - 1];
						for (int l = 0; l < this.currencyGroupSizes.Length; l++)
						{
							string text2 = array3[l + 1];
							this.currencyGroupSizes[l] = text2.Length;
						}
					}
					else
					{
						this.currencyGroupSizes = new int[1];
					}
					if (array[1].StartsWith("(¤ ") && array[1].EndsWith(")"))
					{
						this.currencyNegativePattern = 14;
					}
					else if (array[1].StartsWith("(¤") && array[1].EndsWith(")"))
					{
						this.currencyNegativePattern = 0;
					}
					else if (array[1].StartsWith("¤ ") && array[1].EndsWith("-"))
					{
						this.currencyNegativePattern = 11;
					}
					else if (array[1].StartsWith("¤") && array[1].EndsWith("-"))
					{
						this.currencyNegativePattern = 3;
					}
					else if (array[1].StartsWith("(") && array[1].EndsWith(" ¤"))
					{
						this.currencyNegativePattern = 15;
					}
					else if (array[1].StartsWith("(") && array[1].EndsWith("¤"))
					{
						this.currencyNegativePattern = 4;
					}
					else if (array[1].StartsWith("-") && array[1].EndsWith(" ¤"))
					{
						this.currencyNegativePattern = 8;
					}
					else if (array[1].StartsWith("-") && array[1].EndsWith("¤"))
					{
						this.currencyNegativePattern = 5;
					}
					else if (array[1].StartsWith("-¤ "))
					{
						this.currencyNegativePattern = 9;
					}
					else if (array[1].StartsWith("-¤"))
					{
						this.currencyNegativePattern = 1;
					}
					else if (array[1].StartsWith("¤ -"))
					{
						this.currencyNegativePattern = 12;
					}
					else if (array[1].StartsWith("¤-"))
					{
						this.currencyNegativePattern = 2;
					}
					else if (array[1].EndsWith(" ¤-"))
					{
						this.currencyNegativePattern = 10;
					}
					else if (array[1].EndsWith("¤-"))
					{
						this.currencyNegativePattern = 7;
					}
					else if (array[1].EndsWith("- ¤"))
					{
						this.currencyNegativePattern = 13;
					}
					else if (array[1].EndsWith("-¤"))
					{
						this.currencyNegativePattern = 6;
					}
					else
					{
						this.currencyNegativePattern = 0;
					}
					if (array[0].StartsWith("¤ "))
					{
						this.currencyPositivePattern = 2;
					}
					else if (array[0].StartsWith("¤"))
					{
						this.currencyPositivePattern = 0;
					}
					else if (array[0].EndsWith(" ¤"))
					{
						this.currencyPositivePattern = 3;
					}
					else if (array[0].EndsWith("¤"))
					{
						this.currencyPositivePattern = 1;
					}
					else
					{
						this.currencyPositivePattern = 0;
					}
				}
			}
			if (this.percentFormats.StartsWith("%"))
			{
				this.percentPositivePattern = 2;
				this.percentNegativePattern = 2;
			}
			else if (this.percentFormats.EndsWith(" %"))
			{
				this.percentPositivePattern = 0;
				this.percentNegativePattern = 0;
			}
			else if (this.percentFormats.EndsWith("%"))
			{
				this.percentPositivePattern = 1;
				this.percentNegativePattern = 1;
			}
			else
			{
				this.percentPositivePattern = 0;
				this.percentNegativePattern = 0;
			}
			array2 = this.percentFormats.Split(new char[]
			{
				'.'
			}, 2);
			if (array2.Length == 2)
			{
				this.percentDecimalDigits = 0;
				for (int m = 0; m < array2[1].Length; m++)
				{
					if (array2[1][m] != this.digitPattern[0])
					{
						break;
					}
					this.percentDecimalDigits++;
				}
				string[] array3 = array2[0].Split(new char[]
				{
					','
				});
				if (array3.Length > 1)
				{
					this.percentGroupSizes = new int[array3.Length - 1];
					for (int n = 0; n < this.percentGroupSizes.Length; n++)
					{
						string text3 = array3[n + 1];
						this.percentGroupSizes[n] = text3.Length;
					}
				}
				else
				{
					this.percentGroupSizes = new int[1];
				}
			}
		}

		/// <summary>Gets or sets the number of decimal places to use in currency values.</summary>
		/// <returns>The number of decimal places to use in currency values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 2.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 99. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int CurrencyDecimalDigits
		{
			get
			{
				return this.currencyDecimalDigits;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 99");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.currencyDecimalDigits = value;
			}
		}

		/// <summary>Gets or sets the string to use as the decimal separator in currency values.</summary>
		/// <returns>The string to use as the decimal separator in currency values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is ".".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to an empty string.</exception>
		public string CurrencyDecimalSeparator
		{
			get
			{
				return this.currencyDecimalSeparator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.currencyDecimalSeparator = value;
			}
		}

		/// <summary>Gets or sets the string that separates groups of digits to the left of the decimal in currency values.</summary>
		/// <returns>The string that separates groups of digits to the left of the decimal in currency values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is ",".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string CurrencyGroupSeparator
		{
			get
			{
				return this.currencyGroupSeparator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.currencyGroupSeparator = value;
			}
		}

		/// <summary>Gets or sets the number of digits in each group to the left of the decimal in currency values.</summary>
		/// <returns>The number of digits in each group to the left of the decimal in currency values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is a one-dimensional array with only one element, which is set to 3.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set and the array contains an entry that is less than 0 or greater than 9.-or- The property is being set and the array contains an entry, other than the last entry, that is set to 0. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int[] CurrencyGroupSizes
		{
			get
			{
				return (int[])this.RawCurrencyGroupSizes.Clone();
			}
			set
			{
				this.RawCurrencyGroupSizes = value;
			}
		}

		internal int[] RawCurrencyGroupSizes
		{
			get
			{
				return this.currencyGroupSizes;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				if (value.Length == 0)
				{
					this.currencyGroupSizes = new int[0];
					return;
				}
				int num = value.Length - 1;
				for (int i = 0; i < num; i++)
				{
					if (value[i] < 1 || value[i] > 9)
					{
						throw new ArgumentOutOfRangeException("One of the elements in the array specified is not between 1 and 9");
					}
				}
				if (value[num] < 0 || value[num] > 9)
				{
					throw new ArgumentOutOfRangeException("Last element in the array specified is not between 0 and 9");
				}
				this.currencyGroupSizes = (int[])value.Clone();
			}
		}

		/// <summary>Gets or sets the format pattern for negative currency values.</summary>
		/// <returns>The format pattern for negative currency values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 0, which represents "($n)", where "$" is the <see cref="P:System.Globalization.NumberFormatInfo.CurrencySymbol" /> and <paramref name="n" /> is a number.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 15. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int CurrencyNegativePattern
		{
			get
			{
				return this.currencyNegativePattern;
			}
			set
			{
				if (value < 0 || value > 15)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 15");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.currencyNegativePattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for positive currency values.</summary>
		/// <returns>The format pattern for positive currency values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 0, which represents "$n", where "$" is the <see cref="P:System.Globalization.NumberFormatInfo.CurrencySymbol" /> and <paramref name="n" /> is a number.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 3. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int CurrencyPositivePattern
		{
			get
			{
				return this.currencyPositivePattern;
			}
			set
			{
				if (value < 0 || value > 3)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 3");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.currencyPositivePattern = value;
			}
		}

		/// <summary>Gets or sets the string to use as the currency symbol.</summary>
		/// <returns>The string to use as the currency symbol. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "¤".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string CurrencySymbol
		{
			get
			{
				return this.currencySymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.currencySymbol = value;
			}
		}

		/// <summary>Gets a read-only <see cref="T:System.Globalization.NumberFormatInfo" /> that formats values based on the current culture.</summary>
		/// <returns>A read-only <see cref="T:System.Globalization.NumberFormatInfo" /> based on the <see cref="T:System.Globalization.CultureInfo" /> of the current thread.</returns>
		public static NumberFormatInfo CurrentInfo
		{
			get
			{
				NumberFormatInfo numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;
				numberFormat.isReadOnly = true;
				return numberFormat;
			}
		}

		/// <summary>Gets the default read-only <see cref="T:System.Globalization.NumberFormatInfo" /> that is culture-independent (invariant).</summary>
		/// <returns>The default read-only <see cref="T:System.Globalization.NumberFormatInfo" /> that is culture-independent (invariant).</returns>
		public static NumberFormatInfo InvariantInfo
		{
			get
			{
				return new NumberFormatInfo
				{
					NumberNegativePattern = 1,
					isReadOnly = true
				};
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		/// <summary>Gets or sets the string that represents the IEEE NaN (not a number) value.</summary>
		/// <returns>The string that represents the IEEE NaN (not a number) value. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "NaN".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string NaNSymbol
		{
			get
			{
				return this.nanSymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.nanSymbol = value;
			}
		}

		/// <summary>Gets or sets the string that represents negative infinity.</summary>
		/// <returns>The string that represents negative infinity. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "-Infinity".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string NegativeInfinitySymbol
		{
			get
			{
				return this.negativeInfinitySymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.negativeInfinitySymbol = value;
			}
		}

		/// <summary>Gets or sets the string that denotes that the associated number is negative.</summary>
		/// <returns>The string that denotes that the associated number is negative. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "-".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string NegativeSign
		{
			get
			{
				return this.negativeSign;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.negativeSign = value;
			}
		}

		/// <summary>Gets or sets the number of decimal places to use in numeric values.</summary>
		/// <returns>The number of decimal places to use in numeric values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 2.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 99. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int NumberDecimalDigits
		{
			get
			{
				return this.numberDecimalDigits;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 99");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.numberDecimalDigits = value;
			}
		}

		/// <summary>Gets or sets the string to use as the decimal separator in numeric values.</summary>
		/// <returns>The string to use as the decimal separator in numeric values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is ".".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to an empty string.</exception>
		public string NumberDecimalSeparator
		{
			get
			{
				return this.numberDecimalSeparator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.numberDecimalSeparator = value;
			}
		}

		/// <summary>Gets or sets the string that separates groups of digits to the left of the decimal in numeric values.</summary>
		/// <returns>The string that separates groups of digits to the left of the decimal in numeric values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is ",".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string NumberGroupSeparator
		{
			get
			{
				return this.numberGroupSeparator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.numberGroupSeparator = value;
			}
		}

		/// <summary>Gets or sets the number of digits in each group to the left of the decimal in numeric values.</summary>
		/// <returns>The number of digits in each group to the left of the decimal in numeric values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is a one-dimensional array with only one element, which is set to 3.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set and the array contains an entry that is less than 0 or greater than 9.-or- The property is being set and the array contains an entry, other than the last entry, that is set to 0. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int[] NumberGroupSizes
		{
			get
			{
				return (int[])this.RawNumberGroupSizes.Clone();
			}
			set
			{
				this.RawNumberGroupSizes = value;
			}
		}

		internal int[] RawNumberGroupSizes
		{
			get
			{
				return this.numberGroupSizes;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				if (value.Length == 0)
				{
					this.numberGroupSizes = new int[0];
					return;
				}
				int num = value.Length - 1;
				for (int i = 0; i < num; i++)
				{
					if (value[i] < 1 || value[i] > 9)
					{
						throw new ArgumentOutOfRangeException("One of the elements in the array specified is not between 1 and 9");
					}
				}
				if (value[num] < 0 || value[num] > 9)
				{
					throw new ArgumentOutOfRangeException("Last element in the array specified is not between 0 and 9");
				}
				this.numberGroupSizes = (int[])value.Clone();
			}
		}

		/// <summary>Gets or sets the format pattern for negative numeric values.</summary>
		/// <returns>The format pattern for negative numeric values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 1, which represents "-n", where <paramref name="n" /> is a number.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 4. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int NumberNegativePattern
		{
			get
			{
				return this.numberNegativePattern;
			}
			set
			{
				if (value < 0 || value > 4)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 15");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.numberNegativePattern = value;
			}
		}

		/// <summary>Gets or sets the number of decimal places to use in percent values. </summary>
		/// <returns>The number of decimal places to use in percent values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 2.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 99. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int PercentDecimalDigits
		{
			get
			{
				return this.percentDecimalDigits;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 99");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.percentDecimalDigits = value;
			}
		}

		/// <summary>Gets or sets the string to use as the decimal separator in percent values. </summary>
		/// <returns>The string to use as the decimal separator in percent values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is ".".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to an empty string.</exception>
		public string PercentDecimalSeparator
		{
			get
			{
				return this.percentDecimalSeparator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.percentDecimalSeparator = value;
			}
		}

		/// <summary>Gets or sets the string that separates groups of digits to the left of the decimal in percent values. </summary>
		/// <returns>The string that separates groups of digits to the left of the decimal in percent values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is ",".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string PercentGroupSeparator
		{
			get
			{
				return this.percentGroupSeparator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.percentGroupSeparator = value;
			}
		}

		/// <summary>Gets or sets the number of digits in each group to the left of the decimal in percent values. </summary>
		/// <returns>The number of digits in each group to the left of the decimal in percent values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is a one-dimensional array with only one element, which is set to 3.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set and the array contains an entry that is less than 0 or greater than 9.-or- The property is being set and the array contains an entry, other than the last entry, that is set to 0. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int[] PercentGroupSizes
		{
			get
			{
				return (int[])this.RawPercentGroupSizes.Clone();
			}
			set
			{
				this.RawPercentGroupSizes = value;
			}
		}

		internal int[] RawPercentGroupSizes
		{
			get
			{
				return this.percentGroupSizes;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				if (this == CultureInfo.CurrentCulture.NumberFormat)
				{
					throw new Exception("HERE the value was modified");
				}
				if (value.Length == 0)
				{
					this.percentGroupSizes = new int[0];
					return;
				}
				int num = value.Length - 1;
				for (int i = 0; i < num; i++)
				{
					if (value[i] < 1 || value[i] > 9)
					{
						throw new ArgumentOutOfRangeException("One of the elements in the array specified is not between 1 and 9");
					}
				}
				if (value[num] < 0 || value[num] > 9)
				{
					throw new ArgumentOutOfRangeException("Last element in the array specified is not between 0 and 9");
				}
				this.percentGroupSizes = (int[])value.Clone();
			}
		}

		/// <summary>Gets or sets the format pattern for negative percent values.</summary>
		/// <returns>The format pattern for negative percent values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 0, which represents "-n %", where "%" is the <see cref="P:System.Globalization.NumberFormatInfo.PercentSymbol" /> and <paramref name="n" /> is a number.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 11. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int PercentNegativePattern
		{
			get
			{
				return this.percentNegativePattern;
			}
			set
			{
				if (value < 0 || value > 2)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 15");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.percentNegativePattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for positive percent values.</summary>
		/// <returns>The format pattern for positive percent values. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is 0, which represents "n %", where "%" is the <see cref="P:System.Globalization.NumberFormatInfo.PercentSymbol" /> and <paramref name="n" /> is a number.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is less than 0 or greater than 3. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public int PercentPositivePattern
		{
			get
			{
				return this.percentPositivePattern;
			}
			set
			{
				if (value < 0 || value > 2)
				{
					throw new ArgumentOutOfRangeException("The value specified for the property is less than 0 or greater than 3");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.percentPositivePattern = value;
			}
		}

		/// <summary>Gets or sets the string to use as the percent symbol.</summary>
		/// <returns>The string to use as the percent symbol. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "%".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string PercentSymbol
		{
			get
			{
				return this.percentSymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.percentSymbol = value;
			}
		}

		/// <summary>Gets or sets the string to use as the per mille symbol.</summary>
		/// <returns>The string to use as the per mille symbol. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "‰", which is the Unicode character U+2030.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string PerMilleSymbol
		{
			get
			{
				return this.perMilleSymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.perMilleSymbol = value;
			}
		}

		/// <summary>Gets or sets the string that represents positive infinity.</summary>
		/// <returns>The string that represents positive infinity. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "Infinity".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string PositiveInfinitySymbol
		{
			get
			{
				return this.positiveInfinitySymbol;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.positiveInfinitySymbol = value;
			}
		}

		/// <summary>Gets or sets the string that denotes that the associated number is positive.</summary>
		/// <returns>The string that denotes that the associated number is positive. The default for <see cref="P:System.Globalization.NumberFormatInfo.InvariantInfo" /> is "+".</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.NumberFormatInfo" /> is read-only. </exception>
		public string PositiveSign
		{
			get
			{
				return this.positiveSign;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("The value specified for the property is a null reference");
				}
				if (this.isReadOnly)
				{
					throw new InvalidOperationException("The current instance is read-only and a set operation was attempted");
				}
				this.positiveSign = value;
			}
		}

		/// <summary>Gets an object of the specified type that provides a number formatting service.</summary>
		/// <returns>The current <see cref="T:System.Globalization.NumberFormatInfo" />, if <paramref name="formatType" /> is the same as the type of the current <see cref="T:System.Globalization.NumberFormatInfo" />; otherwise, null.</returns>
		/// <param name="formatType">The <see cref="T:System.Type" /> of the required formatting service. </param>
		public object GetFormat(Type formatType)
		{
			return (formatType != typeof(NumberFormatInfo)) ? null : this;
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Globalization.NumberFormatInfo" />.</summary>
		/// <returns>A new <see cref="T:System.Globalization.NumberFormatInfo" /> copied from the original <see cref="T:System.Globalization.NumberFormatInfo" />.</returns>
		public object Clone()
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)base.MemberwiseClone();
			numberFormatInfo.isReadOnly = false;
			return numberFormatInfo;
		}

		/// <summary>Returns a read-only <see cref="T:System.Globalization.NumberFormatInfo" /> wrapper.</summary>
		/// <returns>A read-only <see cref="T:System.Globalization.NumberFormatInfo" /> wrapper around <paramref name="nfi" />.</returns>
		/// <param name="nfi">The <see cref="T:System.Globalization.NumberFormatInfo" /> to wrap. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="nfi" /> is null. </exception>
		public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi)
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)nfi.Clone();
			numberFormatInfo.isReadOnly = true;
			return numberFormatInfo;
		}

		/// <summary>Gets the <see cref="T:System.Globalization.NumberFormatInfo" /> associated with the specified <see cref="T:System.IFormatProvider" />.</summary>
		/// <returns>The <see cref="T:System.Globalization.NumberFormatInfo" /> associated with the specified <see cref="T:System.IFormatProvider" />.</returns>
		/// <param name="formatProvider">The <see cref="T:System.IFormatProvider" /> used to get the <see cref="T:System.Globalization.NumberFormatInfo" />.-or- null to get <see cref="P:System.Globalization.NumberFormatInfo.CurrentInfo" />. </param>
		public static NumberFormatInfo GetInstance(IFormatProvider formatProvider)
		{
			if (formatProvider != null)
			{
				NumberFormatInfo numberFormatInfo = (NumberFormatInfo)formatProvider.GetFormat(typeof(NumberFormatInfo));
				if (numberFormatInfo != null)
				{
					return numberFormatInfo;
				}
			}
			return NumberFormatInfo.CurrentInfo;
		}
	}
}
