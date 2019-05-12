using System;
using UnityEngine;

namespace StatusObject
{
	public class ToleranceObject : ScriptableObject
	{
		[SerializeField]
		private Tolerance _tolerance;

		public ToleranceObject(Tolerance tolerance)
		{
			this._tolerance = tolerance;
		}

		public Tolerance tolerance
		{
			get
			{
				return this._tolerance;
			}
		}
	}
}
