using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class Vector3ReactiveProperty : ReactiveProperty<Vector3>
	{
		public Vector3ReactiveProperty()
		{
		}

		public Vector3ReactiveProperty(Vector3 initialValue) : base(initialValue)
		{
		}

		protected override IEqualityComparer<Vector3> EqualityComparer
		{
			get
			{
				return UnityEqualityComparer.Vector3;
			}
		}
	}
}
