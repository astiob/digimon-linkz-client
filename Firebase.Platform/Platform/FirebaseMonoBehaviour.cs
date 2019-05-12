using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Firebase.Platform
{
	[Preserve]
	internal sealed class FirebaseMonoBehaviour : MonoBehaviour
	{
		private FirebaseHandler GetFirebaseHandlerOrDestroyGameObject()
		{
			FirebaseHandler defaultInstance = FirebaseHandler.DefaultInstance;
			if (defaultInstance == null)
			{
				Object.Destroy(base.gameObject);
			}
			return defaultInstance;
		}

		[Preserve]
		private void OnEnable()
		{
			this.GetFirebaseHandlerOrDestroyGameObject();
		}

		[Preserve]
		private void Update()
		{
			FirebaseHandler firebaseHandlerOrDestroyGameObject = this.GetFirebaseHandlerOrDestroyGameObject();
			if (firebaseHandlerOrDestroyGameObject != null)
			{
				PlatformInformation.RealtimeSinceStartupSafe = PlatformInformation.RealtimeSinceStartup;
				firebaseHandlerOrDestroyGameObject.Update();
			}
		}

		[Preserve]
		private void OnApplicationFocus(bool hasFocus)
		{
			FirebaseHandler firebaseHandlerOrDestroyGameObject = this.GetFirebaseHandlerOrDestroyGameObject();
			if (firebaseHandlerOrDestroyGameObject != null)
			{
				firebaseHandlerOrDestroyGameObject.OnApplicationFocus(hasFocus);
			}
		}

		private void OnDestroy()
		{
			FirebaseHandler.OnMonoBehaviourDestroyed(this);
		}
	}
}
