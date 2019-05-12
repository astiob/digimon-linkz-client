using System;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	[UsedByNativeCode]
	public struct XRNodeState
	{
		private XRNode m_Type;

		private AvailableTrackingData m_AvailableFields;

		private Vector3 m_Position;

		private Quaternion m_Rotation;

		private Vector3 m_Velocity;

		private Vector3 m_AngularVelocity;

		private Vector3 m_Acceleration;

		private Vector3 m_AngularAcceleration;

		private int m_Tracked;

		private ulong m_UniqueID;

		public ulong uniqueID
		{
			get
			{
				return this.m_UniqueID;
			}
			set
			{
				this.m_UniqueID = value;
			}
		}

		public XRNode nodeType
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		public bool tracked
		{
			get
			{
				return this.m_Tracked == 1;
			}
			set
			{
				this.m_Tracked = ((!value) ? 0 : 1);
			}
		}

		public Vector3 position
		{
			set
			{
				this.m_Position = value;
				this.m_AvailableFields |= AvailableTrackingData.PositionAvailable;
			}
		}

		public Quaternion rotation
		{
			set
			{
				this.m_Rotation = value;
				this.m_AvailableFields |= AvailableTrackingData.RotationAvailable;
			}
		}

		public Vector3 velocity
		{
			set
			{
				this.m_Velocity = value;
				this.m_AvailableFields |= AvailableTrackingData.VelocityAvailable;
			}
		}

		public Vector3 angularVelocity
		{
			set
			{
				this.m_AngularVelocity = value;
				this.m_AvailableFields |= AvailableTrackingData.AngularVelocityAvailable;
			}
		}

		public Vector3 acceleration
		{
			set
			{
				this.m_Acceleration = value;
				this.m_AvailableFields |= AvailableTrackingData.AccelerationAvailable;
			}
		}

		public Vector3 angularAcceleration
		{
			set
			{
				this.m_AngularAcceleration = value;
				this.m_AvailableFields |= AvailableTrackingData.AngularAccelerationAvailable;
			}
		}

		public bool TryGetPosition(out Vector3 position)
		{
			return this.TryGet<Vector3>(this.m_Position, AvailableTrackingData.PositionAvailable, out position);
		}

		public bool TryGetRotation(out Quaternion rotation)
		{
			return this.TryGet<Quaternion>(this.m_Rotation, AvailableTrackingData.RotationAvailable, out rotation);
		}

		public bool TryGetVelocity(out Vector3 velocity)
		{
			return this.TryGet<Vector3>(this.m_Velocity, AvailableTrackingData.VelocityAvailable, out velocity);
		}

		public bool TryGetAngularVelocity(out Vector3 angularVelocity)
		{
			return this.TryGet<Vector3>(this.m_AngularVelocity, AvailableTrackingData.AngularVelocityAvailable, out angularVelocity);
		}

		public bool TryGetAcceleration(out Vector3 acceleration)
		{
			return this.TryGet<Vector3>(this.m_Acceleration, AvailableTrackingData.AccelerationAvailable, out acceleration);
		}

		public bool TryGetAngularAcceleration(out Vector3 angularAcceleration)
		{
			return this.TryGet<Vector3>(this.m_AngularAcceleration, AvailableTrackingData.AngularAccelerationAvailable, out angularAcceleration);
		}

		private bool TryGet<T>(T inValue, AvailableTrackingData availabilityFlag, out T outValue) where T : new()
		{
			bool result;
			if (this.m_Tracked == 1 && (this.m_AvailableFields & availabilityFlag) > AvailableTrackingData.None)
			{
				outValue = inValue;
				result = true;
			}
			else
			{
				outValue = Activator.CreateInstance<T>();
				result = false;
			}
			return result;
		}
	}
}
