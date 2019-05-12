using System;
using UnityEngine.XR;
using UnityEngine.XR.Tango;

namespace UnityEngine.SpatialTracking
{
	[DefaultExecutionOrder(-30000)]
	[AddComponentMenu("XR/Tracked Pose Driver")]
	[Serializable]
	public class TrackedPoseDriver : MonoBehaviour
	{
		[SerializeField]
		private TrackedPoseDriver.DeviceType m_Device;

		[SerializeField]
		private TrackedPoseDriver.TrackedPose m_PoseSource;

		[SerializeField]
		private TrackedPoseDriver.TrackingType m_TrackingType;

		[SerializeField]
		private TrackedPoseDriver.UpdateType m_UpdateType = TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;

		[SerializeField]
		private bool m_UseRelativeTransform = true;

		protected Pose m_OriginPose;

		public TrackedPoseDriver.DeviceType deviceType
		{
			get
			{
				return this.m_Device;
			}
			internal set
			{
				this.m_Device = value;
			}
		}

		public TrackedPoseDriver.TrackedPose poseSource
		{
			get
			{
				return this.m_PoseSource;
			}
			internal set
			{
				this.m_PoseSource = value;
			}
		}

		public bool SetPoseSource(TrackedPoseDriver.DeviceType deviceType, TrackedPoseDriver.TrackedPose pose)
		{
			if (deviceType < (TrackedPoseDriver.DeviceType)TrackedPoseDriverDataDescription.DeviceData.Count)
			{
				TrackedPoseDriverDataDescription.PoseData poseData = TrackedPoseDriverDataDescription.DeviceData[(int)deviceType];
				for (int i = 0; i < poseData.Poses.Count; i++)
				{
					if (poseData.Poses[i] == pose)
					{
						this.poseSource = pose;
						return true;
					}
				}
			}
			return false;
		}

		private bool TryGetTangoPose(CoordinateFrame frame, out Pose pose)
		{
			PoseData poseData;
			bool result;
			if (TangoInputTracking.TryGetPoseAtTime(out poseData, TangoDevice.baseCoordinateFrame, frame, 0.0) && poseData.statusCode == PoseStatus.Valid)
			{
				pose.position = poseData.position;
				pose.rotation = poseData.rotation;
				result = true;
			}
			else
			{
				pose = Pose.identity;
				result = false;
			}
			return result;
		}

		private bool GetDataFromSource(TrackedPoseDriver.DeviceType device, TrackedPoseDriver.TrackedPose poseSource, out Pose resultPose)
		{
			bool result;
			switch (poseSource)
			{
			case TrackedPoseDriver.TrackedPose.LeftEye:
				resultPose.position = InputTracking.GetLocalPosition(XRNode.LeftEye);
				resultPose.rotation = InputTracking.GetLocalRotation(XRNode.LeftEye);
				result = true;
				break;
			case TrackedPoseDriver.TrackedPose.RightEye:
				resultPose.position = InputTracking.GetLocalPosition(XRNode.RightEye);
				resultPose.rotation = InputTracking.GetLocalRotation(XRNode.RightEye);
				result = true;
				break;
			case TrackedPoseDriver.TrackedPose.Center:
				resultPose.position = InputTracking.GetLocalPosition(XRNode.CenterEye);
				resultPose.rotation = InputTracking.GetLocalRotation(XRNode.CenterEye);
				result = true;
				break;
			case TrackedPoseDriver.TrackedPose.Head:
				resultPose.position = InputTracking.GetLocalPosition(XRNode.Head);
				resultPose.rotation = InputTracking.GetLocalRotation(XRNode.Head);
				result = true;
				break;
			case TrackedPoseDriver.TrackedPose.LeftPose:
				resultPose.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
				resultPose.rotation = InputTracking.GetLocalRotation(XRNode.LeftHand);
				result = true;
				break;
			case TrackedPoseDriver.TrackedPose.RightPose:
				resultPose.position = InputTracking.GetLocalPosition(XRNode.RightHand);
				resultPose.rotation = InputTracking.GetLocalRotation(XRNode.RightHand);
				result = true;
				break;
			case TrackedPoseDriver.TrackedPose.ColorCamera:
				result = this.TryGetTangoPose(CoordinateFrame.CameraColor, out resultPose);
				break;
			case TrackedPoseDriver.TrackedPose.DepthCamera:
				result = this.TryGetTangoPose(CoordinateFrame.CameraDepth, out resultPose);
				break;
			case TrackedPoseDriver.TrackedPose.FisheyeCamera:
				result = this.TryGetTangoPose(CoordinateFrame.CameraFisheye, out resultPose);
				break;
			case TrackedPoseDriver.TrackedPose.Device:
				result = this.TryGetTangoPose(CoordinateFrame.Device, out resultPose);
				break;
			case TrackedPoseDriver.TrackedPose.RemotePose:
				resultPose.position = InputTracking.GetLocalPosition(XRNode.RightHand);
				resultPose.rotation = InputTracking.GetLocalRotation(XRNode.RightHand);
				result = true;
				break;
			default:
				resultPose = Pose.identity;
				result = false;
				break;
			}
			return result;
		}

