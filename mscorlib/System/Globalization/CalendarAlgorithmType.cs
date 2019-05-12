using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Specifies whether a calendar is solar-based, lunar-based, or lunisolar-based.</summary>
	[ComVisible(true)]
	public enum CalendarAlgorithmType
	{
		/// <summary>An unknown calendar basis.</summary>
		Unknown,
		/// <summary>A solar-based calendar.</summary>
		SolarCalendar,
		/// <summary>A lunar-based calendar.</summary>
		LunarCalendar,
		/// <summary>A lunisolar-based calendar.</summary>
		LunisolarCalendar
	}
}
