using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes a contact point where the collision occurs.</para>
	/// </summary>
	public struct ContactPoint
	{
		internal Vector3 m_Point;

		internal Vector3 m_Normal;

		internal int m_ThisColliderInstanceID;

		internal int m_OtherColliderInstanceID;

		/// <summary>
		///   <para>The point of contact.</para>
		/// </summary>
		public Vector3 point
		{
			get
			{
				return this.m_Point;
			}
		}

		/// <summary>
		///   <para>Normal of the contact point.</para>
		/// </summary>
		public Vector3 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		/// <summary>
		///   <para>The first collider in contact at the point.</para>
		/// </summary>
		public Collider thisCollider
		{
			get
			{
				return ContactPoint.ColliderFromInstanceId(this.m_ThisColliderInstanceID);
			}
		}

		/// <summary>
		///   <para>The other collider in contact at the point.</para>
		/// </summary>
		public Collider otherCollider
		{
			get
			{
				return ContactPoint.ColliderFromInstanceId(this.m_OtherColliderInstanceID);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider ColliderFromInstanceId(int instanceID);
	}
}
