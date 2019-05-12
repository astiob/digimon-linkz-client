using System;
using UnityEngine;

namespace UniRx
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class MultilineReactivePropertyAttribute : PropertyAttribute
	{
		public MultilineReactivePropertyAttribute()
		{
			this.Lines = 3;
		}

		public MultilineReactivePropertyAttribute(int lines)
		{
			this.Lines = lines;
		}

		public int Lines { get; private set; }
	}
}
