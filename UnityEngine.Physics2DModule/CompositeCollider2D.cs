using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeHeader("Modules/Physics2D/Public/CompositeCollider2D.h")]
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class CompositeCollider2D : Collider2D
	{
		public int GetPath(int index, Vector2[] points)
		{
			if (index < 0 || index >= this.pathCount)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Path index {0} must be in the range of 0 to {1}.", index, this.pathCount - 1));
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			return this.Internal_GetPath(index, points);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_GetPath(int index, Vector2[] points);

		public extern CompositeCollider2D.GeometryType geometryType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern CompositeCollider2D.GenerationType generationType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float vertexDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float edgeRadius { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GenerateGeometry();

		public int GetPathPointCount(int index)
		{
			int num = this.pathCount - 1;
			if (index < 0 || index > num)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Path index {0} must be in the range of 0 to {1}.", index, num));
			}
			return this.GetPathPointCount_Internal(index);
		}

		[NativeMethod("GetPathPointCount_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPathPointCount_Internal(int index);

		public extern int pathCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int pointCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public enum GeometryType
		{
			Outlines,
			Polygons
		}

		public enum GenerationType
		{
			Synchronous,
			Manual
		}
	}
}
