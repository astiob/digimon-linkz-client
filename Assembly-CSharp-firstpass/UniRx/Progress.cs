using System;

namespace UniRx
{
	public class Progress<T> : IProgress<T>
	{
		private readonly Action<T> report;

		public Progress(Action<T> report)
		{
			this.report = report;
		}

		public void Report(T value)
		{
			this.report(value);
		}
	}
}
