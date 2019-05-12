using System;

namespace TMPro
{
	public struct TMP_XmlTagStack<T>
	{
		public T[] itemStack;

		public int index;

		public TMP_XmlTagStack(T[] tagStack)
		{
			this.itemStack = tagStack;
			this.index = 0;
		}

		public void Clear()
		{
			this.index = 0;
		}

		public void SetDefault(T item)
		{
			this.itemStack[0] = item;
			this.index = 1;
		}

		public void Add(T item)
		{
			if (this.index < this.itemStack.Length)
			{
				this.itemStack[this.index] = item;
				this.index++;
			}
		}

		public T Remove()
		{
			this.index--;
			if (this.index <= 0)
			{
				this.index = 1;
				return this.itemStack[0];
			}
			return this.itemStack[this.index - 1];
		}

		public T CurrentItem()
		{
			if (this.index > 0)
			{
				return this.itemStack[this.index - 1];
			}
			return this.itemStack[0];
		}

		public T PreviousItem()
		{
			if (this.index > 1)
			{
				return this.itemStack[this.index - 2];
			}
			return this.itemStack[0];
		}
	}
}
