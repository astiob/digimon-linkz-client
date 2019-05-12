using System;

namespace UnityEngine.CameraParams.Internal
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Digimon Effects/Tools/Camera Target Controller")]
	public sealed class CameraTargetController : MonoBehaviour
	{
		[SerializeField]
		[Range(1f, 179f)]
		private float _fieldOfView = 60f;

		[SerializeField]
		private float _cameraWorldDifference;

		public float fieldOfView
		{
			get
			{
				return this._fieldOfView;
			}
			set
			{
				this._fieldOfView = value;
			}
		}

		public float cameraWorldDifference
		{
			get
			{
				return this._cameraWorldDifference;
			}
			set
			{
				this._cameraWorldDifference = value;
			}
		}
	}
}
