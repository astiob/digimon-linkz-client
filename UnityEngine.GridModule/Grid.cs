using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeType(Header = "Modules/Grid/Public/Grid.h")]
	[NativeHeader("Modules/Grid/Public/GridMarshalling.h")]
	[RequireComponent(typeof(Transform))]
	public sealed class Grid : GridLayout
	{
		public Vector3 GetCellCenterLocal(Vector3Int position)
		{
			return base.CellToLocalInterpolated(position + base.GetLayoutCellCenter());
		}

		public Vector3 GetCellCenterWorld(Vector3Int position)
		{
			return base.LocalToWorld(base.CellToLocalInterpolated(position + base.GetLayoutCellCenter()));
		}

		public new Vector3 cellSize
		{
			[FreeFunction("GridBindings::GetCellSize", HasExplicitThis = true)]
			get
			{
				Vector3 result;
				this.get_cellSize_Injected(out result);
				return result;
			}
			[FreeFunction("GridBindings::SetCellSize", HasExplicitThis = true)]
			set
			{
				this.set_cellSize_Injected(ref value);
			}
		}

		public new Vector3 cellGap
		{
			[FreeFunction("GridBindings::GetCellGap", HasExplicitThis = true)]
			get
			{
				Vector3 result;
				this.get_cellGap_Injected(out result);
				return result;
			}
			[FreeFunction("GridBindings::SetCellGap", HasExplicitThis = true)]
			set
			{
				this.set_cellGap_Injected(ref value);
			}
		}

		public new extern GridLayout.CellLayout cellLayout { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public new extern GridLayout.CellSwizzle cellSwizzle { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[FreeFunction("GridBindings::CellSwizzle")]
		public static Vector3 Swizzle(GridLayout.CellSwizzle swizzle, Vector3 position)
		{
			Vector3 result;
			Grid.Swizzle_Injected(swizzle, ref position, out result);
			return result;
		}

		[FreeFunction("GridBindings::InverseCellSwizzle")]
		public static Vector3 InverseSwizzle(GridLayout.CellSwizzle swizzle, Vector3 position)
		{
			Vector3 result;
			Grid.InverseSwizzle_Injected(swizzle, ref position, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_cellSize_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_cellSize_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_cellGap_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_cellGap_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Swizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InverseSwizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);
	}
}
