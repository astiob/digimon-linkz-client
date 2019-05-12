using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Modules/Grid/Public/GridMarshalling.h")]
	[NativeType(Header = "Modules/Grid/Public/Grid.h")]
	public class GridLayout : Behaviour
	{
		public Vector3 cellSize
		{
			[FreeFunction("GridLayoutBindings::GetCellSize", HasExplicitThis = true)]
			get
			{
				Vector3 result;
				this.get_cellSize_Injected(out result);
				return result;
			}
		}

		public Vector3 cellGap
		{
			[FreeFunction("GridLayoutBindings::GetCellGap", HasExplicitThis = true)]
			get
			{
				Vector3 result;
				this.get_cellGap_Injected(out result);
				return result;
			}
		}

		public extern GridLayout.CellLayout cellLayout { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern GridLayout.CellSwizzle cellSwizzle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[FreeFunction("GridLayoutBindings::GetBoundsLocal", HasExplicitThis = true)]
		public Bounds GetBoundsLocal(Vector3Int cellPosition)
		{
			Bounds result;
			this.GetBoundsLocal_Injected(ref cellPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::CellToLocal", HasExplicitThis = true)]
		public Vector3 CellToLocal(Vector3Int cellPosition)
		{
			Vector3 result;
			this.CellToLocal_Injected(ref cellPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::LocalToCell", HasExplicitThis = true)]
		public Vector3Int LocalToCell(Vector3 localPosition)
		{
			Vector3Int result;
			this.LocalToCell_Injected(ref localPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::CellToLocalInterpolated", HasExplicitThis = true)]
		public Vector3 CellToLocalInterpolated(Vector3 cellPosition)
		{
			Vector3 result;
			this.CellToLocalInterpolated_Injected(ref cellPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::LocalToCellInterpolated", HasExplicitThis = true)]
		public Vector3 LocalToCellInterpolated(Vector3 localPosition)
		{
			Vector3 result;
			this.LocalToCellInterpolated_Injected(ref localPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::CellToWorld", HasExplicitThis = true)]
		public Vector3 CellToWorld(Vector3Int cellPosition)
		{
			Vector3 result;
			this.CellToWorld_Injected(ref cellPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::WorldToCell", HasExplicitThis = true)]
		public Vector3Int WorldToCell(Vector3 worldPosition)
		{
			Vector3Int result;
			this.WorldToCell_Injected(ref worldPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::LocalToWorld", HasExplicitThis = true)]
		public Vector3 LocalToWorld(Vector3 localPosition)
		{
			Vector3 result;
			this.LocalToWorld_Injected(ref localPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::WorldToLocal", HasExplicitThis = true)]
		public Vector3 WorldToLocal(Vector3 worldPosition)
		{
			Vector3 result;
			this.WorldToLocal_Injected(ref worldPosition, out result);
			return result;
		}

		[FreeFunction("GridLayoutBindings::GetLayoutCellCenter", HasExplicitThis = true)]
		public Vector3 GetLayoutCellCenter()
		{
			Vector3 result;
			this.GetLayoutCellCenter_Injected(out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_cellSize_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_cellGap_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetBoundsLocal_Injected(ref Vector3Int cellPosition, out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CellToLocal_Injected(ref Vector3Int cellPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void LocalToCell_Injected(ref Vector3 localPosition, out Vector3Int ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CellToLocalInterpolated_Injected(ref Vector3 cellPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void LocalToCellInterpolated_Injected(ref Vector3 localPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CellToWorld_Injected(ref Vector3Int cellPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void WorldToCell_Injected(ref Vector3 worldPosition, out Vector3Int ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void LocalToWorld_Injected(ref Vector3 localPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void WorldToLocal_Injected(ref Vector3 worldPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLayoutCellCenter_Injected(out Vector3 ret);

		public enum CellLayout
		{
			Rectangle
		}

		public enum CellSwizzle
		{
			XYZ,
			XZY,
			YXZ,
			YZX,
			ZXY,
			ZYX
		}
	}
}
