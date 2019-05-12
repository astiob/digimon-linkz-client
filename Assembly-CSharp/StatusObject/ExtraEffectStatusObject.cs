using System;
using UnityEngine;

namespace StatusObject
{
	public class ExtraEffectStatusObject : ScriptableObject
	{
		[SerializeField]
		private ExtraEffectStatus _extraEffectStatus;

		public ExtraEffectStatusObject(ExtraEffectStatus item)
		{
			this._extraEffectStatus = item;
		}

		public ExtraEffectStatus extraEffectStatus
		{
			get
			{
				return this._extraEffectStatus;
			}
		}
	}
}
