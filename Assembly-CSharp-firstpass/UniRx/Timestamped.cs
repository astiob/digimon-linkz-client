using System;
using System.Collections.Generic;
using System.Globalization;

namespace UniRx
{
	[Serializable]
	public struct Timestamped<T> : IEquatable<Timestamped<T>>
	{
		private readonly DateTimeOffset _timestamp;

		private readonly T _value;

		public Timestamped(T value, DateTimeOffset timestamp)
		{
			this._timestamp = timestamp;
			this._value = value;
		}

		public T Value
		{
			get
			{
				return this._value;
			}
		}

		public DateTimeOffset Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		public bool Equals(Timestamped<T> other)
		{
			return other.Timestamp.Equals(this.Timestamp) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
		}

		public static bool operator ==(Timestamped<T> first, Timestamped<T> second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(Timestamped<T> first, Timestamped<T> second)
		{
			return !first.Equals(second);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Timestamped<T>))
			{
				return false;
			}
			Timestamped<T> other = (Timestamped<T>)obj;
			return this.Equals(other);
		}

		public override int GetHashCode()
		{
			int num;
			if (this.Value == null)
			{
				num = 1979;
			}
			else
			{
				T value = this.Value;
				num = value.GetHashCode();
			}
			int num2 = num;
			return this._timestamp.GetHashCode() ^ num2;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}@{1}", new object[]
			{
				this.Value,
				this.Timestamp
			});
		}
	}
}
