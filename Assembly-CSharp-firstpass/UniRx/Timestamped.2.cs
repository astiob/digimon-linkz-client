using System;

namespace UniRx
{
	public static class Timestamped
	{
		public static Timestamped<T> Create<T>(T value, DateTimeOffset timestamp)
		{
			return new Timestamped<T>(value, timestamp);
		}
	}
}
