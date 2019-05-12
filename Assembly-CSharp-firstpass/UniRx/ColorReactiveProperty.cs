using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class ColorReactiveProperty : ReactiveProperty<Color>
	{
		public ColorReactiveProperty()
		{
		}

		public ColorReactiveProperty(Color initialValue) : base(initialValue)
		{
		}

		protected override IEqualityComparer<Color> EqualityComparer
		{
			get
			{
				return UnityEqualityComparer.Color;
			}
		}
	}
}
