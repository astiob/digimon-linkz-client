using System;

namespace Firebase.Platform
{
	internal interface IClockService
	{
		DateTime Now { get; }

		DateTime UtcNow { get; }
	}
}
