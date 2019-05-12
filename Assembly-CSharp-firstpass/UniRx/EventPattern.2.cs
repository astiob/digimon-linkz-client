using System;
using System.Collections.Generic;

namespace UniRx
{
	public class EventPattern<TSender, TEventArgs> : IEquatable<EventPattern<TSender, TEventArgs>>, IEventPattern<TSender, TEventArgs>
	{
		public EventPattern(TSender sender, TEventArgs e)
		{
			this.Sender = sender;
			this.EventArgs = e;
		}

		public TSender Sender { get; private set; }

		public TEventArgs EventArgs { get; private set; }

		public bool Equals(EventPattern<TSender, TEventArgs> other)
		{
			return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (EqualityComparer<TSender>.Default.Equals(this.Sender, other.Sender) && EqualityComparer<TEventArgs>.Default.Equals(this.EventArgs, other.EventArgs)));
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as EventPattern<TSender, TEventArgs>);
		}

		public override int GetHashCode()
		{
			int hashCode = EqualityComparer<TSender>.Default.GetHashCode(this.Sender);
			int hashCode2 = EqualityComparer<TEventArgs>.Default.GetHashCode(this.EventArgs);
			return (hashCode << 5) + (hashCode ^ hashCode2);
		}

		public static bool operator ==(EventPattern<TSender, TEventArgs> first, EventPattern<TSender, TEventArgs> second)
		{
			return object.Equals(first, second);
		}

		public static bool operator !=(EventPattern<TSender, TEventArgs> first, EventPattern<TSender, TEventArgs> second)
		{
			return !object.Equals(first, second);
		}
	}
}
