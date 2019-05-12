using System;
using UnityEngine;

namespace AdventureScene
{
	public sealed class AdventureCamera
	{
		public Camera camera3D;

		private Animator cameraAnimator;

		private Transform adventureSceneRootTransform;

		private Func<bool> actionCameraAnimation;

		private CameraFieldOfViewAnimation fieldOfViewAnimation;

		private CameraMoveAnimation moveAnimation;

		private CameraRotationAnimation rotationAnimation;

		private AdventureCameraAnimation cameraAnimation;

		public void SetComponent(Camera cam, Animator anim, Transform rootObj)
		{
			this.camera3D = cam;
			this.cameraAnimator = anim;
			this.adventureSceneRootTransform = rootObj;
		}

		public void Reset()
		{
			this.cameraAnimator.transform.parent = this.adventureSceneRootTransform;
			this.fieldOfViewAnimation = null;
			this.moveAnimation = null;
			this.rotationAnimation = null;
			this.cameraAnimation = null;
			this.actionCameraAnimation = null;
			if (null != this.cameraAnimator)
			{
				this.cameraAnimator.runtimeAnimatorController = null;
			}
		}

		public void SetAnimator(RuntimeAnimatorController animator)
		{
			this.cameraAnimator.runtimeAnimatorController = animator;
		}

		public void SetTargetChara(GameObject chara, bool isFollowingFlag)
		{
			Transform transform = this.cameraAnimator.transform;
			if (isFollowingFlag)
			{
				transform.parent = chara.transform;
			}
			else
			{
				transform.parent = this.adventureSceneRootTransform;
				transform.position = chara.transform.position;
				transform.rotation = chara.transform.rotation;
			}
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			this.camera3D.transform.LookAt(this.cameraAnimator.transform);
		}

		public bool SetTargetCharaLocator(GameObject chara, string locatorName, bool isFollowingFlag)
		{
			bool flag = AdventureObject.SetLocator(this.cameraAnimator.transform, ClassSingleton<AdventureSceneData>.Instance.stage.transform, locatorName, isFollowingFlag);
			if (flag)
			{
				if (!isFollowingFlag)
				{
					this.cameraAnimator.transform.parent = this.adventureSceneRootTransform;
				}
				this.camera3D.transform.LookAt(this.cameraAnimator.transform);
			}
			return flag;
		}

		public bool SetTargetStageLocator(string locatorName, bool isFollowingFlag)
		{
			bool flag = AdventureObject.SetLocator(this.cameraAnimator.transform, ClassSingleton<AdventureSceneData>.Instance.stage.transform, locatorName, isFollowingFlag);
			if (flag)
			{
				if (!isFollowingFlag)
				{
					this.cameraAnimator.transform.parent = this.adventureSceneRootTransform;
				}
				this.camera3D.transform.LookAt(this.cameraAnimator.transform);
			}
			return flag;
		}

		public void SetLookAt(Vector3 lookAtStageLocalPosition, Vector3 cameraStageLocalPosition)
		{
			this.cameraAnimator.transform.parent = this.adventureSceneRootTransform;
			Vector3 position = ClassSingleton<AdventureSceneData>.Instance.stage.transform.position;
			this.cameraAnimator.transform.position = position + lookAtStageLocalPosition;
			this.cameraAnimator.transform.localRotation = Quaternion.identity;
			this.camera3D.transform.position = position + cameraStageLocalPosition;
			this.camera3D.transform.LookAt(this.cameraAnimator.transform);
		}

		public void StartAnimation(string startStateName, string endStateName)
		{
			this.cameraAnimation = new AdventureCameraAnimation(this.cameraAnimator, startStateName, endStateName);
			this.actionCameraAnimation = new Func<bool>(this.cameraAnimation.UpdateAnimation);
		}

		public void StopAnimation()
		{
			this.cameraAnimator.transform.parent = this.adventureSceneRootTransform;
			if (null != this.cameraAnimator)
			{
				this.cameraAnimator.runtimeAnimatorController = null;
			}
		}

		public void SetFieldOfView(float fov, float time)
		{
			this.fieldOfViewAnimation = new CameraFieldOfViewAnimation(ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.fieldOfView, fov, time);
			this.actionCameraAnimation = new Func<bool>(this.fieldOfViewAnimation.UpdateFOV);
		}

		public void SetMove(Vector3 stageLocalPosition, float time)
		{
			this.moveAnimation = new CameraMoveAnimation(ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.transform.localPosition, stageLocalPosition, time);
			this.actionCameraAnimation = new Func<bool>(this.moveAnimation.UpdateMove);
		}

		public void SetRotation(Vector3 rotationEulerAngles, bool moveLocator, float time)
		{
			Transform transform = ClassSingleton<AdventureSceneData>.Instance.adventureCamera.camera3D.transform;
			Vector3 endAngles = transform.localEulerAngles + rotationEulerAngles;
			this.rotationAnimation = new CameraRotationAnimation(transform.localEulerAngles, endAngles, time);
			this.actionCameraAnimation = new Func<bool>(this.rotationAnimation.UpdateRotation);
		}

		public bool UpdateAnimation()
		{
			if (this.actionCameraAnimation != null)
			{
				bool flag = this.actionCameraAnimation();
				if (!flag)
				{
					return true;
				}
				this.cameraAnimator.transform.parent = this.adventureSceneRootTransform;
				this.actionCameraAnimation = null;
			}
			return false;
		}
	}
}
