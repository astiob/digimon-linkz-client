using System;

namespace Firebase.Platform.Default
{
	internal class SystemClock : IClockService
	{
		public static readonly IClockService Instance = new SystemClock();

		protected SystemClock()
		{
		}

		public DateTime Now
		{
			get
			{
				return DateTime.Now;
			}
		}

		public DateTime UtcNow
		{
			get
			{
				return DateTime.UtcNow;
			}
		}
	}
}
