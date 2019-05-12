using System;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	[RequiredByNativeCode]
	public class ITilemap
	{
		internal static ITilemap s_Instance;

		internal Tilemap m_Tilemap;

		internal ITilemap()
		{
		}

		internal void SetTilemapInstance(Tilemap tilemap)
		{
			this.m_Tilemap = tilemap;
		}

		public Vector3Int origin
		{
			get
			{
				return this.m_Tilemap.origin;
			}
		}

		public Vector3Int size
		{
			get
			{
				return this.m_Tilemap.size;
			}
		}

		public Bounds localBounds
		{
			get
			{
				return this.m_Tilemap.localBounds;
			}
		}

		public BoundsInt cellBounds
		{
			get
			{
				return this.m_Tilemap.cellBounds;
			}
		}

		public virtual Sprite GetSprite(Vector3Int position)
		{
			return this.m_Tilemap.GetSprite(position);
		}

		public virtual Color GetColor(Vector3Int position)
		{
			return this.m_Tilemap.GetColor(position);
		}

		public virtual Matrix4x4 GetTransformMatrix(Vector3Int position)
		{
			return this.m_Tilemap.GetTransformMatrix(position);
		}

		public virtual TileFlags GetTileFlags(Vector3Int position)
		{
			return this.m_Tilemap.GetTileFlags(position);
		}

		public virtual TileBase GetTile(Vector3Int position)
		{
			return this.m_Tilemap.GetTile(position);
		}

		public virtual T GetTile<T>(Vector3Int position) where T : TileBase
		{
			return this.m_Tilemap.GetTile<T>(position);
		}

		public void RefreshTile(Vector3Int position)
		{
			this.m_Tilemap.RefreshTile(position);
		}

		public T GetComponent<T>()
		{
			return this.m_Tilemap.GetComponent<T>();
		}

		[RequiredByNativeCode]
		private static ITilemap CreateInstance()
		{
			ITilemap.s_Instance = new ITilemap();
			return ITilemap.s_Instance;
		}
	}
}
