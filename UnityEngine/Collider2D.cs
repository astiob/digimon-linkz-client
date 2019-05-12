using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Parent class for collider types used with 2D gameplay.</para>
	/// </summary>
	public class Collider2D : Behaviour
	{
		/// <summary>
		///   <para>Is this collider configured as a trigger?</para>
		/// </summary>
		public extern bool isTrigger { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Whether the collider is used by an attached effector or not.</para>
		/// </summary>
		public extern bool usedByEffector { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The local offset of the collider geometry.</para>
		/// </summary>
		public Vector2 offset
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_offset(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_offset(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_offset(out Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_offset(ref Vector2 value);

		/// <summary>
		///   <para>The Rigidbody2D attached to the Collider2D's GameObject.</para>
		/// </summary>
		public extern Rigidbody2D attachedRigidbody { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of separate shaped regions in the collider.</para>
		/// </summary>
		public extern int shapeCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The world space bounding area of the collider.</para>
		/// </summary>
		public Bounds bounds
		{
			get
			{
				Bounds result;
				this.INTERNAL_get_bounds(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_bounds(out Bounds value);

		internal extern ColliderErrorState2D errorState { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Check if a collider overlaps a point in space.</para>
		/// </summary>
		/// <param name="point">A point in world space.</param>
		public bool OverlapPoint(Vector2 point)
		{
			return Collider2D.INTERNAL_CALL_OverlapPoint(this, ref point);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_OverlapPoint(Collider2D self, ref Vector2 point);

		/// <summary>
		///   <para>The PhysicsMaterial2D that is applied to this collider.</para>
		/// </summary>
		public extern PhysicsMaterial2D sharedMaterial { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Check whether this collider is touching the collider or not.</para>
		/// </summary>
		/// <param name="collider">The collider to check if it is touching this collider.</param>
		/// <returns>
		///   <para>Whether the collider is touching this collider or not.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouching(Collider2D collider);

		/// <summary>
		///   <para>Checks whether this collider is touching any colliders on the specified layerMask or not.</para>
		/// </summary>
		/// <param name="layerMask">Any colliders on any of these layers count as touching.</param>
		/// <returns>
		///   <para>Whether this collider is touching any collider on the specified layerMask or not.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

		[ExcludeFromDocs]
		public bool IsTouchingLayers()
		{
			int layerMask = -1;
			return this.IsTouchingLayers(layerMask);
		}
	}
}
