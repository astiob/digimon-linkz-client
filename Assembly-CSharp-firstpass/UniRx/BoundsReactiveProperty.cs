using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class BoundsReactiveProperty : ReactiveProperty<Bounds>
	{
		public BoundsReactiveProperty()
		{
		}

		public BoundsReactiveProperty(Bounds initialValue) : base(initialValue)
		{
		}

		protected override IEqualityComparer<Bounds> EqualityComparer
		{
			get
			{
				return UnityEqualityComparer.Bounds;
			}
		}
	}
}
