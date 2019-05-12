using System;

namespace UnityEngine.CameraParams.Internal
{
	[AddComponentMenu("Digimon Effects/Tools/Camera Target Index Selector")]
	[DisallowMultipleComponent]
	public sealed class CameraTargetIndexSelector : MonoBehaviour
	{
		[SerializeField]
		private float _cameraTargetIndex;

		public int cameraTargetIndex
		{
			get
			{
				return Mathf.RoundToInt(this._cameraTargetIndex);
			}
		}
	}
}
