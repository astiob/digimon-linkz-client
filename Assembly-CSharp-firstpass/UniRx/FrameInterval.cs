using System;
using System.Collections.Generic;
using System.Globalization;

namespace UniRx
{
	[Serializable]
	public struct FrameInterval<T> : IEquatable<FrameInterval<T>>
	{
		private readonly int _interval;

		private readonly T _value;

		public FrameInterval(T value, int interval)
		{
			this._interval = interval;
			this._value = value;
		}

		public T Value
		{
			get
			{
				return this._value;
			}
		}

		public int Interval
		{
			get
			{
				return this._interval;
			}
		}

		public bool Equals(FrameInterval<T> other)
		{
			return other.Interval.Equals(this.Interval) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
		}

		public static bool operator ==(FrameInterval<T> first, FrameInterval<T> second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(FrameInterval<T> first, FrameInterval<T> second)
		{
			return !first.Equals(second);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is FrameInterval<T>))
			{
				return false;
			}
			FrameInterval<T> other = (FrameInterval<T>)obj;
			return this.Equals(other);
		}

		public override int GetHashCode()
		{
			int num;
			if (this.Value == null)
			{
				num = 1963;
			}
			else
			{
				T value = this.Value;
				num = value.GetHashCode();
			}
			int num2 = num;
			return this.Interval.GetHashCode() ^ num2;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}@{1}", new object[]
			{
				this.Value,
				this.Interval
			});
		}
	}
}
