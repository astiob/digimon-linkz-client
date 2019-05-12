using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Tilemaps
{
	[NativeHeader("Modules/Tilemap/Public/TilemapTile.h")]
	[NativeHeader("Modules/Tilemap/Public/TilemapMarshalling.h")]
	[NativeType(Header = "Modules/Tilemap/Public/Tilemap.h")]
	[NativeHeader("Runtime/Graphics/SpriteFrame.h")]
	[NativeHeader("Modules/Grid/Public/Grid.h")]
	[NativeHeader("Modules/Grid/Public/GridMarshalling.h")]
	[RequireComponent(typeof(Transform))]
	public sealed class Tilemap : GridLayout
	{
		public extern Grid layoutGrid { [NativeMethod(Name = "GetAttachedGrid")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public Vector3 GetCellCenterLocal(Vector3Int position)
		{
			return base.CellToLocalInterpolated(position + this.tileAnchor);
		}

		public Vector3 GetCellCenterWorld(Vector3Int position)
		{
			return base.LocalToWorld(base.CellToLocalInterpolated(position + this.tileAnchor));
		}

		public BoundsInt cellBounds
		{
			get
			{
				return new BoundsInt(this.origin, this.size);
			}
		}

		[NativeProperty("TilemapBoundsScripting")]
		public Bounds localBounds
		{
			get
			{
				Bounds result;
				this.get_localBounds_Injected(out result);
				return result;
			}
		}

		public extern float animationFrameRate { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Color color
		{
			get
			{
				Color result;
				this.get_color_Injected(out result);
				return result;
			}
			set
			{
				this.set_color_Injected(ref value);
			}
		}

		public Vector3Int origin
		{
			get
			{
				Vector3Int result;
				this.get_origin_Injected(out result);
				return result;
			}
			set
			{
				this.set_origin_Injected(ref value);
			}
		}

		public Vector3Int size
		{
			get
			{
				Vector3Int result;
				this.get_size_Injected(out result);
				return result;
			}
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		[NativeProperty(Name = "TileAnchorScripting")]
		public Vector3 tileAnchor
		{
			get
			{
				Vector3 result;
				this.get_tileAnchor_Injected(out result);
				return result;
			}
			set
			{
				this.set_tileAnchor_Injected(ref value);
			}
		}

		public extern Tilemap.Orientation orientation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Matrix4x4 orientationMatrix
		{
			[NativeMethod(Name = "GetTileOrientationMatrix")]
			get
			{
				Matrix4x4 result;
				this.get_orientationMatrix_Injected(out result);
				return result;
			}
			[NativeMethod(Name = "SetOrientationMatrix")]
			set
			{
				this.set_orientationMatrix_Injected(ref value);
			}
		}

		internal Object GetTileAsset(Vector3Int position)
		{
			return this.GetTileAsset_Injected(ref position);
		}

		public TileBase GetTile(Vector3Int position)
		{
			return (TileBase)this.GetTileAsset(position);
		}

		public T GetTile<T>(Vector3Int position) where T : TileBase
		{
			return this.GetTileAsset(position) as T;
		}

		internal Object[] GetTileAssetsBlock(Vector3Int position, Vector3Int blockDimensions)
		{
			return this.GetTileAssetsBlock_Injected(ref position, ref blockDimensions);
		}

		public TileBase[] GetTilesBlock(BoundsInt bounds)
		{
			Object[] tileAssetsBlock = this.GetTileAssetsBlock(bounds.min, bounds.size);
			TileBase[] array = new TileBase[tileAssetsBlock.Length];
			for (int i = 0; i < tileAssetsBlock.Length; i++)
			{
				array[i] = (TileBase)tileAssetsBlock[i];
			}
			return array;
		}

		internal void SetTileAsset(Vector3Int position, Object tile)
		{
			this.SetTileAsset_Injected(ref position, tile);
		}

		public void SetTile(Vector3Int position, TileBase tile)
		{
			this.SetTileAsset(position, tile);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetTileAssets(Vector3Int[] positionArray, Object[] tileArray);

		public void SetTiles(Vector3Int[] positionArray, TileBase[] tileArray)
		{
			this.SetTileAssets(positionArray, tileArray);
		}

		[NativeMethod(Name = "SetTileAssetsBlock")]
		private void INTERNAL_CALL_SetTileAssetsBlock(Vector3Int position, Vector3Int blockDimensions, Object[] tileArray)
		{
			this.INTERNAL_CALL_SetTileAssetsBlock_Injected(ref position, ref blockDimensions, tileArray);
		}

		public void SetTilesBlock(BoundsInt position, TileBase[] tileArray)
		{
			this.INTERNAL_CALL_SetTileAssetsBlock(position.min, position.size, tileArray);
		}

		public bool HasTile(Vector3Int position)
		{
			return this.GetTileAsset(position) != null;
		}

		[NativeMethod(Name = "RefreshTileAsset")]
		public void RefreshTile(Vector3Int position)
		{
			this.RefreshTile_Injected(ref position);
		}

		[NativeMethod(Name = "RefreshAllTileAssets")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RefreshAllTiles();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SwapTileAsset(Object changeTile, Object newTile);

		public void SwapTile(TileBase changeTile, TileBase newTile)
		{
			this.SwapTileAsset(changeTile, newTile);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool ContainsTileAsset(Object tileAsset);

		public bool ContainsTile(TileBase tileAsset)
		{
			return this.ContainsTileAsset(tileAsset);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetUsedTilesCount();

		public int GetUsedTilesNonAlloc(TileBase[] usedTiles)
		{
			return this.Internal_GetUsedTilesNonAlloc(usedTiles);
		}

		[FreeFunction(Name = "TilemapBindings::GetUsedTilesNonAlloc", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int Internal_GetUsedTilesNonAlloc(Object[] usedTiles);

		public Sprite GetSprite(Vector3Int position)
		{
			return this.GetSprite_Injected(ref position);
		}

		public Matrix4x4 GetTransformMatrix(Vector3Int position)
		{
			Matrix4x4 result;
			this.GetTransformMatrix_Injected(ref position, out result);
			return result;
		}

		public void SetTransformMatrix(Vector3Int position, Matrix4x4 transform)
		{
			this.SetTransformMatrix_Injected(ref position, ref transform);
		}

		[NativeMethod(Name = "GetTileColor")]
		public Color GetColor(Vector3Int position)
		{
			Color result;
			this.GetColor_Injected(ref position, out result);
			return result;
		}

		[NativeMethod(Name = "SetTileColor")]
		public void SetColor(Vector3Int position, Color color)
		{
			this.SetColor_Injected(ref position, ref color);
		}

		public TileFlags GetTileFlags(Vector3Int position)
		{
			return this.GetTileFlags_Injected(ref position);
		}

		public void SetTileFlags(Vector3Int position, TileFlags flags)
		{
			this.SetTileFlags_Injected(ref position, flags);
		}

		public void AddTileFlags(Vector3Int position, TileFlags flags)
		{
			this.AddTileFlags_Injected(ref position, flags);
		}

		public void RemoveTileFlags(Vector3Int position, TileFlags flags)
		{
			this.RemoveTileFlags_Injected(ref position, flags);
		}

		[NativeMethod(Name = "GetTileInstantiatedObject")]
		public GameObject GetInstantiatedObject(Vector3Int position)
		{
			return this.GetInstantiatedObject_Injected(ref position);
		}

		[NativeMethod(Name = "SetTileColliderType")]
		public void SetColliderType(Vector3Int position, Tile.ColliderType colliderType)
		{
			this.SetColliderType_Injected(ref position, colliderType);
		}

		[NativeMethod(Name = "GetTileColliderType")]
		public Tile.ColliderType GetColliderType(Vector3Int position)
		{
			return this.GetColliderType_Injected(ref position);
		}

		public void FloodFill(Vector3Int position, TileBase tile)
		{
			this.FloodFillTileAsset(position, tile);
		}

		[NativeMethod(Name = "FloodFill")]
		private void FloodFillTileAsset(Vector3Int position, Object tile)
		{
			this.FloodFillTileAsset_Injected(ref position, tile);
		}

		public void BoxFill(Vector3Int position, TileBase tile, int startX, int startY, int endX, int endY)
		{
			this.BoxFillTileAsset(position, tile, startX, startY, endX, endY);
		}

		[NativeMethod(Name = "BoxFill")]
		private void BoxFillTileAsset(Vector3Int position, Object tile, int startX, int startY, int endX, int endY)
		{
			this.BoxFillTileAsset_Injected(ref position, tile, startX, startY, endX, endY);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearAllTiles();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResizeBounds();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CompressBounds();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localBounds_Injected(out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_color_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_color_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_origin_Injected(out Vector3Int ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_origin_Injected(ref Vector3Int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector3Int ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector3Int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_tileAnchor_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_tileAnchor_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_orientationMatrix_Injected(out Matrix4x4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_orientationMatrix_Injected(ref Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Object GetTileAsset_Injected(ref Vector3Int position);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Object[] GetTileAssetsBlock_Injected(ref Vector3Int position, ref Vector3Int blockDimensions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTileAsset_Injected(ref Vector3Int position, Object tile);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_CALL_SetTileAssetsBlock_Injected(ref Vector3Int position, ref Vector3Int blockDimensions, Object[] tileArray);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RefreshTile_Injected(ref Vector3Int position);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Sprite GetSprite_Injected(ref Vector3Int position);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetTransformMatrix_Injected(ref Vector3Int position, out Matrix4x4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTransformMatrix_Injected(ref Vector3Int position, ref Matrix4x4 transform);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetColor_Injected(ref Vector3Int position, out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetColor_Injected(ref Vector3Int position, ref Color color);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern TileFlags GetTileFlags_Injected(ref Vector3Int position);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTileFlags_Injected(ref Vector3Int position, TileFlags flags);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddTileFlags_Injected(ref Vector3Int position, TileFlags flags);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveTileFlags_Injected(ref Vector3Int position, TileFlags flags);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern GameObject GetInstantiatedObject_Injected(ref Vector3Int position);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetColliderType_Injected(ref Vector3Int position, Tile.ColliderType colliderType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Tile.ColliderType GetColliderType_Injected(ref Vector3Int position);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void FloodFillTileAsset_Injected(ref Vector3Int position, Object tile);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void BoxFillTileAsset_Injected(ref Vector3Int position, Object tile, int startX, int startY, int endX, int endY);

		public enum Orientation
		{
			XY,
			XZ,
			YX,
			YZ,
			ZX,
			ZY,
			Custom
		}
	}
}
