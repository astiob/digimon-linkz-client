using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public sealed class GeometryUtility
	{
		public static Plane[] CalculateFrustumPlanes(Camera camera)
		{
			Plane[] array = new Plane[6];
			GeometryUtility.CalculateFrustumPlanes(camera, array);
			return array;
		}

		public static Plane[] CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix)
		{
			Plane[] array = new Plane[6];
			GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix, array);
			return array;
		}

		public static void CalculateFrustumPlanes(Camera camera, Plane[] planes)
		{
			GeometryUtility.CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix, planes);
		}

		public static void CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix, Plane[] planes)
		{
			if (planes == null)
			{
				throw new ArgumentNullException("planes");
			}
			if (planes.Length != 6)
			{
				throw new ArgumentException("Planes array must be of length 6.", "planes");
			}
			GeometryUtility.Internal_ExtractPlanes(planes, worldToProjectionMatrix);
		}

		public static Bounds CalculateBounds(Vector3[] positions, Matrix4x4 transform)
		{
			if (positions == null)
			{
				throw new ArgumentNullException("positions");
			}
			if (positions.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.", "positions");
			}
			return GeometryUtility.Internal_CalculateBounds(positions, transform);
		}

		public static bool TryCreatePlaneFromPolygon(Vector3[] vertices, out Plane plane)
		{
			bool result;
			if (vertices == null || vertices.Length < 3)
			{
				plane = new Plane(Vector3.up, 0f);
				result = false;
			}
			else if (vertices.Length == 3)
			{
				Vector3 a = vertices[0];
				Vector3 b = vertices[1];
				Vector3 c = vertices[2];
				plane = new Plane(a, b, c);
				result = (plane.normal.sqrMagnitude > 0f);
			}
			else
			{
				Vector3 zero = Vector3.zero;
				int num = vertices.Length - 1;
				Vector3 vector = vertices[num];
				foreach (Vector3 vector2 in vertices)
				{
					zero.x += (vector.y - vector2.y) * (vector.z + vector2.z);
					zero.y += (vector.z - vector2.z) * (vector.x + vector2.x);
					zero.z += (vector.x - vector2.x) * (vector.y + vector2.y);
					vector = vector2;
				}
				zero.Normalize();
				float num2 = 0f;
				foreach (Vector3 rhs in vertices)
				{
					num2 -= Vector3.Dot(zero, rhs);
				}
				num2 /= (float)vertices.Length;
				plane = new Plane(zero, num2);
				result = (plane.normal.sqrMagnitude > 0f);
			}
			return result;
		}

		[FreeFunction("GeometryUtilityScripting::ExtractPlanes")]
		private static void Internal_ExtractPlanes(Plane[] planes, Matrix4x4 worldToProjectionMatrix)
		{
			GeometryUtility.Internal_ExtractPlanes_Injected(planes, ref worldToProjectionMatrix);
		}

		[FreeFunction("GeometryUtilityScripting::TestPlanesAABB")]
		public static bool TestPlanesAABB(Plane[] planes, Bounds bounds)
		{
			return GeometryUtility.TestPlanesAABB_Injected(planes, ref bounds);
		}

		[FreeFunction("GeometryUtilityScripting::CalculateBounds")]
		private static Bounds Internal_CalculateBounds(Vector3[] positions, Matrix4x4 transform)
		{
			Bounds result;
			GeometryUtility.Internal_CalculateBounds_Injected(positions, ref transform, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ExtractPlanes_Injected(Plane[] planes, ref Matrix4x4 worldToProjectionMatrix);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TestPlanesAABB_Injected(Plane[] planes, ref Bounds bounds);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CalculateBounds_Injected(Vector3[] positions, ref Matrix4x4 transform, out Bounds ret);
	}
}
