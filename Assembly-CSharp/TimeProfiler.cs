using System;
using System.Diagnostics;

public static class TimeProfiler
{
	private const double kToMilli = 1000.0;

	private const double kToMicro = 1000000.0;

	private const double kToNano = 1000000000.0;

	private static readonly Stopwatch Stopwatch;

	private static double _frameRate = 60.0;

	private static double _cpuUseRate;

	private static double _elapseTime;

	private static double _totalElapseTime;

	private static readonly Stopwatch TotalStopwatch;

	static TimeProfiler()
	{
		TimeProfiler.Stopwatch = new Stopwatch();
		TimeProfiler.TotalStopwatch = new Stopwatch();
	}

	public static bool IsHighResolution
	{
		get
		{
			return Stopwatch.IsHighResolution;
		}
	}

	public static double FrameRate
	{
		get
		{
			return TimeProfiler._frameRate;
		}
		set
		{
			TimeProfiler._frameRate = value;
		}
	}

	public static double CpuUseRate
	{
		get
		{
			return TimeProfiler._cpuUseRate;
		}
	}

	public static double CpuUseRatePercent
	{
		get
		{
			return TimeProfiler._cpuUseRate * 100.0;
		}
	}

	public static double ElapseTime
	{
		get
		{
			return TimeProfiler._elapseTime;
		}
	}

	public static double ElapseTimeMilli
	{
		get
		{
			return TimeProfiler._elapseTime * 1000.0;
		}
	}

	public static double ElapseTimeMicro
	{
		get
		{
			return TimeProfiler._elapseTime * 1000000.0;
		}
	}

	public static double ElapseTimeNano
	{
		get
		{
			return TimeProfiler._elapseTime * 1000000000.0;
		}
	}

	public static double TotalElapseTime
	{
		get
		{
			return TimeProfiler._totalElapseTime;
		}
	}

	public static void BeginProfile()
	{
		if (TimeProfiler.Stopwatch.IsRunning)
		{
			return;
		}
		TimeProfiler.Stopwatch.Reset();
		TimeProfiler.Stopwatch.Start();
	}

	public static void EndProfile()
	{
		if (!TimeProfiler.Stopwatch.IsRunning)
		{
			return;
		}
		TimeProfiler.Stopwatch.Stop();
		long elapsedTicks = TimeProfiler.Stopwatch.ElapsedTicks;
		TimeProfiler._elapseTime = (double)elapsedTicks / (double)Stopwatch.Frequency;
		TimeProfiler._cpuUseRate = TimeProfiler._elapseTime / (1.0 / TimeProfiler._frameRate);
	}

	public static void BeginTotalProfile()
	{
		if (TimeProfiler.TotalStopwatch.IsRunning)
		{
			return;
		}
		TimeProfiler.TotalStopwatch.Reset();
		TimeProfiler.TotalStopwatch.Start();
	}

	public static void EndTotalProfile()
	{
		if (!TimeProfiler.TotalStopwatch.IsRunning)
		{
			return;
		}
		TimeProfiler.TotalStopwatch.Stop();
		long elapsedTicks = TimeProfiler.TotalStopwatch.ElapsedTicks;
		TimeProfiler._totalElapseTime = (double)elapsedTicks / (double)Stopwatch.Frequency;
	}
}
