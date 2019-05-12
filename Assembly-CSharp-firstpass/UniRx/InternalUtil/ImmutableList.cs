using System;

namespace UniRx.InternalUtil
{
	public class ImmutableList<T>
	{
		public static readonly ImmutableList<T> Empty = new ImmutableList<T>();

		private T[] data;

		private ImmutableList()
		{
			this.data = new T[0];
		}

		public ImmutableList(T[] data)
		{
			this.data = data;
		}

		public T[] Data
		{
			get
			{
				return this.data;
			}
		}

		public ImmutableList<T> Add(T value)
		{
			T[] array = new T[this.data.Length + 1];
			Array.Copy(this.data, array, this.data.Length);
			array[this.data.Length] = value;
			return new ImmutableList<T>(array);
		}

		public ImmutableList<T> Remove(T value)
		{
			int num = this.IndexOf(value);
			if (num < 0)
			{
				return this;
			}
			int num2 = this.data.Length;
			if (num2 == 1)
			{
				return ImmutableList<T>.Empty;
			}
			T[] destinationArray = new T[num2 - 1];
			Array.Copy(this.data, 0, destinationArray, 0, num);
			Array.Copy(this.data, num + 1, destinationArray, num, num2 - num - 1);
			return new ImmutableList<T>(destinationArray);
		}

		public int IndexOf(T value)
		{
			for (int i = 0; i < this.data.Length; i++)
			{
				if (object.Equals(this.data[i], value))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
