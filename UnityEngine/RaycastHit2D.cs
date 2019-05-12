using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Information returned about an object detected by a raycast in 2D physics.</para>
	/// </summary>
	public struct RaycastHit2D
	{
		private Vector2 m_Centroid;

		private Vector2 m_Point;

		private Vector2 m_Normal;

		private float m_Distance;

		private float m_Fraction;

		private Collider2D m_Collider;

		/// <summary>
		///   <para>The centroid of the primitive used to perform the cast.</para>
		/// </summary>
		public Vector2 centroid
		{
			get
			{
				return this.m_Centroid;
			}
			set
			{
				this.m_Centroid = value;
			}
		}

		/// <summary>
		///   <para>The point in world space where the ray hit the collider's surface.</para>
		/// </summary>
		public Vector2 point
		{
			get
			{
				return this.m_Point;
			}
			set
			{
				this.m_Point = value;
			}
		}

		/// <summary>
		///   <para>The normal vector of the surface hit by the ray.</para>
		/// </summary>
		public Vector2 normal
		{
			get
			{
				return this.m_Normal;
			}
			set
			{
				this.m_Normal = value;
			}
		}

		/// <summary>
		///   <para>The distance from the ray origin to the impact point.</para>
		/// </summary>
		public float distance
		{
			get
			{
				return this.m_Distance;
			}
			set
			{
				this.m_Distance = value;
			}
		}

		/// <summary>
		///   <para>Fraction of the distance along the ray that the hit occurred.</para>
		/// </summary>
		public float fraction
		{
			get
			{
				return this.m_Fraction;
			}
			set
			{
				this.m_Fraction = value;
			}
		}

		/// <summary>
		///   <para>The collider hit by the ray.</para>
		/// </summary>
		public Collider2D collider
		{
			get
			{
				return this.m_Collider;
			}
		}

		/// <summary>
		///   <para>The Rigidbody2D attached to the object that was hit.</para>
		/// </summary>
		public Rigidbody2D rigidbody
		{
			get
			{
				return (!(this.collider != null)) ? null : this.collider.attachedRigidbody;
			}
		}

		/// <summary>
		///   <para>The Transform of the object that was hit.</para>
		/// </summary>
		public Transform transform
		{
			get
			{
				Rigidbody2D rigidbody = this.rigidbody;
				if (rigidbody != null)
				{
					return rigidbody.transform;
				}
				if (this.collider != null)
				{
					return this.collider.transform;
				}
				return null;
			}
		}

		public int CompareTo(RaycastHit2D other)
		{
			if (this.collider == null)
			{
				return 1;
			}
			if (other.collider == null)
			{
				return -1;
			}
			return this.fraction.CompareTo(other.fraction);
		}

		public static implicit operator bool(RaycastHit2D hit)
		{
			return hit.collider != null;
		}
	}
}
