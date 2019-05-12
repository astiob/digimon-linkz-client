using System;

namespace UnityEngine.Networking
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/NetworkStartPosition")]
	public class NetworkStartPosition : MonoBehaviour
	{
		public void Awake()
		{
			NetworkManager.RegisterStartPosition(base.transform);
		}

		public void OnDestroy()
		{
			NetworkManager.UnRegisterStartPosition(base.transform);
		}
	}
}
