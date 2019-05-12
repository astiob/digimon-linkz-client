using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Tilemaps
{
	[NativeType(Header = "Modules/Tilemap/Public/TilemapRenderer.h")]
	[NativeHeader("Modules/Tilemap/TilemapRendererJobs.h")]
	[NativeHeader("Modules/Grid/Public/GridMarshalling.h")]
	[RequireComponent(typeof(Tilemap))]
	public sealed class TilemapRenderer : Renderer
	{
		public Vector3Int chunkSize
		{
			get
			{
				Vector3Int result;
				this.get_chunkSize_Injected(out result);
				return result;
			}
			set
			{
				this.set_chunkSize_Injected(ref value);
			}
		}

		public extern int maxChunkCount { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern int maxFrameAge { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern TilemapRenderer.SortOrder sortOrder { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern SpriteMaskInteraction maskInteraction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_chunkSize_Injected(out Vector3Int ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_chunkSize_Injected(ref Vector3Int value);

		public enum SortOrder
		{
			BottomLeft,
			BottomRight,
			TopLeft,
			TopRight
		}
	}
}
