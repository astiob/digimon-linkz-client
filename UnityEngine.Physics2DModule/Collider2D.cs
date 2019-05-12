using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeHeader("Modules/Physics2D/Public/Collider2D.h")]
	[RequireComponent(typeof(Transform))]
	[RequireComponent(typeof(Transform))]
	public class Collider2D : Behaviour
	{
		public int OverlapCollider(ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.OverlapCollider(this, contactFilter, results);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			return this.Raycast(direction, contactFilter, results, positiveInfinity);
		}

		public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return this.Internal_Raycast(direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return this.Raycast(direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return this.Raycast(direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -1;
			return this.Raycast(direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -1;
			float positiveInfinity2 = float.PositiveInfinity;
			return this.Raycast(direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public int Raycast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("Physics2D.AllLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return this.Internal_Raycast(direction, distance, contactFilter, results);
		}

		private int Internal_Raycast(Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Collider2D.INTERNAL_CALL_Internal_Raycast(this, ref direction, distance, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_Raycast(Collider2D self, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance)
		{
			bool ignoreSiblingColliders = true;
			return this.Cast(direction, contactFilter, results, distance, ignoreSiblingColliders);
		}

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			bool ignoreSiblingColliders = true;
			float positiveInfinity = float.PositiveInfinity;
			return this.Cast(direction, contactFilter, results, positiveInfinity, ignoreSiblingColliders);
		}

		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
		{
			return this.Internal_Cast(direction, contactFilter, distance, ignoreSiblingColliders, results);
		}

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results, float distance)
		{
			bool ignoreSiblingColliders = true;
			return this.Cast(direction, results, distance, ignoreSiblingColliders);
		}

		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results)
		{
			bool ignoreSiblingColliders = true;
			float positiveInfinity = float.PositiveInfinity;
			return this.Cast(direction, results, positiveInfinity, ignoreSiblingColliders);
		}

		public int Cast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
		{
			ContactFilter2D contactFilter = default(ContactFilter2D);
			contactFilter.useTriggers = Physics2D.queriesHitTriggers;
			contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(base.gameObject.layer));
			return this.Internal_Cast(direction, contactFilter, distance, ignoreSiblingColliders, results);
		}

		private int Internal_Cast(Vector2 direction, ContactFilter2D contactFilter, float distance, bool ignoreSiblingColliders, RaycastHit2D[] results)
		{
			return Collider2D.INTERNAL_CALL_Internal_Cast(this, ref direction, ref contactFilter, distance, ignoreSiblingColliders, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_Cast(Collider2D self, ref Vector2 direction, ref ContactFilter2D contactFilter, float distance, bool ignoreSiblingColliders, RaycastHit2D[] results);

		public int GetContacts(ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
		}

		public int GetContacts(ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, contactFilter, contacts);
		}

		public int GetContacts(Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
		}

		public int GetContacts(ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, contactFilter, colliders);
		}

		public extern float density { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool isTrigger { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool usedByEffector { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool usedByComposite { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern CompositeCollider2D composite { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public Vector2 offset
		{
			get
			{
				Vector2 result;
				this.get_offset_Injected(out result);
				return result;
			}
			set
			{
				this.set_offset_Injected(ref value);
			}
		}

		public extern Rigidbody2D attachedRigidbody { [NativeMethod("GetAttachedRigidbody_Binding")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int shapeCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public Bounds bounds
		{
			get
			{
				Bounds result;
				this.get_bounds_Injected(out result);
				return result;
			}
		}

		internal extern ColliderErrorState2D errorState { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern bool compositeCapable { [NativeMethod("GetCompositeCapable_Binding")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern PhysicsMaterial2D sharedMaterial { [NativeMethod("GetMaterial")] [MethodImpl(MethodImplOptions.InternalCall)] get; [NativeMethod("SetMaterial")] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float friction { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float bounciness { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouching([NotNull, Writable] Collider2D collider);

		public bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return this.IsTouching_OtherColliderWithFilter(collider, contactFilter);
		}

		[NativeMethod("IsTouching")]
		private bool IsTouching_OtherColliderWithFilter([NotNull, Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return this.IsTouching_OtherColliderWithFilter_Injected(collider, ref contactFilter);
		}

		public bool IsTouching(ContactFilter2D contactFilter)
		{
			return this.IsTouching_AnyColliderWithFilter(contactFilter);
		}

		[NativeMethod("IsTouching")]
		private bool IsTouching_AnyColliderWithFilter(ContactFilter2D contactFilter)
		{
			return this.IsTouching_AnyColliderWithFilter_Injected(ref contactFilter);
		}

		public bool IsTouchingLayers()
		{
			return this.IsTouchingLayers(-1);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

		public bool OverlapPoint(Vector2 point)
		{
			return this.OverlapPoint_Injected(ref point);
		}

		public ColliderDistance2D Distance([Writable] Collider2D collider)
		{
			return Physics2D.Distance(this, collider);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_offset_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_offset_Injected(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsTouching_OtherColliderWithFilter_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsTouching_AnyColliderWithFilter_Injected(ref ContactFilter2D contactFilter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool OverlapPoint_Injected(ref Vector2 point);
	}
}
