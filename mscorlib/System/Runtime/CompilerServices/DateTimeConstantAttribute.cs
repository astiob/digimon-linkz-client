using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Persists an 8-byte <see cref="T:System.DateTime" /> constant for a field or parameter.</summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DateTimeConstantAttribute : CustomConstantAttribute
	{
		private long ticks;

		/// <summary>Initializes a new instance of the DateTimeConstantAttribute class with the number of 100-nanosecond ticks that represent the date and time of this instance.</summary>
		/// <param name="ticks">The number of 100-nanosecond ticks that represent the date and time of this instance. </param>
		public DateTimeConstantAttribute(long ticks)
		{
			this.ticks = ticks;
		}

		internal long Ticks
		{
			get
			{
				return this.ticks;
			}
		}

		/// <summary>Gets the number of 100-nanosecond ticks that represent the date and time of this instance.</summary>
		/// <returns>The number of 100-nanosecond ticks that represent the date and time of this instance.</returns>
		public override object Value
		{
			get
			{
				return this.ticks;
			}
		}
	}
}
