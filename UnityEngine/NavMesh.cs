using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Singleton class to access the baked NavMesh.</para>
	/// </summary>
	public sealed class NavMesh
	{
		/// <summary>
		///   <para>Area mask constant that includes all NavMesh areas.</para>
		/// </summary>
		public const int AllAreas = -1;

		public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int areaMask)
		{
			return NavMesh.INTERNAL_CALL_Raycast(ref sourcePosition, ref targetPosition, out hit, areaMask);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_Raycast(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int areaMask);

		/// <summary>
		///   <para>Calculate a path between two points and store the resulting path.</para>
		/// </summary>
		/// <param name="sourcePosition">The initial position of the path requested.</param>
		/// <param name="targetPosition">The final position of the path requested.</param>
		/// <param name="areaMask">A bitfield mask specifying which NavMesh areas can be passed when calculating a path.</param>
		/// <param name="path">The resulting path.</param>
		/// <returns>
		///   <para>True if a either a complete or partial path is found and false otherwise.</para>
		/// </returns>
		public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			path.ClearCorners();
			return NavMesh.CalculatePathInternal(sourcePosition, targetPosition, areaMask, path);
		}

		internal static bool CalculatePathInternal(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
		{
			return NavMesh.INTERNAL_CALL_CalculatePathInternal(ref sourcePosition, ref targetPosition, areaMask, path);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_CalculatePathInternal(ref Vector3 sourcePosition, ref Vector3 targetPosition, int areaMask, NavMeshPath path);

		public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, int areaMask)
		{
			return NavMesh.INTERNAL_CALL_FindClosestEdge(ref sourcePosition, out hit, areaMask);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_FindClosestEdge(ref Vector3 sourcePosition, out NavMeshHit hit, int areaMask);

		public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
		{
			return NavMesh.INTERNAL_CALL_SamplePosition(ref sourcePosition, out hit, maxDistance, areaMask);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_SamplePosition(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask);

		/// <summary>
		///   <para>Sets the cost for traversing over geometry of the layer type on all agents.</para>
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="cost"></param>
		[Obsolete("Use SetAreaCost instead.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLayerCost(int layer, float cost);

		/// <summary>
		///   <para>Gets the cost for traversing over geometry of the layer type on all agents.</para>
		/// </summary>
		/// <param name="layer"></param>
		[Obsolete("Use GetAreaCost instead.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetLayerCost(int layer);

		/// <summary>
		///   <para>Returns the layer index for a named layer.</para>
		/// </summary>
		/// <param name="layerName"></param>
		[Obsolete("Use GetAreaFromName instead.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNavMeshLayerFromName(string layerName);

		/// <summary>
		///   <para>Sets the cost for finding path over geometry of the area type on all agents.</para>
		/// </summary>
		/// <param name="areaIndex">Index of the area to set.</param>
		/// <param name="cost">New cost.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAreaCost(int areaIndex, float cost);

		/// <summary>
		///   <para>Gets the cost for path finding over geometry of the area type.</para>
		/// </summary>
		/// <param name="areaIndex">Index of the area to get.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetAreaCost(int areaIndex);

		/// <summary>
		///   <para>Returns the area index for a named NavMesh area type.</para>
		/// </summary>
		/// <param name="areaName">Name of the area to look up.</param>
		/// <returns>
		///   <para>Index if the specified are, or -1 if no area found.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetAreaFromName(string areaName);

		/// <summary>
		///   <para>Calculates triangulation of the current navmesh.</para>
		/// </summary>
		public static NavMeshTriangulation CalculateTriangulation()
		{
			return (NavMeshTriangulation)NavMesh.TriangulateInternal();
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object TriangulateInternal();

		[Obsolete("use NavMesh.CalculateTriangulation() instead.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Triangulate(out Vector3[] vertices, out int[] indices);

		[Obsolete("AddOffMeshLinks has no effect and is deprecated.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void AddOffMeshLinks();

		[Obsolete("RestoreNavMesh has no effect and is deprecated.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RestoreNavMesh();

		/// <summary>
		///   <para>Describes how far in the future the agents predict collisions for avoidance.</para>
		/// </summary>
		public static float avoidancePredictionTime
		{
			get
			{
				return NavMesh.GetAvoidancePredictionTime();
			}
			set
			{
				NavMesh.SetAvoidancePredictionTime(value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetAvoidancePredictionTime(float t);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetAvoidancePredictionTime();

		/// <summary>
		///   <para>The maximum amount of nodes processed each frame in the asynchronous pathfinding process.</para>
		/// </summary>
		public static int pathfindingIterationsPerFrame
		{
			get
			{
				return NavMesh.GetPathfindingIterationsPerFrame();
			}
			set
			{
				NavMesh.SetPathfindingIterationsPerFrame(value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetPathfindingIterationsPerFrame(int iter);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPathfindingIterationsPerFrame();
	}
}
