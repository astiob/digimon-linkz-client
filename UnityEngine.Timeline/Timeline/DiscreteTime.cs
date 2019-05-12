using System;

namespace UnityEngine.Timeline
{
	internal struct DiscreteTime : IComparable
	{
		private const double k_Tick = 1E-12;

		public static readonly DiscreteTime kMaxTime = new DiscreteTime(long.MaxValue);

		private readonly long m_DiscreteTime;

		public DiscreteTime(DiscreteTime time)
		{
			this.m_DiscreteTime = time.m_DiscreteTime;
		}

		private DiscreteTime(long time)
		{
			this.m_DiscreteTime = time;
		}

		public DiscreteTime(double time)
		{
			this.m_DiscreteTime = DiscreteTime.DoubleToDiscreteTime(time);
		}

		public DiscreteTime(float time)
		{
			this.m_DiscreteTime = DiscreteTime.FloatToDiscreteTime(time);
		}

		public DiscreteTime(int time)
		{
			this.m_DiscreteTime = DiscreteTime.IntToDiscreteTime(time);
		}

		public DiscreteTime(int frame, double fps)
		{
			this.m_DiscreteTime = DiscreteTime.DoubleToDiscreteTime((double)frame * fps);
		}

		public static double tickValue
		{
			get
			{
				return 1E-12;
			}
		}

		public DiscreteTime OneTickBefore()
		{
			return new DiscreteTime(this.m_DiscreteTime - 1L);
		}

		public DiscreteTime OneTickAfter()
		{
			return new DiscreteTime(this.m_DiscreteTime + 1L);
		}

		public long GetTick()
		{
			return this.m_DiscreteTime;
		}

		public int CompareTo(object obj)
		{
			int result;
			if (obj is DiscreteTime)
			{
				result = this.m_DiscreteTime.CompareTo(((DiscreteTime)obj).m_DiscreteTime);
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public bool Equals(DiscreteTime other)
		{
			return this.m_DiscreteTime == other.m_DiscreteTime;
		}

		public override bool Equals(object obj)
		{
			return obj is DiscreteTime && this.Equals((DiscreteTime)obj);
		}

		private static long DoubleToDiscreteTime(double time)
		{
			double num = time / 1E-12 + 0.5;
			if (num < 9.2233720368547758E+18 && num > -9.2233720368547758E+18)
			{
				return (long)num;
			}
			throw new ArgumentOutOfRangeException("Time is over the discrete range.");
		}

		private static long FloatToDiscreteTime(float time)
		{
			float num = time / 1E-12f + 0.5f;
			if (num < 9.223372E+18f && num > -9.223372E+18f)
			{
				return (long)num;
			}
			throw new ArgumentOutOfRangeException("Time is over the discrete range.");
		}

		private static long IntToDiscreteTime(int time)
		{
			return DiscreteTime.DoubleToDiscreteTime((double)time);
		}

		private static double ToDouble(long time)
		{
			return (double)time * 1E-12;
		}

		private static float ToFloat(long time)
		{
			return (float)DiscreteTime.ToDouble(time);
		}

		public static explicit operator double(DiscreteTime b)
		{
			return DiscreteTime.ToDouble(b.m_DiscreteTime);
		}

		public static explicit operator float(DiscreteTime b)
		{
			return DiscreteTime.ToFloat(b.m_DiscreteTime);
		}

		public static explicit operator long(DiscreteTime b)
		{
			return b.m_DiscreteTime;
		}

		public static explicit operator DiscreteTime(double time)
		{
			return new DiscreteTime(time);
		}

		public static explicit operator DiscreteTime(float time)
		{
			return new DiscreteTime(time);
		}

		public static implicit operator DiscreteTime(int time)
		{
			return new DiscreteTime(time);
		}

		public static explicit operator DiscreteTime(long time)
		{
			return new DiscreteTime(time);
		}

		public static bool operator ==(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime == rhs.m_DiscreteTime;
		}

		public static bool operator !=(DiscreteTime lhs, DiscreteTime rhs)
		{
			return !(lhs == rhs);
		}

		public static bool operator >(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime > rhs.m_DiscreteTime;
		}

		public static bool operator <(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime < rhs.m_DiscreteTime;
		}

		public static bool operator <=(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime <= rhs.m_DiscreteTime;
		}

		public static bool operator >=(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime >= rhs.m_DiscreteTime;
		}

		public static DiscreteTime operator +(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(lhs.m_DiscreteTime + rhs.m_DiscreteTime);
		}

		public static DiscreteTime operator -(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(lhs.m_DiscreteTime - rhs.m_DiscreteTime);
		}

		public override string ToString()
		{
			return this.m_DiscreteTime.ToString();
		}

		public override int GetHashCode()
		{
			return this.m_DiscreteTime.GetHashCode();
		}

		public static DiscreteTime Min(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(Math.Min(lhs.m_DiscreteTime, rhs.m_DiscreteTime));
		}

		public static DiscreteTime Max(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(Math.Max(lhs.m_DiscreteTime, rhs.m_DiscreteTime));
		}

		public static double SnapToNearestTick(double time)
		{
			long time2 = DiscreteTime.DoubleToDiscreteTime(time);
			return DiscreteTime.ToDouble(time2);
		}

		public static float SnapToNearestTick(float time)
		{
			long time2 = DiscreteTime.FloatToDiscreteTime(time);
			return DiscreteTime.ToFloat(time2);
		}

		public static long GetNearestTick(double time)
		{
			return DiscreteTime.DoubleToDiscreteTime(time);
		}
	}
}
