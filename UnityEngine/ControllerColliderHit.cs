using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>ControllerColliderHit is used by CharacterController.OnControllerColliderHit to give detailed information about the collision and how to deal with it.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class ControllerColliderHit
	{
		internal CharacterController m_Controller;

		internal Collider m_Collider;

		internal Vector3 m_Point;

		internal Vector3 m_Normal;

		internal Vector3 m_MoveDirection;

		internal float m_MoveLength;

		internal int m_Push;

		/// <summary>
		///   <para>The controller that hit the collider.</para>
		/// </summary>
		public CharacterController controller
		{
			get
			{
				return this.m_Controller;
			}
		}

		/// <summary>
		///   <para>The collider that was hit by the controller.</para>
		/// </summary>
		public Collider collider
		{
			get
			{
				return this.m_Collider;
			}
		}

		/// <summary>
		///   <para>The rigidbody that was hit by the controller.</para>
		/// </summary>
		public Rigidbody rigidbody
		{
			get
			{
				return this.m_Collider.attachedRigidbody;
			}
		}

		/// <summary>
		///   <para>The game object that was hit by the controller.</para>
		/// </summary>
		public GameObject gameObject
		{
			get
			{
				return this.m_Collider.gameObject;
			}
		}

		/// <summary>
		///   <para>The transform that was hit by the controller.</para>
		/// </summary>
		public Transform transform
		{
			get
			{
				return this.m_Collider.transform;
			}
		}

		/// <summary>
		///   <para>The impact point in world space.</para>
		/// </summary>
		public Vector3 point
		{
			get
			{
				return this.m_Point;
			}
		}

		/// <summary>
		///   <para>The normal of the surface we collided with in world space.</para>
		/// </summary>
		public Vector3 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		/// <summary>
		///   <para>The direction the CharacterController was moving in when the collision occured.</para>
		/// </summary>
		public Vector3 moveDirection
		{
			get
			{
				return this.m_MoveDirection;
			}
		}

		/// <summary>
		///   <para>How far the character has travelled until it hit the collider.</para>
		/// </summary>
		public float moveLength
		{
			get
			{
				return this.m_MoveLength;
			}
		}

		private bool push
		{
			get
			{
				return this.m_Push != 0;
			}
			set
			{
				this.m_Push = ((!value) ? 0 : 1);
			}
		}
	}
}
