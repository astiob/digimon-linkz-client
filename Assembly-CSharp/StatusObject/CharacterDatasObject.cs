using System;
using UnityEngine;

namespace StatusObject
{
	public class CharacterDatasObject : ScriptableObject
	{
		[SerializeField]
		private CharacterDatas _characterDatas;

		public CharacterDatas characterDatas
		{
			get
			{
				return this._characterDatas;
			}
		}
	}
}
