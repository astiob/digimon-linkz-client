using System;

namespace UniRx
{
	public class EventPattern<TEventArgs> : EventPattern<object, TEventArgs>
	{
		public EventPattern(object sender, TEventArgs e) : base(sender, e)
		{
		}
	}
}
