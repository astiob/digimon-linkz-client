using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class InitialIntroductionBox
	{
		[SerializeField]
		private InitialIntroductionFace _faceType;

		[SerializeField]
		[TextArea(1, 3)]
		private string _message = string.Empty;

		public InitialIntroductionBox()
		{
		}

		public InitialIntroductionBox(int faceType, string message)
		{
			this._faceType = (InitialIntroductionFace)faceType;
			this._message = message;
		}

		public InitialIntroductionFace faceType
		{
			get
			{
				return this._faceType;
			}
		}

		public string message
		{
			get
			{
				return this._message;
			}
		}
	}
}
