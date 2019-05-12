using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes a set of bounding spheres that should have their visibility and distances maintained.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class CullingGroup : IDisposable
	{
		internal IntPtr m_Ptr;

		private CullingGroup.StateChanged m_OnStateChanged;

		/// <summary>
		///   <para>Create a CullingGroup.</para>
		/// </summary>
		public CullingGroup()
		{
			this.Init();
		}

		~CullingGroup()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				this.FinalizerFailure();
			}
		}

		/// <summary>
		///   <para>Clean up all memory used by the CullingGroup immediately.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispose();

		/// <summary>
		///   <para>Sets the callback that will be called when a sphere's visibility and/or distance state has changed.</para>
		/// </summary>
		public CullingGroup.StateChanged onStateChanged
		{
			get
			{
				return this.m_OnStateChanged;
			}
			set
			{
				this.m_OnStateChanged = value;
			}
		}

		/// <summary>
		///   <para>Pauses culling group execution.</para>
		/// </summary>
		public extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Locks the CullingGroup to a specific camera.</para>
		/// </summary>
		public extern Camera targetCamera { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Sets the array of bounding sphere definitions that the CullingGroup should compute culling for.</para>
		/// </summary>
		/// <param name="array">The BoundingSpheres to cull.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBoundingSpheres(BoundingSphere[] array);

		/// <summary>
		///   <para>Sets the number of bounding spheres in the bounding spheres array that are actually being used.</para>
		/// </summary>
		/// <param name="count">The number of bounding spheres being used.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBoundingSphereCount(int count);

		/// <summary>
		///   <para>Erase a given bounding sphere by moving the final sphere on top of it.</para>
		/// </summary>
		/// <param name="index">The index of the entry to erase.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void EraseSwapBack(int index);

		public static void EraseSwapBack<T>(int index, T[] myArray, ref int size)
		{
			size--;
			myArray[index] = myArray[size];
		}

		/// <summary>
		///   <para>Retrieve the indices of spheres that have particular visibility and/or distance states.</para>
		/// </summary>
		/// <param name="visible">True if only visible spheres should be retrieved; false if only invisible spheres should be retrieved.</param>
		/// <param name="distanceIndex">The distance band that retrieved spheres must be in.</param>
		/// <param name="result">An array that will be filled with the retrieved sphere indices.</param>
		/// <param name="firstIndex">The index of the sphere to begin searching at.</param>
		/// <returns>
		///   <para>The number of sphere indices found and written into the result array.</para>
		/// </returns>
		public int QueryIndices(bool visible, int[] result, int firstIndex)
		{
			return this.QueryIndices(visible, -1, CullingQueryOptions.IgnoreDistance, result, firstIndex);
		}

		/// <summary>
		///   <para>Retrieve the indices of spheres that have particular visibility and/or distance states.</para>
		/// </summary>
		/// <param name="visible">True if only visible spheres should be retrieved; false if only invisible spheres should be retrieved.</param>
		/// <param name="distanceIndex">The distance band that retrieved spheres must be in.</param>
		/// <param name="result">An array that will be filled with the retrieved sphere indices.</param>
		/// <param name="firstIndex">The index of the sphere to begin searching at.</param>
		/// <returns>
		///   <para>The number of sphere indices found and written into the result array.</para>
		/// </returns>
		public int QueryIndices(int distanceIndex, int[] result, int firstIndex)
		{
			return this.QueryIndices(false, distanceIndex, CullingQueryOptions.IgnoreVisibility, result, firstIndex);
		}

		/// <summary>
		///   <para>Retrieve the indices of spheres that have particular visibility and/or distance states.</para>
		/// </summary>
		/// <param name="visible">True if only visible spheres should be retrieved; false if only invisible spheres should be retrieved.</param>
		/// <param name="distanceIndex">The distance band that retrieved spheres must be in.</param>
		/// <param name="result">An array that will be filled with the retrieved sphere indices.</param>
		/// <param name="firstIndex">The index of the sphere to begin searching at.</param>
		/// <returns>
		///   <para>The number of sphere indices found and written into the result array.</para>
		/// </returns>
		public int QueryIndices(bool visible, int distanceIndex, int[] result, int firstIndex)
		{
			return this.QueryIndices(visible, distanceIndex, CullingQueryOptions.Normal, result, firstIndex);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int QueryIndices(bool visible, int distanceIndex, CullingQueryOptions options, int[] result, int firstIndex);

		/// <summary>
		///   <para>Returns true if the bounding sphere at index is currently visible from any of the contributing cameras.</para>
		/// </summary>
		/// <param name="index">The index of the bounding sphere.</param>
		/// <returns>
		///   <para>True if the sphere is visible; false if it is invisible.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsVisible(int index);

		/// <summary>
		///   <para>Get the current distance band index of a given sphere.</para>
		/// </summary>
		/// <param name="index">The index of the sphere.</param>
		/// <returns>
		///   <para>The sphere's current distance band index.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetDistance(int index);

		/// <summary>
		///   <para>Set bounding distances for 'distance bands' the group should compute, as well as options for how spheres falling into each distance band should be treated.</para>
		/// </summary>
		/// <param name="distances">An array of bounding distances. The distances should be sorted in increasing order.</param>
		/// <param name="distanceBehaviours">An array of CullingDistanceBehaviour settings. The array should be the same length as the array provided to the distances parameter. It can also be omitted or passed as null, in which case all distances will be given CullingDistanceBehaviour.Normal behaviour.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBoundingDistances(float[] distances);

		/// <summary>
		///   <para>Set the reference point from which distance bands are measured.</para>
		/// </summary>
		/// <param name="point">A fixed point to measure the distance from.</param>
		/// <param name="transform">A transform to measure the distance from. The transform's position will be automatically tracked.</param>
		public void SetDistanceReferencePoint(Vector3 point)
		{
			CullingGroup.INTERNAL_CALL_SetDistanceReferencePoint(this, ref point);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetDistanceReferencePoint(CullingGroup self, ref Vector3 point);

		/// <summary>
		///   <para>Set the reference point from which distance bands are measured.</para>
		/// </summary>
		/// <param name="point">A fixed point to measure the distance from.</param>
		/// <param name="transform">A transform to measure the distance from. The transform's position will be automatically tracked.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDistanceReferencePoint(Transform transform);

		[SecuritySafeCritical]
		private unsafe static void SendEvents(CullingGroup cullingGroup, IntPtr eventsPtr, int count)
		{
			CullingGroupEvent* ptr = (CullingGroupEvent*)eventsPtr.ToPointer();
			if (cullingGroup.m_OnStateChanged == null)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				cullingGroup.m_OnStateChanged(ptr[i]);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void FinalizerFailure();

		/// <summary>
		///   <para>This delegate is used for recieving a callback when a sphere's distance or visibility state has changed.</para>
		/// </summary>
		/// <param name="sphere">A CullingGroupEvent that provides information about the sphere that has changed.</param>
		public delegate void StateChanged(CullingGroupEvent sphere);
	}
}
