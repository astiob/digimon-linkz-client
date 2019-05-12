using System;

namespace UniRx
{
	public static class Notification
	{
		public static Notification<T> CreateOnNext<T>(T value)
		{
			return new Notification<T>.OnNextNotification(value);
		}

		public static Notification<T> CreateOnError<T>(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			return new Notification<T>.OnErrorNotification(error);
		}

		public static Notification<T> CreateOnCompleted<T>()
		{
			return new Notification<T>.OnCompletedNotification();
		}
	}
}
