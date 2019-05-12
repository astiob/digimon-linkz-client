using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class QuaternionReactiveProperty : ReactiveProperty<Quaternion>
	{
		public QuaternionReactiveProperty()
		{
		}

		public QuaternionReactiveProperty(Quaternion initialValue) : base(initialValue)
		{
		}

		protected override IEqualityComparer<Quaternion> EqualityComparer
		{
			get
			{
				return UnityEqualityComparer.Quaternion;
			}
		}
	}
}
