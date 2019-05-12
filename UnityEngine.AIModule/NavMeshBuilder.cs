using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.AI
{
	public static class NavMeshBuilder
	{
		public static void CollectSources(Bounds includedWorldBounds, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
		{
			if (markups == null)
			{
				throw new ArgumentNullException("markups");
			}
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			includedWorldBounds.extents = Vector3.Max(includedWorldBounds.extents, 0.001f * Vector3.one);
			NavMeshBuildSource[] collection = NavMeshBuilder.CollectSourcesInternal(includedLayerMask, includedWorldBounds, null, true, geometry, defaultArea, markups.ToArray());
			results.Clear();
			results.AddRange(collection);
		}

		public static void CollectSources(Transform root, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
		{
			if (markups == null)
			{
				throw new ArgumentNullException("markups");
			}
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			NavMeshBuildSource[] collection = NavMeshBuilder.CollectSourcesInternal(includedLayerMask, default(Bounds), root, false, geometry, defaultArea, markups.ToArray());
			results.Clear();
			results.AddRange(collection);
		}

		private static NavMeshBuildSource[] CollectSourcesInternal(int includedLayerMask, Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, NavMeshBuildMarkup[] markups)
		{
			return NavMeshBuilder.INTERNAL_CALL_CollectSourcesInternal(includedLayerMask, ref includedWorldBounds, root, useBounds, geometry, defaultArea, markups);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern NavMeshBuildSource[] INTERNAL_CALL_CollectSourcesInternal(int includedLayerMask, ref Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, NavMeshBuildMarkup[] markups);

		public static NavMeshData BuildNavMeshData(NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds, Vector3 position, Quaternion rotation)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			NavMeshData navMeshData = new NavMeshData(buildSettings.agentTypeID);
			navMeshData.position = position;
			navMeshData.rotation = rotation;
			NavMeshBuilder.UpdateNavMeshDataListInternal(navMeshData, buildSettings, sources, localBounds);
			return navMeshData;
		}

		public static bool UpdateNavMeshData(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return NavMeshBuilder.UpdateNavMeshDataListInternal(data, buildSettings, sources, localBounds);
		}

		private static bool UpdateNavMeshDataListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
		{
			return NavMeshBuilder.INTERNAL_CALL_UpdateNavMeshDataListInternal(data, ref buildSettings, sources, ref localBounds);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_UpdateNavMeshDataListInternal(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);

		public static AsyncOperation UpdateNavMeshDataAsync(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			return NavMeshBuilder.UpdateNavMeshDataAsyncListInternal(data, buildSettings, sources, localBounds);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Cancel(NavMeshData data);

		private static AsyncOperation UpdateNavMeshDataAsyncListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
		{
			return NavMeshBuilder.INTERNAL_CALL_UpdateNavMeshDataAsyncListInternal(data, ref buildSettings, sources, ref localBounds);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AsyncOperation INTERNAL_CALL_UpdateNavMeshDataAsyncListInternal(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);
	}
}
