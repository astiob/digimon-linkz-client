using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Details about a specific point of contact involved in a 2D physics collision.</para>
	/// </summary>
	public struct ContactPoint2D
	{
		internal Vector2 m_Point;

		internal Vector2 m_Normal;

		internal Collider2D m_Collider;

		internal Collider2D m_OtherCollider;

		/// <summary>
		///   <para>The point of contact between the two colliders in world space.</para>
		/// </summary>
		public Vector2 point
		{
			get
			{
				return this.m_Point;
			}
		}

		/// <summary>
		///   <para>Surface normal at the contact point.</para>
		/// </summary>
		public Vector2 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		/// <summary>
		///   <para>The collider attached to the object receiving the collision message.</para>
		/// </summary>
		public Collider2D collider
		{
			get
			{
				return this.m_Collider;
			}
		}

		/// <summary>
		///   <para>The incoming collider involved in the collision at this contact point.</para>
		/// </summary>
		public Collider2D otherCollider
		{
			get
			{
				return this.m_OtherCollider;
			}
		}
	}
}
