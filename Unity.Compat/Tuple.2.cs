using System;

namespace System
{
	public class Tuple<T1, T2>
	{
		public Tuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		public T1 Item1 { get; private set; }

		public T2 Item2 { get; private set; }

		public override bool Equals(object obj)
		{
			Tuple<T1, T2> tuple = obj as Tuple<T1, T2>;
			return tuple != null && object.Equals(this.Item1, tuple.Item1) && object.Equals(this.Item2, tuple.Item2);
		}

		public override int GetHashCode()
		{
			int num;
			if (this.Item1 != null)
			{
				T1 item = this.Item1;
				num = item.GetHashCode();
			}
			else
			{
				num = 0;
			}
			int num2 = num;
			int num3;
			if (this.Item2 != null)
			{
				T2 item2 = this.Item2;
				num3 = item2.GetHashCode();
			}
			else
			{
				num3 = 0;
			}
			int num4 = num3;
			return num2 ^ num4;
		}
	}
}
