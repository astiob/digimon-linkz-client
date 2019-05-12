using System;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	[RequiredByNativeCode]
	public abstract class TileBase : ScriptableObject
	{
		[RequiredByNativeCode]
		public virtual void RefreshTile(Vector3Int position, ITilemap tilemap)
		{
			tilemap.RefreshTile(position);
		}

		[RequiredByNativeCode]
		public virtual void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
		}

		private TileData GetTileDataNoRef(Vector3Int position, ITilemap tilemap)
		{
			TileData result = default(TileData);
			this.GetTileData(position, tilemap, ref result);
			return result;
		}

		[RequiredByNativeCode]
		public virtual bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			return false;
		}

		private TileAnimationData GetTileAnimationDataNoRef(Vector3Int position, ITilemap tilemap)
		{
			TileAnimationData result = default(TileAnimationData);
			this.GetTileAnimationData(position, tilemap, ref result);
			return result;
		}

		[RequiredByNativeCode]
		public virtual bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
		{
			return false;
		}
	}
}
