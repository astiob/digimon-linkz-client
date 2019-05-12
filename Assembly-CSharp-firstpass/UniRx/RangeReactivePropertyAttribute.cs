using System;
using UnityEngine;

namespace UniRx
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class RangeReactivePropertyAttribute : PropertyAttribute
	{
		public RangeReactivePropertyAttribute(float min, float max)
		{
			this.Min = min;
			this.Max = max;
		}

		public float Min { get; private set; }

		public float Max { get; private set; }
	}
}
