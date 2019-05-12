using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class RectReactiveProperty : ReactiveProperty<Rect>
	{
		public RectReactiveProperty()
		{
		}

		public RectReactiveProperty(Rect initialValue) : base(initialValue)
		{
		}

		protected override IEqualityComparer<Rect> EqualityComparer
		{
			get
			{
				return UnityEqualityComparer.Rect;
			}
		}
	}
}
