using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Global settings and helpers for 2D physics.</para>
	/// </summary>
	public class Physics2D
	{
		/// <summary>
		///   <para>Layer mask constant for the default layer that ignores raycasts.</para>
		/// </summary>
		public const int IgnoreRaycastLayer = 4;

		/// <summary>
		///   <para>Layer mask constant that includes all layers participating in raycasts by default.</para>
		/// </summary>
		public const int DefaultRaycastLayers = -5;

		/// <summary>
		///   <para>Layer mask constant that includes all layers.</para>
		/// </summary>
		public const int AllLayers = -1;

		private static List<Rigidbody2D> m_LastDisabledRigidbody2D = new List<Rigidbody2D>();

		/// <summary>
		///   <para>The number of iterations of the physics solver when considering objects' velocities.</para>
		/// </summary>
		public static extern int velocityIterations { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The number of iterations of the physics solver when considering objects' positions.</para>
		/// </summary>
		public static extern int positionIterations { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Acceleration due to gravity.</para>
		/// </summary>
		public static Vector2 gravity
		{
			get
			{
				Vector2 result;
				Physics2D.INTERNAL_get_gravity(out result);
				return result;
			}
			set
			{
				Physics2D.INTERNAL_set_gravity(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_gravity(out Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_gravity(ref Vector2 value);

		/// <summary>
		///   <para>Do raycasts detect Colliders configured as triggers?</para>
		/// </summary>
		public static extern bool queriesHitTriggers { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Do ray/line casts that start inside a collider(s) detect those collider(s)?</para>
		/// </summary>
		public static extern bool queriesStartInColliders { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Whether or not to stop reporting collision callbacks immediately if any of the objects involved in the collision are deleted/moved. </para>
		/// </summary>
		public static extern bool changeStopsCallbacks { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Any collisions with a relative linear velocity below this threshold will be treated as inelastic.</para>
		/// </summary>
		public static extern float velocityThreshold { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum linear position correction used when solving constraints.  This helps to prevent overshoot.</para>
		/// </summary>
		public static extern float maxLinearCorrection { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum angular position correction used when solving constraints.  This helps to prevent overshoot.</para>
		/// </summary>
		public static extern float maxAngularCorrection { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum linear speed of a rigid-body per physics update.  Increasing this can cause numerical problems.</para>
		/// </summary>
		public static extern float maxTranslationSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum angular speed of a rigid-body per physics update.  Increasing this can cause numerical problems.</para>
		/// </summary>
		public static extern float maxRotationSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The minimum contact penetration radius allowed before any separation impulse force is applied.  Extreme caution should be used when modifying this value as making this smaller means that polygons will have an insufficient buffer for continuous collision and making it larger may create artefacts for vertex collision.</para>
		/// </summary>
		public static extern float minPenetrationForPenalty { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The scale factor that controls how fast overlaps are resolved.</para>
		/// </summary>
		public static extern float baumgarteScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The scale factor that controls how fast TOI overlaps are resolved.</para>
		/// </summary>
		public static extern float baumgarteTOIScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The time in seconds that a rigid-body must be still before it will go to sleep.</para>
		/// </summary>
		public static extern float timeToSleep { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>A rigid-body cannot sleep if its linear velocity is above this tolerance.</para>
		/// </summary>
		public static extern float linearSleepTolerance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>A rigid-body cannot sleep if its angular velocity is above this tolerance.</para>
		/// </summary>
		public static extern float angularSleepTolerance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Makes the collision detection system ignore all collisionstriggers between collider1 and collider2/.</para>
		/// </summary>
		/// <param name="collider1">The first collider to compare to collider2.</param>
		/// <param name="collider2">The second collider to compare to collider1.</param>
		/// <param name="ignore">Whether collisionstriggers between collider1 and collider2/ should be ignored or not.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IgnoreCollision(Collider2D collider1, Collider2D collider2, [DefaultValue("true")] bool ignore);

		[ExcludeFromDocs]
		public static void IgnoreCollision(Collider2D collider1, Collider2D collider2)
		{
			bool ignore = true;
			Physics2D.IgnoreCollision(collider1, collider2, ignore);
		}

		/// <summary>
		///   <para>Checks whether the collision detection system will ignore all collisionstriggers between collider1 and collider2/ or not.</para>
		/// </summary>
		/// <param name="collider1">The first collider to compare to collider2.</param>
		/// <param name="collider2">The second collider to compare to collider1.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetIgnoreCollision(Collider2D collider1, Collider2D collider2);

		/// <summary>
		///   <para>Choose whether to detect or ignore collisions between a specified pair of layers.</para>
		/// </summary>
		/// <param name="layer1">ID of the first layer.</param>
		/// <param name="layer2">ID of the second layer.</param>
		/// <param name="ignore">Should collisions between these layers be ignored?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IgnoreLayerCollision(int layer1, int layer2, [DefaultValue("true")] bool ignore);

		[ExcludeFromDocs]
		public static void IgnoreLayerCollision(int layer1, int layer2)
		{
			bool ignore = true;
			Physics2D.IgnoreLayerCollision(layer1, layer2, ignore);
		}

		/// <summary>
		///   <para>Should collisions between the specified layers be ignored?</para>
		/// </summary>
		/// <param name="layer1">ID of first layer.</param>
		/// <param name="layer2">ID of second layer.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetIgnoreLayerCollision(int layer1, int layer2);

		/// <summary>
		///   <para>Check whether collider1 is touching collider2 or not.</para>
		/// </summary>
		/// <param name="collider1">The collider to check if it is touching collider2.</param>
		/// <param name="collider2">The collider to check if it is touching collider1.</param>
		/// <returns>
		///   <para>Whether collider1 is touching collider2 or not.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTouching(Collider2D collider1, Collider2D collider2);

		/// <summary>
		///   <para>Checks whether the collider is touching any colliders on the specified layerMask or not.</para>
		/// </summary>
		/// <param name="collider">The collider to check if it is touching colliders on the layerMask.</param>
		/// <param name="layerMask">Any colliders on any of these layers count as touching.</param>
		/// <returns>
		///   <para>Whether the collider is touching any colliders on the specified layerMask or not.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTouchingLayers(Collider2D collider, [DefaultValue("AllLayers")] int layerMask);

		[ExcludeFromDocs]
		public static bool IsTouchingLayers(Collider2D collider)
		{
			int layerMask = -1;
			return Physics2D.IsTouchingLayers(collider, layerMask);
		}

		internal static void SetEditorDragMovement(bool dragging, GameObject[] objs)
		{
			foreach (Rigidbody2D rigidbody2D in Physics2D.m_LastDisabledRigidbody2D)
			{
				rigidbody2D.isKinematic = false;
			}
			Physics2D.m_LastDisabledRigidbody2D.Clear();
			if (!dragging)
			{
				return;
			}
			foreach (GameObject gameObject in objs)
			{
				Rigidbody2D[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody2D>(false);
				foreach (Rigidbody2D rigidbody2D2 in componentsInChildren)
				{
					if (!rigidbody2D2.isKinematic)
					{
						rigidbody2D2.isKinematic = true;
						Physics2D.m_LastDisabledRigidbody2D.Add(rigidbody2D2);
					}
				}
			}
		}

		private static void Internal_Linecast(Vector2 start, Vector2 end, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_Linecast(ref start, ref end, layerMask, minDepth, maxDepth, out raycastHit);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_Linecast(ref Vector2 start, ref Vector2 end, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit);

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

		/// <summary>
		///   <para>Casts a line against colliders in the scene.</para>
		/// </summary>
		/// <param name="start">The start point of the line in world space.</param>
		/// <param name="end">The end point of the line in world space.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D Linecast(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			RaycastHit2D result;
			Physics2D.Internal_Linecast(start, end, layerMask, minDepth, maxDepth, out result);
			return result;
		}

		/// <summary>
		///   <para>Casts a line against colliders in the scene.</para>
		/// </summary>
		/// <param name="start">The start point of the line in world space.</param>
		/// <param name="end">The end point of the line in world space.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_LinecastAll(ref start, ref end, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_LinecastAll(ref Vector2 start, ref Vector2 end, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Casts a line against colliders in the scene.</para>
		/// </summary>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <param name="start">The start point of the line in world space.</param>
		/// <param name="end">The end point of the line in world space.</param>
		/// <param name="results">Returned array of objects that intersect the line.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_LinecastNonAlloc(ref start, ref end, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_LinecastNonAlloc(ref Vector2 start, ref Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth, float maxDepth);

		private static void Internal_Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_Raycast(ref origin, ref direction, distance, layerMask, minDepth, maxDepth, out raycastHit);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_Raycast(ref Vector2 origin, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit);

		[ExcludeFromDocs]
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

		/// <summary>
		///   <para>Casts a ray against colliders in the scene.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the ray originates.</param>
		/// <param name="direction">Vector representing the direction of the ray.</param>
		/// <param name="distance">Maximum distance over which to cast the ray.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			RaycastHit2D result;
			Physics2D.Internal_Raycast(origin, direction, distance, layerMask, minDepth, maxDepth, out result);
			return result;
		}

		/// <summary>
		///   <para>Casts a ray against colliders in the scene, returning all colliders that contact with it.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the ray originates.</param>
		/// <param name="direction">Vector representing the direction of the ray.</param>
		/// <param name="distance">Maximum distance over which to cast the ray.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_RaycastAll(ref origin, ref direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_RaycastAll(ref origin, ref direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_RaycastAll(ref Vector2 origin, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Casts a ray into the scene.</para>
		/// </summary>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <param name="origin">The point in 2D space where the ray originates.</param>
		/// <param name="direction">Vector representing the direction of the ray.</param>
		/// <param name="results">Array to receive results.</param>
		/// <param name="distance">Maximum distance over which to cast the ray.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_RaycastNonAlloc(ref origin, ref direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_RaycastNonAlloc(ref Vector2 origin, ref Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth, float maxDepth);

		private static void Internal_CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_CircleCast(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth, out raycastHit);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_CircleCast(ref Vector2 origin, float radius, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit);

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

		/// <summary>
		///   <para>Casts a circle against colliders in the scene, returning the first collider to contact with it.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the shape originates.</param>
		/// <param name="radius">The radius of the shape.</param>
		/// <param name="direction">Vector representing the direction of the shape.</param>
		/// <param name="distance">Maximum distance over which to cast the shape.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			RaycastHit2D result;
			Physics2D.Internal_CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth, out result);
			return result;
		}

		/// <summary>
		///   <para>Casts a circle against colliders in the scene, returning all colliders that contact with it.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the shape originates.</param>
		/// <param name="radius">The radius of the shape.</param>
		/// <param name="direction">Vector representing the direction of the shape.</param>
		/// <param name="distance">Maximum distance over which to cast the shape.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_CircleCastAll(ref origin, radius, ref direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_CircleCastAll(ref Vector2 origin, float radius, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Casts a circle into the scene, returning colliders that contact with it into the provided results array.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the shape originates.</param>
		/// <param name="radius">The radius of the shape.</param>
		/// <param name="direction">Vector representing the direction of the shape.</param>
		/// <param name="results">Array to receive results.</param>
		/// <param name="distance">Maximum distance over which to cast the shape.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_CircleCastNonAlloc(ref origin, radius, ref direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_CircleCastNonAlloc(ref Vector2 origin, float radius, ref Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth, float maxDepth);

		private static void Internal_BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_BoxCast(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth, out raycastHit);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_BoxCast(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth, out RaycastHit2D raycastHit);

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

		/// <summary>
		///   <para>Casts a box against colliders in the scene, returning the first collider to contact with it.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the shape originates.</param>
		/// <param name="size">The size of the shape.</param>
		/// <param name="angle">The angle of the shape (in degrees).</param>
		/// <param name="direction">Vector representing the direction of the shape.</param>
		/// <param name="distance">Maximum distance over which to cast the shape.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			RaycastHit2D result;
			Physics2D.Internal_BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth, out result);
			return result;
		}

		/// <summary>
		///   <para>Casts a box against colliders in the scene, returning all colliders that contact with it.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the shape originates.</param>
		/// <param name="size">The size of the shape.</param>
		/// <param name="angle">The angle of the shape (in degrees).</param>
		/// <param name="direction">Vector representing the direction of the shape.</param>
		/// <param name="distance">Maximum distance over which to cast the shape.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_BoxCastAll(ref origin, ref size, angle, ref direction, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_BoxCastAll(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Casts a box into the scene, returning colliders that contact with it into the provided results array.</para>
		/// </summary>
		/// <param name="origin">The point in 2D space where the shape originates.</param>
		/// <param name="size">The size of the shape.</param>
		/// <param name="angle">The angle of the shape (in degrees).</param>
		/// <param name="direction">Vector representing the direction of the shape.</param>
		/// <param name="results">Array to receive results.</param>
		/// <param name="distance">Maximum distance over which to cast the shape.</param>
		/// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, distance, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			float positiveInfinity2 = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_BoxCastNonAlloc(ref origin, ref size, angle, ref direction, results, positiveInfinity2, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_BoxCastNonAlloc(ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth, float maxDepth);

		private static void Internal_GetRayIntersection(Ray ray, float distance, int layerMask, out RaycastHit2D raycastHit)
		{
			Physics2D.INTERNAL_CALL_Internal_GetRayIntersection(ref ray, distance, layerMask, out raycastHit);
		}

		[WrapperlessIcall]
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

		/// <summary>
		///   <para>Cast a 3D ray against the colliders in the scene returning the first collider along the ray.</para>
		/// </summary>
		/// <param name="ray">The 3D ray defining origin and direction to test.</param>
		/// <param name="distance">Maximum distance over which to cast the ray.</param>
		/// <param name="layerMask">Filter to detect colliders only on certain layers.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
		public static RaycastHit2D GetRayIntersection(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
		{
			RaycastHit2D result;
			Physics2D.Internal_GetRayIntersection(ray, distance, layerMask, out result);
			return result;
		}

		/// <summary>
		///   <para>Cast a 3D ray against the colliders in the scene returning all the colliders along the ray.</para>
		/// </summary>
		/// <param name="ray">The 3D ray defining origin and direction to test.</param>
		/// <param name="distance">Maximum distance over which to cast the ray.</param>
		/// <param name="layerMask">Filter to detect colliders only on certain layers.</param>
		/// <returns>
		///   <para>The cast results returned.</para>
		/// </returns>
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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RaycastHit2D[] INTERNAL_CALL_GetRayIntersectionAll(ref Ray ray, float distance, int layerMask);

		/// <summary>
		///   <para>Cast a 3D ray against the colliders in the scene returning the colliders along the ray.</para>
		/// </summary>
		/// <param name="ray">The 3D ray defining origin and direction to test.</param>
		/// <param name="distance">Maximum distance over which to cast the ray.</param>
		/// <param name="layerMask">Filter to detect colliders only on certain layers.</param>
		/// <param name="results">Array to receive results.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetRayIntersectionNonAlloc(ref Ray ray, RaycastHit2D[] results, float distance, int layerMask);

		/// <summary>
		///   <para>Check if a collider overlaps a point in space.</para>
		/// </summary>
		/// <param name="point">A point in world space.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		public static Collider2D OverlapPoint(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapPoint(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapPoint(ref point, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapPoint(ref point, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapPoint(Vector2 point)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapPoint(ref point, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_OverlapPoint(ref Vector2 point, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Get a list of all colliders that overlap a point in space.</para>
		/// </summary>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <param name="point">A point in space.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		public static Collider2D[] OverlapPointAll(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapPointAll(ref point, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapPointAll(ref point, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapPointAll(ref point, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapPointAll(Vector2 point)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapPointAll(ref point, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_OverlapPointAll(ref Vector2 point, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Get a list of all colliders that overlap a point in space.</para>
		/// </summary>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <param name="point">A point in space.</param>
		/// <param name="results">Array to receive results.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapPointNonAlloc(ref point, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_OverlapPointNonAlloc(ref Vector2 point, Collider2D[] results, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Check if a collider falls within a circular area.</para>
		/// </summary>
		/// <param name="point">Centre of the circle.</param>
		/// <param name="radius">Radius of the circle.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		public static Collider2D OverlapCircle(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapCircle(Vector2 point, float radius)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapCircle(ref point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_OverlapCircle(ref Vector2 point, float radius, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Get a list of all colliders that fall within a circular area.</para>
		/// </summary>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <param name="point">Center of the circle.</param>
		/// <param name="radius">Radius of the circle.</param>
		/// <param name="layerMask">Filter to check objects only on specified layers.</param>
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapCircleAll(Vector2 point, float radius)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapCircleAll(ref point, radius, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_OverlapCircleAll(ref Vector2 point, float radius, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Get a list of all colliders that fall within a circular area.</para>
		/// </summary>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <param name="point">Center of the circle.</param>
		/// <param name="radius">Radius of the circle.</param>
		/// <param name="results">Array to receive results.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapCircleNonAlloc(ref point, radius, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_OverlapCircleNonAlloc(ref Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Check if a collider falls within a rectangular area.</para>
		/// </summary>
		/// <param name="pointA">One corner of the rectangle.</param>
		/// <param name="pointB">Diagonally opposite corner of the rectangle.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapArea(ref pointA, ref pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D INTERNAL_CALL_OverlapArea(ref Vector2 pointA, ref Vector2 pointB, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Get a list of all colliders that fall within a rectangular area.</para>
		/// </summary>
		/// <param name="pointA">One corner of the rectangle.</param>
		/// <param name="pointB">Diagonally opposite corner of the rectangle.</param>
		/// <param name="layerMask">Filter to check objects only on specific layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapAreaAll(ref pointA, ref pointB, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider2D[] INTERNAL_CALL_OverlapAreaAll(ref Vector2 pointA, ref Vector2 pointB, int layerMask, float minDepth, float maxDepth);

		/// <summary>
		///   <para>Get a list of all colliders that fall within a specified area.</para>
		/// </summary>
		/// <param name="pointA">One corner of the rectangle.</param>
		/// <param name="pointB">Diagonally opposite corner of the rectangle.</param>
		/// <param name="results">Array to receive results.</param>
		/// <param name="layerMask">Filter to check objects only on specified layers.</param>
		/// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than this value.</param>
		/// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than this value.</param>
		/// <returns>
		///   <para>The number of results returned.</para>
		/// </returns>
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			return Physics2D.INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, minDepth, maxDepth);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask, float minDepth)
		{
			float positiveInfinity = float.PositiveInfinity;
			return Physics2D.INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, minDepth, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			return Physics2D.INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[ExcludeFromDocs]
		public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results)
		{
			float positiveInfinity = float.PositiveInfinity;
			float negativeInfinity = float.NegativeInfinity;
			int layerMask = -5;
			return Physics2D.INTERNAL_CALL_OverlapAreaNonAlloc(ref pointA, ref pointB, results, layerMask, negativeInfinity, positiveInfinity);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_OverlapAreaNonAlloc(ref Vector2 pointA, ref Vector2 pointB, Collider2D[] results, int layerMask, float minDepth, float maxDepth);
	}
}
