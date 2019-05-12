using System;
using UnityEngine;

namespace Facility
{
	public class FarmFacilityAnimationData : ScriptableObject
	{
		public FarmFacilityAnimationData.FacilityAnimationInfo[] animation;

		[Serializable]
		public class FacilityAnimationInfo
		{
			public FacilityAnimationID animeId;

			public int stateHash;
		}
	}
}
