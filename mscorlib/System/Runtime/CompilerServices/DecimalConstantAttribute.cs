using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Stores the value of a <see cref="T:System.Decimal" /> constant in metadata. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class DecimalConstantAttribute : Attribute
	{
		private byte scale;

		private bool sign;

		private int hi;

		private int mid;

		private int low;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.DecimalConstantAttribute" /> class with the specified unsigned integer values.</summary>
		/// <param name="scale">The power of 10 scaling factor that indicates the number of digits to the right of the decimal point. Valid values are 0 through 28 inclusive. </param>
		/// <param name="sign">A value of 0 indicates a positive value, and a value of 1 indicates a negative value. </param>
		/// <param name="hi">The high 32 bits of the 96-bit <see cref="P:System.Runtime.CompilerServices.DecimalConstantAttribute.Value" />. </param>
		/// <param name="mid">The middle 32 bits of the 96-bit <see cref="P:System.Runtime.CompilerServices.DecimalConstantAttribute.Value" />. </param>
		/// <param name="low">The low 32 bits of the 96-bit <see cref="P:System.Runtime.CompilerServices.DecimalConstantAttribute.Value" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="scale" /> &gt; 28. </exception>
		[CLSCompliant(false)]
		public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
		{
			this.scale = scale;
			this.sign = Convert.ToBoolean(sign);
			this.hi = (int)hi;
			this.mid = (int)mid;
			this.low = (int)low;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.DecimalConstantAttribute" /> class with the specified signed integer values. </summary>
		/// <param name="scale">The power of 10 scaling factor that indicates the number of digits to the right of the decimal point. Valid values are 0 through 28 inclusive.</param>
		/// <param name="sign">A value of 0 indicates a positive value, and a value of 1 indicates a negative value.</param>
		/// <param name="hi">The high 32 bits of the 96-bit <see cref="P:System.Runtime.CompilerServices.DecimalConstantAttribute.Value" />.</param>
		/// <param name="mid">The middle 32 bits of the 96-bit <see cref="P:System.Runtime.CompilerServices.DecimalConstantAttribute.Value" />.</param>
		/// <param name="low">The low 32 bits of the 96-bit <see cref="P:System.Runtime.CompilerServices.DecimalConstantAttribute.Value" />.</param>
		public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
		{
			this.scale = scale;
			this.sign = Convert.ToBoolean(sign);
			this.hi = hi;
			this.mid = mid;
			this.low = low;
		}

		/// <summary>Gets the decimal constant stored in this attribute.</summary>
		/// <returns>The decimal constant stored in this attribute.</returns>
		public decimal Value
		{
			get
			{
				return new decimal(this.low, this.mid, this.hi, this.sign, this.scale);
			}
		}
	}
}
