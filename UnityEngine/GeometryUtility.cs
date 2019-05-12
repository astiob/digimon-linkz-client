using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Utility class for common geometric functions.</para>
	/// </summary>
	public sealed class GeometryUtility
	{
		/// <summary>
		///   <para>Calculates frustum planes.</para>
		/// </summary>
		/// <param name="camera"></param>
		public static Plane[] CalculateFrustumPlanes(Camera camera)
		{
			return GeometryUtility.CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix);
		}

		/// <summary>
		///   <para>Calculates frustum planes.</para>
		/// </summary>
		/// <param name="worldToProjectionMatrix"></param>
		public static Plane[] CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix)
		{
			Plane[] array = new Plane[6];
			GeometryUtility.Internal_ExtractPlanes(array, worldToProjectionMatrix);
			return array;
		}

		private static void Internal_ExtractPlanes(Plane[] planes, Matrix4x4 worldToProjectionMatrix)
		{
			GeometryUtility.INTERNAL_CALL_Internal_ExtractPlanes(planes, ref worldToProjectionMatrix);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_ExtractPlanes(Plane[] planes, ref Matrix4x4 worldToProjectionMatrix);

		/// <summary>
		///   <para>Returns true if bounds are inside the plane array.</para>
		/// </summary>
		/// <param name="planes"></param>
		/// <param name="bounds"></param>
		public static bool TestPlanesAABB(Plane[] planes, Bounds bounds)
		{
			return GeometryUtility.INTERNAL_CALL_TestPlanesAABB(planes, ref bounds);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_TestPlanesAABB(Plane[] planes, ref Bounds bounds);
	}
}
