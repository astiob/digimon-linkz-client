using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[StaticAccessor("GetPhysicsManager2D()", StaticAccessorType.Arrow)]
	[NativeHeader("Modules/Physics2D/PhysicsManager2D.h")]
	public class Physics2D
	{
		public const int IgnoreRaycastLayer = 4;

		public const int DefaultRaycastLayers = -5;

		public const int AllLayers = -1;

		private static List<Rigidbody2D> m_LastDisabledRigidbody2D = new List<Rigidbody2D>();

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.Linecast(start, end, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.Linecast(start, end, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.Linecast(start, end, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			RaycastHit2D result;
			Physics2D.Internal_Linecast(start, end, contactFilter, out result);
			return result;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.LinecastAll(start, end, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.LinecastAll(start, end, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.LinecastAll(start, end, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_LinecastAll(start, end, contactFilter);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.LinecastNonAlloc(start, end, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.LinecastNonAlloc(start, end, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.LinecastNonAlloc(start, end, results, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_LinecastNonAlloc(start, end, contactFilter, results);
		}

		public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.Internal_LinecastNonAlloc(start, end, contactFilter, results);
		}

		private static void Internal_Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_Linecast(ref start, ref end, ref contactFilter, out raycastHit);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_Linecast(ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_LinecastAll(Vector2 start, Vector2 end, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_LinecastAll(ref start, ref end, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_LinecastAll(ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter);

		private static int Internal_LinecastNonAlloc(Vector2 start, Vector2 end, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_LinecastNonAlloc(ref start, ref end, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_LinecastNonAlloc(ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		[RequiredByNativeCode]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.Raycast(origin, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.Raycast(origin, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.Raycast(origin, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.Raycast(origin, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			RaycastHit2D result;
			Physics2D.Internal_Raycast(origin, direction, distance, contactFilter, out result);
			return result;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.RaycastAll(origin, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.RaycastAll(origin, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.RaycastAll(origin, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.RaycastAll(origin, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_RaycastAll(origin, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.RaycastNonAlloc(origin, direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_RaycastNonAlloc(origin, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.Raycast(origin, direction, contactFilter, results, positiveInfinity);
		}

		public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.Internal_RaycastNonAlloc(origin, direction, distance, contactFilter, results);
		}

		private static void Internal_Raycast(Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_Raycast(ref origin, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_Raycast(ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_RaycastAll(Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_RaycastAll(ref origin, ref direction, distance, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_RaycastAll(ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_RaycastNonAlloc(Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_RaycastNonAlloc(ref origin, ref direction, distance, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_RaycastNonAlloc(ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CircleCast(origin, radius, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.CircleCast(origin, radius, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.CircleCast(origin, radius, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.CircleCast(origin, radius, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			RaycastHit2D result;
			Physics2D.Internal_CircleCast(origin, radius, direction, distance, contactFilter, out result);
			return result;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.CircleCastAll(origin, radius, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_CircleCastAll(origin, radius, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.CircleCastNonAlloc(origin, radius, direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_CircleCastNonAlloc(origin, radius, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CircleCast(origin, radius, direction, contactFilter, results, positiveInfinity);
		}

		public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.Internal_CircleCastNonAlloc(origin, radius, direction, distance, contactFilter, results);
		}

		private static void Internal_CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_CircleCast(ref origin, radius, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_CircleCast(ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_CircleCastAll(ref origin, radius, ref direction, distance, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_CircleCastAll(ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_CircleCastNonAlloc(ref origin, radius, ref direction, distance, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_CircleCastNonAlloc(ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.BoxCast(origin, size, angle, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			RaycastHit2D result;
			Physics2D.Internal_BoxCast(origin, size, angle, direction, distance, contactFilter, out result);
			return result;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.BoxCastAll(origin, size, angle, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_BoxCastAll(origin, size, angle, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_BoxCastNonAlloc(origin, size, angle, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.BoxCast(origin, size, angle, direction, contactFilter, results, positiveInfinity);
		}

		public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.Internal_BoxCastNonAlloc(origin, size, angle, direction, distance, contactFilter, results);
		}

		private static void Internal_BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_BoxCast(ref origin, ref size, angle, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_BoxCast(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_BoxCastAll(ref origin, ref size, angle, ref direction, distance, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_BoxCastAll(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, distance, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_BoxCastNonAlloc(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			RaycastHit2D result;
			Physics2D.Internal_CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, out result);
			return result;
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, contactFilter);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
		}

		[ExcludeFromDocs]
		public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, contactFilter, results, positiveInfinity);
		}

		public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return Physics2D.Internal_CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
		}

		private static void Internal_CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_CapsuleCast(ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter, out raycastHit);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_CapsuleCast(ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, out RaycastHit2D raycastHit);

		private static RaycastHit2D[] Internal_CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_CapsuleCastAll(ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_Internal_CapsuleCastAll(ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

		private static int Internal_CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_CapsuleCastNonAlloc(ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_CapsuleCastNonAlloc(ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		private static void Internal_GetRayIntersection(Ray ray, float distance, int layerMask, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_GetRayIntersection(ref ray, distance, layerMask, out raycastHit);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_GetRayIntersection(ref Ray ray, float distance, int layerMask, out RaycastHit2D raycastHit);

		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray, float distance)
		{
			int layerMask = -5;
			return Physics2D.GetRayIntersection(ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D GetRayIntersection(Ray ray)
		{
			int layerMask = -5;
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.GetRayIntersection(ray, positiveInfinity, layerMask);
		}

		public static RaycastHit2D GetRayIntersection(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			RaycastHit2D result;
			Physics2D.Internal_GetRayIntersection(ray, distance, layerMask, out result);
			return result;
		}

		[RequiredByNativeCode]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return Physics2D.INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, float distance)
		{
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_GetRayIntersectionAll(ref ray, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] GetRayIntersectionAll(Ray ray)
		{
			int layerMask = -5;
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_GetRayIntersectionAll(ref ray, positiveInfinity, layerMask);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_GetRayIntersectionAll(ref Ray ray, float distance, int layerMask);

		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			return Physics2D.INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, float distance)
		{
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, distance, layerMask);
		}

		[ExcludeFromDocs]
		public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results)
		{
			int layerMask = -5;
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_GetRayIntersectionNonAlloc(ref ray, results, positiveInfinity, layerMask);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetRayIntersectionNonAlloc(ref Ray ray, RaycastHit2D[] results, float distance, int layerMask);

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapPoint(point, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapPoint(point, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapPoint(point, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D OverlapPoint(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapPoint(point, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapPointAll(point, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapPointAll(point, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapPointAll(point, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D[] OverlapPointAll(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapPointAll(point, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapPointNonAlloc(point, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapPointNonAlloc(point, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapPointNonAlloc(point, results, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapPointNonAlloc(point, contactFilter, results);
		}

		public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.Internal_OverlapPointNonAlloc(point, contactFilter, results);
		}

		private static Collider2D Internal_OverlapPoint(Vector2 point, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapPoint(ref point, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapPoint(ref Vector2 point, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapPointAll(Vector2 point, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapPointAll(ref point, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapPointAll(ref Vector2 point, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapPointNonAlloc(Vector2 point, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapPointNonAlloc(ref point, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_OverlapPointNonAlloc(ref Vector2 point, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapCircle(point, radius, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapCircle(point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapCircle(point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D OverlapCircle(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapCircle(point, radius, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapCircleAll(point, radius, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapCircleAll(point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapCircleAll(point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapCircleAll(point, radius, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapCircleNonAlloc(point, radius, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapCircleNonAlloc(point, radius, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapCircleNonAlloc(point, radius, results, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapCircleNonAlloc(point, radius, contactFilter, results);
		}

		public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.Internal_OverlapCircleNonAlloc(point, radius, contactFilter, results);
		}

		private static Collider2D Internal_OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapCircle(ref point, radius, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapCircle(ref Vector2 point, float radius, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapCircleAll(Vector2 point, float radius, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapCircleAll(ref point, radius, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapCircleAll(ref Vector2 point, float radius, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapCircleNonAlloc(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapCircleNonAlloc(ref point, radius, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_OverlapCircleNonAlloc(ref Vector2 point, float radius, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapBox(point, size, angle, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapBox(point, size, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapBox(point, size, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapBox(point, size, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapBoxAll(point, size, angle, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapBoxAll(point, size, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapBoxAll(point, size, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapBoxAll(point, size, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapBoxNonAlloc(point, size, angle, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapBoxNonAlloc(point, size, angle, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapBoxNonAlloc(point, size, angle, results, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapBoxNonAlloc(point, size, angle, contactFilter, results);
		}

		public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.Internal_OverlapBoxNonAlloc(point, size, angle, contactFilter, results);
		}

		private static Collider2D Internal_OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapBox(ref point, ref size, angle, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapBox(ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapBoxAll(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapBoxAll(ref point, ref size, angle, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapBoxAll(ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapBoxNonAlloc(ref point, ref size, angle, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_OverlapBoxNonAlloc(ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapArea(pointA, pointB, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapArea(pointA, pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapArea(pointA, pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapArea(pointA, pointB, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapAreaAll(pointA, pointB, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapAreaAll(pointA, pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapAreaAll(pointA, pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapAreaAll(pointA, pointB, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapAreaNonAlloc(pointA, pointB, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapAreaNonAlloc(pointA, pointB, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapAreaNonAlloc(pointA, pointB, results, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapAreaNonAlloc(pointA, pointB, contactFilter, results);
		}

		public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.Internal_OverlapAreaNonAlloc(pointA, pointB, contactFilter, results);
		}

		private static Collider2D Internal_OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapArea(ref pointA, ref pointB, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapArea(ref Vector2 pointA, ref Vector2 pointB, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapAreaAll(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapAreaAll(ref pointA, ref pointB, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapAreaAll(ref Vector2 pointA, ref Vector2 pointB, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapAreaNonAlloc(ref pointA, ref pointB, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_OverlapAreaNonAlloc(ref Vector2 pointA, ref Vector2 pointB, ref ContactFilter2D contactFilter, Collider2D[] results);

		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapCapsule(point, size, direction, angle, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapCapsule(point, size, direction, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapCapsule(point, size, direction, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapCapsule(point, size, direction, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapCapsuleAll(point, size, direction, angle, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapCapsuleAll(point, size, direction, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapCapsuleAll(point, size, direction, angle, layerMask, negativeInfinity, positiveInfinity);
		}

		public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapCapsuleAll(point, size, direction, angle, contactFilter);
		}

		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, negativeInfinity, positiveInfinity);
		}

		public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return Physics2D.Internal_OverlapCapsuleNonAlloc(point, size, direction, angle, contactFilter, results);
		}

		public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.Internal_OverlapCapsuleNonAlloc(point, size, direction, angle, contactFilter, results);
		}

		private static Collider2D Internal_OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapCapsule(ref point, ref size, direction, angle, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_Internal_OverlapCapsule(ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter);

		private static Collider2D[] Internal_OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapCapsuleAll(ref point, ref size, direction, angle, ref contactFilter);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_Internal_OverlapCapsuleAll(ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter);

		private static int Internal_OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_Internal_OverlapCapsuleNonAlloc(ref point, ref size, direction, angle, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_OverlapCapsuleNonAlloc(ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter, Collider2D[] results);

		public static int OverlapCollider(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_OverlapCollider(collider, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_OverlapCollider(Collider2D collider, ref ContactFilter2D contactFilter, Collider2D[] results);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Rigidbody2D GetRigidbodyFromInstanceID(int instanceID);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Collider2D GetColliderFromInstanceID(int instanceID);

		private static int GetColliderContacts(Collider2D collider, ContactFilter2D contactFilter, ContactPoint2D[] results)
		{
			return Physics2D.INTERNAL_CALL_GetColliderContacts(collider, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetColliderContacts(Collider2D collider, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		private static int GetColliderColliderContacts(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter, ContactPoint2D[] results)
		{
			return Physics2D.INTERNAL_CALL_GetColliderColliderContacts(collider1, collider2, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetColliderColliderContacts(Collider2D collider1, Collider2D collider2, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		private static int GetRigidbodyContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, ContactPoint2D[] results)
		{
			return Physics2D.INTERNAL_CALL_GetRigidbodyContacts(rigidbody, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetRigidbodyContacts(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

		private static int GetColliderContactsCollidersOnly(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_GetColliderContactsCollidersOnly(collider, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetColliderContactsCollidersOnly(Collider2D collider, ref ContactFilter2D contactFilter, Collider2D[] results);

		private static int GetRigidbodyContactsCollidersOnly(Rigidbody2D rigidbody, ContactFilter2D contactFilter, Collider2D[] results)
		{
			return Physics2D.INTERNAL_CALL_GetRigidbodyContactsCollidersOnly(rigidbody, ref contactFilter, results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetRigidbodyContactsCollidersOnly(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, Collider2D[] results);

		public static int GetContacts(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetColliderColliderContacts(collider1, collider2, contactFilter, contacts);
		}

		public static int GetContacts(Collider2D collider, ContactPoint2D[] contacts)
		{
			return Physics2D.GetColliderContacts(collider, default(ContactFilter2D).NoFilter(), contacts);
		}

		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetColliderContacts(collider, contactFilter, contacts);
		}

		public static int GetContacts(Collider2D collider, Collider2D[] colliders)
		{
			return Physics2D.GetColliderContactsCollidersOnly(collider, default(ContactFilter2D).NoFilter(), colliders);
		}

		public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetColliderContactsCollidersOnly(collider, contactFilter, colliders);
		}

		public static int GetContacts(Rigidbody2D rigidbody, ContactPoint2D[] contacts)
		{
			return Physics2D.GetRigidbodyContacts(rigidbody, default(ContactFilter2D).NoFilter(), contacts);
		}

		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetRigidbodyContacts(rigidbody, contactFilter, contacts);
		}

		public static int GetContacts(Rigidbody2D rigidbody, Collider2D[] colliders)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnly(rigidbody, default(ContactFilter2D).NoFilter(), colliders);
		}

		public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetRigidbodyContactsCollidersOnly(rigidbody, contactFilter, colliders);
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern int velocityIterations { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern int positionIterations { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static Vector2 gravity
		{
			get
			{
				Vector2 result;
				Physics2D.get_gravity_Injected(out result);
				return result;
			}
			set
			{
				Physics2D.set_gravity_Injected(ref value);
			}
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool queriesHitTriggers { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool queriesStartInColliders { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool changeStopsCallbacks { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool callbacksOnDisable { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool autoSyncTransforms { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool autoSimulation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float velocityThreshold { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxLinearCorrection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxAngularCorrection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxTranslationSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float maxRotationSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float defaultContactOffset { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float baumgarteScale { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float baumgarteTOIScale { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float timeToSleep { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float linearSleepTolerance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float angularSleepTolerance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool alwaysShowColliders { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool showColliderSleep { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool showColliderContacts { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern bool showColliderAABB { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static extern float contactArrowScale { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[StaticAccessor("GetPhysics2DSettings()")]
		public static Color colliderAwakeColor
		{
			get
			{
				Color result;
				Physics2D.get_colliderAwakeColor_Injected(out result);
				return result;
			}
			set
			{
				Physics2D.set_colliderAwakeColor_Injected(ref value);
			}
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		public static Color colliderAsleepColor
		{
			get
			{
				Color result;
				Physics2D.get_colliderAsleepColor_Injected(out result);
				return result;
			}
			set
			{
				Physics2D.set_colliderAsleepColor_Injected(ref value);
			}
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		public static Color colliderContactColor
		{
			get
			{
				Color result;
				Physics2D.get_colliderContactColor_Injected(out result);
				return result;
			}
			set
			{
				Physics2D.set_colliderContactColor_Injected(ref value);
			}
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		public static Color colliderAABBColor
		{
			get
			{
				Color result;
				Physics2D.get_colliderAABBColor_Injected(out result);
				return result;
			}
			set
			{
				Physics2D.set_colliderAABBColor_Injected(ref value);
			}
		}

		[NativeMethod("Simulate_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Simulate(float step);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SyncTransforms();

		public static void IgnoreCollision([Writable] Collider2D collider1, [Writable] Collider2D collider2)
		{
			Physics2D.IgnoreCollision(collider1, collider2, true);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IgnoreCollision([NotNull, Writable] Collider2D collider1, [NotNull, Writable] Collider2D collider2, [DefaultValue("true")] bool ignore);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetIgnoreCollision([Writable] Collider2D collider1, [Writable] Collider2D collider2);

		public static void IgnoreLayerCollision(int layer1, int layer2)
		{
			Physics2D.IgnoreLayerCollision(layer1, layer2, true);
		}

		public static void IgnoreLayerCollision(int layer1, int layer2, bool ignore)
		{
			if (layer1 < 0 || layer1 > 31)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			if (layer2 < 0 || layer2 > 31)
			{
				throw new ArgumentOutOfRangeException("layer2 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			Physics2D.IgnoreLayerCollision_Internal(layer1, layer2, ignore);
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("IgnoreLayerCollision")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void IgnoreLayerCollision_Internal(int layer1, int layer2, bool ignore);

		public static bool GetIgnoreLayerCollision(int layer1, int layer2)
		{
			if (layer1 < 0 || layer1 > 31)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			if (layer2 < 0 || layer2 > 31)
			{
				throw new ArgumentOutOfRangeException("layer2 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			return Physics2D.GetIgnoreLayerCollision_Internal(layer1, layer2);
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("GetIgnoreLayerCollision")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIgnoreLayerCollision_Internal(int layer1, int layer2);

		public static void SetLayerCollisionMask(int layer, int layerMask)
		{
			if (layer < 0 || layer > 31)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			Physics2D.SetLayerCollisionMask_Internal(layer, layerMask);
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("SetLayerCollisionMask")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLayerCollisionMask_Internal(int layer, int layerMask);

		public static int GetLayerCollisionMask(int layer)
		{
			if (layer < 0 || layer > 31)
			{
				throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
			}
			return Physics2D.GetLayerCollisionMask_Internal(layer);
		}

		[StaticAccessor("GetPhysics2DSettings()")]
		[NativeMethod("GetLayerCollisionMask")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetLayerCollisionMask_Internal(int layer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTouching([Writable, NotNull] Collider2D collider1, [NotNull, Writable] Collider2D collider2);

		public static bool IsTouching([Writable] Collider2D collider1, [Writable] Collider2D collider2, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_TwoCollidersWithFilter(collider1, collider2, contactFilter);
		}

		[NativeMethod("IsTouching")]
		private static bool IsTouching_TwoCollidersWithFilter([NotNull, Writable] Collider2D collider1, [NotNull, Writable] Collider2D collider2, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_TwoCollidersWithFilter_Injected(collider1, collider2, ref contactFilter);
		}

		public static bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_SingleColliderWithFilter(collider, contactFilter);
		}

		[NativeMethod("IsTouching")]
		private static bool IsTouching_SingleColliderWithFilter([Writable, NotNull] Collider2D collider, ContactFilter2D contactFilter)
		{
			return Physics2D.IsTouching_SingleColliderWithFilter_Injected(collider, ref contactFilter);
		}

		public static bool IsTouchingLayers([Writable] Collider2D collider)
		{
			return Physics2D.IsTouchingLayers(collider, -1);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTouchingLayers([Writable, NotNull] Collider2D collider, [DefaultValue("Physics2D.AllLayers")] int layerMask);

		public static ColliderDistance2D Distance([Writable] Collider2D colliderA, [Writable] Collider2D colliderB)
		{
			if (colliderA == null)
			{
				throw new ArgumentNullException("ColliderA cannot be NULL.");
			}
			if (colliderB == null)
			{
				throw new ArgumentNullException("ColliderB cannot be NULL.");
			}
			if (colliderA == colliderB)
			{
				throw new ArgumentException("Cannot calculate the distance between the same collider.");
			}
			return Physics2D.Distance_Internal(colliderA, colliderB);
		}

		[NativeMethod("Distance")]
		[StaticAccessor("GetPhysicsQuery2D()", StaticAccessorType.Arrow)]
		private static ColliderDistance2D Distance_Internal([Writable, NotNull] Collider2D colliderA, [NotNull, Writable] Collider2D colliderB)
		{
			ColliderDistance2D result;
			Physics2D.Distance_Internal_Injected(colliderA, colliderB, out result);
			return result;
		}

		internal static void SetEditorDragMovement(bool dragging, GameObject[] objs)
		{
			foreach (Rigidbody2D rigidbody2D in Physics2D.m_LastDisabledRigidbody2D)
			{
				if (rigidbody2D != null)
				{
					rigidbody2D.SetDragBehaviour(false);
				}
			}
			Physics2D.m_LastDisabledRigidbody2D.Clear();
			if (dragging)
			{
				foreach (GameObject gameObject in objs)
				{
					Rigidbody2D[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody2D>(false);
					foreach (Rigidbody2D rigidbody2D2 in componentsInChildren)
					{
						Physics2D.m_LastDisabledRigidbody2D.Add(rigidbody2D2);
						rigidbody2D2.SetDragBehaviour(true);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_gravity_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_gravity_Injected(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_colliderAwakeColor_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_colliderAwakeColor_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_colliderAsleepColor_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_colliderAsleepColor_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_colliderContactColor_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_colliderContactColor_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_colliderAABBColor_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_colliderAABBColor_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsTouching_TwoCollidersWithFilter_Injected([Writable] Collider2D collider1, [Writable] Collider2D collider2, ref ContactFilter2D contactFilter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsTouching_SingleColliderWithFilter_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Distance_Internal_Injected([Writable] Collider2D colliderA, [Writable] Collider2D colliderB, out ColliderDistance2D ret);
	}
}