		public TrackedPoseDriver.TrackingType trackingType
		{
			get
			{
				return this.m_TrackingType;
			}
			set
			{
				this.m_TrackingType = value;
			}
		}

		public TrackedPoseDriver.UpdateType updateType
		{
			get
			{
				return this.m_UpdateType;
			}
			set
			{
				this.m_UpdateType = value;
			}
		}

		public bool UseRelativeTransform
		{
			get
			{
				return this.m_UseRelativeTransform;
			}
			set
			{
				this.m_UseRelativeTransform = value;
			}
		}

		public Pose originPose
		{
			get
			{
				return this.m_OriginPose;
			}
			set
			{
				this.m_OriginPose = value;
			}
		}

		private void CacheLocalPosition()
		{
			this.m_OriginPose.position = base.transform.localPosition;
			this.m_OriginPose.rotation = base.transform.localRotation;
		}

		private void ResetToCachedLocalPosition()
		{
			this.SetLocalTransform(this.m_OriginPose.position, this.m_OriginPose.rotation);
		}

		protected virtual void Awake()
		{
			this.CacheLocalPosition();
			if (this.HasStereoCamera())
			{
				XRDevice.DisableAutoXRCameraTracking(base.GetComponent<Camera>(), true);
			}
		}

		protected virtual void OnDestroy()
		{
			if (this.HasStereoCamera())
			{
			}
		}

		protected virtual void OnEnable()
		{
			Application.onBeforeRender += this.OnBeforeRender;
		}

		protected virtual void OnDisable()
		{
			this.ResetToCachedLocalPosition();
			Application.onBeforeRender -= this.OnBeforeRender;
		}

		protected virtual void FixedUpdate()
		{
			if (this.m_UpdateType == TrackedPoseDriver.UpdateType.Update || this.m_UpdateType == TrackedPoseDriver.UpdateType.UpdateAndBeforeRender)
			{
				this.PerformUpdate();
			}
		}

		protected virtual void Update()
		{
			if (this.m_UpdateType == TrackedPoseDriver.UpdateType.Update || this.m_UpdateType == TrackedPoseDriver.UpdateType.UpdateAndBeforeRender)
			{
				this.PerformUpdate();
			}
		}

		protected virtual void OnBeforeRender()
		{
			if (this.m_UpdateType == TrackedPoseDriver.UpdateType.BeforeRender || this.m_UpdateType == TrackedPoseDriver.UpdateType.UpdateAndBeforeRender)
			{
				this.PerformUpdate();
			}
		}

		protected virtual void SetLocalTransform(Vector3 newPosition, Quaternion newRotation)
		{
			if (this.m_TrackingType == TrackedPoseDriver.TrackingType.RotationAndPosition || this.m_TrackingType == TrackedPoseDriver.TrackingType.RotationOnly)
			{
				base.transform.localRotation = newRotation;
			}
			if (this.m_TrackingType == TrackedPoseDriver.TrackingType.RotationAndPosition || this.m_TrackingType == TrackedPoseDriver.TrackingType.PositionOnly)
			{
				base.transform.localPosition = newPosition;
			}
		}

		protected Pose TransformPoseByOriginIfNeeded(Pose pose)
		{
			Pose result;
			if (this.m_UseRelativeTransform)
			{
				result = pose.GetTransformedBy(this.m_OriginPose);
			}
			else
			{
				result = pose;
			}
			return result;
		}

		private bool HasStereoCamera()
		{
			Camera component = base.GetComponent<Camera>();
			return component != null && component.stereoEnabled;
		}

		protected virtual void PerformUpdate()
		{
			if (base.enabled)
			{
				Pose pose = default(Pose);
				if (this.GetDataFromSource(this.m_Device, this.m_PoseSource, out pose))
				{
					Pose pose2 = this.TransformPoseByOriginIfNeeded(pose);
					this.SetLocalTransform(pose2.position, pose2.rotation);
				}
			}
		}

		public enum DeviceType
		{
			GenericXRDevice,
			GenericXRController,
			GenericXRRemote
		}

		public enum TrackedPose
		{
			LeftEye,
			RightEye,
			Center,
			Head,
			LeftPose,
			RightPose,
			ColorCamera,
			DepthCamera,
			FisheyeCamera,
			Device,
			RemotePose
		}

		public enum TrackingType
		{
			RotationAndPosition,
			RotationOnly,
			PositionOnly
		}

		public enum UpdateType
		{
			UpdateAndBeforeRender,
			Update,
			BeforeRender
		}
	}
}
