using System;
using System.Collections.Generic;

namespace System.Linq
{
	internal class QuickSort<TElement>
	{
		private TElement[] elements;

		private int[] indexes;

		private SortContext<TElement> context;

		private QuickSort(IEnumerable<TElement> source, SortContext<TElement> context)
		{
			this.elements = source.ToArray<TElement>();
			this.indexes = QuickSort<TElement>.CreateIndexes(this.elements.Length);
			this.context = context;
		}

		private static int[] CreateIndexes(int length)
		{
			int[] array = new int[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = i;
			}
			return array;
		}

		private void PerformSort()
		{
			if (this.elements.Length <= 1)
			{
				return;
			}
			this.context.Initialize(this.elements);
			this.Sort(0, this.indexes.Length - 1);
		}

		private int CompareItems(int first_index, int second_index)
		{
			return this.context.Compare(first_index, second_index);
		}

		private int MedianOfThree(int left, int right)
		{
			int num = (left + right) / 2;
			if (this.CompareItems(this.indexes[num], this.indexes[left]) < 0)
			{
				this.Swap(left, num);
			}
			if (this.CompareItems(this.indexes[right], this.indexes[left]) < 0)
			{
				this.Swap(left, right);
			}
			if (this.CompareItems(this.indexes[right], this.indexes[num]) < 0)
			{
				this.Swap(num, right);
			}
			this.Swap(num, right - 1);
			return this.indexes[right - 1];
		}

		private void Sort(int left, int right)
		{
			if (left + 3 <= right)
			{
				int num = left;
				int num2 = right - 1;
				int second_index = this.MedianOfThree(left, right);
				for (;;)
				{
					while (this.CompareItems(this.indexes[++num], second_index) < 0)
					{
					}
					while (this.CompareItems(this.indexes[--num2], second_index) > 0)
					{
					}
					if (num >= num2)
					{
						break;
					}
					this.Swap(num, num2);
				}
				this.Swap(num, right - 1);
				this.Sort(left, num - 1);
				this.Sort(num + 1, right);
			}
			else
			{
				this.InsertionSort(left, right);
			}
		}

		private void InsertionSort(int left, int right)
		{
			for (int i = left + 1; i <= right; i++)
			{
				int num = this.indexes[i];
				int num2 = i;
				while (num2 > left && this.CompareItems(num, this.indexes[num2 - 1]) < 0)
				{
					this.indexes[num2] = this.indexes[num2 - 1];
					num2--;
				}
				this.indexes[num2] = num;
			}
		}

		private void Swap(int left, int right)
		{
			int num = this.indexes[right];
			this.indexes[right] = this.indexes[left];
			this.indexes[left] = num;
		}

		public static IEnumerable<TElement> Sort(IEnumerable<TElement> source, SortContext<TElement> context)
		{
			QuickSort<TElement> sorter = new QuickSort<TElement>(source, context);
			sorter.PerformSort();
			for (int i = 0; i < sorter.indexes.Length; i++)
			{
				yield return sorter.elements[sorter.indexes[i]];
			}
			yield break;
		}
	}
}
